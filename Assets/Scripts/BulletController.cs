using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    public int damage = 10; 
    public DebugEnemyController enemyController;
    private void OnTriggerEnter(Collider other) {
    if (other.CompareTag("Damageable")) {
            DebugEnemyController enemyController = other.GetComponent<DebugEnemyController>();
            if (enemyController != null) {
                enemyController.takeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
