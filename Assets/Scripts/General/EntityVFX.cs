using System.Collections;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{
    private Material originalMat;
    private SpriteRenderer sr;

    [SerializeField] private Material hitEffect;
    [SerializeField] private float hitEffectDur = 0.15f;

    private Coroutine hitEffectCT;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    public void PlayHitEffect()
    {
        if (hitEffectCT != null)
        {
            StopCoroutine(hitEffectCT);
        }
        hitEffectCT = StartCoroutine(HitEffectCT());
    }

    private IEnumerator HitEffectCT()
    {
        sr.material = hitEffect;
        yield return new WaitForSeconds(hitEffectDur);
        sr.material = originalMat;
    }
}
