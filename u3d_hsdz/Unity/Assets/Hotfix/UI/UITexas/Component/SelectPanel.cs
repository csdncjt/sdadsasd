using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class SelectPanelAwakeSystem : AwakeSystem<SelectPanel>
    {
        public override void Awake(SelectPanel self)
        {
            self.Awake();
        }
    }

    public class CardItem
    {
        public Toggle item { get; set; }
        public Image card { get; set; }
        public sbyte cIndex { get; set; }
    }


    public class SelectPanel : UIBaseComponent
    {
        private ReferenceCollector rc;
        public Button closeBtn;
        public Button okBtn;
        public Text desText;

        public Transform content;

        public Toggle[] addToggles;

        public Image cardBack1;
        public Image cardBack2;
        public Image cardBack3;

        public Image[] showcards = new Image[3];
        public int maxNum;

        public sbyte[] selectIndexs = new sbyte[3];

        private Dictionary<int, CardItem> cards = new Dictionary<int, CardItem>();

        int cardCount;


        REQ_GAME_ASK_TEST rec;
        ReferenceCollector rcPokerSprite;

        Toggle tempCard;

        public void Awake()
        {
            ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle("portraitcard.unity3d");
            GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset("portraitcard.unity3d", "PortraitCard");
            if (null != bundleGameObject)
            {
                GameObject mObj = GameObject.Instantiate(bundleGameObject);
                rcPokerSprite = mObj.GetComponent<ReferenceCollector>();
            }

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            desText = rc.Get<GameObject>("desText").GetComponent<Text>();

            okBtn = rc.Get<GameObject>("okBtn").GetComponent<Button>();
            closeBtn = rc.Get<GameObject>("closeBtn").GetComponent<Button>();
            content = rc.Get<GameObject>("content").transform;

            showcards[0] = rc.Get<GameObject>("card1").GetComponent<Image>();
            showcards[1] = rc.Get<GameObject>("card2").GetComponent<Image>();
            showcards[2] = rc.Get<GameObject>("card3").GetComponent<Image>();

            cardBack1 = rc.Get<GameObject>("cardBack1").GetComponent<Image>();
            cardBack2 = rc.Get<GameObject>("cardBack2").GetComponent<Image>();
            cardBack3 = rc.Get<GameObject>("cardBack3").GetComponent<Image>();

            tempCard = content.GetComponentInChildren<Toggle>();
            tempCard.gameObject.SetActive(false);

            closeBtn.onClick.AddListener(delegate {
                UIComponent.Instance.Remove(UIType.UITexasTest);
            });

            okBtn.onClick.AddListener(delegate {

                bool isOk = true;
                List<sbyte> value = new List<sbyte>();
                if (maxNum == 1)
                {
                    if (selectIndexs[0] == sbyte.MaxValue)//没选
                    {
                        isOk = false;
                    }
                    else
                    {
                        value.Add(selectIndexs[0]);
                    }

                }

                if (maxNum == 3)
                {
                    if (selectIndexs[0] == sbyte.MaxValue || selectIndexs[1] == sbyte.MaxValue || selectIndexs[2] == sbyte.MaxValue)//没选
                    {
                        isOk = false;
                    }
                    else
                    {
                        value.Add(selectIndexs[0]);
                        value.Add(selectIndexs[1]);
                        value.Add(selectIndexs[2]);
                    }
                }
                if (isOk)
                {
                    CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_SEND_TEST()
                    {
                        roomId = GameCache.Instance.room_id,
                        roomPath = GameCache.Instance.room_path,
                        card = value
                    });
                }

                UIComponent.Instance.Remove(UIType.UITexasTest);
            });

            for (int i = 0; i < 3; i++)
            {
                showcards[i].gameObject.SetActive(false);
            }

            cardBack1.gameObject.SetActive(false);
            cardBack2.gameObject.SetActive(false);
            cardBack3.gameObject.SetActive(false);

            registerHandler();
        }

        public override void OnShow(object obj)
        {
            rec = obj as REQ_GAME_ASK_TEST;

            if(null == rec)
                return;

            if (rec.status != 0)
            {
                return;
            }

            showCards(rec.cardnum, rec.cards);
        }

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            if (this.IsDisposed)
                return;
            removeHandler();
            base.Dispose();
        }

        private void RefreshData()
        {
            //CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_ASK_TEST()
            //{
            //    roomId = GameCache.Instance.room_id,
            //    roomPath = GameCache.Instance.room_path,
            //});
        }


        void ClearList()
        {
            int childCount = content.childCount;
            for (int i = 1; i < childCount; i++)
            {
                GameObject.Destroy(content.GetChild(i).gameObject);
            }

            for (int i = 0; i < maxNum; i++)
            {
                selectIndexs[i] = sbyte.MaxValue;
            }

            for (int i = 0; i < 3; i++)
            {
                showcards[i].gameObject.SetActive(false);
            }

            cards.Clear();
        }

        public void showCards(int num, List<sbyte> cardList)
        {
            maxNum = num;
            ClearList();
            initShow(num);
            desText.text = "请选择" + num.ToString() + "张牌";
            cardCount = cardList.Count;
            for (int i = 0; i < cardCount; i++)
            {
                Toggle go = GameObject.Instantiate(tempCard);
                go.transform.SetParent(content);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                go.gameObject.SetActive(true);
                go.isOn = false;

                go.name = cardList[i].ToString();

                //go.onValueChanged.AddListener((bool value) => OnToggleClick(go, value));
                //go.onValueChanged.AddListener(delegate (bool isOn) {
                //    OnToggleClick(go, isOn);//data：欲传的参数
                //});

                go.onValueChanged.AddListener((v) =>
                {
                    OnToggleClick(go);
                });

                Image pcard = go.transform.Find("card").GetComponent<Image>();

                CardItem item = new CardItem();
                item.cIndex = cardList[i];
                pcard.sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(item.cIndex));
                item.item = go;
                item.card = pcard;
                cards[item.cIndex] = item;
            }
        }

        public void OnToggleClick(Toggle item)
        {
            sbyte cardNum = Convert.ToSByte(Convert.ToInt32(item.name));
            if (item.isOn)
            {
                ChooseCard(cardNum);
            }
            else
            {
                DisChooseCard(cardNum);
            }

        }

        public void initShow(int num)
        {
            //if (num == 1)
            //{
            //    cardBack1.gameObject.SetActive(false);
            //    cardBack2.gameObject.SetActive(false);
            //    cardBack3.gameObject.SetActive(false);
            //}
            //else
            //{
            //    cardBack1.gameObject.SetActive(false);
            //    cardBack2.gameObject.SetActive(false);
            //    cardBack3.gameObject.SetActive(false);
            //}
        }

        public void ChooseCard(sbyte cardNum)
        {
            if (cardNum == sbyte.MinValue)
                return;

            for (int i = 0; i < maxNum; i++)
            {
                if (selectIndexs[i] == sbyte.MaxValue)
                {
                    selectIndexs[i] = cardNum;
                    FreshShowCard();
                    return;
                }
            }

            //已经选择，替换第一个
            int yetCardNum = selectIndexs[0];
            selectIndexs[0] = cardNum;
            cards[yetCardNum].item.isOn = false;
            FreshShowCard();

        }

        public void DisChooseCard(sbyte cardNum)
        {
            if (cardNum == sbyte.MinValue)
                return;

            for (int i = 0; i < maxNum; i++)
            {
                if (selectIndexs[i] == cardNum)
                {
                    selectIndexs[i] = sbyte.MaxValue;
                    FreshShowCard();
                    return;
                }
            }
        }

        public void FreshShowCard()
        {
            for (int i = 0; i < maxNum; i++)
            {

                if (selectIndexs[i] == sbyte.MaxValue)
                {
                    showcards[i].gameObject.SetActive(false);
                }
                else
                {
                    showcards[i].gameObject.SetActive(true);
                    showcards[i].sprite = rcPokerSprite.Get<Sprite>(GameUtil.GetCardNameByNum(selectIndexs[i]));
                }
            }
        }

        private void registerHandler()
        {
           // CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_ASK_TEST, HANDLER_REQ_GAME_ASK_TEST);  // 测试
        }

        private void removeHandler()
        {
            //CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_ASK_TEST, HANDLER_REQ_GAME_ASK_TEST);  // 测试
        }


        //protected void HANDLER_REQ_GAME_ASK_TEST(ICPResponse response)
        //{
        //    rec = response as REQ_GAME_ASK_TEST;
        //    if (null == rec)
        //        return;

        //    if (rec.status != 0)
        //    {
        //        return;
        //    }

        //    showCards(rec.cardnum, rec.cards);
        //}


    }
}

   
