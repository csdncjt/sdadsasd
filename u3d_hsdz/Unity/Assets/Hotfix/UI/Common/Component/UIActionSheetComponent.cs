using System;
using System.Collections.Generic;
using System.Net;
using BestHTTP;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIActionSheetComponentSystem : AwakeSystem<UIActionSheetComponent>
    {
        public override void Awake(UIActionSheetComponent self)
        {
            self.Awake();
        }
    }

    public class UIActionSheetComponent : UIBaseComponent
    {
        public sealed class ActionSheetData
        {
            public string[] titles;
            public ActionDelegate actionDelegate;
        }
        private ActionSheetData actionSheetData;
        public delegate void ActionDelegate(int index);

        private List<GameObject> btns = new List<GameObject>();

        private ReferenceCollector rc;
        private Button btn_action;

        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            btn_action = rc.Get<GameObject>("btn_action").GetComponent<Button>();
        }

        public override void OnShow(object obj)
        {
            foreach (GameObject btn in btns)
            {
                UnityEngine.Object.Destroy(btn);
            }
            if (null != obj)
            {
                this.actionSheetData = obj as ActionSheetData;
                if (null != this.actionSheetData)
                {
                    for (int i = 0; i < actionSheetData.titles.Length; i++)
                    {
                        GameObject actionBtn = GetActionBtn(rc.transform);
                        actionBtn.transform.Find("Text").GetComponent<Text>().text = actionSheetData.titles[i];
                        actionBtn.transform.Find("Text").GetComponent<Text>().color = new Color32(253, 230, 189, 255);
                        UIEventListener.Get(actionBtn, i).onIntClick = ClickActionItem;
                        btns.Add(actionBtn);
                    }
                }

                GameObject cancleBtn = GetActionBtn(rc.transform);
                cancleBtn.transform.Find("Text").GetComponent<Text>().text = LanguageManager.Get("adaptation10013");
                cancleBtn.transform.Find("Text").GetComponent<Text>().color = new Color32(255, 255, 255, 255);
                UIEventListener.Get(cancleBtn).onClick = ClickCancel;
                btns.Add(cancleBtn);
            }
        }

        GameObject GetActionBtn(Transform parent)
        {
            GameObject obj = GameObject.Instantiate(btn_action.gameObject);
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
            base.Dispose();
        }

        private void ClickCancel(GameObject go)
        {
            Game.Scene.GetComponent<UIComponent>().Hide(UIType.UIActionSheet, null, 0);
        }

        private void ClickActionItem(GameObject go, int index)
        {
            actionSheetData.actionDelegate(index);
            Game.Scene.GetComponent<UIComponent>().Hide(UIType.UIActionSheet, null, 0);
        }

    }
}
