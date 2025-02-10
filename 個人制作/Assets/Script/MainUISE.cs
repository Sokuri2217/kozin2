using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUISE : SeScript
{
    //�X�N���v�g�擾
    public MainUIScript mainUI;

    //���ʉ�
    public AudioClip consent; //�X�e�[�^�X�ύX�\
    public AudioClip change;  //�X�e�[�^�X�ύX

    //��x�����炷���߂̃t���O
    public bool isConsent;
    public bool isChange; 

    // Start is called before the first frame update
    new void Start()
    {
        isChange = false;
        isConsent = false;
    }

    // Update is called once per frame
    void Update()
    {
        //�X�e�[�^�X�ύX�\
        if(mainUI.changeConsent)
        {
            if(!isConsent)
            {
                se.PlayOneShot(consent);
                isConsent = true;
            }
        }
        else
        {
            isConsent = false;
        }

        //�X�e�[�^�X�ύX
        if(mainUI.changeInput)
        {
            if(!isChange)
            {
                se.PlayOneShot(change);
                isChange = true;
            }
        }
        else
        {
            isChange = false;
        }
    }
}
