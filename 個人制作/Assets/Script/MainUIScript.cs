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
    //武器アイコン
    public Image weapon_icon;
    public Sprite[] wepon;
    public Sprite[] wepon_interval;
    //スキルアイコン
    public  Image skill_icon;
    public Sprite[] skill;

    //ゲーム状態
    public bool open_Option = false;

    //ゴール条件
    public Text currentKill;
    public Text goalSpawnKill;
    //ステータス表記
    public Text now_hpNum;
    public Text max_hpNum;
    public Text now_apNum;
    public Text max_apNum;
    public RectTransform hp;
    bool hpSlide;
    bool hpSlide2;
    public RectTransform ap;
    bool apSlide;
    bool apSlide2;

    //長押し防止
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

        hpSlide = false;
        hpSlide2 = false;
        apSlide = false;
        apSlide2 = false;

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

        //オプション管理
        //Escで操作
        if(gameManager.gamePlay)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !input)
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
        }
        //ボタンで操作
        if(!open_Option)
        {
            menuPanel.SetActive(false);
            camera.SetActive(true);
            gameManager.gamePlay = true;
        } 
        //長押し防止
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

        if (playerController.currentHp < 100.0f && !hpSlide)
        {
            hp.position += new Vector3(0.01f, 0, 0);
            hpSlide = true;
        }
        else if (playerController.currentHp >= 100.0f && hpSlide)
        {
            hp.position -= new Vector3(0.01f, 0, 0);
            hpSlide = false;
        }

        if (playerController.currentAp < 100.0f && !apSlide)
        {
            ap.position += new Vector3(0.01f, 0, 0);
            apSlide = true;
        }
        else if (playerController.currentAp >= 100.0f && apSlide)
        {
            ap.position -= new Vector3(0.01f, 0, 0);
            apSlide = false;
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

    //使用装備をアイコンとして表示
    void Icon()
    {
        PlayerController playerController;
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        if (playerController.interval || playerController.apLost) 
        {
            //武器
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
            //武器
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
       
        //付与効果
        //AP2倍
        if (playerController.skill >= 1 && playerController.skill <= 20)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[0];
        }
        //HP2倍
        else if (playerController.skill >= 21 && playerController.skill <= 40)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[1];
        }
        //攻撃力2倍
        else if (playerController.skill >= 41 && playerController.skill <= 50)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[2];
        }
        //被ダメージ2倍
        else if (playerController.skill >= 51 && playerController.skill <= 60)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[3];
        }
        //移動1.5倍・攻撃力0.75倍
        else if (playerController.skill >= 61 && playerController.skill <= 70)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[4];
        }
        //移動0.75倍・攻撃力1.5倍
        else if (playerController.skill >= 71 && playerController.skill <= 80)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[5];
        }
        //消費AP・攻撃力2倍
        else if (playerController.skill >= 81 && playerController.skill <= 90)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[6];
        }
        //被ダメージ2倍・与ダメージ0.5倍
        else if (playerController.skill >= 91 && playerController.skill <= 95)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[7];
        }
        //被ダメージ0.5倍・与ダメージ2倍 0
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
