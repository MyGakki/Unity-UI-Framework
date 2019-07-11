using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUIForm : BaseUIForm
{
    public Text TextLoginName;
    public Text TextLoginButtonName;

    private void Awake()
    {
        RegisterButtonObjectEvent("Btn_Login",
            P => OpenUIform(ConstDefine.SELECT_HERO_FORM));
    }
    private void Start()
    {
        //string strDisplayInfo = LauguageMgr.GetInstance().ShowText("LogonSystem");

        if (TextLoginName)
        {
            TextLoginName.text = Show("LoginSystem");
        }
        if (TextLoginButtonName)
        {
            TextLoginButtonName.text = Show("LoginSystem");
        }
    }

}
