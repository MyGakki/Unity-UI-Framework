using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesMgr : MonoBehaviour
{
    private static ResourcesMgr _instance;
    //容器键值对集合
    private Hashtable ht = null;

    public static ResourcesMgr GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameObject("_ResourcesMgr").AddComponent<ResourcesMgr>();
        }
        return _instance;
    }

    private void Awake()
    {
        ht = new Hashtable();
    }

    /// <summary>
    /// 加载Resources（带缓存）
    /// </summary>
    /// <typeparam name="T">资源泛型</typeparam>
    /// <param name="path">资源路径</param>
    /// <param name="isCache">是否需要缓存</param>
    /// <returns>加载出来的资源</returns>
    public T LoadResource<T>(string path, bool isCache) where T : Object
    {
        if (ht.Contains(path))
        {
            return ht[path] as T;
        }

        T TResource = Resources.Load<T>(path);
        if (TResource == null)
        {
            Debug.Log("找不到资源， path = " + path);
        }
        else if (isCache)
        {
            ht.Add(path, TResource);
        }

        return TResource;
    }

    /// <summary>
    /// 加载资源并在场景里实例化
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="isCache">是否缓存</param>
    /// <returns>场景中的实例化的资源</returns>
    public GameObject LoadAsset(string path, bool isCache)
    {
        GameObject go = LoadResource<GameObject>(path, isCache);
        GameObject goClone = GameObject.Instantiate<GameObject>(go);
        if (goClone == null)
        {
            Debug.Log("资源克隆失败， path = " + path);
        }
        return goClone;
    }
}
