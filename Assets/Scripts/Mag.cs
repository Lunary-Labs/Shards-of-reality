using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mag : MonoBehaviour
{
    public Transform attack_point; // The origin point of the attack.
    public GameObject projectile_prefab; // The projectile to be launched.
    public float projectile_speed = 10.0f; // The speed at which the projectile is launched.
//public Animator animator; // The character's Animator.

    public float move_speed = 5.0f;

    void Update()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(Horizontal, Vertical, 0.0f) * move_speed * Time.deltaTime;

        transform.Translate(move);
    }
}
