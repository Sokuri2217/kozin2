using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gamePlay;
    public bool gameOver;
    public bool gameClear;

    public GameObject clearPanel;
    public GameObject overPanel;
    public GameObject spawnGoal;
    public GameObject[] goal;
    private int goalNum = 0;
    private bool spawn = false;

    public AudioSource main_Bgm;

    private AudioSource result_Bgm;
    public AudioClip clear_Bgm;
    public AudioClip over_Bgm;

    public bool isBgm;

    //ゲーム状態
    public bool open_Option;
    //長押し防止
    public bool input;

    // Start is called before the first frame update
    void Start()
    {
        gamePlay = true;
        gameOver = false;
        gameClear = false;
        clearPanel.SetActive(false);
        overPanel.SetActive(false);
        spawnGoal.SetActive(false);
        main_Bgm = GetComponent<AudioSource>();
        result_Bgm = GetComponent<AudioSource>();
        isBgm = false;
        open_Option = false;
        input = false;
        //カーソル非表示
        //Cursor.visible = false;

        for (int i = 0; i < 5; i++)
            goal[i].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController playerController = GetComponent<PlayerController>();

        //決着がつくと動かないようにする
        if (!gameClear && !gameOver)
        {
            //Escで一時停止させる
            if (Input.GetKeyDown(KeyCode.Escape) && !input)
            {
                switch (open_Option)
                {
                    case false:
                        //Cursor.visible = true;//カーソル表示
                        open_Option = true;
                        gamePlay = false;
                        Time.timeScale = 0;
                        break;
                    case true:
                        //Cursor.visible = false;//カーソル非表示
                        Time.timeScale = 1;
                        open_Option = false;
                        gamePlay = true;
                        break;
                }
                input = true;
            }
        }

        //長押し防止
        if (Input.GetKeyUp(KeyCode.Escape) && input)
        {
            input = false;
        }
        //ゲームオーバー
        if (playerController.death && !isBgm)  
        {
            gamePlay = false;
            result_Bgm.PlayOneShot(over_Bgm);
            main_Bgm.Stop();
            isBgm = true;
            Invoke("Death", 3.0f);
        }
        //ゲームクリア
        if (gameClear && !isBgm) 
        {
            gamePlay = false;
            result_Bgm.PlayOneShot(clear_Bgm);
            main_Bgm.Stop();
            isBgm = true;
            Invoke("Clear", 1.0f);

        }
        //条件を満たすとゴールを出す
        if (playerController.kill_enemy >= 5 && !spawn)
        {
            spawn = true;
            goalNum = Random.Range(0, 5);
            goal[goalNum].SetActive(true);
            spawnGoal.SetActive(true);
        }
    }

    void Death()
    {
        //Cursor.visible = true;//カーソル表示
        gameOver = true;
        overPanel.SetActive(true);
        Time.timeScale = 0;
    }

    void Clear()
    {
        //Cursor.visible = true;//カーソル表示
        gameClear = true;
        clearPanel.SetActive(true);
        Time.timeScale = 0;
    }
}
