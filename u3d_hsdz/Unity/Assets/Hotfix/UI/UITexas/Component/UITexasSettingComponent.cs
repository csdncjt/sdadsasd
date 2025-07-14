using System;
using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UITexasSettingComponentAwakeSystem : AwakeSystem<UITexasSettingComponent>
    {
        public override void Awake(UITexasSettingComponent self)
        {
            self.Awake();
        }
    }

    public class UITexasSettingComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        private Button buttonClose;
        private Toggle toggleCardType0;
        private Toggle toggleCardType1;
        private Toggle toggleCardType2;
        private Toggle toggleDeskType0;
        private Toggle toggleDeskType1;
        private Toggle toggleDeskType2;
        private Toggle toggleDeskType3;
        private Toggle toggleVoice;

        private Transform transQuickActionGroup;
        private Transform transQuickActionNumGroup;


        private int curCardType = 2;
        private int curDeskType = 0;

        private List<Toggle> togglesCardType;
        private List<Toggle> togglesDeskType;

        private int curQuickActionIndex = 2;
        private static string kQuickActionIndexKEY = "kQuickActionIndexKEY";
        private static string kQuickActionIndexValueKEY = "kQuickActionIndexValueKEY";

        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            buttonClose = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            toggleCardType0 = rc.Get<GameObject>("Toggle_CardType0").GetComponent<Toggle>();
            toggleCardType1 = rc.Get<GameObject>("Toggle_CardType1").GetComponent<Toggle>();
            toggleCardType2 = rc.Get<GameObject>("Toggle_CardType2").GetComponent<Toggle>();
            toggleDeskType0 = rc.Get<GameObject>("Toggle_DeskType0").GetComponent<Toggle>();
            toggleDeskType1 = rc.Get<GameObject>("Toggle_DeskType1").GetComponent<Toggle>();
            toggleDeskType2 = rc.Get<GameObject>("Toggle_DeskType2").GetComponent<Toggle>();
            toggleDeskType3 = rc.Get<GameObject>("Toggle_DeskType3").GetComponent<Toggle>();
            toggleVoice = rc.Get<GameObject>("Toggle_Voice").GetComponent<Toggle>();


            if (null == togglesCardType)
                togglesCardType = new List<Toggle>();
            else
                togglesCardType.Clear();
            togglesCardType.Add(toggleCardType0);
            togglesCardType.Add(toggleCardType1);
            togglesCardType.Add(toggleCardType2);

            if (null == togglesDeskType)
                togglesDeskType = new List<Toggle>();
            else
                togglesDeskType.Clear();
            togglesDeskType.Add(toggleDeskType0);
            togglesDeskType.Add(toggleDeskType1);
            togglesDeskType.Add(toggleDeskType2);
            togglesDeskType.Add(toggleDeskType3);

            UIEventListener.Get(buttonClose.gameObject).onClick = onClickClose;

            toggleCardType0.onValueChanged.AddListener(onValueChangeCardType0);
            toggleCardType1.onValueChanged.AddListener(onValueChangeCardType1);
            toggleCardType2.onValueChanged.AddListener(onValueChangeCardType2);
            toggleDeskType0.onValueChanged.AddListener(onValueChangeDeskType0);
            toggleDeskType1.onValueChanged.AddListener(onValueChangeDeskType1);
            toggleDeskType2.onValueChanged.AddListener(onValueChangeDeskType2);
            toggleDeskType3.onValueChanged.AddListener(onValueChangeDeskType3);
            toggleVoice.onValueChanged.AddListener(onValueChangedVoice);

            transQuickActionGroup = rc.Get<GameObject>("QuickActionGroup").transform;
            transQuickActionNumGroup = rc.Get<GameObject>("QuickActionNumGroup").transform;
            SetUpQuickActionToggles();
        }
         
        private void SetUpQuickActionToggles()
        {
            for (int i = 0; i < transQuickActionGroup.childCount; i++)
            {
                Toggle actionToggle = transQuickActionGroup.GetChild(i).gameObject.GetComponent<Toggle>();
                if (curQuickActionIndex == i)
                {
                    actionToggle.isOn = true;
                }
                UIEventListener.Get(actionToggle.gameObject, i).onIntClick = SelectActionToggle;
                Text textCallPot = actionToggle.transform.Find("Text_CallPot").GetComponent<Text>();
                string numStr = GetCurQuickActionNum(i);
                if (numStr == "0")
                {
                    numStr = "+";
                    textCallPot.fontSize = 70;
                } else
                {
                    textCallPot.fontSize = 40;
                }
                textCallPot.text = numStr;
            }

            SetUpQuickActionNum();
        }

        private string GetNumToggleString(int index)
        {
            string[] nums = { };
            if (curQuickActionIndex > 0 && curQuickActionIndex < 4)
            {
                nums = new string[] { "1/2", "1/3", "2/3", "1x", "1.5x", "Allin" };
            }
            else
            {
                nums = new string[] { "0", "1/2", "1/3", "2/3", "1x", "1.5x", "Allin" };
            }
            return nums[index]; 
        }

        private float GetNumToggleValue(int index)
        {
            float[] nums = { };
            if (curQuickActionIndex > 0 && curQuickActionIndex < 4)
            {
                nums = new float[] { 1.0f/2, 1.0f/3, 2.0f/3, 1.0f, 1.5f, float.MaxValue };
            }
            else
            {
                nums = new float[] { 0f, 1.0f/2, 1.0f/3,2.0f/3, 1.0f, 1.5f, float.MaxValue };
            }
            return nums[index];
        }

        public static string GetCurQuickActionNum(int index)
        {
            string[] defaultActionNums = { "0", "1/2", "2/3", "1x", "0" };
            string numStr = PlayerPrefs.GetString(kQuickActionIndexKEY + $"{index}", defaultActionNums[index]);
            return numStr;
        }

        public static float GetCurQuickActionNumValue(int index)
        {
            float[] defaultActionNums = { 0, 1.0f/2, 2.0f/3, 1.0f, 0};
            float numValue = PlayerPrefs.GetFloat(kQuickActionIndexValueKEY + $"{index}", defaultActionNums[index]);
            return numValue;
        }

    

        private void SetUpQuickActionNum()
        {
            for (int i = 0; i < transQuickActionNumGroup.childCount; i++)
            {
                Toggle numToggle = transQuickActionNumGroup.GetChild(i).gameObject.GetComponent<Toggle>();

                if (curQuickActionIndex > 0 && curQuickActionIndex < 4 && i == transQuickActionNumGroup.childCount - 1)
                {
                    numToggle.gameObject.SetActive(false);
                    return;
                }
                numToggle.gameObject.SetActive(true);
                UIEventListener.Get(numToggle.gameObject, i).onIntClick = SelectNumToggle;
                Text textCallPot = numToggle.transform.Find("Text_CallPot").GetComponent<Text>();
                string numStr = GetNumToggleString(i);
                if (numStr == GetCurQuickActionNum(curQuickActionIndex))
                {
                    numToggle.isOn = true;
                    SelectNumToggle(numToggle.gameObject, i);
                }
                if (numStr == "0")
                {
                    numStr = LanguageManager.Get("adaptation10077");
                }
                textCallPot.text = numStr;

              
            }

        }

        private void SelectActionToggle(GameObject go, int index)
        {
            curQuickActionIndex = index;
            SetUpQuickActionNum();
        }

        private void SelectNumToggle(GameObject go, int index)
        {
            Toggle actionToggle = transQuickActionGroup.GetChild(curQuickActionIndex).gameObject.GetComponent<Toggle>();
            Text textCallPot = actionToggle.transform.Find("Text_CallPot").GetComponent<Text>();
            string numStr = GetNumToggleString(index);
            float numValue = GetNumToggleValue(index);
            PlayerPrefs.SetString(kQuickActionIndexKEY + $"{curQuickActionIndex}", numStr);
            PlayerPrefs.SetFloat(kQuickActionIndexValueKEY + $"{curQuickActionIndex}", numValue);
            if (numStr == "0")
            {
                numStr = "+";
                textCallPot.fontSize = 70;
            }
            else
            {
                textCallPot.fontSize = 40;
            }
            textCallPot.text = numStr;

            for (int i = 0; i < transQuickActionNumGroup.childCount; i++)
            {
                Toggle numToggle = transQuickActionNumGroup.GetChild(i).gameObject.GetComponent<Toggle>();
                Text numTextCallPot = numToggle.transform.Find("Text_CallPot").GetComponent<Text>();
                if (index == i)
                {
                    numTextCallPot.color = new Color32(233, 191, 128, 255);
                } else
                {
                    numTextCallPot.color = new Color32(168, 168, 168, 255);
                }
            }

            // 操作面板实时更新
            UI mUIOperationComponentUI = UIComponent.Instance.Get(UIType.UIOperation);
            if (null != mUIOperationComponentUI && mUIOperationComponentUI.GameObject.activeInHierarchy)
            {
                UIOperationComponent mUIOperationComponent = mUIOperationComponentUI.UiBaseComponent as UIOperationComponent;
                mUIOperationComponent.UpdateTopCallBtns();
            }
        }

        private void onValueChangeCardType0(bool arg0)
        {
            if (arg0)
            {
                int mTmpCardType = 0;
                if (mTmpCardType == curCardType)
                    return;

                bool mSucess = GameCache.Instance.CurGame.SetCardType(mTmpCardType);
                if (mSucess)
                {
                    curCardType = mTmpCardType;
                    PlayerPrefsMgr.mInstance.SetInt(PlayerPrefsKeys.SettingCardType, curCardType);
                }
            }
        }

        private void onValueChangeCardType1(bool arg0)
        {
            if (arg0)
            {
                int mTmpCardType = 1;
                if (mTmpCardType == curCardType)
                    return;

                bool mSucess = GameCache.Instance.CurGame.SetCardType(mTmpCardType);
                if (mSucess)
                {
                    curCardType = mTmpCardType;
                    PlayerPrefsMgr.mInstance.SetInt(PlayerPrefsKeys.SettingCardType, curCardType);
                }
            }
        }

        private void onValueChangeCardType2(bool arg0)
        {
            if (arg0)
            {
                int mTmpCardType = 2;
                if (mTmpCardType == curCardType)
                    return;

                bool mSucess = GameCache.Instance.CurGame.SetCardType(mTmpCardType);
                if (mSucess)
                {
                    curCardType = mTmpCardType;
                    PlayerPrefsMgr.mInstance.SetInt(PlayerPrefsKeys.SettingCardType, curCardType);
                }
            }
        }

        private void onValueChangeDeskType0(bool arg0)
        {
            if (arg0)
            {
                bool mSucess = GameCache.Instance.CurGame.SetDeskType(0);
                if (mSucess)
                {
                    curDeskType = 0;
                    PlayerPrefsMgr.mInstance.SetInt(PlayerPrefsKeys.SettingDeskType, curDeskType);
                }
            }
        }

        private void onValueChangeDeskType1(bool arg0)
        {
            if (arg0)
            {
                bool mSucess = GameCache.Instance.CurGame.SetDeskType(1);
                if (mSucess)
                {
                    curDeskType = 1;
                    PlayerPrefsMgr.mInstance.SetInt(PlayerPrefsKeys.SettingDeskType, curDeskType);
                }
            }
        }

        private void onValueChangeDeskType2(bool arg0)
        {
            if (arg0)
            {
                bool mSucess = GameCache.Instance.CurGame.SetDeskType(2);
                if (mSucess)
                {
                    curDeskType = 2;
                    PlayerPrefsMgr.mInstance.SetInt(PlayerPrefsKeys.SettingDeskType, curDeskType);
                }
            }
        }

        private void onValueChangeDeskType3(bool arg0)
        {
            if (arg0)
            {
                bool mSucess = GameCache.Instance.CurGame.SetDeskType(3);
                if (mSucess)
                {
                    curDeskType = 3;
                    PlayerPrefsMgr.mInstance.SetInt(PlayerPrefsKeys.SettingDeskType, curDeskType);
                }
            }
        }

        private void onValueChangedVoice(bool arg0)
        {
            SoundComponent.Instance.SFXVolume(arg0? 1f : 0f);
        }

        private void onClickClose(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UITexasSetting);
        }

        public override void OnShow(object obj)
        {
            curCardType = PlayerPrefsMgr.mInstance.GetInt(PlayerPrefsKeys.SettingCardType, 2);
            curDeskType = PlayerPrefsMgr.mInstance.GetInt(PlayerPrefsKeys.SettingDeskType, 0);
            InitCardType();
            InitDeskType();
            InitSound();
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            if (null != togglesCardType)
            {
                togglesCardType.Clear();
                togglesCardType = null;
            }

            if (null != togglesDeskType)
            {
                togglesDeskType.Clear();
                togglesDeskType = null;
            }
                
            base.Dispose();
        }

        private void InitDeskType()
        {
            if (null != togglesDeskType && togglesDeskType.Count > 0)
            {
                if (curDeskType < togglesDeskType.Count)
                {
                    togglesDeskType[curDeskType].isOn = true;
                }
                else
                {
                    togglesDeskType[0].isOn = true;
                }
            }
        }

        private void InitCardType()
        {
            if (null != togglesCardType && togglesCardType.Count > 0)
            {
                if (curCardType < togglesCardType.Count)
                {
                    togglesCardType[curCardType].isOn = true;
                }
                else
                {
                    togglesCardType[0].isOn = true;
                }
            }
        }

        private void InitSound()
        {
            toggleVoice.isOn = SoundComponent.Instance.audioManager.SoundVolume > 0f;
        }


    }
}
