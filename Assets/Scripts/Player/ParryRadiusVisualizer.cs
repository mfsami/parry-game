using JetBrains.Annotations;
using UnityEngine;

public class ParryRadiusVisualizer : MonoBehaviour
{
    // half of colliders radius for some reason
    public float radius = 0.74f;
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
