using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using BannerEle = ETHotfix.WEB_common_banner.DataElement;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMatch_BannerComponentSystem : AwakeSystem<UIMatch_BannerComponent>
    {
        public override void Awake(UIMatch_BannerComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UIMatch_BannerComponentUpdateSystem : UpdateSystem<UIMatch_BannerComponent>
    {
        public override void Update(UIMatch_BannerComponent self)
        {
            self.Update();
        }
    }

    public enum MoveDir
    {
        DIR_LEFT = -1,
        DIR_RIGTH = 1
    }

    /// <summary> 页面名: 大厅页的头部bannner  (分离UIMatchComponent的代码)</summary>
    public class UIMatch_BannerComponent : UIBaseComponent
    {

        private ReferenceCollector rc;

        public void Awake()
        {
            mDots = new List<Image>();
            InitUI();
            isLoad = true;
            BannerShowMove();
        }
        public override void OnShow(object obj)
        {
            RequestBanner();
        }
        public override void OnHide()
        {
        }

        public void SetBannerHide()
        {
            mIsHide = true;
        }
        /// <summary>
        /// 是否隐藏了     false才去计算
        /// </summary>
        bool mIsHide = false;

        public void Update()
        {//隐藏也会在update的  
            if (mIsHide == false)
            {
                Update_Banner();
            }
        }
        public override void Dispose()
        {
            isLoad = false;
            mDots = null;
            base.Dispose();
        }
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mBannerListView2 = rc.Get<GameObject>("ScrollView_TopCards").GetComponent<LoopListView2>();
            tDots = rc.Get<GameObject>("Dots").transform;
            imageDot = rc.Get<GameObject>("Image_Dot");
            ImageNonHasShow = rc.Get<GameObject>("ImageNonHasShow");
        }

        #region Banner SuperView
        bool isLoad = true;
        private GameObject ImageNonHasShow;
        private int mSelectIndex = 2;
        private LoopListView2 mBannerListView2;
        private Transform tDots;
        private GameObject imageDot;
        private MoveDir mCurDir = MoveDir.DIR_RIGTH;
        List<Image> mDots;
        List<BannerEle> mBannerEles;
        public void RequestBanner()
        {
            mIsHide = false;
            for (int i = 0; i < mDots.Count; i++)
            {
                GameObject.DestroyImmediate(mDots[i].gameObject);
            }
            mDots.Clear();

            UIMatchModel.mInstance.APIGetBanner(0, 0, tDto =>
            {
                if (ImageNonHasShow == null) return;
                ImageNonHasShow.SetActive(tDto.Count == 0);
                mBannerEles = new List<BannerEle>() { new BannerEle() };
                mBannerEles.AddRange(tDto);
                mBannerEles.Add(new BannerEle());
                if (mBannerListView2.ShownItemCount == 0)
                    mBannerListView2.InitListView(mBannerEles.Count + 1, OnBannerGetItemByIndex);
                else
                    mBannerListView2.SetListItemCount(mBannerEles.Count + 1, true);
                mBannerListView2.SetSnapTargetItemIndex(2);
                imageDot.SetActive(false);
                Image tImgD;
                for (int i = 0; i < tDto.Count; i++)
                {
                    tImgD = GameObject.Instantiate(imageDot).GetComponent<Image>();
                    tImgD.gameObject.SetActive(true);
                    tImgD.transform.SetParent(tDots);
                    tImgD.transform.localPosition = Vector3.zero;
                    tImgD.transform.localScale = Vector3.one;
                    mDots.Add(tImgD);
                }
                SetDotYellowColor(0);
            });
        }
        /// <summary>        /// pIndex从0开始        /// </summary>
        void SetDotYellowColor(int pIndex)
        {
            if (mDots == null || mDots.Count == 0 || pIndex >= mDots.Count || pIndex < 0) return;
            for (int i = 0; i < mDots.Count; i++)
            {
                if (pIndex == i)
                    mDots[pIndex].color = new Color32(233, 191, 128, 255);
                else
                    mDots[i].color = new Color32(39, 39, 43, 255);
            }
            mSelectIndex = pIndex + 2;
        }

        public void Update_Banner()
        {
            if (mBannerListView2 != null)
            {
                mBannerListView2.UpdateAllShownItemSnapData();
                int count = mBannerListView2.ShownItemCount;
                mSelectIndex = mBannerListView2.CurSnapNearestItemIndex;
                SetDotYellowColor(mSelectIndex - 2);
                for (int i = 0; i < count; ++i)
                {
                    LoopListViewItem2 item = mBannerListView2.GetShownItemByIndex(i);
                    if (item.gameObject.name == "view_card(Clone)")
                    {
                        float scale = 1 - Mathf.Abs(item.DistanceWithViewPortSnapCenter) / 6000f;
                        scale = Mathf.Clamp(scale, 0.8f, 1);
                        Transform rootObj = item.gameObject.transform.Find("root_obj");
                        rootObj.GetComponent<CanvasGroup>().alpha = scale;
                        rootObj.transform.localScale = new Vector3(scale, scale, 1);
                    }
                }
            }
        }

        LoopListViewItem2 OnBannerGetItemByIndex(LoopListView2 listView, int index)
        {
            if (index < 0)
            {
                return null;
            }
            LoopListViewItem2 item = null;
            if (index <= 1 || index == (mBannerEles.Count))
            {
                item = listView.NewListViewItem("view_card_enpty");
                return item;
            }
            item = listView.NewListViewItem("view_card");
            var raw = item.gameObject.transform.Find("root_obj").GetComponent<RawImage>();
            var enpetyImage = item.transform.Find("root_obj/EnptyImage").gameObject;
            if (string.IsNullOrEmpty(mBannerEles[index - 1].bannerImg) == false && raw.texture.name.Equals("images_dating_banner_moren_bg"))
            {
                WebImageHelper.SetHttpBanner(raw, mBannerEles[index - 1].bannerImg, delegate
                {
                    enpetyImage.SetActive(false);
                });
            }

            UIEventListener.Get(item.gameObject).onClick = (delegate (GameObject obj)
            {
                if (string.IsNullOrEmpty(mBannerEles[index - 1].bannerUrl) == false)
                {
                    UIComponent.Instance.ShowNoAnimation(UIType.UILogin_LoadingWeb, new UILogin_LoadingWebComponent.LoadingWebData()
                    {
                        mWebUrl = mBannerEles[index - 1].bannerUrl,
                        mTitleTxt = mBannerEles[index - 1].desc
                    });
                }
            });
            return item;
        }
        /// <summary>         计时器         </summary>
        async void BannerShowMove()
        {
            TimerComponent mTC = Game.Scene.ModelScene.GetComponent<TimerComponent>();
            while (isLoad)
            {
                await mTC.WaitAsync(4000);
                if (mBannerEles != null && mBannerEles.Count > 3 && mIsHide == false)//active=false隐藏  也会进入的计算了
                {
                    if (mCurDir == MoveDir.DIR_RIGTH)
                    {
                        if (mSelectIndex < mBannerEles.Count - 2)
                            mSelectIndex++;
                        else
                        {
                            mSelectIndex = mBannerEles.Count - 1;
                            mCurDir = MoveDir.DIR_LEFT;
                        }
                    }
                    else if (mCurDir == MoveDir.DIR_LEFT)
                    {
                        if (mSelectIndex > 2)
                        {
                            mSelectIndex--;
                        }
                        else
                        {
                            mSelectIndex = 3;
                            mCurDir = MoveDir.DIR_RIGTH;
                        }
                    }
                    mBannerListView2.SetSnapTargetItemIndex(mSelectIndex);
                    await mTC.WaitAsync(100);
                    SetDotYellowColor(mSelectIndex - 2);
                }
            }
        }

        #endregion




    }
}
