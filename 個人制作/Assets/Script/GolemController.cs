using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshagentを使う
using UnityEngine.UI;

public class GolemController : MonoBehaviour
{
    private Animator animator;
    public Transform player;      //プレイヤーの位置
    private NavMeshAgent agent;

    public Slider hpSlider;       //HPバー
    public float maxHp;           //最大のHP
    public float currentHp;       //現在のHP
    bool death;                   //死亡フラグ
    bool isDamage = false;        //ダメージ中フラグ
    bool attack = false;          //攻撃フラグ
    bool isAttack = false;        //攻撃フラグ
    private int move;            //アニメーション管理用
    int chaseTime;

    private AudioSource sound = null;

    //ダメージSE
    public AudioClip knife_SE;
    public AudioClip sword_SE;
    public AudioClip Knuckle_SE;

    //public Collider weaponCollider;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        //Sliderを満タンにする。
        hpSlider.value = 1;
        //現在の値を最大値と同じにする
        currentHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        move = 1;
        // 対象のオブジェクトを追いかける
        agent.destination = player.transform.position;

        animator.SetFloat("BossMove", move, 0.1f, Time.deltaTime);
    }
}
