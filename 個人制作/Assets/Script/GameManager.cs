using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool gameOver;
    public bool gameClear;
    public bool gamePlay;

    public GameObject createPrefab;
    public Transform createSpace;
    public GameObject clearPanel;
    public GameObject overPanel;
    public GameObject spawnGoal;
    public GameObject[] goal;
    private int goalNum = 0;
    private bool spawn = false;
    private int maxEnemy;
    public int currentEnemy;

    // �o�ߎ���
    private float time;
    public float spawnTime;
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
        maxEnemy = 10;
        currentEnemy = 0;

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
                        playerController.isStop = true;
                        // �J�[�\�������R�ɓ�������
                        Cursor.lockState = CursorLockMode.None;
                        Time.timeScale = 0;
                        break;
                    case true:
                        Time.timeScale = 1;
                        // �J�[�\������ʒ����Ƀ��b�N����
                        Cursor.lockState = CursorLockMode.Locked;
                        open_Option = false;
                        playerController.isStop = false;
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

        // �O�t���[������̎��Ԃ����Z���Ă���
        time = time + Time.deltaTime;
        // ��1�b�u���Ƀ����_���ɐ��������悤�ɂ���B
        if (time > spawnTime && currentEnemy <= maxEnemy) 
        {
            // rangeA��rangeB��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
            float x = Random.Range(createSpace.position.x, createSpace.position.x);
            // rangeA��rangeB��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
            float y = Random.Range(createSpace.position.y, createSpace.position.y);
            // rangeA��rangeB��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
            float z = Random.Range(createSpace.position.z, createSpace.position.z);
            // GameObject����L�Ō��܂��������_���ȏꏊ�ɐ���
            Instantiate(createPrefab, new Vector3(x, y, z), createPrefab.transform.rotation);
            // �o�ߎ��ԃ��Z�b�g
            time = 0f;
            //�������J�E���g����
            currentEnemy++;
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
        if (playerController.killEnemy >= playerController.goalSpawn && !spawn)
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
