using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCityUIForm : BaseUIForm
{
    private void Awake()
    {
        CurrentUIType.UIForm_ShowMode = UIFormShowMode.HideOther;

        RegisterButtonObjectEvent("Btn_Market",
            P => OpenUIform(ConstDefine.MARKET_UIFORM));
    }
}
