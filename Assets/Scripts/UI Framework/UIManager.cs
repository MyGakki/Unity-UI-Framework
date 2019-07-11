using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance = null;
    //UI窗体预设路径（窗体预设名称， 窗体预设路径）
    private Dictionary<string, string> _dicFormsPaths;
    //缓存所有的窗体
    private Dictionary<string, BaseUIForm> _dicAllUIForms;
    //当前的显示窗体
    private Dictionary<string, BaseUIForm> _dicCurrentUIForms;
    //表示“当前UI窗体”集合的栈
    private Stack<BaseUIForm> _stackCurrentUIForms;
    //UI根节点
    private Transform _traCanvasTransform = null;
    //全屏幕显示的节点
    private Transform _traNormal = null;
    //固定显示的节点
    private Transform _traFixed = null;
    //弹出显示的节点
    private Transform _traPopUp = null;
    //UI管理脚本的节点
    private Transform _traUIScripts = null;

    //获得单例
    public static UIManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameObject("UIManager").AddComponent<UIManager>();
        }
        return _instance;
    }

    //初始化核心数据，加载"UI窗口路径"到集合
    public void Awake()
    {
        //字典初始化
        _dicAllUIForms = new Dictionary<string, BaseUIForm>();
        _dicCurrentUIForms = new Dictionary<string, BaseUIForm>();
        _dicFormsPaths = new Dictionary<string, string>();
        _stackCurrentUIForms = new Stack<BaseUIForm>();
        //初始化加载(根UI窗口)Canvas预设
        InitRootCanvasLoading();
        //得到UI根节点、全屏节点、固定节点、弹出节点
        _traCanvasTransform = GameObject.FindGameObjectWithTag(SysDefine.CANVAS_TAG).transform;
        _traNormal = _traCanvasTransform.Find(SysDefine.NORMAL_UI_NODE);
        _traFixed = _traCanvasTransform.Find(SysDefine.FIXED_UI_NODE);
        _traPopUp = _traCanvasTransform.Find(SysDefine.POP_UI_NODE);
        _traUIScripts = _traCanvasTransform.Find(SysDefine.UISCRIPTS_NODE);
        //把本脚本作为“根UI窗口”的子节点
        this.gameObject.transform.SetParent(_traUIScripts, false);
        //"根UI窗口"在场景转换时，不允许销毁
        DontDestroyOnLoad(_traCanvasTransform);
        //初始化“UI窗体预设”路径数据
        InitUIFormPathData();

    }

    /// <summary>
    /// 显示UI窗口
    /// 1.根据UI窗体的名称，加载到“所有UI窗体”缓存集合中
    /// 2.根据不同的UI窗体的“显示模式”，分别做不同的加载处理
    /// </summary>
    /// <param name="uriFormName">UI窗体预设的名称</param>
    public void ShowUIForms(string uriFormName)
    {
        Debug.Log("ShowUIForms:" + uriFormName);
        BaseUIForm baseUIForm = null;

        //参数检查
        if (string.IsNullOrEmpty(uriFormName)) return;
        
        //加载UI窗体到所有窗体缓存中
        baseUIForm = LoadFormsFromAllUIFormsCache(uriFormName);
        if (baseUIForm == null) return;

        //判断是否清空“栈”集合
        if (baseUIForm.CurrentUIType.IsClearReverseChange)
        {
            ClearStackArray();
        }

        switch(baseUIForm.CurrentUIType.UIForm_ShowMode)
        {
            case UIFormShowMode.Normal:
                EnterUIformCache(uriFormName);
                break;
            case UIFormShowMode.ReverseChange:
                PushUIForms(uriFormName);
                break;
            case UIFormShowMode.HideOther:
                EnterUIFormsToCacheHideOther(uriFormName);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 关闭或返回上一个UI窗口（关闭当前UI窗口）
    /// </summary>
    /// <param name="strUIFormName"></param>
    public void CloseOrReturnUIForms(string strUIFormName)
    {
        BaseUIForm baseUIForm = null;

        if (string.IsNullOrEmpty(strUIFormName)) return;

        //"所有UI窗口缓存"如果没有记录，则直接返回
        _dicAllUIForms.TryGetValue(strUIFormName, out baseUIForm);
        print(baseUIForm==null);
        if (baseUIForm == null) return;

        //根据窗口的显示模式，进行退出处理
        switch(baseUIForm.CurrentUIType.UIForm_ShowMode)
        {
            case UIFormShowMode.Normal:
                ExitUIFormCache(strUIFormName);
                break;
            case UIFormShowMode.ReverseChange:
                PopUIForms();
                break;
            case UIFormShowMode.HideOther:
                ExitUIFormFromCacheAndShowOther(strUIFormName);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 初始化加载(根UI窗口)Canvas预设
    /// </summary>
    private void InitRootCanvasLoading()
    {
        ResourcesMgr.GetInstance().LoadAsset(SysDefine.CANVAS_TAG, false);
    }

    /// <summary>
    /// 初始化“UI窗口预设”路径数据
    /// </summary>
    private void InitUIFormPathData()
    {
        IConfigManager configMgr = new ConfigManagerByJson(SysDefine.SYS_PATH_UIFORMS_CONFIG_INFO);

        if (_dicFormsPaths != null)
        {
            _dicFormsPaths = configMgr.AppSetting;
        }
    }

    private bool ClearStackArray()
    {
        if (_stackCurrentUIForms != null && _stackCurrentUIForms.Count >= 1)
        {
            _stackCurrentUIForms.Clear();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 根据UI窗体的名称，从_dicAllUIForms中加载，如果加载不出来，调用LoadUIForm从Resources中加载
    /// </summary>
    /// <param name="uriFormsName">UI窗体（预设）的名称</param>
    /// <returns>加载的UI窗体基类</returns>
    private BaseUIForm LoadFormsFromAllUIFormsCache(string strFormsName)
    {
        Debug.Log("LoadFormsToAllUIFormsCache:" + strFormsName);
        BaseUIForm baseUIResult = null;

        //判断“所有UI集合缓存”中是否有制定的UI窗体，没有则加载窗体
        _dicAllUIForms.TryGetValue(strFormsName, out baseUIResult);
        if (baseUIResult == null)
        {
            //加载制定名称的“UI窗体”
            baseUIResult = LoadUIForm(strFormsName);
        }

        return baseUIResult;
    }

    /// <summary>
    /// 把窗体加载到“当前窗体”集合中
    /// </summary>
    /// <param name="uriFormName">窗体预设的名称</param>
    private void EnterUIformCache(string uriFormName)
    {
        BaseUIForm baseUIForm;
        BaseUIForm baseUIFormFromAllCache;

        _dicCurrentUIForms.TryGetValue(uriFormName, out baseUIForm);
        if (baseUIForm != null)
            return;
        _dicAllUIForms.TryGetValue(uriFormName, out baseUIFormFromAllCache);
        if (baseUIFormFromAllCache != null)
        {
            _dicCurrentUIForms.Add(uriFormName, baseUIFormFromAllCache);
            baseUIFormFromAllCache.Display();
        }
    }

    /// <summary>
    /// 卸载UI窗体从“当前显示窗体集合”缓存中
    /// </summary>
    /// <param name="strUIFormName"></param>
    private void ExitUIFormCache(string strUIFormName)
    {
        BaseUIForm baseUIForm;

        //“缓存集合”中没有记录，直接返回
        _dicCurrentUIForms.TryGetValue(strUIFormName, out baseUIForm);
        if (baseUIForm == null) return;

        //指定的UI窗体隐藏，并从缓存中移除
        baseUIForm.Hiding();
        _dicCurrentUIForms.Remove(strUIFormName);
    }

    /// <summary>
    /// 加载UI窗体到“当前显示窗体集合”中，且隐藏其他正在显示的页面
    /// </summary>
    /// <param name="strUIFormName"></param>
    private void EnterUIFormsToCacheHideOther(string strUIFormName)
    {
        BaseUIForm baseUIForm;
        BaseUIForm baseUIFormFromAllCache;

        //正在显示的UI窗体集合中有记录，直接返回
        _dicCurrentUIForms.TryGetValue(strUIFormName, out baseUIForm);
        if (baseUIForm != null) return;

        //正在显示UI窗体和栈缓存中的所有窗体隐藏
        foreach (BaseUIForm baseUIFormItem in _dicCurrentUIForms.Values)
        {
            baseUIFormItem.Hiding();
        }
        foreach(BaseUIForm baseUIFormItem in _stackCurrentUIForms)
        {
            baseUIFormItem.Hiding();
        }

        //把要显示的窗体加载到正在显示UI窗体集合中并显示
        _dicAllUIForms.TryGetValue(strUIFormName, out baseUIFormFromAllCache);
        if(baseUIFormFromAllCache != null)
        {
            _dicCurrentUIForms.Add(strUIFormName, baseUIFormFromAllCache);
            baseUIFormFromAllCache.Display();
        }
    }

    /// <summary>
    /// 从_dicCurrentUIForms中卸载当前的页面，并显示其他应显示的页面
    /// </summary>
    /// <param name="strFormName"></param>
    private void ExitUIFormFromCacheAndShowOther(string strFormName)
    {
        BaseUIForm baseUIForm;

        //_dicCurrentUIForms中没有要卸载的记录，直接返回
        _dicCurrentUIForms.TryGetValue(strFormName, out baseUIForm);
        if (baseUIForm == null) return;

        //指定的UI窗体隐藏，并从_disCurrentUIForms中移除
        baseUIForm.Hiding();
        _dicCurrentUIForms.Remove(strFormName);

        //显示_dicCurrentUIForms和_stackCuurentUIForms中的所有界面
        foreach (BaseUIForm baseUIFormItem in _dicCurrentUIForms.Values)
        {
            baseUIFormItem.ReDisplay();
        }
        foreach (BaseUIForm baseUIFormItem in _stackCurrentUIForms)
        {
            baseUIFormItem.ReDisplay();
        }
    }

    private void PushUIForms(string strUIFormName)
    {
        BaseUIForm baseUIForm;

        //冻结栈顶的界面
        if(_stackCurrentUIForms.Count > 0)
        {
            BaseUIForm topUIForms = _stackCurrentUIForms.Peek();
            topUIForms.Freeze();
        }

        //显示指定名称的界面
        _dicAllUIForms.TryGetValue(strUIFormName, out baseUIForm);
        if (baseUIForm != null)
        {
            baseUIForm.Display();
        }
        else
        {
            Debug.Log("PushUIForms出错， strUIFormName = " + strUIFormName);
        }
        //指定的UI界面入栈
        _stackCurrentUIForms.Push(baseUIForm);
    }

    /// <summary>
    /// UI界面出栈
    /// </summary>
    private void PopUIForms()
    {
        if (_stackCurrentUIForms.Count >= 2)
        {
            //栈顶出栈并隐藏
            BaseUIForm topUIForm = _stackCurrentUIForms.Pop();
            topUIForm.Hiding();
            //新栈顶显示
            BaseUIForm nextUIForm = _stackCurrentUIForms.Peek();
            nextUIForm.ReDisplay();
        }
        else if (_stackCurrentUIForms.Count == 1)
        {
            //栈顶出栈并隐藏
            BaseUIForm topUIForm = _stackCurrentUIForms.Pop();
            topUIForm.Hiding();
        }
    }

    /// <summary>
    /// 加载指定名字的“UI窗口”
    /// 1.根据“UI窗口”的名称，加载预制克隆
    /// 2.根据不同预制克隆体带的脚本中不同的“位置信息”，加载到“根窗体”下的不同节点
    /// 3.隐藏创建的UI克隆体
    /// 4.把克隆加入到“所有UI窗体”（缓存）集合中
    /// </summary>
    /// <param name="uriFormName"></param>
    /// <returns></returns>
    private BaseUIForm LoadUIForm(string uriFormName)
    {
        Debug.Log("LoadUIForm:" + uriFormName);
        string strUIFormPath = null;                                                        //UI窗体的路径
        GameObject goCloneUIPrefabs = null;                                       //创建的UI窗体克隆
        BaseUIForm baseUIForm = null;                                                  //窗体基类

        //根据UI窗体名称，得到对应的加载路径
        _dicFormsPaths.TryGetValue(uriFormName, out strUIFormPath);

        //根据“UI窗体”名称，加载预制克隆体
        if(!string.IsNullOrEmpty(strUIFormPath))
        {
            goCloneUIPrefabs = ResourcesMgr.GetInstance().LoadAsset(strUIFormPath, false);
        }
        else
        {
            Debug.LogError("JSON中未定义的窗口");
        }

        //根据克隆体中带的脚本中的不同信息，设计Ui克隆的父节点
        if(_traCanvasTransform != null && goCloneUIPrefabs !=null)
        {
            baseUIForm = goCloneUIPrefabs.GetComponent<BaseUIForm>();
            if (baseUIForm == null)
            {
                Debug.LogError("JSON定义的路径找不到窗口预制");
                return null;
            }
            switch (baseUIForm.CurrentUIType.UIForm_Type)
            {
                case UIFormType.Normal: //普通窗体
                    goCloneUIPrefabs.transform.SetParent(_traNormal, false);
                    break;
                case UIFormType.Fixed:  //固定窗体
                    goCloneUIPrefabs.transform.SetParent(_traFixed, false);
                    break;
                case UIFormType.Popup:  //弹出窗体
                    goCloneUIPrefabs.transform.SetParent(_traPopUp, false);
                    break;
                default:
                    break;
            }
            //隐藏克隆
            goCloneUIPrefabs.SetActive(false);
            //加入克隆到“所有UI窗体（缓存）”集合中
            _dicAllUIForms.Add(uriFormName, baseUIForm);
            return baseUIForm;
        }
        else
        {
            Debug.Log("_traCanvasTransform == null OR goCloneUIPrefab == null, uriFormName = " + uriFormName);
        }
        Debug.Log("Unkown, uriFormName = " + uriFormName);
        return null;
    }



}
