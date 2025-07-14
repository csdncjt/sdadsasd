using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIScrollEventListener : MonoBehaviour, IBeginDragHandler, IEndDragHandler , IPointerUpHandler, IPointerDownHandler , IDragHandler
{
    public delegate void VoidDelegate(PointerEventData pdata);
    public VoidDelegate onBeginDrag;
    public VoidDelegate onEndDrag;
    public VoidDelegate onUp;
    public VoidDelegate onDown;
    public VoidDelegate onDrag;
    bool isDrag = false;
    List<Image> registers = new List<Image>();

    public static UIScrollEventListener Get(GameObject go)
    {
        UIScrollEventListener listener = go.GetComponent<UIScrollEventListener>();
        if (listener == null) listener = go.AddComponent<UIScrollEventListener>();
        return listener;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        if (onBeginDrag != null) onBeginDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        if (onEndDrag != null) onEndDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDrag) {

            SetRegisterEvent(true);
            PraseObject(eventData);
            SetRegisterEvent(false);
        }
        if (onUp != null) onUp(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown(eventData);
    }

    public void OnDrag(PointerEventData eventData) {

        if (onDrag != null) onDrag(eventData);
    }

    public void RegisterButton(GameObject go)
    {
        Image img = go.GetComponent<Image>();
        if (img != null)
        {
            img.raycastTarget = false;
            registers.Add(img);
        }
    }

    void SetRegisterEvent(bool b) {

        if (registers.Count > 0)
        {
            for (int i = 0; i < registers.Count; ++i)
            {
                registers[i].raycastTarget = b;
            }
        }
    }

    void PraseObject(PointerEventData eventData) {

        if (registers.Count > 0)
        {
            for (int i = 0; i < registers.Count; ++i)
            {
                if (EventSystem.current != null)
                {
                    List<RaycastResult> result = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(eventData, result);
                    foreach (RaycastResult r in result)
                    {
                        //Debug.Log(r.gameObject.name);
                        foreach (Image img in registers)
                        {

                            if (img.gameObject.Equals(r.gameObject))
                            {

                                InputField inputfield = img.gameObject.GetComponent<InputField>();
                                if (inputfield != null) inputfield.ActivateInputField();
                            }

                        }
                    }
                }
            }
        }
    }

}