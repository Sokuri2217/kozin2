using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeScript : MonoBehaviour
{
    public AudioSource se;  //SEŠÖ˜A

    //ƒvƒŒƒCƒ„[
    public AudioClip playerDamageSe;      //”í’e
    public AudioClip playerPowerDamageSe; //”íƒ_ƒ[ƒWã¸’†‚Ì”í’e
    public AudioClip itemGet;             //ƒAƒCƒeƒ€Šl“¾

    //“G
    public AudioClip[] enemyDamageSe;      //”í’e
    public AudioClip[] enemyPowerDamageSe; //”íƒ_ƒ[ƒWã¸’†‚Ì”í’e
    //ŒF
    public AudioClip BearDeathSe; //Ž€–S
    //ƒS[ƒŒƒ€
    public AudioClip GolemDeathSe; //Ž€–S

    //UI
    public AudioClip changeConsentSe; //ƒQ[ƒW‚ª‚½‚Ü‚Á‚½‚Æ‚«
    private bool consent;             //‚½‚Ü‚Á‚½‚Æ‚«‚ÌSE‚ª•¡”‰ñ‚È‚ç‚È‚¢‚æ‚¤‚É‚·‚é
    public AudioClip changeSe;        //ƒQ[ƒW‚ðÁ”ï‚µ‚½‚Æ‚«
    private bool use;                 //Á”ï‚µ‚½‚Æ‚«‚ÌSE‚ª•¡”‰ñ‚È‚ç‚È‚¢‚æ‚¤‚É‚·‚é

    //ƒXƒNƒŠƒvƒgŽæ“¾
    public PlayerController playerController;
    public EnemyBear enemyBear;
    public GolemController golemController;
    public GameManager gameManager;
    public MainUIScript mainUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //“G‚ÉUŒ‚‚ª“–‚½‚Á‚½‚Æ‚«‚ÉASE‚ð–Â‚ç‚·
        if (enemyBear.isDamage || golemController.isDamage) 
        {
            //•Ší‚²‚Æ‚É”í’eSE‚ð–Â‚ç‚·
            if (playerController.attack > playerController.firstAttack)
            {
                //ƒvƒŒƒCƒ„[‚ÌUŒ‚—Í‚ªã¸‚µ‚Ä‚¢‚é‚Æ‚«
                se.PlayOneShot(enemyPowerDamageSe[playerController.weapon]);
            }
            else
            {
                //ƒvƒŒƒCƒ„[‚ÌUŒ‚—Í‚ª•Ï‰»‚µ‚Ä‚¢‚È‚¢‚Æ‚«
                se.PlayOneShot(enemyDamageSe[playerController.weapon]);
            }
        }

        if(playerController.isDamage)
        {
            //ƒ_ƒ[ƒWSE‚ð–Â‚ç‚·
            if (playerController.attack > playerController.firstAttack)
            {
                se.PlayOneShot(playerPowerDamageSe);
            }
            else
            {
                se.PlayOneShot(playerDamageSe);
            }
        }

        if(playerController.item)
        {
            //Žæ“¾‰¹‚ð–Â‚ç‚·
            se.PlayOneShot(itemGet);
            playerController.item = false;
        }

        if (mainUI.changeConsent)
        {
            if(!consent)
            {
                se.PlayOneShot(changeConsentSe);
                consent = true;
            }
        }
    }
}
