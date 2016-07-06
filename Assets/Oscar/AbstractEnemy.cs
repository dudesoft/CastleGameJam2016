using UnityEngine;
using System.Collections;

public abstract class AbstractEnemy : MonoBehaviour {
    public delegate void EnemyDiedEventHandler(AbstractEnemy enemy);
    public event EnemyDiedEventHandler Died;

    protected void Die() {
        Died(this);
    }
}
