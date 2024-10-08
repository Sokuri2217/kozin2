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

    //�Q�[���I��
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
            UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
        }
    }

    //�������{�^���̔��ʗp
    public enum Mode
    {
        LoadHome,
        GameEnd,
    }
}
