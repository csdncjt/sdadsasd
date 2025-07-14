using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMTTRankingComponentAwakeSystem : AwakeSystem<UIMTTRankingComponent>
    {
        public override void Awake(UIMTTRankingComponent self)
        {
            self.Awake();
        }
    }

    public class UIMTTRankingComponent : UIBaseComponent
    {
        public sealed class MTTRankingData
        {
            public int ranking;
            public string content;

        }

        private ReferenceCollector rc;
        private Button buttonClose;
        private Image imageContentFrame;
        private Image imageNum0;
        private Image imageNum1;
        private Image imageNum2;
        private Image imageNum3;
        private Image imageQRcode;
        private Text textContent;
        private Text textQRCode;
        private Image imageRewardFrame;
        private Text textReward;

        private List<Image> listNum;

        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            buttonClose = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            imageContentFrame = rc.Get<GameObject>("Image_ContentFrame").GetComponent<Image>();
            imageNum0 = rc.Get<GameObject>("Image_Num0").GetComponent<Image>();
            imageNum1 = rc.Get<GameObject>("Image_Num1").GetComponent<Image>();
            imageNum2 = rc.Get<GameObject>("Image_Num2").GetComponent<Image>();
            imageNum3 = rc.Get<GameObject>("Image_Num3").GetComponent<Image>();
            imageQRcode = rc.Get<GameObject>("Image_QRcode").GetComponent<Image>();
            textContent = rc.Get<GameObject>("Text_Content").GetComponent<Text>();
            textQRCode = rc.Get<GameObject>("Text_QRCode").GetComponent<Text>();
            imageRewardFrame = rc.Get<GameObject>("Image_RewardFrame").GetComponent<Image>();
            textReward = rc.Get<GameObject>("Text_Reward").GetComponent<Text>();



            UIEventListener.Get(buttonClose.gameObject).onClick = onClickClose;

            listNum = new List<Image>();
            listNum.Add(this.imageNum0);
            listNum.Add(this.imageNum1);
            listNum.Add(this.imageNum2);
            listNum.Add(this.imageNum3);
        }

        private void onClickClose(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UIMTT_Ranking);
        }

        public override void OnShow(object obj)
        {
            if (null != obj)
            {
                MTTRankingData data = obj as MTTRankingData;
                if (null != data)
                {
                    SetContent(data.content);
                    SetRankingNum(data.ranking);
                    SetQRCode(data.content);
                }
            }
        }

       
        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            if (null != this.listNum)
            {
                listNum.Clear();
                listNum = null;
            }

            base.Dispose();
        }

        private void SetRankingNum(int ranking)
        {
            char[] tmp = ranking.ToString().ToCharArray();
            int mCount = tmp.Length;
            int width = 150;
            int height = 180;
            int distance = 75;
            Image tmpImage = null;
            int mStartIndex = mCount;
            switch (mCount)
            {
                case 1:
                    tmpImage = listNum[0];
                    tmpImage.rectTransform.sizeDelta = new Vector2(width, height);
                    tmpImage.sprite = this.rc.Get<Sprite>($"mtt_sz_{tmp[0]}");
                    tmpImage.gameObject.SetActive(true);
                    tmpImage.rectTransform.localPosition = new Vector3(0, 0, 0);
                    break;
                case 2:
                    tmpImage = listNum[0];
                    tmpImage.rectTransform.sizeDelta = new Vector2(width, height);
                    tmpImage.sprite = this.rc.Get<Sprite>($"mtt_sz_{tmp[0]}");
                    tmpImage.gameObject.SetActive(true);
                    tmpImage.rectTransform.localPosition = new Vector3(-distance, 0, 0);
                    tmpImage = listNum[1];
                    tmpImage.rectTransform.sizeDelta = new Vector2(width, height);
                    tmpImage.sprite = this.rc.Get<Sprite>($"mtt_sz_{tmp[1]}");
                    tmpImage.gameObject.SetActive(true);
                    tmpImage.rectTransform.localPosition = new Vector3(distance, 0, 0);
                    break;
                case 3:
                    tmpImage = listNum[0];
                    tmpImage.rectTransform.sizeDelta = new Vector2(width, height);
                    tmpImage.sprite = this.rc.Get<Sprite>($"mtt_sz_{tmp[0]}");
                    tmpImage.gameObject.SetActive(true);
                    tmpImage.rectTransform.localPosition = new Vector3(-width, 0, 0);
                    tmpImage = listNum[1];
                    tmpImage.rectTransform.sizeDelta = new Vector2(width, height);
                    tmpImage.sprite = this.rc.Get<Sprite>($"mtt_sz_{tmp[1]}");
                    tmpImage.gameObject.SetActive(true);
                    tmpImage.rectTransform.localPosition = new Vector3(0, 0, 0);
                    tmpImage = listNum[2];
                    tmpImage.rectTransform.sizeDelta = new Vector2(width, height);
                    tmpImage.sprite = this.rc.Get<Sprite>($"mtt_sz_{tmp[2]}");
                    tmpImage.gameObject.SetActive(true);
                    tmpImage.rectTransform.localPosition = new Vector3(width, 0, 0);
                    break;
                case 4:
                    width = 110;
                    height = 130;
                    distance = 55;
                    tmpImage = listNum[0];
                    tmpImage.rectTransform.sizeDelta = new Vector2(width, height);
                    tmpImage.sprite = this.rc.Get<Sprite>($"mtt_sz_{tmp[0]}");
                    tmpImage.gameObject.SetActive(true);
                    tmpImage.rectTransform.localPosition = new Vector3(-distance - width, 0, 0);
                    tmpImage = listNum[1];
                    tmpImage.rectTransform.sizeDelta = new Vector2(width, height);
                    tmpImage.sprite = this.rc.Get<Sprite>($"mtt_sz_{tmp[1]}");
                    tmpImage.gameObject.SetActive(true);
                    tmpImage.rectTransform.localPosition = new Vector3(-distance, 0, 0);
                    tmpImage = listNum[2];
                    tmpImage.rectTransform.sizeDelta = new Vector2(width, height);
                    tmpImage.sprite = this.rc.Get<Sprite>($"mtt_sz_{tmp[2]}");
                    tmpImage.gameObject.SetActive(true);
                    tmpImage.rectTransform.localPosition = new Vector3(distance, 0, 0);
                    tmpImage = listNum[3];
                    tmpImage.rectTransform.sizeDelta = new Vector2(width, height);
                    tmpImage.sprite = this.rc.Get<Sprite>($"mtt_sz_{tmp[3]}");
                    tmpImage.gameObject.SetActive(true);
                    tmpImage.rectTransform.localPosition = new Vector3(distance + width, 0, 0);
                    break;
                default:
                    Log.Debug($"排名有误");
                    return;
            }

            for (int i = mStartIndex, n = listNum.Count; i < n; i++)
            {
                tmpImage = this.listNum[i];
                tmpImage.gameObject.SetActive(false);
            }
        }

        private void SetContent(string content)
        {
            // 比赛结束有奖励
            this.textReward.text = content;
            this.imageRewardFrame.gameObject.SetActive(true);
        }

        private void SetQRCode(string content)
        {
            this.imageQRcode.gameObject.SetActive(false);
            this.textQRCode.gameObject.SetActive(false);
            //if (string.IsNullOrEmpty(content))
            //{
            //    UIMineModel.mInstance.APIPromotionIsOpen(pDto =>
            //    {
            //        if (pDto.status == 0)
            //        {
            //            if (pDto.data != null && pDto.data.Count > 0)
            //            {
            //                for (int i = 0; i < pDto.data.Count; i++)
            //                {
            //                    if (pDto.data[i].functionCode == 4)
            //                    {
            //                        var size = imageQRcode.GetComponent<RectTransform>().sizeDelta;
            //                        var tSprite = QRCodeHelper.GetQRCodeSprite(pDto.data[i].value, (int)size.x, (int)size.y);
            //                        imageQRcode.sprite = tSprite;
            //                        this.imageQRcode.gameObject.SetActive(true);
            //                        this.textQRCode.gameObject.SetActive(true);
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            UIComponent.Instance.Toast(pDto.status);
            //        }
            //    });
            //}
            //else
            //{
            //    this.imageQRcode.gameObject.SetActive(false);
            //    this.textQRCode.gameObject.SetActive(false);
            //}
        }
    }
}
