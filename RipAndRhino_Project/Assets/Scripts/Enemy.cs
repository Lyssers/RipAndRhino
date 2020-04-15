using RhinoGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Unit
{
    [HideInInspector]
    public delegate void EnemyKilled();

    /// <summary>
    /// Called when an enemy is killed.
    /// </summary>
    [HideInInspector]
    public static event EnemyKilled OnEnemyKilled;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.LookAt(other.transform);
            Shoot();
        }
    }

    public override void UnitDied()
    {
        base.UnitDied();
        OnEnemyKilled?.Invoke();
    }
}
