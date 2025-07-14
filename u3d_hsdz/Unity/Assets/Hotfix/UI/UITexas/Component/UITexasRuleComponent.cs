using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UITexasRuleComponentSystem : AwakeSystem<UITexasRuleComponent>
    {
        public override void Awake(UITexasRuleComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UITexasRuleComponentUpdateSystem : UpdateSystem<UITexasRuleComponent>
    {
        public override void Update(UITexasRuleComponent self)
        {
            self.Update();
        }
    }

    public enum EnumRuleOpenLoad
    {
        None,
        ReportLoad = 2
    }

    /// <summary> 页面名: </summary>
    public class UITexasRuleComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        //private UniWebView mUniWebView;
        private RectTransform TranContent;
        private RectTransform ImageBg;
        private RectTransform ToggleGroup;
        private RectTransform cardType;
        private RectTransform ShortCardType;
        private RectTransform Operating_Texas;
        private RectTransform Operating_Omaha;
        private RectTransform Operating_AOF;
        private RectTransform Operating_B;
        private RectTransform Operating_Short;
        private RectTransform OmahaInsurance;
        private RectTransform TexasInsurance;
        private RectTransform ShortInsurance;

        public void Awake()
        {
            InitUI();
        }

        public void Update()
        {
        }

        public override void OnShow(object obj)
        {
            NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            float realTop = safeArea.top * 1242 / safeArea.width;
            TranContent.offsetMin = new Vector2(0f, 0f);
            TranContent.offsetMax = new Vector2(0f, -realTop);

            if (obj == null)
            {
                //if (GlobalData.Instance.PaiRuleURL != null && GlobalData.Instance.PaiRuleURL.Length >= 3)
                //mUniWebView.Load(GlobalData.Instance.PaiRuleURL[0]);
                cardType.gameObject.SetActive(false);
                ShortCardType.gameObject.SetActive(false);
                OmahaInsurance.gameObject.SetActive(false);
                TexasInsurance.gameObject.SetActive(false);
                ShortInsurance.gameObject.SetActive(false);
                if (GameCache.Instance.room_path == 61)
                {
                    Operating_Texas.gameObject.SetActive(true);
                }
                else if (GameCache.Instance.room_path == 91)
                {
                    Operating_Omaha.gameObject.SetActive(true);
                }
                else if (GameCache.Instance.room_path == 21)
                {
                    Operating_Short.gameObject.SetActive(true);
                }
                else if (GameCache.Instance.room_path == 63 || GameCache.Instance.room_path == 93 || GameCache.Instance.room_path == 23)
                {
                    Operating_AOF.gameObject.SetActive(true);
                }
                else if (GameCache.Instance.room_path == 62 || GameCache.Instance.room_path == 92)
                {
                    Operating_B.gameObject.SetActive(true);
                }
            }
            else
            {
                var tLoad = (EnumRuleOpenLoad)obj;
                for (int i = 0; i < mToggles.Count; i++)
                {
                    mToggles[i].isOn = false;
                }
                mToggles[2].isOn = true;
                ClickTopToggle(null, 2);
            }

        }

        public override void OnHide()
        {
        }
        public override void Dispose()
        {
            if (this.IsDisposed)
                return;
            base.Dispose();
        }
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            UIEventListener.Get(rc.Get<GameObject>("ImageMaskClose")).onClick = ClickMaskClose;
            UIEventListener.Get(rc.Get<GameObject>("Toggle_1"), 0).onIntClick = ClickTopToggle;
            UIEventListener.Get(rc.Get<GameObject>("Toggle_2"), 1).onIntClick = ClickTopToggle;
            UIEventListener.Get(rc.Get<GameObject>("Toggle_3"), 2).onIntClick = ClickTopToggle;
            ImageBg = rc.Get<GameObject>("ImageBg").GetComponent<RectTransform>();

            ToggleGroup = rc.Get<GameObject>("ToggleGroup").GetComponent<RectTransform>();
            cardType = rc.Get<GameObject>("CardType").GetComponent<RectTransform>();
            ShortCardType = rc.Get<GameObject>("ShortCardType").GetComponent<RectTransform>();
            Operating_Texas = rc.Get<GameObject>("Operating_Texas").GetComponent<RectTransform>();
            Operating_Omaha = rc.Get<GameObject>("Operating_Omaha").GetComponent<RectTransform>();
            Operating_AOF = rc.Get<GameObject>("Operating_AOF").GetComponent<RectTransform>();
            Operating_B = rc.Get<GameObject>("Operating_B").GetComponent<RectTransform>();
            OmahaInsurance = rc.Get<GameObject>("OmahaInsurance").GetComponent<RectTransform>();
            TexasInsurance = rc.Get<GameObject>("TexasInsurance").GetComponent<RectTransform>();
            TranContent = rc.Get<GameObject>("Content").GetComponent<RectTransform>();
            Operating_Short = rc.Get<GameObject>("Operating_Short").GetComponent<RectTransform>();
            ShortInsurance = rc.Get<GameObject>("ShortInsurance").GetComponent<RectTransform>();

            mToggles = new List<Toggle>();
            mToggles.Add(rc.Get<GameObject>("Toggle_1").GetComponent<Toggle>());
            mToggles.Add(rc.Get<GameObject>("Toggle_2").GetComponent<Toggle>());
            mToggles.Add(rc.Get<GameObject>("Toggle_3").GetComponent<Toggle>());
        }
        List<Toggle> mToggles;

        private void ClickTopToggle(GameObject go, int index)
        {
            if (index == 0)
            {
                //if (GlobalData.Instance.PaiRuleURL != null && GlobalData.Instance.PaiRuleURL.Length >= 3)
                //mUniWebView.Load(GlobalData.Instance.PaiRuleURL[index]);
                cardType.gameObject.SetActive(false);
                ShortCardType.gameObject.SetActive(false);
                OmahaInsurance.gameObject.SetActive(false);
                TexasInsurance.gameObject.SetActive(false);
                ShortInsurance.gameObject.SetActive(false);
                if (GameCache.Instance.room_path == 61)
                {
                    Operating_Texas.gameObject.SetActive(true);
                }
                else if (GameCache.Instance.room_path == 91)
                {
                    Operating_Omaha.gameObject.SetActive(true);
                }
                else if (GameCache.Instance.room_path == 63 || GameCache.Instance.room_path == 93 || GameCache.Instance.room_path == 23)
                {
                    Operating_AOF.gameObject.SetActive(true);
                }
                else if (GameCache.Instance.room_path == 62 || GameCache.Instance.room_path == 92)
                {
                    Operating_B.gameObject.SetActive(true);
                }
                else if (GameCache.Instance.room_path == 21)
                {
                    Operating_Short.gameObject.SetActive(true);
                }
            }
            else
            {
                if (index == 1)
                {
                    cardType.gameObject.SetActive(true);
                    ShortCardType.gameObject.SetActive(false);
                    OmahaInsurance.gameObject.SetActive(false);
                    TexasInsurance.gameObject.SetActive(false);
                    ShortInsurance.gameObject.SetActive(false);
                    if (GameCache.Instance.room_path == 61)
                    {
                        Operating_Texas.gameObject.SetActive(false);
                    }
                    else if (GameCache.Instance.room_path == 91)
                    {
                        Operating_Omaha.gameObject.SetActive(false);
                    }
                    else if (GameCache.Instance.room_path == 63 || GameCache.Instance.room_path == 93 || GameCache.Instance.room_path == 23)
                    {
                        Operating_AOF.gameObject.SetActive(false);
                    }
                    else if (GameCache.Instance.room_path == 62 || GameCache.Instance.room_path == 92)
                    {
                        Operating_B.gameObject.SetActive(false);
                    }
                    else if (GameCache.Instance.room_path == 21)
                    {
                        Operating_Short.gameObject.SetActive(false);
                        cardType.gameObject.SetActive(false);
                        ShortCardType.gameObject.SetActive(true);
                    }
                }
                else
                {
                    cardType.gameObject.SetActive(false);
                    ShortCardType.gameObject.SetActive(false);
                    if (GameCache.Instance.room_path == 61)
                    {
                        Operating_Texas.gameObject.SetActive(false);
                    }
                    else if (GameCache.Instance.room_path == 91)
                    {
                        Operating_Omaha.gameObject.SetActive(false);
                    }
                    else if (GameCache.Instance.room_path == 63 || GameCache.Instance.room_path == 93 || GameCache.Instance.room_path == 23)
                    {
                        Operating_AOF.gameObject.SetActive(false);
                    }
                    else if (GameCache.Instance.room_path == 62 || GameCache.Instance.room_path == 92)
                    {
                        Operating_B.gameObject.SetActive(false);
                    }
                    else if (GameCache.Instance.room_path == 21)
                    {
                        Operating_Short.gameObject.SetActive(false);
                    }

                    //91是奥马哈
                    if (GameCache.Instance.room_path == 91)
                    {
                        OmahaInsurance.gameObject.SetActive(true);
                    }
                    else if (GameCache.Instance.room_path == 21)
                    {
                       ShortInsurance.gameObject.SetActive(true);
                    }
                    else
                    {
                        TexasInsurance.gameObject.SetActive(true);
                    }

                }

            }
        }

        private void ClickMaskClose(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UITexasRule);
        }
    }
}


