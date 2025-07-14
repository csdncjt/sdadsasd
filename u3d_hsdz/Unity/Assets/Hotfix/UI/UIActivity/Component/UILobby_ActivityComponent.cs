using System;
using System.Collections;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{

    [ObjectSystem]
    public class UILobby_ActivityComponentAwakeSystem : AwakeSystem<UILobby_ActivityComponent>
    {
        public override void Awake(UILobby_ActivityComponent self)
        {
            self.Awake();
        }
    }

    public class UILobby_ActivityComponent : UIBaseComponent
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
            UIComponent.Instance.Remove(UIType.UILobby_Activity);
            UIComponent.Instance.ShowNoAnimation(UIType.UIYaoDouEffect, bean);
        }

        private void OnClickClose(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UILobby_Activity);
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

