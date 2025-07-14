using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;


namespace ETHotfix
{
    [ObjectSystem]
    public class UIPromptWifiComponentSystem : AwakeSystem<UIPromptWifiComponent>
    {
        public override void Awake(UIPromptWifiComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UIPromptWifiComponentUpdateSystem : UpdateSystem<UIPromptWifiComponent>
    {
        public override void Update(UIPromptWifiComponent self)
        {
            self.Update();
        }
    }

    /// <summary> 页面名: </summary>
    public class UIPromptWifiComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Transform transContent;

        private Vector3 defaultV3 = Vector3.zero;

        public void Awake()
        {
            InitUI();
        }

        public void Update()
        {

        }

        public override void OnShow(object obj)
        {
            //适配刘海屏
            // NativeManager.SafeArea safeArea = NativeManager.Instance.safeArea;
            // float realTop = safeArea.top * 1242 / safeArea.width;
            // float realBottom = safeArea.bottom * 1242 / safeArea.width;
            //
            // RectTransform rect = transContent as RectTransform;
            // if (null != rect)
            // {
            //     if (defaultV3.x == 0 && defaultV3.y == 0)
            //         defaultV3 = rect.localPosition;
            //     rect.localPosition = new Vector3(defaultV3.x, defaultV3.y - realTop);
            // }
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

        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            transContent = rc.Get<GameObject>("Content").transform;

        }
    }
}


