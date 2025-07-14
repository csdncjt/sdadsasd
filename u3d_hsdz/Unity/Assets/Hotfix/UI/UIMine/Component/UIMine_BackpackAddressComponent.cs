using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using xcharts;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIMine_BackpackAddressComponentSystem : AwakeSystem<UIMine_BackpackAddressComponent>
    {
        public override void Awake(UIMine_BackpackAddressComponent self)
        {
            self.Awake();
        }
    }

    public class UIMine_BackpackAddressComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private InputField address_name_input;
        private InputField address_phone_input;
        private InputField address_wx_input;
        private InputField address_Receiving_input;

        public sealed class BackpackAddressData
        {
            public int backpackId;
            public WEB2_backpack_consignee_info.Data addressData;
            public BackpackAddressDelegate successDelegate;
        }
        public delegate void BackpackAddressDelegate();

        private BackpackAddressData mSealedData;

        public void Awake()
        {
            InitUI();
        }
 
        public override void OnShow(object obj)
        {
            if (obj != null && obj is BackpackAddressData) //背包使用
            {
                mSealedData = obj as BackpackAddressData;
                if (mSealedData.addressData != null)
                {
                    var addressData = mSealedData.addressData;
                    SetInputData(addressData);
                }
                else
                {
                    SetInputNull();
                }
            }
        }

        void SetInputData(WEB2_backpack_consignee_info.Data tData)
        {
            address_name_input.text = tData.userName;
            address_phone_input.text = tData.mobile;
            address_Receiving_input.text = tData.address;
            address_wx_input.text = tData.wechat;
        }

        void SetInputNull()
        {
            address_name_input.text = "";
            address_phone_input.text = "";
            address_Receiving_input.text = "";
            address_wx_input.text = "";
        }


        private void UpdateAddress_Req()
        {
            if (string.IsNullOrEmpty(address_Receiving_input.text) || string.IsNullOrEmpty(address_name_input.text) ||
                string.IsNullOrEmpty(address_phone_input.text) || string.IsNullOrEmpty(address_wx_input.text))
            {
                UIComponent.Instance.Toast(LanguageManager.Get("adaptation10197"));//输入框不能为空
                return;
            }
            if (IsPhoneNumber(address_phone_input.text) == false)
            {
                UIComponent.Instance.Toast(LanguageManager.Get("UIMine_BackPack_inputrightphone"));//请输入正确的手机号码
                return;
            }
            if (mSealedData == null)
            {
                Log.Error(LanguageManager.Get("UIMine_BackPack_dataerr"));//数据出错
            }

            if (mSealedData.backpackId <= 0)//小等于0 修改地地址
            {
                APISaveAddress(pDto =>
                {
                    if (pDto.status == 0)
                    {
                        UIComponent.Instance.RemoveAnimated(UIType.UIMine_BackpackAddress);
                        UIComponent.Instance.Toast(LanguageManager.Get("adaptation10198"));//成功修改地址
                    }
                    else
                    {
                        UIComponent.Instance.Toast("失败");
                    }
                });
            }
            else
            {
                APIApplyPrize(mSealedData.backpackId, pDto =>
                {
                    if (pDto.status == 0)
                    {
                        UIComponent.Instance.RemoveAnimated(UIType.UIMine_BackpackAddress);
                        UIComponent.Instance.Toast(LanguageManager.Get("adaptation10199"));//兑换成功
                        mSealedData.successDelegate();
                    }
                    else
                    {
                        UIComponent.Instance.Toast("失败");
                    }
                });
            }
        }

        /// <summary>        /// 对物品的使用        /// </summary>
        void APIApplyPrize(int pId, Action<WEB2_backpack_goods_apply_prize.ResponseData> pDto)
        {
            var tReq = new WEB2_backpack_goods_apply_prize.RequestData()
            {
                id = mSealedData.backpackId
            };
            HttpRequestComponent.Instance.Send(WEB2_backpack_goods_apply_prize.API, WEB2_backpack_goods_apply_prize.Request(tReq), (resData) =>
            {
                var tRes = WEB2_backpack_goods_apply_prize.Response(resData);
                if (pDto != null)
                    pDto(tRes);
            });
        }

        /// <summary>        /// 保存收货地址        /// </summary>
        void APISaveAddress(Action<WEB2_backpack_consignee_save.ResponseData> pDto)
        {
            var tReq = new WEB2_backpack_consignee_save.RequestData()
            {
                mobile = address_phone_input.text,
                wechat = address_wx_input.text,
                address = address_Receiving_input.text,
                userName = address_name_input.text
            };
            HttpRequestComponent.Instance.Send(WEB2_backpack_consignee_save.API, WEB2_backpack_consignee_save.Request(tReq), (resData) =>
           {
               var tRes = WEB2_backpack_consignee_save.Response(resData);
               if (pDto != null)
                   pDto(tRes);
           });
        }

        public bool IsPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^1(3[0-9]|5[0-9]|7[6-8]|8[0-9])[0-9]{8}$");
        }


        public override void OnHide()
        {

        }
        public override void Dispose()
        {
            base.Dispose();
        }

        #region InitUI
        protected virtual void InitUI()
        {
            rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            UIEventListener.Get(rc.Get<GameObject>("address_close").gameObject).onClick = (go) =>
            {
                Game.Scene.GetComponent<UIComponent>().RemoveAnimated(UIType.UIMine_BackpackAddress);
            };
            UIEventListener.Get(rc.Get<GameObject>("address_confirm").gameObject).onClick = (go) =>
            {
                UpdateAddress_Req();
            };
            address_name_input = rc.Get<GameObject>("address_name_input").GetComponent<InputField>();
            address_phone_input = rc.Get<GameObject>("address_phone_input").GetComponent<InputField>();
            address_wx_input = rc.Get<GameObject>("address_wx_input").GetComponent<InputField>();
            address_Receiving_input = rc.Get<GameObject>("address_Receiving_input").GetComponent<InputField>();
        }
        #endregion

    }
}

/****
 *  var tReq = new WEB_mtt_add_consignee.RequestData()
                {
                    mobile = address_phone_input.text,
                    wechat = address_wx_input.text,
                    address = address_Receiving_input.text,
                    name = address_name_input.text
                };
                HttpRequestComponent.Instance.Send(WEB_mtt_add_consignee.API, WEB_mtt_add_consignee.Request(tReq), async (resData) =>
                {
                    var tRes = WEB_mtt_add_consignee.Response(resData);
                    Game.Scene.GetComponent<UIComponent>().RemoveAnimated(UIType.UIMine_BackpackAddress);
                    Log.Debug(tRes.status == 0 ? "成功修改地址" : "失败");
                    UIComponent.Instance.Toast(tRes.status == 0 ? "修改地址成功" : "失败");
                });


       var tReq = new WEB_mtt_apply_prize.RequestData()
                {
                    id = mSealedData.backpackId
                };
                HttpRequestComponent.Instance.Send(WEB_mtt_apply_prize.API, WEB_mtt_apply_prize.Request(tReq), async (resData) =>
                {
                    var tRes = WEB_mtt_apply_prize.Response(resData);
                    Game.Scene.GetComponent<UIComponent>().RemoveAnimated(UIType.UIMine_BackpackAddress);
                    UIComponent.Instance.Toast(tRes.status == 0 ? "兑换成功" : "失败");
                    mSealedData.successDelegate();
                });

 * */
