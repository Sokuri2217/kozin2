using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMScript : MonoBehaviour
{
    public GameManager gameManager;

    public AudioSource bgm;

    public AudioClip main;
    public AudioClip clear;
    public bool isClear;
    public AudioClip over;
    public bool isOver;

    // Start is called before the first frame update
    void Start()
    {
        bgm.clip = main;
        isClear = false;
        isOver = false;
        bgm.Play();
    }

    // Update is called once per frame
    void Update()
    {
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
