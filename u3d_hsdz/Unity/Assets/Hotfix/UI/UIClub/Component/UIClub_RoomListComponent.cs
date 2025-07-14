using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using HallDto = ETHotfix.WEB2_room_clublist.DataElement;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIClub_RoomListComponentSystem : AwakeSystem<UIClub_RoomListComponent>
    {
        public override void Awake(UIClub_RoomListComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名: </summary>
    public class UIClub_RoomListComponent : UIBaseComponent
    {
        private ReferenceCollector rc;

        private Dictionary<string, Sprite> mDicStrSprite;

        private List<HallDto> mHallDtos;
        private bool isEnterRoom = false;
        public void Awake()
        {
            InitUI();
            LoadSpriteDics();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_RoomList);
            InitSuperView();

            InitRoomListData(true);
        }

        void InitRoomListData(bool isInit)
        {
            UIMatchModel.mInstance.APIClubRoomList(pDto =>
            {
                mHallDtos = pDto.data ?? new List<HallDto>();
                mHallDtos.Add(new HallDto()); //追加一个空数据
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
            AddDicSprite("color_aomaha");
            AddDicSprite("game_icon_61_0");
            AddDicSprite("game_icon_61_1");
            AddDicSprite("game_icon_61_2");
            AddDicSprite("game_icon_91_0");
            AddDicSprite("game_icon_91_1");
            AddDicSprite("game_icon_91_2");
            AddDicSprite("img_default_head");
        }

        void AddDicSprite(string str)
        {
            mDicStrSprite[str] = rc.Get<Sprite>(str);
        }
        #endregion

        #region 列表的Item显示

        Vector3 mItemStartV3 = Vector3.zero;

        private void SetItemData(GameObject go, HallDto pDto)
        {
            if (go == null || pDto == null) return;
            go.transform.Find("Image_People/Text_People").GetComponent<Text>().text = pDto.stdn.ToString() + "/" + pDto.rpu.ToString(); //人数
            if (pDto.rpath == 21 || pDto.rpath == 23)
            {
                go.transform.Find("Image_Blind/Text_Blind").GetComponent<Text>().text = StringHelper.ShowGold(pDto.qianzhu);
            }
            else
            {
                string str1 = StringHelper.ShowGold(pDto.slmz, false);
                string str2 = StringHelper.ShowGold(pDto.slmz * 2, false);
                string str3 = StringHelper.ShowGold(pDto.qianzhu);
                string zhuatou = "";
                if (pDto.zhuatou == 1)
                {
                    zhuatou = StringHelper.ShowGold(pDto.slmz * 4);
                }
                go.transform.Find("Image_Blind/Text_Blind").GetComponent<Text>().text = StringHelper.ShowBlinds(str1, str2, str3, zhuatou);
            }

                go.transform.Find("Image_Time/Text_Time").GetComponent<Text>().text =
                    pDto.rmsec == 0 ? LanguageManager.mInstance.GetLanguageForKeyMoreValue("UIMatch_itemState", 0) : TimeHelper.ShowRemainingTimer(pDto.rmsec * 1000, false) + LanguageManager.mInstance.GetLanguageForKey("UIMatch_itemJiu");
            var tStatu = go.transform.Find("Text_Status").GetComponent<Text>();
            tStatu.text = GetStatuTxt(pDto.state); //0等待中 1进行中 2MTT延迟报名中 3MTT报名截止
            tStatu.color = GetStatuColor(pDto.state);
            go.transform.Find("Image_Status").GetComponent<Image>().sprite = GetStatuSprite(pDto.state);

            //算位置
            var tBlindWidth = go.transform.Find("Image_Blind/Text_Blind").GetComponent<Text>().preferredWidth + 60;
            var tPeopleWidth = tBlindWidth + go.transform.Find("Image_People/Text_People").GetComponent<Text>().preferredWidth + 60;
            if (mItemStartV3 == Vector3.zero)
                mItemStartV3 = go.transform.Find("Image_People").localPosition;
            go.transform.Find("Image_People").localPosition = new Vector3(mItemStartV3.x + tBlindWidth, mItemStartV3.y, mItemStartV3.z);
            go.transform.Find("Image_Time").localPosition = new Vector3(mItemStartV3.x + tPeopleWidth, mItemStartV3.y, mItemStartV3.z);

            go.transform.Find("Image_Type2/Text_Type2").GetComponent<Text>().text = UIMatchModel.mInstance.GetRoomPathStr(pDto.rtype);

            //头像
            go.transform.Find("Item_Icon").GetComponent<Image>().sprite = GetIconSprite(pDto.rtype);

            //smallImages
            for (int i = 1; i < 7; i++)
            {
                go.transform.Find("SmallGo/SmallImage" + i.ToString()).gameObject.SetActive(false);
            }

            var tLangs = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIMatch_Script102");//保险^德州^血战^血进^同时^顺序^赖子^血战
            var count1 = SetSmall(go, "SmallGo/SmallImage1", pDto.isron == 1 ? tLangs[0] : "", mDicStrSprite["color_baoxian"], pDto.isron == 1);//"保险" : "", mDicStrSprite["color_baoxian"], pDto.isron == 1);
            var count2 = SetSmall(go, "SmallGo/SmallImage2", pDto.jpon == 1 ? "JP" : "", mDicStrSprite["color_jp"], pDto.jpon == 1);
            var count3 = 0;
            if (pDto.rtype == RoomPath.Normal.GetHashCode()) //德州局
            {
            }
            else if (pDto.rtype == RoomPath.Normal.GetHashCode()) //必下场
            {
            }
            else if (pDto.rtype == RoomPath.PAP.GetHashCode()) //大菠萝
            {
                SetSmall(go, "SmallGo/SmallImage3", pDto.bpmod == 1 ? tLangs[1] : pDto.bpmod == 2 ? tLangs[2] : tLangs[3], mDicStrSprite["color_xuezhan"]);// "德州" : pDto.bpmod == 2 ? "血战" : "血进",
                SetSmall(go, "SmallGo/SmallImage4", pDto.dlmod == 0 ? tLangs[4] : tLangs[5], mDicStrSprite["color_tongshi"]);//"同时" : "顺序",
                SetSmall(go, "SmallGo/SmallImage5", pDto.jkon == 1 ? tLangs[6] : "", mDicStrSprite["color_laizi"], pDto.jkon == 1);// "赖子" 
            }
            else if (pDto.rtype == RoomPath.Omaha.GetHashCode()) //奥马哈
            {
                count3 = SetSmall(go, "SmallGo/SmallImage3", pDto.omamod == 1 ? tLangs[7] : "", mDicStrSprite["color_xuezhan"], pDto.omamod == 1);// "血战"
            }

            //比下场和AOF显示奥马哈标签
            if (pDto.rpath == RoomPath.OmahaThanOff.GetHashCode() || pDto.rpath == RoomPath.OmahaAof.GetHashCode())
            {
                var str = UIMatchModel.mInstance.GetRoomPathStr(RoomPath.Omaha.GetHashCode());
                SetSmall(go, "SmallGo/SmallImage6", str, mDicStrSprite["color_aomaha"], true);
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
            return LanguageManager.mInstance.GetLanguageForKeyMoreValue("UIMatch_itemState", (pStatus - 3));
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


        LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
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

            if (mHallDtos == null) return null;
            if (index == mHallDtos.Count)
            {
                item = listView.NewListViewItem("ItemPrefab0");
                UpdateLoadingTip(item);
                return item;
            }

            if (index - 1 > mHallDtos.Count) return null;
            var tDto = mHallDtos[index - 1];
            if (tDto == null) return null;

            item = listView.NewListViewItem("ItemInfo");
            if (item.IsInitHandlerCalled == false)
            {
                item.IsInitHandlerCalled = true;
            }

            var go = item.gameObject;
            SetItemData(go, tDto);
            UIEventListener.Get(go).onClick = (p) =>
            {
                if (isEnterRoom)
                    return;
                isEnterRoom = true;
                EnterRoomAPI(tDto.rpath, tDto.rmid);
            };
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

        #region 加入房间

        /// <summary> 加入牌局 </summary>
        void EnterRoomAPI(int pRoomPath, int pRoomId)
        {
            if (pRoomPath == RoomPath.Normal.GetHashCode() || pRoomPath == RoomPath.NormalThanOff.GetHashCode() || pRoomPath == RoomPath.NormalAof.GetHashCode())
            {
                UIMatchModel.mInstance.APIEnterNormalRoom(pRoomPath, pRoomId, pDto =>
                {
                    GameCache.Instance.roomName = pDto.name;
                    GameCache.Instance.roomIP = pDto.gmip;
                    GameCache.Instance.roomPort = int.Parse(pDto.gmprt);
                    GameCache.Instance.room_path = pDto.rpath;
                    GameCache.Instance.rtype = pDto.rtype;
                    GameCache.Instance.room_id = pDto.rmid;
                    GameCache.Instance.client_ip = pDto.cliip;
                    GameCache.Instance.carry_small = pDto.incp;
                    GameCache.Instance.straddle = pDto.sdlon;
                    GameCache.Instance.insurance = pDto.isron;
                    GameCache.Instance.muck_switch = pDto.muckon;
                    GameCache.Instance.jackPot_fund = pDto.jpfnd;
                    GameCache.Instance.jackPot_on = pDto.jpon;
                    GameCache.Instance.jackPot_id = pDto.jpid;
                    GameCache.Instance.shortest_time = pDto.gmxt;
                    AsyncEnterRoom();
                }, () => {
                    isEnterRoom = false;
                });
            }
            else if (pRoomPath == RoomPath.DP.GetHashCode() || pRoomPath == RoomPath.DPAof.GetHashCode())
            {
                UIMatchModel.mInstance.APIEnterShortRoom(pRoomPath, pRoomId, pDto =>
                {
                    GameCache.Instance.roomName = pDto.name;
                    GameCache.Instance.roomIP = pDto.gmip;
                    GameCache.Instance.roomPort = int.Parse(pDto.gmprt);
                    GameCache.Instance.room_path = pDto.rpath;
                    GameCache.Instance.rtype = pDto.rtype;
                    GameCache.Instance.room_id = pDto.rmid;
                    GameCache.Instance.client_ip = pDto.cliip;
                    GameCache.Instance.carry_small = pDto.incp;
                    GameCache.Instance.straddle = pDto.sdlon;
                    GameCache.Instance.insurance = pDto.isron;
                    GameCache.Instance.muck_switch = pDto.muckon;
                    GameCache.Instance.jackPot_fund = pDto.jpfnd;
                    GameCache.Instance.jackPot_on = pDto.jpon;
                    GameCache.Instance.jackPot_id = pDto.jpid;
                    GameCache.Instance.shortest_time = pDto.gmxt;
                    AsyncEnterRoom();
                }, () =>
                {
                    isEnterRoom = false;
                });
            }
            else if (pRoomPath == RoomPath.Omaha.GetHashCode() || pRoomPath == RoomPath.OmahaThanOff.GetHashCode() || pRoomPath == RoomPath.OmahaAof.GetHashCode())
            {
                UIMatchModel.mInstance.APIEnterOmahaRoom(pRoomPath, pRoomId, pDto =>
                {
                    GameCache.Instance.roomName = pDto.name;
                    GameCache.Instance.roomIP = pDto.gmip;
                    GameCache.Instance.roomPort = int.Parse(pDto.gmprt);
                    GameCache.Instance.room_path = pDto.rpath;
                    GameCache.Instance.rtype = pDto.rtype;
                    GameCache.Instance.room_id = pDto.rmid;
                    GameCache.Instance.client_ip = pDto.cliip;
                    GameCache.Instance.carry_small = pDto.incp;
                    GameCache.Instance.straddle = pDto.sdlon;
                    GameCache.Instance.insurance = pDto.isron;
                    GameCache.Instance.muck_switch = pDto.muckon;
                    GameCache.Instance.jackPot_fund = pDto.jpfnd;
                    GameCache.Instance.jackPot_on = pDto.jpon;
                    GameCache.Instance.jackPot_id = pDto.jpid;
                    GameCache.Instance.shortest_time = pDto.gmxt;
                    AsyncEnterRoom();
                }, () => {
                    isEnterRoom = false;
                });
            }
        }

        private async void AsyncEnterRoom()
        {
            UIComponent.Instance.ShowNoAnimation(UIType.UIMatch_Loading); //Loading页面.在UItexas生成就 remove掉        
            UIComponent.Instance.Remove(UIType.UIClub_Info);
            UIComponent.Instance.Remove(UIType.UIClub_RoomList);
            await UIComponent.Instance.ShowAsyncNoAnimation(UIType.UITexas, UIType.UIClub);
            isEnterRoom = false;
        }

        #endregion
    }
}
