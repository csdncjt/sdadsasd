using System;
using System.Collections;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{

    [ObjectSystem]
    public class UISystemHongbaoComponentAwakeSystem : AwakeSystem<UISystemHongbaoComponent>
    {
        public override void Awake(UISystemHongbaoComponent self)
        {
            self.Awake();
        }
    }

    public class UISystemHongbaoComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Button buttonGo;
        private Button buttonClose;

        private string bean;

        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            buttonGo = rc.Get<GameObject>("Button_Go").GetComponent<Button>();
            buttonClose = rc.Get<GameObject>("Button_Close").GetComponent<Button>();

            UIEventListener.Get(buttonGo.gameObject).onClick = OnClickGo;
            UIEventListener.Get(buttonClose.gameObject).onClick = OnClickClose;
        }

        private void OnClickGo(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UISystemHongbao);
            UIComponent.Instance.ShowNoAnimation(UIType.UIYaoDouEffect, bean);
        }

        private void OnClickClose(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UISystemHongbao);
        }

        public override void OnShow(object obj)
        {
            if (null != obj)
            {
                bean = obj as string;
            }
        }

        public override void OnHide()
        {
            rc.gameObject.SetActive(false);
        }

    }
}

