using System.Collections;
using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoAV: MonoBehaviour
{

    public Button ButtonSkin;
    public MediaPlayer MPlayer;
    private AsyncOperation asyncOpt;
    private bool isPlayFinish = false;
    private bool isLoadFinish = false;

    void Awake()
    {
        ButtonSkin.onClick.AddListener(onClickButtonSkin);
        if (null != MPlayer)
        {
            MPlayer.Events.AddListener(MPlayerCall);
        }
    }

    private void onClickButtonSkin()
    {
        if (null != asyncOpt)
            asyncOpt.allowSceneActivation = true;
    }

    // Use this for initialization
    void Start()
    {
        asyncOpt = SceneManager.LoadSceneAsync("Scenes/Init");
        asyncOpt.allowSceneActivation = false;
    }

    private void MPlayerCall(MediaPlayer arg0, MediaPlayerEvent.EventType arg1, ErrorCode arg2)
    {
        if (arg1 == MediaPlayerEvent.EventType.FinishedPlaying)
        {
            isPlayFinish = true;
            if (isPlayFinish && isLoadFinish)
            {
                isPlayFinish = false;
                isLoadFinish = false;
                asyncOpt.allowSceneActivation = true;
            }

        }
    }

    void Update()
    {
        if (asyncOpt.progress >= 0.9f)
        {
            isLoadFinish = true;
        }


        if (isPlayFinish && isLoadFinish)
        {
            isPlayFinish = false;
            isLoadFinish = false;
            asyncOpt.allowSceneActivation = true;
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            //this.LogInfo("游戏进入了后台=》  游戏暂停 一切停止");  // Home到桌面或者打进电话等不适前台的时候
            if (null != this.MPlayer)
            {
                MPlayer.Pause();
            }
        }
        else
        {
            //this.LogInfo("游戏回到了前台  继续监听");  //回到游戏前台的时候触发
            MPlayer.Play();
        }
    }
}
