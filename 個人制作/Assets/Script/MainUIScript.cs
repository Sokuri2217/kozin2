using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIScript : MonoBehaviour
{
    //UI�p�l��
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject camera;
    //�Q�[�����
    public bool open_Option = false;
    //�������h�~
    public bool input = false;

    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(false);
        camera.SetActive(true);
        open_Option = false;
    }

    // Update is called once per frame
    void Update()
    {
        //�I�v�V�����Ǘ�
        //Esc�ő���
        if (Input.GetKeyDown(KeyCode.Escape) && !input) 
        {
            switch (open_Option)
            {
                case false:
                    menuPanel.SetActive(true);
                    camera.SetActive(false);
                    open_Option = true;
                    break;
                case true:
                    menuPanel.SetActive(false);
                    camera.SetActive(true);
                    open_Option = false;
                    break;
            }
            input = true;
        }
        //�{�^���ő���
        if(!open_Option)
        {
            menuPanel.SetActive(false);
            camera.SetActive(true);
        }
            

        if (Input.GetKeyUp(KeyCode.Escape)&&input)
        {
            input = false;
        }
    }

    //�g�p�������A�C�R���Ƃ��ĕ\��
    void Icon()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        //����
        switch (playerController.weapon)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
        //�t�^����
        switch(playerController.skill)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            default:
                break;
        }
    }

    //�̗́EAP���Q�[�W�Ƃ��ĕ\��
    void Gauge()
    {

    }
}
