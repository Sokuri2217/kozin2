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

    //基礎能力
    public float maxHp = 100.0f;  //最大のHP
    public float currentHp;       //現在のHP
    public float maxAp = 100.0f;  //最大のAP
    public float currentAp;       //現在のAP
    public float use_Ap;          //消費AP
    //移動関連
    float horizontal;             //横移動
    float vertical;               //縦移動
    Quaternion horizontalRotation;//向き取得
    Vector3 velocity;             //ベクトル取得
    float speed;                  //移動速度
    float move;                   //歩き、走りの切り替え
    float rotationSpeed;          //向きを変える速度
    //攻撃関連
    public int weapon = 0;        //攻撃手段  
    public int skill = 0;         //付与する効果
    public bool interval = false; //クールタイム中かどうか
    public bool apLost = false;   //攻撃に必要なApが残っているかどうか
    bool input = false;           //長押し防止
    public float attack;          //攻撃力
    public bool isAttack = false; //攻撃中
    public float notAttack = 0;   //動けるようになるまでの時間
    public float hitWeapon = 0.0f;//当たり判定表示時間
    //ダメージ関連
    public float damage;          //受けるダメージ
    public bool isDamage;         //被弾確認
    float currentTime = 0.0f;     //現在の時間取得
    public int kill_enemy;        //倒した敵数
    public int goalspawn;         //ゴール出現に必要な敵数
    public bool isStop;           //ダメージを受けると一時的に動きを止める
    public bool death;
    //Sliderを入れる
    public Slider hpSlider;       //HPバー
    public Slider apSlider;       //Apバー
    //武器
    public GameObject[] use_weapon;     //使用中の武器
    int weapon_num;
    //武器の当たり判定
    public Collider[] weaponCollider;   //武器のコライダー

    void Start()
    {
        //初期化
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

        //攻撃手段を分岐
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

        //追加効果付与
        RandomSkill();

        //Sliderを満タンにする。
        hpSlider.value = 1;
        apSlider.value = 1;
        //現在の値を最大値と同じにする
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

        //攻撃中はその場から移動できない
        if (!isAttack && !isStop && !death)  
        {
            Move3D();
        }

        if(currentAp >= use_Ap)
        {
            //攻撃用関数
            Attack();
        }

        //キルカウントの制御
        if (kill_enemy >= 5)
            kill_enemy = 5;

        //HPの制御
        if (currentHp <= 0.0f)
            currentHp = 0.0f;

        //APの自動回復
        if (currentAp < maxAp)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= 2.0f)
            {
                currentAp += 5.0f;
                currentTime = 0.0f;
            }
        }
        //残りAPが0になったらフラグをたてる
        if (currentAp < use_Ap) apLost = true;
        else apLost = false;

        //最大HPにおける現在のHPをSliderに反映
        hpSlider.value = currentHp / maxHp;
        //最大APにおける現在のAPをSliderに反映。
        apSlider.value = currentAp / maxAp;
    }

    //移動関連
    void Move3D()
    {
        //入力ベクトルの取得
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        velocity = horizontalRotation * new Vector3(horizontal, 0, vertical).normalized;

        //速度の取得
        move = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
        rotationSpeed = 600 * Time.deltaTime;
        transform.position += velocity * Time.deltaTime * speed * move;

        if (velocity.magnitude > 0.5f) 
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

        animator.SetFloat("Speed", velocity.magnitude * move, 0.1f, Time.deltaTime);
    }

    //攻撃関連
    void Attack()
    {
        //左クリックしたときに実行
        if (Input.GetMouseButton(0) && !interval && !input)  
        {
            //クールタイムフラグ
            interval = true;
            //入力フラグ
            input = true;
            ////攻撃中フラグ
            isAttack = true;
            //攻撃処理
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
            //現在のAPから消費APを引く
            currentAp = currentAp - use_Ap;
            Invoke("NotAttack", notAttack);
            Invoke("HitWeapon", hitWeapon);
        }
        //長押し禁止用
        if (Input.GetMouseButtonUp(0))
            input = false;
    }

    //武器
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
    //クールタイム
    void Interval()
    {
        interval = false;
    }

    //付与効果
    void RandomSkill()
    {
        if(skill>=1&&skill<=20)//AP2倍
        {
            maxAp *= 2.0f;
        }
        else if (skill >= 21 && skill <= 40)//HP2倍
        {
            maxHp *= 2.0f;
        }
        else if (skill >= 41 && skill <= 50)//攻撃力2倍
        {
            attack *= 2.0f;
        }
        else if (skill >= 51 && skill <= 60)//被ダメージ2倍
        {
            damage *= 2.0f;
        }
        else if (skill >= 61 && skill <= 70)//移動1.5倍・攻撃力0.75倍
        {
            speed *= 1.5f;
            attack *= 0.75f;
        }
        else if (skill >= 71 && skill <= 80)//移動0.75倍・攻撃力1.5倍
        {
            speed *= 0.75f;
            attack *= 1.5f;
        }
        else if (skill >= 81 && skill <= 90)//消費AP2倍・攻撃力3倍
        {
            use_Ap *= 2.0f;
            attack *= 3.0f;
        }
        else if (skill >= 91 && skill <= 95)//被ダメージ2倍・与ダメージ0.5倍
        {
            damage *= 2.0f;
            attack *= 0.5f;
        }
        else if (skill >= 96 && skill <= 100)//被ダメージ0.5倍・与ダメージ2倍
        {
            damage /= 2.0f;
            attack *= 2.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager gameManager = GetComponent<GameManager>();
        //Enemyタグのオブジェクトに触れると発動
        if (other.CompareTag("enemyweapon") && !isDamage && !death)  
        {
            //現在のHPからダメージを引く
            currentHp -= damage;
            //ダメージSEを鳴らす
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
        //Goalタグのオブジェクトに触れると発動
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
