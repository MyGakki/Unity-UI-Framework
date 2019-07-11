using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerListener : EventTrigger
{
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdataSelect;

    /// <summary>
    /// 得到GameObject上的EventTriggerListener组件
    /// </summary>
    /// <param name="go">监听的游戏对象</param>
    /// <returns>对象上的EventTriggerListener组件</returns>
    public static EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null)
        {
            listener = go.AddComponent<EventTriggerListener>();
        }
        return listener;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
        {
            onClick(gameObject);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null)
        {
            onDown(gameObject);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null)
        {
            onEnter(gameObject);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null)
        {
            onExit(gameObject);
        }
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null)
        {
            onUp(gameObject);
        }
    }

    public override void OnSelect(BaseEventData eventBaseData)
    {
        if (onSelect != null)
        {
            onSelect(gameObject);
        }
    }

    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if(onUpdataSelect != null)
        {
            onUpdataSelect(gameObject);
        }
    }
}
