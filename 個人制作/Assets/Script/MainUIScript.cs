using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIScript : MonoBehaviour
{
    //UIƒpƒlƒ‹
    [SerializeField] GameObject menuPanel;
    //ƒQ[ƒ€ó‘Ô
    public bool open_Option = false;
    //’·‰Ÿ‚µ–h~
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
        //ƒIƒvƒVƒ‡ƒ“ŠÇ—
        //Esc‚Å‘€ì
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
        //ƒ{ƒ^ƒ“‚Å‘€ì
        if(!open_Option)
        {
            menuPanel.SetActive(false);
        }
            

        if (Input.GetKeyUp(KeyCode.Escape)&&input)
        {
            input = false;
        }
    }

    //g—p‘•”õ‚ğƒAƒCƒRƒ“‚Æ‚µ‚Ä•\¦
    void Icon()
    {
        PlayerController playerController;
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        //•Ší
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
        //•t—^Œø‰Ê
        //AP2”{
        if (playerController.skill >= 1 && playerController.skill <= 20)
        {

        }
        //HP2”{
        else if (playerController.skill >= 21 && playerController.skill <= 40)
        {
            
        }
        //UŒ‚—Í2”{
        else if (playerController.skill >= 41 && playerController.skill <= 50)
        {
           
        }
        //”íƒ_ƒ[ƒW2”{
        else if (playerController.skill >= 51 && playerController.skill <= 60)
        {
           
        }
        //ˆÚ“®1.5”{EUŒ‚—Í0.75”{
        else if (playerController.skill >= 61 && playerController.skill <= 70)
        {
            
        }
        //ˆÚ“®0.75”{EUŒ‚—Í1.5”{
        else if (playerController.skill >= 71 && playerController.skill <= 80)
        {
           
        }
        //Á”ïAPEUŒ‚—Í2”{
        else if (playerController.skill >= 81 && playerController.skill <= 90)
        {
            
        }
        //”íƒ_ƒ[ƒW2”{E—^ƒ_ƒ[ƒW0.5”{
        else if (playerController.skill >= 91 && playerController.skill <= 95)
        {
            
        }
        //”íƒ_ƒ[ƒW0.5”{E—^ƒ_ƒ[ƒW2”{
        else if (playerController.skill >= 96 && playerController.skill <= 100)
        {
            
        }
    }
}
