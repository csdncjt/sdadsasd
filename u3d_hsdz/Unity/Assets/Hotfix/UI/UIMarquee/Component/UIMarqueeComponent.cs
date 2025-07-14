using System.Collections.Generic;
using System.Text;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	[ObjectSystem]
	public class UIMarqueeComponentAwakeSystem : AwakeSystem<UIMarqueeComponent>
	{
		public override void Awake(UIMarqueeComponent self)
		{
			self.Awake();
		}
	}

    public class UIMarqueeComponent : UIBaseComponent
	{
		private ReferenceCollector rc;
        private Text textContent;
        private Image imageMask;

        private Queue<string> queueContent = new Queue<string>();
        private Tweener tweener;
        private Vector2 imageMaskSizeDelta;
        private StringBuilder sbContent;

        private bool isInit = false;

        public void Awake()
		{
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            textContent = rc.Get<GameObject>("Text_Content").GetComponent<Text>();
            imageMask = rc.Get<GameObject>("Image_Mask").GetComponent<Image>();

            registerHandler();
        }

		public override void OnShow(object obj)
        {
            if (null != obj)
            {
                if (null == sbContent)
                    sbContent = new StringBuilder();
                else
                    sbContent.Clear();

                sbContent.Append(obj.ToString());
                for (int i = 0; i < 5; i++)
                {
                    queueContent.Enqueue(sbContent.ToString());
                }

                if ((queueContent.Count == 1 || queueContent.Count == 5) && (null == tweener || !tweener.IsPlaying()))
                    PlayMarquee();
            }
        }

        public override void OnHide()
		{
			
		}

		public override void Dispose()
		{
            if (IsDisposed)
            {
                return;
            }

            queueContent.Clear();

            removeHandler();
			base.Dispose();
		}

		private void registerHandler()
		{
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_Guild_BROADCAST, HANDLER_REQ_Guild_BROADCAST);    // 牌局广播
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_GAME_JACKPOT_HIT_BROADCAST, HANDLER_REQ_GAME_JACKPOT_HIT_BROADCAST);    // JackPot击中广播
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_SYSTEM_BROADCAST, HANDLER_REQ_SYSTEM_BROADCAST);  // Res系统广播
        }

        private void removeHandler()
		{
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_Guild_BROADCAST, HANDLER_REQ_Guild_BROADCAST);    // 牌局广播
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_GAME_JACKPOT_HIT_BROADCAST, HANDLER_REQ_GAME_JACKPOT_HIT_BROADCAST);    // JackPot击中广播
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_SYSTEM_BROADCAST, HANDLER_REQ_SYSTEM_BROADCAST);    // Res系统广播
        }

        private void PlayMarquee()
        {
            if (!isInit)
            {
                // 和调用原生获取安全区域方法的顺序有关
                isInit = true;
                //适配刘海屏
                NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
                float realTop = safeArea.top * 1242 / safeArea.width;
                float realBottom = safeArea.bottom * 1242 / safeArea.width;

                Vector3 mCacheLPos = imageMask.rectTransform.localPosition;
                imageMask.rectTransform.localPosition = new Vector3(mCacheLPos.x, mCacheLPos.y - realTop, mCacheLPos.z);
                imageMaskSizeDelta = imageMask.rectTransform.sizeDelta;
            }
            

            if (queueContent.Count == 0)
            {
                textContent.text = string.Empty;
                imageMask.gameObject.SetActive(false);
                tweener.Kill();
                return;
            }

            if (!imageMask.gameObject.activeInHierarchy)
            {
                imageMask.gameObject.SetActive(true);
            }

            string mTmp = queueContent.Dequeue();
            if (string.IsNullOrEmpty(mTmp))
                return;

            textContent.text = mTmp;
            textContent.rectTransform.localPosition = new Vector3(imageMaskSizeDelta.x/2f, textContent.rectTransform.localPosition.y);
            float mTmpTime = textContent.text.Length / 2f;
            if (mTmpTime < 10)
                mTmpTime = 10;
            tweener = textContent.rectTransform.DOLocalMoveX(-(textContent.preferredWidth + imageMaskSizeDelta.x / 2f), mTmpTime).SetEase(Ease.Linear);
            tweener.OnComplete(PlayMarquee);
        }

        private void HANDLER_REQ_Guild_BROADCAST(ICPResponse response)
        {
            REQ_Guild_BROADCAST rec = response as REQ_Guild_BROADCAST;
            if (null == rec)
                return;

            if (rec.status != 0)
                return;

            if (null == sbContent)
                sbContent = new StringBuilder();
            else
                sbContent.Clear();

            // for (int i = 0; i < rec.playTime; i++)
            // {
            //     sbContent.Append("[");
            //     sbContent.Append(rec.roomID);
            //     sbContent.Append("]");
            //     sbContent.Append(rec.content);
            //     if (i < rec.playTime - 1)
            //         sbContent.Append("    ");
            // }

            // sbContent.Append("[");
            // sbContent.Append(rec.roomID);
            // sbContent.Append("]");
            sbContent.Append(rec.content);
            for (int i = 0; i < rec.playTime; i++)
            {
                queueContent.Enqueue(sbContent.ToString());
            }

            if ((queueContent.Count == 1 || queueContent.Count == rec.playTime) && (null == tweener || !tweener.IsPlaying()))
                PlayMarquee();
        }

        private void HANDLER_REQ_GAME_JACKPOT_HIT_BROADCAST(ICPResponse response)
        {
            REQ_GAME_JACKPOT_HIT_BROADCAST rec = response as REQ_GAME_JACKPOT_HIT_BROADCAST;
            if (null == rec)
                return;

            if (rec.status != 0)
                return;

            if (null == sbContent)
                sbContent = new StringBuilder();
            else
                sbContent.Clear();

            // 恭喜xxx击中xxx（牌型），赢得xxxUSDT

            // sbContent.Append("JackPot[");
            // sbContent.Append(rec.userName);
            // sbContent.Append("]");
            // sbContent.Append(" 牌型[");
            // sbContent.Append(CardTypeUtil.GetCardTypeName(rec.cardType));
            // sbContent.Append("]");
            // sbContent.Append(" 奖励[");
            // sbContent.Append(rec.reward);
            // sbContent.Append("]");
            //
            // queueContent.Enqueue(sbContent.ToString());

            sbContent.Append(CPErrorCode.LanguageDescription(20054, new List<object>(){rec.userName, CardTypeUtil.GetCardTypeName(rec.cardType), StringHelper.ShowGold(rec.reward)}));

            int mPlayTime = 3;
            for (int i = 0; i < mPlayTime; i++)
            {
                queueContent.Enqueue(sbContent.ToString());
            }

            if ((queueContent.Count == 1 || queueContent.Count == mPlayTime) && (null == tweener || !tweener.IsPlaying()))
                PlayMarquee();
        }

        private void HANDLER_REQ_SYSTEM_BROADCAST(ICPResponse response)
        {
            REQ_SYSTEM_BROADCAST rec = response as REQ_SYSTEM_BROADCAST;
            if (null == rec)
                return;

            if (rec.status != 0)
                return;

            if (null == sbContent)
                sbContent = new StringBuilder();
            else
                sbContent.Clear();

            // for (int i = 0; i < rec.playTime; i++)
            // {
            //     sbContent.Append(rec.content);
            //     if (i < rec.playTime - 1)
            //         sbContent.Append("    ");
            // }

            for (int i = 0; i < rec.playTime; i++)
            {
                queueContent.Enqueue(rec.content);
            }

            if ((queueContent.Count == 1 || queueContent.Count == rec.playTime) && (null == tweener || !tweener.IsPlaying()))
                PlayMarquee();
        }

        public void TestMarquee(string content, int playTime = 1)
        {
            if (null == sbContent)
                sbContent = new StringBuilder();
            else
                sbContent.Clear();

            for (int i = 0; i < playTime; i++)
            {
                queueContent.Enqueue(content);
            }

            if ((queueContent.Count == 1 || queueContent.Count == playTime) && (null == tweener || !tweener.IsPlaying()))
                PlayMarquee();
        }
    }
}
