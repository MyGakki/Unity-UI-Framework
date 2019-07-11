using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessgeCenter
{
    public delegate void DeleMessageDelivery(KeyValuesUpdate kv);

    public static Dictionary<string, DeleMessageDelivery> _dicMessages = new Dictionary<string, DeleMessageDelivery>();

    /// <summary>
    /// 增加消息的监听
    /// </summary>
    /// <param name="messageType">消息类型</param>
    /// <param name="handler">处理消息的委托</param>
    public static void AddMsgListener(string messageType, DeleMessageDelivery handler)
    {
        if (!_dicMessages.ContainsKey(messageType))
        {
            _dicMessages.Add(messageType, null);
        }
        _dicMessages[messageType] += handler;
    }

    /// <summary>
    /// 取消消息的监听
    /// </summary>
    /// <param name="messageType">消息的类型</param>
    /// <param name="handler">处理消息的委托</param>
    public static void RemoveMsgListener(string messageType, DeleMessageDelivery handler)
    {
        if (_dicMessages.ContainsKey(messageType))
        {
            _dicMessages[messageType] -= handler;
        }
    }

    /// <summary>
    /// 取消所有消息的监听
    /// </summary>
    public static void ClearAllMsgListener()
    {
        if (_dicMessages != null)
        {
            _dicMessages.Clear();
        }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="messageType">消息的分类</param>
    /// <param name="kv">键值对</param>
    public static void SendMessage(string messageType, KeyValuesUpdate kv)
    {
        DeleMessageDelivery del;
        if (_dicMessages.TryGetValue(messageType, out del))
        {
            if (del != null)
            {
                del(kv);
            }
        }
    }
}

/// <summary>
/// 键值对
/// 配合委托，实现委托数据传递
/// </summary>
public class KeyValuesUpdate
{
    //键（消息的名称）
    private string _key;
    //值
    private object _values;


    public string Key
    {
        get { return _key; }
    }

    public object Values
    {
        get { return _values; }
    }

    public KeyValuesUpdate(string key, object valueObj)
    {
        _key = key;
        _values = valueObj;
    }
}
