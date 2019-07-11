using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManagerByJson : IConfigManager
{
    private Dictionary<string, string> _appSetting;

    /// <summary>
    /// JSON解析出来的键值对字典
    /// </summary>
    public Dictionary<string, string> AppSetting
    {
        get
        {
            return _appSetting;
        }
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="jsonPath">Json配置文件的路径</param>
    public ConfigManagerByJson(string jsonPath)
    {
        _appSetting = new Dictionary<string, string>();

        InitAndAnalysisJson(jsonPath);
    }

    /// <summary>
    /// 得到AppSetting的最大值
    /// </summary>
    /// <returns>AppSetting的最大值</returns>
    public int GetAppSettingMaxNumber()
    {
        if (_appSetting!=null && _appSetting.Count >= 1)
        {
            return _appSetting.Count;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 处理Json文件
    /// </summary>
    /// <param name="jsonPath"></param>
    private void InitAndAnalysisJson(string jsonPath)
    {
        TextAsset configInfo = null;
        KeyValuesInfo keyValuesInfoObj = null;

        if (string.IsNullOrEmpty(jsonPath)) return;
        //解析Json
        try
        {
            configInfo = Resources.Load<TextAsset>(jsonPath);
            keyValuesInfoObj = JsonUtility.FromJson<KeyValuesInfo>(configInfo.text);
        }
        catch
        {
            throw new JsonAnlysisException(GetType() + "Analysis Excepton");
        }
        foreach(KeyValuesNode nodeInfo in keyValuesInfoObj.ConfigInfo)
        {
            _appSetting.Add(nodeInfo.Key, nodeInfo.Value);
        }
    }

}
