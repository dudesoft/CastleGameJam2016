using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoPool : MonoBehaviour 
{
    public List<Pool> pools;
    static AutoPool instance;
	// Use this for initialization
	void Awake() {
        pools = new List<Pool>();
        instance = this;
	}
	
	public static Pool GetPool(GameObject prefab, int poolSize)
    {
        foreach (Pool p in instance.pools)
        {
            if (p.prefab == prefab)
            {
                return p;
            }
        }
        Pool newPool = instance.gameObject.AddComponent<Pool>();
        newPool.Initialize(prefab, poolSize, 10);
        instance.pools.Add(newPool);
        return newPool;
    }
}
