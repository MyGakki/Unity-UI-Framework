using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketUIFrom : BaseUIForm
{

    private void Awake()
    {
        CurrentUIType.UIForm_Type = UIFormType.Popup;
        CurrentUIType.UIForm_LucencyType = UIFormLucencyType.TransLucence;
        CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;

        RegisterButtonObjectEvent("Btn_Close",
            P => CloseUIForm());

        RegisterButtonObjectEvent("Btn_Stick",
            P =>
            {
                OpenUIform(ConstDefine.PRO_DETAIL_UIFORM);
                string[] strArray = new string[] { "神杖详情", "神杖详细介绍。。。" };
                SendMessage("Props", "stick", strArray);
            });

        RegisterButtonObjectEvent("Btn_Shoe",
            P =>
            {
                OpenUIform(ConstDefine.PRO_DETAIL_UIFORM);

                string[] strArray = new string[] { "战靴详情", "战靴详细介绍。。。" };
                SendMessage("Props", "shoes", strArray);
            });

        RegisterButtonObjectEvent("Btn_Armour",
            P =>
            {
                OpenUIform(ConstDefine.PRO_DETAIL_UIFORM);

                string[] strArray = new string[] { "盔甲详情", "盔甲详细介绍。。。" };
                SendMessage("Props", "armour", strArray);
            });
    }
}
