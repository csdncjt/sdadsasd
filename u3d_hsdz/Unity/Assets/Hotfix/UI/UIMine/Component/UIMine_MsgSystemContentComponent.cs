using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_MsgSystemContentComponentSystem : AwakeSystem<UIMine_MsgSystemContentComponent>
    {
        public override void Awake(UIMine_MsgSystemContentComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名: </summary>
    public class UIMine_MsgSystemContentComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_MsgSystemContent);
            if (obj != null && obj is string)
            {
                texttitle.text = obj as string;
            }
        }

        public override void OnHide()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        private Text texttitle;
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            texttitle = rc.Get<GameObject>("Text_title").GetComponent<Text>();
        }
    }
}


