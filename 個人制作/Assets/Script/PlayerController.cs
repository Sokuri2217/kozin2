using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    
    Rigidbody rb;
    private AudioSource se;
    public AudioClip damage_se;

    //��b�\��
    public float maxHp = 100.0f; //�ő��HP
    public float currentHp;      //���݂�HP
    public float maxAp = 100.0f; //�ő��AP
    public float currentAp;      //���݂�AP
    public float useAp;          //����AP

    //�ړ��֘A
    private float horizontal;              //���ړ�
    private float vertical;                //�c�ړ�
    private Quaternion horizontalRotation; //�����擾
    private Vector3 velocity;              //�x�N�g���擾
    private Quaternion targetRotation;     //�����̉�]
    public float speed;                    //�ړ����x
    private float move;                    //�����A����̐؂�ւ�
    private float rotationSpeed;           //������ς��鑬�x
    private bool isJump;                   //�W�����v��
    public float jumpPower;                //�W�����v��

    //�U���֘A                                   
    public int weapon = 0;       //�U����i  
    public int skill = 0;        //�t�^�������
    public bool apLost;          //�U���ɕK�v��Ap���c���Ă��邩�ǂ���
    private bool input;          //�������h�~
    public float attack;         //�U����
    private bool isAttack;       //�U����
    private float notAttack = 0; //������悤�ɂȂ�܂ł̎���

    //�_���[�W�֘A                               
    public float damage;              //�󂯂�_���[�W
    private bool isDamage;            //��e�m�F
    private float currentTime = 0.0f; //���݂̎��Ԏ擾
    public bool isStop;               //�_���[�W���󂯂�ƈꎞ�I�ɓ������~�߂�
    public bool death;                //���S�t���O

    //Slider������                             
    public Slider hpSlider; //HP�o�[
    public Slider apSlider; //Ap�o�[

    //����                                       
    public GameObject[] useWeapon; //�g�p���̕���

    //����̓����蔻��                          
    public Collider[] weaponCollider; //����̃R���C�_�[

    //�g�p���픻��p
    public enum Weapon { KNIFE, SWORD, KNUCKLE }

    void Start()
    {
        //������
        animator = GetComponent<Animator>();
        se = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        targetRotation = transform.rotation;
        weapon = Random.Range(0, 3);
        skill = Random.Range(1, 100);
        damage = 5.0f;
        speed = 5.0f;
        isDamage = false;
        death = false;
        // ����̏�����
        for (int i = 0; i < 3; i++)
        {
            useWeapon[i].SetActive(false);
            weaponCollider[i].enabled = false;
        }
        //�g�p���팈��
        InitializeWeapon();
        //�X�e�[�^�X�ω�
        RandomSkill();

        //Slider�𖞃^���ɂ���B
        hpSlider.value = 1;
        apSlider.value = 1;
        //���݂̒l���ő�l�Ɠ����ɂ���
        currentHp = maxHp;
        currentAp = maxAp;

    }

    //�v���C���[�̊�{����
    private void Update()
    {
        if(!death)
        {
            if (!isAttack && !isStop)
            {
                //�U�����͂��̏ꂩ��ړ��ł��Ȃ�
                Jump3D();
                //�U�����͂��̏ꂩ��ړ��ł��Ȃ�
                Move3D();
            }
            //�U���ɕK�v��AP������Ă��違���͎��̏�Ԃ��U�����ł͂Ȃ�
            if (currentAp >= useAp && !isAttack && !isStop) 
            {
                //�U���p�֐�
                Attack();
            }
        }
    }


    private void FixedUpdate()
    {
        GameManager gameManager = GetComponent<GameManager>();

        //���S����
        if (currentHp <= 0.0f && !gameManager.gameOver)  
        {
            gameManager.gameOver = true;
            Death();
        }

        //HP�̐���
        if (currentHp <= 0.0f)
            currentHp = 0.0f;

        //AP�̎����񕜊֐�
        AutoRegenAP();

        //�c��AP��0�ɂȂ�����t���O�����Ă�
        if (currentAp < useAp) apLost = true;
        else apLost = false;

        //�ő�HP�ɂ����錻�݂�HP��Slider�ɔ��f
        hpSlider.value = currentHp / maxHp;
        //�ő�AP�ɂ����錻�݂�AP��Slider�ɔ��f
        apSlider.value = currentAp / maxAp;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager gameManager = GetComponent<GameManager>();

        //�G�̍U���ɓ�����
        if (other.CompareTag("enemyweapon") && !isDamage && gameManager.gamePlay)
        {
            currentHp -= damage;   //���݂�HP����_���[�W������
            isDamage = true;       //�_���[�W����Ԃɂ���
            isStop = true;         //���̏�Œ�~������
            HitWeapon();
            if (currentHp > 0.0f)
            {
                //��e�A�j���[�V�����Đ�
                animator.SetTrigger("damage");
                //�_���[�WSE��炷
                se.PlayOneShot(damage_se);
                //���G����
                Invoke("NotDamage", 1.5f);
                //��e���Ă��瓮����悤�ɂȂ�܂ł̎���
                Invoke("CanMove", 0.5f);
            }
        }

        //Goal�^�O�̃I�u�W�F�N�g�ɐG���Ɣ���
        if (other.CompareTag("Goal"))
        {
            speed = 0.0f;
            gameManager.gameClear = true;
        }
    }

    //���n������W�����v���t���O��false�ɂ���
    private void OnCollisionEnter(Collision other)
    {
        if (isJump)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                isJump = false;
            }
        }
    }
    //�ړ�����
    void Move3D()
    {
        //���̓x�N�g���̎擾
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        velocity = horizontalRotation * new Vector3(horizontal, 0, vertical).normalized;

        //���x�̎擾
        //�X�j�[�N
        if (Input.GetKey(KeyCode.LeftControl))
            move = 1;
        //����
        else if (Input.GetKey(KeyCode.LeftShift))
            move = 3;
        //����
        else
            move = 2;

        rotationSpeed = 600 * Time.deltaTime;
        transform.position += velocity * Time.deltaTime * speed * move;

        if (velocity.magnitude > 0.5f)
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

        animator.SetFloat("Speed", velocity.magnitude * move, 0.1f, Time.deltaTime);
    }

    //�W�����v����
    void Jump3D()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            isJump = true;
        }
    }

    //�U������
    void Attack()
    {
        //���N���b�N�����Ƃ��Ɏ��s
        if (Input.GetMouseButton(0) && !input)
        {
            //���̓t���O
            input = true;
            ////�U�����t���O
            isAttack = true;
            //�U�����t���O��true�ɂ���
            IsAttack();
            //�A�j���[�V�����Đ�
            animator.SetTrigger(GetWeaponAttackTrigger());
            //���݂�AP�������AP������
            currentAp -= useAp;

            //�t���O�ύX
            Invoke("NotAttack", notAttack);
            Invoke("HitWeapon", 0.5f);
        }

        //�������֎~�p
        if (Input.GetMouseButtonUp(0))
            input = false;
    }
    //�g�p���팈��
    void InitializeWeapon()
    {
        if (weapon == (int)Weapon.KNIFE)
        {
            attack = 9.0f;
            useAp = 15.0f;
            notAttack = 0.6f;
        }
        else if(weapon == (int)Weapon.SWORD)
        {
            attack = 14.0f;
            useAp = 25.0f;
            notAttack = 0.5f;
        }
        else if(weapon == (int)Weapon.KNUCKLE)
        {
            attack = 5.0f;
            useAp = 10.0f;
            notAttack = 0.3f;
        }
        useWeapon[weapon].SetActive(true);
    }
    //�U���A�j���[�V����
    string GetWeaponAttackTrigger()
    {
        switch (weapon)
        {
            case (int)Weapon.KNIFE: return "knife";
            case (int)Weapon.SWORD: return "sword";
            case (int)Weapon.KNUCKLE: return "knuckle";
            default: return "";
        }
    }

    //�X�e�[�^�X�ω�
    void RandomSkill()
    {
        if (skill <= 20)//AP2�{
        {
            maxAp *= 2.0f;
        }
        else if (skill <= 40)//HP2�{
        {
            maxHp *= 2.0f;
        }
        else if (skill <= 50)//�U����2�{
        {
            attack *= 2.0f;
        }
        else if (skill <= 60)//��_���[�W2�{
        {
            damage *= 2.0f;
        }
        else if (skill <= 70)//�ړ�1.5�{�E�U����0.75�{
        {
            speed *= 1.5f;
            attack *= 0.75f;
        }
        else if (skill <= 80)//�ړ�0.75�{�E�U����1.5�{
        {
            speed *= 0.75f;
            attack *= 1.5f;
        }
        else if (skill <= 90)//����AP2�{�E�U����3�{
        {
            useAp *= 2.0f;
            attack *= 3.0f;
        }
        else if (skill <= 95)//��_���[�W2�{�E�^�_���[�W0.5�{
        {
            damage *= 2.0f;
            attack *= 0.5f;
        }
        else if (skill <= 100)//��_���[�W0.5�{�E�^�_���[�W2�{
        {
            damage /= 2.0f;
            attack *= 2.0f;
        }
    }

    //AP������
    void AutoRegenAP()
    {
        if (currentAp < maxAp)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= 2.0f)
            {
                currentAp += 10f;
                currentTime = 0f;
            }
        }

        if (currentAp < useAp) apLost = true;
        else apLost = false;
    }

    //�_���[�W���t���O��false�ɂ���
    void NotDamage()
    {
        isDamage = false;
    }
    //�ړ��֎~�t���O��false�ɂ���
    void CanMove()
    {
        isStop = false;
    }
    //����̓����蔻����o��
    void IsAttack()
    {
        weaponCollider[weapon].enabled = true;
    }
    //�U�����t���O��false�ɂ���
    void NotAttack()
    {
        isAttack = false;
    }
    //����̓����蔻�������
    void HitWeapon()
    {
        weaponCollider[weapon].enabled = false;
    }
    //���S����
    void Death()
    {
        speed = 0.0f;
        animator.SetTrigger("death");
    }
}
