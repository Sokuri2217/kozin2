using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Quaternion targetRotation;
    Rigidbody player_rb;
    Vector3 pos;

    //��b�\��
    public int hp = 0;//�̗�
    public int ap = 0;//�A�^�b�N�|�C���g

    //�U���֘A
    public int weapon = 0;       //�U����i  
    public int skill = 0;        //�t�^�������
    public bool interval = false;//�N�[���^�C�������ǂ���
    bool input = false;          //�������h�~

    void Awake()
    {
        //������
        TryGetComponent(out animator);
        targetRotation = transform.rotation;
        player_rb = GetComponent<Rigidbody>();
        pos = transform.position;
        weapon = Random.Range(1, 3);
        skill = Random.Range(1, 10);

        switch (weapon)
        {
            case 1:
                Debug.Log("�i�b�N��");
                break;
            case 2:
                Debug.Log("�i�C�t");
                break;
            case 3:
                Debug.Log("�\�[�h");
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Move3D();

        Attack();
    }

    //�ړ��֘A
    void Move3D()
    {
        MainUIScript mainUIScript; //�ĂԃX�N���v�g�ɂ����Ȃ���
        GameObject obj = GameObject.Find("MainUI"); //Player���Ă����I�u�W�F�N�g��T��
        mainUIScript = obj.GetComponent<MainUIScript>(); //�t���Ă���X�N���v�g���擾

        //���̓x�N�g���̎擾
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        var velocity = horizontalRotation * new Vector3(horizontal, 0, vertical).normalized;

        //if (!mainUIScript.open_Option)
        //{
        //    velocity = new Vector3(0, 0, 0);
        //}

        //���x�̎擾
        var speed = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
        var rotationSpeed = 600 * Time.deltaTime;

        pos += velocity / 50 * speed;
        transform.position = pos;

        if (velocity.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

        //�ړ����x��Animator�ɔ��f
        animator.SetFloat("Speed", velocity.magnitude * speed, 0.1f, Time.deltaTime);
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
        Invoke("Interval", 3.0f);
    }
    void Knife()
    {
        GetComponent<Animator>().SetTrigger("knife");
        Invoke("Interval", 4.0f);
    }
    void Sword()
    {
        GetComponent<Animator>().SetTrigger("sword");
        Invoke("Interval", 6.0f);
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
        switch (skill)
        {
            case 1:
                Debug.Log("1");
                break;
            case 2:
                Debug.Log("2");
                break;
            case 3:
                Debug.Log("3");
                break;
            case 4:
                Debug.Log("4");
                break;
            case 5:
                Debug.Log("5");
                break;
            case 6:
                Debug.Log("6");
                break;
            case 7:
                Debug.Log("7");
                break;
            case 8:
                Debug.Log("8");
                break;
            case 9:
                Debug.Log("9");
                break;
            case 10:
                Debug.Log("10");
                break;
        }
    }
}
