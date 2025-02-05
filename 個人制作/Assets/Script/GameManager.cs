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
    public GameObject spawnArea;
    private int goalNum = 0;
    public bool spawn = false;
    public int maxCount;
    public int currentCount;
    public int killEnemy;             //�|�����G��
    public int goalSpawn;             //�S�[���o���ɕK�v�ȓG��
    public bool isDamageSe;            //�G�̔�ese���Ȃ��Ă���Œ����ǂ���
    public bool isDeathSe;            //�G�̎��Sse���Ȃ��Ă���Œ����ǂ���

    //�{�X�֘A
    //public bool spawnBoss;    //�{�X�o���t���O
    //public GameObject boss;   //�I�u�W�F�N�g
    //public GameObject bossHp; //HP�o�[

    //�Q�[�����
    public bool open_Option;
    //�������h�~
    public bool input;

    public Image fadePanel;             // �t�F�[�h�p��UI�p�l���iImage�j
    public float fadeDuration = 1.0f;   // �t�F�[�h�̊����ɂ����鎞��

    // Start is called before the first frame update
    void Start()
    {
        killEnemy = 0;
        gameOver = false;
        gameClear = false;
        gamePlay = true;
        clearPanel.SetActive(false);
        overPanel.SetActive(false);
        spawnGoal.SetActive(false);
        spawnArea.SetActive(false);
        //boss.SetActive(false);
        open_Option = false;
        input = false;
        isDamageSe = false;
        isDeathSe = false;
        //spawnBoss = false;
        //bossHp.SetActive(false);
        

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
                        playerController.isStop = false;
                        // �J�[�\������ʒ����Ƀ��b�N����
                        Cursor.lockState = CursorLockMode.Locked;
                        open_Option = false;
                        break;
                }
                input = true;
            }
        }
        //�f�o�b�O�p
        {
            //if (Input.GetKeyDown(KeyCode.E))
            //{
            //    currentCount = maxCount;
            //}

            ////�{�X�o��
            //if (currentCount >= maxCount)
            //{
            //    currentCount = maxCount;
            //    if(!spawnBoss)
            //    {
            //        Time.timeScale = 1;
            //        spawnBoss = true;
            //        StartCoroutine(FadeOutAndLoadScene());
            //    }
            //}
        }

        //�������h�~
        if (Input.GetKeyUp(KeyCode.Escape) && input)
        {
            input = false;
        }
        //�L���J�E���g�̐���
        if (killEnemy >= goalSpawn) 
        {
            killEnemy = goalSpawn;
            //�����𖞂����ƃS�[�����o��
            if (!spawn)
            {
                spawn = true;
                goalNum = Random.Range(0, 5);
                goal[goalNum].SetActive(true);
                spawnGoal.SetActive(true);
                spawnArea.SetActive(true);
            }
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

    public IEnumerator FadeOutAndLoadScene()
    {
        fadePanel.enabled = true;                 // �p�l����L����
        float elapsedTime = 0.0f;                 // �o�ߎ��Ԃ�������
        Color startColor = fadePanel.color;       // �t�F�[�h�p�l���̊J�n�F���擾
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // �t�F�[�h�p�l���̍ŏI�F��ݒ�

        // �t�F�[�h�A�E�g�A�j���[�V���������s
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // �o�ߎ��Ԃ𑝂₷
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // �t�F�[�h�̐i�s�x���v�Z
            fadePanel.color = Color.Lerp(startColor, endColor, t); // �p�l���̐F��ύX���ăt�F�[�h�A�E�g
            yield return null;                                     // 1�t���[���ҋ@
        }

        fadePanel.color = endColor;  // �t�F�[�h������������ŏI�F�ɐݒ�
        fadePanel.enabled = false;
        //boss.SetActive(true);
        //bossHp.SetActive(true);
        Time.timeScale = 1;
    }
}
