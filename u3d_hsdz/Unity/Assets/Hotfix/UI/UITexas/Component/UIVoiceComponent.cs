using DragonBones;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIVoiceComponentAwakeSystem : AwakeSystem<UIVoiceComponent>
    {
        public override void Awake(UIVoiceComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UIVoiceComponentUpdateSystem: UpdateSystem<UIVoiceComponent>
    {
        public override void Update(UIVoiceComponent self)
        {
            self.Update();
        }
    }

    public class UIVoiceComponent : UIBaseComponent
    {
        public sealed class VoiceData
        {
            public bool isCancel;
            public bool isContinue;
        }

        private ReferenceCollector rc;
        private UnityArmatureComponent armatureRecording;
        private Transform transCancel;
        private Transform transRecording;
        private Image imageVoiceTime;


        private float maxTimer = 15f;
        private float curTimer = 0;

        private bool isCountdown;

        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            armatureRecording = rc.Get<GameObject>("Armature_Recording").GetComponent<UnityArmatureComponent>();
            transCancel = rc.Get<GameObject>("Cancel").transform;
            transRecording = rc.Get<GameObject>("Recording").transform;
            imageVoiceTime = rc.Get<GameObject>("Image_VoiceTime").GetComponent<Image>();
        }

        public override void OnShow(object obj)
        {
            isCountdown = true;
            if (null == obj)
            {
                curTimer = 0f;
                transCancel.gameObject.SetActive(false);
                transRecording.gameObject.SetActive(true);
                armatureRecording.dragonAnimation.Play("recording");
            }
            else
            {
                VoiceData data = obj as VoiceData;
                if (null == data)
                {
                    curTimer = 0f;
                    transCancel.gameObject.SetActive(false);
                    transRecording.gameObject.SetActive(true);
                    // armatureRecording.dragonAnimation.Play("no-recording");
                    armatureRecording.dragonAnimation.Play("recording");
                }
                else
                {
                    if (!data.isContinue)
                        curTimer = 0f;
                    transRecording.gameObject.SetActive(!data.isCancel);
                    transCancel.gameObject.SetActive(data.isCancel);
                }
            }
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            curTimer = 0;

            base.Dispose();
        }

        public void Update()
        {
            if (!isCountdown)
                return;

            curTimer += Time.deltaTime;
            imageVoiceTime.fillAmount = curTimer / maxTimer;

            if (curTimer >= maxTimer)
            {
                isCountdown = false;
                GameCache.Instance.CurGame.StopRecord(true);
                UIComponent.Instance.HideNoAnimation(UIType.UIVoice);
            }
        }
    }
}
