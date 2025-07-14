using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UICowboyAwakeSystem : AwakeSystem<UICowboy>
    {
        public override void Awake(UICowboy self)
        {
            self.Awake();
        }
    }


    public class UICowboy : UIBaseComponent
    {
        private ReferenceCollector rc;

        public void Awake()
        {
            registerHandler();
        }

        public override void OnShow(object obj)
        {

        }

        public override void OnHide()
        {

        }

        public override void Dispose()
        {
            if (this.IsDisposed)
                return;
            removeHandler();
            base.Dispose();
        }


        private void registerHandler()
        {
        }

        private void removeHandler()
        {
        }




    }
}


