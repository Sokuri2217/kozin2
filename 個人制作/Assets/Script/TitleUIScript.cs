using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUIScript : MonoBehaviour
{
    //���������ʂɈڍs����{�^���̉摜
    public Image option;
    public Sprite openOption;
    public Sprite closeOption;

    //�e�m�F��ʂ̃p�l��
    public GameObject option1Panel;
    public GameObject option2Panel;

    //�\���t���O
    public bool isOption;
    public bool isControl;
    public bool isStatus;

    // Start is called before the first frame update
    void Start()
    {
        //������
        option1Panel.SetActive(false);
        option2Panel.SetActive(false);
        option.sprite = openOption;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOption)
        {
            //�{�^���̉摜�ύX
            option.sprite = closeOption;

            //1�y�[�W�ڂ�\��
            if (!isControl && !isStatus) 
            {
                isControl = true;
            }
            //1�y�[�W�ڂ��\��
            else if(isStatus)
            {
                isControl = false;
            }
        }
        else
        {
            //�摜�����ɖ߂�
            option.sprite = openOption;

            //�S�y�[�W���\��
            isControl = false;
            isStatus = false;
        }

        if (isControl)
        {
            //1�y�[�W�ڂ�\��
            option1Panel.SetActive(true);
        }
        else
        {
            //1�y�[�W�ڂ��\��
            option1Panel.SetActive(false);
        }

        if (isStatus)
        {
            //2�y�[�W�ڂ�\��
            option2Panel.SetActive(true);
        }
        else
        {
            //2�y�[�W�ڂ��\��
            option2Panel.SetActive(false);
        }
    }
}
