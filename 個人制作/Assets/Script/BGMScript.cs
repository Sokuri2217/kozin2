using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMScript : MonoBehaviour
{
    //�X�N���v�g�擾
    public GameManager gameManager;
    public MainUIScript mainUI;

    //�R���|�[�l���g�擾
    public AudioSource bgm;

    //�eBGM
    public AudioClip main;
    public AudioClip battle;
    public AudioClip clear;
    public AudioClip over;

    //�Đ��m�F�t���O
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
        //���������܂ł͓���
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

        //�N���ABGM
        if (gameManager.gameClear)
        {
            bgm.clip = clear;
            if (!isClear) 
            {
                bgm.Play();
                isClear = true;
            }
        }
        //�I�[�o�[BGM
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
