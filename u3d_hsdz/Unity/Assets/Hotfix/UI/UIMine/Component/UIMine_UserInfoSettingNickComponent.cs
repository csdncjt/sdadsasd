using System;
using System.Collections.Generic;
using System.Net;
using BestHTTP;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using xcharts;
using DG.Tweening;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_UserInfoSettingNickComponentSystem : AwakeSystem<UIMine_UserInfoSettingNickComponent>
    {
        public override void Awake(UIMine_UserInfoSettingNickComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_UserInfoSettingNickComponent : UIBaseComponent
    {
        public sealed class SettingNickData
        {
            public string nick;
            public SettingNickDelegate setNickDelegate;
        }

        private SettingNickData settingNickData;
        public delegate void SettingNickDelegate(string nick);

        ReferenceCollector rc;
        InputField InputField_nick;
        Text text_cost_coin;
        Text text_total_coin;
        Button btn_confirm;

        public void Awake()
        {
            InitUI();
        }


        public override void OnShow(object obj)
        {
            SetUpNav(LanguageManager.Get("UIMine_UserInfoSettingNick_title"), UIType.UIMine_UserInfoSettingNick);
            if (null != obj)
            {
                settingNickData = obj as SettingNickData;
                InputField_nick.text = settingNickData.nick;
            }
            text_cost_coin.text = GameCache.Instance.modifyNickNum > 0 ? "3" : "0";
            text_total_coin.text = StringHelper.GetShortString(GameCache.Instance.gold);
        }

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }

        #region UI
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            InputField_nick = rc.Get<GameObject>("InputField_nick").GetComponent<InputField>();
            text_cost_coin = rc.Get<GameObject>("text_cost_coin").GetComponent<Text>();
            text_total_coin = rc.Get<GameObject>("text_total_coin").GetComponent<Text>();
            btn_confirm = rc.Get<GameObject>("btn_confirm").GetComponent<Button>();

            UIEventListener.Get(btn_confirm.gameObject).onClick = ClickConfirmBtn;
        }

        #endregion

        #region Action
        private void ClickConfirmBtn(GameObject go)
        {
            if (string.IsNullOrEmpty(InputField_nick.text))
            {
                UIComponent.Instance.ToastLanguage("UIMine_Setting112");//Toast("昵称不能为空");
                return;
            }
            if (InputField_nick.text.Length > 12)
            {
                UIComponent.Instance.Toast(LanguageManager.Get("UIMine_UserInfoNick_tooLong"));
                return;
            }
            if (StringHelper.IsContainSpecialCharacter(InputField_nick.text))
            {
                UIComponent.Instance.Toast(LanguageManager.Get("UIMine_UserInfoNick_Include_Special"));
                return;
            }
            if (SensitiveWordComponent.Instance.SensitiveWordJudge(LanguageManager.mInstance.GetLanguageForKey("NickName"), InputField_nick.text))
            {
                return;
            }
            CheckNickName();
        }

        private void CheckNickName()
        {
            WEB2_user_exist_nick.RequestData requestData = new WEB2_user_exist_nick.RequestData()
            {
                nickName = InputField_nick.text
            };
            HttpRequestComponent.Instance.Send(
                WEB2_user_exist_nick.API,
                WEB2_user_exist_nick.Request(requestData),
                this.CheckNickInfoUpdate);
        }

        private void CheckNickInfoUpdate(string resData)
        {
            WEB2_user_exist_nick.ResponseData responseData = WEB2_user_exist_nick.Response(resData);
            if (responseData.status == 0)
            {
                if (responseData.data == false)
                {
                    settingNickData.setNickDelegate(InputField_nick.text);
                    Game.Scene.GetComponent<UIComponent>().RemoveAnimated(UIType.UIMine_UserInfoSettingNick);
                }
                else
                {
                    UIComponent.Instance.ToastLanguage("UIMine_Setting111");//Toast("昵称重复了,请重新起一个吧");
                }
            }
            else
            {
                UIComponent.Instance.Toast(responseData.status);
            }
        }

        #endregion
    }
}
