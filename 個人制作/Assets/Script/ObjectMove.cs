using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ObjectMove : MonoBehaviour
{
    //�R���|�[�l���g�Ǘ�
    public Animator animator;  //�A�j���[�V����
    public float move;         //�A�j���[�V�����Ǘ�
    public NavMeshAgent agent; //�ړ��͈͐ݒ�
    public Rigidbody rb;       //�d�͐ݒ�

    //��{�\��
    public float maxHp;      //�ő��HP
    public float currentHp;  //���݂�HP
    public float damage;     //�󂯂�_���[�W

    //�t���O�Ǘ�
    public bool isAttack;  //�U����
    public bool isDamage;  //��e�m�F
    public bool death;     //���S�t���O
    public bool isStop;    //�ꎞ�I�ɓ������~�߂�

    //Slider������                             
    public Slider hpSlider;  //HP�o�[

    // �Q�[���̏�Ԃ��Ǘ�
    public GameManager gameManager;            //GameManager�̏��擾
    public PlayerController playerController;  //PlayerController�̏��擾

    //����̎��
    public enum Weapon{ Knife, Sword, Knuckle }

    // Start is called before the first frame update
    public void Start()
    {
        isDamage = false;
        death = false;

        //Slider�𖞃^���ɂ���B
        hpSlider.value = 1;
    }

    // Update is called once per frame
    public void Update()
    {
        //HP�̐���
        if (currentHp <= 0.0f)
            currentHp = 0.0f;

        //��e���U������ƁA�ꎞ�I�Ɉړ����~�߂�
        if (isAttack || isDamage)
        {
            agent.speed = 0;
        }
    }

    //�_���[�W���t���O��false�ɂ���
    public void NotDamage()
    {
        isDamage = false;
    }
    //�ړ��֎~�t���O��false�ɂ���
    public void CanMove()
    {
        isStop = false;
    }
    //�U�����t���O��false�ɂ���
    public void NotAttack()
    {
        isAttack = false;
    }
}
