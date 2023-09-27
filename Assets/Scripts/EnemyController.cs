using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Stats
    public int maxHealth = 100; // Maximum health of the enemy.
    private int currentHealth; // Current health of the enemy.
    public float projectileSpeed = 5.0f; // Speed of the enemy's projectiles.
    public float shootDelay = 2f; // Delay between enemy shots.
    public float speed; // Movement speed of the enemy.
    private float distance; // Distance between the enemy and the player.
    public bool activateShooting; // Should the enemy shoot?
    public float range = 3f; // Range at which the enemy starts chasing the player.
    
    private float currentTimeShooting = 0f; // Timer for shooting delay.
    
    public Transform selfPos; // Reference to the enemy's transform.
    public GameObject projectilePrefab; // Prefab for enemy projectiles.
    public GameObject player; // Reference to the player GameObject.

    // Dash
    private Vector2 dashDirection; // Direction of the dash.
    public float dashCastTime = 2f; // Duration of the dash casting phase.
    public float dashDuration = 0.5f; // Duration of the dash itself.
    public float dashCooldown = 1f; // Cooldown between dashes.
    public float currentTimeDashing = 0f; // Timer for dash cooldown.
    public Rigidbody2D rb; // Reference to the enemy's Rigidbody2D component.
    public bool isDashing = false; // Is the enemy currently dashing?
    public float dashSpeed = 17f; // Speed of the dash.
    public bool dashCast = false; // Is a dash currently being cast?

    private void Start(){
        rb = gameObject.GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    private void Update(){
        // Death handling
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        // Update shooting and dashing cooldown timers
        currentTimeShooting += Time.deltaTime;
        currentTimeDashing += Time.deltaTime;

        // Shooting delay
        if (currentTimeShooting >= shootDelay){
            if (activateShooting){
                Shoot(); // Call the Shoot function.
                currentTimeShooting = 0.0f;
            }
        }

        // Dashing delay
        // Chasing AI 
        if (distance <= range){
            dashDirection = (player.transform.position - transform.position).normalized;
            if (currentTimeDashing >= dashCooldown){
                Debug.Log(dashDirection);
                currentTimeDashing = 0f;
                StartCoroutine(Dash()); // Start the dash coroutine.
            }
        }
    }

    private void FixedUpdate(){
        if (!isDashing && !dashCast){
            // Calculate the distance and direction to the player for chasing.
            distance = Vector2.Distance(transform.position, player.transform.position);
            Vector2 direction = player.transform.position - transform.position;
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    public int GetCurrentHealth(){
        return currentHealth;
    }

    public void TakeDamage(int damage){
        this.currentHealth -= damage;
        Debug.Log(currentHealth);
    }

    public void Shoot(){
        // Create a projectile and set its velocity.
        Vector3 projectileStartPosition = selfPos.TransformPoint(new Vector3(-1, 0, 0));
        GameObject projectileInstance = Instantiate(projectilePrefab, projectileStartPosition, Quaternion.identity);
        Vector3 leftDirection = new Vector3(-1.0f, 0.0f, 0.0f);
        Rigidbody2D rb = projectileInstance.GetComponent<Rigidbody2D>();
        rb.velocity = leftDirection.normalized * projectileSpeed;
    }

    private IEnumerator Dash(){     dashCast = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashCastTime);

        // Allow movement in both X and Y directions during the dash.
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        isDashing = true;
        rb.velocity = dashDirection * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        dashCast = false;

        // Freeze the position after the dash.
        rb.constraints |= RigidbodyConstraints2D.FreezePositionX;
        rb.constraints |= RigidbodyConstraints2D.FreezePositionY;
        rb.velocity = Vector2.zero;
        isDashing = false;
        Debug.Log("Dash3");
    }

    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Player")){
            // Inflict damage to the player when colliding with the enemy.
            Mag currentPlayer = collision.gameObject.GetComponent<Mag>();
            currentPlayer.TakeDamage(10);
        }
    }
}