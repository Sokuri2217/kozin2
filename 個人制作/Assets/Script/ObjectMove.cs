using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ObjectMove : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public AudioSource se;
    public Rigidbody rb;

    //基本能力
    public float maxHp; //最大のHP
    public float currentHp;      //現在のHP

    public bool isAttack;       //攻撃中
    public float speed;         //移動速度

    public float damage;              //受けるダメージ
    public bool isDamage;             //被弾確認

    public bool death;                //死亡フラグ

    public bool isStop;               //ダメージを受けると一時的に動きを止める

    //Sliderを入れる                             
    public Slider hpSlider; //HPバー

    // ゲームの状態を管理
    public GameManager gameManager;
    public PlayerController playerController;

    //武器の種類
    public enum Weapon{ Knife, Sword, Knuckle }

    // Start is called before the first frame update
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        se = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

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
