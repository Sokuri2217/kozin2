using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMScript : MonoBehaviour
{
    public AudioSource bgm; //BGM関連

    public AudioClip mainBGM;   //メイン
    public AudioClip battleBGM; //戦闘中
    public AudioClip clearBGM;  //ステージクリア
    public AudioClip overBGM;   //ステージオーバー

    //スクリプト取得
    public PlayerController playerController;
    public EnemyBear enemyBear;
    public GolemController golemController;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        bgm.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameClear && !gameManager.gameOver)
        {
            if (enemyBear.isChase)
            {
                bgm.clip = battleBGM;
            }
            else
            {
                bgm.clip = mainBGM;
            }
        }
        else
        {
            if(gameManager.gameClear)
            {
                bgm.clip = clearBGM;
            }
            else if(gameManager.gameOver)
            {
                bgm.clip = overBGM;
            }
        }
    }
}
