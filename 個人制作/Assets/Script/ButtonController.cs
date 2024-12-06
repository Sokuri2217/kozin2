using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    int mode = 0;

    //�^�C�g���Ɉړ�
    public void LoadTitle()
    {
        Time.timeScale = 1;

        StartCoroutine("MoveDelay", 0.5f);
        mode = (int)Mode.Title;

    }
    //�X�e�[�W1�Ɉړ�
    public void LoadMain1()
    {
        Time.timeScale = 1;

        StartCoroutine("MoveDelay", 0.5f);
        mode = (int)Mode.Main1;

    }
    //�Q�[�����ĊJ����
    public void BackGame()
    {
        Time.timeScale = 1;

        StartCoroutine("MoveDelay", 0.1f);
        mode = (int)Mode.Back;
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

        if (mode == (int)Mode.Title)
            SceneManager.LoadScene("title");
        if (mode == (int)Mode.Main1)
            SceneManager.LoadScene("main1");
        if (mode == (int)Mode.Back)
        {
            MainUIScript mainUIScript;
            GameObject obj = GameObject.Find("MainUI");
            mainUIScript = obj.GetComponent<MainUIScript>();

            mainUIScript.open_Option = false;
        }
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
        Title,
        Main1,
        Back,
        GameEnd
    }
}
