using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIScript : MonoBehaviour
{
    // UI�v�f
    public GameObject menuPanel, killCounter, pause, reunion, esc_icon;
    public new GameObject camera;
    public Image weapon_icon, skill_icon;
    public Text currentKill, goalSpawnKill;

    // ����ƃX�L���̃A�C�R��
    public Sprite[] weapon, weapon_interval, skill;

    // �Q�[���̏�Ԃ��Ǘ�
    private GameManager gameManager;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        // �����ݒ�
        menuPanel.SetActive(false);
        reunion.SetActive(false);
        weapon_icon = GetComponent<Image>();
        skill_icon = GetComponent<Image>();

        UpdateSkillIcon(); // �A�C�R�����X�V
    }

    // Update is called once per frame
    void Update()
    {
        // �Q�[���}�l�[�W���[�ƃv���C���[�R���g���[���[���擾
        gameManager = GameObject.Find("Player").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        // �|�[�Y�⃁�j���[�̐؂�ւ�
        if (!gameManager.gameClear && !gameManager.gameOver)
        {
            HandleMenu(); // ���j���[�\���̏���
        }
        else
        {
            camera.SetActive(false);
            esc_icon.SetActive(false);
        }

        // KILL�J�E���g�ƃS�[���̏������X�V
        UpdateKillCount();

        // �S�[�������B����KILL�J�E���g���\����
        if (playerController.killEnemy >= 5) killCounter.SetActive(false);

        //�C���^�[�o�����̃A�C�R������
        UpdateWeaponIcon();
    }

    //�g�p�������A�C�R���Ƃ��ĕ\��
    void UpdateWeaponIcon()
    {
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

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

    // �v���C���[�̃X�L���ɉ����ăX�L���A�C�R�����X�V
    void UpdateSkillIcon()
    {
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        //AP2�{
        if (playerController.skill <= 20)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[0];
        }
        //HP2�{
        else if (playerController.skill <= 40)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[1];
        }
        //�U����2�{
        else if (playerController.skill <= 50)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[2];
        }
        //��_���[�W2�{
        else if (playerController.skill <= 60)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[3];
        }
        //�ړ�1.5�{�E�U����0.75�{
        else if (playerController.skill <= 70)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[4];
        }
        //�ړ�0.75�{�E�U����1.5�{
        else if (playerController.skill <= 80)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[5];
        }
        //����AP�E�U����2�{
        else if (playerController.skill <= 90)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[6];
        }
        //��_���[�W2�{�E�^�_���[�W0.5�{
        else if (playerController.skill <= 95)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[7];
        }
        //��_���[�W0.5�{�E�^�_���[�W2�{ 0
        else if (playerController.skill <= 100)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[8];
        }
    }

    // KILL�J�E���g���X�V���鏈��
    private void UpdateKillCount()
    {
        currentKill.text = playerController.killEnemy.ToString(); // ���݂�KILL��
        goalSpawnKill.text = playerController.goalSpawn.ToString(); // �S�[���o���ɕK�v��KILL��
    }

    public enum Weapon
    {
        KNIFE,
        SWORD,
        KNUCKLE
    }
}
