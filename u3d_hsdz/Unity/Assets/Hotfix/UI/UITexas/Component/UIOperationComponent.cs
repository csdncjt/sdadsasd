using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIOperationComponentSystem : AwakeSystem<UIOperationComponent>
    {
        public override void Awake(UIOperationComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UIOperationComponentUpdate : UpdateSystem<UIOperationComponent>
    {
        public override void Update(UIOperationComponent self)
        {
            self.Update();
        }
    }

    public class UIOperationComponent : UIBaseComponent
    {
        public sealed class OperationData
        {
            public bool firstRound { get; set; }  // 是否第一轮操作
            public int minAnteNum { get; set; }  // 当前最小可加注额，操作按钮上的加注额要用到
            public int canRaise { get; set; }  // 下一操作人右按钮是否加注
            public int callAmount { get; set; }  // 下一操作人跟注额
        }

        private ReferenceCollector rc;
        private Button buttonAllin;
        private Button buttonCall;
        private Button buttonCall0;
        private Button buttonCall1;
        private Button buttonCall2;
        private Button buttonCallLeft;
        private Button buttonCallRight;
        private Button buttonCheck;
        private Button buttonFold;
        private Button buttonFreeCall;
        private Button buttonFreeCallConfirm;
        private Button buttonNum0;
        private Button buttonNum1;
        private Button buttonNum2;
        private Button buttonNum3;
        private Button buttonNum4;
        private Button buttonNum5;
        private Button buttonNum6;
        private Button buttonNum7;
        private Button buttonNum8;
        private Button buttonNum9;
        private Button buttonPreciseCall;
        private Button buttonPreciseCallCloseBottom;
        private Button buttonPreciseCallClose;
        private Button buttonPreciseCallConfirm;
        private Button buttonPreciseCallDelete;
        private Image imageCheckCountDown;
        private Image imageFoldCountDown;
        private Image imageFreeCallMask;
        private Transform transPreciseCall;
        private Slider sliderFreeCall;
        private Slider sliderInvisible;
        private Button buttonSliderHandle;
        private Button buttonInvisibleHandle;
        private Text textCall;
        private Text textCallPot0;
        private Text textCallPot1;
        private Text textCallPot2;
        private Text textCallPotValue0;
        private Text textCallPotValue1;
        private Text textCallPotValue2;
        private Text textCallPotLeft;
        private Text textCallPotValueLeft;
        private Text textCallPotRight;
        private Text textCallPotValueRight;
        private Text textCallTitle0;
        private Text textCallTitle1;
        private Text textCallTitle2;
        private Text textCallTitleLeft;
        private Text textCallTitleRight;
        private Text textFreeCall;
        private Text textFreeCallMax;
        private Text textPreciseCallMinAnteNum;

        private OperationData operationData;

        private int callValue0;
        private int callValue1;
        private int callValue2;
        private int callValueLeft;
        private int callValueRight;

        private int maxBet;

        private StringBuilder sbPreciseCallValue;
        private int callValue;
        private float optCurTime;
        private float optTotalTime;
        private bool isCountDown;
        private bool hadAlertSound;

        private bool isShowingDialog;

        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            buttonAllin = rc.Get<GameObject>("Button_Allin").GetComponent<Button>();
            buttonCall = rc.Get<GameObject>("Button_Call").GetComponent<Button>();
            buttonCall0 = rc.Get<GameObject>("Button_Call0").GetComponent<Button>();
            buttonCall1 = rc.Get<GameObject>("Button_Call1").GetComponent<Button>();
            buttonCall2 = rc.Get<GameObject>("Button_Call2").GetComponent<Button>();
            buttonCallLeft = rc.Get<GameObject>("Button_Call_left").GetComponent<Button>();
            buttonCallRight = rc.Get<GameObject>("Button_Call_right").GetComponent<Button>();
            buttonCheck = rc.Get<GameObject>("Button_Check").GetComponent<Button>();
            buttonFold = rc.Get<GameObject>("Button_Fold").GetComponent<Button>();
            buttonFreeCall = rc.Get<GameObject>("Button_FreeCall").GetComponent<Button>();
            buttonFreeCallConfirm = rc.Get<GameObject>("Button_FreeCall_Confirm").GetComponent<Button>();
            buttonNum0 = rc.Get<GameObject>("Button_Num0").GetComponent<Button>();
            buttonNum1 = rc.Get<GameObject>("Button_Num1").GetComponent<Button>();
            buttonNum2 = rc.Get<GameObject>("Button_Num2").GetComponent<Button>();
            buttonNum3 = rc.Get<GameObject>("Button_Num3").GetComponent<Button>();
            buttonNum4 = rc.Get<GameObject>("Button_Num4").GetComponent<Button>();
            buttonNum5 = rc.Get<GameObject>("Button_Num5").GetComponent<Button>();
            buttonNum6 = rc.Get<GameObject>("Button_Num6").GetComponent<Button>();
            buttonNum7 = rc.Get<GameObject>("Button_Num7").GetComponent<Button>();
            buttonNum8 = rc.Get<GameObject>("Button_Num8").GetComponent<Button>();
            buttonNum9 = rc.Get<GameObject>("Button_Num9").GetComponent<Button>();
            buttonPreciseCall = rc.Get<GameObject>("Button_PreciseCall").GetComponent<Button>();
            buttonPreciseCallClose = rc.Get<GameObject>("Button_PreciseCallClose").GetComponent<Button>();
            buttonPreciseCallCloseBottom = rc.Get<GameObject>("Button_PreciseCall_Close").GetComponent<Button>();
            buttonPreciseCallConfirm = rc.Get<GameObject>("Button_PreciseCall_Confirm").GetComponent<Button>();
            buttonPreciseCallDelete = rc.Get<GameObject>("Button_PreciseCallDelete").GetComponent<Button>();
            imageCheckCountDown = rc.Get<GameObject>("Image_CheckCountDown").GetComponent<Image>();
            imageFoldCountDown = rc.Get<GameObject>("Image_FoldCountDown").GetComponent<Image>();
            imageFreeCallMask = rc.Get<GameObject>("Image_FreeCallMask").GetComponent<Image>();
            transPreciseCall = rc.Get<GameObject>("PreciseCall").transform;
            sliderFreeCall = rc.Get<GameObject>("Slider_FreeCall").GetComponent<Slider>();
            sliderInvisible = rc.Get<GameObject>("Slider_Invisible").GetComponent<Slider>();
            buttonSliderHandle = rc.Get<GameObject>("Button_Handle").GetComponent<Button>();
            buttonInvisibleHandle = rc.Get<GameObject>("Button_Invisible").GetComponent<Button>();
            textCall = rc.Get<GameObject>("Text_Call").GetComponent<Text>();
            textCallPot0 = rc.Get<GameObject>("Text_CallPot0").GetComponent<Text>();
            textCallPot1 = rc.Get<GameObject>("Text_CallPot1").GetComponent<Text>();
            textCallPot2 = rc.Get<GameObject>("Text_CallPot2").GetComponent<Text>();
            textCallPotLeft = rc.Get<GameObject>("Text_CallPotLeft").GetComponent<Text>();
            textCallPotRight = rc.Get<GameObject>("Text_CallPotRight").GetComponent<Text>();
            textCallPotValue0 = rc.Get<GameObject>("Text_CallPotValue0").GetComponent<Text>();
            textCallPotValue1 = rc.Get<GameObject>("Text_CallPotValue1").GetComponent<Text>();
            textCallPotValue2 = rc.Get<GameObject>("Text_CallPotValue2").GetComponent<Text>();
            textCallPotValueLeft = rc.Get<GameObject>("Text_CallPotValueLeft").GetComponent<Text>();
            textCallPotValueRight = rc.Get<GameObject>("Text_CallPotValueRight").GetComponent<Text>();


            textCallTitle0 = rc.Get<GameObject>("Text_CallTitle0").GetComponent<Text>();
            textCallTitle1 = rc.Get<GameObject>("Text_CallTitle1").GetComponent<Text>();
            textCallTitle2 = rc.Get<GameObject>("Text_CallTitle2").GetComponent<Text>();
            textCallTitleLeft = rc.Get<GameObject>("Text_CallTitleLeft").GetComponent<Text>();
            textCallTitleRight = rc.Get<GameObject>("Text_CallTitleRight").GetComponent<Text>();
            textFreeCall = rc.Get<GameObject>("Text_FreeCall").GetComponent<Text>();
            textFreeCallMax = rc.Get<GameObject>("Text_FreeCall_Max").GetComponent<Text>();
            textPreciseCallMinAnteNum = rc.Get<GameObject>("Text_PreciseCallMinAnteNum").GetComponent<Text>();


            UIEventListener.Get(buttonCall.gameObject).onClick = onClickCall;
            UIEventListener.Get(buttonCheck.gameObject).onClick = onClickCheck;
            UIEventListener.Get(buttonCall0.gameObject).onClick = onClickCall0;
            UIEventListener.Get(buttonCall1.gameObject).onClick = onClickCall1;
            UIEventListener.Get(buttonCall2.gameObject).onClick = onClickCall2;
            UIEventListener.Get(buttonCallLeft.gameObject).onClick = onClickCallLeft;
            UIEventListener.Get(buttonCallRight.gameObject).onClick = onClickCallRight;
            UIEventListener.Get(buttonAllin.gameObject).onClick = onClickAllin;
            UIEventListener.Get(buttonFold.gameObject).onClick = onClickFold;
            UIEventListener.Get(buttonInvisibleHandle.gameObject).onDown = onClickFreeCall;
            UIEventListener.Get(buttonFreeCall.gameObject).onClick = onClickFreeCall;
            UIEventListener.Get(buttonInvisibleHandle.gameObject).onUp = onUpInvisibleBtn;
            UIEventListener.Get(buttonFreeCallConfirm.gameObject).onClick = onClickSliderHandle;
            UIEventListener.Get(buttonPreciseCall.gameObject).onClick = onClickPreciseCall;
            UIEventListener.Get(buttonNum1.gameObject).onClick = onClickNum;
            UIEventListener.Get(buttonNum2.gameObject).onClick = onClickNum;
            UIEventListener.Get(buttonNum3.gameObject).onClick = onClickNum;
            UIEventListener.Get(buttonNum4.gameObject).onClick = onClickNum;
            UIEventListener.Get(buttonNum5.gameObject).onClick = onClickNum;
            UIEventListener.Get(buttonNum6.gameObject).onClick = onClickNum;
            UIEventListener.Get(buttonNum7.gameObject).onClick = onClickNum;
            UIEventListener.Get(buttonNum8.gameObject).onClick = onClickNum;
            UIEventListener.Get(buttonNum9.gameObject).onClick = onClickNum;
            UIEventListener.Get(buttonNum0.gameObject).onClick = onClickNum;
            UIEventListener.Get(buttonPreciseCallDelete.gameObject).onClick = onClickPreciseCallDelete;
            UIEventListener.Get(buttonPreciseCallClose.gameObject).onClick = onClickPreciseCallClose;
            UIEventListener.Get(buttonPreciseCallCloseBottom.gameObject).onClick = onClickPreciseCallClose;
            UIEventListener.Get(buttonPreciseCallConfirm.gameObject).onClick = onClickPreciseCallConfirm;
            UIEventListener.Get(imageFreeCallMask.gameObject).onClick = onClickFreeCallMask;
            UIEventListener.Get(buttonSliderHandle.gameObject).onClick = onClickSliderHandle;

            sliderFreeCall.onValueChanged.AddListener(onValueChangeFreeCall);
            sliderInvisible.onValueChanged.AddListener(onValueChangeInvisible);

            registerHandler();
        }

        private void onClickFreeCallMask(GameObject go)
        {
            imageFreeCallMask.gameObject.SetActive(false);
            //隐藏自由加注
            showFreeCall(false);
            //隐藏精确加注
            ShowPreciseCall(false);
        }

        private void onValueChangeInvisible(float arg0)
        {
            if (sliderFreeCall.gameObject.activeInHierarchy)
            {
                sliderFreeCall.value = arg0;
            }
        }

        private void onValueChangeFreeCall(float arg0)
        {
            if (arg0 >= sliderFreeCall.maxValue)
            {
                textFreeCall.text = "ALL IN";
                textFreeCall.color = new Color32(255, 184, 91, 255);
                buttonSliderHandle.gameObject.GetComponent<Image>().sprite = rc.Get<Sprite>("game_operation_button_raise_allin_normal");
            }
            else
            {
                textFreeCall.text = $"{StringHelper.ShowGold(Convert.ToInt32(arg0 * 100))}";
                textFreeCall.color = new Color32(38, 27, 27, 255);
                buttonSliderHandle.gameObject.GetComponent<Image>().sprite = rc.Get<Sprite>("game_operation_button_raise_raise_normal");
            }

        }

        private void onClickPreciseCallClose(GameObject go)
        {
            // Debug.Log($"close===close");
            ShowPreciseCall(false);
        }

        private void onClickPreciseCallDelete(GameObject go)
        {
            if (sbPreciseCallValue.Length < 1)
                return;

            sbPreciseCallValue.Remove(sbPreciseCallValue.Length - 1, 1);
            // textPreciseCallMinAnteNum.text = sbPreciseCallValue.Length == 0 ? $"最小加注金额: {operationData.minAnteNum * 2}" : $"{sbPreciseCallValue}";
            textPreciseCallMinAnteNum.text = sbPreciseCallValue.Length == 0 ? CPErrorCode.LanguageDescription(20036, new List<object>(){ StringHelper.ShowGold(operationData.minAnteNum * 2)}) : $"{sbPreciseCallValue}";
            updatePreciseCallConfirmButton();
        }

        private void onClickNum(GameObject go)
        {
            string mStr = go.name.Substring(go.name.Length - 1);
            if (sbPreciseCallValue.Length == 0 && mStr.Equals("0"))
                return;
            sbPreciseCallValue.Append(mStr);
            textPreciseCallMinAnteNum.text = $"{sbPreciseCallValue}";
            updatePreciseCallConfirmButton();
        }

        private void updatePreciseCallConfirmButton()
        {
            textPreciseCallMinAnteNum.color = new Color32(233, 191, 128, 255);
            if (sbPreciseCallValue.Length > 0)
            {
                buttonPreciseCallConfirm.gameObject.SetActive(true);
                buttonPreciseCallCloseBottom.gameObject.SetActive(false);
                if (Convert.ToInt32(sbPreciseCallValue.ToString()) * 100 >= maxBet)
                {
                    if (maxBet >= GameCache.Instance.CurGame.MainPlayer.chips)
                    {
                        buttonPreciseCallConfirm.gameObject.transform.Find("Text").GetComponent<Text>().text = "ALL IN";
                    }
                    else
                    {
                        buttonPreciseCallConfirm.gameObject.transform.Find("Text").GetComponent<Text>().text = CPErrorCode.LanguageDescription(10012);
                    }
                    textPreciseCallMinAnteNum.text = $"{StringHelper.ShowGold(maxBet)}";
                    sbPreciseCallValue.Clear();
                    sbPreciseCallValue.Append($"{maxBet}");

                    textPreciseCallMinAnteNum.color = new Color32(184, 43, 48, 255);
                }
                else
                {

                    // buttonPreciseCallConfirm.gameObject.transform.Find("Text").GetComponent<Text>().text = "确定";
                    buttonPreciseCallConfirm.gameObject.transform.Find("Text").GetComponent<Text>().text = CPErrorCode.LanguageDescription(10012);
                }
            }
            else
            {
                buttonPreciseCallConfirm.gameObject.SetActive(false);
                buttonPreciseCallCloseBottom.gameObject.SetActive(true);
            }
        }

        private void onClickPreciseCallConfirm(GameObject go)
        {
            if (Convert.ToInt32(sbPreciseCallValue.ToString()) * 100 < operationData.minAnteNum * 2)
            {
                // UIComponent.Instance.Toast($"最小加注金额: {operationData.minAnteNum * 2}");
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(20036, new List<object>(){ StringHelper.ShowGold(operationData.minAnteNum * 2)}));
                return;
            }

            if (int.TryParse(sbPreciseCallValue.ToString(), out callValue))
            {
                callValue = callValue * 100;
                CheckOpt();
            }
            else
            {
                // UIComponent.Instance.Toast("Error");
                UIComponent.Instance.Toast(CPErrorCode.LanguageDescription(10052));
                sbPreciseCallValue.Clear();
            }

            ShowPreciseCall(false);
        }

        private void onClickPreciseCall(GameObject go)
        {
            ShowPreciseCall(true);
        }

        private void onUpInvisibleBtn(GameObject go)
        {
            //sliderInvisible.gameObject.SetActive(false);
        }

        private void onClickFreeCall(GameObject go)
        {
            showFreeCall(true);
        }

        private void ShowPreciseCall(bool show)
        {
            if (show)
            {
                transPreciseCall.gameObject.SetActive(true);
                buttonPreciseCallCloseBottom.gameObject.SetActive(true);
                buttonFreeCall.gameObject.SetActive(false);
                buttonPreciseCallConfirm.gameObject.SetActive(false);

                // textPreciseCallMinAnteNum.text = $"最小加注金额: {operationData.minAnteNum * 2}";
                textPreciseCallMinAnteNum.text = CPErrorCode.LanguageDescription(20036, new List<object>() { StringHelper.ShowGold(operationData.minAnteNum * 2) });
                if (null == sbPreciseCallValue)
                    sbPreciseCallValue = new StringBuilder();
                else
                    sbPreciseCallValue.Clear();
            }
            else
            {
                transPreciseCall.gameObject.SetActive(false);
                buttonPreciseCallCloseBottom.gameObject.SetActive(false);
                buttonFreeCall.gameObject.SetActive(true);
                buttonPreciseCallConfirm.gameObject.SetActive(false);
            }

        }

        //点击自由加注滑块按钮
        private void onClickSliderHandle(GameObject go)
        {
            callValue = Convert.ToInt32(sliderFreeCall.value * 100);
            if (sliderFreeCall.maxValue - sliderFreeCall.value <= 1)
            {
                callValue = GameCache.Instance.CurGame.MainPlayer.chips;
            }
            CheckOpt();

            showFreeCall(false);
        }

        private void showFreeCall(bool show)
        {
            sliderFreeCall.value = sliderFreeCall.minValue;
            sliderInvisible.value = sliderInvisible.minValue;
            if (show)
            {
                imageFreeCallMask.gameObject.SetActive(true);
                sliderFreeCall.gameObject.SetActive(true);
                buttonFreeCallConfirm.gameObject.SetActive(true);
                buttonFreeCall.gameObject.SetActive(false);
                buttonCall0.gameObject.SetActive(false);
                buttonCall1.gameObject.SetActive(false);
                buttonCall2.gameObject.SetActive(false);
                buttonCallLeft.gameObject.SetActive(false);
                buttonCallRight.gameObject.SetActive(false);
                buttonPreciseCall.gameObject.SetActive(false);
            }
            else
            {
                imageFreeCallMask.gameObject.SetActive(false);
                sliderFreeCall.gameObject.SetActive(false);
                buttonFreeCallConfirm.gameObject.SetActive(false);
                buttonFreeCall.gameObject.SetActive(true);
                buttonCall0.gameObject.SetActive(true);
                buttonCall1.gameObject.SetActive(true);
                buttonCall2.gameObject.SetActive(true);
                buttonCallLeft.gameObject.SetActive(callValueLeft > 0);
                buttonCallRight.gameObject.SetActive(callValueRight > 0);
                buttonPreciseCall.gameObject.SetActive(true);

                sliderInvisible.gameObject.SetActive(true);
            }
        }

        private void onClickCall2(GameObject go)
        {
            if (buttonCall2.interactable == false)
                return;

            callValue = callValue2;
            CheckOpt();
        }

        private void onClickCall1(GameObject go)
        {
            if (buttonCall1.interactable == false)
                return;

            callValue = callValue1;
            CheckOpt();
        }

        private void onClickCall0(GameObject go)
        {
            if (buttonCall0.interactable == false)
                return;

            callValue = callValue0;
            CheckOpt();
        }

        private void onClickCallLeft(GameObject go)
        {
            callValue = callValueLeft;
            CheckOpt();
        }

        private void onClickCallRight(GameObject go)
        {
            callValue = callValueRight;
            CheckOpt();
        }

        public void Update()
        {
            if (!isCountDown)
                return;

            if (operationData.callAmount == 0)
            {
                imageCheckCountDown.fillAmount = (optCurTime -= Time.deltaTime) / optTotalTime;

                if (imageCheckCountDown.fillAmount <= 0)
                {
                    isCountDown = false;
                    imageCheckCountDown.gameObject.SetActive(false);
                    if (isShowingDialog)
                        UIComponent.Instance.HideNoAnimation(UIType.UIDialog);
                    isShowingDialog = false;
                    GameCache.Instance.CurGame.OptAction(7, 0);
                }
            }
            else
            {
                imageFoldCountDown.fillAmount = (optCurTime -= Time.deltaTime) / optTotalTime;
                if (imageFoldCountDown.fillAmount <= 0)
                {
                    isCountDown = false;
                    imageFoldCountDown.gameObject.SetActive(false);
                    if (isShowingDialog)
                        UIComponent.Instance.HideNoAnimation(UIType.UIDialog);
                    isShowingDialog = false;
                    GameCache.Instance.CurGame.OptAction(7, 0);
                }
            }

            if (optCurTime < 5.1 && optCurTime > 5 && !hadAlertSound)
            {
                //剩余5秒音效
                SoundComponent.Instance.PlaySFX(SoundComponent.SFX_ACTION_ALERT);
                hadAlertSound = true;
            }
        }

        private void onClickAllin(GameObject go)
        {
            callValue = GameCache.Instance.CurGame.MainPlayer.chips;
            CheckOpt();
        }

        private void onClickCall(GameObject go)
        {
            callValue = operationData.callAmount;
            CheckOpt();
        }

        private void onClickCheck(GameObject go)
        {
            GameCache.Instance.CurGame.OptAction(5, 0);
            isCountDown = false;
        }

        private void onClickFold(GameObject go)
        {
            if (buttonCheck.gameObject.activeInHierarchy)
            {
                //如果可以让牌，需要弹窗询问弃牌还是让牌
                isShowingDialog = true;
                UIComponent.Instance.ShowNoAnimation(UIType.UIDialog, new UIDialogComponent.DialogData()
                {
                    type = UIDialogComponent.DialogData.DialogType.CommitCancel,
                    // title = $"确定弃牌？",
                    title = CPErrorCode.LanguageDescription(20037),
                    // content = $"你可以让牌而不需要任何USDT",
                    content = CPErrorCode.LanguageDescription(20038),
                    // contentCommit = "弃牌",
                    contentCommit = CPErrorCode.LanguageDescription(10047),
                    // contentCancel = "让牌",
                    contentCancel = CPErrorCode.LanguageDescription(10315),
                    actionCommit = () =>
                    {
                        GameCache.Instance.CurGame.OptAction(6, 0);
                        isCountDown = false;
                    },
                    actionCancel = () =>
                    {
                        GameCache.Instance.CurGame.OptAction(5, 0);
                        isCountDown = false;
                    }
                });
                return;
            }
            GameCache.Instance.CurGame.OptAction(6, 0);
        }

        public override void OnShow(object obj)
        {
            if (null == obj)
                return;
            operationData = obj as OperationData;
            if (null == operationData)
                return;

            if (isShowingDialog)
                UIComponent.Instance.HideNoAnimation(UIType.UIDialog);
            isShowingDialog = false;

            RoomPath enumRoomPath = (RoomPath)GameCache.Instance.room_path;

            if (enumRoomPath == RoomPath.NormalAof || enumRoomPath == RoomPath.OmahaAof || enumRoomPath == RoomPath.DPAof)
            {
                //必下场，只显示Allin和Fold
                operationData.minAnteNum = GameCache.Instance.CurGame.MainPlayer.chips;
                operationData.callAmount = GameCache.Instance.CurGame.MainPlayer.chips;
            }

            optCurTime = GameCache.Instance.CurGame.GetOpTime();
            optTotalTime = optCurTime;
            hadAlertSound = false;
            if (optTotalTime < GameCache.Instance.CurGame.opTime)
            {
                optTotalTime = GameCache.Instance.CurGame.opTime;
            }
            isCountDown = false;

            SetUpTopCallBtns();

            buttonFold.gameObject.SetActive(true);
            if (operationData.callAmount == 0)
            {
                buttonCheck.gameObject.SetActive(true);
                buttonCall.gameObject.SetActive(false);
                buttonAllin.gameObject.SetActive(false);

                imageCheckCountDown.gameObject.SetActive(true);
                imageCheckCountDown.fillAmount = 1f;
                isCountDown = true;
            }
            else if (operationData.callAmount < GameCache.Instance.CurGame.MainPlayer.chips)
            {
                buttonCheck.gameObject.SetActive(false);
                buttonCall.gameObject.SetActive(true);
                buttonAllin.gameObject.SetActive(false);

                imageFoldCountDown.gameObject.SetActive(true);
                imageFoldCountDown.fillAmount = 1f;
                isCountDown = true;

                textCall.text = StringHelper.ShowGold(operationData.callAmount, false);
            }
            else if (operationData.callAmount >= GameCache.Instance.CurGame.MainPlayer.chips)
            {
                buttonCheck.gameObject.SetActive(false);
                buttonCall.gameObject.SetActive(false);
                buttonAllin.gameObject.SetActive(true);

                imageFoldCountDown.gameObject.SetActive(true);
                imageFoldCountDown.fillAmount = 1f;
                isCountDown = true;

                textCall.text = StringHelper.ShowGold(GameCache.Instance.CurGame.MainPlayer.chips, false);
            }

            sliderInvisible.gameObject.SetActive(true);

            maxBet = GameCache.Instance.CurGame.MainPlayer.chips;
            if (enumRoomPath == RoomPath.Omaha)
            {
                // omaha 1 pot限注 没开血战MIN([self potMutiplier:1], _totalChips);
                OmahaGame mOmahaGame = GameCache.Instance.CurGame as OmahaGame;
                if (null != mOmahaGame && mOmahaGame.omahaBloody == 0)
                {
                    int mTmp = potMutiplier(1);
                    if (maxBet > mTmp)
                        maxBet = mTmp;
                }
            }
            if (operationData.minAnteNum * 2 < maxBet)
            {
                SetRaiseHidden(operationData.canRaise == 0);
            }
            else if (operationData.callAmount < maxBet)
            {
                //剩余筹码大于跟注额
                SetRaiseHidden(false);
            }
            else
            {
                //最小加注额比剩余筹码还要大，而且剩余筹码比跟注额还小，只能allin了
                SetRaiseHidden(true);
            }

            int maxV = operationData.minAnteNum * 2 < maxBet ? maxBet : operationData.minAnteNum * 2;
            int minV = operationData.minAnteNum * 2;
            sliderFreeCall.maxValue = maxV / 100.0f;
            sliderFreeCall.minValue = minV / 100.0f;
            if (GameCache.Instance.CurGame.smallBlind < 100 && GameCache.Instance.CurGame.roomMode == 0)
            {
                sliderFreeCall.wholeNumbers = false;
            }
            else
            {
                sliderFreeCall.minValue = (float)Math.Ceiling(minV / 100.0f);
                sliderFreeCall.wholeNumbers = true;
            }
            sliderFreeCall.value = sliderFreeCall.minValue;
            sliderInvisible.maxValue = sliderFreeCall.maxValue;
            sliderInvisible.minValue = sliderFreeCall.minValue;
            sliderInvisible.value = sliderFreeCall.value;

            
            textFreeCall.text = $"{StringHelper.ShowGold(Convert.ToInt32(sliderFreeCall.value * 100), false)}";
            textFreeCallMax.text = $"{StringHelper.ShowGold(Convert.ToInt32(sliderFreeCall.maxValue * 100), false)}";
        }

        //此版本不用
        //private void SetUpTopCallBtns()
        //{
        //    int totalChips = GameCache.Instance.CurGame.MainPlayer.chips;

        //    if (operationData.firstRound && !GameCache.Instance.CurGame.RaiseBeforeFlop())
        //    {
        //        int pvalue = GameCache.Instance.CurGame.bigBlind;
        //        if (GameCache.Instance.CurGame.isCurStraddle == 0)
        //        {
        //            pvalue = pvalue * 2;
        //        }
        //        callValue0 = pvalue * 2;
        //        callValue1 = pvalue * 3;
        //        callValue2 = pvalue * 4;

        //        textCallTitle0.text = "BB";
        //        textCallTitle1.text = "BB";
        //        textCallTitle2.text = "BB";

        //        textCallPot0.text = "2X";
        //        textCallPot1.text = "3X";
        //        textCallPot2.text = "4X";

        //        callValueLeft = 0;
        //        callValueRight = 0;
        //    }
        //    else
        //    {
        //        string numLeftStr = UITexasSettingComponent.GetCurQuickActionNum(0);
        //        string num0Str = UITexasSettingComponent.GetCurQuickActionNum(1);
        //        string num1Str = UITexasSettingComponent.GetCurQuickActionNum(2);
        //        string num2Str = UITexasSettingComponent.GetCurQuickActionNum(3);
        //        string numRightStr = UITexasSettingComponent.GetCurQuickActionNum(4);

        //        callValueLeft = numLeftStr == "Allin" ? totalChips : (numLeftStr == "0" ? 0 : potMutiplier(UITexasSettingComponent.GetCurQuickActionNumValue(0)));
        //        callValue0 = num0Str == "Allin" ? totalChips : potMutiplier(UITexasSettingComponent.GetCurQuickActionNumValue(1));
        //        callValue1 = num1Str == "Allin" ? totalChips : potMutiplier(UITexasSettingComponent.GetCurQuickActionNumValue(2));
        //        callValue2 = num2Str == "Allin" ? totalChips : potMutiplier(UITexasSettingComponent.GetCurQuickActionNumValue(3));
        //        callValueRight = numRightStr == "Allin" ? totalChips : (numRightStr == "0" ? 0 : potMutiplier(UITexasSettingComponent.GetCurQuickActionNumValue(4)));

        //        textCallTitle0.text = "POT";
        //        textCallTitle1.text = "POT";
        //        textCallTitle2.text = "POT";

        //        textCallPotLeft.text = UITexasSettingComponent.GetCurQuickActionNum(0);
        //        textCallPot0.text = UITexasSettingComponent.GetCurQuickActionNum(1);
        //        textCallPot1.text = UITexasSettingComponent.GetCurQuickActionNum(2);
        //        textCallPot2.text = UITexasSettingComponent.GetCurQuickActionNum(3);
        //        textCallPotRight.text = UITexasSettingComponent.GetCurQuickActionNum(4);

        //        buttonCall0.interactable = true;
        //        buttonCall1.interactable = true;
        //        buttonCall2.interactable = true;
        //        if (callValue0 < GameCache.Instance.CurGame.minAnteNum * 2)
        //        {
        //            buttonCall0.interactable = false;
        //        }

        //        if (callValue1 < GameCache.Instance.CurGame.minAnteNum * 2)
        //        {
        //            buttonCall1.interactable = false;
        //        }

        //        if (callValue2 < GameCache.Instance.CurGame.minAnteNum * 2)
        //        {
        //            buttonCall2.interactable = false;
        //        }

        //    }

        //    textCallPotValue0.text = callValue0 < totalChips ? StringHelper.GetShortString(callValue0) : "All in";
        //    textCallPotValue1.text = callValue1 < totalChips ? StringHelper.GetShortString(callValue1) : "All in";
        //    textCallPotValue2.text = callValue2 < totalChips ? StringHelper.GetShortString(callValue2) : "All in";
        //    textCallPotValueLeft.text = callValueLeft < totalChips ? StringHelper.GetShortString(callValueLeft) : "All in";
        //    textCallPotValueRight.text = callValueRight < totalChips ? StringHelper.GetShortString(callValueRight) : "All in";
        //}

        private void SetUpTopCallBtns()
        {
            //2bb,3bb,4bb,5bb,6bb
            //1 / 3，1 / 2，2 / 3，1，1.5


            int totalChips = GameCache.Instance.CurGame.MainPlayer.chips;
            int nowante = GameCache.Instance.CurGame.MainPlayer.anteNumber;
            int lastjiazhu = GameCache.Instance.CurGame.lastPlayerBet;

            //int callnum = GameCache.Instance.CurGame.callnum;
            //int foldnum = GameCache.Instance.CurGame.foldnum;

            if (GameCache.Instance.CurGame.round==0 && lastjiazhu == 0)//翻牌前，且没有人加注
            {
                int pvalue = GameCache.Instance.CurGame.bigBlind;
                int groupBet = GameCache.Instance.CurGame.GroupBetBeforeFlop();//有前注
                int anBet = GameCache.Instance.CurGame.groupBet;

                if (GameCache.Instance.room_path == 21 || GameCache.Instance.room_path == 23)
                {
                    pvalue = GameCache.Instance.CurGame.groupBet;
                }

                int xishu = 0;
                if (GameCache.Instance.CurGame.isCurStraddle == 0)
                {
                    pvalue = pvalue * 2;

                    //有straddle 但是没有ante
                    callValue0 = (int)(pvalue * 2.5) - nowante;
                    callValue1 = pvalue * 3 - nowante;
                    callValue2 = (int)(pvalue * 3.5) - nowante;
                    callValueLeft = pvalue * 2 - nowante;
                    callValueRight = pvalue * 4 - nowante;
                    
                    if (groupBet > 0)
                    {
                        //int gamenum = groupBet / anBet;

                        //if (callnum == 0)
                        //{
                        xishu = groupBet / 4;
                        //}
                        //else
                        //{
                        //    xishu = anBet * (gamenum + callnum - foldnum) / 4;
                        //}
                    }
                    //else
                    //{
                    //    xishu = GameCache.Instance.CurGame.smallBlind * (callnum - foldnum) / 4;
                    //}

                }
                else
                {
                    callValue0 = pvalue * 3 - nowante;
                    callValue1 = pvalue * 4 - nowante;
                    callValue2 = pvalue * 5 - nowante;
                    callValueLeft = pvalue * 2 - nowante;
                    callValueRight = pvalue * 6 - nowante;

                    if (groupBet > 0)
                    {
                        xishu = groupBet / 4;
                    }
                    //else
                    //{
                    //    xishu = GameCache.Instance.CurGame.smallBlind * (callnum - foldnum) / 4;
                    //}
                }

                if (GameCache.Instance.room_path == 21 || GameCache.Instance.room_path == 23)
                {
                    xishu = 0;
                }

                callValue0 = callValue0 + xishu;
                callValue1 = callValue1 + xishu;
                callValue2 = callValue2 + xishu;
                callValueLeft = callValueLeft + xishu;
                callValueRight = callValueRight + xishu;

                textCallTitle0.text = "BB";
                textCallTitle1.text = "BB";
                textCallTitle2.text = "BB";
                textCallTitleLeft.text = "BB";
                textCallTitleRight.text = "BB";

                textCallPotLeft.text = "2X";
                textCallPotRight.text = "6X";
                textCallPot0.text = "3X";
                textCallPot1.text = "4X";
                textCallPot2.text = "5X";
            }
            else
            {
                int chae = GameCache.Instance.CurGame.firstBet;
                
                int lastxiazhu = GameCache.Instance.CurGame.callAmount;
                string numLeftStr = UITexasSettingComponent.GetCurQuickActionNum(0);
                string num0Str = UITexasSettingComponent.GetCurQuickActionNum(1);
                string num1Str = UITexasSettingComponent.GetCurQuickActionNum(2);
                string num2Str = UITexasSettingComponent.GetCurQuickActionNum(3);
                string numRightStr = UITexasSettingComponent.GetCurQuickActionNum(4);

                if (lastjiazhu > 0)
                {
                    callValueLeft = potMutiplier2(numLeftStr);
                    callValue0 = potMutiplier2(num0Str);
                    callValue1 = potMutiplier2(num1Str);
                    callValue2 = potMutiplier2(num2Str);
                    callValueRight = potMutiplier2(numRightStr);
                }
                else if (lastjiazhu == 0)
                {
                    callValueLeft = potMutiplier3(numLeftStr);
                    callValue0 = potMutiplier3(num0Str);
                    callValue1 = potMutiplier3(num1Str);
                    callValue2 = potMutiplier3(num2Str);
                    callValueRight = potMutiplier3(numRightStr);
                }
                else
                {
                    callValueLeft = numLeftStr == "Allin" ? totalChips : (numLeftStr == "0" ? 0 : potMutiplier(UITexasSettingComponent.GetCurQuickActionNumValue(0)));
                    callValue0 = num0Str == "Allin" ? totalChips : potMutiplier(UITexasSettingComponent.GetCurQuickActionNumValue(1));
                    callValue1 = num1Str == "Allin" ? totalChips : potMutiplier(UITexasSettingComponent.GetCurQuickActionNumValue(2));
                    callValue2 = num2Str == "Allin" ? totalChips : potMutiplier(UITexasSettingComponent.GetCurQuickActionNumValue(3));
                    callValueRight = numRightStr == "Allin" ? totalChips : (numRightStr == "0" ? 0 : potMutiplier(UITexasSettingComponent.GetCurQuickActionNumValue(4)));
                }

                textCallTitle0.text = "POT";
                textCallTitle1.text = "POT";
                textCallTitle2.text = "POT";
                textCallTitleLeft.text = "POT";
                textCallTitleRight.text = "POT";

                textCallPotLeft.text = UITexasSettingComponent.GetCurQuickActionNum(0);
                textCallPot0.text = UITexasSettingComponent.GetCurQuickActionNum(1);
                textCallPot1.text = UITexasSettingComponent.GetCurQuickActionNum(2);
                textCallPot2.text = UITexasSettingComponent.GetCurQuickActionNum(3);
                textCallPotRight.text = UITexasSettingComponent.GetCurQuickActionNum(4);

                buttonCall0.interactable = true;
                buttonCall1.interactable = true;
                buttonCall2.interactable = true;
                if (callValue0 < GameCache.Instance.CurGame.minAnteNum * 2)
                {
                    buttonCall0.interactable = false;
                }

                if (callValue1 < GameCache.Instance.CurGame.minAnteNum * 2)
                {
                    buttonCall1.interactable = false;
                }

                if (callValue2 < GameCache.Instance.CurGame.minAnteNum * 2)
                {
                    buttonCall2.interactable = false;
                }

            }


            if (GameCache.Instance.CurGame.roomMode > 0)//整数
            {
                callValue0 = (int)Math.Round(callValue0 / 100.0) * 100;
                callValue1 = (int)Math.Round(callValue1 / 100.0) * 100;
                callValue2 = (int)Math.Round(callValue2 / 100.0) * 100;
                callValueLeft = (int)Math.Round(callValueLeft / 100.0) * 100;
                callValueRight = (int)Math.Round(callValueRight / 100.0) * 100;
            }

            textCallPotValue0.text = callValue0 < totalChips ? StringHelper.GetShortString(callValue0) : "All in";
            textCallPotValue1.text = callValue1 < totalChips ? StringHelper.GetShortString(callValue1) : "All in";
            textCallPotValue2.text = callValue2 < totalChips ? StringHelper.GetShortString(callValue2) : "All in";
            textCallPotValueLeft.text = callValueLeft < totalChips ? StringHelper.GetShortString(callValueLeft) : "All in";
            textCallPotValueRight.text = callValueRight < totalChips ? StringHelper.GetShortString(callValueRight) : "All in";
        }

        private void SetRaiseHidden(bool hidden)
        {
            if (hidden)
            {
                buttonCall0.gameObject.SetActive(false);
                buttonCall1.gameObject.SetActive(false);
                buttonCall2.gameObject.SetActive(false);
                buttonCallLeft.gameObject.SetActive(false);
                buttonCallRight.gameObject.SetActive(false);
                buttonFreeCall.gameObject.SetActive(false);
                buttonPreciseCall.gameObject.SetActive(false);
                sliderInvisible.gameObject.SetActive(false);
            }
            else
            {
                buttonCall0.gameObject.SetActive(true);
                buttonCall1.gameObject.SetActive(true);
                buttonCall2.gameObject.SetActive(true);
                buttonCallLeft.gameObject.SetActive(callValueLeft > 0);
                buttonCallRight.gameObject.SetActive(callValueRight > 0);
                buttonFreeCall.gameObject.SetActive(true);
                buttonPreciseCall.gameObject.SetActive(true);
                sliderInvisible.gameObject.SetActive(true);
            }
        }

        public override void OnHide()
        {
            isCountDown = false;

            imageCheckCountDown.gameObject.SetActive(false);
            imageFoldCountDown.gameObject.SetActive(false);

            showFreeCall(false);
            ShowPreciseCall(false);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            removeHandler();

            base.Dispose();
        }

        private int potMutiplier(float times)
        {
            //return (int)Math.Ceiling(operationData.callAmount + (GameCache.Instance.CurGame.alreadAnte + operationData.callAmount) * times);
            return (int)Math.Ceiling(GameCache.Instance.CurGame.potNumber * times);
        }

        private int potMutiplier2(string numStr)
        {
            int totalChips = GameCache.Instance.CurGame.MainPlayer.chips;
            int nowante = GameCache.Instance.CurGame.MainPlayer.anteNumber;
            int chae = GameCache.Instance.CurGame.firstBet;
            int lastjiazhu = GameCache.Instance.CurGame.lastPlayerBet;

            if (numStr == "1/2")
                return 2 * lastjiazhu - nowante;
            else if (numStr == "2/3")
                return (int)(2.5 * lastjiazhu) - nowante;
            else if (numStr == "1x")
                return 3 * lastjiazhu - nowante;
            else if (numStr == "1.5x")
                return (int)(3.5 * lastjiazhu) - nowante;
            else if (numStr == "1/3")
                return chae;
            else if (numStr == "Allin")
                return totalChips;

            return 0;

        }

        private int potMutiplier3(string numStr)
        {
            int totalChips = GameCache.Instance.CurGame.MainPlayer.chips;
            int lastxiazhu = GameCache.Instance.CurGame.callAmount;

            if (numStr == "1/2")
                return (int)(2.5 * lastxiazhu);
            else if (numStr == "2/3")
                return 3 * lastxiazhu;
            else if (numStr == "1x")
                return (int)(3.5 * lastxiazhu);
            else if (numStr == "1.5x")
                return 4 * lastxiazhu;
            else if (numStr == "1/3")
                return 2 * lastxiazhu;
            else if (numStr == "Allin")
                return totalChips;
            return 0;
        }

        private void CheckOpt()
        {
            if (callValue >= GameCache.Instance.CurGame.MainPlayer.chips)
            {
                // allin
                GameCache.Instance.CurGame.OptAction(4, GameCache.Instance.CurGame.MainPlayer.chips);
            }
            else if (callValue == operationData.callAmount)
            {
                // 跟平
                GameCache.Instance.CurGame.OptAction(2, callValue);
            }
            else if (callValue > operationData.callAmount)
            {
                // 加注
                GameCache.Instance.CurGame.OptAction(3, callValue);
            }
            isCountDown = false;
        }

        private void registerHandler()
        {
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_ADD_TIME, HANDLER_REQ_ADD_TIME);  // 操作加时
            CPMessageDispatherComponent.Instance.RegisterHandler(HotfixOpcode.REQ_MTT_ADD_TIME, HANDLER_REQ_MTT_ADD_TIME);  // MTT操作加时
        }

        private void removeHandler()
        {
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_ADD_TIME, HANDLER_REQ_ADD_TIME);  // 操作加时
            CPMessageDispatherComponent.Instance.RemoveHandler(HotfixOpcode.REQ_MTT_ADD_TIME, HANDLER_REQ_MTT_ADD_TIME);  // MTT操作加时
        }

        protected void HANDLER_REQ_ADD_TIME(ICPResponse response)
        {
            REQ_ADD_TIME rec = response as REQ_ADD_TIME;
            if (null == rec)
                return;

            if (rec.status == 0)
            {
                if (rec.playerId == GameCache.Instance.CurGame.MainPlayer.userID)
                {
                    optCurTime += rec.addTime;
                    optTotalTime = optCurTime;
                }
            }
        }

        protected void HANDLER_REQ_MTT_ADD_TIME(ICPResponse response)
        {
            REQ_MTT_ADD_TIME rec = response as REQ_MTT_ADD_TIME;
            if (null == rec)
                return;

            if (rec.baseMsg.status != 0)
            {
                UIComponent.Instance.Toast(CPErrorCode.RoomErrorDescription(HotfixOpcode.REQ_MTT_ADD_TIME, rec.baseMsg.status));
                return;
            }

            Log.Msg(rec.baseMsg);
            HANDLER_REQ_ADD_TIME(rec.baseMsg);
        }

        public void PauseCountDown()
        {
            isCountDown = false;
        }

        public void ContinueCountDown()
        {
            isCountDown = true;
        }

        public void UpdateTopCallBtns()
        {
            SetUpTopCallBtns();

            if(buttonCall0.gameObject.activeInHierarchy)
            {
                buttonCallLeft.gameObject.SetActive(callValueLeft > 0);
                buttonCallRight.gameObject.SetActive(callValueRight > 0);
            }
        }
    }
}
