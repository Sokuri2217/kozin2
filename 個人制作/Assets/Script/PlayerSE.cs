using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSE : SeScript
{
    //�A�C�e���擾��
    public AudioClip itemGet;
    public bool isItemGet;

    //��e��
    public AudioClip damage;      //�ʏ�
    public AudioClip powerDamage; //��_������
    public AudioClip downDamage; //��_������

    // Start is called before the first frame update
    new void Start()
    {
        isItemGet = false;
        isHit = false;
    }

    // Update is called once per frame
    new void Update()
    {
        //�A�C�e���擾
        if(playerController.item)
        {
            //������Ȃ��悤�ɂ���
            if (!isItemGet)
            {
                se.PlayOneShot(itemGet);
                isItemGet = true;
            }
        }
        else
        {
            isItemGet = false;
        }

        //��e
        if(playerController.isDamage)
        {
            //������Ȃ��悤�ɂ���
            if(!isHit)
            {
                //�v���C���[�̃_���[�W�ʂɂ���ĕς���
                if (playerController.damage > playerController.firstDamage) //��_������
                {
                    se.PlayOneShot(powerDamage);
                }
                else if (playerController.damage < playerController.firstDamage) //��_������
                {

                }
                else //�ʏ�
                {
                    se.PlayOneShot(damage);
                }

                isHit = true;
            }
        }
        else
        {
            isHit = false;
        }
    }
}
