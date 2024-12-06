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
    private bool bgm;

    public AudioSource mainBgm;
    private AudioSource result_Bgm;
    public AudioClip over_Bgm;
    public AudioClip clear_Bgm;

    // Start is called before the first frame update
    void Start()
    {
        gamePlay = true;
        gameOver = false;
        gameClear = false;
        bgm = false;
        clearPanel.SetActive(false);
        overPanel.SetActive(false);
        spawnGoal.SetActive(false);
        result_Bgm = GetComponent<AudioSource>();
        mainBgm = GetComponent<AudioSource>();
        for (int i = 0; i < 5; i++) 
        {
            goal[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController playerController = GetComponent<PlayerController>();

        if (playerController.currentHp <= 0) 
        {
            gameOver = true;
            gamePlay = false;
            overPanel.SetActive(true);
            Time.timeScale = 0;
        }

        if (gameClear) 
        {
            gameClear = true;
            gamePlay = false;
            clearPanel.SetActive(true);
            Time.timeScale = 0;
        }

        //if (playerController.kill_enemy >= 5 && !spawn||
        //    Input.GetMouseButton(1)) 
        //{
        //    spawn = true;
        //    goalNum = Random.Range(0, 5);
        //    goal[goalNum].SetActive(true);
        //    spawnGoal.SetActive(true);
        //}
        if (playerController.kill_enemy >= 5 && !spawn)
        {
            spawn = true;
            goalNum = Random.Range(0, 5);
            goal[goalNum].SetActive(true);
            spawnGoal.SetActive(true);
        }
    }
}
