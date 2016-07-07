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
    [Header("General")]
    public SpawnPattern Pattern;

    public int EnemyPrefabIndex;
    public ObjectColor Color;

    [Tooltip("Not used by Unit pattern.")]
    public int EnemyAmount;
    [Tooltip("Unit pattern uses this on a row basis.")]
    public float Period;

    // Line
    [Header("Line")]
    public Vector3 LineDirection;



    // Circle
    [Header("Circle")]
    public float Radius;

    // Unit
    [Header("Unit")]
    public int Rows;
    public Vector3 ForwardDir;
    public float UnitSpacing;




    private bool used = false;
    private EnemySpawner spawner;

    void Start() {
        spawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(!used && col.tag == "Player") {
            switch (Pattern) {
                case SpawnPattern.Point:
                    StartCoroutine(spawner.PointSpawner(transform.position, EnemyAmount, Period, EnemyPrefabIndex, Color));
                    break;
                case SpawnPattern.Line:
                    StartCoroutine(spawner.LineSpawner(transform.position, transform.position + LineDirection, EnemyAmount, Period, EnemyPrefabIndex, Color));
                    break;
                case SpawnPattern.Circle:
                    StartCoroutine(spawner.CircleSpawner(transform.position, Radius, EnemyAmount, Period, EnemyPrefabIndex, Color));
                    break;
                case SpawnPattern.Unit:
                    StartCoroutine(spawner.UnitSpawner(transform.position, ForwardDir, Rows, UnitSpacing, Period, EnemyPrefabIndex, Color));
                    break;
            }
            used = true;
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "SpawnAreaIcon.png");
    }

}
