using System.Collections.Generic;
using ETModel;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DragonBones;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIHongBaoYuComponentAwakeSystem : AwakeSystem<UIHongBaoYuComponent>
    {
        public override void Awake(UIHongBaoYuComponent self)
        {
            self.Awake();
        }
    }

    public class UIHongBaoYuComponent : UIBaseComponent
    {
        private static readonly Vector3 defaultTextContent = new Vector3(0,90, 0);
        private static readonly Vector3 targetTextContent = new Vector3(0, 340, 0);
        private sealed class HongBaoYuData
        {
            public Transform transform;
            public UnityArmatureComponent UnityArmature;
            public Text textContent;
            public Sequence sequence;
            public int userId;
            public bool IsPlaying;
        }

        private ReferenceCollector rc;
        private Transform transHongbaoyu;

        private Queue<HongBaoYuData> queueEnter;
        private List<HongBaoYuData> listOut;

        public void Awake()
        {
            queueEnter = new Queue<HongBaoYuData>();
            listOut = new List<HongBaoYuData>();

            rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            transHongbaoyu = rc.Get<GameObject>("Hongbaoyu").transform;
        }

        public void Play(int userId, string number)
        {
            Seat mSeat =  GameCache.Instance.CurGame.GetSeatByUserId(userId);

             //Seat mSeat = GameCache.Instance.CurGame.GetSeatById((sbyte)1);
            // Seat mSeat = GameCache.Instance.CurGame.GetSeatById((sbyte)userId);

            if (mSeat == null)
            {
                return;
            }

            if (null == queueEnter)
                queueEnter = new Queue<HongBaoYuData>();

            HongBaoYuData mHongBaoYuData = null;
            if (queueEnter.Count > 0)
            {
                mHongBaoYuData = queueEnter.Dequeue();
            }
            else
            {
                mHongBaoYuData = new HongBaoYuData();
                GameObject mGo = GameObject.Instantiate(transHongbaoyu.gameObject);
                mGo.name = userId.ToString();
                mGo.transform.SetParent(this.rc.transform);
                mGo.transform.localPosition = Vector3.zero;
                mGo.transform.localRotation = Quaternion.identity;
                mGo.transform.localScale = Vector3.one;
                mHongBaoYuData.transform = mGo.transform;
                mHongBaoYuData.UnityArmature = mHongBaoYuData.transform.Find("armatureName").GetComponent<UnityArmatureComponent>();
                mHongBaoYuData.textContent = mHongBaoYuData.transform.Find("Text").GetComponent<Text>();
            }

            mHongBaoYuData.textContent.transform.localPosition = defaultTextContent;
            mHongBaoYuData.textContent.text = number;
            mHongBaoYuData.textContent.gameObject.SetActive(false);
            mHongBaoYuData.transform.gameObject.SetActive(true);
            mHongBaoYuData.IsPlaying = true;
            mHongBaoYuData.userId = userId;

            listOut.Add(mHongBaoYuData);

            mHongBaoYuData.sequence = DOTween.Sequence(); 

            Vector3 mTargetLocalPos = GameUtil.ChangeToLocalPos(mSeat.Trans.localPosition, mSeat.Trans.parent, this.rc.transform);

            mHongBaoYuData.transform.localPosition = new Vector3(mTargetLocalPos.x, mTargetLocalPos.y + 500, mTargetLocalPos.z);

            Tweener mTweener = mHongBaoYuData.transform.DOLocalMoveY(mTargetLocalPos.y - 100, 1f);

            mTweener.OnStart(() =>
            {
                mHongBaoYuData.UnityArmature.dragonAnimation.Reset();
                mHongBaoYuData.UnityArmature.dragonAnimation.Play("xialuo");
                SoundComponent.Instance.PlaySFX($"activity_hong_bao_yu");
            });
            mHongBaoYuData.sequence.Append(mTweener);

            // mHongBaoYuData.sequence.AppendInterval(0.5f);

            mTweener = mHongBaoYuData.transform.DOScaleX(1, 0.1f);

            mTweener.OnStart(() =>
            {
                mHongBaoYuData.UnityArmature.dragonAnimation.Reset();
                mHongBaoYuData.UnityArmature.dragonAnimation.Play("sadoudou", 1);
            });
            mHongBaoYuData.sequence.Append(mTweener);

            mHongBaoYuData.sequence.AppendInterval(2f);

            mTweener = mHongBaoYuData.textContent.transform.DOLocalMoveY(targetTextContent.y, 1f);
            mTweener.OnStart(() =>
            {
                mHongBaoYuData.textContent.gameObject.SetActive(true);
            });
            mHongBaoYuData.sequence.Append(mTweener);

            mHongBaoYuData.sequence.AppendInterval(1f);

            mHongBaoYuData.sequence.OnComplete(() =>
            {
                mHongBaoYuData.transform.gameObject.SetActive(false);
                mHongBaoYuData.IsPlaying = false;
                queueEnter.Enqueue(mHongBaoYuData);
                for (int i = this.listOut.Count - 1; i >= 0; i--)
                {
                    if (listOut[i].userId == mHongBaoYuData.userId)
                    {
                        listOut.RemoveAt(i);
                    }
                }
            });
        }

        public override void OnShow(object obj)
        {
            
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;

            if (null != this.queueEnter)
            {
                HongBaoYuData mHongBaoYuData = null;

                while (true)
                {
                    if (queueEnter.Count == 0)
                    {
                        break;
                    }

                    mHongBaoYuData = queueEnter.Dequeue();
                    if (null != mHongBaoYuData && null != mHongBaoYuData.transform)
                    {
                        if (mHongBaoYuData.sequence.IsPlaying())
                            mHongBaoYuData.sequence.Kill();
                        GameObject.Destroy(mHongBaoYuData.transform.gameObject);
                    }
                }

                queueEnter.Clear();
                queueEnter = null;
            }

            if (null != listOut)
            {
                HongBaoYuData mHongBaoYuData = null;
                for (int i = 0, n = listOut.Count; i < n; i++)
                {
                    mHongBaoYuData = listOut[i];
                    if (null != mHongBaoYuData && null != mHongBaoYuData.transform)
                    {
                        if (mHongBaoYuData.sequence.IsPlaying())
                            mHongBaoYuData.sequence.Kill();

                        GameObject.Destroy(mHongBaoYuData.transform.gameObject);
                    }   
                }

                listOut.Clear();
                listOut = null;
            }

            base.Dispose();
        }

        public void Stop(int userId)
        {
            HongBaoYuData mHongBaoYuData = null;
            for (int i = this.listOut.Count - 1; i >= 0; i--)
            {
                mHongBaoYuData = listOut[i];
                if (null != mHongBaoYuData && mHongBaoYuData.userId == userId)
                {
                    if (mHongBaoYuData.sequence.IsPlaying())
                        mHongBaoYuData.sequence.Kill();

                    mHongBaoYuData.IsPlaying = false;
                    mHongBaoYuData.transform.gameObject.SetActive(false);
                    queueEnter.Enqueue(mHongBaoYuData);
                }

                listOut.RemoveAt(i);
            }
        }

        public void AllStop()
        {
            if (null != listOut)
            {
                HongBaoYuData mHongBaoYuData = null;
                for (int i = 0, n = listOut.Count; i < n; i++)
                {
                    mHongBaoYuData = listOut[i];
                    if (null != mHongBaoYuData && null != mHongBaoYuData.transform)
                    {
                        if (mHongBaoYuData.sequence.IsPlaying())
                            mHongBaoYuData.sequence.Kill();

                        mHongBaoYuData.IsPlaying = false;
                        mHongBaoYuData.transform.gameObject.SetActive(false);
                        queueEnter.Enqueue(mHongBaoYuData);
                    }
                }

                listOut.Clear();
            }
        }
    }
}
