using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ObjectMove : MonoBehaviour
{
    //コンポーネント管理
    public Animator animator;  //アニメーション
    public float move;         //アニメーション管理
    public NavMeshAgent agent; //移動範囲設定
    public Rigidbody rb;       //重力設定

    //基本能力
    public float maxHp;      //最大のHP
    public float currentHp;  //現在のHP
    public float damage;     //受けるダメージ

    //フラグ管理
    public bool isAttack;  //攻撃中
    public bool isDamage;  //被弾確認
    public bool death;     //死亡フラグ
    public bool isStop;    //一時的に動きを止める

    //Sliderを入れる                             
    public Slider hpSlider;  //HPバー

    // ゲームの状態を管理
    public GameManager gameManager;            //GameManagerの情報取得
    public PlayerController playerController;  //PlayerControllerの情報取得

    //武器の種類
    public enum Weapon{ Knife, Sword, Knuckle }

    // Start is called before the first frame update
    public void Start()
    {
        isDamage = false;
        death = false;

        //Sliderを満タンにする。
        hpSlider.value = 1;
    }

    // Update is called once per frame
    public void Update()
    {
        //HPの制御
        if (currentHp <= 0.0f)
            currentHp = 0.0f;

        //被弾か攻撃すると、一時的に移動を止める
        if (isAttack || isDamage)
        {
            agent.speed = 0;
        }
    }

    //ダメージ中フラグをfalseにする
    public void NotDamage()
    {
        isDamage = false;
    }
    //移動禁止フラグをfalseにする
    public void CanMove()
    {
        isStop = false;
    }
    //攻撃中フラグをfalseにする
    public void NotAttack()
    {
        isAttack = false;
    }
}
