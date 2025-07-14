using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_SettingNetComponentSystem : AwakeSystem<UIMine_SettingNetComponent>
    {
        public override void Awake(UIMine_SettingNetComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_SettingNetComponent : UIBaseComponent
    {

        private ReferenceCollector rc;
        private Button buttonnet2;
        private Button buttonnet3;
        private Button buttonnet1;
        /// <summary>
        /// 线路,从1开始
        /// </summary>
        public int mNetType = 1;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            SetUpNav( UIType.UIMine_SettingNet);
        }

        public override void OnHide()
        {           
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        List<GameObject> mBtnsChild;
        #region UI
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            mBtnsChild = new List<GameObject>();
            var tStr = "";
            GameObject tGO;
            for (int i = 1; i < 4; i++)
            {
                tStr = "Button_net" + i.ToString();
                tGO = rc.Get<GameObject>(tStr);
                UIEventListener.Get(tGO,i).onIntClick=ClickNets;
                mBtnsChild.Add(tGO.transform.Find("Image_on").gameObject);
            }
        }

        private void ClickNets(GameObject go, int index)
        {
            if (index == mNetType) return;

            for (int i = 0; i < mBtnsChild.Count; i++)
            {
                mBtnsChild[i].SetActive(false);
            }
            mBtnsChild[index - 1].SetActive(true);
            mNetType = index;
        }      


        #endregion


    }
}

