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
    public class UIData_TerminolComponentAwakeSystem : AwakeSystem<UIData_TerminolComponent>
    {
        public override void Awake(UIData_TerminolComponent self)
        {
            self.Awake();
        }
    }

    public class UIData_TerminolComponent : UIBaseComponent
    {
        public void Awake()
        {
            SetUpNav(UIType.UIData_Terminol);

        }


        public override void OnShow(object obj)
        {
            
        }

        public override void OnHide()
        {
            
        }
    }
}
