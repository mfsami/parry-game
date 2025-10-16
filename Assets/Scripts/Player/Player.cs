using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // References



    // Variables

    private bool isParrying;
    private float parryWindowTimer = 2f;

    void Update()
    {
        ProcessInputs();
    }

    // Physics updates should be done in fixed update
    //void FixedUpdate()
    //{
    //    Move();
    //}

    void ProcessInputs()
    {
        // Deal with timers later

        // GetKey returns true for single frame. Coroutine starts at that frame
        // Deal with parry animation in here later
        if (Input.GetKeyDown(KeyCode.Space)){
            StartCoroutine(ParryWindow());
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
        Debug.Log("Parry window closed");
        
    }

    
}
