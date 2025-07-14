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
    public class UIAnimojiComponentSystem : AwakeSystem<UIAnimojiComponent>
    {
        public override void Awake(UIAnimojiComponent self)
        {
            self.Awake();
        }
    }

    public class UIAnimojiComponent : UIBaseComponent
    {
        private ReferenceCollector rc;


        private sealed class AnimojiInfo
        {
            private UnityArmatureComponent armatureFromComp;
            int animationIndex;
            string animojiName;
            Seat fromSeat;
            Seat toSeat;

            public void NewPlaySingleAnimoji(string name, Seat fromSeat, Seat toSeat, Transform tran)
            {
                SoundComponent.Instance.PlaySFX($"{name}");
                this.fromSeat = fromSeat;
                this.toSeat = toSeat;
                this.animojiName = name;
                string bundleName = $"animoji.unity3d";
                ResourcesComponent mResourcesComponent = Game.Scene.ModelScene.GetComponent<ResourcesComponent>();
                mResourcesComponent.LoadBundle(bundleName);
                GameObject mPrefab = mResourcesComponent.GetAsset(bundleName, animojiName) as GameObject;
                if (null == mPrefab)
                    return;
                GameObject mObj = GameObject.Instantiate(mPrefab);
                armatureFromComp = mObj.GetComponent<UnityArmatureComponent>();
                armatureFromComp.transform.parent = tran;
                armatureFromComp.transform.localPosition = Vector3.zero;
                armatureFromComp.transform.localScale = new Vector3(100, 100, 0);
                armatureFromComp.dragonAnimation.Reset();

                //移动到目标头像
                MoveAction(mObj, mResourcesComponent, bundleName);

            }

            async void MoveAction(GameObject mObj, ResourcesComponent mResourcesComponent, string bundleName)
            {
                armatureFromComp.transform.localPosition = FromPos();
                armatureFromComp.transform.DOLocalMove(ToPos(), 0.7f).SetEase(Ease.Linear);

                TimerComponent timerComponent = Game.Scene.ModelScene.GetComponent<TimerComponent>();
                await timerComponent.WaitAsync(700);

                armatureFromComp.dragonAnimation.Play("Sprite", 1);
                armatureFromComp.AddEventListener(DragonBones.EventObject.COMPLETE, (key, go) =>
                {
                    GameObject.Destroy(mObj);
                    mResourcesComponent.UnloadBundle(bundleName);

                });

            }

            public void PlaySingleAnimoji(string name, Seat fromSeat, Seat toSeat, Transform tran)
            {
                SoundComponent.Instance.PlaySFX($"sound_animoji_{name}");

                this.fromSeat = fromSeat;
                this.toSeat = toSeat;
                this.animojiName = name;


                string bundleName = $"animoji_{name}.unity3d";
                ResourcesComponent mResourcesComponent = Game.Scene.ModelScene.GetComponent<ResourcesComponent>();
                mResourcesComponent.LoadBundle(bundleName);
                GameObject mPrefab = mResourcesComponent.GetAsset(bundleName, animojiName) as GameObject;
                if (null == mPrefab)
                    return;
                GameObject mObj = GameObject.Instantiate(mPrefab);
                armatureFromComp = mObj.GetComponent<UnityArmatureComponent>();
                armatureFromComp.transform.parent = tran;
                armatureFromComp.transform.localPosition = Vector3.zero;
                armatureFromComp.transform.localScale = new Vector3(100, 100, 0);
                armatureFromComp.dragonAnimation.Reset();

                List<GameObject> otherObjs = new List<GameObject>();

                if (name == "leishen")
                {
                    //雷神动画，全屏互动
                    armatureFromComp.transform.localPosition = FromPos();

                    PlayLeishenOther(otherObjs, tran, mPrefab, fromSeat);

                }

                if (name == "zhadan" || name == "beer" || name == "dantiao"
                    || name == "king" || name == "shayu" || name == "xiexielaoban"
                    || name == "zhuoji" || name == "stool" || name == "strong" || name == "weak")
                {
                    //从自己头像移动到别人头像上的一类
                    armatureFromComp.transform.localPosition = FromPos();
                    armatureFromComp.transform.DOLocalMove(ToPos(), AnimationTime()).SetEase(Ease.Linear);

                }

                if (name == "aa" || name == "allin" || name == "xiangyan" || name == "dushen" || name == "crying")
                {
                    //只在自己头像上播放的一类
                    armatureFromComp.transform.localPosition = FromPos();
                }

                string betweenBundleName = null;
                if (name == "jiatelin")
                {
                    //自己和他人头像分开播放互动的一类
                    armatureFromComp.transform.localPosition = FromPos();
                    float angle = Mathf.Atan((fromSeat.seatUIInfo.Pos.x - toSeat.seatUIInfo.Pos.x) / (toSeat.seatUIInfo.Pos.y - fromSeat.seatUIInfo.Pos.y));
                    if (fromSeat.seatUIInfo.Pos.y - toSeat.seatUIInfo.Pos.y > 0)
                    {
                        angle += Mathf.PI;
                    }
                    armatureFromComp.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90 * (angle / (Mathf.PI * 0.5f))));

                    betweenBundleName = "animoji_jiatelinhuoyan.unity3d";
                    mResourcesComponent.LoadBundle(betweenBundleName);
                    GameObject mBetweenPrefab = mResourcesComponent.GetAsset(betweenBundleName, "jiatelinhuoyan") as GameObject;
                    if (null == mBetweenPrefab)
                        return;
                    GameObject betweenObj = GameObject.Instantiate(mBetweenPrefab);
                    otherObjs.Add(betweenObj);
                    UnityArmatureComponent armaturebetweenComp = betweenObj.GetComponent<UnityArmatureComponent>();
                    armaturebetweenComp.transform.parent = tran;
                    var dx = fromSeat.seatUIInfo.Pos.x - 170 * Mathf.Sin(angle);
                    var dy = fromSeat.seatUIInfo.Pos.y + 170 * Mathf.Cos(angle);
                    float distanse = Mathf.Sqrt(Mathf.Pow(toSeat.seatUIInfo.Pos.x - fromSeat.seatUIInfo.Pos.x, 2) + Mathf.Pow(toSeat.seatUIInfo.Pos.y - fromSeat.seatUIInfo.Pos.y, 2));
                    float scaleY = (distanse - 800) * 0.15f;
                    armaturebetweenComp.transform.localPosition = new Vector3(dx, dy, 0);
                    armaturebetweenComp.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90 * (angle / (Mathf.PI * 0.5f))));
                    armaturebetweenComp.transform.localScale = new Vector3(100, 100 + scaleY, 0);
                    armaturebetweenComp.dragonAnimation.Reset();
                    armaturebetweenComp.dragonAnimation.Play("newAnimation");

                    GameObject toObj = GameObject.Instantiate(mPrefab);
                    otherObjs.Add(toObj);
                    UnityArmatureComponent armatureToComp = toObj.GetComponent<UnityArmatureComponent>();
                    armatureToComp.transform.parent = tran;
                    armatureToComp.transform.localPosition = ToPos();
                    armatureToComp.transform.localScale = new Vector3(100, 100, 0);
                    armatureToComp.dragonAnimation.Reset();
                    armatureToComp.dragonAnimation.Play(ToAnimationNames(), 1);
                }

                animationIndex = 0;
                StartPlayAnimation();

                armatureFromComp.AddEventListener(DragonBones.EventObject.COMPLETE, (key, go) =>
                {
                    if (animationIndex < animationNames().Length)
                    {
                        StartPlayAnimation();
                    }
                    else
                    {
                        GameObject.Destroy(mObj);
                        foreach (GameObject obj in otherObjs)
                        {
                            GameObject.Destroy(obj);
                        }
                        mResourcesComponent.UnloadBundle(bundleName);
                        if (null != betweenBundleName)
                        {
                            mResourcesComponent.UnloadBundle(betweenBundleName);
                        }
                    }
                });
            }

            async void PlayLeishenOther(List<GameObject> otherObjs, Transform tran, GameObject mPrefab, Seat fromSeat)
            {
                TimerComponent timerComponent = Game.Scene.ModelScene.GetComponent<TimerComponent>();
                await timerComponent.WaitAsync(2000);
                GameObject fullLighting = GameObject.Instantiate(mPrefab);
                otherObjs.Add(fullLighting);
                UnityArmatureComponent armatureFullComp = fullLighting.GetComponent<UnityArmatureComponent>();
                armatureFullComp.transform.parent = tran;
                armatureFullComp.transform.localPosition = Vector3.zero;
                armatureFullComp.transform.localScale = new Vector3(130, 130, 0);
                armatureFullComp.dragonAnimation.Reset();
                armatureFullComp.dragonAnimation.Play("T_shifa", 0);

                SoundComponent.Instance.PlaySFX("sound_animoji_leishen_lightning");

                //每人头顶上的闪电
                Seat mSeat = null;
                List<Seat> listSeat = GameCache.Instance.CurGame.listSeat;
                for (int i = 0, n = listSeat.Count; i < n; i++)
                {
                    mSeat = listSeat[i];
                    if (null != mSeat && null != mSeat.Player && mSeat.Player.userID >= 0 && mSeat.Player.userID != fromSeat.Player.userID)
                    {
                        GameObject userLighting = GameObject.Instantiate(mPrefab);
                        otherObjs.Add(userLighting);
                        UnityArmatureComponent armatureUserComp = userLighting.GetComponent<UnityArmatureComponent>();
                        armatureUserComp.transform.parent = tran;
                        armatureUserComp.transform.localPosition = mSeat.seatUIInfo.Pos + new Vector3(0, -80, 0);
                        armatureUserComp.transform.localScale = new Vector3(100, 100, 0);
                        armatureUserComp.dragonAnimation.Reset();
                        armatureUserComp.dragonAnimation.Play("T_luolei", 0);
                    }
                }
            }

            private async void StartPlayAnimation()
            {
                string animationName = animationNames()[animationIndex];
                int playCout = 1;
                if (animojiName == "shayu" && animationIndex == 1) playCout = 2;
                if (animojiName == "xiangyan" && animationIndex == 1) playCout = 5;
                armatureFromComp.dragonAnimation.Play(animationName, playCout);
                armatureFromComp.armature.flipX = isFlipX();

                if (animojiName == "shayu")
                {
                    //鲨鱼的游动要有角度
                    float angle = Mathf.Atan((fromSeat.seatUIInfo.Pos.x - toSeat.seatUIInfo.Pos.x) / (toSeat.seatUIInfo.Pos.y - fromSeat.seatUIInfo.Pos.y));
                    if (fromSeat.seatUIInfo.Pos.y - toSeat.seatUIInfo.Pos.y > 0) angle += Mathf.PI;

                    if (animationIndex < 2)
                    {
                        armatureFromComp.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90 * (angle / (Mathf.PI * 0.5f))));
                    }
                    else
                    {
                        armatureFromComp.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    }

                    if (animationIndex > 0)
                    {
                        armatureFromComp.transform.localPosition = toSeat.seatUIInfo.Pos;
                    }

                    if (animationIndex == 1)
                    {
                        if (toSeat.seatUIInfo.Pos.x >= 0)
                        {
                            //右方座位
                            angle += Mathf.PI * 0.5f;
                            if (angle > Mathf.PI) angle = angle - Mathf.PI * 2.0f;
                            angle = Mathf.PI * 1.5f + angle;
                        }
                        else
                        {
                            //左方座位
                            angle -= Mathf.PI * 0.5f;
                            if (angle > Mathf.PI) angle = angle - Mathf.PI * 2.0f;
                            angle = Mathf.PI * 1.5f - angle;
                        }

                        float time = angle * 3500 / (Mathf.PI * 2.0f);

                        TimerComponent timerComponent = Game.Scene.ModelScene.GetComponent<TimerComponent>();
                        await timerComponent.WaitAsync(Convert.ToInt32(time));
                        animationIndex++;
                        //SoundComponent.Instance.PlaySFX("sound_animoji_shayu_eat");
                        StartPlayAnimation();
                    }

                }

                if (animojiName == "allin")
                {
                    //推Allin要有角度
                    if (animationIndex == 0)
                    {
                        float angle = Mathf.Atan((fromSeat.seatUIInfo.Pos.x - 0) / (0 - fromSeat.seatUIInfo.Pos.y));
                        if (fromSeat.seatUIInfo.Pos.y - 0 > 0)
                        {
                            angle += Mathf.PI;
                        }
                        var dx = fromSeat.seatUIInfo.Pos.x - 150 * Mathf.Sin(angle);
                        var dy = fromSeat.seatUIInfo.Pos.y + 150 * Mathf.Cos(angle);
                        armatureFromComp.transform.localPosition = new Vector3(dx, dy, 0);
                        armatureFromComp.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90 * (angle / (Mathf.PI * 0.5f))));
                    }
                }

                animationIndex++;
            }

            private string[] animationNames()
            {
                if (animojiName == "zhadan")
                {
                    return new string[] { "reng", "zha" };
                }
                if (animojiName == "aa")
                {
                    return new string[] { "animation" };
                }
                if (animojiName == "allin")
                {
                    return new string[] { "tuichuqu" };
                }
                if (animojiName == "beer")
                {
                    return new string[] { "beer", "cheers" };
                }
                if (animojiName == "dantiao")
                {
                    return new string[] { "reng", "quan" };
                }
                if (animojiName == "dushen")
                {
                    return new string[] { "run" };
                }
                if (animojiName == "jiatelin")
                {
                    return new string[] { "kaiqiang" };
                }
                if (animojiName == "king")
                {
                    return new string[] { "reng", "kan" };
                }
                if (animojiName == "shayu")
                {
                    return new string[] { "run", "huanrao", "tiaoyao" };
                }
                if (animojiName == "xiangyan")
                {
                    return new string[] { "dianyan", "xunhuan" };
                }
                if (animojiName == "xiexielaoban")
                {
                    return new string[] { "1" };
                }
                if (animojiName == "zhuoji")
                {
                    return new string[] { "shenshou", "tiqi" };
                }
                if (animojiName == "leishen")
                {
                    return new string[] { "B_hebing" };
                }
                if (animojiName == "crying")
                {
                    return new string[] { "newAnimation" };
                }
                if (animojiName == "stool")
                {
                    return new string[] { "stool", "lose-stool" };
                }
                if (animojiName == "strong")
                {
                    return new string[] { "click_on_the", "strong" };
                }
                if (animojiName == "weak")
                {
                    return new string[] { "click_on_the", "so_weak" };
                }

                return new string[] { };
            }

            private string ToAnimationNames()
            {
                if (animojiName == "jiatelin")
                {
                    return "dankong";
                }
                return "";
            }

            private Vector3 FromPos()
            {
                //if (animojiName == "dushen")
                //{
                //    if (fromSeat.seatUIInfo.Pos.x > 0)
                //    {
                //        return fromSeat.seatUIInfo.Pos + new Vector3(0, -110, 0);
                //    }
                //    else
                //    {
                //        return fromSeat.seatUIInfo.Pos + new Vector3(0, -110, 0);
                //    }

                //}
                return fromSeat.seatUIInfo.Pos;
            }

            private Vector3 ToPos()
            {
                //if (animojiName == "dantiao")
                //{
                //    if (toSeat.seatUIInfo.Pos.x < 0)
                //    {
                //        return toSeat.seatUIInfo.Pos + new Vector3(100, -100, 0);
                //    }
                //    else
                //    {
                //        return toSeat.seatUIInfo.Pos + new Vector3(-100, -100, 0);
                //    }
                //}
                //if (animojiName == "jiatelin")
                //{
                //    return toSeat.seatUIInfo.Pos + new Vector3(0, -20, 0);
                //}
                //if (animojiName == "king")
                //{
                //    if (toSeat.seatUIInfo.Pos.x < 0)
                //    {
                //        return toSeat.seatUIInfo.Pos + new Vector3(200, -100, 0);
                //    }
                //    else
                //    {
                //        return toSeat.seatUIInfo.Pos + new Vector3(-200, -100, 0);
                //    }
                //}
                //if (animojiName == "xiexielaoban")
                //{
                //    if (toSeat.seatUIInfo.Pos.x < 0)
                //    {
                //        return toSeat.seatUIInfo.Pos + new Vector3(200, -100, 0);
                //    }
                //    else
                //    {
                //        return toSeat.seatUIInfo.Pos + new Vector3(-200, -100, 0);
                //    }
                //}
                //if (animojiName == "shayu")
                //{
                //    float angle = Mathf.Atan((fromSeat.seatUIInfo.Pos.x - toSeat.seatUIInfo.Pos.x) / (toSeat.seatUIInfo.Pos.y - fromSeat.seatUIInfo.Pos.y));
                //    if (fromSeat.seatUIInfo.Pos.y - toSeat.seatUIInfo.Pos.y > 0)
                //    {
                //        angle += Mathf.PI;
                //    }
                //    if (toSeat.seatUIInfo.Pos.x >= 0)
                //    {
                //        //右方座位
                //        angle += Mathf.PI * 0.5f;
                //    }
                //    else
                //    {
                //        //左方座位
                //        angle -= Mathf.PI * 0.5f;
                //    }
                //    var dx = toSeat.seatUIInfo.Pos.x - 180 * Mathf.Sin(angle);
                //    var dy = toSeat.seatUIInfo.Pos.y + 180 * Mathf.Cos(angle);//55
                //    return new Vector3(dx, dy, 0);
                //}
                return toSeat.seatUIInfo.Pos;
            }

            private float AnimationTime()
            {
                if (animojiName == "shayu")
                {
                    return 2.26f;
                }
                return 0.7f;
            }

            private bool isFlipX()
            {
                if (animojiName == "dantiao" || animojiName == "king")
                {
                    return toSeat.seatUIInfo.Pos.x < 0;
                }
                if (animojiName == "shayu")
                {
                    return toSeat.seatUIInfo.Pos.x < 0;
                }
                if (animojiName == "dushen")
                {
                    return fromSeat.seatUIInfo.Pos.x > 0;
                }
                if (animojiName == "xiexielaoban")
                {
                    return toSeat.seatUIInfo.Pos.x >= 0;
                }
                return false;
            }

        }

        public void Awake()
        {
            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
        }

        public void PlayAnimoji(string name, Seat fromSeat, Seat toSeat)
        {
            AnimojiInfo animojiInfo = new AnimojiInfo();
            animojiInfo.NewPlaySingleAnimoji(name, fromSeat, toSeat, rc.gameObject.transform);
        }

        // JackPot击中动画,不属于魔法表情，暂时放这里
        public void PlayJackpotHitAnimation(Seat seat, Vector3 fromPos)
        {
            SoundComponent.Instance.PlaySFX($"sfx_jackpot_hit");
            for (int i = 0; i < 20; i++)
            {
                float chipX = RandomHelper.RandomNumber(-300, 300);
                float chipRaduis = RandomHelper.RandomNumber(0, 100) / 100.0f * 180;
                float chipTime = RandomHelper.RandomNumber(0, 15) / 10.0f;
                int name = RandomHelper.RandomNumber(0, 5);

                string bundleName = "chips.unity3d";
                ResourcesComponent mResourcesComponent = Game.Scene.ModelScene.GetComponent<ResourcesComponent>();
                mResourcesComponent.LoadBundle(bundleName);
                GameObject mPrefab = mResourcesComponent.GetAsset(bundleName, $"chip_{name}") as GameObject;
                if (null == mPrefab)
                    return;

                GameObject mObj = GameObject.Instantiate(mPrefab);
                UnityArmatureComponent armatureComp = mObj.GetComponent<UnityArmatureComponent>();
                armatureComp.transform.parent = rc.gameObject.transform;
                armatureComp.transform.localPosition = new Vector3(chipX, fromPos.y - 50f, 0);
                armatureComp.transform.localScale = new Vector3(100, 100, 0);
                armatureComp.dragonAnimation.Reset();
                armatureComp.dragonAnimation.Play();
                armatureComp.gameObject.SetActive(false);

                Sequence sequence = DOTween.Sequence();
                sequence.Append(armatureComp.transform.DOLocalMove(armatureComp.transform.localPosition, chipTime).OnComplete(() => { armatureComp.gameObject.SetActive(true); }));
                sequence.Append(armatureComp.transform.DOLocalMove(seat.seatUIInfo.Pos, 2.0f).SetEase(Ease.Linear));
                sequence.Join(armatureComp.transform.DOLocalRotate(new Vector3(0, 0, chipRaduis), 2.0f, RotateMode.FastBeyond360).SetEase(Ease.Linear));
                // sequence.AppendCallback(() =>
                // {
                //     armatureComp.dragonAnimation.Stop();
                //     GameObject.Destroy(mObj);
                // });
                sequence.OnComplete(() =>
                {
                    armatureComp.dragonAnimation.Stop();
                    GameObject.Destroy(mObj);
                });
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
