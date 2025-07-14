using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ETHotfix;

public class InputElement
{
    static public Color[] colors = new Color[2] { new Color(1, 1, 1, 1), new Color(217f / 255f, 41f / 255f, 41f / 255f, 1) };
    static public int GetBarStep(Scrollbar bar) {

        return (int)Math.Round(bar.value * (bar.numberOfSteps - 1));
    }
}


public class ScrollBarElement
{

    public Color[] colors;
    Scrollbar scrollbar;
    Transform container;
    Image progressbar;
    public ScrollBarElement(Scrollbar bar , Image imgbar , Transform con , Color[] col = null)
    {
        scrollbar = bar;
        progressbar = imgbar;
        container = con;//进度字体变色
        if (col == null)
        {
            colors = InputElement.colors;
        }
        OnChange(scrollbar.value);
    }

    public void AddEvent() {

        RemoveEvent();
        scrollbar.onValueChanged.AddListener(OnChange);
        scrollbar.image.color = Color.white;
    }

    public void RemoveEvent() {

        scrollbar.onValueChanged.RemoveListener(OnChange);
        scrollbar.image.color = Color.gray;
    }

    void OnChange(float v) {

        //jindu
        progressbar.fillAmount = scrollbar.value;
        //font
        int curstep = step;
        for (int i = 0; i < scrollbar.numberOfSteps; ++i) {

            if (container.childCount > i) {

                Text txt = container.GetChild(i).GetComponent<Text>();
                if (txt) {

                    if (curstep == i) {

                        txt.color = colors[1];
                    }
                    else txt.color = colors[0];
                }
            }
            else
            {
                break;
            }
        }
    }

    public void SetText(int dex , string str , float dis = -1) {

        if (container.childCount > dex)
        {
            Text txt = container.GetChild(dex).GetComponent<Text>();
            if (txt)
            {
                txt.text = str;
                if (dis != -1)
                {
                    txt.transform.localPosition = new Vector3(dis * 1000, 0 , 0);
                }
            }
        }
    }


    //设置头尾数据
    public void SetHeadText(string str1, string str2)
    {
        List<Text> txt = new List<Text>();
        int txtCount = container.childCount;
        for (int i = 0; i < txtCount; i++)
        {
            var t1 = container.GetChild(i).GetComponent<Text>();
            if (t1)
            {
                txt.Add(t1);
            }

        }

        //var txt = container.GetComponentsInChildren<Text>(true);
        int maxIndex = scrollbar.numberOfSteps;

        for (int i = 0; i < txt.Count; i++)
        {
            if (i == 0)
            {
                txt[i].text = str1;
                txt[i].transform.localPosition = new Vector3(-1 * 500, 0, 0);
            }
            else if (i == maxIndex)
            {
                txt[i].text = str2;
                txt[i].transform.localPosition = new Vector3(1 * 500, 0, 0);
            }
            else
            {
                txt[i].text = $"";
            }
        }
           
    }

    public int step
    {
        get
        {
            return InputElement.GetBarStep(scrollbar);
        }
    }

    public Scrollbar bar
    {
        get
        {
            return scrollbar;
        }
    }
}


public class DoubleScrollBarElement
{


    public Color[] colors;
    Scrollbar scrollbar0;
    Scrollbar scrollbar1;
    Transform container;
    Image progressbar;
    int progressbarLen;
    int startp;
    int endp;

    public DoubleScrollBarElement(Scrollbar bar0, Scrollbar bar1 , Image imgbar, Transform con , Color[] col = null)
    {
        if(bar0.numberOfSteps != bar1.numberOfSteps || bar0.numberOfSteps < 2 || bar1.numberOfSteps < 2)
        {
            Debug.LogError("Error bar0.numberOfSteps != bar1.numberOfSteps");
            return;
        }
        scrollbar0 = bar0;
        scrollbar1 = bar1;
        progressbar = imgbar;
        container = con;
        startp = -1;
        endp = -1;
        progressbarLen = (int)progressbar.transform.parent.GetComponent<RectTransform>().sizeDelta.x;
        if (col == null)
        {
            colors = InputElement.colors;
        }
        else
        {
            colors = col;
        }
        OnChange(scrollbar0.value , scrollbar1.value);
    }

    public void AddEvent()
    {

        RemoveEvent();
        scrollbar0.onValueChanged.AddListener(OnListener);
        scrollbar1.onValueChanged.AddListener(OnListener);
    }

    public void RemoveEvent()
    {

        scrollbar0.onValueChanged.RemoveListener(OnListener);
        scrollbar1.onValueChanged.RemoveListener(OnListener);
    }

    public void SetStartp(int startp)
    {
        if (startp > scrollbar0.numberOfSteps - 1)
        {
            return;
        }
        scrollbar0.value = startp / (float)(scrollbar0.numberOfSteps - 1);
    }

    public void SetEndp(int endp)
    {
        if (endp > scrollbar1.numberOfSteps - 1)
        {
            return;
        }
        scrollbar1.value = endp / (float)(scrollbar1.numberOfSteps - 1);
        // Debug.Log($"endp={endp},{scrollbar1.numberOfSteps - 1},{scrollbar1.value}");
    }

    void OnListener(float v) {

        OnChange(scrollbar0.value, scrollbar1.value);
    }

    void OnChange(float v0 , float v1)
    {

        //font
        int step0 = GetStep(scrollbar0);
        int step1 = GetStep(scrollbar1);
        for (int i = 0; i < scrollbar0.numberOfSteps; ++i)
        {
            if (container.childCount > i)
            {

                Text txt = container.GetChild(i).GetComponent<Text>();
                if (txt)
                {
                    if (step0 == i || step1 == i)
                    {
                        txt.color = colors[1];
                    }
                    else txt.color = colors[0];
                }
            }
        }
        //jindu
        startp = Mathf.Min(step0 , step1);
        endp = Mathf.Max(step0, step1);
        int len = endp - startp;
        int width = len * progressbarLen / (scrollbar0.numberOfSteps - 1);
        int x = startp * progressbarLen / (scrollbar0.numberOfSteps - 1);
        progressbar.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, x, width);
        //Debug.Log($"startp = {startp} ,endp = {endp} , len = {len} , width ={ width} x = {x}");
    }

    int GetStep(Scrollbar bar) {

        return (int)Math.Round(bar.value * (bar.numberOfSteps - 1));
    }

    public int stepStart
    {
        get
        {
            return startp;
        }
    }

    public int stepEnd
    {
        get
        {
            return endp;
        }
    }

    public Scrollbar bar0
    {
        get
        {
            return scrollbar0;
        }
    }

    public Scrollbar bar1
    {
        get
        {
            return scrollbar1;
        }
    }
}


public class ToggleElement
{
    public Color[] colors;
    Toggle mtoggle;
    Text txt;

    public ToggleElement(Toggle tg , Text t , Color[] col = null) {

        if (col == null) {

            colors = InputElement.colors;
        }
        mtoggle = tg;
        txt = t;
        OnChange(mtoggle.isOn);
    }

    public void AddEvent() {

        mtoggle.onValueChanged.AddListener(OnChange);
    }

    public void RemoveEvent() {

        mtoggle.onValueChanged.RemoveListener(OnChange);
    }

    void OnChange(bool b) {

        if (b)
        {
            txt.color = colors[1];
        }
        else
        {
            txt.color = colors[0];
        }
    }

    public Toggle toggle
    {
        get { return mtoggle; }
    }
}


public class AreaGridElment
{
    public delegate void OnClickDelegate(GameObject obj, DBArea dbarea);
    public OnClickDelegate clickDelegate;
    GameObject container;
    GameObject prefab;

    public AreaGridElment(GameObject con , GameObject pre) {

        container = con;
        prefab = pre;
    }

    public void Init(List<DBArea> list , OnClickDelegate clickDelegate) {

        int len = Mathf.Max(list.Count, container.transform.childCount);
        for (int i = 0; i < len; ++i)
        {
            if (i < list.Count)
            {
                GameObject o;
                if (container.transform.childCount > i)
                {
                    o = container.transform.GetChild(i).gameObject;
                }
                else
                {
                    o = GameObject.Instantiate(prefab, container.transform);
                }
                o.SetActive(true);
                o.transform.GetChild(0).GetComponent<Text>().text = LanguageManager.mInstance.GetLanguageForKey($"area{list[i].id}"); //list[i].name_zh;
                DBArea dbarea = list[i];
                UIEventListener.Get(o).onClick = (go) =>
                {
                    clickDelegate(go , dbarea);
                };
            }
            else
            {
                container.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}