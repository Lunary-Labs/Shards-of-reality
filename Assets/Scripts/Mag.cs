using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mag : MonoBehaviour{
    private Vector2 movement; // Stores player movement input.
    [SerializeField]
    private float moveSpeed = 3.0f; // Movement speed of the player.
    private Rigidbody2D rb; // Reference to the player's Rigidbody2D component.

    [SerializeField]
    private int maxHealth = 100; // Maximum health of the player.
    [SerializeField]
    private int currentHealth; // Current health of the player.
    [SerializeField]
    private float dashSpeed = 13f; // Speed at which the player dashes.
    [SerializeField]
    private float dashDuration = 0.3f; // Duration of the dash ability.
    [SerializeField]
    private float dashCooldown = 3f;// Cooldown time between dashes.
    [SerializeField] 
    private bool isDashing = false; // Indicates if the player is currently dashing.

    private float currentDashcooldown = 0f; // Clock for dash cooldown.
    private bool canDash = true; 
    private Vector2 dashDirection; // Direction of the dash.
    

    [SerializeField] 
    private float currentInvulnerabilityTime = 2f; // Current duration of invulnerability after taking damage.
    private float invulnerabilityTime = 2f; // Total duration of invulnerability after taking damage.
    private bool vulnerable = true; // Indicates if the player is vulnerable to damage.

    

    void Start() {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component of the player.
    }

    void Update() {
        if (isDashing) {
            return; // If the player is dashing, exit the Update function.
        }

        //INPUT
        // Movement input
        movement.x = Input.GetAxis("Horizontal"); // Get horizontal input (left/right).
        movement.y = Input.GetAxis("Vertical"); // Get vertical input (up/down).

        // Dash input
        if (Input.GetKeyDown(KeyCode.Space)) {
            if(canDash){
                dashDirection = new Vector2(movement.x, movement.y).normalized; // Calculate the dash direction.
                StartCoroutine(Dash()); // Initiate the dash.
            }
        }

        // Update invulnerability timer
        currentInvulnerabilityTime += Time.deltaTime;
        currentDashcooldown += Time.deltaTime;


        //CLOCKS
        // Handle invulnerability duration
        if (invulnerabilityTime > currentInvulnerabilityTime) {
            vulnerable = false; // Player is temporarily invulnerable.
        }
        else{
            vulnerable = true; // Player is vulnerable to damage.
        }

        if (dashCooldown <= currentDashcooldown) {
            canDash = true;
        }

        // Toggle collision with enemy when dashing
        if (isDashing) {
            currentDashcooldown = 0f; 
            canDash = false;
            invulnerabilityTime = dashDuration;
            currentInvulnerabilityTime = 0f;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true); // Disable collisions with enemies.
        }
        else{
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false); // Enable collisions with enemies.
        }
    }

    private void FixedUpdate() {
        if (!isDashing) {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime); // Move the player based on input.
        }
    }

    private IEnumerator Dash() {
        isDashing = true;
        rb.velocity = dashDirection * dashSpeed; // Apply dash velocity.
        yield return new WaitForSeconds(dashDuration); // Wait for the dash duration.
        rb.velocity = Vector2.zero; // Stop the character after dashing.
        isDashing = false;
    }

    public bool CanTakeDamage() {
        if (isDashing) {
            return false; // Player cannot take damage while dashing.
        }
        else{
            return true; // Player can take damage when not dashing.
        }
    }

    public void TakeDamage(int damage) {
        if (vulnerable) {
            currentHealth -= damage; // Reduce player's health.
            invulnerabilityTime = 2f; // Reset invulnerability timer.
            currentInvulnerabilityTime = 0f;
        }
    }

    public int GetCurrentHealth() {
        return currentHealth; // Get the current health of the player.
    }
}
