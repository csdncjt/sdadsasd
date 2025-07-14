using System;
using System.Collections;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{

    [ObjectSystem]
    public class UILobby_FilterComponentAwakeSystem : AwakeSystem<UILobby_FilterComponent>
    {
        public override void Awake(UILobby_FilterComponent self)
        {
            self.Awake();
        }
    }

    public class UILobby_FilterComponent : UIBaseComponent
    {
        string[] allBtns;
        ReferenceCollector rc;
        public void Awake() {

            rc = this.GetParent<UI>().GameObject.transform.Find("view_filter").gameObject.GetComponent<ReferenceCollector>();

            allBtns = new string[]{
                UILobbyAssitent.view_filter_btn_type0 ,
                UILobbyAssitent.view_filter_btn_type1 ,
                UILobbyAssitent.view_filter_btn_type2 ,
                UILobbyAssitent.view_filter_btn_type3 ,
                UILobbyAssitent.view_filter_btn_type4 ,

            UILobbyAssitent.view_filter_btn_behind0,
            UILobbyAssitent.view_filter_btn_behind1,
            UILobbyAssitent.view_filter_btn_behind2,
            UILobbyAssitent.view_filter_btn_num0,
            UILobbyAssitent.view_filter_btn_num1,
            UILobbyAssitent.view_filter_btn_num2,
            UILobbyAssitent.view_filter_btn_qianzhu0,
            UILobbyAssitent.view_filter_btn_qianzhu1,
            UILobbyAssitent.view_filter_btn_baoxian0,
            UILobbyAssitent.view_filter_btn_baoxian1,
            UILobbyAssitent.view_filter_btn_zuowei0,
            UILobbyAssitent.view_filter_btn_zuowei1};
    }

        public override void OnShow()
        {
            rc.gameObject.SetActive(true);
            UIEventListener.Get(rc.Get<GameObject>(UILobbyAssitent.view_filter_btn_close)).onClick = (obj)=>{ this.OnHide(); };
            UIEventListener.Get(rc.Get<GameObject>(UILobbyAssitent.view_filter_btn_clear)).onClick = this.OnClear;
            //UIEventListener.Get(rc.Get<GameObject>("view_filter_btn_type0")).onClick = OnClick;

            UIEventListener.Get(rc.Get<GameObject>(UILobbyAssitent.view_filter_btn_type0) , 0).onIntClick = this.OnType;
            UIEventListener.Get(rc.Get<GameObject>(UILobbyAssitent.view_filter_btn_type1) , 1).onIntClick = this.OnType;
            UIEventListener.Get(rc.Get<GameObject>(UILobbyAssitent.view_filter_btn_type2) , 2).onIntClick = this.OnType;
            UIEventListener.Get(rc.Get<GameObject>(UILobbyAssitent.view_filter_btn_type3) , 3).onIntClick = this.OnType;
            UIEventListener.Get(rc.Get<GameObject>(UILobbyAssitent.view_filter_btn_type4) , 4).onIntClick = this.OnType;
        }

        public override void OnHide()
        {
            rc.gameObject.SetActive(false);
        }

        void OnType(GameObject obj , int type) {

            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            System.Text.StringBuilder activeNotBuilder = new System.Text.StringBuilder();
            switch (type) {

                case 0:
                    builder.Append(UILobbyAssitent.view_filter_btn_type0);
                    break;
                case 1:
                    builder.Append(UILobbyAssitent.view_filter_btn_type1);
                    break;
                case 2:
                    builder.Append(UILobbyAssitent.view_filter_btn_type2);
                    break;
                case 3:
                    builder.Append(UILobbyAssitent.view_filter_btn_type3);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_behind0);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_behind1);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_behind2);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_num0);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_num1);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_num2);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_qianzhu0);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_qianzhu1);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_baoxian0);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_baoxian1);
                    break;
                default:
                    builder.Append(UILobbyAssitent.view_filter_btn_type4);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_behind0);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_behind1);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_behind2);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_qianzhu0);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_qianzhu1);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_baoxian0);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_baoxian1);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_zuowei0);
                    activeNotBuilder.Append(UILobbyAssitent.view_filter_btn_zuowei1);
                    break;
            }

            for (int i = 0; i < allBtns.Length; ++i)
            {
                //Debug.Log($"{allBtns[i]} , {builder.ToString().IndexOf(allBtns[i])}");
                if (builder.ToString().IndexOf(allBtns[i]) != -1)
                {
                    rc.Get<GameObject>(allBtns[i]).GetComponent<Toggle>().isOn = true;
                }
                else
                {
                    rc.Get<GameObject>(allBtns[i]).GetComponent<Toggle>().isOn = false;
                }

                if (activeNotBuilder.ToString().IndexOf(allBtns[i]) != -1)
                {
                    rc.Get<GameObject>(allBtns[i]).GetComponent<Toggle>().interactable = false;
                }
                else
                {
                    rc.Get<GameObject>(allBtns[i]).GetComponent<Toggle>().interactable = true;
                }
            }
        }

        void OnClear(GameObject obj) {

            for (int i = 0; i < allBtns.Length; ++i)
            {
                rc.Get<GameObject>(allBtns[i]).GetComponent<Toggle>().isOn = false;
                rc.Get<GameObject>(allBtns[i]).GetComponent<Toggle>().interactable = true;
            }
        }

        void OnClick(GameObject obj) {

            
        } 
    }
}

