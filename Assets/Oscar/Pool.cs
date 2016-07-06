using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool : MonoBehaviour {
    private GameObject prefab;
    private float resizeCheckPeriod;
    private int referenceSize;
    private Stack<GameObject> pool;

    /// <summary>
    /// What percentage of the pool's reference size needs to be left before the pool grows 
    /// </summary>
    private float growPercentage = 0.25f;
    
    public void Initialize(GameObject prefab, int referenceSize, float resizeCheckPeriod) {
        this.prefab = prefab;
        this.resizeCheckPeriod = resizeCheckPeriod;
        this.referenceSize = referenceSize;
        pool = new Stack<GameObject>(referenceSize);
        GameObject newObj;
        for(int i = 0; i < referenceSize; ++i) {
            newObj = Instantiate(prefab);
            Release(newObj);
        }
        StartCoroutine(ResizePool());
    }

    public GameObject Get() {
        GameObject newObj;
        if(pool.Count > 0) {
            newObj = pool.Pop();
        } else {
            newObj = Instantiate(prefab);
        }
        return newObj;
    }

    public void Release(GameObject obj) {
        obj.transform.parent = transform;
        obj.SetActive(false);
        pool.Push(obj);
    }

    private IEnumerator ResizePool() {
        while(true) {
            if((float)pool.Count / (float)referenceSize <= growPercentage) {
                // Grow
                GameObject newObj;
                for (int i = 0; i < referenceSize; ++i) {
                    newObj = Instantiate(prefab);
                    Release(newObj);
                }
                Debug.Log("Pool being resized");
            }
            yield return new WaitForSeconds(resizeCheckPeriod);
        }
    }
}
