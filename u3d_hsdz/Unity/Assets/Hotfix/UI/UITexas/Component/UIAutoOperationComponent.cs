using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIAutoOperationComponentSystem : AwakeSystem<UIAutoOperationComponent>
    {
        public override void Awake(UIAutoOperationComponent self)
        {
            self.Awake();
        }
    }

    public class UIAutoOperationComponent : UIBaseComponent
    {
        public sealed class AutoOperationData
        {
            public int callAmount { get; set; }  // 下一操作人跟注额
        }

        private ReferenceCollector rc;
        private Toggle toggleAutoFold;
        private Toggle toggleAutoCall;
        private Toggle toggleAutoAllin;
        private Toggle toggleAutoCheck;
        private Text textAutoCall;

        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            toggleAutoFold = rc.Get<GameObject>("Toggle_AutoFold").GetComponent<Toggle>();
            toggleAutoCall = rc.Get<GameObject>("Toggle_AutoCall").GetComponent<Toggle>();
            toggleAutoAllin = rc.Get<GameObject>("Toggle_AutoAllin").GetComponent<Toggle>();
            toggleAutoCheck = rc.Get<GameObject>("Toggle_AutoCheck").GetComponent<Toggle>();
            textAutoCall = rc.Get<GameObject>("Text_AutoCall").GetComponent<Text>();

            toggleAutoFold.onValueChanged.AddListener(onValueChangeAutoFold);
            toggleAutoCall.onValueChanged.AddListener(onValueChangeAutoCall);
            toggleAutoAllin.onValueChanged.AddListener(onValueChangeAutoAllin);
            toggleAutoCheck.onValueChanged.AddListener(onValueChangeAutoCheck);
        }

        public override void OnShow(object obj)
        {
            AutoOperationData autoOperationData = obj as AutoOperationData;
            if (null == autoOperationData)
                return;

            RoomPath enumRoomPath = (RoomPath)GameCache.Instance.room_path;
            if (enumRoomPath == RoomPath.NormalAof || enumRoomPath == RoomPath.OmahaAof || enumRoomPath == RoomPath.DPAof)
            {
                //必下场，只显示Allin和Fold
                autoOperationData.callAmount = GameCache.Instance.CurGame.MainPlayer.chips;
            }

            toggleAutoFold.isOn = GameCache.Instance.CurGame.autoFold;
            toggleAutoCall.isOn = GameCache.Instance.CurGame.autoCall;
            toggleAutoAllin.isOn = GameCache.Instance.CurGame.autoAllin;
            toggleAutoCheck.isOn = GameCache.Instance.CurGame.autoCheck;

            if (autoOperationData.callAmount == 0)
            {
                toggleAutoCheck.gameObject.SetActive(true);
                toggleAutoCall.gameObject.SetActive(false);
                toggleAutoAllin.gameObject.SetActive(false);
            }
            else if (autoOperationData.callAmount < GameCache.Instance.CurGame.MainPlayer.chips)
            {
                toggleAutoCheck.gameObject.SetActive(false);
                toggleAutoCall.gameObject.SetActive(true);
                toggleAutoAllin.gameObject.SetActive(false);
                textAutoCall.text = StringHelper.ShowGold(autoOperationData.callAmount);
            }else if (autoOperationData.callAmount >= GameCache.Instance.CurGame.MainPlayer.chips)
            {
                toggleAutoCheck.gameObject.SetActive(false);
                toggleAutoCall.gameObject.SetActive(false);
                toggleAutoAllin.gameObject.SetActive(true);
            }
        }

        public override void OnHide()
        {
            
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();
        }

        private void onValueChangeAutoFold(bool arg0)
        {
            GameCache.Instance.CurGame.autoFold = arg0;
            if (GameCache.Instance.CurGame.autoFold)
            {
                toggleAutoCheck.isOn = false;
                toggleAutoCall.isOn = false;
                toggleAutoAllin.isOn = false;
            }
        }

        private void onValueChangeAutoCheck(bool arg0)
        {
            GameCache.Instance.CurGame.autoCheck = arg0;
            if (GameCache.Instance.CurGame.autoCheck)
            {
                toggleAutoFold.isOn = false;
                toggleAutoCall.isOn = false;
                toggleAutoAllin.isOn = false;
            }
        }

        private void onValueChangeAutoCall(bool arg0)
        {
            GameCache.Instance.CurGame.autoCall = arg0;
            if (GameCache.Instance.CurGame.autoCall)
            {
                toggleAutoFold.isOn = false;
                toggleAutoCheck.isOn = false;
                toggleAutoAllin.isOn = false;
            }
        }

        private void onValueChangeAutoAllin(bool arg0)
        {
            GameCache.Instance.CurGame.autoAllin = arg0;
            if (GameCache.Instance.CurGame.autoAllin)
            {
                toggleAutoFold.isOn = false;
                toggleAutoCheck.isOn = false;
                toggleAutoCall.isOn = false;
            }
        }
    }
}
