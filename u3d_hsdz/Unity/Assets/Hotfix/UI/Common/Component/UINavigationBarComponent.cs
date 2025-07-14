using System;
using System.Collections.Generic;
using System.Net;
using BestHTTP;
using ETModel;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Util;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static UIEventListener;

namespace ETHotfix
{
    [ObjectSystem]
    public class UINavigationBarComponentAwakeSystem : AwakeSystem<UINavigationBarComponent>
    {
        public override void Awake(UINavigationBarComponent self)
        {
            self.Awake();
        }
    }

    public class UINavigationBarComponent : UIBaseComponent
    {
        public sealed class NavigationBarInfo
        {
            public string Title;
            public VoidDelegate OnClickBack;
            public VoidDelegate OnClickMenu;
        }

        private NavigationBarInfo navInfo;

        private ReferenceCollector rc;
        private GameObject backBtn;
        private GameObject menuBtn;
        private Text titleText;
        private Text rightText;

        public static void SetNavInfo(NavigationBarInfo barInfo)
        {
            Game.Scene.GetComponent<UIComponent>().Show(UIType.UINavigationBar, barInfo, null, 0);
        }

        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            backBtn = rc.Get<GameObject>("Button_back");
            menuBtn = rc.Get<GameObject>("Button_menu");
            titleText = rc.Get<GameObject>("Text_title").GetComponent<Text>();

            rightText = menuBtn.GetComponentInChildren<Text>();

            //适配刘海屏
            LayoutElement layout = this.GetParent<UI>().GameObject.GetComponent<LayoutElement>();
            NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            float realTop = safeArea.top * 1242 / safeArea.width;
            layout.preferredHeight = 146 + realTop;
            layout.minHeight = 146 + realTop;

            //统一配置样式
            if (backBtn.gameObject.activeInHierarchy)
            {
                //返回按钮
                GameObject obj = null;
                if (null != backBtn.transform.Find("Item_IconBg"))
                {
                    obj = backBtn.transform.Find("Item_IconBg").gameObject;
                }
                if (null == obj)
                {
                    if (null != backBtn.transform.Find($"{backBtn.name}(Clone)"))
                    {
                        obj = backBtn.transform.Find($"{backBtn.name}(Clone)").gameObject;
                    }
                }
                if (null == obj)
                {
                    obj = GameObject.Instantiate(backBtn);
                    obj.transform.SetParent(backBtn.transform);
                }

                backBtn.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                RectTransform backBtnTran = backBtn.GetComponent<RectTransform>();
                backBtnTran.localScale = Vector3.one;
                backBtnTran.anchorMin = new Vector2(0, 0);
                backBtnTran.anchorMax = new Vector2(0, 0);
                backBtnTran.pivot = new Vector2(0, 0);
                backBtnTran.anchoredPosition3D = new Vector3(0, 0, 0);
                backBtnTran.sizeDelta = new Vector2(370, 146);

                obj.GetComponent<Image>().raycastTarget = false;
                GameObject.Destroy(obj.GetComponent<Button>());
                RectTransform objTran = obj.GetComponent<RectTransform>();
                objTran.localScale = Vector3.one;
                objTran.anchorMin = new Vector2(0, 0.5f);
                objTran.anchorMax = new Vector2(0, 0.5f);
                objTran.pivot = new Vector2(0, 0.5f);
                objTran.sizeDelta = new Vector2(80, 80);
                objTran.anchoredPosition3D = new Vector3(52, 0, 0);
            }

            if (rc.gameObject.GetComponent<Image>().color.a >= 0.99f)
            {
                rc.gameObject.GetComponent<Image>().color = new Color32(26, 26, 28, 255);
            }

            var gradient = titleText.GetComponent<UnityEngine.UI.Gradient>();
            if (null != gradient)
            {
                gradient.GradientType = GradientType.Horizontal;
                gradient.EffectGradient = new UnityEngine.Gradient()
                { colorKeys = new GradientColorKey[] { new GradientColorKey(new Color32(213, 177, 111, 255), 0), new GradientColorKey(new Color32(164, 138, 91, 255), 1) } };
            }
            if (menuBtn.gameObject.activeInHierarchy == true && rightText != null)//右边按钮
            {
                var gradientRightText = titleText.GetComponent<UnityEngine.UI.Gradient>();
                if (gradientRightText != null)
                {
                    rightText.color = new Color32(255, 255, 255, 255);
                    gradientRightText.GradientType = GradientType.Horizontal;
                    gradientRightText.EffectGradient = new UnityEngine.Gradient()
                    { colorKeys = new GradientColorKey[] { new GradientColorKey(new Color32(213, 177, 111, 255), 0), new GradientColorKey(new Color32(164, 138, 91, 255), 1) } };
                }
            }

            if (menuBtn.gameObject.activeInHierarchy == true)
            {
                RectTransform menuBtnTran = menuBtn.GetComponent<RectTransform>();
                menuBtnTran.localScale = Vector3.one;
                menuBtnTran.anchorMin = new Vector2(1, 0);
                menuBtnTran.anchorMax = new Vector2(1, 0);
                menuBtnTran.pivot = new Vector2(1, 0);
                menuBtnTran.anchoredPosition3D = new Vector3(0, 0, 0);
                menuBtnTran.sizeDelta = new Vector2(140, 146);
            }

            titleText.fontSize = 56;
            titleText.color = new Color32(255, 255, 255, 255);
            RectTransform titleTran = titleText.gameObject.GetComponent<RectTransform>();
            titleTran.anchorMin = new Vector2(0.5f, 0);
            titleTran.anchorMax = new Vector2(0.5f, 0);
            titleTran.pivot = new Vector2(0.5f, 0);
            titleTran.anchoredPosition3D = new Vector3(0, 0, 0);
            titleTran.sizeDelta = new Vector2(500, 146);
            titleText.horizontalOverflow = HorizontalWrapMode.Overflow;
        }

        public override void OnShow(object obj)
        {
            if (null != obj)
            {
                navInfo = obj as NavigationBarInfo;
                UIEventListener.Get(backBtn.gameObject).onClick = navInfo.OnClickBack;
                UIEventListener.Get(menuBtn.gameObject).onClick = navInfo.OnClickMenu;
                if (navInfo.Title != null)
                {
                    titleText.text = navInfo.Title;
                }

            }
        }


        public override void OnHide()
        {
            // Log.Debug($"Segment OnHide");
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
