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

    public void SetAllegianceEnemy() => allegiance = Allegiance.Enemy;
    public void SetAllegiancePlayer() => allegiance = Allegiance.Player;


    // ------- Variables
    public float dmgDealt;
    public float bulletSpeed = 10f;
    public float deflectedBulletSpeed = 20f;
    private bool hasHit = false;
    private Rigidbody2D rb;


    public enum Allegiance { Enemy, Player }
    [SerializeField] private Allegiance allegiance = Allegiance.Enemy;

    private void OnEnable()
    {
        hasHit = false;
        
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        entityVFX = GetComponent<EntityVFX>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (hasHit) return;
        // Parry window hit
        if (other.CompareTag("Parry"))
        {

            // Player exists on Player object in parent not this child
            var playerComp = other.gameObject.GetComponentInParent<Player>();

            if (playerComp && playerComp.isParrying)
            {
                // MAYDAY MAYDAY WE'VE BEEN HIT ALPHA 1 A-1 WE'VE BEEN HIT
                playerComp.ConsumeDurability(1);

                // flip owner
                allegiance = Allegiance.Player;

                SetLayerRecursively(gameObject, LayerMask.NameToLayer("PlayerBullet"));

                //Debug.Log($"[PARRY] reflected bullet from owner={owner?.name ?? "null"} at {Time.time:0.000}");


                // RE-AIM: back to the shooter if we know it; else just bounce back
                if (owner != null)
                {
                    Vector2 dir = ((Vector2)owner.position - (Vector2)transform.position).normalized;
                    rb.linearVelocity = dir * deflectedBulletSpeed;
                }
                else
                {
                    rb.linearVelocity = -rb.linearVelocity; // simple reflect fallback
                }

                // small nudge so we don't keep overlapping the parry collider
                transform.position += (Vector3)(rb.linearVelocity.normalized * 0.05f);

                // Turn off parry window after deflect
                playerComp.isParrying = false;
                return;
            }
        }


        // Notice get component on gameObject. Not collision

        // Check if this collider belongs to something with health
        var hp = other.GetComponentInParent<Health>();
        if (!hp) return;

        // Filter: enemy bullets damage Player; player bullets damage Enemy
        // use the ROOT's tag, not the child collider's tag
        bool hitIsPlayerRoot = hp.gameObject.CompareTag("Player");
        bool hitIsEnemyRoot = hp.gameObject.CompareTag("Enemy");

        bool valid =
            (allegiance == Allegiance.Enemy && hitIsPlayerRoot) ||
            (allegiance == Allegiance.Player && hitIsEnemyRoot);

        if (!valid) return;

        hasHit = true;

        //Debug.Log($"[HIT] alleg={allegiance} targetRoot={hp.name} owner={owner?.name ?? "null"} at {Time.time:0.000}");


        hp.ApplyDamage(dmgDealt, gameObject, $"bullet-{allegiance}");

        var vfx = hp.GetComponent<EntityVFX>();
        if (vfx) vfx.PlayHitEffect();

        Destroy(gameObject);
    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform t in obj.transform) SetLayerRecursively(t.gameObject, layer);
    }


    // Destroys if outside cam view
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }


}
