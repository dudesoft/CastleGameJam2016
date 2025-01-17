﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    public enum WaveColor {
        Predefined,
        PlayerColor,
        NotPlayerColor,
        Random
    }
    public WaveColor WaveColorMode;
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

    // Wave finished area type
    private int enemiesLeft;
    private int precedingLeft;

    public bool WaitForPreceding;
    public List<SpawnArea> PrecedingAreas;

    public delegate void WaveClearedEventHandler();
    public event WaveClearedEventHandler WaveCleared;

    // Time based area type
    public float Delay;

    
    private BenShip player;

    void Start() {
        spawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        // Subscribe to preceding waves        
        if(WaitForPreceding) {
            precedingLeft = PrecedingAreas.Count;
            foreach(SpawnArea sa in PrecedingAreas) {
                sa.WaveCleared += UpdatePrecedingCount;
            }               
        }
        if(WaveColorMode == WaveColor.Random) {
            EnemyColor = (ObjectColor)Random.Range(0, System.Enum.GetNames(typeof(ObjectColor)).Length - 1);
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<BenShip>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(!WaitForPreceding) {
            if (!used && col.tag == "Player") {
                Invoke("TriggerSpawning", Delay);
                used = true;
            }
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


    void TriggerSpawning() {
        if(WaveColorMode == WaveColor.PlayerColor) {
            EnemyColor = player.objectColor;
        } else if(WaveColorMode == WaveColor.NotPlayerColor) {
            EnemyColor = (ObjectColor)(((int)player.objectColor + Random.Range(1, System.Enum.GetNames(typeof(ObjectColor)).Length - 1)) % System.Enum.GetNames(typeof(ObjectColor)).Length);
        }
        switch (Pattern) {
            case SpawnPattern.Point:
                StartCoroutine(spawner.PointSpawner(transform.position, EnemyAmount, Period, EnemyPrefabIndex, EnemyColor, WaveStateUpdater));
                enemiesLeft = EnemyAmount;
                break;
            case SpawnPattern.Line:
                StartCoroutine(spawner.LineSpawner(transform.position, transform.position + LineDirection, EnemyAmount, Period, EnemyPrefabIndex, EnemyColor, WaveStateUpdater));
                enemiesLeft = EnemyAmount;
                break;
            case SpawnPattern.Circle:
                StartCoroutine(spawner.CircleSpawner(transform.position, Radius, EnemyAmount, Period, EnemyPrefabIndex, EnemyColor, WaveStateUpdater));
                enemiesLeft = EnemyAmount;
                break;
            case SpawnPattern.Unit:
                StartCoroutine(spawner.UnitSpawner(transform.position, ForwardDir, Rows, UnitSpacing, Period, EnemyPrefabIndex, EnemyColor, WaveStateUpdater));
                enemiesLeft = 0;
                int rowSize = - 1;
                for (int r = 0; r < Rows; ++r) {
                    rowSize += 2;
                    enemiesLeft += rowSize;
                }
                break;
        }
    }

    void UpdatePrecedingCount() {
        --precedingLeft;
        if(precedingLeft == 0) {
            Invoke("TriggerSpawning", Delay);
        }
    }
    void WaveStateUpdater(FreBaseEnemy deadEnemy) {
        // Update number of enemies left
        --enemiesLeft;
        // Unsubscribe
        deadEnemy.Died -= WaveStateUpdater;
        if (enemiesLeft == 0) {
            if(WaveCleared != null) {
                WaveCleared();
                Debug.Log("Wave cleared");
            }
        }
    }
}
