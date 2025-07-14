using ETModel;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ETHotfix
{
	[ObjectSystem]
	public class UIEmojiPanelComponentSystem : AwakeSystem<UIEmojiPanelComponent>
	{
		public override void Awake(UIEmojiPanelComponent self)
		{
			self.Awake();
		}
	}

	public class UIEmojiPanelComponent : UIBaseComponent
	{
		private ReferenceCollector rc;
		private GameObject maskHide;
		private GameObject gridEmoji;
        private GameObject gridEmoji3D;
        private GameObject gridAnimoji;
        private RectTransform tranContent;
        private Button buttonEmoji;
        private Button buttonEmoji3D;
        private Button buttonAnimoji;


        public void Awake()
		{
			rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            maskHide = rc.Get<GameObject>("MaskHide");
            gridEmoji = rc.Get<GameObject>("grid_emoji");
            gridEmoji3D = rc.Get<GameObject>("grid_emoji_3D");
            gridAnimoji = rc.Get<GameObject>("grid_animoji");
            tranContent = rc.Get<GameObject>("Content").GetComponent<RectTransform>();
            buttonEmoji = rc.Get<GameObject>("Button_Emoji").GetComponent<Button>();
            buttonEmoji3D = rc.Get<GameObject>("Button_Emoji_3D").GetComponent<Button>();
            buttonAnimoji = rc.Get<GameObject>("Button_Animoji").GetComponent<Button>();

            gridEmoji.SetActive(true);
            gridEmoji3D.SetActive(false);
            gridAnimoji.SetActive(false);
            buttonEmoji3D.gameObject.SetActive(false);
            buttonAnimoji.gameObject.SetActive(false);

            NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            float realBottom = safeArea.bottom * 1242 / safeArea.width;
            tranContent.sizeDelta = new Vector2(tranContent.sizeDelta.x, tranContent.sizeDelta.y + realBottom);

            foreach (Transform tranEmoji in gridEmoji.transform)
            {
                UIEventListener.Get(tranEmoji.gameObject).onClick = onClickEmoji;
            }
            //foreach (Transform tranEmoji3D in gridEmoji3D.transform)
            //{
            //    UIEventListener.Get(tranEmoji3D.gameObject).onClick = onClickEmoji;
            //}

            //foreach (Transform tranAnimoji in gridAnimoji.transform)
            //{
            //    string animojiName = tranAnimoji.gameObject.name;
            //    int cost = GameUtil.AnimojiCost(animojiName);
            //    tranAnimoji.gameObject.GetComponentInChildren<Text>().text = StringHelper.ShowGold(cost);
            //    UIEventListener.Get(tranAnimoji.gameObject).onClick = onClickAnimoji;
            //}

            UIEventListener.Get(maskHide).onClick = onClickClose;
            //UIEventListener.Get(buttonEmoji.gameObject).onClick = onSwithToEmoji;
            //UIEventListener.Get(buttonEmoji3D.gameObject).onClick = onSwithToEmoji3D;
            //UIEventListener.Get(buttonAnimoji.gameObject).onClick = onSwithToAnimoji;
        }

		private void onClickClose(GameObject go)
        {
            UIComponent.Instance.HideNoAnimation(UIType.UIEmojiPanel);
		}

        private void onSwithToEmoji(GameObject go)
        {
            gridEmoji.SetActive(true);
            gridEmoji3D.SetActive(false);
            gridAnimoji.SetActive(false);
        }

        private void onSwithToEmoji3D(GameObject go)
        {
            gridEmoji.SetActive(false);
            gridEmoji3D.SetActive(true);
            gridAnimoji.SetActive(false);
        }

        private void onSwithToAnimoji(GameObject go)
        {
            gridEmoji.SetActive(false);
            gridEmoji3D.SetActive(false);
            gridAnimoji.SetActive(true);
        }

        private void onClickEmoji(GameObject go)
        {
            string emojiName = go.name;

            GameCache.Instance.CurGame.SendEmoji(emojiName);

            UIComponent.Instance.HideNoAnimation(UIType.UIEmojiPanel);
        }

        private void onClickAnimoji(GameObject go)
        {
            string animojiName = go.name;

            int cost = GameUtil.AnimojiCost(animojiName);
            if (cost > GameCache.Instance.gold)
            {
                UIComponent.Instance.Toast(LanguageManager.Get("adaptation20010"));
                return;
            }

            GameCache.Instance.CurGame.SendAnimoji(animojiName, GameCache.Instance.nUserId.ToString());

            UIComponent.Instance.HideNoAnimation(UIType.UIEmojiPanel);
        }

        public override void OnShow(object obj)
		{
            tranContent.anchoredPosition = new Vector2(0, -tranContent.sizeDelta.y);
            tranContent.DOAnchorPosY(0, 0.25f);
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
            base.Dispose();
		}
	}
}
