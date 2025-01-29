using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int mode;                    //シーン判別
    public Image fadePanel;             // フェード用のUIパネル（Image）
    public float fadeDuration = 1.0f;   // フェードの完了にかかる時間

    private float attention;             //ボタンの拡大率

    private AudioSource sound;
    public AudioClip onCursor;
    public AudioClip click;

    //押したボタンの判別用
    public enum Mode
    {
        GameEnd,
        Open,
        Control,
        Status,
    }
    private void Start()
    {
        mode = 0;
        attention = 1.3f;
        sound = GetComponent<AudioSource>();
    }
    //シーン移動
    public void SceneLoad(string sceneName)
    {
        Time.timeScale = 1;
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }
    //オプションを開く
    public void OpenOption()
    {
        StartCoroutine("MoveDelay", 0.1f);
        mode = (int)Mode.Open;
    }
    //操作説明を開く
    public void OpenControl()
    {
        StartCoroutine("MoveDelay", 0.4f);
        mode = (int)Mode.Control;
    }
    //次のページを開く
    public void NextPage()
    {
        StartCoroutine("MoveDelay", 0.4f);
        mode = (int)Mode.Status;
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

        TitleUIScript titleUI;
        GameObject obj = GameObject.Find("TitleUI");
        titleUI = obj.GetComponent<TitleUIScript>();

        if (mode == (int)Mode.Open)
        {
            if (!titleUI.isOption)
            {
                titleUI.isOption = true;
            }
            else
            {
                titleUI.isOption = false;
            }
        }
        else if (mode == (int)Mode.Control)
        {
            titleUI.isControl = true;
            titleUI.isStatus = false;
        }
        else if (mode == (int)Mode.Status)
        {
            titleUI.isControl = false;
            titleUI.isStatus = true;
        }

        //ゲームプレイ終了
        if (mode == (int)Mode.GameEnd)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }
    }

    public IEnumerator FadeOutAndLoadScene(string scene)
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
        SceneManager.LoadScene(scene);
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
}
