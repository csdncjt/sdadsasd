using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using ETModel;

namespace ETHotfix
{

    [ObjectSystem]
    public class UILogin_LineComponentSystem : AwakeSystem<UILogin_LineComponent>
    {
        public override void Awake(UILogin_LineComponent self)
        {
            self.Awake();
        }
    }

    public class UILogin_LineComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Button buttonClose;
        private Transform tranToggleGroup;

        private List<GameObject> toggleItems;

        public void Awake() {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            tranToggleGroup = rc.Get<GameObject>("ToggleGroup").transform;
            buttonClose = rc.Get<GameObject>("Button_Close").GetComponent<Button>();

            this.toggleItems = new List<GameObject>();

            UIEventListener.Get(this.buttonClose.gameObject).onClick = onClickClose;
        }

        private void onClickClose(GameObject go)
        {
            Game.Scene.GetComponent<UIComponent>().HideNoAnimation(UIType.UILogin_Line);
        }

        public override void OnShow(object obj)
        {
            string info = PlayerPrefsMgr.mInstance.GetString($"{PlayerPrefsKeys.NET_LINE_SWITCH_INFO}{GlobalData.Instance.serverType}", null);
            if (!string.IsNullOrEmpty(info))
            {
                foreach (GameObject toogleItem in toggleItems)
                {
                    UnityEngine.Object.Destroy(toogleItem);
                }
                toggleItems.Clear();
                NetLineSwitchComponent.NetLineSwitchData lineSwitchData = JsonHelper.FromJson<NetLineSwitchComponent.NetLineSwitchData>(info);
                int selectID = lineSwitchData.selector;
                string userChoice = PlayerPrefsMgr.mInstance.GetString($"{PlayerPrefsKeys.CURRENT_SERVER_USER_CHOICEID}{GlobalData.Instance.serverType}", null);
                if (!string.IsNullOrEmpty(userChoice))
                {
                    selectID = Convert.ToInt32(userChoice);
                }

                foreach (NetLineSwitchComponent.LineInfo lineInfo in lineSwitchData.list)
                {
                    GameObject tooggle = GetToggleItem(tranToggleGroup);
                    toggleItems.Add(tooggle);
                    UIEventListener.Get(tooggle, lineInfo.id).onIntClick = onClickToggle;
                    var nameText = tooggle.transform.Find("Label").GetComponent<Text>();
                    nameText.text = GlobalData.Instance.NameForServerID(lineInfo.id);
                    Toggle tooggleCs = tooggle.GetComponent<Toggle>();
                    tooggleCs.isOn = lineInfo.id == selectID;
                }
            }
        }

        private void onClickToggle(GameObject go, int serverID)
        {
            GlobalData.Instance.UserSwitchServer(serverID);
            Game.Scene.GetComponent<UIComponent>().HideNoAnimation(UIType.UILogin_Line);

            UI uI = UIComponent.Instance.Get(UIType.UIMine_Setting);
            if (null != uI)
            {
                UIMine_SettingComponent mineComponent = uI.UiBaseComponent as UIMine_SettingComponent;
                mineComponent.UpdateNetLine(GlobalData.Instance.NameForServerID(serverID));
            }
        }

        GameObject GetToggleItem(Transform parent)
        {
            GameObject obj = GameObject.Instantiate(rc.Get<GameObject>("Toggle"));
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(parent);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            return obj;
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
            rc = null;
            buttonClose = null;
            tranToggleGroup = null;
            base.Dispose();
        }
    }
}
