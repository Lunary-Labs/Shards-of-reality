using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour{
    public int damage = 10; // The damage dealt by the bullet.
    
    private void OnTriggerEnter2D(Collider2D other){
        // Check if the bullet has hit an enemy (or any other object you want to affect).
        if (other.CompareTag("Enemy")){
            // Get a script or component for handling the enemy's health.
            EnemyController enemyController = other.GetComponent<EnemyController>();

            if (enemyController != null){
                // Inflict damage to the enemy using the enemy's health management script.
                enemyController.TakeDamage(damage);

                // Destroy the bullet when it hits an enemy (or deactivate it if you prefer to reuse it).
                Destroy(gameObject);
            }
        }

        if (other.CompareTag("Player")){
            // Get a script or component for handling the enemy's health.
            Mag player = other.GetComponent<Mag>();

            if (player != null){
                if (player.canTakeDamage()){
                    player.TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
        }
    }
}
