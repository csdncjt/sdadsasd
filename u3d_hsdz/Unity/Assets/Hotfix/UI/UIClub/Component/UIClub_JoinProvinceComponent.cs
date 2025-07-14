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
    public class UIClub_JoinProvinceComponentSystem : AwakeSystem<UIClub_JoinProvinceComponent>
    {
        public override void Awake(UIClub_JoinProvinceComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_JoinProvinceComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        public void Awake()
        {

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_JoinProvince);

            List<DBArea> list = DBAreaComponent.Instance.GetProvinceList();
            GameObject container = rc.Get<GameObject>(UIClubAssitent.joinprovince_provinces);
            GameObject prefab = rc.Get<GameObject>(UIClubAssitent.joinprovince_btn_province);
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
                    int provinceId = list[i].id;
                    string provinceName = LanguageManager.mInstance.GetLanguageForKey($"area{provinceId}");
                    o.transform.GetChild(0).GetComponent<Text>().text = provinceName;
                    int nl = list[i].isNl;
                    UIEventListener.Get(o).onClick = async(go) =>
                    {
                        if (nl == 1) {

                            await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_JoinQuery, new object[2] { provinceName, provinceId }, () => { });
                        }
                        else
                        {
                            await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_JoinCity, provinceId, () => { });
                        }
                        UIComponent.Instance.Remove(UIType.UIClub_JoinProvince);
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


