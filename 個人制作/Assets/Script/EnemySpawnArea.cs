using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnArea : MonoBehaviour
{
    //“G¶¬
    public GameObject enemy;
    public int spawnTime;
    public int spawnLimit;
    public int enemyLimit;

    //ƒXƒNƒŠƒvƒgŽæ“¾
    public MainUIScript mainUI;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.spawn) 
        {
            spawnTime++;

            if (spawnTime >= spawnLimit && mainUI.chaseEnemyNum <= enemyLimit)
            {
                //“G¶¬
                Instantiate(enemy, transform.position, Quaternion.identity);
                spawnTime = 0;
            }
        }
    }
}
