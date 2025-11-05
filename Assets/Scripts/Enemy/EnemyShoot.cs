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

    //private int shotsRemaining = 0; // keep track of shots left in burst
    //private float burstDelay = 0.3f;

    private void Start()
    {
        // first wait before firing
        coolDownRemaining = Random.Range(2f, 5f);
    }

    private void Awake()
    {
        // Safety: auto-find player if not assigned in Inspector
        if (PlayerPos == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) PlayerPos = player.transform;
        }
    }




    public void Shoot()
    {
        Rigidbody2D shotBullet = Instantiate(bullet, FirePoint.position, FirePoint.rotation);
        shotBullet.gameObject.layer = LayerMask.NameToLayer("EnemyBullet");

        var b = shotBullet.GetComponent<Bullet>();
        if (!b) { Destroy(shotBullet.gameObject); return; }

        b.owner = transform.root;          // make THIS enemy the owner
        b.SetAllegianceEnemy();            // tell the bullet it's from an enemy

        b.player = PlayerPos;

        Vector2 direction = (PlayerPos.position - FirePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        shotBullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        shotBullet.linearVelocity = direction * b.bulletSpeed;

    }
}
