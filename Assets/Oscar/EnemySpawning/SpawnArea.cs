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
    public ObjectColor EnemyColor;

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
                    StartCoroutine(spawner.PointSpawner(transform.position, EnemyAmount, Period, EnemyPrefabIndex, EnemyColor));
                    break;
                case SpawnPattern.Line:
                    StartCoroutine(spawner.LineSpawner(transform.position, transform.position + LineDirection, EnemyAmount, Period, EnemyPrefabIndex, EnemyColor));
                    break;
                case SpawnPattern.Circle:
                    StartCoroutine(spawner.CircleSpawner(transform.position, Radius, EnemyAmount, Period, EnemyPrefabIndex, EnemyColor));
                    break;
                case SpawnPattern.Unit:
                    StartCoroutine(spawner.UnitSpawner(transform.position, ForwardDir, Rows, UnitSpacing, Period, EnemyPrefabIndex, EnemyColor));
                    break;
            }
            used = true;
        }
    }

    void OnDrawGizmos() {
        //Gizmos.DrawIcon(transform.position, "SpawnAreaIcon.png");
        switch(EnemyColor) {
            case ObjectColor.Blue:
                Gizmos.color = Color.blue;
                break;
            case ObjectColor.Green:
                Gizmos.color = Color.green;
                break;
            case ObjectColor.Red:
                Gizmos.color = Color.red;
                break;
            case ObjectColor.Yellow:
                Gizmos.color = Color.yellow;
                break;
        }
        switch (Pattern) {
            case SpawnPattern.Point:
                //Gizmos.DrawIcon(transform.position, "EnemyIcon.png");
                //Gizmos.DrawWireSphere(transform.position, 0.2f);
                float angle = 0;
                for (int i = 0; i < 8; ++i) {
                    Gizmos.DrawLine(transform.position, transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0));
                    angle += 45;
                }                
                break;
            case SpawnPattern.Line:
                Gizmos.DrawLine(transform.position, transform.position + LineDirection);
                break;
            case SpawnPattern.Circle:
                Gizmos.DrawWireSphere(transform.position, Radius);
                break;
            case SpawnPattern.Unit:
                Vector3 nDir = ForwardDir.normalized;
                Vector3 orthoDir = new Vector3(nDir.y, -nDir.x, 0);
                int rowSize = -1;
                float sideOffset, frontOffset;
                for (int r = 0; r < Rows; ++r) {
                    rowSize += 2;
                    frontOffset = r * UnitSpacing;
                    sideOffset = Mathf.Max(Mathf.Floor(rowSize / 2.0f) * UnitSpacing, 0.1f);
                    Gizmos.DrawLine(transform.position - orthoDir * sideOffset - nDir * frontOffset, transform.position + orthoDir * sideOffset - nDir * frontOffset);
                }
                break;
        }
    }

}
