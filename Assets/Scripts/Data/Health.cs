using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public float health;

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
