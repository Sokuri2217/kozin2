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
    public int maxHp = 100;     //�ő��HP
    public int currentHp;       //���݂�HP
    public int maxAp = 100;     //�ő��AP
    public int currentAp;       //���݂�AP
    public int use_Ap;          //����AP
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
    public int damage;          //�󂯂�_���[�W
    public bool isDamage;         //��e�m�F
    float currentTime = 0.0f;     //���݂̎��Ԏ擾
    public int kill_enemy;        //�|�����G��
    public int goalspawn;         //�S�[���o���ɕK�v�ȓG��
    //Slider������
    public Slider hpSlider;       //HP�o�[
    public Slider apSlider;       //Ap�o�[
    //����
    public GameObject[] use_weapon;      //�g�p���̕���
    //����̓����蔻��
    public Collider[] weaponCollider;   //����̃R���C�_�[

    void Awake()
    {
        //������
        animator = GetComponent<Animator>();
        se = GetComponent<AudioSource>();
        targetRotation = transform.rotation;
        hitWeapon = 1.0f;
        weapon = Random.Range(0, 3);
        skill = Random.Range(1, 100);
        damage = 10;
        speed = 7.0f;
        kill_enemy = 0;
        goalspawn = 5;
        isDamage = false;

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

        RandomSkill();

        //Slider�𖞃^���ɂ���B
        hpSlider.value = 1;
        apSlider.value = 1;
        //���݂̒l���ő�l�Ɠ����ɂ���
        currentHp = maxHp;
        currentAp = maxAp;

    }
    // Update is called once per frame
    void Update()
    {
        GameManager gameManager = GetComponent<GameManager>();
        //�v���C���̂ݓ���
        if (gameManager.gamePlay)
        { 
            //�U�����͂��̏ꂩ��ړ��ł��Ȃ�
            if (!isAttack) Move3D();

            Attack();

            //�L���J�E���g�̐���
            if (kill_enemy >= 5)  kill_enemy = 5;

            //AP�̎�����
            if (currentAp < maxAp)
            {
                currentTime += Time.deltaTime;

                if (currentTime >= 1.0f)
                {
                    currentAp += 5;
                    currentTime = 0.0f;
                }
            }
            
        }

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
        if (Input.GetMouseButton(0) && !interval && !input && currentAp >= use_Ap)  
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
                    weaponCollider[(int)Weapon.KNIFE].enabled = true;
                    Invoke("Interval", 2.5f);
                    break;
                case (int)Weapon.SWORD:
                    animator.SetTrigger("sword");
                    weaponCollider[(int)Weapon.SWORD].enabled = true;
                    Invoke("Interval", 3.5f);
                    break;
                case (int)Weapon.KNUCKLE:
                    animator.SetTrigger("knuckle");
                    weaponCollider[(int)Weapon.KNUCKLE].enabled = true;
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
        attack = 10.0f;
        use_Ap = 20;
        notAttack = 0.8f;
        use_weapon[(int)Weapon.KNIFE].SetActive(true);
    }
    void Sword()
    {
        attack = 20.0f;
        use_Ap = 30;
        notAttack = 0.5f;
        use_weapon[(int)Weapon.SWORD].SetActive(true);
    }
    void Knuckle()
    {
        attack = 2.5f;
        use_Ap = 10;
        notAttack = 0.3f;
        use_weapon[(int)Weapon.KNUCKLE].SetActive(true);
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
            maxAp *= 2;
        }
        else if (skill >= 21 && skill <= 40)//HP2�{
        {
            maxHp *= 2;
        }
        else if (skill >= 41 && skill <= 50)//�U����2�{
        {
            attack *= 2.0f;
        }
        else if (skill >= 51 && skill <= 60)//��_���[�W2�{
        {
            damage *= 2;
        }
        else if (skill >= 61 && skill <= 70)//�ړ�1.5�{�E�U����0.75�{
        {
            speed *= 1.5f;
            attack   *= 0.75f;
        }
        else if (skill >= 71 && skill <= 80)//�ړ�0.75�{�E�U����1.5�{
        {
            speed *= 0.75f;
            attack *= 1.5f;
        }
        else if (skill >= 81 && skill <= 90)//����AP2�{�E�U����3�{
        {
            use_Ap *= 2;
            attack *= 3.0f;
        }
        else if (skill >= 91 && skill <= 95)//��_���[�W2�{�E�^�_���[�W0.5�{
        {
            damage *= 2;
            attack *= 0.5f;
        }
        else if (skill >= 96 && skill <= 100)//��_���[�W0.5�{�E�^�_���[�W2�{
        {
            damage /= 2;
            attack *= 2.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager gameManager = GetComponent<GameManager>();
        //Enemy�^�O�̃I�u�W�F�N�g�ɐG���Ɣ���
        if(other.CompareTag("enemyweapon"))
        {
            //���݂�HP����_���[�W������
            currentHp -= damage;
            se.PlayOneShot(damage_se);
            isDamage = true;
            if (currentHp <= 0.0f)
            {
                animator.SetBool("death", true);
            }
            else if (!isDamage && gameManager.gamePlay)
            {
                animator.SetTrigger("damage");
                Invoke("NotDamage", 0.5f);
                for (int i = 0; i < 3; i++)
                    weaponCollider[i].enabled = false;
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
            gameManager.gamePlay = false;
        }
    }

    void NotDamage()
    {
        isDamage = false;
    }
    void NotAttack()
    {
        isAttack = false;
    }
    void HitWeapon()
    {
        for (int i = 0; i < 3; i++)
            weaponCollider[i].enabled = false;
    }

    public enum Weapon
    {
        KNIFE,
        SWORD,
        KNUCKLE
    }
}
