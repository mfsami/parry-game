using System.Collections;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    // How enemy shoot works:
    // Instantiate a bullet at firepoints transform.position
    // Towards player

    // Deal with bullet speed + damage in bullet.cs

    // ------- References
    public Transform FirePoint;
    public Transform PlayerPos;
    public GameObject bulletPrefab;

    // ------- Variables
    public Rigidbody2D bullet;
    private float coolDownRemaining = 0f;

    private int shotsRemaining = 0; // keep track of shots left in burst
    private float burstDelay = 0.3f;

    private void Start()
    {
        // first wait before firing
        coolDownRemaining = Random.Range(2f, 5f);
    }



    private void Update()
    {
        coolDownRemaining -= Time.deltaTime;

        if (coolDownRemaining < 0f)
        {
            
            // this will fire until no shots left
            if (shotsRemaining > 0)
            {
                
                Shoot();
                shotsRemaining--;
                coolDownRemaining = burstDelay;
            }

            else
            {
                // start new burst
                shotsRemaining = Random.Range(1, 2); // burst size 
                Shoot();
                shotsRemaining--;

                // if more shots remain, use burst delay, else use a full cooldown
                if (shotsRemaining > 0)
                    coolDownRemaining = burstDelay;
                else
                    coolDownRemaining = Random.Range(2f, 5f);
            }

            
        }
    }


    void Shoot()
    {
        // Instantiate at fire point
        Rigidbody2D shotBullet = Instantiate(bullet, FirePoint.position, FirePoint.rotation);

        // Get bullet component
        Bullet bulletScript = shotBullet.GetComponent<Bullet>();

        // Set the owner and destroy if owner dead
        if (bulletScript) bulletScript.owner = this.transform;
        else Destroy(shotBullet.gameObject);

        bulletScript.player = PlayerPos;
        //Debug.Log($"{this.name} owns bullet {shotBullet.name}");

        // Calculate players direction vector
        // B - A = "how do I get from A to B?"
        Vector2 direction = (PlayerPos.position - FirePoint.position).normalized;

        // Bullet rotation
        // Imagine your direction vector as an arrow from the enemy’s FirePoint to the player.
        // So essentially we want to know "what is the angle of this arrow relative to the x-axis"
        // Mathf.Atan2(direction.y, direction.x) gives the angle (in radians) between the positive x-axis and your vector (x, y).

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Spin object that many degrees around Z and set it to our bullets rotation

        // We use vector3.forward in 2d here because we are always rotating ON the Z axis
        // Imagine a pinned piece of paper on a wall. The pin is the z axis, we can rotate this paper now with this pin on this axis
        shotBullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Set velocity and launch to that direction
        shotBullet.linearVelocity = direction * bulletScript.bulletSpeed;

    }
}
