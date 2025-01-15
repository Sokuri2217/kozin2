using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    int mode;                           //�V�[������
    public Image fadePanel;             // �t�F�[�h�p��UI�p�l���iImage�j
    public float fadeDuration = 1.0f;   // �t�F�[�h�̊����ɂ����鎞��

    private float attention;             //�{�^���̊g�嗦

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
    //�^�C�g���Ɉړ�
    public void LoadTitle()
    {
        Time.timeScale = 1;

        StartCoroutine(FadeOutAndLoadScene());
        mode = (int)Mode.Title;

    }
    //�X�e�[�W1�Ɉړ�
    public void LoadMain1()
    {
        Time.timeScale = 1;

        StartCoroutine(FadeOutAndLoadScene());
        mode = (int)Mode.Main1;

    }
    //�X�e�[�W2�Ɉړ�
    public void LoadMain2()
    {
        Time.timeScale = 1;

         StartCoroutine(FadeOutAndLoadScene());
        mode = (int)Mode.Main2;

    }
    //�I�v�V�������J��
    public void OpenOption()
    {
        Time.timeScale = 1;

        StartCoroutine("MoveDelay", 0.1f);
        mode = (int)Mode.Open;
    }
    //�Q�[�����ĊJ����
    //public void Check_ExtLuck()
    //{
    //    Time.timeScale = 1;

    //    StartCoroutine("MoveDelay", 0.1f);
    //    mode = (int)Mode.ExtLuck;
    //}
    //�Q�[���I��
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
        //�Q�[���v���C�I��
        else if (mode == (int)Mode.GameEnd)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();//�Q�[���v���C�I��
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
        fadePanel.enabled = true;                 // �p�l����L����
        float elapsedTime = 0.0f;                 // �o�ߎ��Ԃ�������
        Color startColor = fadePanel.color;       // �t�F�[�h�p�l���̊J�n�F���擾
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // �t�F�[�h�p�l���̍ŏI�F��ݒ�

        // �t�F�[�h�A�E�g�A�j���[�V���������s
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // �o�ߎ��Ԃ𑝂₷
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // �t�F�[�h�̐i�s�x���v�Z
            fadePanel.color = Color.Lerp(startColor, endColor, t); // �p�l���̐F��ύX���ăt�F�[�h�A�E�g
            yield return null;                                     // 1�t���[���ҋ@
        }

        fadePanel.color = endColor;  // �t�F�[�h������������ŏI�F�ɐݒ�
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

    //�{�^�����N���b�N������
    public void OnClick()
    {
        sound.PlayOneShot(click);
    }

    //�������{�^���̔��ʗp
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
