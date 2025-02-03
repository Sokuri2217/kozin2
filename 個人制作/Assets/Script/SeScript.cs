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

    //�G
    public AudioClip[] enemyDamageSe;      //��e
    public AudioClip[] enemyPowerDamageSe; //��_���[�W�㏸���̔�e
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
        //�G�ɍU�������������Ƃ��ɁASE��炷
        if (enemyBear.isDamage || golemController.isDamage) 
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
        }

        if(playerController.isDamage)
        {
            //�_���[�WSE��炷
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
            //�擾����炷
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
