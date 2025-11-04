using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    // ------- References
    public GameObject bullet;
    public Transform owner; // owner of fired bullet
    public Transform player;
    EntityVFX entityVFX;

    public Player playerScript;

    // ------- Variables
    public float dmgDealt;
    public float bulletSpeed = 10f;


    private void Awake()
    {
        entityVFX = GetComponent<EntityVFX>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        
        // Parry window hit
        if (other.CompareTag("Parry"))
        {
            
            // Player exists on Player object in parent not this child
            Player playerComp = other.gameObject.GetComponentInParent<Player>();
            
            if (playerComp != null && playerComp.isParrying)
            {
                // MAYDAY MAYDAY WE'VE BEEN HIT ALPHA 1 A-1 WE'VE BEEN HIT
                playerComp.ConsumeDurability(1);

                Destroy(gameObject);

                if (owner) playerComp.Deflect(owner);
                else Destroy(gameObject);
                
                
                // Turn off parry window after deflect
                playerComp.isParrying = false;
                return;
            }
        }


        // Notice get component on gameObject. Not collision

        // Check if this collider belongs to something with health
        Health hp = other.gameObject.GetComponent<Health>();

        // Player hit
        if (hp != null)
        {
            hp.health -= dmgDealt;

            // Hit flash
            var vfx = other.GetComponent<EntityVFX>();
            if (vfx != null) vfx.PlayHitEffect();

            Destroy(gameObject);
        }

    }


    // Destroys if outside cam view
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


}
