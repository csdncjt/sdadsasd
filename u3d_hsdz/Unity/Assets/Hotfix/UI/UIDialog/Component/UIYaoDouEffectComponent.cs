using System;
using System.Collections.Generic;
using DG.Tweening;
using ETModel;
using UnityEngine;
using UnityEngine.UI;


namespace ETHotfix
{
    [ObjectSystem]
    public class UIYaoDouEffectComponentSystem : AwakeSystem<UIYaoDouEffectComponent>
    {
        public override void Awake(UIYaoDouEffectComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UIYaoDouEffectComponentUpdateSystem : UpdateSystem<UIYaoDouEffectComponent>
    {
        public override void Update(UIYaoDouEffectComponent self)
        {
            self.Update();
        }
    }

    /// <summary> 页面名: </summary>
    public class UIYaoDouEffectComponent : UIBaseComponent
    {
        private ReferenceCollector rc;
        GameObject buttonCancel;
        private RectTransform RawImage;
        private int mGetGoldBean;
        private Camera mCamera;

        public void Awake()
        {
            InitUI();
        }
        public void Update()
        {
        }

        public override void OnShow(object obj)
        {
            if (null != obj)
            {
                mGetGoldBean = Convert.ToInt32(obj);
                ClickText_LeftTop();
            }
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
            YaoYaoLeLight = rc.Get<GameObject>("YaoYaoLeLight").transform;
            YaoYaoLeLight.gameObject.SetActive(false);
            UIEventListener.Get(rc.Get<GameObject>("Button_Cancel")).onClick = ClickButton_Cancel;
            buttonCancel = rc.Get<GameObject>("Button_Cancel");
            RawImage = rc.Get<GameObject>("RawImage").GetComponent<RectTransform>();
            buttonCancel.SetActive(false);
        }

        private void ClickButton_Cancel(GameObject go)
        {
            UIComponent.Instance.Remove(UIType.UIYaoDouEffect);
            GameObject.DestroyImmediate(mGO);
            UIMatchModel.mInstance.APIActivityKDouGain();
            //UIMatchModel.mInstance.ShowShareGoldBean(mGetGoldBean.ToString());
        }
        private GameObject mGO;

        #region 左上角 滚轮  
        private Transform YaoYaoLeLight;

        public GameObject GetResource(string ab, string prefab)
        {
            ResourcesComponent mResourcesComponent = Game.Scene.ModelScene.GetComponent<ResourcesComponent>();
            mResourcesComponent.LoadBundle(ab);
            GameObject tPrefab = mResourcesComponent.GetAsset(ab, prefab) as GameObject;
            return tPrefab;
        }


        private async void ClickText_LeftTop()
        {
            YaoYaoLeLight.gameObject.SetActive(true);
            YaoYaoLeLight.localEulerAngles = Vector3.zero;
            YaoYaoLeLight.DOLocalRotate(new Vector3(0, 0, -380), 5, RotateMode.FastBeyond360);

            var tAssets = GetResource("camera_yaoyaole.unity3d", "Camera_Yaoyaole");
            mGO = GameObject.Instantiate(tAssets);
            mCamera = mGO.GetComponent<Camera>();
            var tYaoyaole = mGO.transform.Find("yaoyaole");
            var tJinDou = mGO.transform.Find("jindou");
            tJinDou.gameObject.SetActive(false);

            SoundComponent.Instance.PlaySFX(SoundComponent.SFX_EFF_1);
            mGO.transform.localScale = Vector3.one;
            mGO.transform.localEulerAngles = new Vector3(0, 180, 0);
            mGO.transform.localPosition = new Vector3(-0.02f, -1.1f, 6);
            mGO.SetActive(true);//0.2 +0.3 + 0.8
            var tGunlun01 = tYaoyaole.transform.Find("gunlun01");
            var tGunlun02 = tYaoyaole.transform.Find("gunlun02");
            var tGunlun03 = tYaoyaole.transform.Find("gunlun03");
            var tRotates = new int[] { 45, 90, 135, 180, 225, 270, 315, 0 };
            var index01 = UnityEngine.Random.Range(0, tRotates.Length);
            var index02 = UnityEngine.Random.Range(0, tRotates.Length);
            var index03 = UnityEngine.Random.Range(0, tRotates.Length);

            mGO.transform.DOLocalMove(new Vector3(-0.02f, -1.22f, 1.9f), 0.2f).OnComplete(delegate
            {
                mGO.transform.Find("yaoyaole/yaogan").DOLocalRotate(new Vector3(92, 0, 0), 0.3f).OnComplete(delegate
                {
                    var Sequence03 = DOTween.Sequence();
                    Sequence03.Append(tGunlun03.DOLocalRotate(new Vector3(7200, 0, 0), 1.5f, RotateMode.FastBeyond360)).SetEase(Ease.Linear);
                    Sequence03.Append(tGunlun03.DOLocalRotate(new Vector3(tRotates[index01] + 360, 0, 0), 0.7f)).SetEase(Ease.OutQuad);
                    Sequence03.Append(tGunlun03.DOShakeRotation(0.5f, new Vector3(10, 0, 0), 5).SetLoops(-1, LoopType.Yoyo));
                    Sequence03.Play();

                    var Sequence02 = DOTween.Sequence();
                    Sequence02.Append(tGunlun02.DOLocalRotate(new Vector3(7200, 0, 0), 1.9f, RotateMode.FastBeyond360)).SetEase(Ease.Linear);
                    Sequence02.Append(tGunlun02.DOLocalRotate(new Vector3(tRotates[index02] + 360, 0, 0), 0.7f, RotateMode.FastBeyond360)).SetEase(Ease.OutQuad);
                    Sequence02.Append(tGunlun02.DOShakeRotation(0.5f, new Vector3(10, 0, 0), 5).SetLoops(-1, LoopType.Yoyo));
                    Sequence02.Play();

                    var Sequence01 = DOTween.Sequence();
                    Sequence01.Append(tGunlun01.DOLocalRotate(new Vector3(7200, 0, 0), 2.1f, RotateMode.FastBeyond360)).SetEase(Ease.Linear);
                    Sequence01.Append(tGunlun01.DOLocalRotate(new Vector3(tRotates[index03] + 360, 0, 0), 0.7f, RotateMode.FastBeyond360)).SetEase(Ease.OutQuad);
                    Sequence01.Append(tGunlun01.DOShakeRotation(0.5f, new Vector3(10, 0, 0), 5).SetLoops(-1, LoopType.Yoyo));
                    Sequence01.Play();
                });//6*0.7=4.2s  +0.3  +0.2
            });


            await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(3800);
            RawImage.sizeDelta = new Vector2(2208, 2208);
            SoundComponent.Instance.PlaySFX(SoundComponent.SFX_EFF_2);
            tYaoyaole.gameObject.SetActive(false);
            YaoYaoLeLight.gameObject.SetActive(false);
            tJinDou.gameObject.SetActive(true);

            var textShow = tJinDou.transform.Find("Texture/TextShow").GetComponent<TextMesh>();
            textShow.text = "";
            //mGetGoldBean = UnityEngine.Random.Range(101, 9999);
            textShow.text = mGetGoldBean.ToString();
            await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(500);

            mCamera.clearFlags = CameraClearFlags.Depth;//摄相机的改变
            mCamera.targetTexture = null;


            textShow.transform.DOScale(0.11f, 0.6f).OnComplete(delegate
            {
                textShow.transform.DOScale(0.088f, 0.2f);
            });

            await Game.Scene.ModelScene.GetComponent<TimerComponent>().WaitAsync(2000);
            buttonCancel.SetActive(true);
        }


        ParticleSystem douziPS;

        void PlayerDouziPs()
        {
            douziPS.Play();
        }

        #endregion

    }
}


