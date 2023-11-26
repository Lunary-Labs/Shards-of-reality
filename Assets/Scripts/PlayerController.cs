using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //references
    private Animator anim;
    private Rigidbody rb;
    private Transform orientation;
    public Camera mainCamera;
    public GameObject projectilePrefab; 
    private int groundLayer;
    
    //Dash
    private float dashForce = 80f;
    private float dashUpwardForce;
    private float dashDuration = 2f;
    private bool isDashing = false;

    //Clock 
    private float dashCd = 1.5f;
    private float dashCdTimer;

    private float MoveSpeed = 7;
    private float angleOffset = 45f;

    //Shoot
    private float projectileSpeed = 10f;
    private float projectileHeight = 1f;


    void Start() {
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        orientation = transform;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update() {
        //movement
        float horInput = Input.GetAxisRaw("Horizontal");
        float verInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = Quaternion.Euler(0, angleOffset, 0) * new Vector3(horInput, 0, verInput);
        inputDirection.Normalize(); 
        if (inputDirection != Vector3.zero) {
            transform.forward = inputDirection;
        }
        rb.velocity = inputDirection * MoveSpeed;

        //Handle animation bool 
        if(rb.velocity == new Vector3(0, 0, 0)) {
            anim.SetBool("IsRunning", false);
        }
        else {
            anim.SetBool("IsRunning", true);
        }

        //dash handle
        if (Input.GetButtonDown("Jump") && !isDashing) {
            Debug.Log("jump");
            dash();
        }

        if (Input.GetMouseButtonDown(1)) {
            mouseShoot();
            Debug.Log("shooting");
        }
    }

    //Dash
    private void dash() {
        Vector3 forceToApply = orientation.forward * dashForce;
        rb.AddForce(forceToApply, ForceMode.Impulse);
        isDashing = true;
        Invoke(nameof(resetDash), dashDuration);
    }

    private void resetDash() {
        isDashing = false;
    }

    private Vector3 GetMousePosition() {    
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer)) {
            return hit.point;
        }

        return Vector3.zero;
    }

    private void mouseShoot() {
        Vector3 mousePosition = GetMousePosition();
        Vector3 direction = mousePosition - transform.position;

        Vector3 instantiatePosition = transform.position + Vector3.up * projectileHeight;
        GameObject projectile = Instantiate(projectilePrefab, instantiatePosition, Quaternion.identity);
        projectile.transform.forward = direction.normalized;
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        projectileRb.AddForce(direction.normalized * projectileSpeed, ForceMode.Impulse);

        Destroy(projectile, 5f);
    }
}


