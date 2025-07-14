using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;


namespace ETHotfix
{
    [ObjectSystem]
    public class UICustomerServerComponentSystem : AwakeSystem<UICustomerServerComponent>
    {
        public override void Awake(UICustomerServerComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UICustomerServerComponentUpdateSystem : UpdateSystem<UICustomerServerComponent>
    {
        public override void Update(UICustomerServerComponent self)
        {
            self.Update();
        }
    }

    /// <summary> 页面名: </summary>
    public class UICustomerServerComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Text textWX;
        private Text textTe;
        private Image imageIcon2;
        private Image imageIcon1;
        private Dictionary<string, Sprite> mDicContactStr;

        public void Awake()
        {
            InitUI();
        }

        public void Update()
        {

        }

        public override void OnShow(object obj)
        {
            if (obj == null)
            {
                UIMineModel.mInstance.APIGetClubContactList(pDto =>
                {
                    SetData(pDto);
                });
            }
            else if (obj != null && obj is List<WEB2_club_view.RecordArrElement>)
            {
                var pDto = obj as List<WEB2_club_view.RecordArrElement>;
                SetData(pDto);
            }
        }

        async void SetData(List<WEB2_club_view.RecordArrElement> pDto)
        {
            if (pDto != null)
            {
                for (int i = 0; i < pDto.Count; i++)
                {
                    if (i == 0)
                    {
                        textWX.text = pDto[i].content;
                        imageIcon1.sprite = mDicContactStr["icon_" + pDto[i].type.ToString()];
                    }
                    if (i == 1)
                    {
                        textTe.text = pDto[i].content;
                        imageIcon2.sprite = mDicContactStr["icon_" + pDto[i].type.ToString()];
                    }
                }
                imageIcon1.color = new Color32(255, 255, 255, 255);
                imageIcon2.color = new Color32(255, 255, 255, 255);

                if (pDto.Count == 0)
                {
                    UIComponent.Instance.ToastLanguage("UIWallet_ShowNoClub005");
                    await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(2000);
                    UIComponent.Instance.Remove(UIType.UICustomerServer);
                }
            }
            else
            {
                UIComponent.Instance.ToastLanguage("UIWallet_ShowNoClub005");
                await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(2000);
                UIComponent.Instance.Remove(UIType.UICustomerServer);
            }
        }        

        public override void OnHide()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            textWX = rc.Get<GameObject>("Text_WX").GetComponent<Text>();
            textTe = rc.Get<GameObject>("Text_Te").GetComponent<Text>();
            UIEventListener.Get(rc.Get<GameObject>("CloseBtn")).onClick = ClickCloseBtn;
            UIEventListener.Get(rc.Get<GameObject>("CopyWX")).onClick = ClickCopyWX;
            UIEventListener.Get(rc.Get<GameObject>("CopyTe")).onClick = ClickCopyTe;
            imageIcon2 = rc.Get<GameObject>("Image_icon2").GetComponent<Image>();
            imageIcon1 = rc.Get<GameObject>("Image_icon1").GetComponent<Image>();
            imageIcon1.color = new Color32(0, 0, 0, 0);
            imageIcon2.color = new Color32(0, 0, 0, 0);


            mDicContactStr = new Dictionary<string, Sprite>() {
                { "icon_1",rc.Get<Sprite>("iconWX")},
                { "icon_2",rc.Get<Sprite>("iconTe")},
                { "icon_3",rc.Get<Sprite>("iconSkype")},
                { "icon_4",rc.Get<Sprite>("iconSugram")},
            };

            textWX.text = "";
            textTe.text = "";


        }

        private void ClickCopyTe(GameObject go)
        {
            UniClipboard.SetText(textTe.text);
            UIComponent.Instance.ToastLanguage("Become106");
        }

        private void ClickCopyWX(GameObject go)
        {
            UniClipboard.SetText(textWX.text);
            UIComponent.Instance.ToastLanguage("Become106");
        }

        private void ClickCloseBtn(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UICustomerServer);
        }
    }
}

