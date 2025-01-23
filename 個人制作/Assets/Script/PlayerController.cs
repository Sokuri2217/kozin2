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

    //基礎能力
    public float maxHp = 100.0f; //最大のHP
    public float currentHp;      //現在のHP
    public float maxAp = 100.0f; //最大のAP
    public float currentAp;      //現在のAP
    public float useAp;          //消費AP

    //移動関連
    private float horizontal;              //横移動
    private float vertical;                //縦移動
    private Quaternion horizontalRotation; //向き取得
    private Vector3 velocity;              //ベクトル取得
    private Quaternion targetRotation;     //向きの回転
    public float speed;                    //移動速度
    private float move;                    //歩き、走りの切り替え
    private float rotationSpeed;           //向きを変える速度
    private bool isJump;                   //ジャンプ中
    public float jumpPower;                //ジャンプ力

    //攻撃関連                                   
    public int weapon = 0;       //攻撃手段  
    public int skill = 0;        //付与する効果
    public bool apLost;          //攻撃に必要なApが残っているかどうか
    private bool input;          //長押し防止
    public float attack;         //攻撃力
    private bool isAttack;       //攻撃中
    private float notAttack = 0; //動けるようになるまでの時間

    //ダメージ関連                               
    public float damage;              //受けるダメージ
    private bool isDamage;            //被弾確認
    private float currentTime = 0.0f; //現在の時間取得
    public bool isStop;               //ダメージを受けると一時的に動きを止める
    public bool death;                //死亡フラグ

    //Sliderを入れる                             
    public Slider hpSlider; //HPバー
    public Slider apSlider; //Apバー

    //武器                                       
    public GameObject[] useWeapon; //使用中の武器

    //武器の当たり判定                          
    public Collider[] weaponCollider; //武器のコライダー

    //使用武器判定用
    public enum Weapon { KNIFE, SWORD, KNUCKLE }

    void Start()
    {
        //初期化
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
        // 武器の初期化
        for (int i = 0; i < 3; i++)
        {
            useWeapon[i].SetActive(false);
            weaponCollider[i].enabled = false;
        }
        //使用武器決定
        InitializeWeapon();
        //ステータス変化
        RandomSkill();

        //Sliderを満タンにする。
        hpSlider.value = 1;
        apSlider.value = 1;
        //現在の値を最大値と同じにする
        currentHp = maxHp;
        currentAp = maxAp;

    }

    //プレイヤーの基本操作
    private void Update()
    {
        if(!death)
        {
            if (!isAttack && !isStop)
            {
                //攻撃中はその場から移動できない
                Jump3D();
                //攻撃中はその場から移動できない
                Move3D();
            }
            //攻撃に必要なAPが足りている＆入力時の状態が攻撃中ではない
            if (currentAp >= useAp && !isAttack && !isStop) 
            {
                //攻撃用関数
                Attack();
            }
        }
    }


    private void FixedUpdate()
    {
        GameManager gameManager = GetComponent<GameManager>();

        //死亡処理
        if (currentHp <= 0.0f && !gameManager.gameOver)  
        {
            gameManager.gameOver = true;
            Death();
        }

        //HPの制御
        if (currentHp <= 0.0f)
            currentHp = 0.0f;

        //APの自動回復関数
        AutoRegenAP();

        //残りAPが0になったらフラグをたてる
        if (currentAp < useAp) apLost = true;
        else apLost = false;

        //最大HPにおける現在のHPをSliderに反映
        hpSlider.value = currentHp / maxHp;
        //最大APにおける現在のAPをSliderに反映
        apSlider.value = currentAp / maxAp;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager gameManager = GetComponent<GameManager>();

        //敵の攻撃に当たる
        if (other.CompareTag("enemyweapon") && !isDamage && gameManager.gamePlay)
        {
            currentHp -= damage;   //現在のHPからダメージを引く
            isDamage = true;       //ダメージ中状態にする
            isStop = true;         //その場で停止させる
            HitWeapon();
            if (currentHp > 0.0f)
            {
                //被弾アニメーション再生
                animator.SetTrigger("damage");
                //ダメージSEを鳴らす
                se.PlayOneShot(damage_se);
                //無敵時間
                Invoke("NotDamage", 1.5f);
                //被弾してから動けるようになるまでの時間
                Invoke("CanMove", 0.5f);
            }
        }

        //Goalタグのオブジェクトに触れると発動
        if (other.CompareTag("Goal"))
        {
            speed = 0.0f;
            gameManager.gameClear = true;
        }
    }

    //着地したらジャンプ中フラグをfalseにする
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
    //移動処理
    void Move3D()
    {
        //入力ベクトルの取得
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        velocity = horizontalRotation * new Vector3(horizontal, 0, vertical).normalized;

        //速度の取得
        //スニーク
        if (Input.GetKey(KeyCode.LeftControl))
            move = 1;
        //走る
        else if (Input.GetKey(KeyCode.LeftShift))
            move = 3;
        //歩く
        else
            move = 2;

        rotationSpeed = 600 * Time.deltaTime;
        transform.position += velocity * Time.deltaTime * speed * move;

        if (velocity.magnitude > 0.5f)
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

        animator.SetFloat("Speed", velocity.magnitude * move, 0.1f, Time.deltaTime);
    }

    //ジャンプ処理
    void Jump3D()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
        {
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            isJump = true;
        }
    }

    //攻撃処理
    void Attack()
    {
        //左クリックしたときに実行
        if (Input.GetMouseButton(0) && !input)
        {
            //入力フラグ
            input = true;
            ////攻撃中フラグ
            isAttack = true;
            //攻撃中フラグをtrueにする
            IsAttack();
            //アニメーション再生
            animator.SetTrigger(GetWeaponAttackTrigger());
            //現在のAPから消費APを引く
            currentAp -= useAp;

            //フラグ変更
            Invoke("NotAttack", notAttack);
            Invoke("HitWeapon", 0.5f);
        }

        //長押し禁止用
        if (Input.GetMouseButtonUp(0))
            input = false;
    }
    //使用武器決定
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
    //攻撃アニメーション
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

    //ステータス変化
    void RandomSkill()
    {
        if (skill <= 20)//AP2倍
        {
            maxAp *= 2.0f;
        }
        else if (skill <= 40)//HP2倍
        {
            maxHp *= 2.0f;
        }
        else if (skill <= 50)//攻撃力2倍
        {
            attack *= 2.0f;
        }
        else if (skill <= 60)//被ダメージ2倍
        {
            damage *= 2.0f;
        }
        else if (skill <= 70)//移動1.5倍・攻撃力0.75倍
        {
            speed *= 1.5f;
            attack *= 0.75f;
        }
        else if (skill <= 80)//移動0.75倍・攻撃力1.5倍
        {
            speed *= 0.75f;
            attack *= 1.5f;
        }
        else if (skill <= 90)//消費AP2倍・攻撃力3倍
        {
            useAp *= 2.0f;
            attack *= 3.0f;
        }
        else if (skill <= 95)//被ダメージ2倍・与ダメージ0.5倍
        {
            damage *= 2.0f;
            attack *= 0.5f;
        }
        else if (skill <= 100)//被ダメージ0.5倍・与ダメージ2倍
        {
            damage /= 2.0f;
            attack *= 2.0f;
        }
    }

    //AP自動回復
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

    //ダメージ中フラグをfalseにする
    void NotDamage()
    {
        isDamage = false;
    }
    //移動禁止フラグをfalseにする
    void CanMove()
    {
        isStop = false;
    }
    //武器の当たり判定を出す
    void IsAttack()
    {
        weaponCollider[weapon].enabled = true;
    }
    //攻撃中フラグをfalseにする
    void NotAttack()
    {
        isAttack = false;
    }
    //武器の当たり判定を消す
    void HitWeapon()
    {
        weaponCollider[weapon].enabled = false;
    }
    //死亡処理
    void Death()
    {
        speed = 0.0f;
        animator.SetTrigger("death");
    }
}
