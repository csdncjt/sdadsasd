using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	[ObjectSystem]
	public class UiLobbyComponentAwakeSystem : AwakeSystem<UILobbyComponent>
	{
		public override void Awake(UILobbyComponent self)
		{
			self.Awake();
		}
	}

    public class UILobbyComponent : UIBaseComponent
	{

        ReferenceCollector rc;
        public void Awake()
		{

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            //默认打开主界面
            UIComponent.Instance.Get(UIType.UILobby_Menu).GetComponent<UILobby_MenuComponent>().ChangeView(UILobby_MenuComponent.ePageType.MATCH);
        }

        
        public override void OnShow(object obj)
        {
            
        }

        public override void OnHide()
        {
            
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
