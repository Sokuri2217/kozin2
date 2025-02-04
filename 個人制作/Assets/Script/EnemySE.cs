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
    new void Update()
    {
        //“G‚ª”í’e‚µ‚½‚Æ‚«
        //€–S”»’è‚É‚È‚Á‚Ä‚¢‚È‚¢‚Æ‚«
        //•¡”‰ñ–Â‚ç‚È‚¢‚æ‚¤‚É‚·‚é
        if ((bear.isDamage && !bear.death) || (golem.isDamage && !golem.death))
        {
            if (!isHit)
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
            }
            
        }
        else
        {
            isHit = false;
        }

        //ŒF€–S
        if (bear.death) 
        {
            if (!isBearDeath)
            {
                se.PlayOneShot(bearDeath);
                isBearDeath = true;
            }
        }
        else
        {
            isBearDeath = false;
        }

        //ƒS[ƒŒƒ€€–S
        if (golem.death) 
        {
            if(!isGolemDeath)
            {
                se.PlayOneShot(golemDeath);
                isGolemDeath = true;
            }
        }
        else
        {
            isGolemDeath = false;
        }
    }
}
