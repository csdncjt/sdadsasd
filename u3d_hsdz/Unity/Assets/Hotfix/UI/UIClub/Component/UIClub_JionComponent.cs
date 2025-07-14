
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
    public class UIClub_JionComponentSystem : AwakeSystem<UIClub_JionComponent>
    {
        public override void Awake(UIClub_JionComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_JionComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIClub_Join);

            UpdateLanuage(rc.Get<GameObject>(UIClubAssitent.join_btn_country0), 0);
            UpdateLanuage(rc.Get<GameObject>(UIClubAssitent.join_btn_country1), 1);
            UpdateLanuage(rc.Get<GameObject>(UIClubAssitent.join_btn_country2), 2);
            UpdateLanuage(rc.Get<GameObject>(UIClubAssitent.join_btn_country3), 3);
            UpdateLanuage(rc.Get<GameObject>(UIClubAssitent.join_btn_country4), 4);
            UpdateLanuage(rc.Get<GameObject>(UIClubAssitent.join_btn_country5), 5);
            UpdateLanuage(rc.Get<GameObject>(UIClubAssitent.join_btn_country6), 6);

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.join_btn_queryId)).onClick = async (go) =>
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_JoinQuery, null, () => { });
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.join_btn_country0)).onClick = async (go) =>
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_JoinProvince, null, () => { });
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.join_btn_country1)).onClick = async (go) =>
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_JoinCountry, 1, () => { });
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.join_btn_country2)).onClick = async (go) =>
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_JoinCountry, 2, () => { });
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.join_btn_country3)).onClick = async (go) =>
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_JoinCountry, 3, () => { });
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.join_btn_country4)).onClick = async (go) =>
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_JoinCountry, 4, () => { });
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.join_btn_country5)).onClick = async (go) =>
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_JoinCountry, 5, () => { });
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.join_btn_country6)).onClick = async (go) =>
            {
                await Game.Scene.GetComponent<UIComponent>().ShowAsyncNoAnimation(UIType.UIClub_JoinCountry, 6, () => { });
            };

            WEB2_club_hotClubs.ResponseData res = obj as WEB2_club_hotClubs.ResponseData;
            if (res != null && res.data.list.Count > 0)
            {
                rc.Get<GameObject>(UIClubAssitent.join_mode_hotclub).SetActive(true);
                GameObject[] hotObjs = new GameObject[5]
                {
                    rc.Get<GameObject>(UIClubAssitent.join_culbicon_0),
                    rc.Get<GameObject>(UIClubAssitent.join_culbicon_1),
                    rc.Get<GameObject>(UIClubAssitent.join_culbicon_2),
                    rc.Get<GameObject>(UIClubAssitent.join_culbicon_3),
                    rc.Get<GameObject>(UIClubAssitent.join_culbicon_4)
                };
                for (int i = 0; i < hotObjs.Length; ++i)
                {
                    if (i < res.data.list.Count)
                    {
                        int randomId = res.data.list[i].randomId;
                        string headerId = res.data.list[i].clubHeader;
                        Head h = HeadCache.GetHead(eHeadType.CLUB, randomId.ToString());
                        if (h.headId == string.Empty || h.headId != headerId)
                        {
                            //重新加载
                            //Debug.Log($"重新加载 {headerId}");
                            int dex = i;
                            WebImageHelper.GetHeadImage(headerId, (t2d) => {

                                hotObjs[dex].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = t2d;
                                //缓存头像
                                h.headId = headerId;
                                h.t2d = t2d;
                            });
                        }
                        else
                        {
                            //已存在图片
                            hotObjs[i].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = h.t2d;
                        }

                        //hotObjs[i].transform.GetChild(1).GetComponent<Text>().text = res.data.list[i].clubName;
                        SetLargeTextContent(hotObjs[i].transform.GetChild(1).GetComponent<Text>(), res.data.list[i].clubName);
                        int areaId;
                        int.TryParse(res.data.list[i].clubAreaId, out areaId);
                        hotObjs[i].transform.GetChild(2).GetComponent<Text>().text = DBAreaComponent.Instance.QueryCity(areaId);
                    }
                    else
                    {
                        hotObjs[i].SetActive(false);
                    }
                }

                UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.join_btn_recommend)).onClick = async (go) =>
                {
                    Game.EventSystem.Run(UIClubEventIdType.CLUB_HOT_LIST);
                };
            }
            else
            {
                rc.Get<GameObject>(UIClubAssitent.join_mode_hotclub).SetActive(false);
                UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.join_btn_recommend)).onClick = (go) =>{};
            }
            UITrManager.Instance.Append(UIComponent.Instance.Get(UIType.UIClub).GameObject, rc.gameObject);
        }

        public override void OnHide()
        {

            UIComponent.Instance.Remove(UIType.UIClub_CreatCountry);
            UIComponent.Instance.Remove(UIType.UIClub_CreatProvince);
            UIComponent.Instance.Remove(UIType.UIClub_JoinCity);

            UITrManager.Instance.Remove(rc.gameObject);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            base.Dispose();
            UITrManager.Instance.Remove(rc.gameObject);
            rc = null;
        }

        public void SetArea(string _areaName, int _areaId) {


        }


        public void SetLargeTextContent(Text pText, string pStr)
        {

            float pWidth = pText.GetComponent<RectTransform>().sizeDelta.x;
            var font = pText.font;
            font.RequestCharactersInTexture(pStr, pText.fontSize, pText.fontStyle);
            CharacterInfo characterInfo;
            //font.GetCharacterInfo('' , out characterInfo, pText.fontSize);
            float offlen = pText.fontSize;
            //Log.Debug($"----------offlen = {offlen}");
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            float width = 0;
            for (int i = 0; i < pStr.Length; i++)
            {
                font.GetCharacterInfo(pStr[i], out characterInfo, pText.fontSize);
                width += characterInfo.advance;
                if (width <= pWidth - offlen)
                {
                    sb.Append(pStr[i]);
                    //Log.Debug($"width = {width}");
                }
                else break;
            }
            if(width + offlen > pWidth)sb.Append("...");
            pText.text = sb.ToString();
        }

        void UpdateLanuage(GameObject g, int areaId)
        {

            Text t = g.transform.GetChild(0).GetComponent<Text>();
            t.text = LanguageManager.mInstance.GetLanguageForKey($"area{areaId}");
        }
    }
}

