using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Quaternion targetRotation;
    Rigidbody rb;
    private AudioSource se;
    public AudioClip damage_se;

    //��b�\��
    public float maxHp = 100.0f;  //�ő��HP
    public float currentHp;       //���݂�HP
    public float maxAp = 100.0f;  //�ő��AP
    public float currentAp;       //���݂�AP
    public float use_Ap;          //����AP
    //�ړ��֘A
    float horizontal;             //���ړ�
    float vertical;               //�c�ړ�
    Quaternion horizontalRotation;//�����擾
    Vector3 velocity;             //�x�N�g���擾
    float speed;                  //�ړ����x
    float move;                   //�����A����̐؂�ւ�
    float rotationSpeed;          //������ς��鑬�x
    //�U���֘A
    public int weapon = 0;        //�U����i  
    public int skill = 0;         //�t�^�������
    public bool interval = false; //�N�[���^�C�������ǂ���
    public bool apLost = false;   //�U���ɕK�v��Ap���c���Ă��邩�ǂ���
    bool input = false;           //�������h�~
    public float attack;          //�U����
    public bool isAttack = false; //�U����
    public float notAttack = 0;   //������悤�ɂȂ�܂ł̎���
    public float hitWeapon = 0.0f;//�����蔻��\������
    //�_���[�W�֘A
    public float damage;          //�󂯂�_���[�W
    public bool isDamage;         //��e�m�F
    float currentTime = 0.0f;     //���݂̎��Ԏ擾
    public int kill_enemy;        //�|�����G��
    public int goalspawn;         //�S�[���o���ɕK�v�ȓG��
    public bool isStop;           //�_���[�W���󂯂�ƈꎞ�I�ɓ������~�߂�
    public bool death;
    //Slider������
    public Slider hpSlider;       //HP�o�[
    public Slider apSlider;       //Ap�o�[
    //����
    public GameObject[] use_weapon;     //�g�p���̕���
    int weapon_num;
    //����̓����蔻��
    public Collider[] weaponCollider;   //����̃R���C�_�[

    void Start()
    {
        //������
        animator = GetComponent<Animator>();
        se = GetComponent<AudioSource>();
        targetRotation = transform.rotation;
        hitWeapon = 1.0f;
        weapon = Random.Range(0, 3);
        skill = Random.Range(1, 100);
        damage = 5.0f;
        speed = 7.0f;
        kill_enemy = 0;
        goalspawn = 5;
        isDamage = false;
        death = false;

        for (int i = 0; i < 3; i++) 
        {
            use_weapon[i].SetActive(false);
            weaponCollider[i].enabled = false;
        }

        //�U����i�𕪊�
        switch (weapon)
        {
            case (int)Weapon.KNIFE:
                Knife();
                break;
            case (int)Weapon.SWORD:
                Sword();
                break;
            case (int)Weapon.KNUCKLE:
                Knuckle();
                break;
        }

        //�ǉ����ʕt�^
        RandomSkill();

        //Slider�𖞃^���ɂ���B
        hpSlider.value = 1;
        apSlider.value = 1;
        //���݂̒l���ő�l�Ɠ����ɂ���
        currentHp = maxHp;
        currentAp = maxAp;

    }

    private void FixedUpdate()
    {
        GameManager gameManager = GetComponent<GameManager>();

        if (Input.GetMouseButton(1))
        {
            currentHp = 0.0f;
           // kill_enemy = 5;
        }

        if (currentHp <= 0.0f && !death) 
        {
            death = true;
            animator.SetTrigger("death");
        }

        //�U�����͂��̏ꂩ��ړ��ł��Ȃ�
        if (!isAttack && !isStop && !death)  
        {
            Move3D();
        }

        if(currentAp >= use_Ap)
        {
            //�U���p�֐�
            Attack();
        }

        //�L���J�E���g�̐���
        if (kill_enemy >= 5)
            kill_enemy = 5;

        //HP�̐���
        if (currentHp <= 0.0f)
            currentHp = 0.0f;

        //AP�̎�����
        if (currentAp < maxAp)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= 2.0f)
            {
                currentAp += 5.0f;
                currentTime = 0.0f;
            }
        }
        //�c��AP��0�ɂȂ�����t���O�����Ă�
        if (currentAp < use_Ap) apLost = true;
        else apLost = false;

        //�ő�HP�ɂ����錻�݂�HP��Slider�ɔ��f
        hpSlider.value = currentHp / maxHp;
        //�ő�AP�ɂ����錻�݂�AP��Slider�ɔ��f�B
        apSlider.value = currentAp / maxAp;
    }

    //�ړ��֘A
    void Move3D()
    {
        //���̓x�N�g���̎擾
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        velocity = horizontalRotation * new Vector3(horizontal, 0, vertical).normalized;

        //���x�̎擾
        move = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
        rotationSpeed = 600 * Time.deltaTime;
        transform.position += velocity * Time.deltaTime * speed * move;

        if (velocity.magnitude > 0.5f) 
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

        animator.SetFloat("Speed", velocity.magnitude * move, 0.1f, Time.deltaTime);
    }

    //�U���֘A
    void Attack()
    {
        //���N���b�N�����Ƃ��Ɏ��s
        if (Input.GetMouseButton(0) && !interval && !input)  
        {
            //�N�[���^�C���t���O
            interval = true;
            //���̓t���O
            input = true;
            ////�U�����t���O
            isAttack = true;
            //�U������
            switch (weapon)
            {
                case (int)Weapon.KNIFE:
                    animator.SetTrigger("knife");
                    Invoke("IsAttack", 0.05f);
                    Invoke("Interval", 2.5f);
                    break;
                case (int)Weapon.SWORD:
                    animator.SetTrigger("sword");
                    Invoke("IsAttack", 0.05f);
                    Invoke("Interval", 3.5f);
                    break;
                case (int)Weapon.KNUCKLE:
                    animator.SetTrigger("knuckle");
                    Invoke("IsAttack", 0.05f);
                    Invoke("Interval", 1.0f);
                    break;
            }
            //���݂�AP�������AP������
            currentAp = currentAp - use_Ap;
            Invoke("NotAttack", notAttack);
            Invoke("HitWeapon", hitWeapon);
        }
        //�������֎~�p
        if (Input.GetMouseButtonUp(0))
            input = false;
    }

    //����
    void Knife()
    {
        attack = 9.0f;
        use_Ap = 20.0f;
        notAttack = 0.8f;
        use_weapon[(int)Weapon.KNIFE].SetActive(true);
        weapon_num = (int)Weapon.KNIFE;
    }
    void Sword()
    {
        attack = 14.0f;
        use_Ap = 30.0f;
        notAttack = 0.5f;
        use_weapon[(int)Weapon.SWORD].SetActive(true);
        weapon_num = (int)Weapon.SWORD;
    }
    void Knuckle()
    {
        attack = 5.0f;
        use_Ap = 10.0f;
        notAttack = 0.3f;
        use_weapon[(int)Weapon.KNUCKLE].SetActive(true);
        weapon_num = (int)Weapon.KNUCKLE;
    }
    //�N�[���^�C��
    void Interval()
    {
        interval = false;
    }

    //�t�^����
    void RandomSkill()
    {
        if(skill>=1&&skill<=20)//AP2�{
        {
            maxAp *= 2.0f;
        }
        else if (skill >= 21 && skill <= 40)//HP2�{
        {
            maxHp *= 2.0f;
        }
        else if (skill >= 41 && skill <= 50)//�U����2�{
        {
            attack *= 2.0f;
        }
        else if (skill >= 51 && skill <= 60)//��_���[�W2�{
        {
            damage *= 2.0f;
        }
        else if (skill >= 61 && skill <= 70)//�ړ�1.5�{�E�U����0.75�{
        {
            speed *= 1.5f;
            attack *= 0.75f;
        }
        else if (skill >= 71 && skill <= 80)//�ړ�0.75�{�E�U����1.5�{
        {
            speed *= 0.75f;
            attack *= 1.5f;
        }
        else if (skill >= 81 && skill <= 90)//����AP2�{�E�U����3�{
        {
            use_Ap *= 2.0f;
            attack *= 3.0f;
        }
        else if (skill >= 91 && skill <= 95)//��_���[�W2�{�E�^�_���[�W0.5�{
        {
            damage *= 2.0f;
            attack *= 0.5f;
        }
        else if (skill >= 96 && skill <= 100)//��_���[�W0.5�{�E�^�_���[�W2�{
        {
            damage /= 2.0f;
            attack *= 2.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager gameManager = GetComponent<GameManager>();
        //Enemy�^�O�̃I�u�W�F�N�g�ɐG���Ɣ���
        if (other.CompareTag("enemyweapon") && !isDamage && !death)  
        {
            //���݂�HP����_���[�W������
            currentHp -= damage;
            //�_���[�WSE��炷
            se.PlayOneShot(damage_se);
            isDamage = true;
            isStop = true;
            HitWeapon();
            if (currentHp <= 0.0f)
            {
                animator.SetTrigger("death");
            }
            else
            {
                animator.SetTrigger("damage");
                Invoke("NotDamage", 0.4f);
                Invoke("CanMove", 0.5f);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        //Goal�^�O�̃I�u�W�F�N�g�ɐG���Ɣ���
        if (other.CompareTag("Goal"))
        {
            GameManager gameManager = GetComponent<GameManager>();
            gameManager.gameClear = true;
        }
    }

    void NotDamage()
    {
        isDamage = false;
    }
    void CanMove()
    {
        isStop = false;
    }
    void IsAttack()
    {
        weaponCollider[weapon_num].enabled = true;
    }
    void NotAttack()
    {
        isAttack = false;
    }
    void HitWeapon()
    {
        weaponCollider[weapon_num].enabled = false;
    }

    public enum Weapon
    {
        KNIFE,
        SWORD,
        KNUCKLE
    }
}
