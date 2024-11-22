using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Quaternion targetRotation;
    Rigidbody rb;
    public AudioSource se;
    public AudioClip damage_se;

    //��b�\��
    public float maxHp = 100;     //�ő��HP
    public float currentHp;       //���݂�HP
    public float maxAp = 100;     //�ő��AP
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
    //�_���[�W�֘A
    public float damage;          //�󂯂�_���[�W
    public bool isDamage;         //��e�m�F
    float currentTime = 0.0f;     //���݂̎��Ԏ擾
    public int kill_enemy;        //�|�����G��
    public int goalspawn;         //�S�[���o���ɕK�v�ȓG��
    //Slider������
    public Slider hpSlider;       //HP�o�[
    public Slider apSlider;       //Ap�o�[
    //����
    public GameObject knife;      //�i�C�t
    public GameObject sword;      //�\�[�h
    public GameObject spear;      //�X�s�A�[
    //����̓����蔻��
    public Collider knifeCollider;
    public Collider swordCollider;
    public Collider spearCollider;

    void Awake()
    {
        //������
        animator = GetComponent<Animator>();
        se = GetComponent<AudioSource>();
        targetRotation = transform.rotation;
        knife.SetActive(false);
        sword.SetActive(false);
        spear.SetActive(false);
        knifeCollider.enabled = false;
        swordCollider.enabled = false;
        spearCollider.enabled = false;
        weapon = Random.Range(1, 4);
        skill = Random.Range(1, 100);
        damage = 10.0f;
        speed = 7.5f;
        kill_enemy = 0;
        goalspawn = 5;
        isDamage = false;

        //�U����i�𕪊�
        switch (weapon)
        {
            case (int)Weapon.Knife:
                Knife();
                break;
            case (int)Weapon.Sword:
                Sword();
                break;
            case (int)Weapon.Spear:
                Spear();
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
            if(!isAttack)
                Move3D();  //�ړ�

            Attack();  //�U��

            //�L���J�E���g�̐���
            if(kill_enemy>=5)
            {
                kill_enemy = 5;
            }

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
        }

        if (currentAp < use_Ap)
        {
            apLost = true;
        }
        else
        {
            apLost = false;
        }

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
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        }
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
            isAttack = true;

            //�U������
            switch (weapon)
            {
                case (int)Weapon.Knife:
                    animator.SetTrigger("knife");
                    knifeCollider.enabled = true;
                    Invoke("Interval", 2.0f);
                    break;
                case (int)Weapon.Sword:
                    animator.SetTrigger("sword");
                    swordCollider.enabled = true;
                    Invoke("Interval", 5.0f);
                    break;
                case (int)Weapon.Spear:
                    animator.SetTrigger("spear");
                    spearCollider.enabled = true;
                    Invoke("Interval", 8.0f);
                    break;
            }
            //���݂�AP�������AP������
            currentAp = currentAp - use_Ap;
            Invoke("NotAttack", notAttack);
        }
        else
        {
            knifeCollider.enabled = false;
            swordCollider.enabled = false;
            spearCollider.enabled = false;
        }

        //�������֎~�p
            if (Input.GetMouseButtonUp(0))
        {
            input = false;
        }
    }

    //����
    void Knife()
    {
        attack = 5.0f;
        use_Ap = 10.0f;
        notAttack = 1.0f;
        knife.SetActive(true);
    }
    void Sword()
    {
        attack = 10.0f;
        use_Ap = 25.0f;
        notAttack = 1.0f;
        sword.SetActive(true);
    }
    void Spear()
    {
        attack = 20.0f;
        use_Ap = 50.0f;
        notAttack = 2.0f;
        spear.SetActive(true);
    }
    //�N�[���^�C��
    void Interval()
    {
        Debug.Log("�U���\�ł�");
        interval = false;
    }

    //�t�^����
    void RandomSkill()
    {
        if(skill>=1&&skill<=20)//AP2�{
        {
            maxAp *= 2;
            Debug.Log("AP2�{");
        }
        else if (skill >= 21 && skill <= 40)//HP2�{
        {
            maxHp *= 2;
            Debug.Log("HP2�{");
        }
        else if (skill >= 41 && skill <= 50)//�U����2�{
        {
            attack *= 2.0f;
            Debug.Log("�U����2�{");
        }
        else if (skill >= 51 && skill <= 60)//��_���[�W2�{
        {
            damage *= 2.0f;
            Debug.Log("��_���[�W2�{");
        }
        else if (skill >= 61 && skill <= 70)//�ړ�1.5�{�E�U����0.75�{
        {
            speed *= 0.75f;
            attack   *= 0.75f;
            Debug.Log("�ړ�1.5�{�E�U����0.75�{");
        }
        else if (skill >= 71 && skill <= 80)//�ړ�0.75�{�E�U����1.5�{
        {
            speed *= 1.5f;
            attack *= 1.5f;
            Debug.Log("�ړ�0.75�{�E�U����1.5�{");
        }
        else if (skill >= 81 && skill <= 90)//����AP�E�U����2�{
        {
            use_Ap *= 2.0f;
            attack *= 2.0f;
            Debug.Log("����AP�E�U����2�{");
        }
        else if (skill >= 91 && skill <= 95)//��_���[�W2�{�E�^�_���[�W0.5�{
        {
            damage *= 2.0f;
            attack *= 0.5f;
            Debug.Log("��_���[�W2�{�E�^�_���[�W0.5�{");
        }
        else if (skill >= 96 && skill <= 100)//��_���[�W0.5�{�E�^�_���[�W2�{
        {
            damage *= 0.5f;
            attack *= 2.0f;
            Debug.Log("��_���[�W0.5�{�E�^�_���[�W2�{");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Wall")
        {
            Debug.Log("�Ǔ�������");
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

        //Enemy�^�O�̃I�u�W�F�N�g�ɐG���Ɣ���
        if (other.CompareTag("Enemy") && !isDamage) 
        {
            //���݂�HP����_���[�W������
            currentHp -= damage;
            se.PlayOneShot(damage_se);
            isDamage = true;
            Invoke("NotDamage", 3.0f);
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

    public enum Weapon
    {
        Knife = 1,
        Sword,
        Spear
    }
}
