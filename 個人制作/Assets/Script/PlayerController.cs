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
    //�ő�HP�ƌ��݂�HP
    public float maxHp = 100;
    public float currentHp;
    //�ő�AP�ƌ��݂�AP
    public float maxAp = 100;
    public float currentAp;
    public float use_Ap;//����AP
    //�ړ��֘A
    float horizontal;
    float vertical;
    Quaternion horizontalRotation;
    public Vector3 velocity;
    float speed;
    //Slider������
    public Slider hpSlider;
    public Slider apSlider;

    //�U���֘A
    public int weapon = 0;       //�U����i  
    public int skill = 0;        //�t�^�������
    public bool interval = false;//�N�[���^�C�������ǂ���
    bool input = false;          //�������h�~
    public float attack;        //�U����

    //�_���[�W�֘A
    public float damage = 10.0f;

    void Awake()
    {
        //������
        TryGetComponent(out animator);
        targetRotation = transform.rotation;
        player_rb = GetComponent<Rigidbody>();
        pos = transform.position;
        weapon = Random.Range(1, 3);
        skill = Random.Range(1, 100);

        //Slider�𖞃^���ɂ���B
        hpSlider.value = 1;
        apSlider.value = 1;
        //���݂̒l���ő�l�Ɠ����ɂ���
        currentHp = maxHp;
        currentAp = maxAp;

        RandomSkill();
    }
    // Update is called once per frame
    void Update()
    {
        Move3D();

        Attack();

        if (maxAp > currentAp)
            Invoke("ApHeal", 2.0f);
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
        var move = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
        var rotationSpeed = 600 * Time.deltaTime;

        speed= Input.GetKey(KeyCode.LeftShift) ? 2 : 1;

        pos += velocity / 50 * speed;
        transform.position = pos;

        if (velocity.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

        //�ړ����x��Animator�ɔ��f
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

            //�U����i�𕪊�
            switch (weapon)
            {
                case 1:
                    Knuckle();
                    break;
                case 2:
                    Knife();
                    break;
                case 3:
                    Sword();
                    break;
            }
        }
        //�������֎~�p
        if (Input.GetMouseButtonUp(0))
        {
            input = false;
        }
    }
    //����
    void Knuckle()
    {
        GetComponent<Animator>().SetTrigger("knuckle");
        attack = 5.0f;
        use_Ap = 10.0f;
        Invoke("Interval", 1.0f);
        //���݂�AP�������AP������
        currentAp = currentAp - use_Ap;
        //�ő�AP�ɂ����錻�݂�AP��Slider�ɔ��f�B
        apSlider.value = currentAp / maxAp;
    }
    void Knife()
    {
        GetComponent<Animator>().SetTrigger("knife");
        attack = 10.0f;
        use_Ap = 25.0f;
        Invoke("Interval", 4.0f);
        //���݂�AP�������AP������
        currentAp = currentAp - use_Ap;
        //�ő�AP�ɂ����錻�݂�AP��Slider�ɔ��f�B
        apSlider.value = currentAp / maxAp;
    }
    void Sword()
    {
        GetComponent<Animator>().SetTrigger("sword");
        attack = 20.0f;
        use_Ap = 50.0f;
        Invoke("Interval", 7.0f);
        //���݂�AP�������AP������
        currentAp = currentAp - use_Ap;
        //�ő�AP�ɂ����錻�݂�AP��Slider�ɔ��f�B
        apSlider.value = currentAp / maxAp;
    }
    //�N�[���^�C��
    void Interval()
    {
        Debug.Log("�U���\�ł�");
        interval = false;
    }
    //AP�֘A
    void ApHeal()
    {
        currentAp += 5.0f;
        if (currentAp >= 100.0f)
            currentAp = 100.0f;
        if (currentAp <= 0.0f)
            Invoke("ApLost", 10.0f);
        apSlider.value = currentAp / maxAp;
    }
    void ApLost()
    {
        currentAp = maxAp;
        apSlider.value = currentAp / maxAp;
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
            speed *= 1.5f;
            attack   *= 0.75f;
            Debug.Log("�ړ�1.5�{�E�U����0.75�{");
        }
        else if (skill >= 71 && skill <= 80)//�ړ�0.75�{�E�U����1.5�{
        {
            speed *= 0.75f;
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
            use_Ap *= 0.5f;
            attack *= 2.0f;
            Debug.Log("��_���[�W0.5�{�E�^�_���[�W2�{");
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        //Enemy�^�O�̃I�u�W�F�N�g�ɐG���Ɣ���
        if (collider.gameObject.tag == "Enemy")
        {
            //���݂�HP����_���[�W������
            currentHp = currentHp - damage;

            //�ő�HP�ɂ����錻�݂�HP��Slider�ɔ��f�B
            hpSlider.value = currentHp / maxHp;
        }
    }
}
