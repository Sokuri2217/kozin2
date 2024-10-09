using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TitleButtonScript : MonoBehaviour
{
    public Button home;
    public Button endgame;
    int mode = 0;

    public void LoadScene()
    {
        StartCoroutine("MoveDelay", 1.0f);
        mode = (int)Mode.LoadHome;
        
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

        if(mode==(int)Mode.LoadHome)
            SceneManager.LoadScene("home");
        if (mode == (int)Mode.GameEnd)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }
    }

    public void Click()
    {
        //ボタンを押せないようにする
        GetComponent<Button>().interactable = false;
    }

    //押したボタンの判別用
    public enum Mode
    {
        LoadHome,
        GameEnd,
    }
}
