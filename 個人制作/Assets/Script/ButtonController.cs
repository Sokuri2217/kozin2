using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    int mode;

    private void Start()
    {
        mode = 0;
    }
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
    //�X�e�[�W2�Ɉړ�
    public void LoadMain2()
    {
        Time.timeScale = 1;

        StartCoroutine("MoveDelay", 0.5f);
        mode = (int)Mode.Main2;

    }
    //�X�e�[�W3�Ɉړ�
    public void LoadMain3()
    {
        Time.timeScale = 1;

        StartCoroutine("MoveDelay", 0.5f);
        mode = (int)Mode.Main3;

    }
    //�X�e�[�W4�Ɉړ�
    public void LoadMain4()
    {
        Time.timeScale = 1;

        StartCoroutine("MoveDelay", 0.5f);
        mode = (int)Mode.Main4;

    }
    //�X�e�[�W5�Ɉړ�
    public void LoadMain5()
    {
        Time.timeScale = 1;

        StartCoroutine("MoveDelay", 0.5f);
        mode = (int)Mode.Main5;

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
            GameManager gameManager;
            GameObject player = GameObject.Find("Player");
            gameManager = player.GetComponent<GameManager>();

            gameManager.open_Option = false;
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

    void Update()
    {
        
    }

    //�������{�^���̔��ʗp
    public enum Mode
    {
        Title,
        Main1,
        Main2,
        Main3,
        Main4,
        Main5,
        Back,
        GameEnd
    }
}
