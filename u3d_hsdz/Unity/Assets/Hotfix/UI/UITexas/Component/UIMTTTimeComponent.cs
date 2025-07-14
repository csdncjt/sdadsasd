using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMTTTimeComponentAwakeSystem : AwakeSystem<UIMTTTimeComponent>
    {
        public override void Awake(UIMTTTimeComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UIMTTTimeComponentUpdateSystem: UpdateSystem<UIMTTTimeComponent>
    {
        public override void Update(UIMTTTimeComponent self)
        {
            self.Update();
        }
    }


    public class UIMTTTimeComponent : UIBaseComponent
    {
        public sealed class MTTTimeData
        {
            public string nickname { get; set; }
            public int second { get; set; }
        }

        private ReferenceCollector rc;
        private Text textMTTTimeName;
        private Text textMTTTime;

        private bool timeCounting;
        private float timeDeltaTime;
        private int time;

        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            textMTTTimeName = rc.Get<GameObject>("Text_MTTTimeName").GetComponent<Text>();
            textMTTTime = rc.Get<GameObject>("Text_MTTTime").GetComponent<Text>();
        }

        public void Update()
        {
            if (this.timeCounting)
            {
                if (Time.time - this.timeDeltaTime > 1f)
                {
                    this.timeDeltaTime = Time.time;
                    this.time -= 1;
                    if (this.time <= 0)
                    {
                        this.timeCounting = false;
                        // 返回游戏
                        if (null != textMTTTime)
                            textMTTTime.text = "00:00";

                        UIComponent.Instance.HideNoAnimation(UIType.UIMTTTime);
                    }
                    else
                    {
                        // 返回游戏 10:20
                        if (null != textMTTTime)
                            textMTTTime.text = $"{(this.time / 60).ToString().PadLeft(2,'0')}:{(this.time % 60).ToString().PadLeft(2, '0')}";
                    }

                    if (this.time <= 30)
                    {
                        (GameCache.Instance.CurGame as MTTGame).countDownTo30Second();
                    }
                }
            }
        }

        public override void OnShow(object obj)
        {

            if (null != obj)
            {
                MTTTimeData data = obj as MTTTimeData;
                if (null != data)
                {
                    textMTTTimeName.text = data.nickname;
                    if (data.second > 0)
                    {
                        timeCounting = true;
                        time = data.second;
                    }
                    else
                    {
                        if (null != textMTTTime)
                            textMTTTime.text = "00:00";
                    }
                }
            }
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
   
            base.Dispose();
        }
    }
}
