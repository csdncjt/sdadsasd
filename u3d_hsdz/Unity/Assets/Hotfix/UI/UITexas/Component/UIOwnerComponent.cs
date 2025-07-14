using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UiOwnerComponentSystem : AwakeSystem<UIOwnerComponent>
    {
        public override void Awake(UIOwnerComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UIOwnerComponentUpdateSystem : UpdateSystem<UIOwnerComponent>
    {
        public override void Update(UIOwnerComponent self)
        {
            self.Update();
        }
    }

    public class UIOwnerComponent : UIBaseComponent
    {
        public sealed class OwnerData
        {
            public int roomPath { get; set; }
            public int roomId { get; set; }  // 房间号
            public int currentMinRate { get; set; } // 当前最小带入倍数
            public int currentMaxRate { get; set; } // 当前最大带入倍数
            public int muck_switch { get; set; }  // 0 关闭 1 开启
            public sbyte spectatorsVoice { get; set; }  // 0 关闭 1 开启
            public int straddle { get; set; }  // 0 关闭 1 开启
        }

        private OwnerData ownerData;
        private int minutes = 30;  // 加时多少分钟
        private bool checkUpdate = false;

        private ReferenceCollector rc;
        private Button buttonClose;
        private Button buttonPause;
        private Button buttonFinishGame;
        private Button buttonShare;
        private Button buttonSetting;
        private Toggle toggleMuck;
        private Toggle toggleStraddle;
        private Toggle toggleVoice;
        private Slider sliderDelay;
        private GameObject goMultipleSlider;
        DoubleScrollBarElement sliderMultiple;
        private Text textConsume;
        private Transform Vis_Background;


        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            buttonClose = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            buttonPause = rc.Get<GameObject>("Button_Pause").GetComponent<Button>();
            buttonFinishGame = rc.Get<GameObject>("Button_FinishGame").GetComponent<Button>();
            buttonShare = rc.Get<GameObject>("Button_Share").GetComponent<Button>();
            buttonSetting = rc.Get<GameObject>("Button_Setting").GetComponent<Button>();
            toggleMuck = rc.Get<GameObject>("Toggle_Muck").GetComponent<Toggle>();
            toggleStraddle = rc.Get<GameObject>("Toggle_Straddle").GetComponent<Toggle>();
            toggleVoice = rc.Get<GameObject>("Toggle_Voice").GetComponent<Toggle>();
            sliderDelay = rc.Get<GameObject>("Slider_Delay").GetComponent<Slider>();
            textConsume = rc.Get<GameObject>("Text_Consume").GetComponent<Text>();
            goMultipleSlider = rc.Get<GameObject>("Slider_multiple");


            sliderDelay.onValueChanged.AddListener(onValueChangedSliderDelay);

            UIEventListener.Get(buttonClose.gameObject).onClick = onClickClose;
            UIEventListener.Get(buttonPause.gameObject).onClick = onClickPause;
            UIEventListener.Get(buttonFinishGame.gameObject).onClick = onClickFinishGame;
            UIEventListener.Get(buttonShare.gameObject).onClick = onClickShare;
            UIEventListener.Get(buttonSetting.gameObject).onClick = onClickSetting;

            registerHandler();

            if (LanguageManager.mInstance.mCurLanguage == 1)
            {
                Vis_Background = rc.Get<GameObject>("Vis_Background").transform;
                Vis_Background.localPosition = new Vector3(Vis_Background.localPosition.x +90, Vis_Background.localPosition.y, 0);
            }
        }

        public void Update()
        {
            if (checkUpdate)
                UpdateSettingBtn();
        }

        private void UpdateSettingBtn()
        {
            bool hadChange = false;
            if ((ownerData.muck_switch == 0 && toggleMuck.isOn) || (ownerData.muck_switch != 0 && !toggleMuck.isOn))
            {
                hadChange = true;
            }

            if ((ownerData.straddle == 0 && toggleStraddle.isOn) || (ownerData.straddle != 0 && !toggleStraddle.isOn))
            {
                hadChange = true;
            }

            if ((ownerData.spectatorsVoice == 0 && toggleVoice.isOn) || (ownerData.spectatorsVoice != 0 && !toggleVoice.isOn))
            {
                hadChange = true;
            }

            if (ownerData.currentMinRate != (sliderMultiple.stepStart + 1) || ownerData.currentMaxRate != (sliderMultiple.stepEnd + 1))
            {
                hadChange = true;
            }

            if (sliderDelay.value > 0)
            {
                hadChange = true;
            }

            buttonSetting.interactable = hadChange;
            Image settingIcon = buttonSetting.gameObject.transform.Find("Image").gameObject.GetComponent<Image>();
            settingIcon.sprite = hadChange ? rc.Get<Sprite>("setting_button_setting") : rc.Get<Sprite>("setting_button_setting_unclick");
            Text settingTitle = buttonSetting.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
            settingTitle.color = hadChange ? new Color32(255, 200, 83, 255) : new Color32(175, 175, 175, 255);
        }

        private void onClickSetting(GameObject go)
        {
            if (!buttonSetting.interactable)
                return;

            CPGameSessionComponent mCpGameSessionComponent = Game.Scene.GetComponent<CPGameSessionComponent>();
            if (null == mCpGameSessionComponent)
                return;

            if ((ownerData.muck_switch == 0 && toggleMuck.isOn) || (ownerData.muck_switch != 0 && !toggleMuck.isOn))
            {
                mCpGameSessionComponent.HotfixSession.Send(new REQ_GAME_RECV_PRE_START_GAME_MUCK()
                {
                    roomID = GameCache.Instance.room_id,
                    roomPath = GameCache.Instance.room_path,
                    bMuck = toggleMuck.isOn ? 1 : 0
                });
            }

            if ((ownerData.straddle == 0 && toggleStraddle.isOn) || (ownerData.straddle != 0 && !toggleStraddle.isOn))
            {
                mCpGameSessionComponent.HotfixSession.Send(new REQ_GAME_RECV_PRE_START_GAME_STRADDLE()
                {
                    roomID = GameCache.Instance.room_id,
                    roomPath = GameCache.Instance.room_path,
                    bStraddle = toggleStraddle.isOn ? 1 : 0
                });
            }

            if ((ownerData.spectatorsVoice == 0 && toggleVoice.isOn) || (ownerData.spectatorsVoice != 0 && !toggleVoice.isOn))
            {
                mCpGameSessionComponent.HotfixSession.Send(new REQ_GAME_RECV_SPECTATORS_VOICE()
                {
                    roomID = GameCache.Instance.room_id,
                    roomPath = GameCache.Instance.room_path,
                    bSpectatorsVoice = toggleVoice.isOn ? 1 : 0
                });
            }

            if (ownerData.currentMinRate != (sliderMultiple.stepStart + 1) || ownerData.currentMaxRate != (sliderMultiple.stepEnd + 1))
            {
                mCpGameSessionComponent.HotfixSession.Send(new REQ_GAME_CONTROL_RANGE()
                {
                    roomId = GameCache.Instance.room_id,
                    roomPath = GameCache.Instance.room_path,
                    minTimes = sliderMultiple.stepStart + 1,
                    maxTimes = sliderMultiple.stepEnd + 1
                });
            }

            if (sliderDelay.value > 0)
            {
                mCpGameSessionComponent.HotfixSession.Send(new REQ_ADD_ROOM_TIME()
                {
                    roomID = GameCache.Instance.room_id,
                    roomPath = GameCache.Instance.room_path,
                    minutes = (int)sliderDelay.value * minutes
                });
            }

            Game.Scene.GetComponent<UIComponent>().Hide(UIType.UIOwner, null, 0);
        }

        private void onClickShare(GameObject go)
        {
            ETHotfix.UIComponent.Instance.Toast("功能尚未开放");
            UIComponent.Instance.HideNoAnimation(UIType.UIOwner);
        }

        private void onClickFinishGame(GameObject go)
        {
            Game.Scene.GetComponent<UIComponent>().Show(UIType.UIDialog, new UIDialogComponent.DialogData()
            {
                type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                // title = "温馨提示",
                title = CPErrorCode.LanguageDescription(10007),
                // content = $"是否解散房间?",
                content = CPErrorCode.LanguageDescription(20039),
                // contentCommit = "解散",
                contentCommit = CPErrorCode.LanguageDescription(10304),
                // contentCancel = "取消",
                contentCancel = CPErrorCode.LanguageDescription(10013),
                actionCommit = () =>
                {
                    UIComponent.Instance.HideNoAnimation(UIType.UIOwner);
                    Game.Scene.GetComponent<CPGameSessionComponent>().HotfixSession.Send(new REQ_FINISHGAME()
                    {
                        roomPath = GameCache.Instance.room_path,
                        roomID = GameCache.Instance.room_id
                    });
                },
                actionCancel = null
            }, null, 0);
        }

        private void onClickPause(GameObject go)
        {
            UIComponent.Instance.HideNoAnimation(UIType.UIOwner);
            Game.Scene.GetComponent<CPGameSessionComponent>().HotfixSession.Send(new REQ_GAME_PAUSE()
            {
                roomPath = GameCache.Instance.room_path,
                roomID = GameCache.Instance.room_id,
                bPause = 1
            });
        }

        private void onClickClose(GameObject go)
        {
            Game.Scene.GetComponent<UIComponent>().Hide(UIType.UIOwner, null, 0);
        }

        private void onValueChangedSliderDelay(float arg0)
        {
            // textConsume.text = $"花费: {arg0 * minutes}";
            textConsume.text = CPErrorCode.LanguageDescription(20040, new List<object>() { arg0 * minutes });
        }


        public override void OnShow(object obj)
        {
            if (null == obj)
                return;
            checkUpdate = true;

            ownerData = obj as OwnerData;

            if (ownerData.roomPath == (int)RoomPath.NormalAof || ownerData.roomPath == (int)RoomPath.OmahaAof)
            {
                goMultipleSlider.SetActive(false);
                sliderDelay.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -415);
            }
            else
            {
                goMultipleSlider.SetActive(true);
                sliderDelay.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -315);

            }

            toggleStraddle.isOn = ownerData.straddle == 1;
            toggleMuck.isOn = ownerData.muck_switch != 0;
            toggleVoice.isOn = ownerData.spectatorsVoice == 1;
            sliderDelay.value = 0;

            sliderMultiple = new DoubleScrollBarElement(
                    rc.Get<GameObject>("scrollbar_multiple0").GetComponent<Scrollbar>(),
                    rc.Get<GameObject>("scrollbar_multiple1").GetComponent<Scrollbar>(),
                    rc.Get<GameObject>("multiple_progress").GetComponent<Image>(),
                    rc.Get<GameObject>("multiple_container").transform,
                    new Color[2] { new Color(168f / 255f, 168f / 255f, 168f / 255f, 1), new Color(233f / 255f, 191f / 255f, 128f / 255f, 1) }
                );
            sliderMultiple.AddEvent();
            sliderMultiple.SetStartp(ownerData.currentMinRate - 1);
            sliderMultiple.SetEndp(ownerData.currentMaxRate - 1);
        }

        public override void OnHide()
        {
            checkUpdate = false;
            // Log.Debug($"Owner OnHide");
            sliderMultiple.RemoveEvent();
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            removeHandler();
            base.Dispose();
        }

        private void registerHandler()
        {

        }

        private void removeHandler()
        {

        }
    }
}
