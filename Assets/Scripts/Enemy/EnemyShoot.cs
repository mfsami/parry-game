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

        // Set velocity
        shotBullet.linearVelocity = transform.right * bulletSpeed;

    }
}
