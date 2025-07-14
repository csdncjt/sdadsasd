using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UITexasComplaintComponentSystem : AwakeSystem<UITexasComplaintComponent>
    {
        public override void Awake(UITexasComplaintComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名: </summary>
    public class UITexasComplaintComponent : UIBaseComponent
    {
        public sealed class ComplaintData
        {
            public int mLoadPanelPlayerId;
            public List<PlayerComplaint> mLoadPanelPlayers;
        }
        /// <summary>         索引0=未选中       索引1=选中         </summary>
        Color32[] mColor32Select = new Color32[] { new Color32(168, 168, 168, 255), new Color32(185, 44, 49, 255) };
        private ReferenceCollector rc;
        private InputField inputfieldDesc;
        private Toggle Toggle1;
        private Toggle Toggle0;
        private RectTransform Image_Dialog;
        private GameObject TogglePlayer;
        List<Toggle> mPlayers_Toggle = new List<Toggle>();
        List<Text> mPlayers_Text = new List<Text>();
        private Transform PlayersTr;
        public int mLoadPanelPlayerId;
        private Text textDesc;


        public void Awake()
        {
            InitUI();
        }
        List<PlayerComplaint> mPlayers;
        public override void OnShow(object obj)
        {
            if (obj != null && obj is ComplaintData)
            {
                mPlayers = (obj as ComplaintData).mLoadPanelPlayers;
                mLoadPanelPlayerId = (obj as ComplaintData).mLoadPanelPlayerId;
                mRoomId = GameCache.Instance.room_id;

                var count = mPlayers.Count;
                GameObject tmp;
                var localV3 = TogglePlayer.transform.localPosition;
                for (int i = 0; i < count; i++)
                {
                    tmp = GameObject.Instantiate(TogglePlayer);
                    tmp.transform.SetParent(PlayersTr);
                    tmp.transform.localScale = new Vector3(1, 1, 1);
                    var tText = tmp.transform.Find("Text_Name").GetComponent<Text>();
                    var tToggle = tmp.GetComponent<Toggle>();

                    WebImageHelper.SetHeadImage(tmp.transform.Find("Mask/MemberIcon").GetComponent<RawImage>(), mPlayers[i].headPic);
                    var tmpY = -(i / 2) * 100 + localV3.y;
                    var tmpX = (i % 2) * 433 + localV3.x;
                    tmp.transform.localPosition = new Vector3(tmpX, tmpY, 0);

                    if (mPlayers[i].userID == mLoadPanelPlayerId)//投拆面板 入者
                    {
                        tToggle.isOn = true;
                        tToggle.interactable = false;
                        tText.color = mColor32Select[1];
                    }
                    tText.text = mPlayers[i].nick;
                    mPlayers_Toggle.Add(tToggle);
                    mPlayers_Text.Add(tText);
                    UIEventListener.Get(tmp).onClick = OnClickPlayerToggles;

                    tmp.gameObject.name = mPlayers[i].userID.ToString() + "_" + mPlayers[i].nick;
                }
                TogglePlayer.SetActive(false);
                int y = (int)Math.Ceiling((float)count / 2) * 85;
                Image_Dialog.sizeDelta = new Vector2(1008, 850 + y);
            }
        }

       /// <summary>         1.设置文本框  2.昵称的颜色</summary>
        private void OnClickPlayerToggles(GameObject go)
        {     
            var count = mPlayers_Toggle.Count;
            for (int i = 0; i < count; i++)
            {
                if (mPlayers_Toggle[i].isOn)
                {
                    var tIsOn = mPlayers_Toggle[i].gameObject.name.Split('_');
                    mPlayers_Text[i].color = mColor32Select[1];  
                }
                else
                {
                    mPlayers_Text[i].color = mColor32Select[0];
                }
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
            UIEventListener.Get(rc.Get<GameObject>("SureBtn")).onClick = ClickSureBtn;
            UIEventListener.Get(rc.Get<GameObject>("CloseBtn")).onClick = ClickCloseBtn;
            inputfieldDesc = rc.Get<GameObject>("inputField_Desc").GetComponent<InputField>();
            TogglePlayer = rc.Get<GameObject>("TogglePlayer").gameObject;

            Toggle1 = rc.Get<GameObject>("Toggle1").GetComponent<Toggle>();
            Toggle0 = rc.Get<GameObject>("Toggle0").GetComponent<Toggle>();
            textDesc = rc.Get<GameObject>("Text_Desc").GetComponent<Text>();
            Image_Dialog = rc.Get<GameObject>("Image_Dialog").GetComponent<RectTransform>();
            UIEventListener.Get(Toggle0.gameObject).onClick = OnClickToggle;
            UIEventListener.Get(Toggle1.gameObject).onClick = OnClickToggle;
            mPlayers_Toggle = new List<Toggle>();
            mPlayers_Text = new List<Text>();
            PlayersTr = rc.Get<GameObject>("Players").transform;

            APIQueryComplaintFee();
        }

        private void OnClickToggle(GameObject go)
        {
            if (Toggle0.isOn)
            {
                int y = (int)Math.Ceiling((float)mPlayers.Count / 2) * 85;
                Image_Dialog.sizeDelta = new Vector2(1008, 850 + y);
                PlayersTr.gameObject.SetActive(true);
                inputfieldDesc.text = "";
                //OnClickPlayerToggles(null);
            }
            else if (Toggle1.isOn)
            {
                Image_Dialog.sizeDelta = new Vector2(1008, 850);
                PlayersTr.gameObject.SetActive(false);
                inputfieldDesc.text = "";
                // inputfieldDesc.text = GameCache.Instance.CurGame.GetPlayerPlayerNickName(mLoadPanelPlayerId) + LanguageManager.mInstance.GetLanguageForKey("Complanin009");
            }
        }

        /// <summary>         点击关闭 按钮         </summary>
        private void ClickCloseBtn(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UITexasComplaint);
        }


        /// <summary>         点击提交 按钮         </summary>
        private void ClickSureBtn(GameObject go)
        {
            var tComplaintIds = new List<int>();
            if (Toggle0.isOn)//伙牌投诉
            {
                var count = mPlayers_Toggle.Count;
                for (int i = 0; i < count; i++)
                {
                    if (mPlayers_Toggle[i].isOn)
                    {
                        var tIsOn = mPlayers_Toggle[i].gameObject.name.Split('_');
                        tComplaintIds.Add(int.Parse(tIsOn[0]));
                    }
                }
                if (tComplaintIds.Count < 2)
                {
                    UIComponent.Instance.ToastLanguage("Complanin002");//投拆伙牌要两人以上");
                    return;
                }
            }
            else//另两种
            {
                tComplaintIds.Add(mLoadPanelPlayerId);
            }

            if (inputfieldDesc.text.Length > 50)
            {
                UIComponent.Instance.ToastLanguage("Complanin001");
                return;
            }

            APIComplaintRequest(tComplaintIds, delegate
            {
                UIComponent.Instance.ToastLanguage("Complanin003");//您的反馈已收到，谢谢您的反馈
            });
            UIComponent.Instance.Remove(UIType.UITexasComplaint);
        }


        #region 请求
        int mRoomId;
        void APIComplaintRequest(List<int> pUserIds, Action pSuccAct)
        {
            var sb = new System.Text.StringBuilder();
            if (Toggle0.isOn)//伙牌投诉
            {
                var count = mPlayers_Toggle.Count;
                for (int i = 0; i < count; i++)
                {
                    if (mPlayers_Toggle[i].isOn)
                    {
                        var tIsOn = mPlayers_Toggle[i].gameObject.name.Split('_');
                        sb.Append(tIsOn[1] + ",");
                    }
                }
                sb.Append("在牌局为'");
                sb.Append(GameCache.Instance.roomName);
                sb.Append("'中怀疑打伙牌");
            }
            else if (Toggle1.isOn)
            {
                sb.Append(GameCache.Instance.CurGame.GetPlayerPlayerNickName(mLoadPanelPlayerId));
                sb.Append("在牌局为'");
                sb.Append(GameCache.Instance.roomName);
                sb.Append("'中");
            }

            var requestData = new WEB2_complaintRequest.RequestData()

            {
                roomid = mRoomId,
                type = Toggle0.isOn ? 1 : Toggle1.isOn ? 3 : 2,
                content = sb.ToString() + "," + inputfieldDesc.text.Trim(),
                userIds = pUserIds,
                bbChip= GameCache.Instance.CurGame.bigBlind
            };
            HttpRequestComponent.Instance.Send(
                WEB2_complaintRequest.API,
                WEB2_complaintRequest.Request(requestData), pDto =>
                {
                    var responseData = WEB2_complaintRequest.Response(pDto);
                    if (responseData.status == 0)
                    {
                        pSuccAct();
                    }
                    else
                    {
                        UIComponent.Instance.Toast(responseData.status);
                    }
                });
        }


        void APIQueryComplaintFee()
        {
            var requestData = new WEB2_queryComplaintFee.RequestData()
            {
                bbChip = GameCache.Instance.CurGame.bigBlind
            };
            HttpRequestComponent.Instance.Send(
                WEB2_queryComplaintFee.API,
                WEB2_queryComplaintFee.Request(requestData), pDto =>
                {
                    var responseData = WEB2_queryComplaintFee.Response(pDto);
                    if (responseData.status == 0)
                    {
                        textDesc.text = UIMineModel.mInstance.GetLanguageFormat("UITexasCom009", responseData.data.ToString());
                    }
                    else
                    {
                        UIComponent.Instance.Toast(responseData.status);
                    }
                });
        }
        #endregion
    }
}





