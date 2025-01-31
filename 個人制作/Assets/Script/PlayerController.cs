using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : ObjectMove
{   
    //��b�\��
    public float maxAp;        //�ő��AP
    public float currentAp;    //���݂�AP

    //����X�e�[�^�X�ۑ��p
    public float firstMaxHp;    
    public float firstNowHp;    
    public float firstMaxAp;    
    public float firstNowAp;    
    public float firstSpeed;
    public float firstAttack;
    public float firstDamage;
    public float firstUseAp;

    //�ړ��֘A
    private float horizontal;              //���ړ�
    private float vertical;                //�c�ړ�
    private Quaternion horizontalRotation; //�����擾
    private Vector3 velocity;              //�x�N�g���擾
    private Quaternion targetRotation;     //�����̉�]
    public  float speed;                   //�ړ����x
    private float rotationSpeed;           //������ς��鑬�x7
    private bool isJump;                   //�W�����v��
    public bool jumpFirst;                 //�W�����v�̏o�n�߂Œ��f����Ȃ��l�ɂ���t���O
    public float jumpPower;                //�W�����v��

    //�U���֘A                                   
    public int weapon;       //�U����i  
    public int skill;        //�t�^�������
    public float attack;     //�U����
    private float notAttack; //������悤�ɂȂ�܂ł̎���

    //�񕜔{��
    public float healHp; //HP�񕜔{��
    public float healAp; //AP�񕜔{��

    //AP�֘A
    public float useAp;               //����AP
    public bool apLost;               //�U���ɕK�v��Ap���c���Ă��邩�ǂ���
    private float currentTime = 0.0f; //���݂̎��Ԏ擾

    //����֘A                                
    public GameObject[] useWeapon;    //�g�p���̕���I�u�W�F�N�g                     
    public Collider[] weaponCollider; //����̓����蔻��

    //�Q�[�W    
    public Slider apSlider; //Ap�o�[

    //����炷
    public AudioClip damageSe;      //��e
    public AudioClip powerDamageSe; //��_���[�W�㏸���̔�e
    public AudioClip itemGet;       //�A�C�e���l��

    //���͊֘A
    //�������h�~
    private bool input;       //�U��

    //UI���擾
    public MainUIScript mainUI;

    new void Start()
    {
        GameManager gameManager = GetComponent<GameManager>();

        //������
        targetRotation = transform.rotation;
        weapon = Random.Range(0, 3);
        skill = Random.Range(1, 100);
        damage = 5.0f;
        // ����̏�����
        for (int i = 0; i < 3; i++)
        {
            useWeapon[i].SetActive(false);
            weaponCollider[i].enabled = false;
        }
        //�g�p���팈��
        InitializeWeapon();

        //�����l�ۑ�
        firstMaxHp = maxHp;
        firstMaxAp = maxAp;
        firstSpeed = speed;
        firstAttack = attack;
        firstDamage = damage;
        firstUseAp = useAp;

        //�X�e�[�^�X�ω�
        RandomSkill();

        //Slider�𖞃^���ɂ���B
        apSlider.value = 1;
        //���݂̒l���ő�l�Ɠ����ɂ���
        currentHp = maxHp;
        currentAp = maxAp;
    }

    //�v���C���[�̊�{����
    //�X�e�[�^�X�ω�
    private new void Update()
    {
        //���S���������͍U�����A��e���ȊO
        if(!death)
        {
            if (!isAttack && !isStop)
            {
                Jump3D();
                Move3D();
            }
            //�U���p�֐�
            Attack();

            if(mainUI.changeInput)
            {
                skill = Random.Range(1, 100);
                RandomSkill();
                mainUI.changeInput = false;
            }
        }
    }

    //AP�Ǘ��Ǝ��S����
    private void FixedUpdate()
    {
        firstNowHp = currentHp;
        firstNowAp = currentAp;

        //���S����
        if (currentHp <= 0.0f && !gameManager.gameOver)
        {
            gameManager.gameOver = true;
            death = true;
            Death();
        }

        //AP�̎����񕜊֐�
        AutoRegenAP();
        //���݂�HP������𒴂��Ȃ��悤�ɂ���
        if (currentHp >= maxHp)
        {
            currentHp = maxHp;
        }
        //���݂�AP������𒴂��Ȃ��悤�ɂ���
        if (currentAp >= maxAp) 
        {
            currentAp = maxAp;
        }

        //�ő�HP�ɂ����錻�݂�HP��Slider�ɔ��f
        hpSlider.value = currentHp / maxHp;
        //�ő�AP�ɂ����錻�݂�AP��Slider�ɔ��f
        apSlider.value = currentAp / maxAp;
    }

    private void OnTriggerEnter(Collider other)
    {
        //�G�̍U���ɓ�����
        if(!isDamage && gameManager.gamePlay)
        {
            if (other.CompareTag("enemyweapon")||
                other.CompareTag("nearweapon"))
            {
                currentHp -= damage;   //���݂�HP����_���[�W������

                if (currentHp > 0.0f)
                {
                    //��e�A�j���[�V�����Đ�
                    animator.SetTrigger("damage");
                    //�_���[�WSE��炷
                    if (damage > firstDamage) 
                    {
                        se.PlayOneShot(powerDamageSe);
                    }
                    else
                    {
                        se.PlayOneShot(damageSe);
                    }
                    //���G����
                    Invoke("NotDamage", 1.1f);
                    //��e���Ă��瓮����悤�ɂȂ�܂ł̎���
                    Invoke("CanMove", 0.5f);
                }

                isDamage = true;       //�_���[�W����Ԃɂ���
                isStop = true;         //���̏�Œ�~������
                HitWeapon();
            }
        }

        //�񕜃A�C�e���ɐG���
        if(other.CompareTag("healItem"))
        {
            currentHp += maxHp * healHp;
            currentAp += maxAp * healAp;
            //�擾����炷
            se.PlayOneShot(itemGet);
            //�A�C�e������������
            Destroy(other.gameObject);
        }

        if (other.CompareTag("allHeal"))
        {
            currentHp = maxHp;
            currentAp = maxAp;
            //�擾����炷
            se.PlayOneShot(itemGet);
            //�A�C�e������������
            Destroy(other.gameObject);
        }

        //�n�ʂɐG���
        if (other.CompareTag("Ground") && !jumpFirst) 
        {
            rb.isKinematic = true;
            agent.enabled = true;
            isJump = false;
        }

        //Goal�ɐG���
        if (other.CompareTag("Goal"))
        {
            speed = 0.0f;
            gameManager.gameClear = true;
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
        transform.position += velocity * Time.deltaTime * move * speed;

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
            rb.isKinematic = false;
            agent.enabled = false;
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            isJump = true;
            jumpFirst = true;
            Invoke("FirstCancel", 0.015f);
        }
    }

    //�U������
    void Attack()
    {
        if(!isStop)
        {
            //���N���b�N�����Ƃ��Ɏ��s
            //�U���ɕK�v��AP������Ă��違���͎��̏�Ԃ��U�����ł͂Ȃ�
            if (Input.GetMouseButton(0) && !input && currentAp >= useAp && !isAttack) 
            {
                //���̓t���O
                input = true;
                //�U�����t���O
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
        }

        if (!Input.GetMouseButton(0)) 
        {
            input = false;
        }
        
    }
    //�g�p���팈��
    void InitializeWeapon()
    {
        if (weapon == (int)Weapon.Knife)
        {
            attack = 9.5f;
            useAp = 15.0f;
            notAttack = 0.6f;
        }
        else if(weapon == (int)Weapon.Sword)
        {
            attack = 14.5f;
            useAp = 25.0f;
            notAttack = 0.5f;
        }
        else if(weapon == (int)Weapon.Knuckle)
        {
            attack = 7.5f;
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
            case (int)Weapon.Knife: return "knife";
            case (int)Weapon.Sword: return "sword";
            case (int)Weapon.Knuckle: return "knuckle";
            default: return "";
        }
    }

    //�X�e�[�^�X�ω�
    void RandomSkill()
    {
        //�ꎞ�I�ɃX�e�[�^�X�������l�ɂ���
        maxHp = firstMaxHp;
        currentHp = firstNowHp;
        maxAp = firstMaxAp;
        currentAp = firstNowAp;
        speed = firstSpeed;
        attack = firstAttack;
        damage = firstDamage;
        useAp = firstUseAp;

        if (skill <= 20)//AP2�{
        {
            maxAp *= 2.0f;
            currentAp *= 2.0f;
        }
        else if (skill <= 40)//HP2�{
        {
            maxHp *= 2.0f;
            currentHp *= 2.0f;
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
        else if (skill <= 95)//��_���[�W1.5�{�E�^�_���[�W0.75�{
        {
            damage *= 1.5f;
            attack *= 0.75f;
        }
        else if (skill <= 100)//��_���[�W0.5�{�E�^�_���[�W2�{
        {
            damage /= 2.0f;
            attack *= 2.0f;
        }
    }
    //�W�����v�̏o�n�ߏ�Ԃ���������
    void FirstCancel()
    {
        jumpFirst = false;
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
    //����̓����蔻����o��
    void IsAttack()
    {
        weaponCollider[weapon].enabled = true;
    }
    //����̓����蔻�������
    void HitWeapon()
    {
        weaponCollider[weapon].enabled = false;
    }
    //���S����
    public void Death()
    {
        speed = 0.0f;
        animator.SetTrigger("death");
    }
}
