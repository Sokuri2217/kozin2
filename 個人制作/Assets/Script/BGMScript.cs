using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMScript : MonoBehaviour
{
    public AudioSource bgm; //BGM�֘A

    public AudioClip mainBGM;   //���C��
    public AudioClip battleBGM; //�퓬��
    public AudioClip clearBGM;  //�X�e�[�W�N���A
    public AudioClip overBGM;   //�X�e�[�W�I�[�o�[

    //�X�N���v�g�擾
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
