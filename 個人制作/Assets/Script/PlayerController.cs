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
    bool input = false;           //長押し防止
    public float attack;          //攻撃力
    //ダメージ関連
    public float damage;          //受けるダメージ
    float currentTime = 0.0f;     //現在の時間取得
    public int kill_enemy;        //倒した敵数

    //Sliderを入れる
    public Slider hpSlider;       //HPバー
    public Slider apSlider;       //Apバー
    //武器
    public GameObject knife;      //ナイフ
    public GameObject sword;      //ソード
    public GameObject spear;      //スピアー

    void Awake()
    {
        //初期化
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
            Move3D();  //移動

            Attack();  //攻撃

            if (Input.GetMouseButton(1))
            {
                kill_enemy++;
            }

            //APの自動回復
            if (currentAp < maxAp)
            {
                currentTime += Time.deltaTime;

                if (currentTime >= 1.0f)
                {
                    currentAp += 1.0f;
                    currentTime = 0.0f;
                }
            }
            //現在のAPが消費APより少ないと攻撃できない
            if (currentAp < use_Ap) 
            {
                interval = true;
            }
            else
            {
                interval = false;
            }

            //移動速度をAnimatorに反映
            animator.SetFloat("Speed", velocity.magnitude * move, 0.1f, Time.deltaTime);
            //最大HPにおける現在のHPをSliderに反映
            hpSlider.value = currentHp / maxHp;
            //最大APにおける現在のAPをSliderに反映。
            apSlider.value = currentAp / maxAp;
        }
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

        pos += velocity / speed * move;
        transform.position = pos;

        if (velocity.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
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

            //攻撃処理
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

            //現在のAPから消費APを引く
            currentAp = currentAp - use_Ap;
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

    //void OnCollisionStay(Collision collision)
    //{
    //    //Enemyタグのオブジェクトに触れると発動
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        //現在のHPからダメージを引く
    //        currentHp = currentHp - damage;

    //        //最大HPにおける現在のHPをSliderに反映
    //        hpSlider.value = currentHp / maxHp;
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        //Goalタグのオブジェクトに触れると発動
        if (other.CompareTag("Goal"))
        {
            GameManager gameManager = GetComponent<GameManager>();

            gameManager.gameClear = true;
            gameManager.gamePlay = false;
        }

        //Enemyタグのオブジェクトに触れると発動
        if (other.CompareTag("Enemy"))
        {
            //現在のHPからダメージを引く
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
