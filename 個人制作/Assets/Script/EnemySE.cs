using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySE : SeScript
{
    //�X�N���v�g�擾
    public EnemyBear bear;
    public EnemySkeleton skeleton;
    public GolemController golem;

    //��e
    public AudioClip[] damage;
    public AudioClip[] powerDamage;
    public AudioClip[] downDamage;

    //���S
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
        //�G����e�����Ƃ�
        //���S����ɂȂ��Ă��Ȃ��Ƃ�
        //�������Ȃ��悤�ɂ���
        if ((bear.isDamage && !bear.death) || (golem.isDamage && !golem.death))
        {
            if (!isHit)
            {
                if (playerController.attack > playerController.firstAttack) //��_������
                {
                    se.PlayOneShot(powerDamage[playerController.weapon]);
                }
                else if (playerController.attack < playerController.firstAttack) //��_������
                {
                    se.PlayOneShot(downDamage[playerController.weapon]);
                }
                else //�ʏ�
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

        //�F���S
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

        //�S�[�������S
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
