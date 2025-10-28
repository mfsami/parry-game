using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    // ------- References
    public GameObject bullet;
    public Transform owner; // owner of fired bullet
    public Transform player;
    public Bullet bulletScript;
    public GameObject parryVisual;
    public ComboUI comboUI;


    // ------- Variables
    public float newBulletSpeed = 15f;
    public bool isParrying;
    public float swordDurMax = 5f;
    public float swordDur = 5f;
    bool comboActive = false;
    


    // ------- Timers
    private float parryWindowTimer = 0.1f; // 250 ms window
    private float parryCooldown = 0.1f; // 800 ms cooldown after failed attempt

    // ------- Animations
    [SerializeField] Animator anim;
    const int BaseLayer = 0;
    bool NextAttackTracker = true;

    // Arrays
    public List<KeyCode> combo = new List<KeyCode>();
    List<KeyCode> comboInput = new List<KeyCode>();

    KeyCode[] possibleKeys = {
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow
    };

    private void Awake()
    {
        if (!anim)
        {
            anim = GetComponent<Animator>();
        }
    }

    void Update()
    {
        ProcessInputs();
        
    }


    void ProcessInputs()
    {
        anim.SetBool("IsSharpening", swordDur <= 0);

        if (!isParrying && Input.GetKeyDown(KeyCode.Space))
        {
            
            StartCoroutine(ParryWindow());

            anim.SetBool("InAttack", true);

            anim.SetBool("NextAttackTracker", NextAttackTracker);
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Attack");
            NextAttackTracker = !NextAttackTracker;

            StartCoroutine(ClearInAttackWhenDone());
        }

        // If durability is broken, generate a combo
        if (swordDur <= 0 && !comboActive)
        {
            comboActive = true;
            CreateDurCombo();
            Debug.Log("New combo created!");

        }

        // Register player inputs
        if (comboActive)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) RegisterCombo(KeyCode.UpArrow);
            if (Input.GetKeyDown(KeyCode.DownArrow)) RegisterCombo(KeyCode.DownArrow);
            if (Input.GetKeyDown(KeyCode.LeftArrow)) RegisterCombo(KeyCode.LeftArrow);
            if (Input.GetKeyDown(KeyCode.RightArrow)) RegisterCombo(KeyCode.RightArrow);
        }
    }

    void CreateDurCombo()
    {
        
        combo = new List<KeyCode>();

        // Randomize combo
        // 5 is combo length
        for (int i = 0; i < 4; i++)
        {
            // clarify random from unity engine not system
            KeyCode randomKey = possibleKeys[UnityEngine.Random.Range(0, possibleKeys.Length)];
            combo.Add(randomKey);
            
        }

        comboUI.ShowCombo(combo);

        Debug.Log(string.Join(", ", combo));
    }

    void RegisterCombo(KeyCode key)
    {
        comboInput.Add(key);

        // Check inputs
        for (int i = 0;  i < comboInput.Count; i++)
        {
            // One wrong input
            if (comboInput[i] != combo[i])
            {
                Debug.Log("WRONG INPUT, RESET");
                Debug.Log(string.Join(", ", comboInput));
                comboInput.Clear();
                return;
            }

            if (comboInput.Count == combo.Count)
            {
                Debug.Log("DURABILITY RESTORED");
                swordDur = swordDurMax;

                comboInput.Clear();
                comboActive = false;

                comboUI.HideCombo();

            }
        }
    }

    public void ConsumeDurability(int amount = 1)
    {
        // reduce durability on each hit
        swordDur = Mathf.Max(0, swordDur - amount);
        // Update sharpening state
        
        Debug.Log(swordDur);
    }

    IEnumerator ParryWindow()
    {
        // Open parry window.... then timer
        isParrying = true;
        parryVisual.SetActive(true);

        // Wait for seconds
        yield return new WaitForSeconds(parryWindowTimer);

        // Window closes after timer
        //Debug.Log("Parry window closed");

        // Cool down to prevent spam
        yield return new WaitForSeconds(parryCooldown);
        parryVisual.SetActive(false);

        // Can parry again after cooldown
        isParrying = false;


    }

    IEnumerator ClearInAttackWhenDone()
    {
        // Wait until the current state finishes
        yield return null; // let Animator enter the Attack state first
        var info = anim.GetCurrentAnimatorStateInfo(0);
        float len = info.length / Mathf.Max(0.0001f, info.speed); // seconds
        yield return new WaitForSeconds(len);
        anim.SetBool("InAttack", false);
    }

    public void Deflect(Transform owner)
    {
        // Instantiate new bullet at player to enemy
        var shotBullet = Instantiate(bullet, player.position, player.rotation);
        var rb = shotBullet.GetComponent<Rigidbody2D>();

        // Calculate enemy direction vector
        // B - A = "how do I get from A to B?"
        Vector2 direction = (owner.position - player.position).normalized;

        // Bullet rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        shotBullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Set velocity and launch to that direction
        rb.linearVelocity = direction * newBulletSpeed;

        // Change new bullets layer
        shotBullet.gameObject.layer = LayerMask.NameToLayer("DeflectedBullet");
        Debug.Log($"{rb.name} is now on layer {LayerMask.LayerToName(rb.gameObject.layer)}");


    }
}
