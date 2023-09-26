using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mag : MonoBehaviour{
    public float moveSpeed = 5.0f;             
    public float dashDistance = 10f;
    
    public Rigidbody2D rb;

    public int maxHealth = 100;
    public int currentHealth;
    public float dashSpeed = 20f;
    public float dashDuration = 0.25f;
    public float dashCooldown = 1f;
    public bool isDashing = false;

    private Vector2 dashDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = 100;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontal, vertical, 0.0f) * moveSpeed * Time.deltaTime;
        transform.Translate(move);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            dashDirection = new Vector2(horizontal, vertical).normalized;
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        rb.velocity = dashDirection * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = Vector2.zero; // Arrêtez le personnage après le dash.
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
    }

    public bool canTakeDamage(){
        if(this.isDashing){
            return false;
        }
        else{
            return true;
        }
    }

    public void TakeDamage(int damage){
        this.currentHealth -= damage;
    }

    public int getCurrentHealth(){
        return currentHealth;
    }
}
