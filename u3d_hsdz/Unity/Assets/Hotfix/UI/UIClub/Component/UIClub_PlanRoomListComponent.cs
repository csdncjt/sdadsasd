using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using PlanDto = ETHotfix.WEB2_roomplan_list.DataElement;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIClub_PlanRoomListComponentSystem : AwakeSystem<UIClub_PlanRoomListComponent>
    {
        public override void Awake(UIClub_PlanRoomListComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名: </summary>
    public class UIClub_PlanRoomListComponent : UIBaseComponent
    {
        private ReferenceCollector rc;

        private Dictionary<string, Sprite> mDicStrSprite;

        private List<PlanDto> mHallDtos;

        public void Awake()
        {
            InitUI();
            LoadSpriteDics();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_PlanRoomList,() => {
                UIComponent.Instance.ShowNoAnimation(UIType.UIClub_RoomCreat, 0);
            });
            InitSuperView();

            InitRoomListData(true);
        }

        void InitRoomListData(bool isInit)
        {
            UIMatchModel.mInstance.APIPlanRoomList(pDto =>
            {
                mHallDtos = pDto.data ?? new List<PlanDto>();
                mHallDtos.Add(new PlanDto()); //追加一个空数据
                if (isInit)
                {
                    mLoopListView.InitListView(mHallDtos.Count, OnGetItemByIndex);
                }
                else
                {
                    OnDataSourceLoadMoreFinished(true);
                }
                
            });
        }

        #region  sprite load出来到dic

        void LoadSpriteDics()
        {
            mDicStrSprite = new Dictionary<string, Sprite>();
            AddDicSprite("icon_list_playing");
            AddDicSprite("icon_list_waiting");
            AddDicSprite("color_jp");
            AddDicSprite("color_baoxian");
            AddDicSprite("color_laizi");
            AddDicSprite("color_tongshi");
            AddDicSprite("color_xuezhan");
            //AddDicSprite("game_icon_61_0");
            //AddDicSprite("game_icon_61_1");
            //AddDicSprite("game_icon_61_2");
            //AddDicSprite("game_icon_91_0");
            //AddDicSprite("game_icon_91_1");
            //AddDicSprite("game_icon_91_2");
            //AddDicSprite("img_default_head");
        }

        void AddDicSprite(string str)
        {
            mDicStrSprite[str] = rc.Get<Sprite>(str);
        }
        #endregion

        #region 列表的Item显示

        private void SetItemData(GameObject go, PlanDto pDto)
        {
            if (go == null || pDto == null) return;
            go.transform.Find("Text_Creator").GetComponent<Text>().text = LanguageManager.Get("UIClub_PlanRomList_Creator") + pDto.creatorName;
            go.transform.Find("Image_People/Text_People").GetComponent<Text>().text = pDto.playerCount.ToString(); //人数

            string str1 = StringHelper.ShowGold(pDto.sbChip, false);
            string str2 = StringHelper.ShowGold(pDto.sbChip * 2, false);
            string str3 = StringHelper.ShowGold(pDto.qianzhu);
            go.transform.Find("Image_Blind/Text_Blind").GetComponent<Text>().text = StringHelper.ShowBlinds(str1,str2,str3); //盲注
            go.transform.Find("Image_TimeStart/Text_TimeStart").GetComponent<Text>().text =
                    LanguageManager.Get("UIClub_PlanRomList_StartTime") + pDto.startTime;
            go.transform.Find("Image_TimeEnd/Text_TimeEnd").GetComponent<Text>().text =
                    LanguageManager.Get("UIClub_PlanRomList_EndTime") + pDto.endTime;
            // WebImageHelper.SetHeadImage(go.transform.Find("Item_Icon").GetComponent<RawImage>(), pDto.logo);
            var tStatu = go.transform.Find("Text_Status").GetComponent<Text>();
            tStatu.text = GetStatuTxt(pDto.status); //计划状态。0-不开启，1-开启，2-结束
            tStatu.color = GetStatuColor(pDto.status);
            go.transform.Find("Image_Status").GetComponent<Image>().sprite = GetStatuSprite(pDto.status);

            go.transform.Find("Image_Type2/Text_Type2").GetComponent<Text>().text = UIMatchModel.mInstance.GetRealRoomPathStr(pDto.roomPath);

            //头像
            //go.transform.Find("Item_Icon").GetComponent<Image>().sprite = GetIconSprite(pDto.roomPath);

            //smallImages
            for (int i = 1; i < 6; i++)
            {
                go.transform.Find("SmallGo/SmallImage" + i.ToString()).gameObject.SetActive(false);
            }

            var tLangs = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIMatch_Script102");//保险^德州^血战^血进^同时^顺序^赖子^血战
            var count1 = SetSmall(go, "SmallGo/SmallImage1", pDto.isron == 1 ? tLangs[0] : "", mDicStrSprite["color_baoxian"], pDto.isron == 1);//"保险" : "", mDicStrSprite["color_baoxian"], pDto.isron == 1);
            var count2 = SetSmall(go, "SmallGo/SmallImage2", pDto.jpon == 1 ? "JP" : "", mDicStrSprite["color_jp"], pDto.jpon == 1);
            var count3 = 0;
            if (pDto.roomPath == RoomPath.Normal.GetHashCode()) //德州局
            {
            }
            else if (pDto.roomPath == RoomPath.Normal.GetHashCode()) //必下场
            {
            }
            else if (pDto.roomPath == RoomPath.PAP.GetHashCode()) //大菠萝
            {
            }
            else if (pDto.roomPath == RoomPath.Omaha.GetHashCode()) //奥马哈
            {
                count3 = SetSmall(go, "SmallGo/SmallImage3", pDto.omamod == 1 ? tLangs[7] : "", mDicStrSprite["color_xuezhan"], pDto.omamod == 1);// "血战"
            }

            var tItemTitle = go.transform.Find("Text_Title").GetComponent<Text>();
            if (count1 + count2 + count3 < 3)
            {
                tItemTitle.text = pDto.name;
            }
            else
            {
                UIMatchModel.mInstance.SetLargeTextContent(tItemTitle, pDto.name, 560);
            }
            tItemTitle.GetComponent<RectTransform>().sizeDelta = new Vector2(tItemTitle.preferredWidth, tItemTitle.preferredHeight);
            go.transform.Find("SmallGo").GetComponent<RectTransform>().localPosition = new Vector3(278 + tItemTitle.preferredWidth, -65, 0);

            Button buttonMore = go.transform.Find("Button_More").GetComponent<Button>();
            GameObject imageMoreBg = go.transform.Find("Image_MoreBg").gameObject;
            imageMoreBg.SetActive(false);
            UIEventListener.Get(buttonMore.gameObject).onClick = (tmp) =>
            {
                imageMoreBg.SetActive(!imageMoreBg.activeInHierarchy);
            };

            buttonMore.gameObject.SetActive(pDto.self == 1);

            Button buttonPauseStart = imageMoreBg.transform.Find("Button_PauseStart").GetComponent<Button>();
            Image btnImg = buttonPauseStart.transform.Find("Image").GetComponent<Image>();
            Text btnText = buttonPauseStart.transform.Find("Text").GetComponent<Text>();
            if (pDto.status == 0)
            {
                btnImg.sprite = buttonPauseStart.GetComponent<ReferenceCollector>().Get<Sprite>("community_edit_icon_qidong");
                btnText.text = LanguageManager.Get("UIClub_PlanRomList_Run");
            }
            else
            {
                btnImg.sprite = buttonPauseStart.GetComponent<ReferenceCollector>().Get<Sprite>("community_edit_icon_zanting");
                btnText.text = LanguageManager.Get("UIClub_PlanRomList_Pause");
            }
            UIEventListener.Get(buttonPauseStart.gameObject).onClick = (tmp) =>
            {
                UIMatchModel.mInstance.APIRoomPlanOperate(pDto.id, pDto.status == 0 ? 1 : 0, res =>
                {
                    InitRoomListData(false);
                });
            };
            Button buttonEnd = imageMoreBg.transform.Find("Button_End").GetComponent<Button>();
            UIEventListener.Get(buttonEnd.gameObject).onClick = (tmp) =>
            {
                UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                {
                    type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                    title = string.Empty,
                    content = LanguageManager.Get("UIClub_PlanRomList_EndConfirm"),
                    // contentCommit = "确定",
                    contentCommit = CPErrorCode.LanguageDescription(10012),
                    // contentCancel = "取消",
                    contentCancel = CPErrorCode.LanguageDescription(10013),
                    actionCommit = () =>
                    {
                        UIMatchModel.mInstance.APIRoomPlanOperate(pDto.id, 2, res =>
                        {
                            InitRoomListData(false);
                        });
                    },
                    actionCancel = null
                });
            };
        }

        int SetSmall(GameObject go, string pathImg, string txt, Sprite sprite, bool show = true)
        {
            int i = 0;
            go.transform.Find(pathImg).gameObject.SetActive(show);
            if (show)
            {
                go.transform.Find(pathImg + "/Text_Small").GetComponent<Text>().text = txt;
                go.transform.Find(pathImg).GetComponent<Image>().sprite = sprite;
                i = 1;
            }
            return i;
        }

        private Sprite GetStatuSprite(int pStatus)
        {
            if (pStatus == 0)
            {
                return mDicStrSprite["icon_list_waiting"];
            }
            else
            {
                return mDicStrSprite["icon_list_playing"];
            }
        }


        private Sprite GetIconSprite(int roomPath)
        {
            int mTmpCurLanguage = LanguageManager.mInstance.mCurLanguage;
            if (roomPath == 61 || roomPath == 62 || roomPath == 63)
            {
                string name = string.Format("game_icon_61_{0}", mTmpCurLanguage);
                return mDicStrSprite[name];
            }
            else if (roomPath == 91 || roomPath == 92 || roomPath == 93)
            {
                string name = string.Format("game_icon_91_{0}", mTmpCurLanguage);
                return mDicStrSprite[name];
            }
            return mDicStrSprite["img_default_head"];
        }

        string GetStatuTxt(int pStatus)
        {
            if (pStatus == 0)
            {
                return LanguageManager.Get("UIClub_PlanRomList_Paused");
            } else if (pStatus == 1)
            {
                return LanguageManager.Get("UIClub_PlanRomList_Runing");
            } else if (pStatus == 2)
            {
                return "已结束";
            }
            return "";
        }

        Color GetStatuColor(int pStatus)
        {
            if (pStatus == 0)
            {
                return new Color32(233, 191, 128, 255); //黄
            }
            else
            {
                return new Color32(61, 179, 102, 255);
            }
        }


        #endregion

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
        }

        #region SupperScrollView 
        int mDirDrag = -1;//-1上面的菊花     1下面的菊花
        private LoopListView2 mLoopListView;
        private LoadingTipStatus mLoadingTipStatus = LoadingTipStatus.None;
        float mLoadingTipItemHeight = 100;

        private void InitSuperView()
        {
            mLoopListView = rc.Get<GameObject>("ScrollView").GetComponent<LoopListView2>();
            mLoopListView.mOnEndDragAction = OnEndDrag;
            mLoopListView.mOnDownMoreDragAction = OnDownMoreDragAction;
            mLoopListView.mOnUpRefreshDragAction = OnUpRefreshDragAction;
        }


        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)//mCurTop不同时
        {
            if (index < 0)
            {
                return null;
            }
            LoopListViewItem2 item = null;
            if (index == 0)
            {
                mLoadingTipStatus = LoadingTipStatus.None;
                item = listView.NewListViewItem("ItemPrefab0");
                UpdateLoadingTip(item);
                return item;
            }

            var tDto = mHallDtos[index - 1];
            if (tDto == null)
            {
                return null;
            }
            item = listView.NewListViewItem("ItemInfo");
            ListItem2 itemScript = item.GetComponent<ListItem2>();
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
            }
            var go = item.gameObject;
            SetItemData(go, tDto);
            //UIEventListener.Get(go).onClick = (tmp) =>
            //{
            //    //UIComponent.Instance.Show(UIType.UIMine_PromotersBecomeBook);
            //};
            return item;
        }



        void UpdateLoadingTip(LoopListViewItem2 item)
        {
            if (item == null)
            {
                return;
            }
            ListItem0 itemScript0 = item.GetComponent<ListItem0>();
            if (itemScript0 == null)
            {
                return;
            }
            if (mLoadingTipStatus == LoadingTipStatus.None)
            {
                itemScript0.mRoot.SetActive(false);
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
            }
            else if (mLoadingTipStatus == LoadingTipStatus.WaitRelease)
            {
                itemScript0.mRoot.SetActive(true);
                itemScript0.mText.text = LanguageManager.mInstance.GetLanguageForKey("SuperView1");// "放开加载更多 ...";
                itemScript0.mArrow.SetActive(true);
                itemScript0.mWaitingIcon.SetActive(false);
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mLoadingTipItemHeight);
            }
            else if (mLoadingTipStatus == LoadingTipStatus.WaitLoad)
            {
                itemScript0.mRoot.SetActive(true);
                itemScript0.mArrow.SetActive(false);
                itemScript0.mWaitingIcon.SetActive(true);
                itemScript0.mText.text = LanguageManager.mInstance.GetLanguageForKey("SuperView2");// "加载中 ...";
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mLoadingTipItemHeight);
            }
            else if (mLoadingTipStatus == LoadingTipStatus.NoMoreData)
            {
                itemScript0.mRoot.SetActive(true);
                itemScript0.mArrow.SetActive(false);
                itemScript0.mWaitingIcon.SetActive(false);
                itemScript0.mText.text = LanguageManager.mInstance.GetLanguageForKey("SuperView3");//  "已经是最后一条啦";
                item.CachedRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mLoadingTipItemHeight);
            }
        }

        void OnEndDrag()
        {
            if (mDirDrag == -1)
            {
                if (mLoopListView.ShownItemCount == 0)
                {
                    return;
                }
                if (mLoadingTipStatus != LoadingTipStatus.None && mLoadingTipStatus != LoadingTipStatus.WaitRelease)
                {
                    return;
                }
                LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(0);
                if (item == null)
                {
                    return;
                }
                mLoopListView.OnItemSizeChanged(item.ItemIndex);
                if (mLoadingTipStatus != LoadingTipStatus.WaitRelease)
                {
                    return;
                }
                mLoadingTipStatus = LoadingTipStatus.WaitLoad;
                UpdateLoadingTip(item);
                InitRoomListData(false);
            }
        }

        /// <summary>  刷新用true,  追加使用count>0   </summary>
        async void OnDataSourceLoadMoreFinished(bool pLength)
        {
            if (mLoopListView.ShownItemCount == 0)
            {
                return;
            }

            await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(500);
            mLoadingTipStatus = pLength ? LoadingTipStatus.None : LoadingTipStatus.NoMoreData;
            mLoopListView.SetListItemCount(mHallDtos.Count, false);
            mLoopListView.RefreshAllShownItem();
        }

        /// <summary>         下面的菊花         </summary>
        private void OnDownMoreDragAction()
        {

        }

        /// <summary>         上面的菊花         </summary>
        private void OnUpRefreshDragAction()
        {
            mDirDrag = -1;
            if (mLoopListView.ShownItemCount == 0)
            {
                return;
            }
            if (mLoadingTipStatus != LoadingTipStatus.None && mLoadingTipStatus != LoadingTipStatus.WaitRelease)
            {
                return;
            }
            LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(0);
            if (item == null)
            {
                return;
            }
            ScrollRect sr = mLoopListView.ScrollRect;
            Vector3 pos = sr.content.anchoredPosition3D;
            if (pos.y < -mLoadingTipItemHeight)
            {
                if (mLoadingTipStatus != LoadingTipStatus.None)
                {
                    return;
                }
                mLoadingTipStatus = LoadingTipStatus.WaitRelease;
                UpdateLoadingTip(item);
                item.CachedRectTransform.anchoredPosition3D = new Vector3(0, mLoadingTipItemHeight, 0);
            }
            else
            {
                if (mLoadingTipStatus != LoadingTipStatus.WaitRelease)
                {
                    return;
                }
                mLoadingTipStatus = LoadingTipStatus.None;
                UpdateLoadingTip(item);
                item.CachedRectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
            }
        }

        #endregion
    }
}
