using UnityEngine;
using UnityEngine.UI;
namespace ETModel
{

    public class LanguageText : MonoBehaviour
    {
        public string mKey;
        private Text textLbl;
        //除登录页在start               其他都在Awake
        public bool mUseAwake = true;

        private void Awake()
        {
            if (mUseAwake)
            {
                LoadContent();
            }
        }

        private void Start()
        {
            if (mUseAwake == false)
                LoadContent();
        }

        void LoadContent()
        {
            if (string.IsNullOrEmpty(mKey)) return;
            textLbl = this.GetComponent<Text>();
            if (textLbl == null) { Debug.LogError("没Text 组件呀~~"); return; }
            textLbl.text = LanguageManager.mInstance.GetLanguageForKey(mKey);
            LanguageManager.mInstance.EActTextChange += EventActionChangeTxt;//加上监听
        }


        private void EventActionChangeTxt()
        {
            if (textLbl != null)
                textLbl.text = LanguageManager.mInstance.GetLanguageForKey(mKey);
        }

        public void OnDestroy()
        {
            LanguageManager.mInstance.EActTextChange -= EventActionChangeTxt;
        }

    }

}
