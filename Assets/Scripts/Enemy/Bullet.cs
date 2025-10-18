using UnityEngine;

public class Bullet : MonoBehaviour
{

    // For now destroy when hitting player
    // Set to continuous in inspector to prevent tunneling

    // Jus use tags for now, switch later, get it working

    // ------- References
    public Rigidbody2D bullet;
    public Transform owner; // owner of fired bullet
    public Transform player;

    // ------- Variables
    public float dmgDealt;
    public float bulletSpeed = 5f;



    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Trigger with: {other.name}, tag={other.tag}");

        // Parry window hit
        if (other.CompareTag("Parry"))
        {
            // Player exists on Player object in parent not this child
            Player player = other.gameObject.GetComponentInParent<Player>();
            //Debug.Log("In Parry Window");
            if (player != null && player.isParrying)
            {
                Destroy(gameObject);
                Deflect();
                Debug.Log("parry");
                return;
            }
        }


        // Notice get component on gameObject. Not collision

        // Check if this collider belongs to something with health
        Health hp = other.gameObject.GetComponent<Health>();



        // Player hit
        if (other.CompareTag("Player"))
        {
            hp.health -= dmgDealt;
            Debug.Log($"Hit {other.name}, HP: {hp.health}");
            Destroy(gameObject);
            

        }

        
        

    }

    public void Deflect()
    {
        // Instantiate new bullet at player to enemy
        Rigidbody2D shotBullet = Instantiate(bullet, player.position, player.rotation);

        Debug.Log($"Reflected spawned: {shotBullet != null}");

        // Calculate enemy direction vector
        // B - A = "how do I get from A to B?"
        Vector2 direction = (owner.position - player.position).normalized;

        // Bullet rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        shotBullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Set velocity and launch to that direction
        shotBullet.linearVelocity = direction * bulletSpeed;
        
    }

    // Destroys if outside cam view
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


}
