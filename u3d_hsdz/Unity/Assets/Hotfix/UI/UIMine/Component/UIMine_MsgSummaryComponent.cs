using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using DtoMsg = ETHotfix.WEB2_message_nums_detail.DataElement;


namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_MsgSummaryComponentSystem : AwakeSystem<UIMine_MsgSummaryComponent>
    {
        public override void Awake(UIMine_MsgSummaryComponent self)
        {
            self.Awake();
        }
    }

    public enum EnumMSG
    {
        MSG_Club = 1,//俱乐部
        MSG_League,//联盟
        MSG_System,//系统
        MSG_Money,//钱包
        MSG_Share,//分享赚USDT
        MSG_Backpack = 6//背包消息
    }

    public enum EnumMSGFROM
    {
        MSGFROM_Msg = 1,//大厅
        MSGFROM_Club,//俱乐部
        MSGFROM_Game,//游戏
    }

    /// <summary> 页面名: </summary>
    public class UIMine_MsgSummaryComponent : UIBaseComponent
    {
        public sealed class MsgSummaryData
        {
            /// <summary>0=大厅,             1我的</summary>
            public int mOpenResource;//
            public Dictionary<int, DtoMsg> mDicMsgLast;
        }


        private ReferenceCollector rc;
        Dictionary<EnumMSG, GameObject> mDicEnumGO = new Dictionary<EnumMSG, GameObject>();
        /// <summary>0=大厅,             1我的</summary>
        private int mCurOpenLoad;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            if (obj != null && obj is MsgSummaryData)
            {
                var tMsgClass = obj as MsgSummaryData;
                mCurOpenLoad = tMsgClass.mOpenResource;
                var pDtos = tMsgClass.mDicMsgLast;


                for (int i = 1; i < 7; i++)
                {
                    SetItemInfo(mDicEnumGO[(EnumMSG)i]);
                }
                if (pDtos != null && pDtos.Count > 0)
                {
                    foreach (var item in pDtos)
                    {
                        SetItemInfo(mDicEnumGO[(EnumMSG)item.Key], item.Value);
                    }
                }
            }
            SetUpNav(UIType.UIMine_MsgSummary, null, ClickLeftBtn);
        }

        private void ClickLeftBtn()
        {
            UIComponent.Instance.Remove(UIType.UIMine_MsgSummary);
            UIMineModel.mInstance.RemoveMsgSummary(mCurOpenLoad);
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
            mDicEnumGO = new Dictionary<EnumMSG, GameObject>();
            string tName = string.Empty;
            for (int i = 1; i < 7; i++)
            {
                tName = "ItemInfo_" + i.ToString();
                var tItemInfo = rc.Get<GameObject>(tName);
                UIEventListener.Get(tItemInfo, i).onIntClick = OnClickTypeItem;
                mDicEnumGO[(EnumMSG)i] = tItemInfo;
            }
            mCurOpenLoad = 0;
        }


        void SetItemInfo(GameObject go, DtoMsg pDto)
        {
            go.transform.Find("Text_Time").GetComponent<Text>().text = TimeHelper.GetDateTimer(pDto.time).ToString("yyyy/MM/dd HH:mm");

            string tValue = UIMineModel.mInstance.GetMsg(pDto.type);
            if (string.IsNullOrEmpty(tValue) == false)
            {
                if (pDto.type == 611) pDto.title = "...";

                var str1 = pDto.content;
                var str2 = pDto.remark;
                int bnum = 0;

                //if (StringHelper.isNumberic(str2, out bnum))
                //{
                //    str2 = StringHelper.ShowGold(bnum);
                //}
                //else
                //{
                //    if (StringHelper.isNumberic(str1, out bnum))
                //    {
                //        str1 = StringHelper.ShowGold(bnum);
                //    }
                //}


                go.transform.Find("Text_Content").GetComponent<Text>().text = string.Format(tValue, str1, str2, pDto.title);
            }
            go.transform.Find("Image_Num").gameObject.SetActive(pDto.num > 0);
            if (pDto.num > 0)
            {
                go.transform.Find("Image_Num/Text_Num").GetComponent<Text>().text = pDto.num.ToString();
            }
        }

        void SetItemInfo(GameObject go)
        {
            go.transform.Find("Text_Time").GetComponent<Text>().text = "";
            go.transform.Find("Text_Content").GetComponent<Text>().text = LanguageManager.mInstance.GetLanguageForKey("MsgContentNull"); //"暂无消息";
            go.transform.Find("Image_Num").gameObject.SetActive(false);
        }


        private void OnClickTypeItem(GameObject go, int index)
        {
            if (index == EnumMSG.MSG_System.GetHashCode())
            {
                UIComponent.Instance.Show(UIType.UIMine_MsgSystem);
                UIMineModel.mInstance.APIMsgClearNoRead(index, delegate
                      {
                          mDicEnumGO[EnumMSG.MSG_System].transform.Find("Image_Num").gameObject.SetActive(false);
                      });
            }
            else
            {
                UIComponent.Instance.Show(UIType.UIMine_MsgNormal, new object[2] { index ,EnumMSGFROM.MSGFROM_Msg});
                UIMineModel.mInstance.APIMsgClearNoRead(index, delegate
                {
                    mDicEnumGO[(EnumMSG)index].transform.Find("Image_Num").gameObject.SetActive(false); ;
                });
            }
        }


    }
}
