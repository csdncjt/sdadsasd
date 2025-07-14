using System;
using System.Collections.Generic;
using System.Net;
using BestHTTP;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{



    public class ScrollObj
    {
        public GameObject obj;
        public int dex;
    }

    public class CZScrollRect
    {

        public enum TipType
        {
            UNDO_REFRESH = 0,
            PULL_REFRESH = 1,
            UNDO_APPEND = 2,
            PULL_AAPEND = 3,
            NODATA = 4,
            NONE = 5
        }

        const int OPEAT_HEIGHT = 100;//高度差判断操作类型
        const int INIT_NUM_LIMIT = 8;//列表实例化个数

        public delegate void OperatDelegate();
        public delegate void OperatObjDelegate(GameObject obj , int index);
        public delegate void OperatTextObjDelegate(GameObject obj, TipType t);
        public OperatDelegate onRefresh;//下拉刷新时回调
        public OperatDelegate onAppend;//需要加载更多时回调
        public OperatObjDelegate onScrollObj;//需刷新时回调
        public OperatTextObjDelegate onUpdateTextObj;//需刷新文本状态时回调

        public ScrollRect scrollRect;//ScrollRect
        private RectTransform scrollRectContent;//RectTransform
        public GameObject prefab;//实例化的对象
        public GameObject text_up;//下拉刷新文本
        public GameObject text_down;//上拉加载更多文本

        TipType textup_status;

        int opeartLen = 0;//记录总长度
        public int layoutwidth = 1242;//填写item的长度
        public int limitNum = 8;//列表实例化个数
        public float interval = 200;//每个item的高度
        public float spacing = 5;//每个tiem的间隔

        ScrollObj[] list;//用于管理生成的对象
        int opeartType;
        int pageindex;//页码
        bool bHasMore;//是否能加载更多
        int halfWidth;

        //
        public GameObject batchContent;

        public CZScrollRect()
        {
            opeartType = -1;
            hasMore = false;
        }

        /// <summary>
        /// 用于控制scrollrect是否能够滑动
        /// （用于等待网络请求等业务）
        /// </summary>
        public bool vertical
        {
            get{
                return scrollRect.vertical;
            }set{
                scrollRect.vertical = value;
            }
        }

        /// <summary>
        /// 是否存在更多
        /// </summary>
        public bool hasMore
        {
            get{
                return bHasMore;
            }set{
                bHasMore = value;
            }
        }

        /// <summary>
        /// 获取对象所在的索引
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetObjIndex(GameObject obj) {

            for(int i = 0; i < list.Length; ++i)
            {
                if (obj.Equals(list[i].obj))
                {
                    return list[i].dex;
                }
            }
            return -1;
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        public void Init() {

            list = new ScrollObj[Mathf.Max(limitNum, INIT_NUM_LIMIT)];
            scrollRectContent = scrollRect.content;
            halfWidth = layoutwidth / 2;
            //此处监听drag事件
            UIScrollEventListener.Get(scrollRect.gameObject).onDrag = (data) =>
            {
                float recty = -scrollRectContent.rect.y - scrollRect.GetComponent<RectTransform>().sizeDelta.y;//Log.Debug($"{scrollRectContent.anchoredPosition.y} , {recty} , {-scrollRectContent.rect.y}");
                if (scrollRectContent.anchoredPosition.y >= recty + OPEAT_HEIGHT)
                {
                    if (bHasMore)
                    {
                        //松开可以加载更多
                        if (textup_status != TipType.UNDO_APPEND) {

                            textup_status = TipType.UNDO_APPEND;
                            if (onUpdateTextObj != null) onUpdateTextObj(text_down, TipType.UNDO_APPEND);
                        }
                    }
                    else
                    {
                        //没有更多数据了
                        if (textup_status != TipType.NODATA)
                        {
                            textup_status = TipType.NODATA;
                            if (onUpdateTextObj != null) onUpdateTextObj(text_down, TipType.NODATA);
                        }

                    }
                    opeartType = 1;
                }
                else if (scrollRectContent.anchoredPosition.y > recty)
                {
                    if (bHasMore)
                    {
                        //上拉可以加载更多
                        if (textup_status != TipType.PULL_AAPEND)
                        {
                            textup_status = TipType.PULL_AAPEND;
                            if (onUpdateTextObj != null) onUpdateTextObj(text_down, TipType.PULL_AAPEND);
                        }
                    }
                    else
                    {
                        //没有更多数据了
                        if (textup_status != TipType.NODATA)
                        {
                            textup_status = TipType.NODATA;
                            if (onUpdateTextObj != null) onUpdateTextObj(text_down, TipType.NODATA);
                        }
                    }
                    opeartType = -1;
                }
                else if (scrollRectContent.anchoredPosition.y <= -OPEAT_HEIGHT)
                {
                    //松开可以刷新
                    if (textup_status != TipType.UNDO_REFRESH)
                    {
                        textup_status = TipType.UNDO_REFRESH;
                        if (onUpdateTextObj != null) onUpdateTextObj(text_up, TipType.UNDO_REFRESH);
                    }
                    opeartType = 0;
                }
                else if (scrollRectContent.anchoredPosition.y < 0)
                {
                    //下拉可以刷新
                    if (textup_status != TipType.PULL_REFRESH)
                    {
                        textup_status = TipType.PULL_REFRESH;
                        if (onUpdateTextObj != null) onUpdateTextObj(text_up, TipType.PULL_REFRESH);
                    }
                    opeartType = -1;
                }
                else
                {
                    opeartType = -1;
                }
                UpdateUiInfo();
            };

            //此处是监听鼠标点击事件  
            UIScrollEventListener.Get(scrollRect.gameObject).onUp = (data) =>
            {
                if (opeartType == 0)
                {
                    if (onRefresh != null)
                    {
                        scrollRect.vertical = false;
                        onRefresh();
                    }
                }
                else if (opeartType == 1)
                {
                    if (bHasMore && onRefresh != null)
                    {
                        scrollRect.vertical = false;
                        onAppend();
                    }
                }
            };

            scrollRect.onValueChanged.RemoveAllListeners();
            scrollRect.onValueChanged.AddListener(OnScrollChange);
        }

        void OnScrollChange(Vector2 v) {

            //TOP TO BUTTOM  计算页码
            int curIndex = Mathf.Min(Mathf.Max(Mathf.FloorToInt(scrollRectContent.anchoredPosition.y / (interval + spacing)) - 1, 0), opeartLen - limitNum);
            UpdatePageIndex(curIndex);
        }

        /// <summary>
        /// 刷新页面 （重置长度）
        /// </summary>
        /// <param name="len"></param>
        public void Refresh(int len)
        {
            //Debug.Log($"Refresh len = {len}");
            if (len < 0)return;
            int count = 0;
            for (int i = 0; i < list.Length; ++i) {

                if (list[i] == null) list[i] = new ScrollObj();
                if (i < len) {

                    if (list[i].obj == null)
                    {
                        list[i].obj = GetNewObject();
                    }
                    list[i].dex = i;
                    list[i].obj.SetActive(true);
                    SetPosition(list[i].obj, list[i].dex);
                    if (onScrollObj != null) onScrollObj(list[i].obj, list[i].dex);
                    count++;
                }
                else
                {
                    if(list[i].obj != null) list[i].obj.SetActive(false);
                }
            }
            opeartLen = len;
            UpdatePageIndex(0);//重置页码
            scrollRectContent.localPosition = Vector3.zero;
            scrollRect.verticalScrollbar.value = 1;
            UpdateUiInfo();
            scrollRect.vertical = true;
        }

        /// <summary>
        /// 追加长度
        /// </summary>
        /// <param name="len"></param>
        public void Append(int len)
        {
            //Debug.Log($"Append len = {len}");
            if (len < 0) return;
            if(len == 0)
            {
                if (onUpdateTextObj != null) {

                    onUpdateTextObj(text_up, TipType.NONE);
                    onUpdateTextObj(text_down, TipType.NONE);
                }
            }
            if (opeartLen < list.Length) {

                int showlen = Mathf.Min(list.Length - opeartLen , len);//Debug.Log($"showlen = {showlen}");
                for (int i = 0; i < showlen; ++i) {

                    
                    int dex = opeartLen + i;//Debug.Log(dex);
                    if (list[dex] == null) list[dex] = new ScrollObj();
                    if (list[dex].obj == null)
                    {
                        list[dex].obj = GetNewObject();
                    }
                    list[dex].dex = dex;
                    list[dex].obj.SetActive(true);
                    SetPosition(list[i].obj, list[i].dex);
                    if (onScrollObj != null) onScrollObj(list[dex].obj, list[dex].dex);
                }
            }
            opeartLen += len;
            UpdateUiInfo();
            scrollRect.vertical = true;
        }

        /// <summary>
        /// 实时刷新页面
        /// </summary>
        /// <param name="pdex"></param>
        void UpdatePageIndex(int pdex) {

            if (opeartLen <= list.Length || pageindex == pdex) return;//Debug.Log($"pdex = {pdex}");
            int x = Mathf.FloorToInt(pdex / limitNum);
            int y = pdex % limitNum;
            for (int i = 0; i < limitNum; ++i)
            {
                int d = 0;
                if (i < y)
                {
                    d = (x + 1) * limitNum + i;
                }
                else
                {
                    d = Mathf.Max(x, 0) * limitNum + i;
                }
                if (list[i].dex != d) {

                    list[i].dex = d;
                    if (list[i].obj != null) {

                        SetPosition(list[i].obj, list[i].dex);
                        if (onScrollObj != null) onScrollObj(list[i].obj, list[i].dex);
                    }
                }
            }
            pageindex = pdex;
        }

        /// <summary>
        /// 刷新content的高度
        /// </summary>
        void UpdateUiInfo() {

            //Debug.Log($"opeartLen = {opeartLen} {opeartLen * (interval + spacing)} - {scrollRect.GetComponent<RectTransform>().sizeDelta.y}");
            scrollRectContent.sizeDelta = new Vector2(0, Math.Max(opeartLen * (interval + spacing), scrollRect.GetComponent<RectTransform>().sizeDelta.y));
            if(text_up != null) text_up.transform.localPosition = new Vector3(text_up.transform.localPosition.x, OPEAT_HEIGHT - 50, 0);
            if (text_down != null) text_down.transform.localPosition = new Vector3(text_down.transform.localPosition.x, -scrollRectContent.sizeDelta.y - OPEAT_HEIGHT + 50, 0);
        }

        void SetPosition(GameObject obj , int dex) {

            //obj.transform.localPosition = new Vector3(0, -dex * (interval + spacing) - interval / 2, 0);
            float y = -dex * (interval + spacing) - interval;
            obj.transform.GetComponent<RectTransform>().offsetMin = new Vector2(0 , y);
            obj.transform.GetComponent<RectTransform>().offsetMax = new Vector2(0, y + interval);
        }

        GameObject GetNewObject() {

            return GameObject.Instantiate(prefab, scrollRectContent);
        }

        public void Dispose()
        {
            scrollRect.onValueChanged.RemoveAllListeners();
            scrollRect = null;
            scrollRectContent = null;
            prefab = null;
            text_up = null;
            text_down = null;
            list = null;
            onAppend = null;
            onRefresh = null;
            onScrollObj = null;
        }
    }
}


