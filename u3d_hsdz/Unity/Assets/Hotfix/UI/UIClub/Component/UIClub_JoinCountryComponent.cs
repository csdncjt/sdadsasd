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
    public class UIClub_JoinCountryComponentSystem : AwakeSystem<UIClub_JoinCountryComponent>
    {
        public override void Awake(UIClub_JoinCountryComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_JoinCountryComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        public void Awake()
        {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_JoinCountry);

            int areaId = (int)obj;
            List<DBArea> list = DBAreaComponent.Instance.GetCountryList(areaId);
            GameObject container = rc.Get<GameObject>(UIClubAssitent.joincountry_countrys);
            GameObject prefab = rc.Get<GameObject>(UIClubAssitent.joincountry_btn_country);
            int len = Mathf.Max(list.Count , container.transform.childCount);
            for (int i = 0; i < len; ++i) {

                if (i < list.Count) {

                    GameObject o;
                    if (container.transform.childCount > i)
                    {

                        o = container.transform.GetChild(i).gameObject;
                    }
                    else
                    {
                        o = GameObject.Instantiate(prefab, container.transform);
                    }
                    o.SetActive(true);
                    int countryid = list[i].id;
                    string countryname = LanguageManager.mInstance.GetLanguageForKey($"area{countryid}");
                    o.transform.GetChild(0).GetComponent<Text>().text = countryname;
                    UIEventListener.Get(o).onClick = async(go) =>
                    {
                        //Debug.Log(countryid);
                        await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_JoinQuery, new object[2] { countryname, countryid }, () => { });
                        UIComponent.Instance.Remove(UIType.UIClub_JoinCountry);
                    };
                }
                else
                {
                    container.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

        }

        public override void OnHide()
        {

        }
    }
}

