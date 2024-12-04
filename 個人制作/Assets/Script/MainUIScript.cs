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

    //�Q�[�����
    public bool open_Option = false;
    public bool gameset = false;
    public Text currentKill;
    public Text goalSpawnKill;
    public Text now_hpNum;
    public Text max_hpNum;
    public Text now_apNum;
    public Text max_apNum;

    //�������h�~
    public bool input = false;

    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(false);
        camera.SetActive(true);
        killCounter.SetActive(true);

        weapon_icon = GetComponent<Image>();
        skill_icon = GetComponent<Image>();
        open_Option = false;

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
        //Esc�ő���
        if (Input.GetKeyDown(KeyCode.Escape) && !input && gameManager.gamePlay)  
        {
            switch (open_Option)
            {
                case false:
                    menuPanel.SetActive(true);
                    camera.SetActive(false);
                    gameManager.gamePlay = false;
                    open_Option = true;
                    break;
                case true:
                    menuPanel.SetActive(false);
                    camera.SetActive(true);
                    gameManager.gamePlay = true;
                    open_Option = false;
                    break;
            }
            input = true;
        }
        //�{�^���ő���
        if(!open_Option)
        {
            menuPanel.SetActive(false);
            camera.SetActive(true);
            gameManager.gamePlay = true;
        } 
        //�������h�~
        if (Input.GetKeyUp(KeyCode.Escape)&&input)
        {
            input = false;
        }

        if(!gameManager.gamePlay)
        {
            camera.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        currentKill.text = playerController.kill_enemy.ToString();
        goalSpawnKill.text = playerController.goalspawn.ToString();

        now_hpNum.text = playerController.currentHp.ToString();
        max_hpNum.text = playerController.maxHp.ToString();
        now_apNum.text = playerController.currentAp.ToString();
        max_apNum.text = playerController.maxAp.ToString();

        if (playerController.kill_enemy >= 5) 
        {
            killCounter.SetActive(false);
        }

        Icon();
    }

    //�g�p�������A�C�R���Ƃ��ĕ\��
    void Icon()
    {
        PlayerController playerController;
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        if (playerController.interval || playerController.apLost) 
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
        else if (!playerController.interval)
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
}
