using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIScript : MonoBehaviour
{
    //UIパネル
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject camera;
    //ゲーム状態
    public bool open_Option = false;
    //長押し防止
    public bool input = false;

    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(false);
        camera.SetActive(true);
        open_Option = false;
    }

    // Update is called once per frame
    void Update()
    {
        //オプション管理
        //Escで操作
        if (Input.GetKeyDown(KeyCode.Escape) && !input) 
        {
            switch (open_Option)
            {
                case false:
                    menuPanel.SetActive(true);
                    camera.SetActive(false);
                    open_Option = true;
                    break;
                case true:
                    menuPanel.SetActive(false);
                    camera.SetActive(true);
                    open_Option = false;
                    break;
            }
            input = true;
        }
        //ボタンで操作
        if(!open_Option)
        {
            menuPanel.SetActive(false);
            camera.SetActive(true);
        }
            

        if (Input.GetKeyUp(KeyCode.Escape)&&input)
        {
            input = false;
        }
    }

    //使用装備をアイコンとして表示
    void Icon()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        //武器
        switch (playerController.weapon)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
        //付与効果
        switch(playerController.skill)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            default:
                break;
        }
    }

    //体力・APをゲージとして表示
    void Gauge()
    {

    }
}
