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
    public float newBulletSpeed = 15f;
    public bool isParrying;
    public float swordDurMax = 5f;
    public float swordDur = 5f;

    // ------- Timers
    private float parryWindowTimer = 0.1f; // 250 ms window
    private float parryCooldown = 0.1f; // 800 ms cooldown after failed attempt

    // ------- Animations
    [SerializeField] Animator anim;
    const int BaseLayer = 0;
    bool NextAttackTracker = true;

    private void Awake()
    {
        if (!anim)
        {
            anim = GetComponent<Animator>();
        }
    }

    void Update()
    {
        ProcessInputs();
    }


    void ProcessInputs()
    {
        // Update sharpening state
        anim.SetBool("IsSharpening", swordDur <= 0);

        if (!isParrying && Input.GetKeyDown(KeyCode.Space))
        {
            // Each parry costs 1 durability
            swordDur--;

            StartCoroutine(ParryWindow());
            anim.SetBool("NextAttackTracker", NextAttackTracker);
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Attack");

            NextAttackTracker = !NextAttackTracker;
        }

        // restore key for testing
        if (Input.GetKeyDown(KeyCode.R))
        {
            swordDur = swordDurMax;
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
        rb.linearVelocity = direction * newBulletSpeed;

        // Change new bullets layer
        shotBullet.gameObject.layer = LayerMask.NameToLayer("DeflectedBullet");
        Debug.Log($"{rb.name} is now on layer {LayerMask.LayerToName(rb.gameObject.layer)}");


    }
}
