using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    GameObject[] enemiesInScene;
    // Always append all enemies in the scene into this list
    // Randomly choose en enemy from the list, call shoot
    // Timer starts, at 0, call another enemy shoot

    public float shootTimer = 3;
    private float currentTime;


    private void Start()
    {
        enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        currentTime = shootTimer;
    }


    private void Update()
    {
        //Debug.Log(enemiesInScene.Length);
        ChooseEnemy();
    }

    void ChooseEnemy()
    {

        currentTime -= Time.deltaTime;

        if (enemiesInScene == null) return;

        if (currentTime <= 0)
        {
            int randomIndex = Random.Range(0, enemiesInScene.Length);
            GameObject chosenEnemy = enemiesInScene[randomIndex];
            Debug.Log(chosenEnemy.name);
            chosenEnemy.GetComponentInChildren<EnemyShoot>().Shoot();

            
            currentTime = shootTimer;
        }

        
    }

    
}
