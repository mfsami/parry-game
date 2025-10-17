using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Deal with oncollisionenter and check radius parry collision and deal with destruction... defelction here?
    // For now destroy when hitting player


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
    // Destroys if outside cam view
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
