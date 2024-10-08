using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TitleButtonScript : MonoBehaviour
{
    public Button Home;
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
            SceneManager.LoadScene("Home");
        if (mode == (int)Mode.GameEnd)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
        }
    }

    //押したボタンの判別用
    public enum Mode
    {
        LoadHome,
        GameEnd,
    }
}
