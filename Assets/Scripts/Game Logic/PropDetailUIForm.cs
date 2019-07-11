using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropDetailUIForm : BaseUIForm
{
    public Text TextName;
    public Text TextDetail;

    private void Awake()
    {
        CurrentUIType.UIForm_Type = UIFormType.Popup;
        CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        CurrentUIType.UIForm_LucencyType = UIFormLucencyType.TransLucence;

        RegisterButtonObjectEvent("Btn_Close",
            p => CloseUIForm());

        ReceiveMessage("Props",
            p =>
            {
                if (TextName)
                {
                    string[] strArray = p.Values as string[];
                    TextName.text = strArray[0];
                    TextDetail.text = strArray[1];
                }
            });
    }
}
