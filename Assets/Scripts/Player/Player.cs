using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // ------- References



    // ------- Variables

    public bool isParrying;

    // Timers
    private float parryWindowTimer = 1f;
    private float parryCooldown = 2f;

    void Update()
    {
        ProcessInputs();
    }


    void ProcessInputs()
    {
        // Deal with timers later

        // GetKey returns true for single frame. Coroutine starts at that frame
        // Deal with parry animation in here later

        if (!isParrying)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(ParryWindow());
            }
        }

    }

    IEnumerator ParryWindow()
    {
        // Open parry window.... then timer
        isParrying = true;
        Debug.Log("Parry window open");

        // Wait for seconds
        yield return new WaitForSeconds(parryWindowTimer);

        // Window closes after timer
        //Debug.Log("Parry window closed");

        // Cool down to prevent spam
        yield return new WaitForSeconds(parryCooldown);

        // Can parry again after cooldown
        isParrying = false;


    }
}
