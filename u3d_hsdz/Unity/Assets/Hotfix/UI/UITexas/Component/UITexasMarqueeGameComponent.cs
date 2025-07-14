using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using RewardData = ETHotfix.WEB2_raffle_user_prize_query.RewardData;

namespace ETHotfix
{
    [ObjectSystem]
    public class UITexasMarqueeGameComponentAwakeSystem : AwakeSystem<UITexasMarqueeGameComponent>
    {
        public override void Awake(UITexasMarqueeGameComponent self)
        {
            self.Awake();
        }
    }

    public class UITexasMarqueeGameComponent : UIBaseComponent
    {
        private static string spriteNameNum = "pmd_gl_";

        private ReferenceCollector rc;
        private Button buttonClose;
        private Image imageNum0;
        private Image imageNum1;
        private Image imageNum2;
        private Image imageNum3;
        private Image imageNum4;
        private Image imageNum5;
        private Image imageNum6;
        private Image imageNum7;
        private Image imageNum8;

        private List<Image> listNum;

        private string coin;

        //奖励信息列表
        static List<RewardData> rewardDatas;
        CZScrollRect scrollcomponent;

        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            buttonClose = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            imageNum0 = rc.Get<GameObject>("Image_Num0").GetComponent<Image>();
            imageNum1 = rc.Get<GameObject>("Image_Num1").GetComponent<Image>();
            imageNum2 = rc.Get<GameObject>("Image_Num2").GetComponent<Image>();
            imageNum3 = rc.Get<GameObject>("Image_Num3").GetComponent<Image>();
            imageNum4 = rc.Get<GameObject>("Image_Num4").GetComponent<Image>();
            imageNum5 = rc.Get<GameObject>("Image_Num5").GetComponent<Image>();
            imageNum6 = rc.Get<GameObject>("Image_Num6").GetComponent<Image>();
            imageNum7 = rc.Get<GameObject>("Image_Num7").GetComponent<Image>();
            imageNum8 = rc.Get<GameObject>("Image_Num8").GetComponent<Image>();

            if (null == listNum)
                listNum = new List<Image>();
            else
                listNum.Clear();
            listNum.Add(imageNum8);
            listNum.Add(imageNum7);
            listNum.Add(imageNum6);
            listNum.Add(imageNum5);
            listNum.Add(imageNum4);
            listNum.Add(imageNum3);
            listNum.Add(imageNum2);
            listNum.Add(imageNum1);
            listNum.Add(imageNum0);

            UIEventListener.Get(this.buttonClose.gameObject).onClick = onClickClose;

            UIEventListener.Get(rc.Get<GameObject>("Button_Explain")).onClick = (go) => { OnPage(0); };
            UIEventListener.Get(rc.Get<GameObject>("Button_Reward")).onClick = (go) => { OnPage(1); };

            scrollcomponent = new CZScrollRect();
            scrollcomponent.prefab = rc.Get<GameObject>("reward_item");
            scrollcomponent.scrollRect = rc.Get<GameObject>("reward_scrollview").GetComponent<ScrollRect>();
            scrollcomponent.onScrollObj = OnScrollObj;
            scrollcomponent.interval = 60;
            scrollcomponent.limitNum = 12;
            scrollcomponent.spacing = 5;
            scrollcomponent.Init();
        }

        private void onClickClose(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UITexasMarqueeGame);
        }

        public override void OnShow(object obj)
        {
            coin = string.Empty;
            if (null != obj)
                coin = obj.ToString();
            UpdateCoin();
            OnPage(0);
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;

            if (null != listNum)
                listNum.Clear();

            scrollcomponent.Dispose();
            scrollcomponent = null;
            rc = null;
            base.Dispose();

        }

        public void UpdateCoin()
        {
            if (!string.IsNullOrEmpty(this.coin))
            {
                int mCoin = int.Parse(this.coin);
                int pCoin = StringHelper.GetRateGold(mCoin);
                this.coin = pCoin.ToString().PadLeft(9, '0');
                for (int i = 0, n = this.listNum.Count; i < n; i++)
                {
                    this.listNum[i].sprite = this.rc.Get<Sprite>($"{spriteNameNum}{coin[i]}");
                }
            }
            else
            {
                for (int i = 0, n = this.listNum.Count; i < n; i++)
                {
                    this.listNum[i].sprite = this.rc.Get<Sprite>($"{spriteNameNum}{i}");
                }
            }
        }

        void OnPage(int page){

            if (page == 0) {

                rc.Get<GameObject>("Button_Explain").GetComponent<Image>().color = new Color(1,1,1,1);
                rc.Get<GameObject>("Button_Reward").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                rc.Get<GameObject>("Image_Frame").GetComponent<Image>().sprite = rc.Get<Sprite>("pmd_tc_bg1");
                rc.Get<GameObject>("page_explain").SetActive(true);
                rc.Get<GameObject>("page_reward").SetActive(false);
                rc.Get<GameObject>("Text_Explain").GetComponent<Text>().color = new Color32(25,21,21,255);
                rc.Get<GameObject>("Text_Reward").GetComponent<Text>().color = new Color32(233, 191, 128, 255);
            }
            else
            {
                rc.Get<GameObject>("Button_Explain").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                rc.Get<GameObject>("Button_Reward").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                rc.Get<GameObject>("Image_Frame").GetComponent<Image>().sprite = rc.Get<Sprite>("pmd_tc_bg2");
                rc.Get<GameObject>("page_explain").SetActive(false);
                rc.Get<GameObject>("page_reward").SetActive(true);
                rc.Get<GameObject>("Text_Explain").GetComponent<Text>().color = new Color32(233, 191, 128, 255);
                rc.Get<GameObject>("Text_Reward").GetComponent<Text>().color = new Color32(25, 21, 21, 255);

                OnRefresh();

                //获取奖励列表
                WEB2_raffle_user_prize_query.RequestData prizequery_req = new WEB2_raffle_user_prize_query.RequestData(){};
                HttpRequestComponent.Instance.Send(

                    WEB2_raffle_user_prize_query.API,
                    WEB2_raffle_user_prize_query.Request(prizequery_req),
                    (resData) =>
                    {
                        WEB2_raffle_user_prize_query.ResponseData prizequery_res = WEB2_raffle_user_prize_query.Response(resData);
                        if (prizequery_res.status == 0)
                        {
                            rewardDatas = prizequery_res.data;
                            OnRefresh();
                        }
                        else
                        {
                            //获取列表失败
                            Log.Error($"Error status = {prizequery_res.status} msg = {prizequery_res.msg}");
                            UIComponent.Instance.Toast(prizequery_res.status);
                        }
                    });

            }
        }

        void OnScrollObj(GameObject obj, int index)
        {
            int dex = index + 1;//第一个不显示
            obj.transform.GetChild(0).GetComponent<Text>().text = rewardDatas[dex].nickName; ;
            obj.transform.GetChild(1).GetComponent<Text>().text = StringHelper.ShowBlindName(rewardDatas[dex].manzhu);
            obj.transform.GetChild(2).GetComponent<Text>().text = StringHelper.ShowGold(rewardDatas[dex].kdou);
            obj.transform.GetChild(3).GetComponent<Text>().text = rewardDatas[dex].date;
        }

        void OnRefresh() {

            if (rewardDatas == null)
            {
                scrollcomponent.Refresh(0);
                return;
            }

            //刷新第一名信息
            if (rewardDatas.Count >= 1)
            {

                rc.Get<GameObject>("Text_Mvp_Name").GetComponent<Text>().text = rewardDatas[0].nickName;
                rc.Get<GameObject>("Text_Mvp_Lv").GetComponent<Text>().text = StringHelper.ShowBlindName(rewardDatas[0].manzhu);
                rc.Get<GameObject>("Text_Mvp_Reward").GetComponent<Text>().text = StringHelper.ShowGold(rewardDatas[0].kdou);
                rc.Get<GameObject>("Text_Mvp_Time").GetComponent<Text>().text = rewardDatas[0].date;
                //获取头像
                string headerId = rewardDatas[0].headImg;
                GameObject headerObj = rc.Get<GameObject>("Image_headMVP");
                if (headerId.Trim().Equals("-1"))
                {

                    headerObj.GetComponent<RawImage>().texture = WebImageHelper.GetDefaultHead();
                }
                else
                {
                    WebImageHelper.GetHeadImage(headerId, (t2d) => {

                        if (headerObj != null)
                        {
                            headerObj.GetComponent<RawImage>().texture = t2d;
                        }
                    });
                }
            }
            else
            {
                //列表没有数据时 清空mvp信息
                rc.Get<GameObject>("Text_Mvp_Name").GetComponent<Text>().text = "-";
                rc.Get<GameObject>("Text_Mvp_Lv").GetComponent<Text>().text = "-";
                rc.Get<GameObject>("Text_Mvp_Reward").GetComponent<Text>().text = "-";
                rc.Get<GameObject>("Text_Mvp_Time").GetComponent<Text>().text = "-";
            }
            scrollcomponent.Refresh(Math.Max(0, rewardDatas.Count - 1));//第一个不显示
        }
    }
}
