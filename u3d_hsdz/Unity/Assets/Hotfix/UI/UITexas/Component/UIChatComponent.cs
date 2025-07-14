using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;

//牌局内聊天
namespace ETHotfix
{
    public class IMTextInfo
    {
        public int type;//0-Emoji 1-魔法表情
        public string sender;//发送者
        public string receiver;//接受者，魔法表情用
        public string content;//表情代号等
    }

    [ObjectSystem]
    public class UIChatComponentAwakeSystem : AwakeSystem<UIChatComponent>
    {
        public override void Awake(UIChatComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UIChatComponentUpdateSystem : UpdateSystem<UIChatComponent>
    {
        public override void Update(UIChatComponent self)
        {
            self.Update();
        }
    }

    public class UIChatComponent : Component
    {
        private long MaxTime = 20;// 秒
        enum VoiceState
        {
            IDLE = 0,
            PLAYING = 1,
            RECORD = 2
        }

        public bool isAutoPlayVoice;//是否允许自动播放音效
        private VoiceState voiceState;
        private Queue<IMMes> voicelist;//语音信息缓存列表
        private float recordTime;
        private bool isBackground;
        private IMMes lastSoundMes;

        public void Awake()
        {
            isAutoPlayVoice = true;
            voiceState = VoiceState.IDLE;
            voicelist = new Queue<IMMes>();
            //侦听后台返回的IM信息
            ETModel.Game.Hotfix.OnGroupMes += OnChatMes;
            ETModel.Game.Hotfix.OnApplicationPauseTrue += OnApplicationFocusOut;
            ETModel.Game.Hotfix.OnApplicationPauseFalse += OnApplicationFocusIn;
        }

        public void Update()
        {

            if (voiceState == VoiceState.PLAYING)
            {
                if (!IsPlayingVoice())
                {
                    StopLastVoice();//用于移除上一次播放内容//
                    if (voicelist.Count > 0)
                    {
                        //队列存在语音时播放
                        IMMes mes = voicelist.Dequeue();
                        PlayVoice(mes);
                    }
                    else
                    {
                        voiceState = VoiceState.IDLE;
                        SoundComponent.Instance.audioManager.IsMusicOn = true;
                    }
                    
                }
            }
            else if (voiceState == VoiceState.RECORD)
            {

                recordTime += Time.deltaTime;
                if (recordTime > this.MaxTime)
                {
                    StopRecord(true);
                }
            }

        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            Log.Debug("--UIChatComponent dispose--");
            base.Dispose();
            StopVoice();
            ETModel.Game.Hotfix.OnGroupMes -= OnChatMes;
            // ETModel.Game.Hotfix.OnGroupMes = null;//释放时停止侦听
            ETModel.Game.Hotfix.OnApplicationPauseTrue -= OnApplicationFocusOut;
            ETModel.Game.Hotfix.OnApplicationPauseFalse -= OnApplicationFocusIn;
        }

        /// <summary>
        /// IM信息事件
        /// </summary>
        /// <param name="value"></param>
        void OnChatMes(string value)
        {
            Log.Debug($"im新消息 : {value}");
            if (isBackground) return; //在后台时不处理信息
            try
            {
                IMMes mes = JsonHelper.FromJson<IMMes>(value);
                if (mes.mesType == 0)
                {
                    //文字信息
                    IMTextInfo info = JsonHelper.FromJson<IMTextInfo>(mes.mesContent);
                    if (info.type == 0)
                    {
                        //Emoji
                        GameCache.Instance.CurGame.onReceiveEmoji(info.content, info.sender);
                    }
                    else if (info.type == 1)
                    {
                        //魔法表情
                        GameCache.Instance.CurGame.onReceiveAnimoji(info.content, info.sender, info.receiver);
                    }
                }
                else if (mes.mesType == 1)
                {
                    //语音信息
                    if (!isAutoPlayVoice) return;
                    if (voiceState == VoiceState.IDLE)
                    {

                        PlayVoice(mes);
                    }
                    else
                    {
                        //添加入列表
                        voicelist.Enqueue(mes);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error($"错误im信息 e = {e}");
            }
        }

        public void SendEmojiMsg(string name)
        {
            IMTextInfo info = new IMTextInfo()
            {
                sender = GameCache.Instance.nUserId.ToString(),
                receiver = GameCache.Instance.nUserId.ToString(),
                type = 0,
                content = name
            };
            string infoJsonStr = JsonHelper.ToJson(info);
            SendMes(infoJsonStr);
        }

        public void SendAnimojiMsg(string name, string receiver)
        {
            IMTextInfo info = new IMTextInfo()
            {
                sender = GameCache.Instance.nUserId.ToString(),
                receiver = receiver,
                type = 1,
                content = name
            };
            string infoJsonStr = JsonHelper.ToJson(info);
            SendMes(infoJsonStr);

            //GameCache.Instance.CurGame.onReceiveAnimoji(name, GameCache.Instance.nUserId.ToString(), receiver);

            //扣费
            WEB2_expression_use.RequestData requestData = new WEB2_expression_use.RequestData()
            {
                emojiKey = name
            };
            HttpRequestComponent.Instance.Send(WEB2_expression_use.API,
                WEB2_expression_use.Request(requestData), json =>
                {
                    var tDto = WEB2_expression_use.Response(json);
                    if (tDto.status == 0)
                    {
                        GameCache.Instance.gold = tDto.data.chip;
                    }
                });
        }

        /// <summary>
        /// 发送文字信息
        /// </summary>
        /// <param name="mes"></param>
        public void SendMes(string mes)
        {

            IMSdkComponent.Instance.SendMessage(mes);
        }

        /// <summary>
        /// 播放录音音效
        /// </summary>
        public void PlayVoice(IMMes mes)
        {
            //判断是否开启了观众语音
            if (GameCache.Instance.CurGame.PlayRecordByUserId(mes.sender, mes.duration) || GameCache.Instance.CurGame.bSpectatorsVoice == 1)
            {
                lastSoundMes = mes;
                Log.Debug($"PlayVoice sender = {mes.sender} , duration = {mes.duration}");
                voiceState = VoiceState.PLAYING;
                IMSdkComponent.Instance.PlayVoice(mes.mesContent);
                SoundComponent.Instance.audioManager.IsMusicOn = false;
            }
        }

        /// <summary>
        /// 自动播放下一段录音
        /// </summary>
        public void PlayVoiceNext() {

            voiceState = VoiceState.PLAYING;
        }

        /// <summary>
        /// 停止播放录音音效
        /// </summary>
        public void StopVoice()
        {

            voiceState = VoiceState.IDLE;
            IMSdkComponent.Instance.StopVoice();
            SoundComponent.Instance.audioManager.IsMusicOn = true;
            StopLastVoice();
        }


        public void StopLastVoice() {

            GameCache.Instance.CurGame.StopAllRecord();
        }

        /// <summary>
        /// 停止播放录音音效
        /// </summary>
        public bool IsPlayingVoice()
        {
            return IMSdkComponent.Instance.IsPlayingVoice();
        }

        /// <summary>
        /// 返回最近一次播放录音来源的发送者
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsPlayingUserVoice(string userId)
        {
            if (null == lastSoundMes)
                return false;
            return lastSoundMes.sender == userId ? true : false;
        }

        /// <summary>
        /// 开始录音
        /// </summary>
        public bool StartRecord()
        {
            if (IsPlayingVoice())
            {
                StopVoice();
            }

            bool isStart = IMSdkComponent.Instance.StartRecord();
            if (isStart)
            {

                Log.Debug("开始录音");
                // UIComponent.Instance.Toast("开始录音");
                SoundComponent.Instance.audioManager.IsMusicOn = false;
                voiceState = VoiceState.RECORD;
                recordTime = 0;
            }
            else
            {
                Log.Debug("录音失败");
                if (isAutoPlayVoice)
                {
                    voiceState = VoiceState.PLAYING;
                }
                else voiceState = VoiceState.IDLE;
            }
            return isStart;
        }

        /// <summary>
        /// 停止录音
        /// </summary>
        public long StopRecord(bool isSend)
        {
            if (voiceState != VoiceState.RECORD) return 0;
            long mRecordTime = IMSdkComponent.Instance.StopRecord(isSend);
            voiceState = VoiceState.IDLE;
            SoundComponent.Instance.audioManager.IsMusicOn = true;
            // UIComponent.Instance.Toast("停止录音");
            if (mRecordTime > 0)
            {
                if (isAutoPlayVoice)
                {
                    voiceState = VoiceState.PLAYING;
                }
            }
            else
            {
                Log.Debug("录音失败2");
            }
            return mRecordTime;
        }

        public void PauseRecord()
        {

            IMSdkComponent.Instance.PauseRecord();
        }

        public void ContinueRecord()
        {

            IMSdkComponent.Instance.ContinueRecord();
        }

        /// <summary>
        /// 从后台切入应用时调用
        /// </summary>
        void OnApplicationFocusIn() {

            Log.Debug("OnApplicationFocusIn");
            DelayBackGroundSetOn();
        }

        async void DelayBackGroundSetOn() {

            ETModel.TimerComponent timer = ETModel.Game.Scene.GetComponent<TimerComponent>();
            await timer.WaitAsync(100);
            isBackground = false;
        }

        /// <summary>
        /// 应用切出
        /// </summary>
        void OnApplicationFocusOut() {

            Log.Debug("OnApplicationFocusOut");
            if (IsPlayingVoice())
            {
                StopVoice();//退出时停止该段语音
            }
            //清除所有语音
            voicelist.Clear();
            isBackground = true;
        }
    }
}
