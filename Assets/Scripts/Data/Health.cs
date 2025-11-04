using UnityEngine;
using UnityEngine.InputSystem.Processors;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] public float health;
    Animator anim;
    public bool isDead { get; private set; }
    public event Action OnDied;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDead) return;

        if (health <= 0)
        {
            if (CompareTag("Enemy"))
            {
                //anim.SetTrigger("Dead");
                isDead = true;  // prevent repeating the trigger
                
                Destroy(gameObject, 1.5f); // delay for animation
            }
            else if (CompareTag("Player"))
            {
                anim.SetTrigger("PlayerDead");
                isDead = true;

                // broadcast
                OnDied?.Invoke();
                //Debug.Log("Player Died");

            }

        }

        }

}
