using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIOrderDialogComponentSystem : AwakeSystem<UIOrderDialogComponent>
    {
        public override void Awake(UIOrderDialogComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名: </summary>
    public class UIOrderDialogComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Text textOrder;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            if (obj != null && obj is string) {
                textOrder.text = (obj as string);
            }
        }

        public override void OnHide()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            UIEventListener.Get(rc.Get<GameObject>("CloseBtn")).onClick = ClickCloseBtn;
            UIEventListener.Get(rc.Get<GameObject>("CopyBtn")).onClick = ClickCopyBtn;
            textOrder = rc.Get<GameObject>("Text_Order").GetComponent<Text>();
        }

        private void ClickCopyBtn(GameObject go)
        {
            UniClipboard.SetText(textOrder.text);
            UIComponent.Instance.ToastLanguage("Become106");
            UIComponent.Instance.Remove(UIType.UIOrderDialog);
        }

        private void ClickCloseBtn(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UIOrderDialog);
        }
    }
}


