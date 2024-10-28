using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIScript : MonoBehaviour
{
    //UI�p�l��
    [SerializeField] GameObject menuPanel;
    //�Q�[�����
    public bool open_Option = false;
    //�������h�~
    public bool input = false;

    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(false);
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
                    open_Option = true;
                    break;
                case true:
                    menuPanel.SetActive(false);
                    open_Option = false;
                    break;
            }
            input = true;
        }
        //�{�^���ő���
        if(!open_Option)
        {
            menuPanel.SetActive(false);
        }
            

        if (Input.GetKeyUp(KeyCode.Escape)&&input)
        {
            input = false;
        }
    }

    //�g�p�������A�C�R���Ƃ��ĕ\��
    void Icon()
    {
        PlayerController playerController;
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

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
        //AP2�{
        if (playerController.skill >= 1 && playerController.skill <= 20)
        {

        }
        //HP2�{
        else if (playerController.skill >= 21 && playerController.skill <= 40)
        {
            
        }
        //�U����2�{
        else if (playerController.skill >= 41 && playerController.skill <= 50)
        {
           
        }
        //��_���[�W2�{
        else if (playerController.skill >= 51 && playerController.skill <= 60)
        {
           
        }
        //�ړ�1.5�{�E�U����0.75�{
        else if (playerController.skill >= 61 && playerController.skill <= 70)
        {
            
        }
        //�ړ�0.75�{�E�U����1.5�{
        else if (playerController.skill >= 71 && playerController.skill <= 80)
        {
           
        }
        //����AP�E�U����2�{
        else if (playerController.skill >= 81 && playerController.skill <= 90)
        {
            
        }
        //��_���[�W2�{�E�^�_���[�W0.5�{
        else if (playerController.skill >= 91 && playerController.skill <= 95)
        {
            
        }
        //��_���[�W0.5�{�E�^�_���[�W2�{
        else if (playerController.skill >= 96 && playerController.skill <= 100)
        {
            
        }
    }
}
