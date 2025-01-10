using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIScript : MonoBehaviour
{
    //UI
    public GameObject menuPanel;
    public new GameObject camera;
    public GameObject killCounter;
    //����A�C�R��
    public Image weapon_icon;
    public Sprite[] wepon;
    public Sprite[] wepon_interval;
    //�X�L���A�C�R��
    public  Image skill_icon;
    public Sprite[] skill;

    public bool input = false;

    //�S�[������
    public Text currentKill;
    public Text goalSpawnKill;

    //public Image fadePanel;             // �t�F�[�h�p��UI�p�l���iImage�j
    //public float fadeDuration = 1.0f;   // �t�F�[�h�̊����ɂ����鎞��

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(FadeOutAndLoadScene());

        menuPanel.SetActive(false);
        camera.SetActive(true);
        killCounter.SetActive(true);

        weapon_icon = GetComponent<Image>();
        skill_icon = GetComponent<Image>();

        //����\��
        Icon();
    }

    // Update is called once per frame
    void Update()
    {
        GameManager gameManager;
        PlayerController playerController;
        GameObject obj = GameObject.Find("Player");
        gameManager = obj.GetComponent<GameManager>();
        playerController = obj.GetComponent<PlayerController>();

        //�I�v�V�����Ǘ�
        //�{�^���ő���
        if(!gameManager.gameClear && !gameManager.gameOver)
        {
            if (gameManager.open_Option)
            {
                // �J�[�\�������R�ɓ�������
                Cursor.lockState = CursorLockMode.None;
                menuPanel.SetActive(true);
                camera.SetActive(false);
            }
            else
            {
                // �J�[�\������ʒ����Ƀ��b�N����
                Cursor.lockState = CursorLockMode.Locked;
                menuPanel.SetActive(false);
                camera.SetActive(true);
            }
        }

        if (gameManager.gameClear || gameManager.gameOver) 
        {
            camera.SetActive(false);
        }
        //KILL�J�E���g��\��������
        currentKill.text = playerController.kill_enemy.ToString();
        goalSpawnKill.text = playerController.goalspawn.ToString();

        //�S�[���o������KILL�J�E���g���\���ɂ���
        if (playerController.kill_enemy >= 5)
            killCounter.SetActive(false);

        //�C���^�[�o�����̃A�C�R������
        Icon();
    }

    //�g�p�������A�C�R���Ƃ��ĕ\��
    void Icon()
    {
        PlayerController playerController;
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        if (playerController.apLost) 
        {
            //����
            switch (playerController.weapon)
            {
                case (int)Weapon.KNIFE:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon_interval[(int)Weapon.KNIFE];
                    break;
                case (int)Weapon.SWORD:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon_interval[(int)Weapon.SWORD];
                    break;
                case (int)Weapon.KNUCKLE:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon_interval[(int)Weapon.KNUCKLE];
                    break;
            }
        }
        else
        {
            //����
            switch (playerController.weapon)
            {
                case (int)Weapon.KNIFE:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon[(int)Weapon.KNIFE];
                    break;
                case (int)Weapon.SWORD:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon[(int)Weapon.SWORD];
                    break;
                case (int)Weapon.KNUCKLE:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon[(int)Weapon.KNUCKLE];
                    break;
            }
        }
       
        //�t�^����
        //AP2�{
        if (playerController.skill >= 1 && playerController.skill <= 20)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[0];
        }
        //HP2�{
        else if (playerController.skill >= 21 && playerController.skill <= 40)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[1];
        }
        //�U����2�{
        else if (playerController.skill >= 41 && playerController.skill <= 50)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[2];
        }
        //��_���[�W2�{
        else if (playerController.skill >= 51 && playerController.skill <= 60)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[3];
        }
        //�ړ�1.5�{�E�U����0.75�{
        else if (playerController.skill >= 61 && playerController.skill <= 70)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[4];
        }
        //�ړ�0.75�{�E�U����1.5�{
        else if (playerController.skill >= 71 && playerController.skill <= 80)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[5];
        }
        //����AP�E�U����2�{
        else if (playerController.skill >= 81 && playerController.skill <= 90)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[6];
        }
        //��_���[�W2�{�E�^�_���[�W0.5�{
        else if (playerController.skill >= 91 && playerController.skill <= 95)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[7];
        }
        //��_���[�W0.5�{�E�^�_���[�W2�{ 0
        else if (playerController.skill >= 96 && playerController.skill <= 100)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[8];
        }
    }

    public enum Weapon
    {
        KNIFE,
        SWORD,
        KNUCKLE
    }

    //public IEnumerator FadeOutAndLoadScene()
    //{
    //    fadePanel.enabled = true;                 // �p�l����L����
    //    float elapsedTime = 0.0f;                 // �o�ߎ��Ԃ�������
    //    Color startColor = fadePanel.color;       // �t�F�[�h�p�l���̊J�n�F���擾
    //    Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // �t�F�[�h�p�l���̍ŏI�F��ݒ�

    //    // �t�F�[�h�A�E�g�A�j���[�V���������s
    //    while (elapsedTime < fadeDuration)
    //    {
    //        elapsedTime += Time.deltaTime;                        // �o�ߎ��Ԃ𑝂₷
    //        float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // �t�F�[�h�̐i�s�x���v�Z
    //        fadePanel.color = Color.Lerp(startColor, endColor, t); // �p�l���̐F��ύX���ăt�F�[�h�A�E�g
    //        yield return null;                                     // 1�t���[���ҋ@
    //    }

    //    fadePanel.color = endColor;  // �t�F�[�h������������ŏI�F�ɐݒ�
    //}
}
