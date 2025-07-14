using System.Collections.Generic;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class OmahaGameAwakeSystem : AwakeSystem<OmahaGame, Component>
    {
        public override void Awake(OmahaGame self, Component component)
        {
            self.Awake(component);
        }
    }

    [ObjectSystem]
    public class OmahaGameUpdateSystem : UpdateSystem<OmahaGame>
    {
        public override void Update(OmahaGame self)
        {
            self.Update();
        }
    }

    public partial class OmahaGame : TexasGame
    {
        public List<sbyte> showCardsIdOmaha;    // 结束时是否亮手牌 0 不亮 1亮 对应四张牌
        public int omahaBloody;  //  奥马哈血战模式0 1

        public override void OnShow(object obj)
        {
            UpdateRoom(obj);
        }

        public override void UpdateRoom(object obj)
        {
            REQ_GAME_OMAHA_ENTER_ROOM rec = obj as REQ_GAME_OMAHA_ENTER_ROOM;
            if (null == rec)
                return;

            showCardsIdOmaha = rec.showCardsId;
            omahaBloody = rec.omahaBloody;
            base.UpdateRoomCommon(rec.baseMsg, obj);
        }

        protected override List<sbyte> GetEnptyHandCards()
        {
            return new List<sbyte>() { -1, -1, -1, -1 };
        }

        protected override List<sbyte> GetHandCardsAtEnterRoom(object obj, int index)
        {
            REQ_GAME_OMAHA_ENTER_ROOM rec = obj as REQ_GAME_OMAHA_ENTER_ROOM;
            sbyte mFirstCard = rec.baseMsg.firstCardarray[index];
            sbyte mSecondCard = rec.baseMsg.SecondArray[index];
            sbyte mThirdCard = rec.thirdCardArray[index];
            sbyte mForthCard = rec.fourthCardArray[index];
            return new List<sbyte>() { mFirstCard, mSecondCard, mThirdCard, mForthCard };
        }

        protected override List<sbyte> GetHandCardsAtRecvWinner(object obj, int index)
        {
            REQ_GAME_OMAHA_RECV_WINNER rec = obj as REQ_GAME_OMAHA_RECV_WINNER;
            sbyte mFirstCard = rec.baseMsg.firstCardArray[index];
            sbyte mSecondCard = rec.baseMsg.secondCardArray[index];
            sbyte mThirdCard = rec.thirdCardArray[index];
            sbyte mForthCard = rec.fourthCardArray[index];
            return new List<sbyte>() { mFirstCard, mSecondCard, mThirdCard, mForthCard };
        }

        protected override List<sbyte> GetHandCardsAtRecvStartInfo(object obj, int index)
        {
            REQ_GAME_OMAHA_RECV_START_INFOR rec = obj as REQ_GAME_OMAHA_RECV_START_INFOR;
            sbyte mFirstCard = rec.baseMsg.firstCardArray[index];
            sbyte mSecondCard = rec.baseMsg.SecondCardArray[index];
            sbyte mThirdCard = rec.thirdCardArray[index];
            sbyte mForthCard = rec.fourthCardArray[index];
            return new List<sbyte>() { mFirstCard, mSecondCard, mThirdCard, mForthCard };
        }

        protected override string GetRoomTypeDes()
        {
            // return omahaBloody == 0 ? "奥马哈" : "奥马哈(血战)";
            return omahaBloody == 0 ? CPErrorCode.LanguageDescription(10009) : CPErrorCode.LanguageDescription(10010);
        }

        protected override void SetUpLogo()
        {
            imageLogo.sprite = this.rc.Get<Sprite>("icon_omaha");
            // imageLogo.SetNativeSize();
        }


        protected override void ResetShowCardsId()
        {
            if (null == showCardsIdOmaha)
            {
                showCardsIdOmaha = new List<sbyte>() { 0, 0, 0, 0 };
            }
            else
            {
                if (this.showCardsIdOmaha.Count != 4)
                {
                    showCardsIdOmaha.Clear();
                    for (int i = 0; i < 4; i++)
                    {
                        showCardsIdOmaha.Add(0);
                    }
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        showCardsIdOmaha[i] = 0;
                    }
                }
            }
        }

        protected override CardType GetCardType(out List<sbyte> highlightCards)
        {
            return CardTypeUtil.GetOmahaCardType(mainPlayer.cards, GameCache.Instance.CurGame.cards, out highlightCards);
        }

        public override int HandCards
        {
            get
            {
                return 4;
            }
        }

        // protected override void ClearAllData()
        // {
        //     base.ClearAllData();
        //     if (null != showCardsIdOmaha)
        //     {
        //         showCardsIdOmaha.Clear();
        //         showCardsIdOmaha = null;
        //     }
        //
        //     omahaBloody = 0;
        // }

        #region UI

        protected override void InitSeatByCount(int seatCount)
        {
            SeatOmaha mSeat = null;
            GameObject mGo = null;
            SeatUIInfo[] mInfos = GameUtil.SeatUIInfos[seatCount];
            for (int i = 0; i < seatCount; i++)
            {
                mGo = GameObject.Instantiate(transSeat.gameObject, transSeat.parent);
                mGo.name = $"Seat{i}";
                mGo.transform.localPosition = mInfos[i].Pos;
                mGo.transform.localRotation = Quaternion.identity;
                mGo.transform.localScale = Vector3.one;
                mSeat = ComponentFactory.CreateWithId<SeatOmaha, Transform>(i, mGo.transform);
                mSeat.InitSeatUIInfo(mInfos[i]);
                listSeat.Add(mSeat);
                dicSeatOnlyClient.Add(mSeat.ClientSeatId, mSeat);
            }
        }

        #endregion 

    }
}
