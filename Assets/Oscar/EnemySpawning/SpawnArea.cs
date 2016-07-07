using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpawnArea: MonoBehaviour {
    public enum SpawnPattern {
        Point,
        Line,
        Circle,
        Unit
    }
    public SpawnPattern Pattern;

    public int EnemyPrefabIndex;

    public Vector3 Position;

    // Line
    public Vector3 LineStart;
    public Vector3 LineEnd;

    public int EnemyAmount;
    public float Period;

    // Circle
    public float Radius;

    // Unit
    public int Rows;
    public Vector3 ForwardDir;
    public float UnitSpacing;

    private bool used = false;
    private EnemySpawner spawner;

    void Start() {
        spawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
    }

    void OnTriggerEnter2D(Collision2D col) {
        if(!used && col.collider.tag == "Player") {
            switch (Pattern) {
                case SpawnPattern.Point:
                    StartCoroutine(spawner.PointSpawner(Position, EnemyAmount, Period, EnemyPrefabIndex));
                    break;
                case SpawnPattern.Line:
                    StartCoroutine(spawner.LineSpawner(LineStart, LineEnd, EnemyAmount, Period, EnemyPrefabIndex));
                    break;
                case SpawnPattern.Circle:
                    StartCoroutine(spawner.CircleSpawner(Position, Radius, EnemyAmount, Period, EnemyPrefabIndex));
                    break;
                case SpawnPattern.Unit:
                    StartCoroutine(spawner.UnitSpawner(Position, ForwardDir, Rows, UnitSpacing, Period, EnemyPrefabIndex));
                    break;
            }
            used = true;
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "SpawnAreaIcon.png");
    }

}
