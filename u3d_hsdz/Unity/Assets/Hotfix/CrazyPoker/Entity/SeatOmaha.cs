using System.Collections.Generic;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class SeatOmahaAwakeSystem : AwakeSystem<SeatOmaha, Transform>
    {
        public override void Awake(SeatOmaha self, Transform trans)
        {
            self.Awake(trans);
        }
    }

    public class SeatOmaha : Seat
    {  
        protected override void InitUI(Transform trans)
        {
            base.InitUI(trans);

            listCardUIInfos.Add(new CardUIInfo()
            {
                    imageCard = imageCard2,
                    imageSelect = imageSelectCard2,
                    imageBack = imageCardBack2,
                    imageEye = imageEyeCard2,
            });
            listCardUIInfos.Add(new CardUIInfo()
            {
                    imageCard = imageCard3,
                    imageSelect = imageSelectCard3,
                    imageBack = imageCardBack3,
                    imageEye = imageEyeCard3,
            });

            listSmallCardUIInfos.Add(new CardUIInfo()
            {
                    imageCard = imageSmallCard2,
                    imageSelect = imageSmallSelectCard2,
            });
            listSmallCardUIInfos.Add(new CardUIInfo()
            {
                    imageCard = imageSmallCard3,
                    imageSelect = imageSmallSelectCard3,
            });

            listImageSmallCardBack.Add(imageSmallCardBack2);
            listImageSmallCardBack.Add(imageSmallCardBack3);

            UIEventListener.Get(listCardUIInfos[2].imageCard.gameObject).onClick = onClickCard;
            UIEventListener.Get(listCardUIInfos[3].imageCard.gameObject).onClick = onClickCard;
        }

        protected override void InitUIStaticData()
        {
            if (myCardsPos.Count != 4)
            {
                myCardsPos.Clear();
                myCardsPos.Add(new Vector3(-20, 0));
                myCardsPos.Add(new Vector3(40, 0));
                myCardsPos.Add(new Vector3(100, 0));
                myCardsPos.Add(new Vector3(160, 0));
            }

            if (myCardsRot.Count != 4)
            {
                myCardsRot.Clear();
                myCardsRot.Add(new Vector3(0, 0));
                myCardsRot.Add(new Vector3(0, 0));
                myCardsRot.Add(new Vector3(0, 0));
                myCardsRot.Add(new Vector3(0, 0));
            }

            if (backSmallCardPos.Count != 4)
            {
                backSmallCardPos.Clear();
                backSmallCardPos.Add(new Vector3(30, -8));
                backSmallCardPos.Add(new Vector3(22, -4));
                backSmallCardPos.Add(new Vector3(11, 0));
                backSmallCardPos.Add(new Vector3(0, 0));

                backSmallCardPos.Add(new Vector3(-32, 8));
                backSmallCardPos.Add(new Vector3(-20, 9));
                backSmallCardPos.Add(new Vector3(-8, 8));
                backSmallCardPos.Add(new Vector3(0, 0));

            }

            if (backSmallCardRot.Count != 4)
            {
                backSmallCardRot.Clear();
                backSmallCardRot.Add(new Vector3(0, 0, -60));
                backSmallCardRot.Add(new Vector3(0, 0, -50));
                backSmallCardRot.Add(new Vector3(0, 0, -36));
                backSmallCardRot.Add(new Vector3(0, 0, -18));

                backSmallCardRot.Add(new Vector3(0, 0, 35));
                backSmallCardRot.Add(new Vector3(0, 0, 15));
                backSmallCardRot.Add(new Vector3(0, 0, 0));
                backSmallCardRot.Add(new Vector3(0, 0, -18));
            }

            if (smallCardPos.Count != 4)
            {
                smallCardPos.Clear();
                smallCardPos.Add(new Vector3(-45, 0));
                smallCardPos.Add(new Vector3(-15, 0));
                smallCardPos.Add(new Vector3(15, 0));
                smallCardPos.Add(new Vector3(45, 0));
            }
        }

        public override void UpdateShowCardsId()
        {
            if (null == Player || null == GameCache.Instance.CurGame.MainPlayer || GameCache.Instance.CurGame.MainPlayer.userID != Player.userID ||
                GameCache.Instance.CurGame.MainPlayer.seatID != seatID)
            {
                return;
            }

            OmahaGame omahaGame = GameCache.Instance.CurGame as OmahaGame;
            if (null == omahaGame)
                return;

            for (int i = 0, n = omahaGame.showCardsIdOmaha.Count; i < n; i++)
            {
                listCardUIInfos[i].imageEye.gameObject.SetActive(omahaGame.showCardsIdOmaha[i] != 0);
            }
        }

        protected override Vector3 GetBackSmallCardPos(int index)
        {
            int mOffset = 0;
            Vector3 mTmpV3 = Trans.localPosition;
            if (mTmpV3.x > 0)
            {
                mOffset = 4;
            }
            return backSmallCardPos[index + mOffset];
        }

        protected override Vector3 GetBackSmallCardRot(int index)
        {
            int mOffset = 0;
            Vector3 mTmpV3 = Trans.localPosition;
            if (mTmpV3.x > 0)
            {
                mOffset = 4;
            }
            return backSmallCardRot[index + mOffset];
        }

        #region UI事件

        protected override void onClickCard(GameObject go)
        {
            int mIndex = -1;

            if (!int.TryParse(go.name.Substring(go.name.Length - 1), out mIndex))
            {
                return;
            }

            // 亮牌
            if (null == Player || Player.userID != GameCache.Instance.CurGame.MainPlayer.userID ||
                seatID != GameCache.Instance.CurGame.MainPlayer.seatID || Player.canPlayStatus == 0)
            {
                return;
            }

            List<sbyte> mShowCards = new List<sbyte>(){0, 0, 0, 0};
            CardUIInfo mCardUiInfo = null;
            for (int i = 0, n = listCardUIInfos.Count; i < n; i++)
            {
                mCardUiInfo = listCardUIInfos[i];
                if (null == mCardUiInfo)
                {
                    mShowCards[i] = 0;
                    continue;
                }

                if (i == mIndex)
                {
                    mShowCards[i] = (sbyte)(mCardUiInfo.imageEye.gameObject.activeInHierarchy ? 0 : 1);
                }
                else
                {
                    mShowCards[i] = (sbyte)(mCardUiInfo.imageEye.gameObject.activeInHierarchy ? 1 : 0);
                }
            }

            CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_GAME_OMAHA_SHOWDOWN()
            {
                roomID = GameCache.Instance.room_id,
                roomPath = GameCache.Instance.room_path,
                showCards = mShowCards
            });
        }

        #endregion
    }
}
