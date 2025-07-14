using System;
using System.Collections.Generic;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


namespace ETHotfix
{
    [ObjectSystem]
    public class UILobby_MenuComponentAwakeSystem : AwakeSystem<UILobby_MenuComponent>
    {
        public override void Awake(UILobby_MenuComponent self)
        {
            self.Awake();
        }
    }

    public class UILobby_MenuComponent : UIBaseComponent
    {

        public enum ePageType
        {
            Hide = -1,
            DISCOVER = 0,
            CLUB = 1,
            MATCH = 2,
            DATA = 3,
            MYINFO = 4,
        }

        ReferenceCollector rc;
        ReferenceCollector rcBtnImg;
        RectTransform rectTranBtn;
        Transform tranImageMove;
        Button btnDiscover;
        Button btnClub;
        Button btnLobby;
        Button btnData;
        Button btnMy;
        ePageType pageIndex;
        bool isShowAsyns;
        protected Sequence sequenceClickBtn;

        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            rcBtnImg = rc.Get<GameObject>("Ref_btnImg").GetComponent<ReferenceCollector>();
            rectTranBtn = rc.Get<GameObject>("mode_btn").GetComponent<RectTransform>();
            tranImageMove = rc.Get<GameObject>("Image_move").transform;

            btnDiscover = rc.Get<GameObject>(UILobbyAssitent.menu_btn_discover).GetComponent<Button>();
            btnClub = rc.Get<GameObject>(UILobbyAssitent.menu_btn_club).GetComponent<Button>();
            btnLobby = rc.Get<GameObject>(UILobbyAssitent.menu_btn_lobby).GetComponent<Button>();
            btnData = rc.Get<GameObject>(UILobbyAssitent.menu_btn_data).GetComponent<Button>();
            btnMy = rc.Get<GameObject>(UILobbyAssitent.menu_btn_my).GetComponent<Button>();

            UIEventListener.Get(btnDiscover.gameObject, (int)ePageType.DISCOVER).onIntClick = onClikButton;
            UIEventListener.Get(btnClub.gameObject, (int)ePageType.CLUB).onIntClick = onClikButton;
            UIEventListener.Get(btnLobby.gameObject, (int)ePageType.MATCH).onIntClick = onClikButton;
            UIEventListener.Get(btnData.gameObject, (int)ePageType.DATA).onIntClick = onClikButton;
            UIEventListener.Get(btnMy.gameObject, (int)ePageType.MYINFO).onIntClick = onClikButton;

            //pageIndex = ePageType.MATCH;//默认页码
            isShowAsyns = false;

            NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            float realBottom = safeArea.bottom * 1242 / safeArea.width;
            rectTranBtn.localPosition = new Vector3(0, realBottom, 0);
        }

        private void onClikButton(GameObject go, int page)
        {
            clickAnimation(go);
            ChangeView((ePageType)page);
        }

        public override void OnShow(object obj)
        {
            //移除UIDefault界面
            GameObject defaultObj = GameObject.Find("Global/UI/LobbyCanvas/UIDefault(Clone)");
            if (null != defaultObj)
            {
                GameObject.Destroy(defaultObj);
            }
        }

        private void clickAnimation(GameObject go)
        {
            sequenceClickBtn = DOTween.Sequence();
            sequenceClickBtn.Append(go.transform.DOScale(new Vector3(0.6f, 0.6f, 1), 0.05f));
            sequenceClickBtn.Append(go.transform.DOScale(new Vector3(1.0f, 1.0f, 1), 0.2f));
        }


        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            base.Dispose();
            pageIndex = ePageType.Hide;
        }

        public async void ChangeView(ePageType _pageIndex)
        {
            //Debug.Log($"ChangeView _pageIndex = {_pageIndex} isShowAsyns = {isShowAsyns}");
            if (pageIndex != _pageIndex && !isShowAsyns)
            {
                tranImageMove.DOLocalMoveX(btnDiscover.GetComponent<RectTransform>().sizeDelta.x * ((int)_pageIndex - 2), 0.3f);
                //onshow
                isShowAsyns = true;
                HightLightBtn(_pageIndex);
                switch (_pageIndex)
                {
                    case ePageType.DISCOVER://发现页
                        await ETHotfix.UIComponent.Instance.ShowAsyncNoAnimation(UIType.UIDiscover, null, () => { ViewCall(_pageIndex); });
                        break;
                    case ePageType.CLUB://俱乐部
                        await ETHotfix.UIComponent.Instance.ShowAsyncNoAnimation(UIType.UIClub, null, () => { ViewCall(_pageIndex); });
                        break;
                    case ePageType.MATCH://大厅
                        await ETHotfix.UIComponent.Instance.ShowAsyncNoAnimation(UIType.UIMatch, null, () => { ViewCall(_pageIndex); });

                        // 编辑器测试时候用
                        if (Application.isEditor && GameCache.Instance.isActivity == 1)
                            UIMatchModel.mInstance.APIActivityRechargeApply(1);

                        if (GameCache.Instance.kDouNum > 0)
                        {
                            int tmp = GameCache.Instance.kDouNum;
                            UIComponent.Instance.ShowNoAnimation(UIType.UIFirstRechargeActivitySuccess, tmp);
                            GameCache.Instance.kDouNum = 0;
                        }
                        else
                        {
                            if (GameCache.Instance.isActivity == 2 && GameCache.Instance.isFirstShowActivity)
                            {
                                GameCache.Instance.isFirstShowActivity = false;
                                UIComponent.Instance.ShowNoAnimation(UIType.UIFirstRechargeActivity);
                            }

                            if (GameCache.Instance.isActivity == 2 || GameCache.Instance.isActivity == 3)
                            {
                                UIMatchModel.mInstance.APIActivityRechargeQurey();
                            } 
                        }

                        break;
                    case ePageType.DATA://数据
                        await ETHotfix.UIComponent.Instance.ShowAsyncNoAnimation(UIType.UIData, null, () => { ViewCall(_pageIndex); });
                        break;
                    default://我的Mine
                        await ETHotfix.UIComponent.Instance.ShowAsyncNoAnimation(UIType.UIMine, null, () => { ViewCall(_pageIndex); });
                        //UIMineModel.mInstance.APIPromotionIsOpen(async pDto =>
                        //{
                        //    await ETHotfix.UIComponent.Instance.ShowAsyncNoAnimation(UIType.UIMine, pDto, () => { ViewCall(_pageIndex); });
                        //});
                        break;
                }
            }
        }

        void ViewCall(ePageType _pageIndex)
        {

            //Debug.Log($" ViewCall _pageIndex = {_pageIndex} isShowAsyns = {isShowAsyns}");
            if (pageIndex == _pageIndex) return;
            //onhide
            switch (pageIndex)
            {
                case ePageType.DISCOVER:
                    Game.Scene.GetComponent<UIComponent>().Remove(UIType.UIDiscover);
                    // UIComponent.Instance.HideNoAnimation(UIType.UIDiscover);
                    break;
                case ePageType.CLUB:
                    UIComponent.Instance.Remove(UIType.UIClub);
                    //Game.Scene.GetComponent<UIComponent>().Hide(UIType.UIClub , null , 0);
                    break;
                case ePageType.MATCH:
                    UIComponent.Instance.HideNoAnimation(UIType.UIMatch);
                    break;
                case ePageType.DATA:
                    //UIComponent.Instance.Remove(UIType.UIData);
                    Game.Scene.GetComponent<UIComponent>().Hide(UIType.UIData , null , 0);
                    break;
                default:
                    //UIComponent.Instance.Remove(UIType.UIMine);
                    Game.Scene.GetComponent<UIComponent>().Hide(UIType.UIMine, null, 0);
                    break;
            }
            pageIndex = _pageIndex;
            isShowAsyns = false;
            // System.GC.Collect();//只在主页切换时调起GC
        }

        void HightLightBtn(ePageType _pageIndex)
        {
            switch (_pageIndex)
            {
                case ePageType.DISCOVER:
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_discover).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_find_current");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_club).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_club_normal");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_data).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_data_normal");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_my).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_my_normal");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_lobby).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_lobby_normal");

                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_discover).transform.Find("Text").GetComponent<Text>().color = new Color32(234, 192, 129, 255);
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_club).transform.Find("Text").GetComponent<Text>().color = new Color32(139, 110, 76, 255);
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_data).transform.Find("Text").GetComponent<Text>().color = new Color32(139, 110, 76, 255);
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_my).transform.Find("Text").GetComponent<Text>().color = new Color32(139, 110, 76, 255);
                    break;
                case ePageType.CLUB:
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_discover).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_find_normal");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_club).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_club_current");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_data).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_data_normal");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_my).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_my_normal");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_lobby).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_lobby_normal");

                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_discover).transform.Find("Text").GetComponent<Text>().color = new Color32(139, 110, 76, 255);
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_club).transform.Find("Text").GetComponent<Text>().color = new Color32(234, 192, 129, 255);
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_data).transform.Find("Text").GetComponent<Text>().color = new Color32(139, 110, 76, 255);
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_my).transform.Find("Text").GetComponent<Text>().color = new Color32(139, 110, 76, 255);
                    break;
                case ePageType.MATCH:
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_discover).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_find_normal");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_club).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_club_normal");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_data).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_data_normal");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_my).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_my_normal");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_lobby).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_lobby_current");

                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_discover).transform.Find("Text").GetComponent<Text>().color = new Color32(139, 110, 76, 255);
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_club).transform.Find("Text").GetComponent<Text>().color = new Color32(139, 110, 76, 255);
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_data).transform.Find("Text").GetComponent<Text>().color = new Color32(139, 110, 76, 255);
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_my).transform.Find("Text").GetComponent<Text>().color = new Color32(139, 110, 76, 255);
                    break;
                case ePageType.DATA:
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_discover).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_find_normal");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_club).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_club_normal");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_data).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_data_current");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_my).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_my_normal");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_lobby).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_lobby_normal");

                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_discover).transform.Find("Text").GetComponent<Text>().color = new Color32(139, 110, 76, 255);
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_club).transform.Find("Text").GetComponent<Text>().color = new Color32(139, 110, 76, 255);
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_data).transform.Find("Text").GetComponent<Text>().color = new Color32(234, 192, 129, 255);
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_my).transform.Find("Text").GetComponent<Text>().color = new Color32(139, 110, 76, 255);
                    break;
                default:
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_discover).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_find_normal");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_club).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_club_normal");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_data).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_data_normal");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_my).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_my_current");
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_lobby).transform.Find("Image").GetComponent<Image>().sprite = rcBtnImg.Get<Sprite>("nav_lobby_normal");

                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_discover).transform.Find("Text").GetComponent<Text>().color = new Color32(139, 110, 76, 255);
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_club).transform.Find("Text").GetComponent<Text>().color = new Color32(139, 110, 76, 255);
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_data).transform.Find("Text").GetComponent<Text>().color = new Color32(139, 110, 76, 255);
                    rc.Get<GameObject>(UILobbyAssitent.menu_btn_my).transform.Find("Text").GetComponent<Text>().color = new Color32(234, 192, 129, 255);
                    break;
            }
        }
    }
}




