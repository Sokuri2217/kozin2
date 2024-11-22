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

    //基礎能力
    public float maxHp = 100;     //最大のHP
    public float currentHp;       //現在のHP
    public float maxAp = 100;     //最大のAP
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
    //ダメージ関連
    public float damage;          //受けるダメージ
    public bool isDamage;         //被弾確認
    float currentTime = 0.0f;     //現在の時間取得
    public int kill_enemy;        //倒した敵数
    public int goalspawn;         //ゴール出現に必要な敵数
    //Sliderを入れる
    public Slider hpSlider;       //HPバー
    public Slider apSlider;       //Apバー
    //武器
    public GameObject knife;      //ナイフ
    public GameObject sword;      //ソード
    public GameObject spear;      //スピアー
    //武器の当たり判定
    public Collider knifeCollider;
    public Collider swordCollider;
    public Collider spearCollider;

    void Awake()
    {
        //初期化
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

        //攻撃手段を分岐
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

        //Sliderを満タンにする。
        hpSlider.value = 1;
        apSlider.value = 1;
        //現在の値を最大値と同じにする
        currentHp = maxHp;
        currentAp = maxAp;

    }
    // Update is called once per frame
    void Update()
    {
        GameManager gameManager = GetComponent<GameManager>();
        //プレイ中のみ動く
        if (gameManager.gamePlay)
        {
            if(!isAttack)
                Move3D();  //移動

            Attack();  //攻撃

            //キルカウントの制御
            if(kill_enemy>=5)
            {
                kill_enemy = 5;
            }

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
        }

        if (currentAp < use_Ap)
        {
            apLost = true;
        }
        else
        {
            apLost = false;
        }

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
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

        animator.SetFloat("Speed", velocity.magnitude * move, 0.1f, Time.deltaTime);
    }

    //攻撃関連
    void Attack()
    {
        //左クリックしたときに実行
        if (Input.GetMouseButton(0) && !interval && !input && currentAp >= use_Ap)  
        {
            //クールタイムフラグ
            interval = true;
            //入力フラグ
            input = true;
            isAttack = true;

            //攻撃処理
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
            //現在のAPから消費APを引く
            currentAp = currentAp - use_Ap;
            Invoke("NotAttack", notAttack);
        }
        else
        {
            knifeCollider.enabled = false;
            swordCollider.enabled = false;
            spearCollider.enabled = false;
        }

        //長押し禁止用
            if (Input.GetMouseButtonUp(0))
        {
            input = false;
        }
    }

    //武器
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
    //クールタイム
    void Interval()
    {
        Debug.Log("攻撃可能です");
        interval = false;
    }

    //付与効果
    void RandomSkill()
    {
        if(skill>=1&&skill<=20)//AP2倍
        {
            maxAp *= 2;
            Debug.Log("AP2倍");
        }
        else if (skill >= 21 && skill <= 40)//HP2倍
        {
            maxHp *= 2;
            Debug.Log("HP2倍");
        }
        else if (skill >= 41 && skill <= 50)//攻撃力2倍
        {
            attack *= 2.0f;
            Debug.Log("攻撃力2倍");
        }
        else if (skill >= 51 && skill <= 60)//被ダメージ2倍
        {
            damage *= 2.0f;
            Debug.Log("被ダメージ2倍");
        }
        else if (skill >= 61 && skill <= 70)//移動1.5倍・攻撃力0.75倍
        {
            speed *= 0.75f;
            attack   *= 0.75f;
            Debug.Log("移動1.5倍・攻撃力0.75倍");
        }
        else if (skill >= 71 && skill <= 80)//移動0.75倍・攻撃力1.5倍
        {
            speed *= 1.5f;
            attack *= 1.5f;
            Debug.Log("移動0.75倍・攻撃力1.5倍");
        }
        else if (skill >= 81 && skill <= 90)//消費AP・攻撃力2倍
        {
            use_Ap *= 2.0f;
            attack *= 2.0f;
            Debug.Log("消費AP・攻撃力2倍");
        }
        else if (skill >= 91 && skill <= 95)//被ダメージ2倍・与ダメージ0.5倍
        {
            damage *= 2.0f;
            attack *= 0.5f;
            Debug.Log("被ダメージ2倍・与ダメージ0.5倍");
        }
        else if (skill >= 96 && skill <= 100)//被ダメージ0.5倍・与ダメージ2倍
        {
            damage *= 0.5f;
            attack *= 2.0f;
            Debug.Log("被ダメージ0.5倍・与ダメージ2倍");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Wall")
        {
            Debug.Log("壁当たった");
        }
    }

    void OnTriggerStay(Collider other)
    {
        //Goalタグのオブジェクトに触れると発動
        if (other.CompareTag("Goal"))
        {
            GameManager gameManager = GetComponent<GameManager>();

            gameManager.gameClear = true;
            gameManager.gamePlay = false;
        }

        //Enemyタグのオブジェクトに触れると発動
        if (other.CompareTag("Enemy") && !isDamage) 
        {
            //現在のHPからダメージを引く
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
