using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Quaternion targetRotation;
    Rigidbody player_rb;
    Vector3 pos;

    //基礎能力
    public int hp = 0;//体力
    public int ap = 0;//アタックポイント

    //攻撃関連
    public int weapon = 0;         //攻撃手段  
    public int skill = 0;         //付与する効果
    public bool interval = false;  //クールタイム中かどうか
    bool input = false;  //長押し防止

    void Awake()
    {
        //初期化
        TryGetComponent(out animator);
        targetRotation = transform.rotation;
        player_rb = GetComponent<Rigidbody>();
        pos = transform.position;
        weapon = Random.Range(1, 3);
        skill = Random.Range(1, 10);

        switch (weapon)
        {
            case 1:
                Debug.Log("ナックル");
                break;
            case 2:
                Debug.Log("ナイフ");
                break;
            case 3:
                Debug.Log("ソード");
                break;
            default:
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Move3D();

        Attack();
    }

    //移動関連
    void Move3D()
    {
        //入力ベクトルの取得
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        var velocity = horizontalRotation * new Vector3(horizontal, 0, vertical).normalized;

        //速度の取得
        var speed = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
        var rotationSpeed = 600 * Time.deltaTime;

        pos += velocity / 50 * speed;
        transform.position = pos;

        if (velocity.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

        //移動速度をAnimatorに反映
        animator.SetFloat("Speed", velocity.magnitude * speed, 0.1f, Time.deltaTime);
    }

    //攻撃関連
    void Attack()
    {
        // 左ボタンが押されていたら実行
        if (Input.GetMouseButton(0) && !interval && !input)
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
                default:
                    break;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            input = false;
        }
    }

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

    void Interval()
    {
        Debug.Log("攻撃可能です");
        interval = false;
    }

    //付与効果
    void RandomSkill()
    {

    }
}
