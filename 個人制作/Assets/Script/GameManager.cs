using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gamePlay;
    public bool gameOver;
    public bool gameClear;

    public GameObject clearPanel;
    public GameObject overPanel;
    public GameObject spawnGoal;
    public GameObject[] goal;
    private int goalNum = 0;
    private bool spawn = false;

    public AudioSource main_Bgm;

    private AudioSource result_Bgm;
    public AudioClip clear_Bgm;
    public AudioClip over_Bgm;

    public bool isBgm;

    //�Q�[�����
    public bool open_Option;
    //�������h�~
    public bool input;

    // Start is called before the first frame update
    void Start()
    {
        gamePlay = true;
        gameOver = false;
        gameClear = false;
        clearPanel.SetActive(false);
        overPanel.SetActive(false);
        spawnGoal.SetActive(false);
        main_Bgm = GetComponent<AudioSource>();
        result_Bgm = GetComponent<AudioSource>();
        isBgm = false;
        open_Option = false;
        input = false;
        //�J�[�\����\��
        //Cursor.visible = false;

        for (int i = 0; i < 5; i++)
            goal[i].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController playerController = GetComponent<PlayerController>();

        //���������Ɠ����Ȃ��悤�ɂ���
        if (!gameClear && !gameOver)
        {
            //Esc�ňꎞ��~������
            if (Input.GetKeyDown(KeyCode.Escape) && !input)
            {
                switch (open_Option)
                {
                    case false:
                        //Cursor.visible = true;//�J�[�\���\��
                        open_Option = true;
                        gamePlay = false;
                        Time.timeScale = 0;
                        break;
                    case true:
                        //Cursor.visible = false;//�J�[�\����\��
                        Time.timeScale = 1;
                        open_Option = false;
                        gamePlay = true;
                        break;
                }
                input = true;
            }
        }

        //�������h�~
        if (Input.GetKeyUp(KeyCode.Escape) && input)
        {
            input = false;
        }
        //�Q�[���I�[�o�[
        if (playerController.death && !isBgm)  
        {
            gamePlay = false;
            result_Bgm.PlayOneShot(over_Bgm);
            main_Bgm.Stop();
            isBgm = true;
            Invoke("Death", 3.0f);
        }
        //�Q�[���N���A
        if (gameClear && !isBgm) 
        {
            gamePlay = false;
            result_Bgm.PlayOneShot(clear_Bgm);
            main_Bgm.Stop();
            isBgm = true;
            Invoke("Clear", 1.0f);

        }
        //�����𖞂����ƃS�[�����o��
        if (playerController.kill_enemy >= 5 && !spawn)
        {
            spawn = true;
            goalNum = Random.Range(0, 5);
            goal[goalNum].SetActive(true);
            spawnGoal.SetActive(true);
        }
    }

    void Death()
    {
        //Cursor.visible = true;//�J�[�\���\��
        gameOver = true;
        overPanel.SetActive(true);
        Time.timeScale = 0;
    }

    void Clear()
    {
        //Cursor.visible = true;//�J�[�\���\��
        gameClear = true;
        clearPanel.SetActive(true);
        Time.timeScale = 0;
    }
}
