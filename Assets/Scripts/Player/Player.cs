using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // ------- References
    public GameObject bullet;
    public Transform owner; // owner of fired bullet
    public Transform player;
    Bullet bulletScript;
    public GameObject parryVisual;


    // ------- Variables
    public float bulletSpeed = 15f;

    public bool isParrying;

    // Timers
    private float parryWindowTimer = 0.1f; // 250 ms window
    private float parryCooldown = 0.1f; // 800 ms cooldown after failed attempt

    void Update()
    {
        ProcessInputs();
    }


    void ProcessInputs()
    {
        // Deal with timers later

        // GetKey returns true for single frame. Coroutine starts at that frame
        // Deal with parry animation in here later


        if (!isParrying)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(ParryWindow());
                
            }
            
        }

        

        

    }

    IEnumerator ParryWindow()
    {
        // Open parry window.... then timer
        isParrying = true;
        parryVisual.SetActive(true);

        // Wait for seconds
        yield return new WaitForSeconds(parryWindowTimer);

        // Window closes after timer
        //Debug.Log("Parry window closed");

        // Cool down to prevent spam
        yield return new WaitForSeconds(parryCooldown);
        parryVisual.SetActive(false);

        // Can parry again after cooldown
        isParrying = false;


    }

    public void Deflect(Transform owner)
    {
        // Instantiate new bullet at player to enemy
        var shotBullet = Instantiate(bullet, player.position, player.rotation);
        var rb = shotBullet.GetComponent<Rigidbody2D>();

        // Calculate enemy direction vector
        // B - A = "how do I get from A to B?"
        Vector2 direction = (owner.position - player.position).normalized;

        // Bullet rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        shotBullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Set velocity and launch to that direction
        rb.linearVelocity = direction * bulletSpeed;

        // Change new bullets layer
        shotBullet.gameObject.layer = LayerMask.NameToLayer("DeflectedBullet");
        Debug.Log($"{rb.name} is now on layer {LayerMask.LayerToName(rb.gameObject.layer)}");


    }
}
