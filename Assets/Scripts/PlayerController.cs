using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Animator anim;
    private Rigidbody rb;
    private float MoveSpeed = 5;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {

        //movement
        float horInput = Input.GetAxisRaw("Horizontal");
        float verInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(horInput, 0, verInput).normalized;
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
    }
}
