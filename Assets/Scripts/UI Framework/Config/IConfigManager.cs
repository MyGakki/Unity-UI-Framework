using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IConfigManager
{
    /// <summary>
    /// 只读属性，应用设置
    /// 得到键值对集合数据
    /// </summary>
    Dictionary<string, string> AppSetting { get; }

    /// <summary>
    /// 得到配置文件的最大数量
    /// </summary>
    /// <returns></returns>
    int GetAppSettingMaxNumber();
}

[Serializable]
internal class KeyValuesInfo
{
    /// <summary>
    /// JSON解析出来的键值对的列表
    /// </summary>
    public List<KeyValuesNode> ConfigInfo = null;
}

[Serializable]
internal class KeyValuesNode
{
    /// <summary>
    /// 键
    /// </summary>
    public string Key = null;
    /// <summary>
    /// 值
    /// </summary>
    public string Value = null;
}
