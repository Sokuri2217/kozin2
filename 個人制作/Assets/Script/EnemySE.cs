using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySE : SeScript
{
    //ƒXƒNƒŠƒvƒgæ“¾
    public EnemyBear bear;
    public EnemySkeleton skeleton;
    public GolemController golem;

    //”í’e
    public AudioClip[] damage;
    public AudioClip[] powerDamage;
    public AudioClip[] downDamage;

    //€–S
    public AudioClip bearDeath;
    public bool isBearDeath;
    public AudioClip golemDeath;
    public bool isGolemDeath;

    // Start is called before the first frame update
    new void Start()
    {
        isHit = false;
        isBearDeath = false;
        isGolemDeath = false;
    }

    // Update is called once per frame
    void Update()
    {
        //“G‚ª”í’e‚µ‚½‚Æ‚«
        //€–S”»’è‚É‚È‚Á‚Ä‚¢‚È‚¢‚Æ‚«
        //•¡”‰ñ–Â‚ç‚È‚¢‚æ‚¤‚É‚·‚é
        if ((bear.isDamage && !bear.death) || (golem.isDamage && !golem.death))
        {
            if (!isHit && !gameManager.isDamageSe) 
            {
                if (playerController.attack > playerController.firstAttack) //”íƒ_ƒ‘‰Á
                {
                    se.PlayOneShot(powerDamage[playerController.weapon]);
                }
                else if (playerController.attack < playerController.firstAttack) //”íƒ_ƒŒ¸­
                {
                    se.PlayOneShot(downDamage[playerController.weapon]);
                }
                else //’Êí
                {
                    se.PlayOneShot(damage[playerController.weapon]);
                }
                isHit = true;
                gameManager.isDamageSe = true;
            }
            
        }
        else
        {
            isHit = false;
            gameManager.isDamageSe = false;
        }

        //ŒF€–S
        if (bear.death) 
        {
            //€–S‚ÌSE
            //–Â‚Á‚Ä‚¢‚éÅ’†‚È‚ç‚Î‘¼‚Ì“G‚ª€–S‚µ‚Ä‚à–Â‚ç‚³‚È‚¢
            if (!isBearDeath && !gameManager.isDeathSe) 
            {
                se.PlayOneShot(bearDeath);
                isBearDeath = true;
                gameManager.isDeathSe = true;
            }
        }
        else
        {
            isBearDeath = false;
            gameManager.isDeathSe = false;
        }

        //ƒS[ƒŒƒ€€–S
        if (golem.death) 
        {
            //€–S‚ÌSE
            //–Â‚Á‚Ä‚¢‚éÅ’†‚È‚ç‚Î‘¼‚Ì“G‚ª€–S‚µ‚Ä‚à–Â‚ç‚³‚È‚¢
            if (!isGolemDeath && !gameManager.isDeathSe)
            {
                se.PlayOneShot(golemDeath);
                isGolemDeath = true;
                gameManager.isDeathSe = true;
            }
        }
        else
        {
            isGolemDeath = false;
            gameManager.isDeathSe = false;
        }
    }
}
