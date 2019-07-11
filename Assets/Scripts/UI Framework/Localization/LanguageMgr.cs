using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageMgr
{
    private static LanguageMgr _instance;

    private Dictionary<string, string> _dicLanguageCache;

    private LanguageMgr()
    {
        _dicLanguageCache = new Dictionary<string, string>();

        InitLanguageCache();
    }



    public static LanguageMgr GetInstance()
    {
        if (_instance == null)
        {
            _instance = new LanguageMgr();
        }
        return _instance;
    }

    public string ShowText(string languageID)
    {
        string strQuery = string.Empty;

        if (string.IsNullOrEmpty(languageID)) return null;

        if (_dicLanguageCache != null && _dicLanguageCache.Count >= 1)
        {
            _dicLanguageCache.TryGetValue(languageID, out strQuery);
            if (!string.IsNullOrEmpty(strQuery))
            {
                return strQuery;
            }
        }
        Debug.Log(GetType() + "Language ID  = " + languageID);
        return null;
    }

    private void InitLanguageCache()
    {
        IConfigManager config = new ConfigManagerByJson("LanguageJSONConfig");
        if (config != null)
        {
            _dicLanguageCache = config.AppSetting;
        }
    }
}

