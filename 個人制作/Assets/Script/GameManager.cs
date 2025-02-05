using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool gameOver;
    public bool gameClear;
    public bool gamePlay;

    public GameObject clearPanel;
    public GameObject overPanel;
    public GameObject spawnGoal;
    public GameObject[] goal;
    public GameObject spawnArea;
    private int goalNum = 0;
    public bool spawn = false;
    public int maxCount;
    public int currentCount;
    public int killEnemy;             //倒した敵数
    public int goalSpawn;             //ゴール出現に必要な敵数
    public bool isDamageSe;            //敵の被弾seがなっている最中かどうか
    public bool isDeathSe;            //敵の死亡seがなっている最中かどうか

    //ボス関連
    //public bool spawnBoss;    //ボス出現フラグ
    //public GameObject boss;   //オブジェクト
    //public GameObject bossHp; //HPバー

    //ゲーム状態
    public bool open_Option;
    //長押し防止
    public bool input;

    public Image fadePanel;             // フェード用のUIパネル（Image）
    public float fadeDuration = 1.0f;   // フェードの完了にかかる時間

    // Start is called before the first frame update
    void Start()
    {
        killEnemy = 0;
        gameOver = false;
        gameClear = false;
        gamePlay = true;
        clearPanel.SetActive(false);
        overPanel.SetActive(false);
        spawnGoal.SetActive(false);
        spawnArea.SetActive(false);
        //boss.SetActive(false);
        open_Option = false;
        input = false;
        isDamageSe = false;
        isDeathSe = false;
        //spawnBoss = false;
        //bossHp.SetActive(false);
        

        // カーソルを画面中央にロックする
        Cursor.lockState = CursorLockMode.Locked;

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
                        open_Option = true;
                        playerController.isStop = true;
                        // カーソルを自由に動かせる
                        Cursor.lockState = CursorLockMode.None;
                        Time.timeScale = 0;
                        break;
                    case true:
                        Time.timeScale = 1;
                        playerController.isStop = false;
                        // カーソルを画面中央にロックする
                        Cursor.lockState = CursorLockMode.Locked;
                        open_Option = false;
                        break;
                }
                input = true;
            }
        }
        //デバッグ用
        {
            //if (Input.GetKeyDown(KeyCode.E))
            //{
            //    currentCount = maxCount;
            //}

            ////ボス出現
            //if (currentCount >= maxCount)
            //{
            //    currentCount = maxCount;
            //    if(!spawnBoss)
            //    {
            //        Time.timeScale = 1;
            //        spawnBoss = true;
            //        StartCoroutine(FadeOutAndLoadScene());
            //    }
            //}
        }

        //長押し防止
        if (Input.GetKeyUp(KeyCode.Escape) && input)
        {
            input = false;
        }
        //キルカウントの制御
        if (killEnemy >= goalSpawn) 
        {
            killEnemy = goalSpawn;
            //条件を満たすとゴールを出す
            if (!spawn)
            {
                spawn = true;
                goalNum = Random.Range(0, 5);
                goal[goalNum].SetActive(true);
                spawnGoal.SetActive(true);
                spawnArea.SetActive(true);
            }
        }

        //ゲームオーバー
        if (gameOver && gamePlay)   
        {
            gamePlay = false;
            Invoke("Over", 3.5f);
        }
        //ゲームクリア
        if (gameClear && gamePlay)  
        {
            gamePlay = false;
            Invoke("Clear", 0.3f);
        }
        //クリアかゲームオーバーになるとspawnGoalを非表示にする
        if(gameClear||gameOver)
        {
            spawnGoal.SetActive(false);
        }
    }

    void Over()
    {
        overPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
    void Clear()
    {
        clearPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public IEnumerator FadeOutAndLoadScene()
    {
        fadePanel.enabled = true;                 // パネルを有効化
        float elapsedTime = 0.0f;                 // 経過時間を初期化
        Color startColor = fadePanel.color;       // フェードパネルの開始色を取得
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // フェードパネルの最終色を設定

        // フェードアウトアニメーションを実行
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // 経過時間を増やす
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // フェードの進行度を計算
            fadePanel.color = Color.Lerp(startColor, endColor, t); // パネルの色を変更してフェードアウト
            yield return null;                                     // 1フレーム待機
        }

        fadePanel.color = endColor;  // フェードが完了したら最終色に設定
        fadePanel.enabled = false;
        //boss.SetActive(true);
        //bossHp.SetActive(true);
        Time.timeScale = 1;
    }
}
