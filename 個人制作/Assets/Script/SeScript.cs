using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeScript : MonoBehaviour
{
    public AudioSource se;  //SE�֘A

    //�v���C���[
    public AudioClip playerDamageSe;      //��e
    public AudioClip playerPowerDamageSe; //��_���[�W�㏸���̔�e
    public AudioClip itemGet;             //�A�C�e���l��
    public bool playerIsSound;

    //�G
    public AudioClip[] enemyDamageSe;      //��e
    public AudioClip[] enemyPowerDamageSe; //��_���[�W�㏸���̔�e
    public bool enemyIsSound;
    //�F
    public AudioClip BearDeathSe; //���S
    //�S�[����
    public AudioClip GolemDeathSe; //���S

    //UI
    public AudioClip changeConsentSe; //�Q�[�W�����܂����Ƃ�
    private bool consent;             //���܂����Ƃ���SE��������Ȃ�Ȃ��悤�ɂ���
    public AudioClip changeSe;        //�Q�[�W��������Ƃ�
    private bool use;                 //������Ƃ���SE��������Ȃ�Ȃ��悤�ɂ���

    //�X�N���v�g�擾
    public PlayerController playerController;
    public PlayerAttack playerAttack;
    public GameManager gameManager;
    public MainUIScript mainUI;

    // Start is called before the first frame update
    void Start()
    {
        enemyIsSound = false;
        playerIsSound = false;
    }

    // Update is called once per frame
    void Update()
    {
        //�G�ɍU�������������Ƃ��ɁASE��炷
        if (playerAttack.hit)  
        {
            if(!enemyIsSound)
            {
                //���킲�Ƃɔ�eSE��炷
                if (playerController.attack > playerController.firstAttack)
                {
                    //�v���C���[�̍U���͂��㏸���Ă���Ƃ�
                    se.PlayOneShot(enemyPowerDamageSe[playerController.weapon]);
                }
                else
                {
                    //�v���C���[�̍U���͂��ω����Ă��Ȃ��Ƃ�
                    se.PlayOneShot(enemyDamageSe[playerController.weapon]);
                }

                enemyIsSound = true;
            }
        }

        //�G�����S����Ƃ�
        //�F
        //if(enemyBear.death)
        //{
        //    se.PlayOneShot(BearDeathSe);
        //}
        //if(golemController.death)
        //{
        //    se.PlayOneShot(GolemDeathSe);
        //}

        //�G�̍U���ɓ��������Ƃ�
        if(playerController.isDamage)
        {
            if (!playerIsSound)
            {
                //�_���[�WSE��炷
                if (playerController.damage > playerController.firstDamage)
                {
                    se.PlayOneShot(playerPowerDamageSe);
                }
                else
                {
                    se.PlayOneShot(playerDamageSe);
                }
                playerIsSound = true;
            }
        }
        else
        {
            playerIsSound = false;
        }

        //�A�C�e��������Ƃ�
        if (playerController.item)
        {
            //�擾����炷
            se.PlayOneShot(itemGet);
            playerController.item = false;
        }

        //�Q�[�W���܂����Ƃ�
        if (mainUI.changeConsent)
        {
            if (!consent)
            {
                se.PlayOneShot(changeConsentSe);
                consent = true;
            }
        }
        //�Q�[�W������Ƃ�
        else if (mainUI.changeInput) 
        {
            if (!use)
            {
                se.PlayOneShot(changeSe);
                use = true;
                mainUI.changeInput = false;
            }
        }
        else
        {
            consent = false;
            use = false;
        }
    }
}
