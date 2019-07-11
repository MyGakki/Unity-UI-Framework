using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI窗口（位置）类型
public enum UIFormType
{
    //普通窗口
    Normal,
    //固定窗口
    Fixed,
    //弹出窗口
    Popup
}

//UI窗体的显示类型
public enum UIFormShowMode
{
    //普通
    Normal,
    //反向切换
    ReverseChange,
    //隐藏其他
    HideOther
}

public enum UIFormLucencyType
{
    //完全透明，不能穿透
    Luceny,
    //半透明，不能穿透
    TransLucence,
    //低透明，不能穿透
    ImPenetrable,
    //能穿透
    Penetra
}

public class BaseUIForm : MonoBehaviour
{
    //当前窗口的类型
    private UIType _currentUIType = new UIType();
    internal UIType CurrentUIType
    {
        set
        {
            _currentUIType = value;
        }
        get
        {
            return _currentUIType;
        }
    }

    /// <summary>
    /// 页面显示
    /// </summary>
    public virtual void Display()
    {
        this.gameObject.SetActive(true);
        //对于PopUp窗口调用模态窗体
        if(_currentUIType.UIForm_Type == UIFormType.Popup)
        {
            print("PopUp");
            UIMaskMgr.GetInstance().SetMaskWindow(this.gameObject, _currentUIType.UIForm_LucencyType);
        }
    }

    /// <summary>
    /// 页面隐藏(页面不在栈中)
    /// </summary>
    public virtual void Hiding()
    {
        this.gameObject.SetActive(false);
        //对于PopUp窗口取消模态窗体
        if (_currentUIType.UIForm_Type == UIFormType.Popup)
        {
            UIMaskMgr.GetInstance().CancelMaskWindow();
        }
    }

    /// <summary>
    /// 页面重新显示
    /// </summary>
    public virtual void ReDisplay()
    {
        this.gameObject.SetActive(true);
        //对于PopUp窗口调用模态窗体
        if (_currentUIType.UIForm_Type == UIFormType.Popup)
        {
            UIMaskMgr.GetInstance().SetMaskWindow(this.gameObject, _currentUIType.UIForm_LucencyType);
        }
    }

    /// <summary>
    /// 页面冻结（页面还在栈中）
    /// </summary>
    public virtual void Freeze()
    {
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// 注册按钮事件
    /// </summary>
    /// <param name="buttonName">按钮节点名称</param>
    /// <param name="handle">委托：需要注册的方法</param>
    protected void RegisterButtonObjectEvent(string buttonName, EventTriggerListener.VoidDelegate handle)
    {
        GameObject goButton = UnityHelper.FindTheChildNode(gameObject, buttonName).gameObject;
        if (goButton != null)
        {
            EventTriggerListener.Get(goButton).onClick = handle;
        }
    }

    /// <summary>
    /// 使用UIManager的单例调用ShowUIForms
    /// 打开UI窗体
    /// </summary>
    /// <param name="uiFormName"></param>
    protected void OpenUIform(string uiFormName)
    {
        print("OpenUIform:" + uiFormName);
        UIManager.GetInstance().ShowUIForms(uiFormName);
    }

    /// <summary>
    /// 关闭当前窗口
    /// </summary>
    protected void CloseUIForm()
    {
        string strUIFromName = string.Empty;
        int intPosition = -1;

        strUIFromName = GetType().ToString();

        intPosition = strUIFromName.IndexOf(".");
        if(intPosition != -1)
        {
            strUIFromName = strUIFromName.Substring(intPosition + 1);
        }
        print(strUIFromName);
        UIManager.GetInstance().CloseOrReturnUIForms(strUIFromName);
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msgType">消息的类型</param>
    /// <param name="msgName">消息的名称</param>
    /// <param name="msgContent">消息的内容</param>
    protected void SendMessage(string msgType, string msgName, object msgContent)
    {
        KeyValuesUpdate kvs = new KeyValuesUpdate(msgName, msgContent);
        MessgeCenter.SendMessage(msgType, kvs);
    }

    /// <summary>
    /// 接收消息
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="handler"></param>
    public void ReceiveMessage(string messageType, MessgeCenter.DeleMessageDelivery handler)
    {
        MessgeCenter.AddMsgListener(messageType, handler);
    }

    public string Show(string id)
    {
        string strResult = string.Empty;

        strResult = LanguageMgr.GetInstance().ShowText(id);
        return strResult;
    }
}
