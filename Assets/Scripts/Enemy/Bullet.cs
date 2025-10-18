using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    // For now destroy when hitting player
    // Set to continuous in inspector to prevent tunneling

    // Jus use tags for now, switch later, get it working

    // ------- References
    public GameObject bullet;
    public Transform owner; // owner of fired bullet
    public Transform player;

    // ------- Variables
    public float dmgDealt;
    public float bulletSpeed = 5f;
    



    private void OnTriggerEnter2D(Collider2D other)
    {
        
        // Parry window hit
        if (other.CompareTag("Parry"))
        {
            // Player exists on Player object in parent not this child
            Player playerComp = other.gameObject.GetComponentInParent<Player>();
            
            if (playerComp != null && playerComp.isParrying)
            {
                Destroy(gameObject);
                playerComp.Deflect(owner);
                
                // Turn off parry window after deflect
                playerComp.isParrying = false;
                //Debug.Log("parry");
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
            Destroy(gameObject);
        }

    }


    // Destroys if outside cam view
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


}
