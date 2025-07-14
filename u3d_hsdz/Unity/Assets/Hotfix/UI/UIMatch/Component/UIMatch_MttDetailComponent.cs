using System;
using System.Collections;
using System.Collections.Generic;
using ETModel;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;
using DtoRoomInfo = ETHotfix.WEB2_room_mtt_view.Data;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMatch_MttDetailComponentSystem : AwakeSystem<UIMatch_MttDetailComponent>
    {
        public override void Awake(UIMatch_MttDetailComponent self)
        {
            self.Awake();
        }
    }

    public class UIMatch_MttDetailComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Transform transContents;
        Button btnSignUp;
        Text textBtn;

        /// <summary>头部 seg 从1开始   </summary>
        int mCurSegTop = 0;
		int matchID;

        bool hadInit = false;

		DtoRoomInfo roomInfo;

        string[] viewTypes = { UIType.UIMatch_MttDetailState, UIType.UIMatch_MttDetailPlayer, UIType.UIMatch_MttDetailReward, UIType.UIMatch_MttDetailDesk, UIType.UIMatch_MttDetailBlind };

        public void Awake()
        {
            mCurSegTop = 0;
            InitUI();
        }

		#region InitUI
		protected virtual void InitUI()
		{
			rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			transContents = rc.Get<GameObject>("Contents").transform;
            btnSignUp = rc.Get<GameObject>("Button_SignUp").GetComponent<Button>();
            textBtn = btnSignUp.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
            UIEventListener.Get(btnSignUp.gameObject).onClick = onClickSignBtn;
        }
        #endregion

        public override void OnShow(object obj)
        {
            SetUpNav(LanguageManager.Get("MTT_Official"), UIType.UIMatch_MttDetail);
            if (!hadInit)
            {
                UISegmentControlComponent.SetUp(rc.transform, new UISegmentControlComponent.SegmentData()
                {
                    //Titles = new string[] {"状态", "玩家", "奖励", "牌桌", "盲注" },
                    Titles = LanguageManager.mInstance.GetLanguageForKeyMoreValues("UIMatchMTTDetailList"),
                    OnClick = OnSegmentClick,
                    N_S_Fonts = new[] { 48, 48 },
                    N_S_Color = new Color32[] { new Color32(179, 142, 86, 255), new Color32(255, 255, 255, 255) },
                    IsEffer = true
                });
            }
            hadInit = true;

			matchID = (int)obj;

			ObtainMttDetailInfo();
        }


        /// <summary>         点击头部 选项         </summary>
        private void OnSegmentClick(GameObject go, int index)
        {
            if (index == mCurSegTop)
            {
                return;
            }
            mCurSegTop = index;
            for (int i = 0; i < transContents.childCount; i++)
            {
                Transform trans = transContents.GetChild(i);
                if (transContents.Find(viewTypes[index]) == trans)
                {
                    UIComponent.Instance.ShowNoAnimation(trans.name, roomInfo);
                }else
                {
                    UIComponent.Instance.HideNoAnimation(trans.name);
                }
            }

            if (index == 0)
			{
				ObtainMttDetailInfo();
			}
        }

        private void ObtainMttDetailInfo()
		{
            if (IsDisposed) return;
			UIMatchModel.mInstance.APIMTTView(matchID, tDot =>
			{
				roomInfo = tDot.data;
				UpdateBtn();
                MTTGameUtil.initList(roomInfo.blindNote, roomInfo.ante);
                GameCache.Instance.client_ip = roomInfo.clientIp;

                UI mUI = UIComponent.Instance.Get(UIType.UIMatch_MttDetailState);
                if (null != mUI)
                {
                    UIMatch_MttDetailStateComponent mUIComponent = mUI.UiBaseComponent as UIMatch_MttDetailStateComponent;
                    mUIComponent.UpdateInfo(roomInfo);
                }
            });
		}

        private void onClickSignBtn(GameObject go)
        {
            if (go.GetComponent<Button>().interactable == false)
                return;
            var tDto = new WEB2_room_mtt_list.RoomListElement();
            tDto.gameStatus = roomInfo.gameStatus;
            tDto.matchId = roomInfo.matchId;
            tDto.hasGpsLimit = roomInfo.hasGpsLimit;
            tDto.canPlayerRebuy = roomInfo.canPlayerRebuy;
            UIMatchMTTModel.mInstance.onMTTAction(tDto, UIType.UIMatch_MttList, finish =>
            {
                ObtainMttDetailInfo();
            });
        }

        private void UpdateBtn()
		{
            if (roomInfo.canPlayerRebuy)
            {
                //重购
                btnSignUp.interactable = true;
                textBtn.text = LanguageManager.Get("MTT_Rebuy");
            }
            else
            {
                // 主按钮状态
                //0:可报名;1:等待开赛;2:延期报名;3:进行中;4:立即进入;5:报名截止。
                btnSignUp.interactable = false;
                if (roomInfo.gameStatus == 0)
                {
                    //报名
                    textBtn.text = LanguageManager.Get("MTT-Apply");
                    btnSignUp.interactable = true;
                }
                else if (roomInfo.gameStatus == 1)
                {
                    //等待开赛
                    textBtn.text = LanguageManager.Get("mtt_btn_waiting_start");
                }
                else if (roomInfo.gameStatus == 2)
                {
                    //延时报名
                    textBtn.text = LanguageManager.Get("mtt_btn_delay");
                    btnSignUp.interactable = true;
                }
                else if (roomInfo.gameStatus == 3)
                {
                    //进行中
                    textBtn.text = LanguageManager.Get("mtt_btn_ongoing");
                    btnSignUp.interactable = true;
                }
                else if (roomInfo.gameStatus == 4)
                {
                    //立即进入
                    textBtn.text = LanguageManager.Get("mtt_btn_enter");
                    btnSignUp.interactable = true;
                }
                else if (roomInfo.gameStatus == 5)
                {
                    //报名截止
                    textBtn.text = LanguageManager.Get("mtt_btn_sign_up_deadline");
                }
                else if (roomInfo.gameStatus == 6)
                {
                    //等待审核
                    textBtn.text = LanguageManager.Get("mtt_btn_waiting_approval");
                }
                else if (roomInfo.gameStatus == 7)
                {
                    //已淘汰，且不能重购
                    textBtn.text = LanguageManager.Get("mtt_btn_rebuy");
                }
            }
            if (btnSignUp.interactable)
            {
                textBtn.color = new Color32(249, 243, 189, 255);
            }
            else
            {
                textBtn.color = new Color32(175, 175, 175, 255);
            }
		}

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            hadInit = false;
            base.Dispose();
        }




    }


}

