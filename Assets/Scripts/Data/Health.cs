using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Health : MonoBehaviour
{
    [SerializeField] public float health;
    Animator anim;
    private bool isDead;

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
                anim.SetTrigger("Dead");
                isDead = true;  // prevent repeating the trigger
                
                Destroy(gameObject, 1.5f); // delay for animation
            }
            else if (CompareTag("Player"))
            {
                
                Debug.Log("Player Died");
                
            }

        }

        }

}
