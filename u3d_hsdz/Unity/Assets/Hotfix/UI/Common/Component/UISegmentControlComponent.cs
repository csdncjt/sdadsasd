using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ETHotfix
{
    [ObjectSystem]
    public class UISegmentControlComponentAwakeSystem : AwakeSystem<UISegmentControlComponent>
    {
        public override void Awake(UISegmentControlComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UISegmentControlComponentUpdateSystem : UpdateSystem<UISegmentControlComponent>
    {
        public override void Update(UISegmentControlComponent self)
        {
            self.Update();
        }
    }

    public class UISegmentControlComponent : UIBaseComponent
    {
        public sealed class SegmentData
        {
            public string[] Titles;
            public ClickDelegate OnClick;
            /// <summary> 正常字体  选中字体大小 暂不用 48字号</summary>
            public int[] N_S_Fonts;//
            public Color32[] N_S_Color;
            /// <summary> 选中有渐变  shadow</summary>
            public bool IsEffer;
        }

        public delegate void ClickDelegate(GameObject go, int index);
        private SegmentData segmentData;

        private ReferenceCollector rc;
        private List<GameObject> segmentBtns;
        private List<Text> mTexts;
        private GameObject line;
        private int selectedIndex;
        private bool hadSelected;

        public static bool TextUpdate;

        public static void SetUp(Transform transform, SegmentData segmentData)
        {
            Transform segmentTransform = transform.Find(UIType.UISegmentControl);
            if (null != segmentTransform)
            {
                GameObject segmentGo = segmentTransform.gameObject;
                UI segmentUi = ComponentFactory.Create<UI, GameObject>(segmentGo);
                segmentUi.AddUIBaseComponent<UISegmentControlComponent>();
                segmentUi.UiBaseComponent.OnShow(segmentData);
            }
        }

        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            line = rc.Get<GameObject>("line_img");
            this.segmentBtns = new List<GameObject>();
            this.mTexts = new List<Text>();
            this.selectedIndex = 0;
            this.hadSelected = false;
            TextUpdate = false;
        }

        public void Update()
        {
            if (!hadSelected)
            {
                if (selectedIndex >= 0 && this.segmentBtns.Count > 0)
                {
                    GameObject go = segmentBtns[this.selectedIndex];
                    if (go != null)
                        line.transform.localPosition = new Vector3(go.transform.localPosition.x, -(rc.GetComponent<RectTransform>().sizeDelta.y - 2), 0.0f);
                }
            }

            if (TextUpdate)
            {
                TextUpdate = false;
                onUpdateText();
            }
        }

        public override void OnShow(object obj)
        {
            if (null != obj)
            {
                SegmentData newSegmentData = obj as SegmentData;
                segmentData = newSegmentData;
                foreach (GameObject segmentBtn in segmentBtns)
                {
                    UnityEngine.Object.Destroy(segmentBtn);
                }
                segmentBtns.Clear();
                for (int i = 0; i < segmentData.Titles.Length; i++)
                {
                    GameObject segmentBtn = GetSegmentBtn(rc.transform);
                    var tText = segmentBtn.GetComponent<Button>().transform.Find("Text").GetComponent<Text>();
                    tText.text = segmentData.Titles[i];
                    UIEventListener.Get(segmentBtn, i).onIntClick = SegmentAction;
                    this.segmentBtns.Add(segmentBtn);
                    this.mTexts.Add(tText);
                    if (i == 0 && this.segmentData.N_S_Fonts != null && this.segmentData.N_S_Fonts.Length == 2)
                    {//首个字体要大点
                        tText.fontSize = this.segmentData.N_S_Fonts[1];
                    }
                    if (i == 0 && this.segmentData.N_S_Color != null && this.segmentData.N_S_Color.Length == 2)
                    {
                        tText.color = this.segmentData.N_S_Color[1];
                    }
                    if (i == 0 && this.segmentData.IsEffer == true)
                    {//要报错,自找prefab加上此渐变
                        tText.GetComponent<UnityEngine.UI.Gradient>().enabled = true;
                        tText.GetComponent<Shadow>().enabled = true;
                    }
                }

            }
        }

        public void onUpdateText()
        {
            string[] titles = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIDataList");
            for (int i = 0; i < titles.Length; i++)
            {
                if (this.segmentBtns[i] != null)
                {
                    var tText = this.segmentBtns[i].GetComponent<Button>().transform.Find("Text").GetComponent<Text>();
                    tText.text = titles[i];
                }
            }
        }

        private void SegmentAction(GameObject go, int index)
        {
            this.selectedIndex = index;
            this.hadSelected = true;
            segmentData.OnClick(go, index);
            if (this.segmentData.N_S_Fonts != null && this.segmentData.N_S_Fonts.Length == 2)
            {
                for (int i = 0; i < this.mTexts.Count; i++)
                {
                    this.mTexts[i].fontSize = this.segmentData.N_S_Fonts[0];
                }
                this.mTexts[index].fontSize = this.segmentData.N_S_Fonts[1];
            }
            if (this.segmentData.N_S_Color != null && this.segmentData.N_S_Color.Length == 2)
            {
                for (int i = 0; i < this.mTexts.Count; i++)
                {
                    this.mTexts[i].color = this.segmentData.N_S_Color[0];
                }
                this.mTexts[index].color = this.segmentData.N_S_Color[1];
            }
            if (this.segmentData.IsEffer == true)
            {
                for (int i = 0; i < this.mTexts.Count; i++)
                {
                    mTexts[i].GetComponent<UnityEngine.UI.Gradient>().enabled = false;
                    mTexts[i].GetComponent<Shadow>().enabled = false;
                }
                mTexts[index].GetComponent<UnityEngine.UI.Gradient>().enabled = true;
                mTexts[index].GetComponent<Shadow>().enabled = true;
            }

            Vector3 newVector = new Vector3(go.transform.localPosition.x, -(rc.GetComponent<RectTransform>().sizeDelta.y - 2), 0.0f);
            Tweener tweener = line.transform.DOLocalMove(newVector, 0.3f).SetLoops(0).SetEase(Ease.Linear);
            tweener.Play();
        }

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }


        GameObject GetSegmentBtn(Transform parent)
        {
            GameObject obj = GameObject.Instantiate(rc.Get<GameObject>("segment_btn"));
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(parent);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            return obj;
        }

    }
}
