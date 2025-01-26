using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshagent���g��
using UnityEngine.UI;


public class GolemController : ObjectMove
{
    //�ړ��֘A
    public Transform player;      //�v���C���[�̈ʒu
    public int speed;              //�ړ����x

    //�퓬�֘A
    public GameObject shotPos;   //�������I�u�W�F�N�g�𔭎˂���ʒu
    public GameObject rock;      //�������U���̊�I�u�W�F�N�g
    public float rockSpeed;      //��̑��x
    public bool attack = false;  //�U���t���O
    private float attackTime;    //�U������܂ł̎���
    public float attackStart;    //�U�����n�߂鎞��
    public bool attackFar;       //�������U��
    public bool attackNear;      //�ߋ����U��

    //�_���[�WSE
    public AudioClip[] damage_Se;

    //����̓����蔻��
    public Collider nearCollider;  //�ߋ����U���p�R���C�_�[

    //�S�[���I�u�W�F�N�g
    public GameObject goal;

    //�{�XUI
    public GameObject bossUi;

    // Start is called before the first frame update
    private new void Start()
    {
        //�ړ����x�̐ݒ�
        agent.speed = speed;
        //�{�X�̉E����擾�i�������̔��ˈʒu�̂��߁j
        shotPos = GameObject.Find("Index_Proximal_R");
        //���݂̒l���ő�l�Ɠ����ɂ���
        currentHp = maxHp;
        //�ߋ����U���̓����蔻�������
        nearCollider.enabled = false;
        bossUi.SetActive(true);
    }


    private new void Update()
    {
        move = 1.0f;

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
            move = 0.0f;
            agent.speed = 0;

            if(!isAttack)
            {
                transform.LookAt(player.transform);

                //�ߋ����U��
                animator.SetTrigger("nearattack");
                nearCollider.enabled = true;

                ////�ߋ���
                //if (attackNear) 
                //{
                //    animator.SetTrigger("nearattack");
                //    nearCollider.enabled = true;
                //}
                ////������
                //else if (attackFar)
                //{
                //    animator.SetTrigger("farattack");
                //    //rock = (GameObject)Instantiate(rock, this.transform.position, Quaternion.identity);
                //    //rock.transform.parent = shotPos.transform;
                //    //Invoke("RockShot", 1.6f);
                //}
                isAttack = true;
                attackTime = 0.0f;
                Invoke("NotWeapon", 3.0f);
            }
        }

        GameObject obj = GameObject.Find("Player");
        gameManager = obj.GetComponent<GameManager>();

        if (gameManager.gameOver)
        {
            agent.speed = 0;
            move = 0.0f;
        }

        //�̗͂�0�ȉ��ɂȂ�ƁA���S�A�j���[�V������\�����I�u�W�F�N�g������
        if (currentHp <= 0.0f && !death)
        {
            animator.SetTrigger("death");
            bossUi.SetActive(false);
            agent.speed = 0;
            death = true;
            Invoke("Death", 0.6f);
        }
        animator.SetFloat("EnemyBoss", speed, 0.1f, Time.deltaTime);

        //�ő�HP�ɂ����錻�݂�HP��Slider�ɔ��f
        hpSlider.value = currentHp / maxHp;

        //�f�o�b�O�p
        //{
        //    if (Input.GetKeyDown(KeyCode.E))
        //    {
        //        currentHp = 0.0f;
        //    }
        //}
        
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        //weapon�^�O�̃I�u�W�F�N�g�ɐG���Ɣ���
        if (other.CompareTag("weapon"))
        {
            if(!isAttack)
            {
                GetComponent<Animator>().SetTrigger("damage");
            }
            //���킲�Ƃɔ�eSE��炷
            se.PlayOneShot(damage_Se[playerController.weapon]);
            //���݂�HP����_���[�W������
            currentHp -= playerController.attack;
        }
    }

    //void RockShot()
    //{
        
    //    rock.GetComponent<Rigidbody>().velocity = shotPos.transform.forward * rockSpeed;
    //}

    void Death()
    {
        //���S����
        //���̏�ɃS�[���𐶐�
        Instantiate(goal, transform.position, Quaternion.identity);
        //�I�u�W�F�N�g����������
        Destroy(gameObject);
    }
    //����R���C�_�[����
    void NotWeapon()
    {
        isAttack = false;
        agent.speed = speed;
        nearCollider.enabled = false;
    }


}
