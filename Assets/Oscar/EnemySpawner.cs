using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
    Ideas for enemy distribution:
        - Measure frequency of player in each color
        - Measure # of enemies of each color
        - Spawn in empty areas
        - Create temporal spawning points / areas

    TODO:
        - Make spawn patterns able to spawn several types of enemies and different colors of enemies

*/
public class EnemySpawner : MonoBehaviour {
    [SerializeField]
    private List<FreBaseEnemy> enemyPrefabs;

    private Dictionary<System.Type, Pool> pools;
    [SerializeField]
    private int poolStartSize;

    // Testing
    public enum SpawnPattern {
        Point,
        Line,
        Circle,
        Unit
    }
    public SpawnPattern Pattern;

    public Rect SpawningArea;

    public int minSpawnerEnemies;
    public int maxSpawnerEnemies;
    public float minSpawnerPeriod;
    public float maxSpawnerPeriod;

    public float minSpawnerRadius;
    public float maxSpawnerRadius;

    public int minSpawnerRows;
    public int maxSpawnerRows;

    public float unitSpacing;

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
        //Testing
	    if(Input.GetKeyDown(KeyCode.Space)) {
            switch (Pattern) {
                case SpawnPattern.Point:
                    Vector3 point = new Vector3(Random.Range(SpawningArea.min.x, SpawningArea.max.x), Random.Range(SpawningArea.min.y, SpawningArea.max.y), 0);
                    StartCoroutine(PointSpawner(point, Random.Range(minSpawnerEnemies, maxSpawnerEnemies), Random.Range(minSpawnerPeriod, maxSpawnerPeriod), 0));
                    break;
                case SpawnPattern.Line:
                    Vector3 start = new Vector3(Random.Range(SpawningArea.min.x, SpawningArea.max.x), Random.Range(SpawningArea.min.y, SpawningArea.max.y), 0);
                    Vector3 end = new Vector3(Random.Range(SpawningArea.min.x, SpawningArea.max.x), Random.Range(SpawningArea.min.y, SpawningArea.max.y), 0);
                    StartCoroutine(LineSpawner(start, end, Random.Range(minSpawnerEnemies, maxSpawnerEnemies), Random.Range(minSpawnerPeriod, maxSpawnerPeriod), 0));
                    break;
                case SpawnPattern.Circle:
                    Vector3 center = new Vector3(Random.Range(SpawningArea.min.x, SpawningArea.max.x), Random.Range(SpawningArea.min.y, SpawningArea.max.y), 0);
                    StartCoroutine(CircleSpawner(center, Random.Range(minSpawnerRadius, maxSpawnerRadius), Random.Range(minSpawnerEnemies, maxSpawnerEnemies), Random.Range(minSpawnerPeriod, maxSpawnerPeriod), 0));
                    break;
                case SpawnPattern.Unit:
                    Vector3 tip = new Vector3(Random.Range(SpawningArea.min.x, SpawningArea.max.x), Random.Range(SpawningArea.min.y, SpawningArea.max.y), 0);
                    StartCoroutine(UnitSpawner(tip, Random.insideUnitCircle, Random.Range(minSpawnerRows, maxSpawnerRows), unitSpacing, Random.Range(minSpawnerPeriod, maxSpawnerPeriod), 0));
                    break;
            }
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
            FreBaseEnemy newEnemy = newEnemyObj.GetComponent<FreBaseEnemy>();
            // Subscribe to death event
            newEnemy.Died += HandleDeadEnemy;
            // Set position and enable
            newEnemy.transform.position = position;
            newEnemyObj.SetActive(true);
        } else {
            Debug.LogWarning("There is no pool for " + type);
        }        
    }

    void HandleDeadEnemy(FreBaseEnemy enemy) {
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

    /// <summary>
    /// Spawns amount enemies at point with a period delay between them.
    /// </summary>
    IEnumerator PointSpawner(Vector3 point, int amount, float period, int prefabIndex) {
        for(int i = 0; i < amount; ++i) {
            // Spawn
            Spawn(prefabIndex, point);
            if (period > 0) {
                yield return new WaitForSeconds(period);
            }     
        }
    }
    /// <summary>
    /// Spawns amount enemies along the line defined by start and end with a period delay between them.
    /// </summary>
    IEnumerator LineSpawner(Vector3 start, Vector3 end, int amount, float period, int prefabIndex) {
        for (int i = 0; i < amount; ++i) {
            // Spawn
            Spawn(prefabIndex, Vector3.Lerp(start, end, (float)i / (float)amount));
            if (period > 0) {
                yield return new WaitForSeconds(period);
            }
        }
        yield break;
    }
    /// <summary>
    /// Spawns amount anemies along the circle defined by center and radius with a period delay between them.
    /// </summary>
    IEnumerator CircleSpawner(Vector3 center, float radius, int amount, float period, int prefabIndex) {
        float frac;
        float twoPi = 2 * Mathf.PI;
        for (int i = 0; i < amount; ++i) {
            // Spawn
            frac = ((float)i / (float)amount) * twoPi;
            Spawn(prefabIndex, center + new Vector3(Mathf.Cos(frac), Mathf.Sin(frac), 0) * radius);
            if (period > 0) {
                yield return new WaitForSeconds(period);
            }
        }
        yield break;
    }
    /// <summary>
    /// Spawns enemies in a triangle unit formation with a period delay between them.
    /// </summary>
    IEnumerator UnitSpawner(Vector3 tip, Vector3 dir, int rows, float spacing, float rowPeriod, int prefabIndex) {
        Vector3 orthoDir = new Vector3(dir.y, -dir.x, 0);
        int rowSize = -1;
        float sideOffset, frontOffset;
        for (int r = 0; r < rows; ++r) {
            rowSize += 2;
            frontOffset = r * spacing;
            sideOffset = - Mathf.Floor(rowSize / 2.0f) * spacing;
            for (int n = 0; n < rowSize; ++n) {
                // Spawn                
                Spawn(prefabIndex, tip + sideOffset * orthoDir + frontOffset * dir);
                sideOffset += spacing;
            }
            if (rowPeriod > 0) {
                yield return new WaitForSeconds(rowPeriod);
            }
        }
    }
}
