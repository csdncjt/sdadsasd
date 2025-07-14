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
    public class UIClub_TransComponentSystem : AwakeSystem<UIClub_TransComponent>
    {
        public override void Awake(UIClub_TransComponent self)
        {
            self.Awake();
        }
    }

    public class UIClub_TransComponent : UIBaseComponent
    {

        ReferenceCollector rc;
        int chargesType;//充值类型
        int iClubId;
        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        public override void OnShow(object obj)
        {
            object[] arr = obj as object[];
            iClubId = (int)arr[0];
            chargesType = (int)arr[1];
            UpdateChargesType(chargesType);
            SetUpNav(UIType.UIClub_Trans);
            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.trans_btn_thrid)).onClick = (go) =>
            {
                //Log.Debug("平台充值");
                if (chargesType != 1) {

                    UIComponent.Instance.ShowNoAnimation(UIType.UIDialog,
                                new UIDialogComponent.DialogData()
                                {
                                    type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                                    title = LanguageManager.mInstance.GetLanguageForKey("club_trans_0"),//"系统消息",
                                    content = LanguageManager.mInstance.GetLanguageForKey("club_trans_1"),//"您是否同意交易模式由俱乐部交易模式变为平台交易模式",
                                    contentCommit = LanguageManager.mInstance.GetLanguageForKey("club_trans_3"),//"是",
                                    contentCancel = LanguageManager.mInstance.GetLanguageForKey("club_trans_4"),//否,
                                    actionCommit = ()=>{

                                        ChangeChargesType(1);
                                    }
                                });
                    
                }
            };

            UIEventListener.Get(rc.Get<GameObject>(UIClubAssitent.trans_btn_own)).onClick = (go) =>
            {
                //Log.Debug("俱乐部充值");
                if (chargesType != 0) {

                    UIComponent.Instance.ShowNoAnimation(UIType.UIDialog,
                                new UIDialogComponent.DialogData()
                                {
                                    type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                                    title = LanguageManager.mInstance.GetLanguageForKey("club_trans_0"),//"系统消息",
                                    content = LanguageManager.mInstance.GetLanguageForKey("club_trans_2"),//"您是否同意交易模式由平台交易模式变为俱乐部交易模式",
                                    contentCommit = LanguageManager.mInstance.GetLanguageForKey("club_trans_3"),//"是",
                                    contentCancel = LanguageManager.mInstance.GetLanguageForKey("club_trans_4"),//否,
                                    actionCommit = () => {

                                        ChangeChargesType(0);
                                    }
                                });
                }
            };

            UITrManager.Instance.Append(UIComponent.Instance.Get(UIType.UIClub_Info).GameObject, rc.gameObject);
        }

        public override void OnHide()
        {
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

        void ChangeChargesType(int type) {

            WEB_club_mod_trans.RequestData modtrans_req = new WEB_club_mod_trans.RequestData()
            {
                transType = type
            };

            HttpRequestComponent.Instance.Send(WEB_club_mod_trans.API, WEB_club_mod_trans.Request(modtrans_req), (resData) => {

                WEB_club_mod_trans.ResponseData modtrans_res = WEB_club_mod_trans.Response(resData);
                if (modtrans_res.status != 0)
                {

                    Log.Debug($"error WEB_club_list status = {modtrans_res.status} meg = {modtrans_res.msg}");
                    UIComponent.Instance.Toast(modtrans_res.status);
                    return;
                }
                else
                {
                    Log.Debug("修改成功");
                    LanguageManager.mInstance.GetLanguageForKey("club_info_8");//修改成功
                    chargesType = type;
                    UpdateChargesType(chargesType);
                    Game.EventSystem.Run(UIClubEventIdType.CLUB_INFO, iClubId);
                }
            });
        }

        void UpdateChargesType(int type) {

            if (0 == type)
            {

                rc.Get<GameObject>(UIClubAssitent.trans_btn_thrid).GetComponent<Image>().sprite = rc.Get<Sprite>("images_radio_normal");
                rc.Get<GameObject>(UIClubAssitent.trans_btn_own).GetComponent<Image>().sprite = rc.Get<Sprite>("images_radio_on");
            }
            else
            {
                rc.Get<GameObject>(UIClubAssitent.trans_btn_thrid).GetComponent<Image>().sprite = rc.Get<Sprite>("images_radio_on");
                rc.Get<GameObject>(UIClubAssitent.trans_btn_own).GetComponent<Image>().sprite = rc.Get<Sprite>("images_radio_normal");
            }
        }
    }
}
