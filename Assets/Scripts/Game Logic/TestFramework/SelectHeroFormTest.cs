using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHeroFormTest : BaseUIForm
{

    private void Awake()
    {
        CurrentUIType.UIForm_ShowMode = UIFormShowMode.HideOther;
    }
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.AddListener<string>(EventType.LookOverHreoInfo, str => OpenUIform(str));
        RegisterButtonObjectEvent("Btn_SwordsmanDetail", g => EventCenter.Broadcast<string>(EventType.LookOverHreoInfo,
             "Hero Info Form"));
        RegisterButtonObjectEvent("Btn_MagicianDetail", g => EventCenter.Broadcast<string>(EventType.LookOverHreoInfo,
             "Hero Info Form"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
