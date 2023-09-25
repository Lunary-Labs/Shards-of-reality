using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mag : MonoBehaviour{
    public float moveSpeed = 5.0f;             // Speed of character movement.
    public float dashDistance = 5f;            // Distance of the dash.
    public bool dashAvailable = true;          // Is dashing available?
    public bool isDashing = false;             // Is the character currently dashing?
    public float dashDuration = 0.1f;          // Duration of the dash.
    private float dashTimer = 0.0f;           // Timer for dash cooldown.

    void Update(){
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Dash detection
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && dashAvailable){
            StartDash(horizontal, vertical);
        }

        if (isDashing){
            PerformDash();
        }
        else{
            Move(horizontal, vertical);
        }

        // Timer for dash cooldown
        if (dashTimer > 0){
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0){
                dashAvailable = true;
            }
        }
    }

    private void StartDash(float horizontal, float vertical){
        // Save the character's current Z-axis value.
        float currentZ = transform.position.z;

        // Calculate the new position using only the X and Y axes.
        Vector3 newPosition = transform.position + new Vector3(horizontal * dashDistance, vertical * dashDistance, 0.0f);

        // Restore the Z-axis value to its previous state.
        newPosition.z = currentZ;

        // Apply the new position.
        transform.position = newPosition;

        isDashing = true;
        dashTimer = dashDuration;
    }

    private void PerformDash(){
        // The character continues moving during the dash in the specified direction.
        // This part of the code remains the same as before.
        transform.Translate(new Vector3(transform.forward.x, transform.forward.y, 0) * moveSpeed * Time.deltaTime);

        // Check if the distance traveled is greater than or equal to the dash distance.
        if (Vector3.Distance(transform.position, transform.position + transform.forward * dashDistance) >= dashDistance){
            isDashing = false;
        }
    }

    private void Move(float horizontal, float vertical){
        // Manage the character's normal movement by adjusting only the X and Y axes.
        Vector3 move = new Vector3(horizontal, vertical, 0.0f) * moveSpeed * Time.deltaTime;
        transform.Translate(move);
    }
}
