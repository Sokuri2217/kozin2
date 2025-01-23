using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ObjectMove : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public AudioSource se;
    public Rigidbody rb;

    //��{�\��
    public float maxHp; //�ő��HP
    public float currentHp;      //���݂�HP

    public bool isAttack;       //�U����
    public float speed;         //�ړ����x

    public float damage;              //�󂯂�_���[�W
    public bool isDamage;             //��e�m�F

    public bool death;                //���S�t���O

    public bool isStop;               //�_���[�W���󂯂�ƈꎞ�I�ɓ������~�߂�

    //Slider������                             
    public Slider hpSlider; //HP�o�[

    // �Q�[���̏�Ԃ��Ǘ�
    public GameManager gameManager;
    public PlayerController playerController;

    //����̎��
    public enum Weapon{ Knife, Sword, Knuckle }

    // Start is called before the first frame update
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        se = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

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
