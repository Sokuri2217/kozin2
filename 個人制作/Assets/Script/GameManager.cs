using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //ゲームの状態
    public bool gameOver;
    public bool gameClear;
    public bool gamePlay;

    public GameObject clearPanel; //クリアパネル
    public GameObject overPanel; //オーバーパネル
    public GameObject spawnGoal; //ゴール出現
    public GameObject defenceDown; //防御ダウン
    public GameObject[] goal; //ゴールオブジェクト
    private int goalNum = 0; //ゴールの抽選用
    public bool spawn = false; //ゴール生成フラグ
    public int killEnemy; //倒した敵数
    public int goalSpawn; //ゴール出現に必要な敵数
    public bool isDamageSe; //敵の被弾seがなっている最中かどうか
    public bool isDeathSe; //敵の死亡seがなっている最中かどうか

    //ボス関連
    //public bool spawnBoss;    //ボス出現フラグ
    //public GameObject boss;   //オブジェクト
    //public GameObject bossHp; //HPバー

    //ゲーム状態
    public bool openOption;
    //長押し防止
    public bool input;

    public Image fadePanel;             // フェード用のUIパネル（Image）
    public float fadeDuration = 1.0f;   // フェードの完了にかかる時間

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        killEnemy = 0;
        gameOver = false;
        gameClear = false;
        gamePlay = true;
        clearPanel.SetActive(false);
        overPanel.SetActive(false);
        spawnGoal.SetActive(false);
        defenceDown.SetActive(false);
        //boss.SetActive(false);
        openOption = false;
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
                switch (openOption)
                {
                    case false:
                        openOption = true;
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
                        openOption = false;
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
        //キルカウントの制御
        if (killEnemy >= goalSpawn) 
        {
            killEnemy = goalSpawn;
            //条件を満たすとゴールを出す
            if (!spawn)
            {
                spawn = true;
                playerController.damage *= 3;
                playerController.firstDamage *= 3;
                goalNum = Random.Range(0, 5);
                goal[goalNum].SetActive(true);
                spawnGoal.SetActive(true);
                defenceDown.SetActive(true);
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
