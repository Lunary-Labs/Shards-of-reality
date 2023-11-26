using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEnemyController : MonoBehaviour
{
    private int currentHealth = 100;

    public void takeDamage(int damage) {
        this.currentHealth -= damage;
        Debug.Log(currentHealth);
    }

    private void Update() {
        if(currentHealth <= 0) {
            Destroy(gameObject);
        }
    }
}
