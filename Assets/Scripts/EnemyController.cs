using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject projectilePrefab;
    public Transform selfPos;
    public float projectileSpeed = 5.0f;
    public float shootDelay = 2f;
    private float currentTime = 0.0f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update(){
        if(currentHealth <= 0){
            Destroy(gameObject);
        }

        currentTime += Time.deltaTime;

        if (currentTime >= shootDelay)
            {
                Shoot();

                currentTime = 0.0f;
            }
    }

    public int getCurrentHealth(){
        return currentHealth;
    }

    public void TakeDamage(int damage){
        this.currentHealth -= damage;
        Debug.Log(currentHealth);
    }

    public void Shoot(){
        Vector3 projectileStartPosition = selfPos.TransformPoint(new Vector3(-1, 0, 0));
        GameObject projectileInstance = Instantiate(projectilePrefab, projectileStartPosition, Quaternion.identity);
        Vector3 leftDirection = new Vector3(-1.0f, 0.0f, 0.0f);
        Rigidbody2D rb = projectileInstance.GetComponent<Rigidbody2D>();
        rb.velocity = leftDirection.normalized * projectileSpeed;
    }
    
}
