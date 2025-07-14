using System;
using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

namespace ETHotfix
{
    public partial class Seat : Entity
    {
        #region FSM

        #region 座位待机
        public virtual void IdleEnter()
        {
            Trans.gameObject.SetActive(true);
        }

        public virtual void IdleExecute()
        {

        }

        public virtual void IdleExit()
        {

        }
        #endregion

        #region 空座位
        public virtual void EmptyEnter()
        {
            Player = null;

            SetNickname(string.Empty);
            SetCoin(string.Empty);

            imageBanker.gameObject.SetActive(false);
            imageStraddle.gameObject.SetActive(false);
            imageHeadFrame.gameObject.SetActive(false);
            transSmallCardBacks.gameObject.SetActive(false);
            imageHolding.gameObject.SetActive(false);
            HideCards(listCardUIInfos);
            HideCards(listSmallCardUIInfos);
            imageTrust.gameObject.SetActive(false);
            imageCountDown.gameObject.SetActive(false);
            imageBubble.gameObject.SetActive(false);
            transCurRoundHaveBet.gameObject.SetActive(false);
            imageWinner.gameObject.SetActive(false);
            imageBubbleBackDesk.gameObject.SetActive(false);
            imageBackDesk.gameObject.SetActive(false);
            imageCardType.gameObject.SetActive(false);
            imageRecyclingWinChip.gameObject.SetActive(false);
            FoldHeadGray(false);

            if (null != armatureVoice.dragonAnimation && armatureVoice.dragonAnimation.isPlaying)
                armatureVoice.dragonAnimation.Stop();
            armatureVoice.gameObject.SetActive(false);

            StopAllinArmature();
            StopWinArmature();
            StopLightArmature();

            imageEmpty.gameObject.SetActive(true);
        }

        public virtual void EmptyExecute()
        {

        }

        public virtual void EmptyExit()
        {

        }
        #endregion

        #region 坐下
        public virtual void SitEnter()
        {
            SetClient0BubblePos();

            UpdateHead();
            UpdateNickname();
            UpdateCoin();
            UpdateHolding();
            UpdateShowCardsId();
            UpdateCurRoundHaveBet();
            UpdateCards();
            UpdateBanker();

            StopAllinArmature();
            StopWinArmature();

            imageHeadFrame.gameObject.SetActive(true);
            imageEmpty.gameObject.SetActive(false);
        }

        public virtual void SitExecute()
        {

        }

        public virtual void SitExit()
        {

        }
        #endregion

        #region 坐下动画
        public virtual void SitAnimationEnter()
        {
            FsmLogicComponent.SM.ChangeState(SeatSit<Entity>.Instance);

            //sequenceSitAnimationEnter = DOTween.Sequence();
            //sequenceSitAnimationEnter.Append(Trans.DOScaleX(0, 0.15f).OnComplete(() =>
            //{
            //    FsmLogicComponent.SM.ChangeState(SeatSit<Entity>.Instance);
            //}));
            //sequenceSitAnimationEnter.Append(Trans.DOScaleX(1, 0.15f));
        }

        public virtual void SitAnimationExecute()
        {

        }

        public virtual void SitAnimationExit()
        {

        }
        #endregion

        #region 站起
        public virtual void StandupEnter()
        {
            HideCards(listCardUIInfos);
            HideCards(listSmallCardUIInfos);
            FsmLogicComponent.SM.ChangeState(SeatEmpty<Entity>.Instance);
        }

        public virtual void StandupExecute()
        {

        }

        public virtual void StandupExit()
        {

        }
        #endregion

        #region 站起动画
        public virtual void StandupAnimationEnter()
        {
            sequenceStandupAnimationEnter = DOTween.Sequence();
            sequenceStandupAnimationEnter.Append(Trans.DOScaleX(0, 0.15f).OnComplete(() =>
            {
                FsmLogicComponent.SM.ChangeState(SeatStandup<Entity>.Instance);
            }));
            sequenceStandupAnimationEnter.Append(Trans.DOScaleX(1, 0.15f));
        }

        public virtual void StandupAnimationExecute()
        {

        }

        public virtual void StandupAnimationExit()
        {

        }
        #endregion

        #region 带入
        public virtual void AddChipsEnter()
        {
            UpdateCoin();
            UpdateHolding();
        }

        public virtual void AddChipsExecute()
        {

        }

        public virtual void AddChipsExit()
        {

        }
        #endregion

        #region 等待开始
        public virtual void WaitStartEnter()
        {
            UpdateHead();
            // UpdateNickname();
            UpdateCoin();

            imageHeadFrame.gameObject.SetActive(true);
            imageEmpty.gameObject.SetActive(false);
        }

        public virtual void WaitStartExecute()
        {

        }

        public virtual void WaitStartExit()
        {

        }
        #endregion

        #region 每手开始
        public virtual void StartEnter()
        {
            HideCards(listCardUIInfos);
            HideCards(listSmallCardUIInfos);
            HideCardBack();
        }

        public virtual void StartExecute()
        {

        }

        public virtual void StartExit()
        {

        }
        #endregion

        #region 等待补盲
        public virtual void WaitBlindEnter()
        {

        }

        public virtual void WaitBlindExecute()
        {

        }

        public virtual void WaitBlindExit()
        {

        }
        #endregion

        #region 开始转游戏中
        public virtual void StartToPlayingEnter()
        {
            UpdateBanker();
            UpdateCoin();
            UpdateCards();
            UpdateCurRoundHaveBet();
            UpdateShowCardsId();

            StopAllinArmature();
            StopWinArmature();
        }

        public virtual void StartToPlayingExecute()
        {

        }

        public virtual void StartToPlayingExit()
        {

        }
        #endregion

        #region 操作中
        public virtual void OperationEnter()
        {
            if (IsMySeat)
            {
                SoundComponent.Instance.PlaySFX(SoundComponent.SFX_DESK_PLAYER_TURN);
                return;
            }
                
            StartCountDown(GameCache.Instance.CurGame.GetOpTime());
        }

        public virtual void OperationExecute()
        {
            if (!isCountDown)
                return;

            imageCountDown.fillAmount = (optCurTime -= Time.deltaTime) / optTotalTime;
            if (imageCountDown.fillAmount <= 0)
            {
                isCountDown = false;
                imageCountDown.gameObject.SetActive(false);
                PlayLightArmature();
            }
        }

        public virtual void OperationExit()
        {
            StopCountDown();
        }
        #endregion

        #region 下注
        public virtual void PutChipEnter()
        {
            UpdateCoin();
            UpdateCurRoundHaveBet();
            UpdateBubble();

            PlayBetAnimation();
        }

        public virtual void PutChipExecute()
        {

        }

        public virtual void PutChipExit()
        {

        }
        #endregion

        #region 跟注
        public virtual void CallEnter()
        {
            UpdateCoin();
            UpdateCurRoundHaveBet();
            UpdateBubble();

            PlayBetAnimation();
        }

        public virtual void CallExecute()
        {

        }

        public virtual void CallExit()
        {

        }
        #endregion

        #region 加注
        public virtual void RaiseEnter()
        {
            UpdateCoin();
            UpdateCurRoundHaveBet();
            UpdateBubble();

            PlayBetAnimation();
        }

        public virtual void RaiseExecute()
        {

        }

        public virtual void RaiseExit()
        {

        }
        #endregion

        #region 全下
        public virtual void AllinEnter()
        {
            UpdateCoin();
            UpdateCurRoundHaveBet();
            UpdateBubble();

            PlayBetAnimation();
        }

        public virtual void AllinExecute()
        {

        }

        public virtual void AllinExit()
        {

        }
        #endregion

        #region 让牌
        public virtual void CheckEnter()
        {
            UpdateBubble();

            SoundComponent.Instance.PlaySFX(SoundComponent.SFX_DESK_PLAYER_CHECK);
        }

        public virtual void CheckExecute()
        {

        }

        public virtual void CheckExit()
        {

        }
        #endregion

        #region 弃牌
        public virtual void FoldEnter()
        {
            UpdateBubble();
            FoldHeadGray(Player.isFold);
            PlayFoldAnimation();
            // HideCardType();

            SoundComponent.Instance.PlaySFX(SoundComponent.SFX_DESK_PLAYER_FOLD);
        }

        public virtual void FoldExecute()
        {

        }

        public virtual void FoldExit()
        {

        }
        #endregion

        #region 超时
        public virtual void TimeoutEnter()
        {
            UpdateBubble();
        }

        public virtual void TimeoutExecute()
        {

        }

        public virtual void TimeoutExit()
        {

        }
        #endregion

        #region 空闲等待下一局
        public virtual void WaitNextRoundEnter()
        {

        }

        public virtual void WaitNextRoundExecute()
        {

        }

        public virtual void WaitNextRoundExit()
        {

        }
        #endregion

        #region 等待入座
        public virtual void WaitSitDownEnter()
        {

        }

        public virtual void WaitSitDownExecute()
        {

        }

        public virtual void WaitSitDownExit()
        {

        }
        #endregion

        #region straddle
        public virtual void StraddleEnter()
        {
            UpdateBubble();
        }

        public virtual void StraddleExecute()
        {

        }

        public virtual void StraddleExit()
        {

        }
        #endregion

        #region 盖牌/翻牌
        public virtual void DrawEnter()
        {

        }

        public virtual void DrawExecute()
        {

        }

        public virtual void DrawExit()
        {

        }
        #endregion

        #region 留座
        public virtual void ReservationEnter()
        {

        }

        public virtual void ReservationExecute()
        {

        }

        public virtual void ReservationExit()
        {

        }
        #endregion

        #region 占座等待
        public virtual void ReservationWaitEnter()
        {

        }

        public virtual void ReservationWaitExecute()
        {

        }

        public virtual void ReservationWaitExit()
        {

        }
        #endregion

        #region 等待其他玩家操作
        public virtual void WaitOtherEnter()
        {

        }

        public virtual void WaitOtherExecute()
        {

        }

        public virtual void WaitOtherExit()
        {

        }
        #endregion

        #region 比牌
        public virtual void CompareEnter()
        {

        }

        public virtual void CompareExecute()
        {

        }

        public virtual void CompareExit()
        {

        }
        #endregion

        #region 本轮结束
        public virtual void RoundEndEnter()
        {
            HideCards(listCardUIInfos);
            HideCards(listSmallCardUIInfos);
            HideCardBack();
            ClearCurRoundHaveBet();
            UpdateShowCardsId();
            HideBubbleInsurance();

            imageCardType.gameObject.SetActive(false);

            StopAllinArmature();
            StopWinArmature();
        }

        public virtual void RoundEndExecute()
        {

        }

        public virtual void RoundEndExit()
        {

        }
        #endregion

        #region 购买保险
        public virtual void InsuranceEnter()
        {
            StartCountDown(Player.timeLeft_insurance, true);
            this.ShowBubbleInsuranceCountDown();
        }

        public virtual void InsuranceExecute()
        {
            if (!isCountDown)
                return;

            imageCountDown.fillAmount = (optCurTime -= Time.deltaTime) / optTotalTime;
            if (imageCountDown.fillAmount <= 0)
            {
                isCountDown = false;
                imageCountDown.gameObject.SetActive(false);
                PlayLightArmature();
            }

            if (Player.userID != GameCache.Instance.CurGame.MainPlayer.userID)
            {
                double leftTime = Math.Ceiling(optCurTime);
                if (leftTime < 0)
                    leftTime = 0;
                textBubbleInsuranceCountDown.text = CPErrorCode.LanguageDescription(20062, new List<object>() { leftTime });
            }
        }

        public virtual void InsuranceExit()
        {
            StopCountDown();
            HideBubbleInsuranceCountDown();
        }
        #endregion

        #region 留座离桌
        public virtual void KeepEnter()
        {
            ShowBubbleBackDesk();
        }

        public virtual void KeepExecute()
        {
            if (Time.time - keepSeatDeltaTime > 1f)
            {
                keepSeatDeltaTime = Time.time;
                keepSeatLeftTime -= 1;
                if (keepSeatLeftTime <= 0)
                {
                    bKeepSeatCounting = false;
                }
                else
                {
                    textBubbleBackDesk.text = CPErrorCode.LanguageDescription(20061, new List<object>() { keepSeatLeftTime });
                }
            }
        }

        public virtual void KeepExit()
        {
            HideBubbleBackDesk();
        }
        #endregion

        #region 带入审核
        public virtual void WaitDairuEnter()
        {
            UI mUIAddChips = UIComponent.Instance.Get(UIType.UIAddChips);
            if (null != mUIAddChips && mUIAddChips.GameObject.activeInHierarchy)
            {
                UIComponent.Instance.HideNoAnimation(UIType.UIAddChips);
            }

            ShowBubbleWaitDairu();
        }

        public virtual void WaitDairuExecute()
        {
            if (Time.time - keepSeatDeltaTime > 1f)
            {
                keepSeatDeltaTime = Time.time;
                Player.timeLeft_dairu -= 1;
                if (Player.timeLeft_dairu <= 0)
                {
                    bKeepSeatCounting = false;
                }
                else
                {
                    textBubbleDauru.text = CPErrorCode.LanguageDescription(20178, new List<object>() { Player.timeLeft_dairu });
                }
            }
        }

        public virtual void WaitDairuExit()
        {
            HideBubbleWaitDairu();
        }
        #endregion

        #endregion
    }
}
