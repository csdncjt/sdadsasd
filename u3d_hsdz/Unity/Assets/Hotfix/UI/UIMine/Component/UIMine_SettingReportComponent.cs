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
    public class UIMine_SettingReportComponentSystem : AwakeSystem<UIMine_SettingReportComponent>
    {
        public override void Awake(UIMine_SettingReportComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_SettingReportComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Image imageBtn;


        private Text textreport;

        public void Awake()
        {
            InitUI();
        }



        public override void OnShow(object obj)
        {
            SetUpNav( UIType.UIMine_SettingReport);
        }

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }

        #region UI
        Dictionary<string, Sprite> mDicStrSprite;
        private InputField mode_inputField;

        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mDicStrSprite = new Dictionary<string, Sprite>() {
                { "ImageRed",rc.Get<Sprite>("ImageRed")},
                { "ImageGray",rc.Get<Sprite>("ImageGray")},
                            };

            mode_inputField = rc.Get<GameObject>("mode_inputField").GetComponent<InputField>();
            mode_inputField.onValueChanged.AddListener(OnChangeInput);
            UIEventListener.Get(rc.Get<GameObject>("Button_report")).onClick = ClickReport;
            imageBtn = rc.Get<GameObject>("Button_report").GetComponent<Image>();
            textreport = rc.Get<GameObject>("Text_report").GetComponent<Text>();
        }

        private void ClickReport(GameObject go)
        {
            if (mode_inputField.text.Length <= 0)
            {
                UIComponent.Instance.ToastLanguage("UIMine_Setting109");//Toast("请先输入内容");
                return;
            }

            UIMineModel.mInstance.APIReportOther(mode_inputField.text, delegate
            {
                UIComponent.Instance.ToastLanguage("UIMine_Setting108");//Toast("举报成功~");
                UIComponent.Instance.Remove(UIType.UIMine_SettingReport);
            });
        }

        private void OnChangeInput(string arg0)
        {
            if (mode_inputField.text.Length > 0)
            {
                imageBtn.sprite = mDicStrSprite["ImageRed"];
                textreport.color = new Color32(249, 243, 189, 255);
             //   imageBtn.SetNativeSize();
            }
            else
            {
                imageBtn.sprite = mDicStrSprite["ImageGray"];
                textreport.color = new Color32(175, 175, 175, 255);
             //   imageBtn.SetNativeSize();
            }
        }



        #endregion



    }
}

