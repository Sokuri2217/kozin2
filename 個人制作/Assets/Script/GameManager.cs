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

        for (int i = 0; i < 5; i++)
            goal[i].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController playerController = GetComponent<PlayerController>();

        if (playerController.currentHp <= 0 && !isBgm)  
        {
            gameOver = true;
            gamePlay = false;
            overPanel.SetActive(true);
            result_Bgm.PlayOneShot(over_Bgm);
            main_Bgm.Stop();
            isBgm = true;
            Time.timeScale = 0;
        }
        if (gameClear && !isBgm) 
        {
            gameClear = true;
            gamePlay = false;
            clearPanel.SetActive(true);
            result_Bgm.PlayOneShot(clear_Bgm);
            main_Bgm.Stop();
            isBgm = true;
            Time.timeScale = 0;
        }

        if (playerController.kill_enemy >= 5 && !spawn)
        {
            spawn = true;
            goalNum = Random.Range(0, 5);
            goal[goalNum].SetActive(true);
            spawnGoal.SetActive(true);
        }
    }
}
