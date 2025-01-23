using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIScript : MonoBehaviour
{
    // UI要素
    public GameObject menuPanel, killCounter, pause, reunion, esc_icon;
    public new GameObject camera;
    public Image weapon_icon, skill_icon;
    public Text currentKill, goalSpawnKill;

    // 武器とスキルのアイコン
    public Sprite[] weapon, weapon_interval, skill;

    // ゲームの状態を管理
    private GameManager gameManager;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        // 初期設定
        menuPanel.SetActive(false);
        reunion.SetActive(false);
        weapon_icon = GetComponent<Image>();
        skill_icon = GetComponent<Image>();

        UpdateSkillIcon(); // アイコンを更新
    }

    // Update is called once per frame
    void Update()
    {
        // ゲームマネージャーとプレイヤーコントローラーを取得
        gameManager = GameObject.Find("Player").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        // ポーズやメニューの切り替え
        if (!gameManager.gameClear && !gameManager.gameOver)
        {
            HandleMenu(); // メニュー表示の処理
        }
        else
        {
            camera.SetActive(false);
            esc_icon.SetActive(false);
        }

        // KILLカウントとゴールの条件を更新
        UpdateKillCount();

        // ゴール条件達成でKILLカウントを非表示に
        if (playerController.killEnemy >= 5) killCounter.SetActive(false);

        //インターバル中のアイコン制御
        UpdateWeaponIcon();
    }

    //使用装備をアイコンとして表示
    void UpdateWeaponIcon()
    {
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        // プレイヤーの武器の状態に応じてアイコンを変更
        if (playerController.apLost)
        {
            // APが足りない場合、インターバル用の武器アイコンを表示
            GameObject.Find("Weapon").GetComponent<Image>().sprite = weapon_interval[playerController.weapon];
        }
        else
        {
            // 通常の武器アイコンを表示
            GameObject.Find("Weapon").GetComponent<Image>().sprite = weapon[playerController.weapon];
        }
    }

    // メニューの表示と非表示を切り替える処理
    private void HandleMenu()
    {
        if (gameManager.open_Option)
        {
            // オプションメニューを表示
            Cursor.lockState = CursorLockMode.None; // カーソルを自由に動かせるようにする
            menuPanel.SetActive(true);
            pause.SetActive(false);
            reunion.SetActive(true);
            camera.SetActive(false);
        }
        else
        {
            // 通常ゲーム画面に戻る
            Cursor.lockState = CursorLockMode.Locked; // カーソルを画面中央にロック
            menuPanel.SetActive(false);
            reunion.SetActive(false);
            pause.SetActive(true);
            camera.SetActive(true);
        }
    }

    // プレイヤーのスキルに応じてスキルアイコンを更新
    void UpdateSkillIcon()
    {
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        //AP2倍
        if (playerController.skill <= 20)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[0];
        }
        //HP2倍
        else if (playerController.skill <= 40)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[1];
        }
        //攻撃力2倍
        else if (playerController.skill <= 50)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[2];
        }
        //被ダメージ2倍
        else if (playerController.skill <= 60)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[3];
        }
        //移動1.5倍・攻撃力0.75倍
        else if (playerController.skill <= 70)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[4];
        }
        //移動0.75倍・攻撃力1.5倍
        else if (playerController.skill <= 80)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[5];
        }
        //消費AP・攻撃力2倍
        else if (playerController.skill <= 90)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[6];
        }
        //被ダメージ2倍・与ダメージ0.5倍
        else if (playerController.skill <= 95)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[7];
        }
        //被ダメージ0.5倍・与ダメージ2倍 0
        else if (playerController.skill <= 100)
        {
            GameObject.Find("Skill").GetComponent<Image>().sprite = skill[8];
        }
    }

    // KILLカウントを更新する処理
    private void UpdateKillCount()
    {
        currentKill.text = playerController.killEnemy.ToString(); // 現在のKILL数
        goalSpawnKill.text = playerController.goalSpawn.ToString(); // ゴール出現に必要なKILL数
    }

    public enum Weapon
    {
        KNIFE,
        SWORD,
        KNUCKLE
    }
}
