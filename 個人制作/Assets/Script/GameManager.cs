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

    //ÉQÅ[ÉÄèÛë‘
    public bool open_Option;
    //í∑âüÇµñhé~
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

        for (int i = 0; i < 5; i++)
            goal[i].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController playerController = GetComponent<PlayerController>();

        //åàíÖÇ™Ç¬Ç≠Ç∆ìÆÇ©Ç»Ç¢ÇÊÇ§Ç…Ç∑ÇÈ
        if (!gameClear && !gameOver) 
        {
            //EscÇ≈àÍéûí‚é~Ç≥ÇπÇÈ
            if (Input.GetKeyDown(KeyCode.Escape) && !input)
            {
                switch (open_Option)
                {
                    case false:
                        open_Option = true;
                        gamePlay = false;
                        Time.timeScale = 0;
                        break;
                    case true:
                        Time.timeScale = 1;
                        open_Option = false;
                        gamePlay = true;
                        break;
                }
                input = true;
            }
        }

        //í∑âüÇµñhé~
        if (Input.GetKeyUp(KeyCode.Escape) && input)
        {
            input = false;
        }
        //ÉQÅ[ÉÄÉIÅ[ÉoÅ[
        if (playerController.currentHp <= 0 && !isBgm)  
        {
            gamePlay = false;
            result_Bgm.PlayOneShot(over_Bgm);
            main_Bgm.Stop();
            isBgm = true;
            Invoke("Death", 3.0f);
        }
        //ÉQÅ[ÉÄÉNÉäÉA
        if (gameClear && !isBgm) 
        {
            gamePlay = false;
            result_Bgm.PlayOneShot(clear_Bgm);
            main_Bgm.Stop();
            isBgm = true;
            Invoke("Clear", 1.0f);

        }
        //èåèÇñûÇΩÇ∑Ç∆ÉSÅ[ÉãÇèoÇ∑
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
        gameOver = true;
        overPanel.SetActive(true);
        Time.timeScale = 0;
    }

    void Clear()
    {
        gameClear = true;
        clearPanel.SetActive(true);
        Time.timeScale = 0;
    }
}
