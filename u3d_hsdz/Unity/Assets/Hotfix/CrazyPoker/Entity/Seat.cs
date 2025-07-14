using System;
using System.Collections.Generic;
using DG.Tweening;
using DragonBones;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class SeatAwakeSystem : AwakeSystem<Seat, Transform>
    {
        public override void Awake(Seat self, Transform trans)
        {
            self.Awake(trans);
        }
    }

    public sealed class SeatUIInfo
    {
        public Vector3 Pos { get; set; }
        public Vector3 BankerPos { get; set; }
        public Vector3 CardBackPos { get; set; }
        public Vector3 CardBackRot { get; set; }
        public Vector3 CardsPos { get; set; }
        public Vector3 CurRoundHaveBetPos { get; set; }
        public Vector3 BubblePos { get; set; }
    }

    public partial class Seat : Entity
    {
        public sealed class CardUIInfo
        {
            public Image imageCard;
            public Image imageSelect;
            public Image imageBack;
            public Image imageEye;
        }

        /// <summary>
        /// 自己手牌位置
        /// </summary>
        protected static readonly List<Vector3> myCardsPos = new List<Vector3>();
        /// <summary>
        /// 自己手牌旋转
        /// </summary>
        protected static readonly List<Vector3> myCardsRot = new List<Vector3>();

        /// <summary>
        /// 小手牌
        /// </summary>
        protected static readonly List<Vector3> backSmallCardPos = new List<Vector3>();
        /// <summary>
        /// 小手牌
        /// </summary>
        protected static readonly List<Vector3> backSmallCardRot = new List<Vector3>();

        /// <summary>
        /// 输赢时显示的手牌位置
        /// </summary>
        protected static readonly List<Vector3> smallCardPos = new List<Vector3>();

        public FSMLogicComponent FsmLogicComponent;

        public int ClientSeatId;    // 客户端座位号
        public sbyte seatID { get; set; }   // 服务器座位号

        public Player Player { get; set; }  // 玩家信息

        public bool isSmall { get; set; }   // 是否小盲
        public bool isBig { get; set; }     // 是否大盲
        public bool isBank { get; set; }    // 是否庄家
        public bool isStraddle { get; set; }// 是否Straddle

        public int keepSeatLeftTime { get; set; }  // 留座剩余时间（s）

        #region Tweener

        protected Tweener tweenerPlayRecyclingWinChipAnimation;
        protected Sequence sequencePlayRecyclingChipAnimation;
        protected Sequence sequencePlayDealAnimation;
        protected Sequence sequenceSitAnimationEnter;
        protected Sequence sequenceStandupAnimationEnter;
        protected Tweener tweenerPlayBankerAnimation;
        protected Tweener tweenerPlayBetAnimation;
        protected Sequence sequencePlayFoldAnimation;
        protected Sequence sequenceUpdateBubble;
        protected Tweener tweenerHideBubble;
        protected Tweener tweenerUpdateBubbleInsurance;

        #endregion


        public virtual void Awake(Transform trans)
        {
            FsmLogicComponent = GetComponent<FSMLogicComponent>() ?? AddComponent<FSMLogicComponent, Entity>(this);
            InitUI(trans);
            InitUIStaticData();
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            KillAllTweener();

            if (null != FsmLogicComponent)
            {
                FsmLogicComponent.Dispose();
                RemoveComponent<FSMLogicComponent>();
                FsmLogicComponent = null;
            }

            ClearUI();
            ClearData();

            base.Dispose();
        }

        #region UI

        public SeatUIInfo seatUIInfo;

        public Transform Trans { get; set; }    // UI
        protected ReferenceCollector rc;
        protected UnityArmatureComponent armatureAllin;
        protected UnityArmatureComponent armatureWin;
        protected UnityArmatureComponent armatureYouWin;
        protected UnityArmatureComponent armatureVoice;
        protected UnityArmatureComponent armatureLight;
        protected Transform transBubble;
        protected Transform transCards;
        protected Transform transCurRoundHaveBet;
        protected Image imageBackDesk;
        protected Image imageBanker;
        protected Image imageBubble;
        protected Image imageBubbleBackDesk;
        protected Image Image_BubbleWaitDairu;
        protected Image imageBubbleInsurance;
        protected Image imageBubbleInsuranceCountDown;
        protected Image imageCard0;
        protected Image imageCard1;
        protected Image imageCard2;
        protected Image imageCard3;
        protected Image imageCardBack0;
        protected Image imageCardBack1;
        protected Image imageCardBack2;
        protected Image imageCardBack3;
        protected Image imageCardType;
        protected Image imageCoinShadow;
        protected Image imageCountDown;
        protected Image imageCurRoundHaveBetFrame;
        protected Image imageEmpty;
        protected Image imageEyeCard0;
        protected Image imageEyeCard1;
        protected Image imageEyeCard2;
        protected Image imageEyeCard3;
        protected Image imageHeadFrame;
        protected Image imageHeadGray;
        protected Image imageHolding;
        protected Image imageIconChip;
        protected Image imageNicknameShadow;
        protected Image imageRecyclingWinChip;
        protected Image imageSelectCard0;
        protected Image imageSelectCard1;
        protected Image imageSelectCard2;
        protected Image imageSelectCard3;
        protected Image imageSmallCard0;
        protected Image imageSmallCard1;
        protected Image imageSmallCard2;
        protected Image imageSmallCard3;
        protected Image imageSmallCardBack0;
        protected Image imageSmallCardBack1;
        protected Image imageSmallCardBack2;
        protected Image imageSmallCardBack3;
        protected Image imageSmallCardType;
        protected Image imageSmallSelectCard0;
        protected Image imageSmallSelectCard1;
        protected Image imageSmallSelectCard2;
        protected Image imageSmallSelectCard3;
        protected Image imageStraddle;
        protected Image imageWinner;
        protected Transform transMyCardType;
        protected Image imageTrust;
        protected RawImage rawimageHead;
        protected Transform transSmallCardBacks;
        protected Transform transSmallCards;
        protected Text textBubble;
        protected Text textBubbleBackDesk;
        protected Text textBubbleInsurance;
        protected Text textBubbleInsuranceCountDown;
        protected Text textBubbleDauru;
        protected Text textCardType;
        protected Text textCoin;
        protected Text textCurRoundHaveBet;
        protected Text textHolding;
        protected Text textNickname;
        protected Text textSmallCardType;
        protected Text textWinner;
        protected Transform transWinner;
        protected GameObject goEmoji;

        public List<CardUIInfo> listCardUIInfos;
        public List<CardUIInfo> listSmallCardUIInfos;
        /// <summary>
        /// 发的手牌
        /// </summary>
        protected List<Image> listImageSmallCardBack;

        protected float optCurTime;
        protected float optTotalTime;
        protected bool isCountDown;

        protected Vector3 defaultIconChipLocalPos;

        protected UIEmojiComponent emojiComponent;

        /// <summary>
        /// 是否留座离桌倒计时中
        /// </summary>
        protected bool bKeepSeatCounting;
        protected float keepSeatDeltaTime;

        public float time_05 = 0.4f;

        protected virtual void InitUI(Transform trans)
        {
            Trans = trans;
            rc = Trans.GetComponent<ReferenceCollector>();
            armatureAllin = rc.Get<GameObject>("Armature_Allin").GetComponent<UnityArmatureComponent>();
            armatureWin = rc.Get<GameObject>("Armature_Win").GetComponent<UnityArmatureComponent>();
            armatureYouWin = rc.Get<GameObject>("Armature_YouWin").GetComponent<UnityArmatureComponent>();
            armatureVoice = rc.Get<GameObject>("Armature_Voice").GetComponent<UnityArmatureComponent>();
            armatureLight = rc.Get<GameObject>("Armature_Light").GetComponent<UnityArmatureComponent>();
            transBubble = rc.Get<GameObject>("Bubble").transform;
            transCards = rc.Get<GameObject>("Cards").transform;
            transCurRoundHaveBet = rc.Get<GameObject>("CurRoundHaveBet").transform;
            imageBackDesk = rc.Get<GameObject>("Image_BackDesk").GetComponent<Image>();
            imageBanker = rc.Get<GameObject>("Image_Banker").GetComponent<Image>();
            imageBubble = rc.Get<GameObject>("Image_Bubble").GetComponent<Image>();
            imageBubbleBackDesk = rc.Get<GameObject>("Image_BubbleBackDesk").GetComponent<Image>();
            Image_BubbleWaitDairu = rc.Get<GameObject>("Image_BubbleWaitDairu").GetComponent<Image>();
            imageBubbleInsurance = rc.Get<GameObject>("Image_BubbleInsurance").GetComponent<Image>();
            imageBubbleInsuranceCountDown = rc.Get<GameObject>("Image_BubbleInsuranceCountDown").GetComponent<Image>();
            imageCard0 = rc.Get<GameObject>("Image_Card0").GetComponent<Image>();
            imageCard1 = rc.Get<GameObject>("Image_Card1").GetComponent<Image>();
            imageCard2 = rc.Get<GameObject>("Image_Card2").GetComponent<Image>();
            imageCard3 = rc.Get<GameObject>("Image_Card3").GetComponent<Image>();
            imageCardBack0 = rc.Get<GameObject>("Image_CardBack0").GetComponent<Image>();
            imageCardBack1 = rc.Get<GameObject>("Image_CardBack1").GetComponent<Image>();
            imageCardBack2 = rc.Get<GameObject>("Image_CardBack2").GetComponent<Image>();
            imageCardBack3 = rc.Get<GameObject>("Image_CardBack3").GetComponent<Image>();
            imageCardType = rc.Get<GameObject>("Image_CardType").GetComponent<Image>();
            imageCoinShadow = rc.Get<GameObject>("Image_CoinShadow").GetComponent<Image>();
            imageCountDown = rc.Get<GameObject>("Image_CountDown").GetComponent<Image>();
            imageCurRoundHaveBetFrame = rc.Get<GameObject>("Image_CurRoundHaveBetFrame").GetComponent<Image>();
            imageEmpty = rc.Get<GameObject>("Image_Empty").GetComponent<Image>();
            imageEyeCard0 = rc.Get<GameObject>("Image_EyeCard0").GetComponent<Image>();
            imageEyeCard1 = rc.Get<GameObject>("Image_EyeCard1").GetComponent<Image>();
            imageEyeCard2 = rc.Get<GameObject>("Image_EyeCard2").GetComponent<Image>();
            imageEyeCard3 = rc.Get<GameObject>("Image_EyeCard3").GetComponent<Image>();
            imageHeadFrame = rc.Get<GameObject>("Image_HeadFrame").GetComponent<Image>();
            imageHeadGray = rc.Get<GameObject>("Image_HeadGray").GetComponent<Image>();
            imageHolding = rc.Get<GameObject>("Image_Holding").GetComponent<Image>();
            imageIconChip = rc.Get<GameObject>("Image_IconChip").GetComponent<Image>();
            imageNicknameShadow = rc.Get<GameObject>("Image_NicknameShadow").GetComponent<Image>();
            imageRecyclingWinChip = rc.Get<GameObject>("Image_RecyclingWinChip").GetComponent<Image>();
            imageSelectCard0 = rc.Get<GameObject>("Image_SelectCard0").GetComponent<Image>();
            imageSelectCard1 = rc.Get<GameObject>("Image_SelectCard1").GetComponent<Image>();
            imageSelectCard2 = rc.Get<GameObject>("Image_SelectCard2").GetComponent<Image>();
            imageSelectCard3 = rc.Get<GameObject>("Image_SelectCard3").GetComponent<Image>();
            imageSmallCard0 = rc.Get<GameObject>("Image_SmallCard0").GetComponent<Image>();
            imageSmallCard1 = rc.Get<GameObject>("Image_SmallCard1").GetComponent<Image>();
            imageSmallCard2 = rc.Get<GameObject>("Image_SmallCard2").GetComponent<Image>();
            imageSmallCard3 = rc.Get<GameObject>("Image_SmallCard3").GetComponent<Image>();
            imageSmallCardBack0 = rc.Get<GameObject>("Image_SmallCardBack0").GetComponent<Image>();
            imageSmallCardBack1 = rc.Get<GameObject>("Image_SmallCardBack1").GetComponent<Image>();
            imageSmallCardBack2 = rc.Get<GameObject>("Image_SmallCardBack2").GetComponent<Image>();
            imageSmallCardBack3 = rc.Get<GameObject>("Image_SmallCardBack3").GetComponent<Image>();
            imageSmallCardType = rc.Get<GameObject>("Image_SmallCardType").GetComponent<Image>();
            imageSmallSelectCard0 = rc.Get<GameObject>("Image_SmallSelectCard0").GetComponent<Image>();
            imageSmallSelectCard1 = rc.Get<GameObject>("Image_SmallSelectCard1").GetComponent<Image>();
            imageSmallSelectCard2 = rc.Get<GameObject>("Image_SmallSelectCard2").GetComponent<Image>();
            imageSmallSelectCard3 = rc.Get<GameObject>("Image_SmallSelectCard3").GetComponent<Image>();
            imageStraddle = rc.Get<GameObject>("Image_Straddle").GetComponent<Image>();
            imageTrust = rc.Get<GameObject>("Image_Trust").GetComponent<Image>();
            imageWinner = rc.Get<GameObject>("Image_Winner").GetComponent<Image>();
            transMyCardType = rc.Get<GameObject>("MyCardType").transform;
            rawimageHead = rc.Get<GameObject>("RawImage_Head").GetComponent<RawImage>();
            transSmallCardBacks = rc.Get<GameObject>("SmallCardBacks").transform;
            transSmallCards = rc.Get<GameObject>("SmallCards").transform;
            textBubble = rc.Get<GameObject>("Text_Bubble").GetComponent<Text>();
            textBubbleBackDesk = rc.Get<GameObject>("Text_BubbleBackDesk").GetComponent<Text>();
            textBubbleInsurance = rc.Get<GameObject>("Text_BubbleInsurance").GetComponent<Text>();
            textBubbleInsuranceCountDown = rc.Get<GameObject>("Text_BubbleInsuranceCountDown").GetComponent<Text>();
            textBubbleDauru = rc.Get<GameObject>("Text_BubbleWaitDairu").GetComponent<Text>();
            textCardType = rc.Get<GameObject>("Text_CardType").GetComponent<Text>();
            textCoin = rc.Get<GameObject>("Text_Coin").GetComponent<Text>();
            textCurRoundHaveBet = rc.Get<GameObject>("Text_CurRoundHaveBet").GetComponent<Text>();
            textHolding = rc.Get<GameObject>("Text_Holding").GetComponent<Text>();
            textNickname = rc.Get<GameObject>("Text_Nickname").GetComponent<Text>();
            textSmallCardType = rc.Get<GameObject>("Text_SmallCardType").GetComponent<Text>();
            textWinner = rc.Get<GameObject>("Text_Winner").GetComponent<Text>();
            transWinner = rc.Get<GameObject>("Winner").transform;
            goEmoji = rc.Get<GameObject>("Image_Emoji");
            emojiComponent = ComponentFactory.Create<UIEmojiComponent, Transform>(goEmoji.transform);

            if (null == listCardUIInfos)
            {
                listCardUIInfos = new List<CardUIInfo>();
            }
            if (listCardUIInfos.Count > 0)
                listCardUIInfos.Clear();

            listCardUIInfos.Add(new CardUIInfo()
            {
                imageCard = imageCard0,
                imageSelect = imageSelectCard0,
                imageBack = imageCardBack0,
                imageEye = imageEyeCard0,
            });
            listCardUIInfos.Add(new CardUIInfo()
            {
                imageCard = imageCard1,
                imageSelect = imageSelectCard1,
                imageBack = imageCardBack1,
                imageEye = imageEyeCard1,
            });

            if (null == listSmallCardUIInfos)
                listSmallCardUIInfos = new List<CardUIInfo>();
            if (listSmallCardUIInfos.Count > 0)
                listSmallCardUIInfos.Clear();

            listSmallCardUIInfos.Add(new CardUIInfo()
            {
                imageCard = imageSmallCard0,
                imageSelect = imageSmallSelectCard0,
            });
            listSmallCardUIInfos.Add(new CardUIInfo()
            {
                imageCard = imageSmallCard1,
                imageSelect = imageSmallSelectCard1,
            });

            if (null == listImageSmallCardBack)
                listImageSmallCardBack = new List<Image>();
            if (listImageSmallCardBack.Count > 0)
                listImageSmallCardBack.Clear();

            listImageSmallCardBack.Add(imageSmallCardBack0);
            listImageSmallCardBack.Add(imageSmallCardBack1);

            UIEventListener.Get(imageEmpty.gameObject).onClick = onClickEmpty;
            UIEventListener.Get(rawimageHead.gameObject).onClick = onClickHead;
            UIEventListener.Get(imageBackDesk.gameObject).onClick = onClickBackDesk;

            for (int i = 0, n = listCardUIInfos.Count; i < n; i++)
            {
                UIEventListener.Get(listCardUIInfos[i].imageCard.gameObject).onClick = onClickCard;
            }
        }

        /// <summary>
        /// 清空UI
        /// </summary>
        protected virtual void ClearUI()
        {
            Trans = null;
            rc = null;
            armatureAllin = null;
            armatureWin = null;
            armatureYouWin = null;
            armatureVoice = null;
            armatureLight = null;
            transBubble = null;
            transCards = null;
            transCurRoundHaveBet = null;
            imageBackDesk = null;
            imageBanker = null;
            imageBubble = null;
            imageBubbleBackDesk = null;
            Image_BubbleWaitDairu = null;
            imageBubbleInsurance = null;
            imageBubbleInsuranceCountDown = null;
            imageCard0 = null;
            imageCard1 = null;
            imageCard2 = null;
            imageCard3 = null;
            imageCardBack0 = null;
            imageCardBack1 = null;
            imageCardBack2 = null;
            imageCardBack3 = null;
            imageCardType = null;
            imageCoinShadow = null;
            imageCountDown = null;
            imageCurRoundHaveBetFrame = null;
            imageEmpty = null;
            imageEyeCard0 = null;
            imageEyeCard1 = null;
            imageEyeCard2 = null;
            imageEyeCard3 = null;
            imageHeadFrame = null;
            imageHeadGray = null;
            imageHolding = null;
            imageIconChip = null;
            imageNicknameShadow = null;
            imageRecyclingWinChip = null;
            imageSelectCard0 = null;
            imageSelectCard1 = null;
            imageSelectCard2 = null;
            imageSelectCard3 = null;
            imageSmallCard0 = null;
            imageSmallCard1 = null;
            imageSmallCard2 = null;
            imageSmallCard3 = null;
            imageSmallCardBack0 = null;
            imageSmallCardBack1 = null;
            imageSmallCardBack2 = null;
            imageSmallCardBack3 = null;
            imageSmallCardType = null;
            imageSmallSelectCard0 = null;
            imageSmallSelectCard1 = null;
            imageSmallSelectCard2 = null;
            imageSmallSelectCard3 = null;
            imageStraddle = null;
            imageTrust = null;
            imageWinner = null;
            transMyCardType = null;
            rawimageHead = null;
            transSmallCardBacks = null;
            transSmallCards = null;
            textBubble = null;
            textBubbleBackDesk = null;
            textBubbleInsurance = null;
            textBubbleInsuranceCountDown = null;
            textCardType = null;
            textCoin = null;
            textCurRoundHaveBet = null;
            textHolding = null;
            textNickname = null;
            textSmallCardType = null;
            textWinner = null;
            transWinner = null;
            goEmoji = null;
            emojiComponent = null;

            if (null != listCardUIInfos)
            {
                listCardUIInfos.Clear();
                listCardUIInfos = null;
            }

            if (null != listSmallCardUIInfos)
            {
                listSmallCardUIInfos.Clear();
                listSmallCardUIInfos = null;
            }

            if (null != listImageSmallCardBack)
            {
                listImageSmallCardBack.Clear();
                listImageSmallCardBack = null;
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public virtual void ClearData()
        {
            ClientSeatId = -1;
            seatID = -1;
            if (null != Player)
            {
                Player.Dispose();
                Player = null;
            }

            isSmall = false;
            isBig = false;
            isBank = false;
            isStraddle = false;
            keepSeatLeftTime = 0;

            optCurTime = 0;
            optTotalTime = 0;
            isCountDown = false;

            defaultIconChipLocalPos = Vector3.zero;

            bKeepSeatCounting = false;
            keepSeatDeltaTime = 0;
        }

        protected virtual void InitUIStaticData()
        {
            if (myCardsPos.Count != 2)
            {
                myCardsPos.Clear();
                myCardsPos.Add(new Vector3(0, 0));
                myCardsPos.Add(new Vector3(67, 2));
            }

            if (myCardsRot.Count != 2)
            {
                myCardsRot.Clear();
                myCardsRot.Add(new Vector3(0, 0));
                myCardsRot.Add(new Vector3(0, 0, -8));
            }

            if (backSmallCardPos.Count != 2)
            {
                backSmallCardPos.Clear();
                backSmallCardPos.Add(new Vector3(0, 0));
                backSmallCardPos.Add(new Vector3(0, 0));
            }

            if (backSmallCardRot.Count != 2)
            {
                backSmallCardRot.Clear();
                backSmallCardRot.Add(new Vector3(0, 0, 0));
                backSmallCardRot.Add(new Vector3(0, 0, -10));
            }

            if (smallCardPos.Count != 2)
            {
                smallCardPos.Clear();
                smallCardPos.Add(new Vector3(-37, 0));
                smallCardPos.Add(new Vector3(37, 0));
            }

        }

        /// <summary>
        /// 初始化SeatUI元素
        /// </summary>
        /// <param name="info"></param>
        public virtual void InitSeatUIInfo(SeatUIInfo info)
        {
            ClientSeatId = Convert.ToInt32(Trans.name.Substring(Trans.name.Length - 1));
            seatUIInfo = info;
            Trans.localPosition = info.Pos;
            imageBanker.transform.localPosition = info.BankerPos;
            transSmallCardBacks.localPosition = info.CardBackPos;
            transSmallCardBacks.localRotation = Quaternion.Euler(info.CardBackRot);
            transCards.localPosition = info.CardsPos;
            transCurRoundHaveBet.transform.localPosition = info.CurRoundHaveBetPos;
            if (info.BankerPos.x < 0)
            {
                imageCurRoundHaveBetFrame.rectTransform.pivot = new Vector2(0, 0.5f);
                textCurRoundHaveBet.alignment = TextAnchor.MiddleRight;
            }
            else
            {
                imageCurRoundHaveBetFrame.rectTransform.pivot = new Vector2(1, 0.5f);
                textCurRoundHaveBet.alignment = TextAnchor.MiddleLeft;
            }

            if (Trans.localPosition.x > 0)
            {
                armatureVoice.transform.localPosition = new Vector3(-90, 50, 0);
            }
            else
            {
                armatureVoice.transform.localPosition = new Vector3(90, 50, 0);
            }

            // imageBubble.transform.localPosition = ClientSeatId == 0 ? new Vector3(-190, 47) : new Vector3(0, 100);
            if (ClientSeatId == 0)
            {
                imageBubble.transform.SetParent(transMyCardType);
                imageBubbleInsurance.transform.SetParent(transMyCardType);
                imageBubbleInsuranceCountDown.transform.SetParent(transMyCardType);

                imageBubble.rectTransform.sizeDelta = new Vector2(145f, 57f);
            }
            else
            {
                RectTransform mRectTransform = imageBubble.rectTransform;
                mRectTransform.SetParent(transBubble);
                mRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                mRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                mRectTransform.localPosition = info.BubblePos;
                mRectTransform.sizeDelta = new Vector2(145f, 57f);

                mRectTransform = imageBubbleInsurance.rectTransform;
                mRectTransform.SetParent(transBubble);
                mRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                mRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                mRectTransform.localPosition = info.BubblePos;

                mRectTransform = imageBubbleInsuranceCountDown.rectTransform;
                mRectTransform.SetParent(transBubble);
                mRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                mRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                mRectTransform.localPosition = info.BubblePos;
            }
        }

        /// <summary>
        /// 设置0号位观众气泡
        /// </summary>
        public virtual void SetClient0BubblePos()
        {
            if (ClientSeatId != 0)
                return;

            if (null != Player && Player.userID == GameCache.Instance.CurGame.MainPlayer.userID)
            {
                return;
            }

            // RectTransform mRectTransform = imageBubble.rectTransform;
            // mRectTransform.SetParent(transBubble);
            // mRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            // mRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            // mRectTransform.localPosition = seatUIInfo.BubblePos;

            RectTransform mRectTransform = imageBubble.rectTransform;
            mRectTransform.SetParent(transMyCardType);
            mRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            mRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            mRectTransform.localPosition = Vector3.zero;

            transCurRoundHaveBet.localPosition = seatUIInfo.CurRoundHaveBetPos;
        }

        protected virtual Vector3 GetBackSmallCardPos(int index)
        {
            return backSmallCardPos[index];
        }

        protected virtual Vector3 GetBackSmallCardRot(int index)
        {
            return backSmallCardRot[index];
        }

        #region UI事件

        protected virtual void onClickCard(GameObject go)
        {
            var mTmpSequencePlayDealAnimation = GameCache.Instance.CurGame.GetSequencePlayDealAnimation();
            if (null != mTmpSequencePlayDealAnimation && mTmpSequencePlayDealAnimation.IsPlaying())
            {
                return;
            }

            // 亮牌
            if (null == Player || Player.userID != GameCache.Instance.CurGame.MainPlayer.userID ||
                seatID != GameCache.Instance.CurGame.MainPlayer.seatID || Player.canPlayStatus == 0)
            {
                return;
            }

            string mTmp = go.name.Substring(go.name.Length - 1);
            int mCardIndex = -1;
            if (int.TryParse(mTmp, out mCardIndex))
            {
                bool mActive = listCardUIInfos[mCardIndex].imageEye.gameObject.activeInHierarchy;
                listCardUIInfos[mCardIndex].imageEye.gameObject.SetActive(!mActive);

                sbyte pokernum = 0;
                if (!listCardUIInfos[0].imageEye.gameObject.activeInHierarchy && !listCardUIInfos[1].imageEye.gameObject.activeInHierarchy)
                {
                    pokernum = 0;
                }
                else if (listCardUIInfos[0].imageEye.gameObject.activeInHierarchy && !listCardUIInfos[1].imageEye.gameObject.activeInHierarchy)
                {
                    pokernum = 1;
                }
                else if (!listCardUIInfos[0].imageEye.gameObject.activeInHierarchy && listCardUIInfos[1].imageEye.gameObject.activeInHierarchy)
                {
                    pokernum = 2;
                }
                else if (listCardUIInfos[0].imageEye.gameObject.activeInHierarchy && listCardUIInfos[1].imageEye.gameObject.activeInHierarchy)
                {
                    pokernum = 3;
                }

                CPGameSessionComponent.Instance.HotfixSession.Send(new REQ_SHOWDOWN()
                {
                    roomID = GameCache.Instance.room_id,
                    roomPath = GameCache.Instance.room_path,
                    pokernum = pokernum
                });
            }
        }

        protected void onClickBackDesk(GameObject go)
        {
            if (ClientSeatId != 0)
                return;

            GameCache.Instance.CurGame.BackDesk(seatID);
        }

        protected void onClickEmpty(GameObject go)
        {
            GameCache.Instance.CurGame.Sitdown(ClientSeatId);
        }

        protected void onClickHead(GameObject go)
        {
            // 查看个人信息
            GameCache.Instance.CurGame.CheckPlayerInfo(Player.userID);
        }

        #endregion


        public bool IsMySeat
        {
            get
            {
                if (null == Player)
                    return false;
                return Player.userID == GameCache.Instance.CurGame.MainPlayer.userID && seatID == GameCache.Instance.CurGame.MainPlayer.seatID;
            }
        }

        /// <summary>
        /// 操作延时
        /// </summary>
        public virtual void AddOperationTime(int addTime)
        {
            optCurTime += addTime;
            optTotalTime = optCurTime;
        }

        #region 刷新UI

        /// <summary>
        /// 刷新头像
        /// </summary>
        protected virtual void UpdateHead()
        {
            if (null == Player)
            {
                imageEmpty.gameObject.SetActive(true);
                imageHeadFrame.gameObject.SetActive(false);
            }
            else
            {
                WebImageHelper.SetHeadImage(rawimageHead, Player.headPic);
            }
        }

        /// <summary>
        /// 刷新昵称
        /// </summary>
		public virtual void UpdateNickname()
        {
            SetNickname(null == Player ? string.Empty : CacheDataManager.mInstance.GetRemarkName(Player.userID, Player.nick));
            // textNickname.text = null == Player ? string.Empty : CacheDataManager.mInstance.GetRemarkName(Player.userID, Player.nick);
            if (null != Player)
            {
                textNickname.color = GetRemarkColor32(CacheDataManager.mInstance.GetRemarkNameColor(Player.userID));
            }
        }

        public virtual void SetNickname(string name)
        {
            textNickname.text = name;
            float mTmpWidth = 0;
            if (textNickname.preferredWidth > 0 && textNickname.preferredWidth < textNickname.rectTransform.sizeDelta.x)
                mTmpWidth = textNickname.preferredWidth + 36;
            else if (textNickname.preferredWidth >= textNickname.rectTransform.sizeDelta.x)
                mTmpWidth = textNickname.rectTransform.sizeDelta.x;
            imageNicknameShadow.rectTransform.sizeDelta = new Vector2(mTmpWidth, textNickname.fontSize + 24);
        }

        Color32 GetRemarkColor32(int pIndex)
        {
            if (pIndex == 0)
                return new Color32(185, 45, 49, 255);
            else if (pIndex == 1)
                return new Color32(68, 199, 173, 255); 
            else if (pIndex == 2)
                return new Color32(227, 182, 114, 255);
            return new Color32(255, 255, 255, 255);
        }

        /// <summary>
        /// 刷新筹码
        /// </summary>
		public virtual void UpdateCoin()
        {
            // 占座状态(18)不显示，其余显示筹码
            if (null == Player)
            {
                SetCoin(string.Empty);
                // textCoin.text = string.Empty;
            }
            else
            {
                if (Player.status == 18)
                {
                    SetCoin(string.Empty);
                    // textCoin.text = string.Empty;
                }
                else
                {
                    if (Player.chips < 0)
                    {
                        SetCoin(string.Empty);
                    }
                    else
                    {
                        SetCoin(StringHelper.GetShortString(Player.chips));
                    }

                    // textCoin.text = StringHelper.GetShortString(Player.chips);
                }
            }
        }

        public virtual void SetCoin(string coin)
        {
            textCoin.text = coin;
            float mTmpWidth = 0;
            if (textCoin.preferredWidth > 0 && textCoin.preferredWidth < textCoin.rectTransform.sizeDelta.x)
                mTmpWidth = textCoin.preferredWidth + 36;
            else if (textCoin.preferredWidth >= textCoin.rectTransform.sizeDelta.x)
                mTmpWidth = textCoin.rectTransform.sizeDelta.x;
            imageCoinShadow.rectTransform.sizeDelta = new Vector2(mTmpWidth, textCoin.fontSize + 24);
        }

        /// <summary>
        /// 刷新扑克牌样式
        /// </summary>
        public void UpdateCardSettingType()
        {
            if (null != Player)
            {
                for (int i = 0, n = Player.cards.Count; i < n; i++)
                {
                    listCardUIInfos[i].imageCard.sprite = GameCache.Instance.CurGame.GetPokerSpriteBySpriteName(GameUtil.GetCardNameByNum(Player.cards[i]));
                }
            }
        }

        /// <summary>
        /// 刷新手牌
        /// </summary>
        public virtual void UpdateCards()
        {
            if (IsMySeat)
            {
                HideCards(listSmallCardUIInfos);
                HideCardBack();
                if (Player?.cards != null)
                {
                    bool hadCard = false;
                    if (Player.cards.Count > 0 && Player.cards[0] != -1)
                    {
                        //有牌必定显示
                        hadCard = true;
                    }
                    if (hadCard || Player.isPlaying)
                    {
                        for (int i = 0, n = listCardUIInfos.Count; i < n; i++)
                        {
                            listCardUIInfos[i].imageCard.color = Player.isFold ? Color.gray : Color.white;
                        }
                        ShowCards(listCardUIInfos);
                    }
                    else
                    {
                        HideCards(listCardUIInfos);
                    }
                }
                else
                {
                    HideCards(listCardUIInfos);
                }
            }
            else
            {
                HideCards(listCardUIInfos);
                if (Player?.cards != null)
                {
                    bool mShow = false;
                    for (int i = 0, n = Player.cards.Count; i < n; i++)
                    {
                        if (Player.cards[i] != -1)
                        {
                            mShow = true;
                            break;
                        }
                    }

                    if (Player.cards.Count > 0 && mShow)
                    {
                        ShowCards(listSmallCardUIInfos);
                        HideCardBack();
                    }
                    else
                    {
                        HideCards(listSmallCardUIInfos);
                        if (Player.isPlaying)
                        {
                            ShowCardBack();
                        }
                        else
                        {
                            HideCardBack();
                        }
                    }
                }
                else
                {
                    HideCards(listSmallCardUIInfos);
                    HideCardBack();
                }
            }
        }

        /// <summary>
        /// 可见手牌数量
        /// </summary>
        public virtual int CardsCount()
        {
            int iCount = 0;
            for (int i = 0, n = Player.cards.Count; i < n; i++)
            {
                if (Player.cards[i] != -1)
                {
                    iCount++;
                }
            }
            return iCount;
        }

        /// <summary>
        /// 显示手牌
        /// </summary>
        protected virtual void ShowCards(List<CardUIInfo> list)
        {
            int mUpdateStart = 0;
            int mUpdateEnd = 0;
            int mHideStart = 0;
            int mHideEnd = 0;

            if (Player?.cards != null)
            {
                if (Player.cards.Count > list.Count)
                {
                    mUpdateStart = 0;
                    mUpdateEnd = list.Count;
                }
                else if (Player.cards.Count < list.Count)
                {
                    mUpdateStart = 0;
                    mUpdateEnd = Player.cards.Count;

                    mHideStart = mUpdateEnd + 1;
                    mHideEnd = list.Count;
                }
                else
                {
                    mUpdateStart = 0;
                    mUpdateEnd = Player.cards.Count;
                }
            }

            for (int i = mUpdateStart; i < mUpdateEnd; i++)
            {
                if (IsMySeat)
                {
                    list[i].imageCard.transform.localPosition = myCardsPos[i];
                    list[i].imageCard.transform.localRotation = Quaternion.Euler(myCardsRot[i]);
                }
                else
                {
                    list[i].imageCard.transform.localPosition = smallCardPos[i];
                }
                sbyte mCard = Player.cards[i];
                list[i].imageCard.sprite = GameCache.Instance.CurGame.GetPokerSpriteBySpriteName(GameUtil.GetCardNameByNum(mCard));
                list[i].imageCard.gameObject.SetActive(true);
            }

            for (int i = mHideStart; i < mHideEnd; i++)
            {
                list[i].imageCard.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 隐藏手牌
        /// </summary>
		protected virtual void HideCards(List<CardUIInfo> list)
        {
            for (int i = 0, n = list.Count; i < n; i++)
            {
                list[i].imageCard.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 显示手牌背面
        /// </summary>
        protected virtual void ShowCardBack()
        {
            for (int i = 0, n = listImageSmallCardBack.Count; i < n; i++)
            {
                listImageSmallCardBack[i].gameObject.SetActive(true);
                listImageSmallCardBack[i].transform.localPosition = GetBackSmallCardPos(i);
                listImageSmallCardBack[i].transform.localRotation = Quaternion.Euler(GetBackSmallCardRot(i));
            }

            transSmallCardBacks.localPosition = seatUIInfo.CardBackPos;
            transSmallCardBacks.gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏手牌背面
        /// </summary>
        protected virtual void HideCardBack()
        {
            transSmallCardBacks.gameObject.SetActive(false);
        }

        /// <summary>
        /// 刷新庄家标识
        /// </summary>
		protected virtual void UpdateBanker()
        {
            imageBanker.gameObject.SetActive(isBank);
        }

        /// <summary>
        /// 刷新赢的筹码数量
        /// </summary>
        /// <param name="winChip"></param>
        public virtual void UpdateWinCoin()
        {
            if (null != Player && Player.winChips >= 0)
            {
                RectTransform mRectTransform = imageWinner.rectTransform;
                if (null != mRectTransform && null != textWinner)
                {

                    textWinner.text = $"+{StringHelper.ShowGold(Player.winChips, false)}";
                    mRectTransform.sizeDelta = new Vector2(textWinner.preferredWidth + 20, mRectTransform.sizeDelta.y);
                    imageWinner.gameObject.SetActive(true);
                }
            }
        }

        /// <summary>
        /// 刷新手牌牌型高亮
        /// </summary>
        /// <param name="type"></param>
        /// <param name="hightCards"></param>
        public virtual void UpdateCardType(CardType type, List<sbyte> hightCards)
        {
            if (Player.canPlayStatus == 0 || GameCache.Instance.CurGame.GetCurPublicCardsCount() == 0 || CardsCount() == 0)
            {
                imageCardType.gameObject.SetActive(false);
                imageSmallCardType.gameObject.SetActive(false);
                for (int i = 0, n = Player.cards.Count; i < n; i++)
                {
                    listCardUIInfos[i].imageSelect.gameObject.SetActive(false);
                }
                return;
            }

            if (IsMySeat)
            {
                imageSmallCardType.gameObject.SetActive(false);
                textCardType.text = CardTypeUtil.GetCardTypeName(type);
                (imageCardType.transform as RectTransform).sizeDelta = new Vector2(textCardType.preferredWidth + 26, 57);
                // if (type == CardType.RoyalFlush)
                // {
                //     imageCardType.sprite = rc.Get<Sprite>("match_paixing_tishi_02");
                // }
                // else
                // {
                //     imageCardType.sprite = rc.Get<Sprite>("match_paixing_tishi_01");
                // }
                imageCardType.sprite = rc.Get<Sprite>("match_paixing_tishi_01");
                // imageCardType.SetNativeSize();
                imageCardType.gameObject.SetActive(true);

                for (int i = 0, n = Player.cards.Count; i < n; i++)
                {
                    listCardUIInfos[i].imageSelect.gameObject.SetActive(false);
                    for (int j = 0, m = hightCards.Count; j < m; j++)
                    {
                        if (Player.cards[i] == hightCards[j])
                        {
                            listCardUIInfos[i].imageSelect.gameObject.SetActive(true);
                            break;
                        }
                    }
                }
            }
            else
            {
                imageCardType.gameObject.SetActive(false);
                textSmallCardType.text = CardTypeUtil.GetCardTypeName(type);
                if (type == CardType.RoyalFlush)
                {
                    imageCardType.sprite = rc.Get<Sprite>("match_paixing_tishi_04");
                }
                else
                {
                    imageCardType.sprite = rc.Get<Sprite>("match_paixing_tishi_03");
                }
                // imageSmallCardType.SetNativeSize();
                imageSmallCardType.gameObject.SetActive(true);

                for (int i = 0, n = Player.cards.Count; i < n; i++)
                {
                    listSmallCardUIInfos[i].imageSelect.gameObject.SetActive(false);
                    for (int j = 0, m = hightCards.Count; j < m; j++)
                    {
                        if (Player.cards[i] == hightCards[j])
                        {
                            listSmallCardUIInfos[i].imageSelect.gameObject.SetActive(true);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 隐藏手牌牌型高亮
        /// </summary>
        public virtual void HideCardType()
        {
            if (null == Player || null == Player.cards)
                return;

            for (int i = 0, n = Player.cards.Count; i < n; i++)
            {
                listCardUIInfos[i].imageSelect.gameObject.SetActive(false);
            }
            imageSmallCardType.gameObject.SetActive(false);
            imageCardType.gameObject.SetActive(false);
        }

        /// <summary>
        /// 刷新托管
        /// </summary>
        public virtual void UpdateTrust()
        {
            imageTrust.gameObject.SetActive(Player.trust == 1);
        }

        /// <summary>
        /// 刷新占座
        /// </summary>
        public virtual void UpdateHolding()
        {
            if (null == Player)
            {
                imageHolding.gameObject.SetActive(false);
            }
            else
            {
                imageHolding.gameObject.SetActive(Player.status == 18);
            }
        }

        /// <summary>
        /// 刷新本手下注筹码
        /// </summary>
        public virtual void UpdateCurRoundHaveBet()
        {
            float mOffset = 5f;
            // 0不显示
            //if (null == Player || Player.anteNumber <= 0 || (Player.status >= 8 && Player.status != 10))
            if (null == Player || Player.anteNumber <= 0 || Player.status <= 0 || (Player.status >= 8 && Player.status != 10))
            {
                transCurRoundHaveBet.gameObject.SetActive(false);
                return;
            }

            // if (ClientSeatId == 0 && null != Player)
            // {
            //     SetNickname(string.Empty);
            // }

            string mChipSpriteName = GameUtil.GetChipSpriteName(Player.anteNumber);
            imageIconChip.sprite = GameCache.Instance.CurGame.GetChipSpriteBySpriteName(mChipSpriteName);
            textCurRoundHaveBet.text = StringHelper.GetShortString(Player.anteNumber);

            RectTransform mRectTransform = imageCurRoundHaveBetFrame.rectTransform;
            // mRectTransform.sizeDelta = new Vector2(textCurRoundHaveBet.preferredWidth + imageIconChip.rectTransform.sizeDelta.x + 10f, mRectTransform.sizeDelta.y);
            mRectTransform.sizeDelta = new Vector2(textCurRoundHaveBet.preferredWidth + imageIconChip.rectTransform.sizeDelta.x, mRectTransform.sizeDelta.y);

            Vector3 mTmpV3 = Trans.localPosition;
            if (mTmpV3.x <= 0)
            {
                textCurRoundHaveBet.alignment = TextAnchor.MiddleRight;
            }
            else
            {
                textCurRoundHaveBet.alignment = TextAnchor.MiddleLeft;
            }
            if (mTmpV3.y > GameUtil.SeatPosV3[0].y && mTmpV3.y < GameUtil.SeatPosV3[7].y)
            {
                if (mTmpV3.x < 0)
                {
                    // 左
                    mRectTransform.pivot = new Vector2(0, 0.5f);
                    mRectTransform.localPosition = new Vector3(-mOffset, 0);
                }
                else if (mTmpV3.x > 0)
                {
                    // 右
                    mRectTransform.pivot = new Vector2(1f, 0.5f);
                    mRectTransform.localPosition = new Vector3(mOffset, 0);
                }

                imageIconChip.rectTransform.localPosition = Vector3.zero;
            }
            else
            {
                mRectTransform.pivot = new Vector2(0, 0.5f);
                mRectTransform.localPosition = new Vector3(-mRectTransform.sizeDelta.x / 2f - mOffset, mRectTransform.localPosition.y);
                imageIconChip.rectTransform.localPosition = new Vector3(-mRectTransform.sizeDelta.x / 2f, 0);
            }

            defaultIconChipLocalPos = imageIconChip.transform.localPosition;
            imageIconChip.gameObject.SetActive(true);
            transCurRoundHaveBet.gameObject.SetActive(true);
        }

        /// <summary>
        /// 刷新前注
        /// </summary>
        public virtual void UpdateGroupBet()
        {
            float mOffset = 10f;
            // if (ClientSeatId == 0 && null != Player)
            // {
            //     SetNickname(string.Empty);
            // }

            string mChipSpriteName = GameUtil.GetChipSpriteName(GameCache.Instance.CurGame.groupBet);
            imageIconChip.sprite = GameCache.Instance.CurGame.GetChipSpriteBySpriteName(mChipSpriteName);
            textCurRoundHaveBet.text = StringHelper.GetShortString(GameCache.Instance.CurGame.groupBet);
            RectTransform mRectTransform = imageCurRoundHaveBetFrame.transform as RectTransform;
            mRectTransform.sizeDelta = new Vector2(textCurRoundHaveBet.preferredWidth + imageIconChip.rectTransform.sizeDelta.x, mRectTransform.sizeDelta.y);

            Vector3 mTmpV3 = Trans.localPosition;
            if (mTmpV3.x <= 0)
            {
                textCurRoundHaveBet.alignment = TextAnchor.MiddleRight;
            }
            else
            {
                textCurRoundHaveBet.alignment = TextAnchor.MiddleLeft;
            }
            if (mTmpV3.y > GameUtil.SeatPosV3[0].y && mTmpV3.y < GameUtil.SeatPosV3[7].y)
            {
                if (mTmpV3.x < 0)
                {
                    // 左
                    mRectTransform.pivot = new Vector2(0, 0.5f);
                    mRectTransform.localPosition = new Vector3(-mOffset, 0);
                }
                else if (mTmpV3.x > 0)
                {
                    // 右
                    mRectTransform.pivot = new Vector2(1f, 0.5f);
                    mRectTransform.localPosition = new Vector3(mOffset, 0);
                }
                imageIconChip.rectTransform.localPosition = Vector3.zero;
            }
            else
            {
                mRectTransform.pivot = new Vector2(0, 0.5f);
                mRectTransform.localPosition = new Vector3(-mRectTransform.sizeDelta.x / 2f - mOffset, mRectTransform.localPosition.y);
                imageIconChip.rectTransform.localPosition = new Vector3(-mRectTransform.sizeDelta.x / 2f, 0);
            }

            defaultIconChipLocalPos = imageIconChip.transform.localPosition;
            imageIconChip.gameObject.SetActive(true);
            transCurRoundHaveBet.gameObject.SetActive(true);
        }

        /// <summary>
        /// 清空本手下注筹码
        /// </summary>
        public virtual void ClearCurRoundHaveBet()
        {
            textCurRoundHaveBet.text = string.Empty;
            transCurRoundHaveBet.gameObject.SetActive(false);
        }

        /// <summary>
        /// 显示过庄气泡
        /// </summary>
        public virtual void ShowGuozhuangBuuble()
        {
            // SetNickname(string.Empty);

            imageBubble.sprite = rc.Get<Sprite>("match_icon_kanpai");
            //textBubble.text = "过庄";
            textBubble.text = CPErrorCode.LanguageDescription(10043);

            PlayUpdateBubbleAnimation();
        }

        /// <summary>
        /// 刷新气泡
        /// </summary>
        public virtual void UpdateBubble()
        {
            // 1.出现筹码时隐藏昵称
            // 2.操作提示与牌型提示，只出现一个则与头像居中对齐，出现两个则以居中对齐的线对称上下摆放

            // 1:下注  2:跟注  3:加注  4:全下 5:让牌  6:弃牌 10:straddle--客户端
            if (null == Player)
            {
                if (imageBubble.gameObject.activeInHierarchy)
                    imageBubble.gameObject.SetActive(false);
                return;
            }

            // SetNickname(string.Empty); // 气泡时，不显示昵称，避免重叠

            switch (Player.status)
            {
                case 1:
                    imageBubble.sprite = null;
                    textBubble.text = string.Empty;
                    StopAllinArmature();
                    //imageBubble.sprite = rc.Get<Sprite>("match_icon_jiazhu");
                    //textBubble.text = CPErrorCode.LanguageDescription(11044);
                    //StopAllinArmature();
                    break;
                case 2:
                    imageBubble.sprite = rc.Get<Sprite>("match_icon_genzhu");
                    // textBubble.text = "跟注";
                    textBubble.text = CPErrorCode.LanguageDescription(10044);
                    StopAllinArmature();
                    break;
                case 3:
                    imageBubble.sprite = rc.Get<Sprite>("match_icon_jiazhu");
                    // textBubble.text = "加注";
                    textBubble.text = CPErrorCode.LanguageDescription(10045);
                    StopAllinArmature();
                    break;
                case 4:
                    imageBubble.sprite = rc.Get<Sprite>("match_icon_allin");
                    textBubble.text = "ALL IN";
                    PlayAllinArmature();
                    break;
                case 5:
                    imageBubble.sprite = rc.Get<Sprite>("match_icon_kanpai");
                    // textBubble.text = "看牌";
                    textBubble.text = CPErrorCode.LanguageDescription(10046);
                    StopAllinArmature();
                    break;
                case 6:
                    imageBubble.sprite = rc.Get<Sprite>("match_icon_qipai");
                    // textBubble.text = "弃牌";
                    textBubble.text = CPErrorCode.LanguageDescription(10047);
                    StopAllinArmature();
                    break;
                case 10:
                    imageBubble.sprite = rc.Get<Sprite>("match_icon_straddle");
                    textBubble.text = "Straddle";
                    StopAllinArmature();
                    break;
                case 11:
                    imageBubble.sprite = rc.Get<Sprite>("match_icon_genzhu");
                    // textBubble.text = "盖牌";
                    textBubble.text = CPErrorCode.LanguageDescription(10048);
                    StopAllinArmature();
                    break;
                default:
                    imageBubble.sprite = null;
                    textBubble.text = string.Empty;
                    StopAllinArmature();
                    break;
            }


            if (null == imageBubble.sprite)
            {
                imageBubble.color = Color.white;
                imageBubble.transform.localScale = new Vector3(1, 1, 1);
                imageBubble.gameObject.SetActive(false);
                sequenceUpdateBubble = null;

                // 第一轮大小盲阶段，没有任何跟注/加注等操作，断线重连回来，显示大小盲筹码的时候不用显示昵称
                // if (Player.status != 1) 
                //     UpdateNickname();
                //if (Player.status != -1)
                if (Player.status != 1)
                {
                    UpdateNickname();
                }
                else
                {
                    if (ClientSeatId != 0)
                    {
                        UpdateNickname();
                    }
                    else
                    {
                        if (Player.chips == 0)
                        {
                            UpdateNickname();
                        }
                    }
                }
            }
            else
            {
                // imageBubble.SetNativeSize();
                if (!string.IsNullOrEmpty(textBubble.text))
                {
                    (imageBubble.transform as RectTransform).sizeDelta = new Vector2(textBubble.preferredWidth + 26, 57);
                    if (null == sequenceUpdateBubble || !sequenceUpdateBubble.IsPlaying())
                        PlayUpdateBubbleAnimation();
                }
                else
                {
                    if (null != sequenceUpdateBubble && sequenceUpdateBubble.IsPlaying())
                        sequenceUpdateBubble.Kill(true);

                    imageBubble.color = Color.white;
                    imageBubble.transform.localScale = new Vector3(1, 1, 1);
                    imageBubble.gameObject.SetActive(false);
                    sequenceUpdateBubble = null;
                }
            }
        }

        public virtual void FoldHeadGray(bool active)
        {
            imageHeadGray.gameObject.SetActive(active);

            if (IsMySeat)
            {
                CardUIInfo mCardUiInfo = null;
                for (int i = 0, n = listCardUIInfos.Count; i < n; i++)
                {
                    mCardUiInfo = listCardUIInfos[i];
                    if (null == mCardUiInfo)
                        continue;

                    mCardUiInfo.imageSelect.gameObject.SetActive(false);

                    mCardUiInfo.imageCard.color = active ? Color.gray : Color.white;
                }
            }
        }

        /// <summary>
        /// 隐藏气泡
        /// </summary>
        public virtual void HideBubble()
        {
            if (imageBubble.gameObject.activeInHierarchy)
            {
                if (null == sequenceUpdateBubble || !sequenceUpdateBubble.IsPlaying())
                {
                    imageBubble.color = Color.white;
                    imageBubble.transform.localScale = Vector3.one;
                }
            }
            tweenerHideBubble = imageBubble.transform.DOScale(new Vector3(0, 0, 1), 0.2f).SetDelay(1f).OnComplete(() =>
            {
                imageBubble.gameObject.SetActive(false);
                UpdateNickname();
            });

            HideBubbleInsurance();
            HideBubbleInsuranceCountDown();
        }

        /// <summary>
        /// 显示占座离桌
        /// </summary>
        protected virtual void ShowBubbleBackDesk()
        {
            // textBubbleBackDesk.text = $"留座{keepSeatLeftTime}秒";
            textBubbleBackDesk.text = CPErrorCode.LanguageDescription(20061, new List<object>() { keepSeatLeftTime });
            imageBubbleBackDesk.gameObject.SetActive(true);
            if (null != Player && GameCache.Instance.CurGame.MainPlayer.userID == Player.userID)
            {
                imageBackDesk.gameObject.SetActive(true);
            }
            bKeepSeatCounting = true;
        }

        /// <summary>
        /// 隐藏占座离桌
        /// </summary>
        protected virtual void HideBubbleBackDesk()
        {
            bKeepSeatCounting = false;
            imageBubbleBackDesk.gameObject.SetActive(false);
            imageBackDesk.gameObject.SetActive(false);
        }

        /// <summary>
        /// 显示等待审核
        /// </summary>
        protected virtual void ShowBubbleWaitDairu()
        {

            textBubbleDauru.text = CPErrorCode.LanguageDescription(20178, new List<object>() { Player.timeLeft_dairu < 0 ? 0 : Player.timeLeft_dairu });
            Image_BubbleWaitDairu.gameObject.SetActive(true);
            bKeepSeatCounting = true;
        }

        /// <summary>
        /// 隐藏等待审核
        /// </summary>
        public virtual void HideBubbleWaitDairu()
        {
            bKeepSeatCounting = false;
            Image_BubbleWaitDairu.gameObject.SetActive(false);
        }

        /// <summary>
        /// 显示保险冒泡
        /// </summary>
        protected virtual void ShowBubbleInsuranceCountDown()
        {
            if (null == Player || GameCache.Instance.CurGame.MainPlayer.userID == Player.userID)
            {
                return;
            }
            // textBubbleInsuranceCountDown.text = $"购买剩余{Player.timeLeft_insurance}秒";
            textBubbleInsuranceCountDown.text = CPErrorCode.LanguageDescription(20062, new List<object>() { Player.timeLeft_insurance < 0 ? 0 : Player.timeLeft_insurance });
            imageBubbleInsuranceCountDown.gameObject.SetActive(true);

            HideBubbleInsurance();
        }

        /// <summary>
        /// 隐藏保险冒泡
        /// </summary>
        public virtual void HideBubbleInsuranceCountDown()
        {
            if (null != textBubbleInsuranceCountDown)
                imageBubbleInsuranceCountDown.gameObject.SetActive(false);
        }

        public virtual void HideBubbleInsurance()
        {
            if (imageBubbleInsurance.gameObject.activeInHierarchy)
            {
                imageBubbleInsurance.gameObject.SetActive(false);
            }
        }


        /// <summary>
        /// 刷新购买保险数量
        /// </summary>
        public virtual void UpdateBubbleInsurance()
        {
            isCountDown = false;
            imageCountDown.gameObject.SetActive(false);
            StopLightArmature();

            imageBubble.sprite = rc.Get<Sprite>("match_icon_jiazhu");
            if (Player.totalInsuredAmount + Player.autoInsuredAmount == 0)
            {
                // textBubble.text = "不保";
                textBubble.text = CPErrorCode.LanguageDescription(10049);

            }
            else
            {
                if (Player.autoInsuredAmount > 0)
                {
                    // textBubbleInsurance.text = $"投保{Player.totalInsuredAmount}+背保{Player.autoInsuredAmount}";
                    textBubble.text = CPErrorCode.LanguageDescription(20058, new List<object>() { StringHelper.ShowGold(Player.totalInsuredAmount), StringHelper.ShowGold(Player.autoInsuredAmount) });
                }
                else
                {
                    // imageBubble.SetNativeSize();
                    // textBubble.text = $"投保{Player.totalInsuredAmount}";
                    textBubble.text = CPErrorCode.LanguageDescription(20059, new List<object>() { StringHelper.ShowGold(Player.totalInsuredAmount)});
                }
            }
            (imageBubble.transform as RectTransform).sizeDelta = new Vector2(textBubble.preferredWidth + 26, 57);
            PlayUpdateBubbleAnimation();
        }

        /// <summary>
        /// 刷新保险赔付气泡
        /// </summary>
        public virtual void UpdateBubbleInsuranceClaim()
        {
            if (Player.claimInsuredAmount > 0)
            {
                imageBubble.sprite = rc.Get<Sprite>("match_icon_jiazhu");
                // imageBubble.SetNativeSize();
                // textBubble.text = $"赔付{Player.claimInsuredAmount}";
                textBubble.text = CPErrorCode.LanguageDescription(20060, new List<object>() { StringHelper.ShowGold(Player.claimInsuredAmount)});

                PlayUpdateBubbleAnimation();
            }
        }

        /// <summary>
        /// 刷新回收赢的筹码
        /// </summary>
        public virtual void UpdateRecyclingWinChip()
        {
            imageRecyclingWinChip.sprite = GameCache.Instance.CurGame.GetChipSpriteBySpriteName(GameUtil.GetChipSpriteName(Player.recyclingChip));
        }

        /// <summary>
        /// 刷新亮牌眼睛
        /// </summary>
        public virtual void UpdateShowCardsId()
        {
            if (null == Player || null == GameCache.Instance.CurGame.MainPlayer || GameCache.Instance.CurGame.MainPlayer.userID != Player.userID ||
                GameCache.Instance.CurGame.MainPlayer.seatID != seatID)
            {
                return;
            }

            switch (GameCache.Instance.CurGame.showCardsId)
            {
                case 0:
                    {
                        for (int i = 0, n = listCardUIInfos.Count; i < n; i++)
                        {
                            listCardUIInfos[i].imageEye.gameObject.SetActive(false);
                        }

                        break;
                    }
                case 1:
                    listCardUIInfos[0].imageEye.gameObject.SetActive(true);
                    listCardUIInfos[1].imageEye.gameObject.SetActive(false);
                    break;
                case 2:
                    listCardUIInfos[0].imageEye.gameObject.SetActive(false);
                    listCardUIInfos[1].imageEye.gameObject.SetActive(true);
                    break;
                case 3:
                    {
                        for (int i = 0, n = listCardUIInfos.Count; i < n; i++)
                        {
                            listCardUIInfos[i].imageEye.gameObject.SetActive(true);
                        }

                        break;
                    }
            }
        }

        public virtual void PlayRecord(long duration)
        {
            string animationName;
            if (Trans.localPosition.x > 0)
            {
                animationName = "right_play_voice";
            }
            else
            {
                animationName = "left_play_voice";
            }
            armatureVoice.gameObject.SetActive(true);
            if (null != armatureVoice.dragonAnimation)
            {
                armatureVoice.dragonAnimation.Reset();
                armatureVoice.dragonAnimation.Play(animationName);
            }
        }

        public virtual void StopRecord()
        {

            if (null != armatureVoice.dragonAnimation && armatureVoice.dragonAnimation.isPlaying)
                armatureVoice.dragonAnimation.Stop();
            armatureVoice.gameObject.SetActive(false);
        }

        public virtual async void ShowEmoji(string name)
        {
            emojiComponent.PlayEmoji(name);
        }

        #endregion

        /// <summary>
        /// 刷新状态机，主要用户刷新冒泡
        /// </summary>
        public virtual void UpdateFSMbyStatus()
        {
            if (null == Player)
            {
                FsmLogicComponent.SM.ChangeState(SeatEmpty<Entity>.Instance);
                return;
            }

            FsmLogicComponent.SM.ChangeState(SeatSit<Entity>.Instance);
            UpdateBubble();
            switch (Player.status)
            {
                case 0:
                    // 未操作过(显示名字)
                    FsmLogicComponent.SM.ChangeState(SeatWaitStart<Entity>.Instance);
                    break;
                case 1:
                    // 下注
                    // FsmLogicComponent.SM.ChangeState(SeatPutChip<Entity>.Instance);
                    FsmLogicComponent.SM.ChangeState(SeatWaitOther<Entity>.Instance);
                    break;
                case 2:
                    // 跟注
                    // FsmLogicComponent.SM.ChangeState(SeatCall<Entity>.Instance);
                    UpdateCards();
                    FsmLogicComponent.SM.ChangeState(SeatWaitOther<Entity>.Instance);
                    break;
                case 3:
                    // 加注
                    // FsmLogicComponent.SM.ChangeState(SeatRaise<Entity>.Instance);
                    FsmLogicComponent.SM.ChangeState(SeatWaitOther<Entity>.Instance);
                    break;
                case 4:
                    // 全下
                    // FsmLogicComponent.SM.ChangeState(SeatAllin<Entity>.Instance);
                    UpdateCards();
                    FsmLogicComponent.SM.ChangeState(SeatWaitOther<Entity>.Instance);
                    break;
                case 5:
                    // 让牌
                    // FsmLogicComponent.SM.ChangeState(SeatCheck<Entity>.Instance);
                    FsmLogicComponent.SM.ChangeState(SeatWaitOther<Entity>.Instance);
                    break;
                case 6:
                    // 弃牌
                    // FsmLogicComponent.SM.ChangeState(SeatFold<Entity>.Instance);
                    Player.isFold = true;
                    FoldHeadGray(Player.isFold);
                    UpdateCards();
                    HideCardBack();
                    FsmLogicComponent.SM.ChangeState(SeatWaitOther<Entity>.Instance);
                    break;
                case 7:
                    // 超时
                    FsmLogicComponent.SM.ChangeState(SeatTimeout<Entity>.Instance);
                    break;
                case 8:
                    // 空闲等待下一局
                    FsmLogicComponent.SM.ChangeState(SeatWaitStart<Entity>.Instance);
                    break;
                case 9:
                    // 等待入座

                    break;
                case 10:
                    // straddle

                    // todo straddle状态
                    break;
                case 11:
                    // 盖牌

                    // todo 盖牌状态
                    break;
                case 15:
                    // 表示玩家为留座状态
                    FsmLogicComponent.SM.ChangeState(SeatKeep<Entity>.Instance);
                    break;
                case 18:
                    // 表示玩家为占座等待状态

                    // todo 占座等待状态
                    break;
            }
        }

        #region 倒计时

        /// <summary>
        /// 开始倒计时
        /// </summary>
        /// <param name="countDown"></param>
        public virtual void StartCountDown(float countDown, bool isInsruance = false)
        {
            optCurTime = countDown;
            optTotalTime = optCurTime;
            int defaultOpTime = GameCache.Instance.CurGame.opTime;
            if (isInsruance)
                defaultOpTime = 30;
            if (optTotalTime < defaultOpTime)
            {
                optTotalTime = defaultOpTime;
            }
            isCountDown = true;
            imageCountDown.fillAmount = 1f;
            imageCountDown.gameObject.SetActive(true);
            StopLightArmature();
        }

        /// <summary>
        /// 暂停倒计时
        /// </summary>
        public virtual void PauseCountDown()
        {
            isCountDown = false;
        }

        /// <summary>
        /// 继续倒计时
        /// </summary>
        public virtual void ContinueCountDown()
        {
            isCountDown = true;
        }

        /// <summary>
        /// 设置倒计时时间
        /// </summary>
        /// <param name="setValue"></param>
        public virtual void SetCountDown(float setValue)
        {
            optCurTime = setValue;
            optTotalTime = optCurTime;
            imageCountDown.fillAmount = 1f;
            StopLightArmature();
        }

        /// <summary>
        /// 增加倒计时时间
        /// </summary>
        /// <param name="addValue"></param>
        public virtual void AddCountDown(float addValue)
        {
            optCurTime += addValue;
            optTotalTime = optCurTime;
            isCountDown = true;
            imageCountDown.fillAmount = 1f;
            StopLightArmature();
        }

        /// <summary>
        /// 减少倒计时时间
        /// </summary>
        /// <param name="subValue"></param>
        public virtual void SubCountDown(float subValue)
        {
            optCurTime -= subValue;
            if (optCurTime <= 0)
            {
                StopCountDown();
            }
            else
            {
                optTotalTime = optCurTime;
                imageCountDown.fillAmount = 1f;
                StopLightArmature();
            }
        }

        /// <summary>
        /// 停止倒计时
        /// </summary>
        public virtual void StopCountDown()
        {
            if (isCountDown)
            {
                isCountDown = false;
                imageCountDown.gameObject.SetActive(false);
            }
            StopLightArmature();
        }

        /// <summary>
        /// 播放赢了回收筹码动画
        /// </summary>
        public bool CanPlayRecyclingWinChipAnimation
        {
            get
            {
                return true;
               // return imageIconChip.gameObject.activeInHierarchy;
            }
        }

        #endregion

        #region Tweener动画

        /// <summary>
        /// 删除所有Tweener动画
        /// </summary>
        /// <param name="complete">true马上设置为结束值</param>
        public virtual void KillAllTweener(bool complete = false)
        {
            if (null != tweenerPlayRecyclingWinChipAnimation && tweenerPlayRecyclingWinChipAnimation.IsPlaying())
            {
                tweenerPlayRecyclingWinChipAnimation.Kill(complete);
            }

            tweenerPlayRecyclingWinChipAnimation = null;

            if (null != sequencePlayRecyclingChipAnimation && sequencePlayRecyclingChipAnimation.IsPlaying())
            {
                sequencePlayRecyclingChipAnimation.Kill(complete);
            }

            sequencePlayRecyclingChipAnimation = null;

            if (null != sequencePlayDealAnimation && sequencePlayDealAnimation.IsPlaying())
            {
                sequencePlayDealAnimation.Kill(complete);
            }

            sequencePlayDealAnimation = null;

            if (null != sequenceSitAnimationEnter && sequenceSitAnimationEnter.IsPlaying())
            {
                sequenceSitAnimationEnter.Kill(complete);
            }

            sequenceSitAnimationEnter = null;

            if (null != sequenceStandupAnimationEnter && sequenceStandupAnimationEnter.IsPlaying())
            {
                sequenceStandupAnimationEnter.Kill(complete);
            }

            sequenceStandupAnimationEnter = null;

            if (null != tweenerPlayBankerAnimation && tweenerPlayBankerAnimation.IsPlaying())
            {
                tweenerPlayBankerAnimation.Kill(complete);
            }

            tweenerPlayBankerAnimation = null;

            if (null != tweenerPlayBetAnimation && tweenerPlayBetAnimation.IsPlaying())
            {
                tweenerPlayBetAnimation.Kill(complete);
            }

            tweenerPlayBetAnimation = null;

            if (null != sequencePlayFoldAnimation && sequencePlayFoldAnimation.IsPlaying())
            {
                sequencePlayFoldAnimation.Kill(complete);
            }

            sequencePlayFoldAnimation = null;

            if (null != this.sequenceUpdateBubble && sequenceUpdateBubble.IsPlaying())
            {
                sequenceUpdateBubble.Kill(complete);
            }

            sequenceUpdateBubble = null;

            if (null != tweenerHideBubble && tweenerHideBubble.IsPlaying())
            {
                tweenerHideBubble.Kill(complete);
            }

            tweenerHideBubble = null;

            if (null != tweenerUpdateBubbleInsurance && tweenerUpdateBubbleInsurance.IsPlaying())
            {
                tweenerUpdateBubbleInsurance.Kill(complete);
            }

            tweenerUpdateBubbleInsurance = null;
        }

        /// <summary>
        /// 播放庄家动画
        /// </summary>
        /// <returns></returns>
        public virtual Tweener PlayBankerAnimation()
        {
            if (GameCache.Instance.CurGame.lastBankerIndex == -1 || seatID == GameCache.Instance.CurGame.lastBankerIndex)
                return null;

            Seat mSeat = GameCache.Instance.CurGame.GetSeatById(GameCache.Instance.CurGame.lastBankerIndex);
            if (null == mSeat)
                return null;

            mSeat.imageBanker.gameObject.SetActive(false);
            imageBanker.transform.localPosition = GameUtil.ChangeToLocalPos(mSeat.seatUIInfo.BankerPos, mSeat.Trans, Trans);
            imageBanker.gameObject.SetActive(true);
            return imageBanker.transform.DOLocalMove(seatUIInfo.BankerPos, 0.3f);
        }

        /// <summary>
        /// 播放下注动画
        /// </summary>
        public virtual Tweener PlayBetAnimation()
        {
            imageIconChip.transform.localPosition = GameUtil.ChangeToLocalPos(imageEmpty.transform.localPosition, Trans.transform, transCurRoundHaveBet.transform);
            tweenerPlayBetAnimation = imageIconChip.transform.DOLocalMove(defaultIconChipLocalPos, 0.2f).OnStart(() =>
            {
                SoundComponent.Instance.PlaySFX(SoundComponent.SFX_DESK_POST_RAISE);
            });
            imageIconChip.gameObject.SetActive(true);
            return tweenerPlayBetAnimation;
        }

        /// <summary>
        /// 播放回收筹码动画
        /// </summary>
        public virtual Sequence PlayRecyclingChipAnimation(int targetType = -1)
        {
            sequencePlayRecyclingChipAnimation = DOTween.Sequence();
           // if (imageIconChip.gameObject.activeInHierarchy)
            {
                sequencePlayRecyclingChipAnimation.Append(DOTween.To(val => imageCurRoundHaveBetFrame.rectTransform.sizeDelta = new Vector2(val, imageCurRoundHaveBetFrame.rectTransform.sizeDelta.y), imageCurRoundHaveBetFrame.rectTransform.sizeDelta.x, 0, time_05));
                sequencePlayRecyclingChipAnimation.Append(imageIconChip.transform.DOLocalMove(transCurRoundHaveBet.transform.InverseTransformPoint(GameCache.Instance.CurGame.GetRecyclingChipPosV3(targetType)), time_05)
                                                                  .OnStart(() =>
                                                                  {
                                                                      SoundComponent.Instance.PlaySFX(SoundComponent.SFX_DESK_MOVE_CHIPS);
                                                                  })
                                                                  .OnComplete(() =>
                                                                  {
                                                                      imageIconChip.gameObject.SetActive(false);
                                                                  }));
            }

            return sequencePlayRecyclingChipAnimation;
        }

        /// <summary>
        /// 播放发牌动画
        /// </summary>
        public virtual Sequence PlayDealAnimation(Vector3 targetPos)
        {
            for (int i = 0, n = listCardUIInfos.Count; i < n; i++)
            {
                listCardUIInfos[i].imageSelect.gameObject.SetActive(false);
            }
            for (int i = 0, n = listSmallCardUIInfos.Count; i < n; i++)
            {
                listSmallCardUIInfos[i].imageSelect.gameObject.SetActive(false);
            }

            if (GameCache.Instance.CurGame.MainPlayer.seatID != seatID)
            {
                // 其他玩家发牌动画
                sequencePlayDealAnimation = DOTween.Sequence();
                Vector3 mLocalPos = transSmallCardBacks.InverseTransformPoint(targetPos);
                for (int i = 0, n = listImageSmallCardBack.Count; i < n; i++)
                {
                    listImageSmallCardBack[i].transform.localPosition = mLocalPos;
                    listImageSmallCardBack[i].transform.localRotation = Quaternion.Euler(0, 0, -transSmallCardBacks.localRotation.eulerAngles.z);
                    // listImageSmallCardBack[i].gameObject.SetActive(true);
                    GameObject mTmpObj = listImageSmallCardBack[i].gameObject;
                    if (i == 0)
                    {
                        sequencePlayDealAnimation.Append(listImageSmallCardBack[i].transform.DOLocalMove(GetBackSmallCardPos(i), time_05).OnStart(() =>
                        {
                            mTmpObj.gameObject.SetActive(true);

                            SoundComponent.Instance.PlaySFX(SoundComponent.SFX_DESK_NEW_CARD);
                        }));
                    }
                    else
                    {
                        sequencePlayDealAnimation.Join(listImageSmallCardBack[i].transform.DOLocalMove(GetBackSmallCardPos(i), time_05).OnStart(() =>
                        {
                            mTmpObj.gameObject.SetActive(true);
                        }));
                    }

                    Vector3 mTargetRot = GetBackSmallCardRot(i);
                    if (transSmallCardBacks.localPosition.x > 0)
                        mTargetRot.z += 360f;

                    sequencePlayDealAnimation.Join(listImageSmallCardBack[i].transform.DOLocalRotate(mTargetRot, time_05, RotateMode.FastBeyond360).SetEase(Ease.Linear));
                }

                sequencePlayDealAnimation.OnStart(() =>
                {
                    transSmallCardBacks.gameObject.SetActive(true);
                });

                // transSmallCardBacks.gameObject.SetActive(true);
                return sequencePlayDealAnimation;
            }
            else
            {
                sequencePlayDealAnimation = DOTween.Sequence();
                for (int i = 0, n = Player.cards.Count; i < n; i++)
                {
                    listCardUIInfos[i].imageCard.sprite = GameCache.Instance.CurGame.GetPokerSpriteBySpriteName(GameUtil.GetCardNameByNum(Player.cards[i]));
                    listCardUIInfos[i].imageCard.color = Color.white;
                    listCardUIInfos[i].imageBack.sprite = GameCache.Instance.CurGame.GetPokerSpriteBySpriteName(GameUtil.GetCardNameByNum(-1));
                    listCardUIInfos[i].imageBack.color = Color.white;
                    listCardUIInfos[i].imageBack.gameObject.SetActive(true);
                    listCardUIInfos[i].imageCard.rectTransform.localScale = new Vector3(0.5f, 0.5f);
                    listCardUIInfos[i].imageCard.rectTransform.localRotation = Quaternion.Euler(0, 0, 0);
                    listCardUIInfos[i].imageCard.rectTransform.localPosition = listCardUIInfos[i].imageCard.rectTransform.parent.InverseTransformPoint(targetPos);
                    listCardUIInfos[i].imageCard.gameObject.SetActive(true);
                    if (i == 0)
                    {
                        sequencePlayDealAnimation.Append(listCardUIInfos[i].imageCard.rectTransform.DOLocalMove(myCardsPos[i], 0.4f).OnStart(() =>
                        {
                            SoundComponent.Instance.PlaySFX(SoundComponent.SFX_DESK_NEW_CARD);
                        }));
                    }
                    else
                    {
                        sequencePlayDealAnimation.Join(listCardUIInfos[i].imageCard.rectTransform.DOLocalMove(myCardsPos[i], 0.4f));
                    }

                    int tmp = i;
                    sequencePlayDealAnimation.Join(listCardUIInfos[i].imageBack.DOFade(0, 0.25f).OnComplete(() =>
                    {
                        listCardUIInfos[tmp].imageBack.gameObject.SetActive(false);
                    }));
                    sequencePlayDealAnimation.Join(listCardUIInfos[i].imageCard.rectTransform.DOLocalRotate(new Vector3(0, 0, myCardsRot[i].z + 360), time_05, RotateMode.FastBeyond360).SetEase(Ease.Linear));
                    sequencePlayDealAnimation.Join(listCardUIInfos[i].imageCard.rectTransform.DOScale(Vector3.one, time_05));
                }

                sequencePlayDealAnimation.OnStart(() =>
                {

                });
                return sequencePlayDealAnimation;
            }
        }

        /// <summary>
        /// 播放赢了回收筹码动画
        /// </summary>
        /// <returns></returns>
        public virtual Tweener PlayRecyclingWinChipAnimation(Vector3 sourcePos)
        {
            imageRecyclingWinChip.transform.localPosition = imageRecyclingWinChip.transform.parent.InverseTransformPoint(sourcePos);
            tweenerPlayRecyclingWinChipAnimation = imageRecyclingWinChip.transform.DOLocalMove(GameUtil.ChangeToLocalPos(imageHeadFrame.transform.localPosition, imageHeadFrame.transform.parent, Trans), time_05).OnComplete(() =>
            {
                imageRecyclingWinChip.gameObject.SetActive(false);
            });
            tweenerPlayRecyclingWinChipAnimation.OnStart(() =>
            {
                imageRecyclingWinChip.gameObject.SetActive(true);
            });
            return tweenerPlayRecyclingWinChipAnimation;
        }

        /// <summary>
        /// 播放弃牌动画
        /// </summary>
        public virtual void PlayFoldAnimation()
        {
            sequencePlayFoldAnimation = DOTween.Sequence();
            if (!IsMySeat)
            {
                // 其他玩家弃牌
                sequencePlayFoldAnimation.Append(transSmallCardBacks.DOLocalMove(transSmallCardBacks.parent.InverseTransformPoint(GameCache.Instance.CurGame.GetRecyclingChipPosV3(-1)), time_05));
                for (int i = 0, n = listImageSmallCardBack.Count; i < n; i++)
                {
                    sequencePlayFoldAnimation.Join(listImageSmallCardBack[i].DOFade(0, 0.3f));
                }
                sequencePlayFoldAnimation.OnComplete(() =>
                {
                    transSmallCardBacks.localPosition = seatUIInfo.CardBackPos;
                    for (int i = 0, n = listImageSmallCardBack.Count; i < n; i++)
                    {
                        listImageSmallCardBack[i].color = Color.white;
                    }

                    transSmallCardBacks.gameObject.SetActive(false);
                });
            }
            else
            {
                // 自己弃牌
                // Vector3 mTargetPos = transCards.InverseTransformPoint(GameCache.Instance.CurGame.GetMyFoldPosV3());
                // for (int i = 0, n = listCardUIInfos.Count; i < n; i++)
                // {
                //     CardUIInfo mCardUIInfo = listCardUIInfos[i];
                //     mCardUIInfo.imageEye.gameObject.SetActive(false);
                //     mCardUIInfo.imageSelect.gameObject.SetActive(false);
                //     if (i == 0)
                //     {
                //         sequencePlayFoldAnimation.Append(mCardUIInfo.imageCard.transform.DOLocalMove(mTargetPos, 0.5f));
                //     }
                //     else
                //     {
                //         sequencePlayFoldAnimation.Join(mCardUIInfo.imageCard.transform.DOLocalMove(mTargetPos, 0.5f));
                //     }
                //     Vector3 mTargetRot = mCardUIInfo.imageCard.transform.localRotation.eulerAngles;
                //     mTargetRot.z += 360f;
                //     sequencePlayFoldAnimation.Join(mCardUIInfo.imageCard.transform.DOLocalRotate(mTargetRot, 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear));
                //     sequencePlayFoldAnimation.Join(mCardUIInfo.imageCard.DOFade(0, 0.3f));
                // }
                //
                // sequencePlayFoldAnimation.OnComplete(() =>
                // {
                //     for (int i = 0, n = listCardUIInfos.Count; i < n; i++)
                //     {
                //         CardUIInfo mCardUIInfo = listCardUIInfos[i];
                //         mCardUIInfo.imageCard.gameObject.SetActive(false);
                //         mCardUIInfo.imageCard.color = Color.white;
                //     }
                // });
            }
        }

        /// <summary>
        /// 播放allin动画
        /// </summary>
        public virtual void PlayAllinArmature()
        {
            armatureAllin.gameObject.SetActive(true);
            if (null != armatureAllin.dragonAnimation)
            {
                armatureAllin.dragonAnimation.Reset();
                armatureAllin.dragonAnimation.Play();
            }
        }

        /// <summary>
        /// 停止allin动画
        /// </summary>
        public virtual void StopAllinArmature()
        {
            if (null != armatureAllin.dragonAnimation && armatureAllin.dragonAnimation.isPlaying)
                armatureAllin.dragonAnimation.Stop();
            armatureAllin.gameObject.SetActive(false);
        }

        /// <summary>
        /// 播放光点动画
        /// </summary>
        public virtual void PlayLightArmature()
        {
            armatureLight.gameObject.SetActive(true);
            if (null != armatureLight.dragonAnimation)
            {
                armatureLight.dragonAnimation.Reset();
                armatureLight.dragonAnimation.Play();
            }
        }

        /// <summary>
        /// 停止关点动画
        /// </summary>
        public virtual void StopLightArmature()
        {
            if (null != armatureLight.dragonAnimation && armatureLight.dragonAnimation.isPlaying)
                armatureLight.dragonAnimation.Stop();
            armatureLight.gameObject.SetActive(false);
        }

        /// <summary>
        /// 播放赢牌头像特效
        /// </summary>
        public virtual void PlayWinArmature()
        {
            UpdateWinCoin();

            transWinner.gameObject.SetActive(true);
            if (IsMySeat)
            {
                if (null != armatureWin.dragonAnimation && armatureWin.dragonAnimation.isPlaying)
                    armatureWin.armature.animation.Stop();
                armatureWin.gameObject.SetActive(false);

                armatureYouWin.gameObject.SetActive(true);
                if (null != armatureYouWin.dragonAnimation)
                {
                    armatureYouWin.dragonAnimation.Reset();
                    armatureYouWin.dragonAnimation.Play();
                }
            }
            else
            {
                if (null != armatureYouWin.dragonAnimation && armatureYouWin.dragonAnimation.isPlaying)
                    armatureYouWin.armature.animation.Stop();
                armatureYouWin.gameObject.SetActive(false);

                armatureWin.gameObject.SetActive(true);
                if (null != armatureWin.dragonAnimation)
                {
                    armatureWin.dragonAnimation.Reset();
                    armatureWin.dragonAnimation.Play();
                }
            }
        }

        /// <summary>
        /// 停止赢牌头像动画
        /// </summary>
        public virtual void StopWinArmature()
        {
            if (null != armatureYouWin.dragonAnimation && armatureYouWin.dragonAnimation.isPlaying)
                armatureYouWin.dragonAnimation.Stop();
            armatureYouWin.gameObject.SetActive(false);
            if (null != armatureWin.dragonAnimation && armatureWin.dragonAnimation.isPlaying)
                armatureWin.dragonAnimation.Stop();
            armatureWin.gameObject.SetActive(false);
            transWinner.gameObject.SetActive(false);

            imageWinner.gameObject.SetActive(false);
        }

        /// <summary>
        /// 播放更新气泡动画
        /// </summary>
        protected void PlayUpdateBubbleAnimation()
        {
            sequenceUpdateBubble = DOTween.Sequence();
            imageBubble.transform.localScale = new Vector3(0.8f, 0.8f, 1);
            imageBubble.color = new Color(1, 1, 1, 0);
            sequenceUpdateBubble.OnStart(() => { imageBubble.gameObject.SetActive(true); });

            sequenceUpdateBubble.Append(imageBubble.transform.DOScale(new Vector3(1, 1, 1), 0.2f));
            sequenceUpdateBubble.Join(imageBubble.DOFade(1, 0.1f));
            sequenceUpdateBubble.Append(imageBubble.transform.DOScale(new Vector3(0.95f, 0.95f, 1), 0.1f));
            sequenceUpdateBubble.Append(imageBubble.transform.DOScale(new Vector3(1, 1, 1), 0.1f));
        }

        #endregion

        #endregion
    }
}
