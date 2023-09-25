using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_controller : MonoBehaviour
{
    public Transform character;
    public Transform weapon; // The weapon attached to the character.
    public GameObject projectilePrefab; // The projectile to be fired.
    public float projectileSpeed = 5.0f;
    public float offset = 0.1f; // Adjust offset values as needed.

    private void Update()
    {
        // Get the mouse position in screen pixels.
        Vector3 mousePosition = Input.mousePosition;

        Vector3 offsetVector = new Vector3(offset, offset, 0); // Adjust offset values as needed.

        // Convert the mouse position to a point in the game world.
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, character.position.z - Camera.main.transform.position.z)) + offsetVector;

        // Calculate the direction between the weapon's position and the mouse position.
        Vector3 direction = worldMousePosition - weapon.position; // Use the weapon's position here.

        // Calculate the angle in degrees between the weapon and the mouse position.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the weapon around the character based on the angle.
        weapon.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (Input.GetMouseButtonDown(0)) // Left mouse button click.
        {
            FireProjectile(direction.normalized);
        }
    }

    private void FireProjectile(Vector3 direction)
    {
        // Calculate the starting position of the projectiles from the end of the weapon.
        Vector3 projectileStartPosition = weapon.TransformPoint(new Vector3(1, 0, 0)); // Adjust the offset based on the weapon's length.

        // Create an instance of the projectile and position it at the starting position.
        GameObject projectileInstance = Instantiate(projectilePrefab, projectileStartPosition, Quaternion.identity);

        // Get the Rigidbody2D of the projectile and set its velocity to move in the firing direction.
        Vector3 shootDirection = weapon.TransformDirection(Vector3.right); // Shooting direction based on the weapon's rotation.
        Rigidbody2D rb = projectileInstance.GetComponent<Rigidbody2D>();
        rb.velocity = shootDirection.normalized * projectileSpeed;
    }
}
