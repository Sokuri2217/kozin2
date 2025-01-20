using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshagent���g��
using UnityEngine.UI;

public class GolemController : MonoBehaviour
{
    private Animator animator;
    public Transform player;      //�v���C���[�̈ʒu
    private NavMeshAgent agent;

    public Slider hpSlider;       //HP�o�[
    public float maxHp;           //�ő��HP
    public float currentHp;       //���݂�HP
    bool death;                   //���S�t���O
    bool isDamage = false;        //�_���[�W���t���O
    bool attack = false;          //�U���t���O
    bool isAttack = false;        //�U���t���O
    private int move;            //�A�j���[�V�����Ǘ��p
    int chaseTime;

    private AudioSource sound = null;

    //�_���[�WSE
    public AudioClip knife_SE;
    public AudioClip sword_SE;
    public AudioClip Knuckle_SE;

    //public Collider weaponCollider;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        //Slider�𖞃^���ɂ���B
        hpSlider.value = 1;
        //���݂̒l���ő�l�Ɠ����ɂ���
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        move = 1;
        // �Ώۂ̃I�u�W�F�N�g��ǂ�������
        agent.destination = player.transform.position;

        animator.SetFloat("BossMove", move, 0.1f, Time.deltaTime);
    }
}
