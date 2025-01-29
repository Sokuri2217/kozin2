using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int mode;                    //�V�[������
    public Image fadePanel;             // �t�F�[�h�p��UI�p�l���iImage�j
    public float fadeDuration = 1.0f;   // �t�F�[�h�̊����ɂ����鎞��

    private float attention;             //�{�^���̊g�嗦

    private AudioSource sound;
    public AudioClip onCursor;
    public AudioClip click;

    //�������{�^���̔��ʗp
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
    //�V�[���ړ�
    public void SceneLoad(string sceneName)
    {
        Time.timeScale = 1;
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }
    //�I�v�V�������J��
    public void OpenOption()
    {
        StartCoroutine("MoveDelay", 0.1f);
        mode = (int)Mode.Open;
    }
    //����������J��
    public void OpenControl()
    {
        StartCoroutine("MoveDelay", 0.4f);
        mode = (int)Mode.Control;
    }
    //���̃y�[�W���J��
    public void NextPage()
    {
        StartCoroutine("MoveDelay", 0.4f);
        mode = (int)Mode.Status;
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

        //�Q�[���v���C�I��
        if (mode == (int)Mode.GameEnd)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
        }
    }

    public IEnumerator FadeOutAndLoadScene(string scene)
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

    //�{�^�����N���b�N������
    public void OnClick()
    {
        sound.PlayOneShot(click);
    }
}
