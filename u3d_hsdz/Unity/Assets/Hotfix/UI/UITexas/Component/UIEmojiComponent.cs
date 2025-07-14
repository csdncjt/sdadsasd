using System;
using System.Collections.Generic;
using System.Net;
using BestHTTP;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DragonBones;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIEmojiComponentAwakeSystem : AwakeSystem<UIEmojiComponent, Transform>
    {
        public override void Awake(UIEmojiComponent self, Transform trans)
        {
            self.Awake(trans);
        }
    }

    public class UIEmojiComponent : UIBaseComponent
    {
        private UnityArmatureComponent armatureComp;
        private Transform Tran;

        public void Awake(Transform trans)
        {
            this.Tran = trans;
        }

        public async void PlayEmoji(string name)
        {
            string bundleName = "emoji.unity3d";
            ResourcesComponent mResourcesComponent = Game.Scene.ModelScene.GetComponent<ResourcesComponent>();
            mResourcesComponent.LoadBundle(bundleName);
            string assetName = $"emoji_{name}";
            string animationName = null;
            int playTime = -1;
            if (name.Contains("3d"))
            {
                //3d表情是同一个龙骨文件
                assetName = "emoji_3d";
                animationName = $"emoji_{name}";
                playTime = 1;
            }
            GameObject mPrefab = mResourcesComponent.GetAsset(bundleName, assetName) as GameObject;
            if (null == mPrefab)
                return;
            if (Tran.childCount > 0)
            {
                GameObject.Destroy(Tran.GetChild(0).gameObject);
            }

            GameObject mObj = GameObject.Instantiate(mPrefab);
            armatureComp = mObj.GetComponent<UnityArmatureComponent>();
            armatureComp.transform.parent = Tran;
            armatureComp.transform.localPosition = Vector3.zero;
            armatureComp.transform.localScale = new Vector3(100, 100, 0);
            armatureComp.dragonAnimation.Reset();
            armatureComp.dragonAnimation.Play(animationName, playTime);

            if (name.Contains("3d"))
            {
                armatureComp.AddEventListener(DragonBones.EventObject.COMPLETE, (key, go) =>
                {
                    GameObject.Destroy(mObj);
                });
            }
            else
            {
                TimerComponent iOSTimerComponent = Game.Scene.ModelScene.GetComponent<TimerComponent>();
                await iOSTimerComponent.WaitAsync(2500);

                GameObject.Destroy(mObj);
            }

        }

        public override void OnShow(object obj)
        {
        }

        public override void OnHide()
        {
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            base.Dispose();
        }
    }
}
