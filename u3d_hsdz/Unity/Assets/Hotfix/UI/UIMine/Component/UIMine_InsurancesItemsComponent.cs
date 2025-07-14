using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using InsuranceDto = ETHotfix.WEB2_record_detail.InsurancesElement;


namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_InsurancesItemsComponentSystem : AwakeSystem<UIMine_InsurancesItemsComponent>
    {
        public override void Awake(UIMine_InsurancesItemsComponent self)
        {
            self.Awake();
        }
    }

    public class ClubNameDto
    {
        public string name;
        public int score;
    }

    /// <summary> 页面名: </summary>
    public class UIMine_InsurancesItemsComponent : UIBaseComponent
    {
        public sealed class InsurancesData
        {
            public List<InsuranceDto> mInsuranceDtos;
            public string timeStr;
        }

        private ReferenceCollector rc;
        private RectTransform rectcontent;
        private GameObject ItemInfo;
        private GameObject ItemGroup;
        Dictionary<int, List<InsuranceDto>> mInsuranceDtos;
        List<ClubNameDto> mClubDatas = new List<ClubNameDto>();
        private Text textTime;


        public void Awake()
        {
            InitUI();
            mInsuranceDtos = new Dictionary<int, List<InsuranceDto>>();
            mClubDatas = new List<ClubNameDto>();
            float gridWidthCount = rc.GetComponent<RectTransform>().rect.width;
            Log.Debug(gridWidthCount.ToString());
        }

        public override void OnShow(object obj)
        {
            SetUpNav(UIType.UIMine_InsurancesItems);
            if (obj != null && obj is InsurancesData)
            {
                var tDto = obj as InsurancesData;
                if (tDto == null || tDto.mInsuranceDtos == null || tDto.mInsuranceDtos.Count == 0) return;

                float gridWidthCount = rc.GetComponent<RectTransform>().rect.width;
                Log.Debug(gridWidthCount.ToString());

                textTime.text = tDto.timeStr;
                CalcData(tDto.mInsuranceDtos);
                RefreshView(gridWidthCount);         
            }
        }


        private void CalcData(List<InsuranceDto> pDtos)
        {
            for (int i = 0; i < pDtos.Count; i++)
            {
                var tDto = pDtos[i];
                if (mInsuranceDtos.ContainsKey(tDto.clubId))
                {
                    var ts = mInsuranceDtos[tDto.clubId];
                    ts.Add(tDto);
                }
                else
                {
                    var tMp = new List<InsuranceDto>() { tDto };
                    mInsuranceDtos[tDto.clubId] = tMp;
                }
            }

            foreach (var item in mInsuranceDtos)
            {
                var key = item.Key;
                var values = item.Value;
                var count = 0;
                var name = "";
                for (int i = 0; i < values.Count; i++)
                {
                    name = values[i].clubName;
                    count = count - values[i].insurance;
                }
                mClubDatas.Add(new ClubNameDto { name = name, score = count });
            }
        }

        private void RefreshView(float width)
        {
            if (mInsuranceDtos == null || mInsuranceDtos.Count == 0) return;

            GameObject tempItem;
            int heights = 0;
            int index = 0;
            foreach (var item in mInsuranceDtos)
            {
                var key = item.Key;
                var values = item.Value;
                tempItem = GameObject.Instantiate(ItemGroup, rectcontent.transform);

                tempItem.GetComponent<RectTransform>().sizeDelta = new Vector2(width,96);
                tempItem.transform.Find("Item_GroupName").GetComponent<Text>().text = mClubDatas[index].name;
                var clubTxt = tempItem.transform.Find("Item_ScoreTxt").GetComponent<Text>();
                clubTxt.text =  StringHelper.ShowGold(mClubDatas[index].score);
                if (mClubDatas[index].score > 0)
                {
                    clubTxt.color = new Color32(217, 41, 41, 255);
                    clubTxt.text = "+" + clubTxt.text;
                }
                else if (mClubDatas[index].score < 0)
                {
                    clubTxt.color = new Color32(41, 217, 53, 255);
                }
                tempItem.gameObject.SetActive(true);
                heights = heights + 96;
                for (int i = 0; i < values.Count; i++)
                {
                    tempItem = GameObject.Instantiate(ItemInfo, rectcontent.transform);
                    tempItem.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 165);
                    tempItem.gameObject.SetActive(true);
                    heights = heights + 165;
                    tempItem.transform.Find("Item_NameTxt").GetComponent<Text>().text = values[i].nickName;

                    WebImageHelper.SetHeadImage(tempItem.transform.Find("Mask/MemberIcon").GetComponent<RawImage>(), values[i].head);

                    var scoreTxt = tempItem.transform.Find("Item_ScoreTxt").GetComponent<Text>();
                    scoreTxt.text = StringHelper.ShowGold(values[i].insurance);
                    if (values[i].insurance > 0)
                    {
                        scoreTxt.color = new Color32(217, 41, 41, 255);
                        scoreTxt.text = "+" + scoreTxt.text;
                    }
                    else if (values[i].insurance < 0)
                    {
                        scoreTxt.color = new Color32(41, 217, 53, 255);
                    }
                }
                index++;
            }
            rectcontent.sizeDelta = new Vector2(0, heights);
           rectcontent.localPosition = new Vector3(-(width*0.5f), 0, 0);// Vector3.zero;            
        }

        public override void OnHide()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            rectcontent = rc.Get<GameObject>("Normal_scrollview_Content").GetComponent<RectTransform>();
            ItemInfo = rc.Get<GameObject>("Item_info");
            ItemGroup = rc.Get<GameObject>("Item_group");
            textTime = rc.Get<GameObject>("Text_Time").GetComponent<Text>();
        }
    }
}


