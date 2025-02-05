using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMScript : MonoBehaviour
{
    //スクリプト取得
    public GameManager gameManager;
    public MainUIScript mainUI;

    //コンポーネント取得
    public AudioSource bgm;

    //各BGM
    public AudioClip main;
    public AudioClip battle;
    public AudioClip clear;
    public AudioClip over;

    //再生確認フラグ
    public bool isMain;
    public bool isBattle;
    public bool isClear;
    public bool isOver;

    // Start is called before the first frame update
    void Start()
    {
        bgm.clip = main;
        isMain = true;
        isBattle = false;
        isClear = false;
        isOver = false;
        bgm.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //決着がつくまでは動く
        if (!gameManager.gameClear && !gameManager.gameOver) 
        {
            if (mainUI.chaseEnemyNum > 0)
            {
                bgm.clip = battle;

                if (!isBattle)
                {
                    bgm.Play();
                    isBattle = true;
                    isMain = false;
                }
            }
            else
            {
                bgm.clip = main;

                if (!isMain)
                {
                    bgm.Play();
                    isMain = true;
                    isBattle = false;
                }
            }
        }

        //クリアBGM
        if (gameManager.gameClear)
        {
            bgm.clip = clear;
            if (!isClear) 
            {
                bgm.Play();
                isClear = true;
            }
        }
        //オーバーBGM
        else if (gameManager.gameOver)
        {
            bgm.clip = over;

            if (!isOver) 
            {
                bgm.Play();
                isOver = true;
            }
        }
    }
}
