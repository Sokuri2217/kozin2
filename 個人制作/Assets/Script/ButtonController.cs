using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    int mode;                           //シーン判別
    public Image fadePanel;             // フェード用のUIパネル（Image）
    public float fadeDuration = 1.0f;   // フェードの完了にかかる時間

    private float attention;             //ボタンの拡大率

    public GameObject option_Panel;
    bool isOpen;

    private AudioSource sound;
    public AudioClip onCursor;
    public AudioClip click;

    private void Start()
    {
        mode = 0;
        attention = 1.3f;
        sound = GetComponent<AudioSource>();
        option_Panel.SetActive(false);
        isOpen = false;
    }
    //タイトルに移動
    public void LoadTitle()
    {
        Time.timeScale = 1;

        StartCoroutine(FadeOutAndLoadScene());
        mode = (int)Mode.Title;

    }
    //ステージ1に移動
    public void LoadMain1()
    {
        Time.timeScale = 1;

        StartCoroutine(FadeOutAndLoadScene());
        mode = (int)Mode.Main1;

    }
    //ステージ2に移動
    public void LoadMain2()
    {
        Time.timeScale = 1;

         StartCoroutine(FadeOutAndLoadScene());
        mode = (int)Mode.Main2;

    }
    //オプションを開く
    public void OpenOption()
    {
        Time.timeScale = 1;

        StartCoroutine("MoveDelay", 0.1f);
        mode = (int)Mode.Open;
    }
    //ゲームを再開する
    //public void Check_ExtLuck()
    //{
    //    Time.timeScale = 1;

    //    StartCoroutine("MoveDelay", 0.1f);
    //    mode = (int)Mode.ExtLuck;
    //}
    //ゲーム終了
    public void EndGame()
    {
        StartCoroutine("MoveDelay", 1.0f);
        mode = (int)Mode.GameEnd;

    }
    private IEnumerator MoveDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (mode == (int)Mode.ExtLuck)
        {
            GameManager gameManager;
            GameObject player = GameObject.Find("Player");
            gameManager = player.GetComponent<GameManager>();

            gameManager.open_Option = false;
        }
        //ゲームプレイ終了
        else if (mode == (int)Mode.GameEnd)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }
        else if(mode == (int)Mode.Open)
        {
            if(!isOpen)
            {
                option_Panel.SetActive(true);
                isOpen = true;
            }
            else
            {
                option_Panel.SetActive(false);
                isOpen = false;
            }
        }
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
        if (mode == (int)Mode.Title)
            SceneManager.LoadScene("Title");
        else if (mode == (int)Mode.Main1)
            SceneManager.LoadScene("Main1");
        else if (mode == (int)Mode.Main2)
            SceneManager.LoadScene("Main2");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        sound.PlayOneShot(onCursor);
        gameObject.transform.localScale = Vector3.one * attention;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.localScale = Vector3.one;
    }

    //ボタンをクリックした時
    public void OnClick()
    {
        sound.PlayOneShot(click);
    }

    //押したボタンの判別用
    public enum Mode
    {
        Title = 1,
        Main1,
        Main2,
        ExtLuck,
        GameEnd,
        Open,
    }
    
}
