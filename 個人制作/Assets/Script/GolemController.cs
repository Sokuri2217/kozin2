using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshagent���g��
using UnityEngine.UI;


public class GolemController : ObjectMove
{
    //�ړ��֘A
    public Transform player;      //�v���C���[�̈ʒu

    //�퓬�֘A
    public bool attack = false;  //�U���t���O
    private float attackTime;    //�U������܂ł̎���
    public float attackStart;    //�U�����n�߂鎞��
    public bool attackFar;       //�������U��
    public bool attackNear;      //�ߋ����U��

    //�_���[�WSE
    public AudioClip[] damage_Se;

    //����̓����蔻��
    public Collider nearCollider;
    public Collider farCollider;

    // Start is called before the first frame update
    private new void Start()
    {
        //weaponCollider.enabled = false;
        speed = 0.0f;

        //���݂̒l���ő�l�Ɠ����ɂ���
        currentHp = maxHp;

        nearCollider.enabled = false;
        farCollider.enabled = false;
    }


    private new void Update()
    {
        speed = 1.0f;
        // �Ώۂ̃I�u�W�F�N�g��ǂ�������
        agent.destination = player.transform.position;

        //�v���C���[���߂��ɂ���Ƃ�
        if ((transform.position.x - player.transform.position.x) < 7.0f &&
            (transform.position.z - player.transform.position.z) < 7.0f)
        {
            attackNear = true;
            attackFar = false;
        }
        //�v���C���[�������ɂ���Ƃ�
        else
        {
            attackFar = true;
            attackNear = false;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isAttack) 
        {
            attackTime++;
        }

        //�ړ��J�n�����莞�Ԍo�ƁA���̏�Ŏ~�܂�v���C���[�Ƃ̋����ɓK�����U��������
        if (attackTime >= attackStart)  
        {
            speed = 0.0f;
            agent.speed = 0;

            //�ߋ���
            if (!isAttack && attackNear)
            {
                animator.SetTrigger("nearattack");
                nearCollider.enabled = true;
            }
            //������
            else if (attackFar) 
            {
                animator.SetTrigger("farattack");
                farCollider.enabled = true;
            }
            isAttack = true;
            attackTime = 0.0f;
            Invoke("NotWeapon", 10.0f);
        }
        else
        {
            speed = 1.0f;
            agent.speed = 10;
        }

        

        GameObject obj = GameObject.Find("Player");
        gameManager = obj.GetComponent<GameManager>();

        if (gameManager.gameOver)
        {
            agent.speed = 0;
            speed = 0.0f;
        }

        //�̗͂�0�ȉ��ɂȂ�ƁA���S�A�j���[�V������\�����I�u�W�F�N�g������
        if (currentHp <= 0.0f && !death)
        {
            animator.SetTrigger("death");
            agent.speed = 0;
            death = true;
            Invoke("Death", 0.6f);
        }
        animator.SetFloat("EnemyBoss", speed, 0.1f, Time.deltaTime);

        //�ő�HP�ɂ����錻�݂�HP��Slider�ɔ��f
        hpSlider.value = currentHp / maxHp;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        //weapon�^�O�̃I�u�W�F�N�g�ɐG���Ɣ���
        if (other.CompareTag("weapon") && !isDamage)
        {
            if(!isAttack)
            {
                GetComponent<Animator>().SetTrigger("damage");
            }
            isDamage = true;
            //���킲�Ƃɔ�eSE��炷
            se.PlayOneShot(damage_Se[playerController.weapon]);
            //���݂�HP����_���[�W������
            currentHp -= playerController.attack;
        }
    }

    void Death()
    {
        //���S����
        //�I�u�W�F�N�g����������
        Destroy(gameObject);
    }
    //����R���C�_�[����
    void NotWeapon()
    {
        isAttack = false;
        nearCollider.enabled = false;
        farCollider.enabled = false;
    }


}
