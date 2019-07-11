using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMaskMgr : MonoBehaviour
{
    //UIMaskMgr单例
    private static UIMaskMgr _instance = null;
    //UI根节点对象
    private GameObject _goCanvasRoot = null;
    //UI脚本节点对象
    private Transform _traUIScriptNode = null;
    //顶层面板
    private GameObject _goTopPanel;
    //遮罩面板
    private GameObject _goMaskPanel;
    //UI相机
    private Camera _uiCamera;
    //UI相机的原始景深
    private float _originalUICameraDepth;

    public static UIMaskMgr GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameObject("_UIMskMgr").AddComponent<UIMaskMgr>();
        }
        return _instance;
    }

    private void Awake()
    {
        //获得UI的根节点对象、脚本节点对象
        _goCanvasRoot = GameObject.FindGameObjectWithTag(SysDefine.CANVAS_TAG);
        _traUIScriptNode = _goCanvasRoot.transform.Find(SysDefine.UISCRIPTS_NODE);
        //实例化该脚本并作为“脚本节点对象”的子物体
        UnityHelper.AddChildNodeToParentNode(_traUIScriptNode, gameObject.transform);
        //得到“顶层面板”，“遮罩面板”
        _goTopPanel = _goCanvasRoot;
        _goMaskPanel = UnityHelper.FindTheChildNode(_goCanvasRoot, SysDefine.UI_MASKPANEL_NAME).gameObject;
        //得到UI摄像机
        _uiCamera = GameObject.FindGameObjectWithTag(SysDefine.UICAMERA_TAG).GetComponent<Camera>();
        if (_uiCamera != null)
        {
            //得到UI摄像机的景深
            _originalUICameraDepth = _uiCamera.depth;
        }
        else
        {
            Debug.Log("UI_Camera is null");
        }

    }
    /// <summary>
    /// 设置遮罩状态
    /// </summary>
    /// <param name="goDisplayUIForms">需要显示的UI窗体</param>
    /// <param name="lucencyType">透明度属性</param>
    public void SetMaskWindow(GameObject goDisplayUIForms, UIFormLucencyType lucencyType = UIFormLucencyType.Luceny)
    {
        //顶层窗体下移
        _goTopPanel.transform.SetAsLastSibling();
        //启用遮罩并设置透明度
        switch(lucencyType)
        {
            //完全透明，不能穿透
            case UIFormLucencyType.Luceny:
                _goMaskPanel.SetActive(true);
                Color newColor1 = new Color(SysDefine.SYS_UIMASK_LUCENCY_COLOR_RGB, 
                    SysDefine.SYS_UIMASK_LUCENCY_COLOR_RGB, 
                    SysDefine.SYS_UIMASK_LUCENCY_COLOR_RGB, 
                    SysDefine.SYS_UIMASK_LUCENCY_COLOR_RGB_A);
                _goMaskPanel.GetComponent<Image>().color = newColor1;
                break;
            //半透明，不能穿透
            case UIFormLucencyType.TransLucence:
                _goMaskPanel.SetActive(true);
                Color newColor2 = new Color(SysDefine.SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB,
                    SysDefine.SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB,
                    SysDefine.SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB,
                    SysDefine.SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB_A);
                _goMaskPanel.GetComponent<Image>().color = newColor2;
                break;
            //低透明，不能穿透
            case UIFormLucencyType.ImPenetrable:
                _goMaskPanel.SetActive(true);
                Color newColor3 = new Color(SysDefine.SYS_UIMASK_IMPENETRABLE_COLOR_RGB,
                    SysDefine.SYS_UIMASK_IMPENETRABLE_COLOR_RGB,
                    SysDefine.SYS_UIMASK_IMPENETRABLE_COLOR_RGB,
                    SysDefine.SYS_UIMASK_IMPENETRABLE_COLOR_RGB_A);
                _goMaskPanel.GetComponent<Image>().color = newColor3;
                break;
           //可以穿透
            case UIFormLucencyType.Penetra:
                if (_goMaskPanel.activeInHierarchy)
                {
                    _goMaskPanel.SetActive(false);
                }
                break;
            default:
                break;
        }
        //遮罩窗体下移
        _goMaskPanel.transform.SetAsLastSibling();
        //显示窗体下移
        goDisplayUIForms.transform.SetAsLastSibling();
        //增加当前UI摄像机的景深，确保当前摄像机为最前显示
        if(_uiCamera != null)
        {
            _uiCamera.depth += 100;
        }
    }

    /// <summary>
    ///取消遮罩状态 
    /// </summary>
    public void CancelMaskWindow()
    {
        //顶层窗体上移
        _goTopPanel.transform.SetAsFirstSibling();
        //隐藏遮罩
        if (_goMaskPanel.activeInHierarchy)
        {
            _goMaskPanel.SetActive(false);
        }
        //恢复UI摄像机的景深
        if (_uiCamera != null)
        {
            _uiCamera.depth = _originalUICameraDepth;
        }
    }
}
