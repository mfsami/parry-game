using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class BulletManager : MonoBehaviour
{

    List<GameObject> enemiesInScene = new List<GameObject>();
    // Randomly choose en enemy from the list, call shoot
    // Timer starts, at 0, call another enemy shoot

    public float shootTimer = 2;
    private float currentTime;


    private void Start()
    {
        enemiesInScene.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        currentTime = shootTimer;
    }


    private void Update()
    {
        //Debug.Log(enemiesInScene.Length);
        ChooseEnemy();
    }

    void ChooseEnemy()
    {

        enemiesInScene.RemoveAll(e => e == null);   // cleanup nulls

        if (enemiesInScene.Count == 0) return;

        currentTime -= Time.deltaTime;

        if (enemiesInScene == null) return;

        if (currentTime <= 0)
        {
            int randomIndex = Random.Range(0, enemiesInScene.Count);
            GameObject chosenEnemy = enemiesInScene[randomIndex];
            Debug.Log(chosenEnemy.name);
            chosenEnemy.GetComponentInChildren<EnemyShoot>().Shoot();

            
            currentTime = shootTimer;
        }

        
    }

    
}
