using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Quaternion targetRotation;
    Rigidbody player_rb;
    Vector3 pos;

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
    bool input = false;           //�������h�~
    public float attack;          //�U����
    //�_���[�W�֘A
    public float damage;          //�󂯂�_���[�W
    float currentTime = 0.0f;     //���݂̎��Ԏ擾
    public int kill_enemy;        //�|�����G��

    //Slider������
    public Slider hpSlider;       //HP�o�[
    public Slider apSlider;       //Ap�o�[
    //����
    public GameObject knife;      //�i�C�t
    public GameObject sword;      //�\�[�h
    public GameObject spear;      //�X�s�A�[

    void Awake()
    {
        //������
        TryGetComponent(out animator);
        targetRotation = transform.rotation;
        player_rb = GetComponent<Rigidbody>();
        pos = transform.position;
        knife.SetActive(false);
        sword.SetActive(false);
        spear.SetActive(false);
        weapon = Random.Range(1, 3);
        skill = Random.Range(1, 100);
        damage = 10.0f;
        speed = 50.0f;
        kill_enemy = 0;

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
            Move3D();  //�ړ�

            Attack();  //�U��

            if (Input.GetMouseButton(1))
            {
                kill_enemy++;
            }

            //AP�̎�����
            if (currentAp < maxAp)
            {
                currentTime += Time.deltaTime;

                if (currentTime >= 1.0f)
                {
                    currentAp += 1.0f;
                    currentTime = 0.0f;
                }
            }
            //���݂�AP������AP��菭�Ȃ��ƍU���ł��Ȃ�
            if (currentAp < use_Ap) 
            {
                interval = true;
            }
            else
            {
                interval = false;
            }

            //�ړ����x��Animator�ɔ��f
            animator.SetFloat("Speed", velocity.magnitude * move, 0.1f, Time.deltaTime);
            //�ő�HP�ɂ����錻�݂�HP��Slider�ɔ��f
            hpSlider.value = currentHp / maxHp;
            //�ő�AP�ɂ����錻�݂�AP��Slider�ɔ��f�B
            apSlider.value = currentAp / maxAp;
        }
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

        pos += velocity / speed * move;
        transform.position = pos;

        if (velocity.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
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

            //�U������
            switch (weapon)
            {
                case (int)Weapon.Knife:
                    Invoke("Interval", 1.0f);
                    break;
                case (int)Weapon.Sword:
                    Invoke("Interval", 4.0f);
                    break;
                case (int)Weapon.Spear:
                    Invoke("Interval", 7.0f);
                    break;
            }

            //���݂�AP�������AP������
            currentAp = currentAp - use_Ap;
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
        //GetComponent<Animator>().SetTrigger("knuckle");
        attack = 5.0f;
        use_Ap = 10.0f;
        knife.SetActive(true);
    }
    void Sword()
    {
        //GetComponent<Animator>().SetTrigger("knife");
        attack = 10.0f;
        use_Ap = 25.0f;
        sword.SetActive(true);
    }
    void Spear()
    {
        //GetComponent<Animator>().SetTrigger("sword");
        attack = 20.0f;
        use_Ap = 50.0f;
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

    //void OnCollisionStay(Collision collision)
    //{
    //    //Enemy�^�O�̃I�u�W�F�N�g�ɐG���Ɣ���
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        //���݂�HP����_���[�W������
    //        currentHp = currentHp - damage;

    //        //�ő�HP�ɂ����錻�݂�HP��Slider�ɔ��f
    //        hpSlider.value = currentHp / maxHp;
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        //Goal�^�O�̃I�u�W�F�N�g�ɐG���Ɣ���
        if (other.CompareTag("Goal"))
        {
            GameManager gameManager = GetComponent<GameManager>();

            gameManager.gameClear = true;
            gameManager.gamePlay = false;
        }

        //Enemy�^�O�̃I�u�W�F�N�g�ɐG���Ɣ���
        if (other.CompareTag("Enemy"))
        {
            //���݂�HP����_���[�W������
            currentHp = currentHp - damage;
        }
    }

    public enum Weapon
    {
        Spear,
        Knife,
        Sword
    }
}
