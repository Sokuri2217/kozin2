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
    private bool spawnDAYO = false;
    private bool uyoku = false;

    // Start is called before the first frame update
    void Start()
    {
        gamePlay = true;
        gameOver = false;
        gameClear = false;
        clearPanel.SetActive(false);
        overPanel.SetActive(false);
        spawnGoal.SetActive(false);
        for(int i=0;i<5;i++)
        {
            goal[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController playerController = GetComponent<PlayerController>();

        if (playerController.currentHp <= 0.0f)
        {
            gameOver = true;
            gamePlay = false;
            overPanel.SetActive(true);
            spawnGoal.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }

        if (gameClear)
        {
            gameClear = true;
            gamePlay = false;
            clearPanel.SetActive(true);
            spawnGoal.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
        }

        if (playerController.kill_enemy >= 5 && !spawnDAYO) 
        {
            spawnDAYO = true;
            goalNum = Random.Range(0, 5);
            goal[goalNum].SetActive(true);
            spawnGoal.SetActive(true);
        }
        if (Input.GetMouseButton(1) && !uyoku)  
        {
           uyoku = true;
            goalNum = Random.Range(0, 5);
            goal[goalNum].SetActive(true);
            spawnGoal.SetActive(true);
        }
    }
}
