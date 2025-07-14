using System.Collections.Generic;

namespace ETHotfix
{
    public partial class OmahaGame : TexasGame
    {
        protected override void registerHandler()
        {
            base.registerHandler();

            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_OMAHA_RECV_WINNER, HANDLER_REQ_GAME_OMAHA_RECV_WINNER);    // 奥马哈收到胜利者  (跟普通局25协议一样的返回，多出了145，146字段，则后两张手牌)
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_OMAHA_RECV_START_INFOR, HANDLER_REQ_GAME_OMAHA_RECV_START_INFOR);    // 奥马哈新一手开始  (跟普通局不同：150，151则后两张手牌)
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_OMAHA_PLAYER_CARDS, HANDLER_REQ_GAME_OMAHA_PLAYER_CARDS);    // 奥马哈Allin下发玩家手牌  (跟普通局不同：133，134则后两张手牌)
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_OMAHA_SHOWDOWN, HANDLER_REQ_GAME_OMAHA_SHOWDOWN);    // 奥马哈设置结束时亮的手牌  (跟普通局不同：133改为数组)
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_OMAHA_SHOW_CARDS, HANDLER_REQ_GAME_OMAHA_SHOW_CARDS);    // 奥马哈玩家亮牌协议（此协议在比牌后，其他玩家结束点击亮牌触发）

        }

        protected override void removeHandler()
        {
            base.removeHandler();

            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_OMAHA_RECV_WINNER, HANDLER_REQ_GAME_OMAHA_RECV_WINNER);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_OMAHA_RECV_START_INFOR, HANDLER_REQ_GAME_OMAHA_RECV_START_INFOR);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_OMAHA_PLAYER_CARDS, HANDLER_REQ_GAME_OMAHA_PLAYER_CARDS);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_OMAHA_SHOWDOWN, HANDLER_REQ_GAME_OMAHA_SHOWDOWN);
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_OMAHA_SHOW_CARDS, HANDLER_REQ_GAME_OMAHA_SHOW_CARDS);

        }

        protected void HANDLER_REQ_GAME_OMAHA_RECV_WINNER(ICPResponse response)
        {
            REQ_GAME_OMAHA_RECV_WINNER rec = response as REQ_GAME_OMAHA_RECV_WINNER;
            if (null == rec)
                return;

            handleWinnerInfoCommon(rec.baseMsg, response);
        }

        protected void HANDLER_REQ_GAME_OMAHA_RECV_START_INFOR(ICPResponse response)
        {
            REQ_GAME_OMAHA_RECV_START_INFOR rec = response as REQ_GAME_OMAHA_RECV_START_INFOR;
            if (null == rec)
                return;

            handleRecvStartInfoCommon(rec.baseMsg, response);
        }

        protected void HANDLER_REQ_GAME_OMAHA_PLAYER_CARDS(ICPResponse response)
        {
            REQ_GAME_OMAHA_PLAYER_CARDS rec = response as REQ_GAME_OMAHA_PLAYER_CARDS;
            if (null == rec)
                return;

            SeatOmaha mSeat = null;
            for (int i = 0, n = rec.playerSeat.Count; i < n; i++)
            {
                if (rec.playerSeat[i] == -1)
                    continue;

                mSeat = GetSeatById(rec.playerSeat[i]) as SeatOmaha;
                if (null == mSeat)
                    return;

                mSeat.Player.SetCards(new List<sbyte>() { rec.firstCards[i], rec.secondCards[i], rec.thirdCards[i], rec.fourthCards[i] });
                mSeat.UpdateCards();
            }
        }

        protected void HANDLER_REQ_GAME_OMAHA_SHOWDOWN(ICPResponse response)
        {
            REQ_GAME_OMAHA_SHOWDOWN rec = response as REQ_GAME_OMAHA_SHOWDOWN;
            if (null == rec)
                return;

            if (rec.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_GAME_OMAHA_SHOWDOWN,rec.status));
                return;
            }

            showCardsIdOmaha = rec.showCardsRec;

            Seat mSeat = GetSeatById(MainPlayer.seatID);
            if (null == mSeat)
                return;

            mSeat.UpdateShowCardsId();
        }


        protected void HANDLER_REQ_GAME_OMAHA_SHOW_CARDS(ICPResponse response)
        {
            REQ_GAME_OMAHA_SHOW_CARDS rec = response as REQ_GAME_OMAHA_SHOW_CARDS;
            if (null == rec)
                return;

            SeatOmaha mSeat = GetSeatById(rec.seatId) as SeatOmaha;
            if (null != mSeat && null != mSeat.Player)
            {
                mSeat.Player.SetCards(rec.cards);
                mSeat.UpdateCards();
            }
        }
    }
}
