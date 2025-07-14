using ETModel;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_UserInfoSettingComponentSystem : AwakeSystem<UIMine_UserInfoSettingComponent>
    {
        public override void Awake(UIMine_UserInfoSettingComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_UserInfoSettingComponent : UIBaseComponent
    {
        UserGender selectGender = (UserGender)GameCache.Instance.sex;

        ReferenceCollector rc;
        Button btn_head;
        Button btn_nick;
        Button btn_gender;
        RawImage img_head;
        Text text_nickName;
        Text text_gender;
        Image img_gender;
        private byte[] headPicData;
        private GameObject ModifyGo;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(LanguageManager.Get("UIMine_UserInfoSetting_title"), UIType.UIMine_UserInfoSetting, SaveAction);

            WebImageHelper.SetHeadImage(img_head, GameCache.Instance.headPic);

            headPicData = null;
            text_nickName.text = GameCache.Instance.nick;
            text_gender.text = selectGender == UserGender.Male ? LanguageManager.Get("UIMine_UserInfoSetting_Male") : LanguageManager.Get("UIMine_UserInfoSetting_Female");
            img_gender.sprite = selectGender == UserGender.Male ? rc.Get<Sprite>("my_boy_icon") : rc.Get<Sprite>("my_girl_icon");

            if (UIMineModel.mInstance.modifyHeadTime <= 0 || GameCache.Instance.modifyNickNum <= 0)
            {
                ModifyGo.SetActive(true);
            }
        }

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            Game.EventSystem.Run(EventIdType.Mission_Refresh);
            base.Dispose();
        }

        public void OnNativeGetHeadImage(string imagePath)
        {
            ETModel.Game.Hotfix.OnGetHeadImagePath = null;
            // Debug.Log("ongetHead3");
            FileStream fs = new FileStream(imagePath, FileMode.Open);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            Texture2D originalTex = new Texture2D(320, 320);
            var iSLoad = originalTex.LoadImage(buffer);
            originalTex.Apply();
            img_head.texture = originalTex;

            headPicData = buffer;
        }

        #region UI
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            btn_head = rc.Get<GameObject>("btn_head").GetComponent<Button>();
            btn_nick = rc.Get<GameObject>("btn_nick").GetComponent<Button>();
            btn_gender = rc.Get<GameObject>("btn_gender").GetComponent<Button>();
            img_head = rc.Get<GameObject>("img_head").GetComponent<RawImage>();
            text_nickName = rc.Get<GameObject>("text_nickName").GetComponent<Text>();
            text_gender = rc.Get<GameObject>("text_gender").GetComponent<Text>();
            img_gender = rc.Get<GameObject>("img_gender").GetComponent<Image>();
            ModifyGo = rc.Get<GameObject>("ModifyGo");
            ModifyGo.SetActive(false);

            UIEventListener.Get(btn_head.gameObject).onClick = ClickHeadBtn;
            UIEventListener.Get(btn_nick.gameObject).onClick = ClickNickBtn;
            UIEventListener.Get(btn_gender.gameObject).onClick = ClickGenderBtn;
        }

        #endregion

        #region Action
        private async void ClickHeadBtn(GameObject go)
        {
            await Game.Scene.GetComponent<UIComponent>().ShowAsync(UIType.UIActionSheet, new UIActionSheetComponent.ActionSheetData()
            {
                titles = new string[] { LanguageManager.Get("UIMine_UserInfoSetting_Camera"), LanguageManager.Get("UIMine_UserInfoSetting_album") },
                actionDelegate = (index) =>
                {
                    if (index == 0)
                    {
                        NativeManager.TakeHeadImage("1");
                        ETModel.Game.Hotfix.OnGetHeadImagePath = OnNativeGetHeadImage;
                    }
                    if (index == 1)
                    {
                        NativeManager.TakeHeadImage("2");
                        ETModel.Game.Hotfix.OnGetHeadImagePath = OnNativeGetHeadImage;
                    }
                }
            }, () => { });
        }

        private async void ClickNickBtn(GameObject go)
        {
            await Game.Scene.GetComponent<UIComponent>().ShowAsync(UIType.UIMine_UserInfoSettingNick, new UIMine_UserInfoSettingNickComponent.SettingNickData()
            {
                nick = text_nickName.text,
                setNickDelegate = (nick) =>
                {
                    text_nickName.text = nick;
                }
            }, () => { });
        }

        private async void ClickGenderBtn(GameObject go)
        {
            await Game.Scene.GetComponent<UIComponent>().ShowAsync(UIType.UIActionSheet, new UIActionSheetComponent.ActionSheetData()
            {
                titles = new string[] { LanguageManager.Get("UIMine_UserInfoSetting_Male"), LanguageManager.Get("UIMine_UserInfoSetting_Female") },
                actionDelegate = (index) =>
                {
                    // Debug.Log($"{index}");
                    selectGender = (UserGender)index;
                    text_gender.text = selectGender == UserGender.Male ? LanguageManager.Get("UIMine_UserInfoSetting_Male") : LanguageManager.Get("UIMine_UserInfoSetting_Female");
                    img_gender.sprite = selectGender == UserGender.Male ? rc.Get<Sprite>("my_boy_icon") : rc.Get<Sprite>("my_girl_icon");
                }
            }, () => { });
        }

        private void SaveAction()
        {
            if (headPicData != null)
            {
                //有修改头像先上传头像
                uploadHeadPic(headPicData);
            }
            else
            {
                SaveUserInfo();
            }
        }


        private void uploadHeadPic(byte[] data)
        {
            HttpRequestComponent.Instance.SendPicUpload(
                WEB_upload_head.API,
                data,
                this.OpenUploadHead);
        }

        private void OpenUploadHead(string resData)
        {
            Log.Debug(resData);
            WEB_upload_head.ResponseData responseData = WEB_upload_head.Response(resData);
            if (responseData.status == 0)
            {
                GameCache.Instance.headPic = responseData.data.pic_name;
                headPicData = null;
            }
            SaveUserInfo();
        }

        private void SaveUserInfo()
        {
            WEB2_user_mod_user_info.RequestData requestData = new WEB2_user_mod_user_info.RequestData()
            {
                nickName = text_nickName.text,
                head = GameCache.Instance.headPic,
                sex = (int)selectGender,
                sign = ""

            };
            HttpRequestComponent.Instance.Send(
                WEB2_user_mod_user_info.API,
                WEB2_user_mod_user_info.Request(requestData),
                this.OpenUserInfoUpdate);
        }

        private async void OpenUserInfoUpdate(string resData)
        {
            Log.Debug(resData);
            WEB2_user_mod_user_info.ResponseData responseData = WEB2_user_mod_user_info.Response(resData);
            if (responseData.status == 0)
            {
                if (responseData.data != null && responseData.data.presentation > 0)
                {
                    UIComponent.Instance.ToastFormat1("UIMine_Setting110", responseData.data.presentation.ToString());//Toast("恭喜您，完成首次修改头像和昵称，获得" + responseData.data.presentation.ToString() + "USDT");
                    await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(1000);
                }

                GameCache.Instance.nick = text_nickName.text;
                GameCache.Instance.sex = (sbyte)selectGender;
                Game.Scene.GetComponent<UIComponent>().RemoveAnimated(UIType.UIMine_UserInfoSetting);

                UI uI = UIComponent.Instance.Get(UIType.UIMine);
                if (null != uI)
                {
                    UIMineComponent mineComponent = uI.UiBaseComponent as UIMineComponent;
                    mineComponent.ObtainUserInfo();
                }
            }
            else
            {
                string errorDes = CPErrorCode.ErrorDescription(responseData.status);
                UIComponent.Instance.Toast(errorDes);
            }
        }

        #endregion
    }
}
