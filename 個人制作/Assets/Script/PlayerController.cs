using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : ObjectMove
{   
    //基礎能力
    public float maxAp;        //最大のAP
    public float currentAp;    //現在のAP

    //初回ステータス保存用
    public float firstMaxHp;    
    public float firstNowHp;    
    public float firstMaxAp;    
    public float firstNowAp;    
    public float firstSpeed;
    public float firstAttack;
    public float firstDamage;
    public float firstUseAp;

    //移動関連
    private float horizontal;              //横移動
    private float vertical;                //縦移動
    private Quaternion horizontalRotation; //向き取得
    private Vector3 velocity;              //ベクトル取得
    private Quaternion targetRotation;     //向きの回転
    public  float speed;                   //移動速度
    private float rotationSpeed;           //向きを変える速度7
    private bool isJump;                   //ジャンプ中
    public bool jumpFirst;                 //ジャンプの出始めで中断されない様にするフラグ
    public float jumpPower;                //ジャンプ力

    //攻撃関連                                   
    public int weapon;       //攻撃手段  
    public int skill;        //付与する効果
    public float attack;     //攻撃力
    private float notAttack; //動けるようになるまでの時間

    //回復倍率
    public float healHp; //HP回復倍率
    public float healAp; //AP回復倍率

    //AP関連
    public float useAp;               //消費AP
    public bool apLost;               //攻撃に必要なApが残っているかどうか
    private float currentTime = 0.0f; //現在の時間取得

    //武器関連                                
    public GameObject[] useWeapon;    //使用中の武器オブジェクト                     
    public Collider[] weaponCollider; //武器の当たり判定

    //ゲージ    
    public Slider apSlider; //Apバー

    //音を鳴らす
    public AudioClip damageSe;      //被弾
    public AudioClip powerDamageSe; //被ダメージ上昇中の被弾
    public AudioClip itemGet;       //アイテム獲得

    //入力関連
    //長押し防止
    private bool input;       //攻撃

    //UI情報取得
    public MainUIScript mainUI;

    new void Start()
    {
        GameManager gameManager = GetComponent<GameManager>();

        //初期化
        targetRotation = transform.rotation;
        weapon = Random.Range(0, 3);
        skill = Random.Range(1, 100);
        damage = 5.0f;
        // 武器の初期化
        for (int i = 0; i < 3; i++)
        {
            useWeapon[i].SetActive(false);
            weaponCollider[i].enabled = false;
        }
        //使用武器決定
        InitializeWeapon();

        //初期値保存
        firstMaxHp = maxHp;
        firstMaxAp = maxAp;
        firstSpeed = speed;
        firstAttack = attack;
        firstDamage = damage;
        firstUseAp = useAp;

        //ステータス変化
        RandomSkill();

        //Sliderを満タンにする。
        apSlider.value = 1;
        //現在の値を最大値と同じにする
        currentHp = maxHp;
        currentAp = maxAp;
    }

    //プレイヤーの基本操作
    //ステータス変化
    private new void Update()
    {
        //死亡時もしくは攻撃中、被弾時以外
        if(!death)
        {
            if (!isAttack && !isStop)
            {
                Jump3D();
                Move3D();
            }
            //攻撃用関数
            Attack();

            if(mainUI.changeInput)
            {
                skill = Random.Range(1, 100);
                RandomSkill();
                mainUI.changeInput = false;
            }
        }
    }

    //AP管理と死亡処理
    private void FixedUpdate()
    {
        firstNowHp = currentHp;
        firstNowAp = currentAp;

        //死亡処理
        if (currentHp <= 0.0f && !gameManager.gameOver)
        {
            gameManager.gameOver = true;
            death = true;
            Death();
        }

        //APの自動回復関数
        AutoRegenAP();
        //現在のHPが上限を超えないようにする
        if (currentHp >= maxHp)
        {
            currentHp = maxHp;
        }
        //現在のAPが上限を超えないようにする
        if (currentAp >= maxAp) 
        {
            currentAp = maxAp;
        }

        //最大HPにおける現在のHPをSliderに反映
        hpSlider.value = currentHp / maxHp;
        //最大APにおける現在のAPをSliderに反映
        apSlider.value = currentAp / maxAp;
    }

    private void OnTriggerEnter(Collider other)
    {
        //敵の攻撃に当たる
        if(!isDamage && gameManager.gamePlay)
        {
            if (other.CompareTag("enemyweapon")||
                other.CompareTag("nearweapon"))
            {
                currentHp -= damage;   //現在のHPからダメージを引く

                if (currentHp > 0.0f)
                {
                    //被弾アニメーション再生
                    animator.SetTrigger("damage");
                    //ダメージSEを鳴らす
                    if (damage > firstDamage) 
                    {
                        se.PlayOneShot(powerDamageSe);
                    }
                    else
                    {
                        se.PlayOneShot(damageSe);
                    }
                    //無敵時間
                    Invoke("NotDamage", 1.1f);
                    //被弾してから動けるようになるまでの時間
                    Invoke("CanMove", 0.5f);
                }

                isDamage = true;       //ダメージ中状態にする
                isStop = true;         //その場で停止させる
                HitWeapon();
            }
        }

        //回復アイテムに触れる
        if(other.CompareTag("healItem"))
        {
            currentHp += maxHp * healHp;
            currentAp += maxAp * healAp;
            //取得音を鳴らす
            se.PlayOneShot(itemGet);
            //アイテムを消去する
            Destroy(other.gameObject);
        }

        if (other.CompareTag("allHeal"))
        {
            currentHp = maxHp;
            currentAp = maxAp;
            //取得音を鳴らす
            se.PlayOneShot(itemGet);
            //アイテムを消去する
            Destroy(other.gameObject);
        }

        //地面に触れる
        if (other.CompareTag("Ground") && !jumpFirst) 
        {
            rb.isKinematic = true;
            agent.enabled = true;
            isJump = false;
        }

        //Goalに触れる
        if (other.CompareTag("Goal"))
        {
            speed = 0.0f;
            gameManager.gameClear = true;
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
        transform.position += velocity * Time.deltaTime * move * speed;

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
            rb.isKinematic = false;
            agent.enabled = false;
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            isJump = true;
            jumpFirst = true;
            Invoke("FirstCancel", 0.015f);
        }
    }

    //攻撃処理
    void Attack()
    {
        if(!isStop)
        {
            //左クリックしたときに実行
            //攻撃に必要なAPが足りている＆入力時の状態が攻撃中ではない
            if (Input.GetMouseButton(0) && !input && currentAp >= useAp && !isAttack) 
            {
                //入力フラグ
                input = true;
                //攻撃中フラグ
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
        }

        if (!Input.GetMouseButton(0)) 
        {
            input = false;
        }
        
    }
    //使用武器決定
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
    //攻撃アニメーション
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

    //ステータス変化
    void RandomSkill()
    {
        //一時的にステータスを初期値にする
        maxHp = firstMaxHp;
        currentHp = firstNowHp;
        maxAp = firstMaxAp;
        currentAp = firstNowAp;
        speed = firstSpeed;
        attack = firstAttack;
        damage = firstDamage;
        useAp = firstUseAp;

        if (skill <= 20)//AP2倍
        {
            maxAp *= 2.0f;
            currentAp *= 2.0f;
        }
        else if (skill <= 40)//HP2倍
        {
            maxHp *= 2.0f;
            currentHp *= 2.0f;
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
        else if (skill <= 95)//被ダメージ1.5倍・与ダメージ0.75倍
        {
            damage *= 1.5f;
            attack *= 0.75f;
        }
        else if (skill <= 100)//被ダメージ0.5倍・与ダメージ2倍
        {
            damage /= 2.0f;
            attack *= 2.0f;
        }
    }
    //ジャンプの出始め状態を解除する
    void FirstCancel()
    {
        jumpFirst = false;
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
    //武器の当たり判定を出す
    void IsAttack()
    {
        weaponCollider[weapon].enabled = true;
    }
    //武器の当たり判定を消す
    void HitWeapon()
    {
        weaponCollider[weapon].enabled = false;
    }
    //死亡処理
    public void Death()
    {
        speed = 0.0f;
        animator.SetTrigger("death");
    }
}
