using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    // For now destroy when hitting player
    // Set to continuous in inspector to prevent tunneling

    // Jus use tags for now, switch later, get it working

    // ------- References


    // ------- Variables
    public float dmgDealt;



    private void OnTriggerEnter2D(Collider2D other)
    {

        // Notice get component on gameObject. Not collision

        // Check if this collider belongs to something with health
        Health hp = other.gameObject.GetComponent<Health>();

        // Player hit
        if (hp != null )
        {
            hp.health -= dmgDealt;
            Debug.Log($"Hit {other.name}, HP: {hp.health}");
            Destroy(gameObject);
            return; // force stop so it doesn't check parry

        }

        // Parry window hit
        if (other.CompareTag("Parry"))
        {
            // Player exists on Player object in parent not this child
            Player player = other.gameObject.GetComponentInParent<Player>();
            Debug.Log("In Parry Window");
            if (player != null && player.isParrying)
            {
                Debug.Log("PARRIED");
                // DEAL WITH DEFLECTION / DESTRUCION
                // Destroy for now
                Destroy(gameObject);

            }
        }

    }
    // Destroys if outside cam view
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


}
