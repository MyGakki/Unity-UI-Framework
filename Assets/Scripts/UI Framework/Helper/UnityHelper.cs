using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityHelper : MonoBehaviour
{

    /// <summary>
    /// 使用深度优先查找子对象
    /// </summary>
    /// <param name="goParent">父对象的GameObject</param>
    /// <param name="childName">子对象的名字</param>
    /// <returns></returns>
    public static Transform FindTheChildNode(GameObject goParent, string childName)
    {
        Transform searchTrans = null;
        searchTrans = goParent.transform.Find(childName);
        if (searchTrans == null)
        {
            foreach (Transform trans in goParent.transform)
            {
                searchTrans = FindTheChildNode(trans.gameObject, childName);
                if (searchTrans != null)
                {
                    return searchTrans;
                }
            }
        }
        return searchTrans;
    }
    /// <summary>
    /// 获得子对象上的脚本
    /// </summary>
    /// <typeparam name="T">脚本的泛型</typeparam>
    /// <param name="goParent">父对象</param>
    /// <param name="childName">子对象名</param>
    /// <returns></returns>
    public static T GetTheChildNodeComponentScript<T> (GameObject goParent, string childName) where T:Component
    {
        Transform searchTranformNode = null;

        searchTranformNode = FindTheChildNode(goParent, childName);
        if (searchTranformNode != null)
        {
            return searchTranformNode.gameObject.GetComponent<T>();
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 给子节点添加脚本
    /// </summary>
    /// <typeparam name="T">脚本的泛型</typeparam>
    /// <param name="goParent">父节点</param>
    /// <param name="childName">子节点名</param>
    /// <returns></returns>
    public static T AddChildNodeComponent<T>(GameObject goParent, string childName) where T : Component
    {
        Transform searchTransform = null;

        searchTransform = FindTheChildNode(goParent, childName);
        if (searchTransform != null)
        {
            //删除已有的同名脚本
            T[] componentScriptsArray = searchTransform.GetComponents<T>();
            for (int i = 0; i < componentScriptsArray.Length; i++)
            {
                if (componentScriptsArray[i] != null)
                {
                    Destroy(componentScriptsArray[i]);
                }
            }
            return searchTransform.gameObject.AddComponent<T>();
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 把child设置为parent的子节点
    /// </summary>
    /// <param name="parents">方法结束后，变为父节点</param>
    /// <param name="child">方法结束后，变为子节点</param>
    public static void AddChildNodeToParentNode(Transform parent, Transform child)
    {
        child.SetParent(parent, false);
        child.localPosition = Vector3.zero;
        child.localEulerAngles = Vector3.zero;
        child.localScale = Vector3.one;
    }
}
