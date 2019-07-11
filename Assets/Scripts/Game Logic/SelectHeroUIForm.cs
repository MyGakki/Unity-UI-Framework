using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHeroUIForm : BaseUIForm
{
    void Awake()
    {
        CurrentUIType.UIForm_ShowMode = UIFormShowMode.HideOther;

        RegisterButtonObjectEvent("Btn_Confirm",
            P=> {
                OpenUIform(ConstDefine.MAIN_CITY_UIFORM);
                OpenUIform(ConstDefine.HERO_INFO_UIFORM);
            });

        RegisterButtonObjectEvent("Btn_Close",
            P => CloseUIForm());
          
    }
}
