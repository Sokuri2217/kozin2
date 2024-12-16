using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameOver;
    public bool gameClear;

    public GameObject clearPanel;
    public GameObject overPanel;
    public GameObject spawnGoal;
    public GameObject[] goal;
    private int goalNum = 0;
    private bool spawn = false;

    //�Q�[�����
    public bool open_Option;
    //�������h�~
    public bool input;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        gameClear = false;
        clearPanel.SetActive(false);
        overPanel.SetActive(false);
        spawnGoal.SetActive(false);
        open_Option = false;
        input = false;
        //�J�[�\����\��
        Cursor.visible = false;

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
                        Cursor.visible = true;//�J�[�\���\��
                        open_Option = true;
                        Time.timeScale = 0;
                        break;
                    case true:
                        Cursor.visible = false;//�J�[�\����\��
                        Time.timeScale = 1;
                        open_Option = false;
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
        if (playerController.death)  
        {
            gameOver = true;
            Cursor.visible = true;//�J�[�\���\��
            overPanel.SetActive(true);
            Time.timeScale = 0;
        }
        //�Q�[���N���A
        if (gameClear) 
        {
            gameClear = true;
            Cursor.visible = true;//�J�[�\���\��
            clearPanel.SetActive(true);
            Time.timeScale = 0;
        }
        //�����𖞂����ƃS�[�����o��
        if (playerController.kill_enemy >= 5 && !spawn)
        {
            spawn = true;
            goalNum = Random.Range(0, 5);
            goal[goalNum].SetActive(true);
            spawnGoal.SetActive(true);
        }

        //�N���A���Q�[���I�[�o�[�ɂȂ��spawnGoal���\���ɂ���
        if(gameClear||gameOver)
        {
            spawnGoal.SetActive(false);
        }
    }

    void Death()
    {
        
    }

    void Clear()
    {
        
    }
}
