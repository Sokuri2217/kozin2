using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIScript : MonoBehaviour
{
    //UI
    public GameObject menuPanel;
    public new GameObject camera;
    public GameObject killCounter;
    //武器アイコン
    public Image weapon_icon;
    public Sprite[] wepon;
    public Sprite[] wepon_interval;
    //スキルアイコン
    public  Image skill_icon;
    public Sprite[] skill;

    public bool input = false;

    //ゴール条件
    public Text currentKill;
    public Text goalSpawnKill;

    //public Image fadePanel;             // フェード用のUIパネル（Image）
    //public float fadeDuration = 1.0f;   // フェードの完了にかかる時間

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(FadeOutAndLoadScene());

        menuPanel.SetActive(false);
        camera.SetActive(true);
        killCounter.SetActive(true);

        weapon_icon = GetComponent<Image>();
        skill_icon = GetComponent<Image>();

        //初回表示
        Icon();
    }

    // Update is called once per frame
    void Update()
    {
        GameManager gameManager;
        PlayerController playerController;
        GameObject obj = GameObject.Find("Player");
        gameManager = obj.GetComponent<GameManager>();
        playerController = obj.GetComponent<PlayerController>();

        //オプション管理
        //ボタンで操作
        if(!gameManager.gameClear && !gameManager.gameOver)
        {
            if (gameManager.open_Option)
            {
                // カーソルを自由に動かせる
                Cursor.lockState = CursorLockMode.None;
                menuPanel.SetActive(true);
                camera.SetActive(false);
            }
            else
            {
                // カーソルを画面中央にロックする
                Cursor.lockState = CursorLockMode.Locked;
                menuPanel.SetActive(false);
                camera.SetActive(true);
            }
        }

        if (gameManager.gameClear || gameManager.gameOver) 
        {
            camera.SetActive(false);
        }
        //KILLカウントを表示させる
        currentKill.text = playerController.kill_enemy.ToString();
        goalSpawnKill.text = playerController.goalspawn.ToString();

        //ゴール出現時にKILLカウントを非表示にする
        if (playerController.kill_enemy >= 5)
            killCounter.SetActive(false);

        //インターバル中のアイコン制御
        Icon();
    }

    //使用装備をアイコンとして表示
    void Icon()
    {
        PlayerController playerController;
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        if (playerController.apLost) 
        {
            //武器
            switch (playerController.weapon)
            {
                case (int)Weapon.KNIFE:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon_interval[(int)Weapon.KNIFE];
                    break;
                case (int)Weapon.SWORD:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon_interval[(int)Weapon.SWORD];
                    break;
                case (int)Weapon.KNUCKLE:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon_interval[(int)Weapon.KNUCKLE];
                    break;
            }
        }
        else
        {
            //武器
            switch (playerController.weapon)
            {
                case (int)Weapon.KNIFE:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon[(int)Weapon.KNIFE];
                    break;
                case (int)Weapon.SWORD:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon[(int)Weapon.SWORD];
                    break;
                case (int)Weapon.KNUCKLE:
                    GameObject.Find("Weapon").GetComponent<Image>().sprite = wepon[(int)Weapon.KNUCKLE];
                    break;
            }
        }
       
        //付与効果
        //AP2倍
        if (playerController.skill >= 1 && playerController.skill <= 20)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[0];
        }
        //HP2倍
        else if (playerController.skill >= 21 && playerController.skill <= 40)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[1];
        }
        //攻撃力2倍
        else if (playerController.skill >= 41 && playerController.skill <= 50)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[2];
        }
        //被ダメージ2倍
        else if (playerController.skill >= 51 && playerController.skill <= 60)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[3];
        }
        //移動1.5倍・攻撃力0.75倍
        else if (playerController.skill >= 61 && playerController.skill <= 70)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[4];
        }
        //移動0.75倍・攻撃力1.5倍
        else if (playerController.skill >= 71 && playerController.skill <= 80)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[5];
        }
        //消費AP・攻撃力2倍
        else if (playerController.skill >= 81 && playerController.skill <= 90)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[6];
        }
        //被ダメージ2倍・与ダメージ0.5倍
        else if (playerController.skill >= 91 && playerController.skill <= 95)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[7];
        }
        //被ダメージ0.5倍・与ダメージ2倍 0
        else if (playerController.skill >= 96 && playerController.skill <= 100)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[8];
        }
    }

    public enum Weapon
    {
        KNIFE,
        SWORD,
        KNUCKLE
    }

    //public IEnumerator FadeOutAndLoadScene()
    //{
    //    fadePanel.enabled = true;                 // パネルを有効化
    //    float elapsedTime = 0.0f;                 // 経過時間を初期化
    //    Color startColor = fadePanel.color;       // フェードパネルの開始色を取得
    //    Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // フェードパネルの最終色を設定

    //    // フェードアウトアニメーションを実行
    //    while (elapsedTime < fadeDuration)
    //    {
    //        elapsedTime += Time.deltaTime;                        // 経過時間を増やす
    //        float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // フェードの進行度を計算
    //        fadePanel.color = Color.Lerp(startColor, endColor, t); // パネルの色を変更してフェードアウト
    //        yield return null;                                     // 1フレーム待機
    //    }

    //    fadePanel.color = endColor;  // フェードが完了したら最終色に設定
    //}
}
