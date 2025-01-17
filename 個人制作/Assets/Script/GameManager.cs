using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool gameOver;
    public bool gameClear;
    public bool gamePlay;

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
        gamePlay = true;
        clearPanel.SetActive(false);
        overPanel.SetActive(false);
        spawnGoal.SetActive(false);
        open_Option = false;
        input = false;

        // �J�[�\������ʒ����Ƀ��b�N����
        Cursor.lockState = CursorLockMode.Locked;

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
                        open_Option = true;
                        // �J�[�\�������R�ɓ�������
                        Cursor.lockState = CursorLockMode.None;
                        Time.timeScale = 0;
                        break;
                    case true:
                        Time.timeScale = 1;
                        // �J�[�\������ʒ����Ƀ��b�N����
                        Cursor.lockState = CursorLockMode.Locked;
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
        if (gameOver && gamePlay)   
        {
            gamePlay = false;
            Invoke("Over", 3.5f);
        }
        //�Q�[���N���A
        if (gameClear && gamePlay)  
        {
            gamePlay = false;
            Invoke("Clear", 0.3f);
        }
        //�����𖞂����ƃS�[�����o��
        if (playerController.killEnemy >= 5 && !spawn)
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

    void Over()
    {
        overPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
    void Clear()
    {
        clearPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
}
