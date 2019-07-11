public class SysDefine
{
    /// <summary>
    /// 根UI的TAG
    /// </summary>
    public const string CANVAS_TAG = "Canvas";
    /// <summary>
    /// UI摄像机的TAG
    /// </summary>
    public const string UICAMERA_TAG = "UICamera";
    /// <summary>
    /// 挂载Script Manager的节点名
    /// </summary>
    public const string UISCRIPTS_NODE = "ScriptsNode";

    /// <summary>
    /// 普通窗口的节点名
    /// </summary>
    public const string NORMAL_UI_NODE = "Normal";
    /// <summary>
    /// 固定位置窗口的节点名
    /// </summary>
    public const string FIXED_UI_NODE = "Fixed";
    /// <summary>
    /// 弹出窗口的节点名
    /// </summary>
    public const string POP_UI_NODE = "PopUp";
    /// <summary>
    /// 模态窗口的遮罩名
    /// </summary>
    public const string UI_MASKPANEL_NAME = "UIMaskPanel";
    /// <summary>
    /// 保存UI Prefab和路径的键值对的JSON文件的路径
    /// </summary>
    public const string SYS_PATH_UIFORMS_CONFIG_INFO = "UIFormsConfigInfo";

    public const float SYS_UIMASK_LUCENCY_COLOR_RGB = 255 / 255F;
    public const float SYS_UIMASK_LUCENCY_COLOR_RGB_A = 0F / 255F;

    public const float SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB = 220 / 255F;
    public const float SYS_UIMASK_TRANS_LUCENCY_COLOR_RGB_A = 50F / 255F;

    public const float SYS_UIMASK_IMPENETRABLE_COLOR_RGB = 50 / 255F;
    public const float SYS_UIMASK_IMPENETRABLE_COLOR_RGB_A = 200F / 255F;
}