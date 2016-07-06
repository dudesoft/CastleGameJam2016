using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
    Ideas for enemy distribution:
        - Measure frequency of player in each color
        - Measure # of enemies of each color
        - Spawn in empty areas
        - Create temporal spawning points / areas

*/
public class EnemySpawner : MonoBehaviour {
    [SerializeField]
    private List<AbstractEnemy> enemyPrefabs;

    private Dictionary<System.Type, Pool> pools;
    [SerializeField]
    private int poolStartSize;

	void Awake () {
        // Initialize pools
        // Create one pool per enemy prefab
        pools = new Dictionary<System.Type, Pool>(enemyPrefabs.Count);
        Pool newPool;
        GameObject newPoolObj;
	    for(int i = 0; i < enemyPrefabs.Count; ++i) {
            // Create pool
            newPoolObj = new GameObject();
            newPoolObj.transform.parent = transform;
            newPool = newPoolObj.AddComponent<Pool>();
            newPool.Initialize(enemyPrefabs[i].gameObject, poolStartSize, 1);
            // Add to pool dictionary
            pools.Add(enemyPrefabs[i].GetType(), newPool);
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Space)) {
            Spawn(0, Vector3.zero);
        }
	}

    void Spawn(int prefabIndex, Vector3 position) {
        System.Type type = enemyPrefabs[prefabIndex].GetType();
        Spawn(type, position);
    }

    void Spawn(System.Type type, Vector3 position) {
        Pool p;
        if (pools.TryGetValue(type, out p)) {
            // Get instance
            GameObject newEnemyObj = p.Get();
            AbstractEnemy newEnemy = newEnemyObj.GetComponent<AbstractEnemy>();
            // Subscribe to death event
            newEnemy.Died += HandleDeadEnemy;
            // Set position and enable
            newEnemy.transform.position = position;
            newEnemyObj.SetActive(true);
        } else {
            Debug.LogWarning("There is no pool for " + type);
        }        
    }

    void HandleDeadEnemy(AbstractEnemy enemy) {
        // Unsubscribe from death event
        enemy.Died -= HandleDeadEnemy;
        // Find pool and release object
        System.Type type = enemy.GetType();
        Pool p;
        if (pools.TryGetValue(type, out p)) {
            p.Release(enemy.gameObject);
        } else {
            Debug.LogWarning("There is no pool for " + type);
        }
    }
}
