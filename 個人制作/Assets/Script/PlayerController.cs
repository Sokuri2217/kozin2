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
    //最大HPと現在のHP
    public float maxHp = 100;
    public float currentHp;
    //最大APと現在のAP
    public float maxAp = 100;
    public float currentAp;
    public float use_Ap;//消費AP
    //移動関連
    float horizontal;
    float vertical;
    Quaternion horizontalRotation;
    public Vector3 velocity;
    float speed;
    //Sliderを入れる
    public Slider hpSlider;
    public Slider apSlider;

    //攻撃関連
    public int weapon = 0;       //攻撃手段  
    public int skill = 0;        //付与する効果
    public bool interval = false;//クールタイム中かどうか
    bool input = false;          //長押し防止
    public float attack;        //攻撃力

    //ダメージ関連
    public float damage = 10.0f;

    void Awake()
    {
        //初期化
        TryGetComponent(out animator);
        targetRotation = transform.rotation;
        player_rb = GetComponent<Rigidbody>();
        pos = transform.position;
        weapon = Random.Range(1, 3);
        skill = Random.Range(1, 100);

        //Sliderを満タンにする。
        hpSlider.value = 1;
        apSlider.value = 1;
        //現在の値を最大値と同じにする
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

    //移動関連
    void Move3D()
    {
        //入力ベクトルの取得
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        velocity = horizontalRotation * new Vector3(horizontal, 0, vertical).normalized;

        //速度の取得
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

        //移動速度をAnimatorに反映
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

            //攻撃手段を分岐
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
        //長押し禁止用
        if (Input.GetMouseButtonUp(0))
        {
            input = false;
        }
    }
    //武器
    void Knuckle()
    {
        GetComponent<Animator>().SetTrigger("knuckle");
        attack = 5.0f;
        use_Ap = 10.0f;
        Invoke("Interval", 1.0f);
        //現在のAPから消費APを引く
        currentAp = currentAp - use_Ap;
        //最大APにおける現在のAPをSliderに反映。
        apSlider.value = currentAp / maxAp;
    }
    void Knife()
    {
        GetComponent<Animator>().SetTrigger("knife");
        attack = 10.0f;
        use_Ap = 25.0f;
        Invoke("Interval", 4.0f);
        //現在のAPから消費APを引く
        currentAp = currentAp - use_Ap;
        //最大APにおける現在のAPをSliderに反映。
        apSlider.value = currentAp / maxAp;
    }
    void Sword()
    {
        GetComponent<Animator>().SetTrigger("sword");
        attack = 20.0f;
        use_Ap = 50.0f;
        Invoke("Interval", 7.0f);
        //現在のAPから消費APを引く
        currentAp = currentAp - use_Ap;
        //最大APにおける現在のAPをSliderに反映。
        apSlider.value = currentAp / maxAp;
    }
    //クールタイム
    void Interval()
    {
        Debug.Log("攻撃可能です");
        interval = false;
    }
    //AP関連
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
            speed *= 1.5f;
            attack   *= 0.75f;
            Debug.Log("移動1.5倍・攻撃力0.75倍");
        }
        else if (skill >= 71 && skill <= 80)//移動0.75倍・攻撃力1.5倍
        {
            speed *= 0.75f;
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
            use_Ap *= 0.5f;
            attack *= 2.0f;
            Debug.Log("被ダメージ0.5倍・与ダメージ2倍");
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        //Enemyタグのオブジェクトに触れると発動
        if (collider.gameObject.tag == "Enemy")
        {
            //現在のHPからダメージを引く
            currentHp = currentHp - damage;

            //最大HPにおける現在のHPをSliderに反映。
            hpSlider.value = currentHp / maxHp;
        }
    }
}
