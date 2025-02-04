using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIScript : MonoBehaviour
{
    // UI�v�f
    public GameObject menuPanel;
    public GameObject killCounter;
    public GameObject pause;
    public GameObject reunion;
    public GameObject escIcon;
    public new GameObject camera;
    public Image weaponIcon;
    public Image skillIcon;
    public Image changeIcon;
    public Text currentKill;
    public Text goalSpawnKill;
    public Text chaseEnemy;
    public int randomSkill;
    public float changeTime;   //�ω����N����܂ł̎���
    public float change;       //�v���p
    public bool changeConsent; //�ω��J�n�t���O
    public GameObject keyR;    //�ω�����{�^��

    // ����ƃX�L���̃A�C�R��
    public Sprite[] weapon;
    public Sprite[] weapon_interval;
    public Sprite[] skills;

    //���͊֘A
    public bool changeInput; //�C�ӕω�

    //���O�̕ω���ۑ�
    public int beforeStatus;


    // �Q�[���̏�Ԃ��Ǘ�
    public GameManager gameManager;
    public PlayerController playerController;

    public enum Weapon
    {
        KNIFE,
        SWORD,
        KNUCKLE
    }

    // Start is called before the first frame update
    void Start()
    {
        // �����ݒ�
        menuPanel.SetActive(false);
        reunion.SetActive(false);
        keyR.SetActive(false);
        changeConsent = false;
        changeInput = false;
        beforeStatus = -1;
        changeIcon.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // �|�[�Y�⃁�j���[�̐؂�ւ�
        if (!gameManager.gameClear && !gameManager.gameOver)
        {
            HandleMenu(); // ���j���[�\���̏���

            //�ꎞ��~���͓����Ȃ�
            if (!gameManager.open_Option) 
            {
                //�X�e�[�^�X�ω�
                ChangeExtLuck();
            }
        }
        else
        {
            camera.SetActive(false);
            escIcon.SetActive(false);
        }

        // �S�[�������B����KILL�J�E���g���\����
        if (gameManager.killEnemy >= gameManager.goalSpawn) 
            killCounter.SetActive(false);

        //�X�e�[�^�X�ω��Q�[�W�̏�Ԃ��X�V
        changeIcon.fillAmount = change / changeTime;

        // KILL�J�E���g�ƃS�[���̏������X�V
        UpdateKillCount();
        //�C���^�[�o�����̃A�C�R������
        UpdateWeaponIcon();
        // �A�C�R�����X�V
        UpdateSkillIcon();
        //�A�C�R���ύX
        GameObject.Find("Skill").GetComponent<Image>().sprite = skills[randomSkill];
        //���݂̕ω���ۑ�
        beforeStatus = randomSkill;
    }

    //�g�p�������A�C�R���Ƃ��ĕ\��
    void UpdateWeaponIcon()
    {
        // �v���C���[�̕���̏�Ԃɉ����ăA�C�R����ύX
        if (playerController.apLost)
        {
            // AP������Ȃ��ꍇ�A�C���^�[�o���p�̕���A�C�R����\��
            GameObject.Find("Weapon").GetComponent<Image>().sprite = weapon_interval[playerController.weapon];
        }
        else
        {
            // �ʏ�̕���A�C�R����\��
            GameObject.Find("Weapon").GetComponent<Image>().sprite = weapon[playerController.weapon];
        }
    }

    // ���j���[�̕\���Ɣ�\����؂�ւ��鏈��
    private void HandleMenu()
    {
        if (gameManager.open_Option)
        {
            // �I�v�V�������j���[��\��
            Cursor.lockState = CursorLockMode.None; // �J�[�\�������R�ɓ�������悤�ɂ���
            menuPanel.SetActive(true);
            pause.SetActive(false);
            reunion.SetActive(true);
            camera.SetActive(false);
        }
        else
        {
            // �ʏ�Q�[����ʂɖ߂�
            Cursor.lockState = CursorLockMode.Locked; // �J�[�\������ʒ����Ƀ��b�N
            menuPanel.SetActive(false);
            reunion.SetActive(false);
            pause.SetActive(true);
            camera.SetActive(true);
        }
    }

    void ChangeExtLuck()
    {
        if (!changeConsent)
        {
            change++;

            if (change >= changeTime)
            {
                changeConsent = true;
                keyR.SetActive(true);
                change = changeTime;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R) && !changeInput)
            {
                do
                {
                    playerController.skill = Random.Range(1, 100);

                    // �A�C�R�����X�V
                    UpdateSkillIcon();
                } while (beforeStatus == randomSkill);
                beforeStatus = randomSkill;
                keyR.SetActive(false);
                changeConsent = false;
                changeInput = true;
                change = 0.0f;
                //�A�C�R���ύX
                GameObject.Find("Skill").GetComponent<Image>().sprite = skills[randomSkill];
            }
        }

        if (Input.GetKeyUp(KeyCode.R) && changeInput)
        {
            changeInput = false;
        }
    }

    // �v���C���[�̃X�L���ɉ����ăX�L���A�C�R�����X�V
    void UpdateSkillIcon()
    {
        //AP2�{
        if (playerController.skill <= 20)
        {
            randomSkill = 0;
        }
        //HP2�{
        else if (playerController.skill <= 40)
        {
            randomSkill = 1;
        }
        //�U����2�{
        else if (playerController.skill <= 50)
        {
            randomSkill = 2;
        }
        //��_���[�W2�{
        else if (playerController.skill <= 60)
        {
            randomSkill = 3;
        }
        //�ړ�1.5�{�E�U����0.75�{
        else if (playerController.skill <= 70)
        {
            randomSkill = 4;
        }
        //�ړ�0.75�{�E�U����1.5�{
        else if (playerController.skill <= 80)
        {
            randomSkill = 5;
        }
        //����AP�E�U����2�{
        else if (playerController.skill <= 90)
        {
            randomSkill = 6;
        }
        //��_���[�W2�{�E�^�_���[�W0.5�{
        else if (playerController.skill <= 95)
        {
            randomSkill = 7;
        }
        //��_���[�W0.5�{�E�^�_���[�W2�{
        else if (playerController.skill <= 100)
        {
            randomSkill = 8;
        }
    }

    // KILL�J�E���g���X�V���鏈��
    private void UpdateKillCount()
    {
        currentKill.text = gameManager.killEnemy.ToString(); // ���݂�KILL��
        goalSpawnKill.text = gameManager.goalSpawn.ToString(); // �S�[���o���ɕK�v��KILL��
    }

    ////�`�F�C�X���̓G���X�V
    //private void UpdateChaseCount()
    //{
    //    chaseEnemy.text
    //}
}
