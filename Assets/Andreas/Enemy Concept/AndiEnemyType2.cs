﻿using UnityEngine;
using System.Collections;

public class AndiEnemyType2 : FreBaseEnemy
{
    private Vector3 targetLocation;

    public float speed = 2;
    private GameObject player;
    private bool engaging = true;

	protected override void Init()
	{
        speed = speed * Random.Range(1, 1.5f);
        player = GameObject.FindGameObjectWithTag("Player");
	}	

    void Update()
    {
        Move();
    }

    void TurnToTarget()
    {
        if (engaging)
        {
            targetLocation = player.transform.position;
        }
        Vector3 vectorToTarget = targetLocation - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
    }

    private Vector3 GetNewTargetLocationOutSide()
    {
        return RotateAroundPoint(player.transform.position, transform.position, Quaternion.Euler(0, 0, Random.Range(-80, 80)));
    }

    private Vector3 RotateAroundPoint(Vector3 pivot, Vector3 point, Quaternion angle)
    {
        return (angle * (point - pivot) + pivot) + (pivot - point) * -Random.Range(3f, 5f);
    }

    public virtual void Move()
    {
        TurnToTarget();

        transform.position = transform.position + transform.right * speed * Time.deltaTime;

        if (engaging && Vector3.Distance(transform.position, targetLocation) < 3)
        {
            targetLocation = GetNewTargetLocationOutSide();
            engaging = !engaging;
            return;
        }

        if (Vector3.Distance(transform.position, targetLocation) < 1)
        {
            engaging = !engaging;
        }
    }
}