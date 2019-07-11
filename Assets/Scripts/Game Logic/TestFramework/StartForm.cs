using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartForm: BaseUIForm
{
    private void Awake()
    {
        EventCenter.AddListener<string>(EventType.GameStart, str => OpenUIform(str));
        RegisterButtonObjectEvent("Btn_Start",g=>EventCenter.Broadcast<string>(EventType.GameStart,
            "Select Hero Form"));
    }
}
