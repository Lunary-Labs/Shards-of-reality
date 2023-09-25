using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update(){
        if(currentHealth <= 0){
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage){
        this.currentHealth -= damage;
        Debug.Log(currentHealth);
    }
    
}
