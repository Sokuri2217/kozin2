using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMScript : MonoBehaviour
{
    public GameManager gameManager;
    public MainUIScript mainUI;

    public AudioSource bgm;

    public AudioClip main;
    public bool isMain;
    public AudioClip battle;
    public bool isBattle;
    public AudioClip clear;
    public bool isClear;
    public AudioClip over;
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
        //Œˆ’…‚ª‚Â‚­‚Ü‚Å‚Í“®‚­
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

        if (gameManager.gameClear)
        {
            bgm.clip = clear;
            if (!isClear) 
            {
                bgm.Play();
                isClear = true;
            }
        }
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
