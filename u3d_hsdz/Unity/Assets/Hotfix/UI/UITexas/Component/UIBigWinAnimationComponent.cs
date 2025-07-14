using System;
using System.Collections.Generic;
using System.Net;
using BestHTTP;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DragonBones;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIBigWinAnimationComponentAwakeSystem : AwakeSystem<UIBigWinAnimationComponent>
    {
        public override void Awake(UIBigWinAnimationComponent self)
        {
            self.Awake();
        }
    }

    public class UIBigWinAnimationComponent : UIBaseComponent
    {
        public sealed class WinnerData
        {
            public List<sbyte> mCards;
            public sbyte seatID;
        }
        private WinnerData winnerData;
        List<GameObject> cardObjects = new List<GameObject>();

        private ReferenceCollector rc;
        private UnityArmatureComponent armatureJP;
        private UnityArmatureComponent armatureWin;
        private UnityArmatureComponent armaturekuang;
        private RawImage headImg;
        private Text textname;
        private Text textJP;
        private Text textwin;

        private float CardAnimationDuration = 1.4f;

        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            armatureJP = rc.Get<GameObject>("armature_JP").GetComponent<UnityArmatureComponent>();
            armatureWin = rc.Get<GameObject>("armature_Win").GetComponent<UnityArmatureComponent>();
            armaturekuang = rc.Get<GameObject>("armature_kuang").GetComponent<UnityArmatureComponent>();
            headImg = rc.Get<GameObject>("img_head").GetComponent<RawImage>();
            textname = rc.Get<GameObject>("Text_name").GetComponent<Text>();
            textJP = rc.Get<GameObject>("Text_JP").GetComponent<Text>();
            textwin = rc.Get<GameObject>("Text_win").GetComponent<Text>();
        }

        public override void OnShow(object obj)
        {
            GameCache.Instance.CurGame.isPlayingBigWinAnimation = true;
            if (null != obj)
            {
                winnerData = obj as WinnerData;

                foreach (GameObject gameObject in cardObjects)
                {
                    GameObject.Destroy(gameObject);
                }
                cardObjects.Clear();

                Seat seat = GameCache.Instance.CurGame.GetSeatById(winnerData.seatID);

                WebImageHelper.SetHeadImage(headImg, seat.Player.headPic);
                textname.text = CacheDataManager.mInstance.GetRemarkName(seat.Player.userID, seat.Player.nick);
                textwin.text = $"+{StringHelper.ShowGold(seat.Player.winChips)}";

                armatureWin.gameObject.SetActive(true);
                if (null != armatureWin.dragonAnimation)
                {
                    armatureWin.dragonAnimation.Reset();
                    armatureWin.dragonAnimation.Play();
                }

                SoundComponent.Instance.PlaySFX("sfx_bigwin");

                if (seat.Player.hitJackPotChips > 0)
                {
                    // 有击中JP
                    armatureJP.gameObject.SetActive(true);
                    if (null != armatureJP.dragonAnimation)
                    {
                        armatureJP.dragonAnimation.Reset();
                        armatureJP.dragonAnimation.Play();
                    }

                    textJP.gameObject.SetActive(true);
                    textJP.text = $"+{StringHelper.ShowGold(seat.Player.hitJackPotChips)}";
                    textwin.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(465, -86.5f, 0);
                }
                else
                {
                    armatureJP.gameObject.SetActive(false);

                    textJP.gameObject.SetActive(false);
                    textwin.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(231, -86.5f, 0);
                }

                Sequence sequenceAnimation = DOTween.Sequence();
                rc.gameObject.GetComponent<Image>().color = Color.clear;
                sequenceAnimation.Append(rc.gameObject.GetComponent<Image>().DOFade(0.8f, CardAnimationDuration));

                for (int i = 0; i < winnerData.mCards.Count; i++)
                {
                    sbyte cardIndex = winnerData.mCards[i];
                    if (cardIndex >=0 && cardIndex <= 4)
                    {
                        PublicCardInfo cardInfo = GameCache.Instance.CurGame.listCards[cardIndex];
                        GameObject newCard = GameObject.Instantiate(cardInfo.trans.gameObject);
                        newCard.gameObject.SetActive(true);
                        newCard.transform.SetParent(rc.transform);
                        newCard.transform.localScale = Vector3.one;
                        newCard.transform.localPosition = GameUtil.ChangeToLocalPos(cardInfo.trans, newCard.transform);
                        cardObjects.Add(newCard);

                        GameObject mObj = GameObject.Instantiate(armaturekuang.gameObject);
                        mObj.SetActive(true);
                        UnityArmatureComponent armatureComp = mObj.GetComponent<UnityArmatureComponent>();
                        armatureComp.transform.parent = newCard.transform;
                        armatureComp.transform.localPosition = new Vector3(0, -(newCard.transform as RectTransform).sizeDelta.y * 0.45f, 0);
                        armatureComp.transform.localScale = new Vector3(100, 100, 0);
                        armatureComp.dragonAnimation.Reset();
                        armatureComp.dragonAnimation.Play();

                        //cardInfo.trans.gameObject.SetActive(false);

                        Vector3 newPosition = GameCache.Instance.CurGame.listDefaultPublicCardsLPos[i];
                        newPosition.y = -300;
                        sequenceAnimation.Join(newCard.transform.DOLocalMove(newPosition, CardAnimationDuration));

                    }
                    if (cardIndex > 4 && cardIndex <= 8)
                    {
                        Seat.CardUIInfo seatInfo;
                        if (seat.IsMySeat)
                        {
                            seatInfo = seat.listCardUIInfos[cardIndex - 5];
                        }
                        else
                        {
                            seatInfo = seat.listSmallCardUIInfos[cardIndex - 5];
                        }
                        GameObject newCard = GameObject.Instantiate(seatInfo.imageCard.gameObject);
                        newCard.gameObject.SetActive(true);
                        newCard.transform.SetParent(rc.transform);
                        newCard.transform.localScale = Vector3.one;
                        newCard.transform.localPosition = GameUtil.ChangeToLocalPos(seatInfo.imageCard.transform, newCard.transform);
                        cardObjects.Add(newCard);

                        //seatInfo.imageCard.gameObject.SetActive(false);

                        Vector3 newPosition = GameCache.Instance.CurGame.listDefaultPublicCardsLPos[i];
                        newPosition.y = -300;
                        sequenceAnimation.Join(newCard.transform.DOLocalMove(newPosition, CardAnimationDuration));
                        sequenceAnimation.Join(newCard.transform.DOLocalRotate(Vector3.zero, CardAnimationDuration));
                        sequenceAnimation.Join(newCard.transform.DOScale((GameCache.Instance.CurGame.listCards[0].trans as RectTransform).sizeDelta.y / seatInfo.imageCard.rectTransform.sizeDelta.y, CardAnimationDuration));
                    }
                }
                sequenceAnimation.AppendInterval(3);
                sequenceAnimation.OnComplete(() =>
                {
                    if (rc.gameObject.activeInHierarchy)
                        UIComponent.Instance.HideNoAnimation(UIType.UIBigWinAnimation);
                });
                sequenceAnimation.Play();
            }

        }

        public override void OnHide()
        {
            GameCache.Instance.CurGame.isPlayingBigWinAnimation = false;
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            base.Dispose();
        }
    }
}
