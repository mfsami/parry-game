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

    // ------- Variables
    public float bulletSpeed;
    public Rigidbody2D bullet;
    private float coolDown = 2f;
    private float coolDownRemaining = 0f;


    void Update()
    {
        // Timer starts
        coolDownRemaining -= Time.deltaTime;

        if (coolDownRemaining < 0f)
        {
            Shoot();

            // Reset timer
            coolDownRemaining = coolDown;
        }
        // stop overcomplicating things. you dont always need coroutines
    }


    void Shoot()
    {
        // Instantiate at fire point
        Rigidbody2D shotBullet = Instantiate(bullet, FirePoint.position, FirePoint.rotation);

        // Calculate players direction vector
        // Subtracting gives vector dir from firepoint -> player
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
        shotBullet.linearVelocity = direction * bulletSpeed;

    }
}
