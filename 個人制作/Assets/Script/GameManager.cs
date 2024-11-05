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
    public GameObject goal;

    // Start is called before the first frame update
    void Start()
    {
        gamePlay = true;
        gameOver = false;
        gameClear = false;
        clearPanel.SetActive(false);
        overPanel.SetActive(false);
        goal.SetActive(false);
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
        }

        if (gameClear)
        {
            clearPanel.SetActive(true);
        }

        if (playerController.kill_enemy >= 5)
        {
            goal.SetActive(true);
        }
    }
}
