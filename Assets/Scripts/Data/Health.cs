using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] public float health;
    Animator anim;
    public bool isDead { get; private set; }
    public event Action OnDied;

    void Awake() => anim = GetComponent<Animator>();

    // ---- PUBLIC DAMAGE ENTRY POINT ----
    public void ApplyDamage(float amount)
    {
        if (isDead) return;

        health = Mathf.Max(0f, health - amount);

        if (health <= 0f)
            HandleDeath();
    }

    // ---- CENTRALIZED DEATH BEHAVIOR ----
    private void HandleDeath()
    {
        if (isDead) return;
        isDead = true;

        
        if (CompareTag("Enemy"))
        {
            // anim?.SetTrigger("Dead");  // if you add it later
            Destroy(gameObject);   // allow death anim/VFX add delay later
        }
        else if (CompareTag("Player"))
        {
            anim?.SetTrigger("PlayerDead");
            OnDied?.Invoke();
        }
    }
}
