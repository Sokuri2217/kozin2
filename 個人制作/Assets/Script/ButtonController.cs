using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    int mode;

    private void Start()
    {
        mode = 0;
    }
    //タイトルに移動
    public void LoadTitle()
    {
        Time.timeScale = 1;

        StartCoroutine("MoveDelay", 0.5f);
        mode = (int)Mode.Title;

    }
    //ステージ1に移動
    public void LoadMain1()
    {
        Time.timeScale = 1;

        StartCoroutine("MoveDelay", 0.5f);
        mode = (int)Mode.Main1;

    }
    //ステージ2に移動
    public void LoadMain2()
    {
        Time.timeScale = 1;

        StartCoroutine("MoveDelay", 0.5f);
        mode = (int)Mode.Main2;

    }
    //ステージ3に移動
    public void LoadMain3()
    {
        Time.timeScale = 1;

        StartCoroutine("MoveDelay", 0.5f);
        mode = (int)Mode.Main3;

    }
    //ステージ4に移動
    public void LoadMain4()
    {
        Time.timeScale = 1;

        StartCoroutine("MoveDelay", 0.5f);
        mode = (int)Mode.Main4;

    }
    //ステージ5に移動
    public void LoadMain5()
    {
        Time.timeScale = 1;

        StartCoroutine("MoveDelay", 0.5f);
        mode = (int)Mode.Main5;

    }
    //ゲームを再開する
    public void BackGame()
    {
        Time.timeScale = 1;

        StartCoroutine("MoveDelay", 0.1f);
        mode = (int)Mode.Back;
    }
    //ゲーム終了
    public void EndGame()
    {
        StartCoroutine("MoveDelay", 1.0f);
        mode = (int)Mode.GameEnd;

    }
    private IEnumerator MoveDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (mode == (int)Mode.Title)
            SceneManager.LoadScene("title");
        if (mode == (int)Mode.Main1)
            SceneManager.LoadScene("main1");
        if (mode == (int)Mode.Back)
        {
            GameManager gameManager;
            GameObject player = GameObject.Find("Player");
            gameManager = player.GetComponent<GameManager>();

            gameManager.open_Option = false;
        }
        if (mode == (int)Mode.GameEnd)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }
    }

    void Update()
    {
        
    }

    //押したボタンの判別用
    public enum Mode
    {
        Title,
        Main1,
        Main2,
        Main3,
        Main4,
        Main5,
        Back,
        GameEnd
    }
}
