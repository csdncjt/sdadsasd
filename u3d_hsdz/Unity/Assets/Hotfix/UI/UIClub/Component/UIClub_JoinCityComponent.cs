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
    public class UIClub_JoinCityComponentSystem : AwakeSystem<UIClub_JoinCityComponent>
    {
        public override void Awake(UIClub_JoinCityComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_JoinCityComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        public void Awake()
        {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_JoinCity);

            int provinceId = (int)obj;
            List<DBArea> list = DBAreaComponent.Instance.GetCityList(provinceId);
            GameObject container = rc.Get<GameObject>(UIClubAssitent.joincity_citys);
            GameObject prefab = rc.Get<GameObject>(UIClubAssitent.joincity_btn_citys);
            int len = Mathf.Max(list.Count, container.transform.childCount);
            for (int i = 0; i < len; ++i)
            {

                if (i < list.Count)
                {

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
                    int cityid = list[i].id;
                    string cityname = LanguageManager.mInstance.GetLanguageForKey($"area{cityid}");
                    o.transform.GetChild(0).GetComponent<Text>().text = cityname;
                    UIEventListener.Get(o).onClick = async(go) =>
                    {
                        //Debug.Log(cityid);
                        await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_JoinQuery, new object[2] { cityname , cityid }, () => { });
                        UIComponent.Instance.Remove(UIType.UIClub_JoinCity);
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



