using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;


namespace ETHotfix
{
    [ObjectSystem]
    public class UIUserRemarksComponentSystem : AwakeSystem<UIUserRemarksComponent>
    {
        public override void Awake(UIUserRemarksComponent self)
        {
            self.Awake();
        }
    }

    /// <summary> 页面名: </summary>
    public class UIUserRemarksComponent : UIBaseComponent
    {
        public sealed class RemarkData
        {
            public string RandomId;
            public int userId;
            public RemardDataDelegate successDelegateAPI;
        }

        public delegate void RemardDataDelegate();

        private RemarkData mRemarkData;
        private ReferenceCollector rc;
        private InputField inputfieldBattle;
        private Transform transColor1;
        private Transform transColor2;
        private Transform transColor3;
        private InputField inputfieldName;

        public void Awake()
        {
            InitUI();
        }

        public override void OnShow(object obj)
        {
            if (obj != null && obj is RemarkData)
            {
                mRemarkData = obj as RemarkData;
                var tDto = CacheDataManager.mInstance.GetSNSBatchRelation(mRemarkData.RandomId);
                if (tDto == null) return;
                inputfieldBattle.text = tDto.remark;
                inputfieldName.text = tDto.remarkName;
                for (int i = 0; i < mToggle.Count; i++)
                {
                    mToggle[i].isOn = false;
                }
                if (tDto.remarkColor >= 0)
                {
                    mToggle[tDto.remarkColor].isOn = true;
                    mCurToggleColor = tDto.remarkColor;
                }
            }
        }

        public override void OnHide()
        {
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
                return;
            base.Dispose();
        }
        List<Toggle> mToggle = new List<Toggle>();
        protected virtual void InitUI()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            inputfieldBattle = rc.Get<GameObject>("InputField_Battle").GetComponent<InputField>();
            transColor1 = rc.Get<GameObject>("Color1").transform;
            transColor2 = rc.Get<GameObject>("Color2").transform;
            transColor3 = rc.Get<GameObject>("Color3").transform;
            inputfieldName = rc.Get<GameObject>("InputField_Name").GetComponent<InputField>();

            UIEventListener.Get(transColor1.gameObject, 0).onIntClick = ClickToggle;
            UIEventListener.Get(transColor2.gameObject, 1).onIntClick = ClickToggle;
            UIEventListener.Get(transColor3.gameObject, 2).onIntClick = ClickToggle;

            mToggle = new List<Toggle>();
            mToggle.Add(transColor1.GetComponent<Toggle>());
            mToggle.Add(transColor2.GetComponent<Toggle>());
            mToggle.Add(transColor3.GetComponent<Toggle>());

            UIEventListener.Get(rc.Get<GameObject>("CommitBtn")).onClick = ClickCommit;
            UIEventListener.Get(rc.Get<GameObject>("CancelBtn")).onClick = ClickCancel;
            UIEventListener.Get(rc.Get<GameObject>("CloseBtn")).onClick = ClickCancel;
        }

        private int mCurToggleColor = -1;
        private void ClickToggle(GameObject go, int index)
        {
            if (mCurToggleColor == index) return;
            mCurToggleColor = index;
        }

        private void ClickCancel(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UIUserRemarks);
        }

        private void ClickCommit(GameObject go)
        {
            bool tReq = true;
            if (mCurToggleColor == -1 && string.IsNullOrEmpty(inputfieldBattle.name) && string.IsNullOrEmpty(inputfieldName.text))
            {
                tReq = false;
            }
            if (tReq == true)
            {
                API2Remark(mRemarkData.RandomId, mCurToggleColor, inputfieldName.text, inputfieldBattle.text, pDto =>
                  {
                      if (pDto.status == 0)
                      {
                          // UIComponent.Instance.Toast("修改成功");
                          UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10079));
                          var tNewDto = new WEB2_sns_batch_relations.DataElement();
                          tNewDto.randomNum = mRemarkData.RandomId;
                          tNewDto.userId = mRemarkData.userId;
                          tNewDto.remark = inputfieldBattle.text;
                          tNewDto.remarkColor = mCurToggleColor;
                          tNewDto.remarkName = inputfieldName.text;
                          CacheDataManager.mInstance.AddModifySNSBatchDto(tNewDto);
                          ClickCancel(null);
                          mRemarkData.successDelegateAPI();
                      }
                      else
                      {
                          UIComponent.Instance.Toast(pDto.status);
                      }
                  });
            }
            else
            {
                // UIComponent.Instance.Toast("一个都没填");
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10321));
            }
        }

        void API2Remark(string pRanomId, int pColor, string pName, string pRemark, Action<WEB2_sns_mod_remark.ResponseData> pAct)
        {
            var web = new WEB2_sns_mod_remark.RequestData()
            {
                remarkColor = pColor,
                remarkName = pName,
                otherRandomId = pRanomId,
                remark = pRemark
            };
            HttpRequestComponent.Instance.Send(WEB2_sns_mod_remark.API,
             WEB2_sns_mod_remark.Request(web), res =>
             {
                 var tDto = WEB2_sns_mod_remark.Response(res);
                 if (pAct != null)
                     pAct(tDto);
             });
        }
    }
}


