using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

public static class Pooler
{
    private static Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

    public static (GameObject, bool) PoolSpawn(GameObject go, Vector3 pos, Quaternion rot, GameObject newParent = null)
    {
        GameObject obj;
        string key = go.name.Replace(" (Clone)", "");

        if (pools.ContainsKey(key))
        {
            if (pools[key].inactive.Count == 0)
            {
                if (newParent == null)
                {
                    newParent = pools[key].parent;
                }

                obj = Object.Instantiate(go, pos, rot, newParent.transform);
                return (obj, true);
            }
            else
            {
                obj = pools[key].inactive.Pop();
                obj.transform.position = pos;
                obj.transform.rotation = rot;
                if (newParent != null)
                {
                    obj.transform.SetParent(newParent.transform);
                }
                obj.SetActive(true);
                return (obj, false);
            }
        }
        else
        {
            if (newParent == null)
            {
                newParent = new GameObject($"{key}_POOL");
            }

            Pool newPool = new Pool(newParent);
            pools.Add(key, newPool);
            obj = Object.Instantiate(go, pos, rot, newParent.transform);
            return (obj, true);
        }
    }

    public static void PoolDespawn(GameObject go)
    {
        string key = go.name.Replace("(Clone)", "").Trim();
        if (pools.ContainsKey(key))
        {
            pools[key].inactive.Push(go);
            go.transform.position = pools[key].parent.transform.position;
            go.SetActive(false);
        }
        else
        {
            GameObject newParent = new GameObject($"{key}_POOL");
            Pool newPool = new Pool(newParent);
            pools.Add(key, newPool);
            go.transform.SetParent(newParent.transform);
            pools[key].inactive.Push(go);
            go.SetActive(false);
        }
    }

    public static void ClearAllPools()
    {
        pools.Clear();
    }

    public static void DestroyAllInactiveObjects()
    {
        foreach (var pool in pools.Values)
        {
            while (pool.inactive.Count > 0)
            {
                GameObject go = pool.inactive.Pop();
                Object.Destroy(go);
            }

            // Leak here, parent doesn't get destroyed if new is created, I just don't wanna fix it


            //if (pool.inactive.Count == pool.parent.gameObject.transform.childCount && pool.parent != null)
            //Object.Destroy(pool.parent);
        }

        //pools.Clear();
    }

    public static List<GameObject> GetAllInactiveObjects()
    {
        List<GameObject> inactiveObjects = new List<GameObject>();
        foreach (var pool in pools.Values)
        {
            foreach (var go in pool.inactive)
            {
                inactiveObjects.Add(go);
            }
        }
        return inactiveObjects;
    }
}