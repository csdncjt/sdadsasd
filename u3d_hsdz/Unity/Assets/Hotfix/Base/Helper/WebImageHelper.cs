using System;
using UnityEngine;
using UnityEngine.UI;
using BestHTTP;
using ETModel;
using System.Collections.Generic;

namespace ETHotfix
{
    public static class WebImageHelper
    {
        private static Texture2D defaultHead;
        private static Dictionary<string, Texture> mUrlTexture = new Dictionary<string, Texture>();//key=后缀,value=Texture

        public static void SetHeadImage(RawImage rawImage, string headID)
        {
            string url = $"{GlobalData.Instance.HeadUrl}{headID}";
            Texture tTex = null;
            if (mUrlTexture.TryGetValue(url, out tTex))
            {
                rawImage.texture = tTex;
            }
            else
            {
                if (null == defaultHead)
                {
                    ResourcesComponent mResourcesComponent = Game.Scene.ModelScene.GetComponent<ResourcesComponent>();
                    mResourcesComponent.LoadBundle("unit.unity3d");
                    GameObject mPrefab = mResourcesComponent.GetAsset("unit.unity3d", "Unit") as GameObject;
                    if (null != mPrefab)
                    {
                        GameObject mObj = GameObject.Instantiate(mPrefab);
                        mObj.transform.localPosition = Vector3.zero;
                        mObj.transform.localScale = Vector3.one;
                        ReferenceCollector mRc = mObj.GetComponent<ReferenceCollector>();
                        defaultHead = mRc.Get<Texture2D>("hand_iocn_default_avatar");
                    }
                }
                rawImage.texture = defaultHead;
                BestHTTP.HTTPManager.SendRequest(url, HTTPMethods.Get,
                         (request, response) =>
                         {
                             switch (request.State)
                             {
                                 case HTTPRequestStates.Finished:
                                     if (response.IsSuccess)
                                     {
                                         rawImage.texture = response.DataAsTexture2D;
                                         mUrlTexture[url] = response.DataAsTexture2D;
                                     }
                                     break;
                                 default:
                                     break;
                             }
                         });
            }

        }

        public static void SetUrlImage(RawImage rawImage, string fullUrl)
        {
            Texture tTex = null;
            if (mUrlTexture.TryGetValue(fullUrl, out tTex))
            {
                rawImage.texture = tTex;
            }
            else
            {
                rawImage.texture = null;
                BestHTTP.HTTPManager.SendRequest(fullUrl, HTTPMethods.Get,
                         (request, response) =>
                         {
                             switch (request.State)
                             {
                                 case HTTPRequestStates.Finished:
                                     if (response.IsSuccess)
                                     {
                                         rawImage.texture = response.DataAsTexture2D;
                                         mUrlTexture[fullUrl] = response.DataAsTexture2D;
                                     }
                                     break;
                                 default:
                                     break;
                             }
                         });
            }

        }


        private static Dictionary<string, byte[]> mBannerImages = new Dictionary<string, byte[]>();
        public static void SetHttpBanner(RawImage rawImage, string pLastUrl, Action pAct = null)
        {
            byte[] t2dData = null;
            if (mBannerImages.TryGetValue(pLastUrl, out t2dData))
            {
                Texture2D t2d = new Texture2D(100, 100);
                t2d.LoadImage(t2dData);
                rawImage.texture = t2d;
                if (pAct != null)
                    pAct();
                return;
            }
            try
            {
                BestHTTP.HTTPManager.SendRequest((GlobalData.Instance.BannerImageUrl + pLastUrl), HTTPMethods.Get,
                (request, response) =>
                {
                    switch (request.State)
                    {
                        case HTTPRequestStates.Finished:
                            if (response.IsSuccess)
                            {
                                byte[] data = response.Data.Clone() as byte[];
                                if (!mBannerImages.ContainsKey(pLastUrl))
                                {
                                    mBannerImages.Add(pLastUrl, data);
                                }
                                Texture2D t2d = new Texture2D(100, 100);
                                t2d.LoadImage(data);
                                if (rawImage != null)//回调到t2d  rawImage 已destroy                            
                                    rawImage.texture = t2d;
                           
                                if (pAct != null)
                                    pAct();
                            }
                            break;
                        default:
                            break;
                    }
                });
            }
            catch
            {

            }
        }

        /// <summary>
        /// SetHttpBanner很相同...多个pRawImgName
        /// </summary>
        public static void SetUIMatchBanner(RawImage rawImage, string pLastUrl,string pRawImgName, Action pAct)
        {
            byte[] t2dData = null;
            if (mBannerImages.TryGetValue(pLastUrl, out t2dData))
            {
                Texture2D t2d = new Texture2D(100, 100);
                t2d.LoadImage(t2dData);
                rawImage.texture = t2d;
                rawImage.texture.name = pRawImgName;
                if (pAct != null)
                    pAct();
                return;
            }
            try
            {
                BestHTTP.HTTPManager.SendRequest((GlobalData.Instance.BannerImageUrl + pLastUrl), HTTPMethods.Get,
                (request, response) =>
                {
                    switch (request.State)
                    {
                        case HTTPRequestStates.Finished:
                            if (response.IsSuccess)
                            {
                                byte[] data = response.Data.Clone() as byte[];
                                if (!mBannerImages.ContainsKey(pLastUrl))
                                {
                                    mBannerImages.Add(pLastUrl, data);
                                }
                                Texture2D t2d = new Texture2D(100, 100);
                                t2d.LoadImage(data);
                                if (rawImage != null)//回调到t2d  rawImage 已destroy
                                {
                                    rawImage.texture = t2d;
                                    rawImage.texture.name = pRawImgName;
                                }
                                if (pAct != null)
                                    pAct();
                            }
                            break;
                        default:
                            break;
                    }
                });
            }
            catch
            {

            }
        }



        public static Texture2D GetDefaultHead()
        {

            if (null == defaultHead)
            {
                ResourcesComponent mResourcesComponent = Game.Scene.ModelScene.GetComponent<ResourcesComponent>();
                mResourcesComponent.LoadBundle("unit.unity3d");
                GameObject mPrefab = mResourcesComponent.GetAsset("unit.unity3d", "Unit") as GameObject;
                if (null != mPrefab)
                {
                    GameObject mObj = GameObject.Instantiate(mPrefab);
                    mObj.transform.localPosition = Vector3.zero;
                    mObj.transform.localScale = Vector3.one;
                    ReferenceCollector mRc = mObj.GetComponent<ReferenceCollector>();
                    defaultHead = mRc.Get<Texture2D>("hand_iocn_default_avatar");
                }
            }
            return defaultHead;
        }

        public static void GetHeadImage(string headID, Action<Texture2D> action)
        {
            //返回默认头像
            if (headID == "-1" && action != null)
            {
                action(GetDefaultHead());
                return;
            }
            BestHTTP.HTTPManager.SendRequest($"{GlobalData.Instance.HeadUrl}{headID}", HTTPMethods.Get,
                     (request, response) =>
                     {
                         switch (request.State)
                         {
                             case HTTPRequestStates.Finished:
                                 if (response.IsSuccess)
                                 {
                                     //Log.Debug($"response.Data.Length = {response.Data.Length}");
                                     action(response.DataAsTexture2D);
                                 }
                                 else
                                 {
                                     action(GetDefaultHead());
                                 }
                                 break;
                             default:
                                 action(GetDefaultHead());
                                 break;
                         }
                     });
        }
    }
}