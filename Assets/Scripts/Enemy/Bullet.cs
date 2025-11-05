using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // ======================= REFERENCES / FIELDS =======================
    
    public Transform owner;              // Shooter's root. Set by EnemyShoot on spawn; used to re-aim on parry.
 

    // Allegiance = who this bullet belongs to (Enemy when fired, Player after parry).
    public enum Allegiance { Enemy, Player }
    [SerializeField] private Allegiance allegiance = Allegiance.Enemy;
    public void SetAllegianceEnemy() => allegiance = Allegiance.Enemy;
    public void SetAllegiancePlayer() => allegiance = Allegiance.Player;

    // ======================= TUNABLES / RUNTIME =======================
    
    public float dmgDealt = 1f;                  // Damage dealt on hit.
    public float bulletSpeed = 10f;              // Default enemy bullet speed.
    public float deflectedBulletSpeed = 20f;     // Deflected speed.
    private bool hasHit = false;                 // One-hit guard so a bullet doesn't double-apply in same frame.
    private Rigidbody2D rb;                      // Cached RB2D

    private void OnEnable()
    {
        // Reset per-life state 
        hasHit = false;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Guard: already hit something this frame
        if (hasHit) return;

        // ---------- PARRY WINDOW ----------
        // Reflects if we hit the parry trigger *while* the player is parrying.
        if (other.CompareTag("Parry"))
        {
            
            var playerComp = other.GetComponentInParent<Player>();

            if (playerComp && playerComp.isParrying)
            {
                // MAYDAY SWORD DOWN SWORD DOWN SHES BEEN HIT
                playerComp.ConsumeDurability(1);

                // Allegience flipped to player, can damage enemies
                allegiance = Allegiance.Player;

                // flip physics layers
                SetLayerRecursively(gameObject, LayerMask.NameToLayer("PlayerBullet"));

                // Re-aim the reflected bullet back to its shooter if we know who fired it.
                if (owner != null)
                {
                    Vector2 dir = ((Vector2)owner.position - (Vector2)transform.position).normalized;
                    rb.linearVelocity = dir * deflectedBulletSpeed;
                }
                else
                {
                    // Fallback if no enemy
                    Destroy(gameObject);
                }

                // Small positional nudge so we exit the parry trigger this frame (avoids re-trigger spam).
                transform.position += (Vector3)(rb.linearVelocity.normalized * 0.05f);

                // Close the parry window for this attempt.
                playerComp.isParrying = false;
                return; 
            }
        }

        
        var hp = other.GetComponentInParent<Health>();
        if (!hp) return;


        // Allegiance determines which *side* we are allowed to damage:
        //  - Enemy bullet can only damage Player-tagged roots.
        //  - Player (parried) bullet can only damage Enemy-tagged roots.

        bool hitIsPlayerRoot = hp.gameObject.CompareTag("Player");
        bool hitIsEnemyRoot = hp.gameObject.CompareTag("Enemy");

        bool valid =
            (allegiance == Allegiance.Enemy && hitIsPlayerRoot) ||
            (allegiance == Allegiance.Player && hitIsEnemyRoot);

        if (!valid) return;

        hasHit = true;


        hp.ApplyDamage(dmgDealt);

        // Hit flash on damaged entity
        var vfx = hp.GetComponent<EntityVFX>();
        if (vfx) vfx.PlayHitEffect();

        // Bullet is done after a successful hit.
        Destroy(gameObject);
    }

 
    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform t in obj.transform)
            SetLayerRecursively(t.gameObject, layer);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
