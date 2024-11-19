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
    //ïêäÌÉAÉCÉRÉì
    public Image weapon_icon;
    public Sprite wepon_knife;
    public Sprite wepon_knife_interval;
    public Sprite wepon_sword;
    public Sprite wepon_sword_interval;
    public Sprite wepon_spear;
    public Sprite wepon_spear_interval;
    //ÉXÉLÉãÉAÉCÉRÉì
    public  Image skill_icon;
    public Sprite ap_x2;
    public Sprite hp_x2;
    public Sprite attack_x2;
    public Sprite defense_x05;
    public Sprite speed15_attck075;
    public Sprite speed075_attack15;
    public Sprite useap2_attack2;
    public Sprite defense05_attack05;
    public Sprite defense2_attack2;
    

    //ÉQÅ[ÉÄèÛë‘
    public bool open_Option = false;
    public bool gameset = false;
    public Text currentKill;
    public Text goalSpawnKill;

    //í∑âüÇµñhé~
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

        //ÉIÉvÉVÉáÉìä«óù
        //EscÇ≈ëÄçÏ
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
        //É{É^ÉìÇ≈ëÄçÏ
        if(!open_Option)
        {
            menuPanel.SetActive(false);
            camera.SetActive(true);
            gameManager.gamePlay = true;
        } 
        //í∑âüÇµñhé~
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

        if (playerController.kill_enemy >= 5) 
        {
            killCounter.SetActive(false);
        }

        Icon();
    }

    //égópëïîıÇÉAÉCÉRÉìÇ∆ÇµÇƒï\é¶
    void Icon()
    {
        PlayerController playerController;
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        if (playerController.interval)
        {
            //ïêäÌ
            switch (playerController.weapon)
            {
                case (int)Weapon.Knife:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon_knife_interval;
                    break;
                case (int)Weapon.Sword:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon_sword_interval;
                    break;
                case (int)Weapon.Spear:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon_spear_interval;
                    break;
            }
        }
        else if (!playerController.interval)
        {
            //ïêäÌ
            switch (playerController.weapon)
            {
                case (int)Weapon.Knife:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon_knife;
                    break;
                case (int)Weapon.Sword:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon_sword;
                    break;
                case (int)Weapon.Spear:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon_spear;
                    break;
            }
        }
       
        //ïtó^å¯â 
        //AP2î{
        if (playerController.skill >= 1 && playerController.skill <= 20)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = ap_x2;
        }
        //HP2î{
        else if (playerController.skill >= 21 && playerController.skill <= 40)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = hp_x2;
        }
        //çUåÇóÕ2î{
        else if (playerController.skill >= 41 && playerController.skill <= 50)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = attack_x2;
        }
        //îÌÉ_ÉÅÅ[ÉW2î{
        else if (playerController.skill >= 51 && playerController.skill <= 60)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = defense_x05;
        }
        //à⁄ìÆ1.5î{ÅEçUåÇóÕ0.75î{
        else if (playerController.skill >= 61 && playerController.skill <= 70)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = speed15_attck075;
        }
        //à⁄ìÆ0.75î{ÅEçUåÇóÕ1.5î{
        else if (playerController.skill >= 71 && playerController.skill <= 80)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = speed075_attack15;
        }
        //è¡îÔAPÅEçUåÇóÕ2î{
        else if (playerController.skill >= 81 && playerController.skill <= 90)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = useap2_attack2;
        }
        //îÌÉ_ÉÅÅ[ÉW2î{ÅEó^É_ÉÅÅ[ÉW0.5î{
        else if (playerController.skill >= 91 && playerController.skill <= 95)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = defense05_attack05;
        }
        //îÌÉ_ÉÅÅ[ÉW0.5î{ÅEó^É_ÉÅÅ[ÉW2î{ 0
        else if (playerController.skill >= 96 && playerController.skill <= 100)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = defense2_attack2;
        }
    }

    public enum Weapon
    {
        Knife,
        Sword,
        Spear
    }
}
