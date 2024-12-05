using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshagentを使う
using UnityEngine.UI;


public class EnemyBear : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    public Transform[] goals;     //徘徊ポイント
    public Transform player;      //プレイヤーの位置
    public new Transform  camera; //カメラの位置
    private int destNum = 0;　　　//向かう場所
    public Slider hpSlider;       //HPバー
    public float maxHp = 30;      //最大のHP
    public float currentHp;       //現在のHP
    bool death = false;           //死亡フラグ
    bool isDamage = false;        //ダメージ中フラグ
    bool isStop = false;          //停止フラグ
    bool attack = false;          //攻撃フラグ
    bool isAttack = false;        //攻撃フラグ
    public bool isChase = false;  //追跡フラグ
    private float chaseTime = 0;  //追跡解除用のカウント
    private int speed;            //アニメーション管理用

    private AudioSource sound = null;
    public AudioClip knife_SE;
    public AudioClip sword_SE;
    public AudioClip Knuckle_SE;

    //public AudioSource main_Bgm;
    //public AudioSource battle_Bgm;

    public Collider weaponCollider;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goals[destNum].position;
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        weaponCollider.enabled = false;
        speed = 0;

        //Sliderを満タンにする。
        hpSlider.value = 1;
        //現在の値を最大値と同じにする
        currentHp = maxHp;
    }

    void nextGoal()
    {

        destNum = Random.Range(0, 3);

        agent.destination = goals[destNum].position;

        Debug.Log(destNum);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameManager gameManager;
        GameObject obj = GameObject.Find("Player");
        gameManager = obj.GetComponent<GameManager>();

        if (!isStop)
        {
            if (agent.remainingDistance < 0.5f && !isChase)  
            {
                speed = 1;
                nextGoal(); 
            }
        }
        if (isAttack)
        {
            agent.speed = 0;
        }
        else if (isChase && chaseTime < 3.0f)
        {
            speed = 2;
            agent.speed = 15;
            // 対象のオブジェクトを追いかける
            agent.destination = player.transform.position;
        }
        else if (chaseTime >= 3.0f) 
        {
            isChase = false;
            speed = 1;
            agent.speed = 2;
        }

        if ((transform.position.x - player.transform.position.x) < 1.0f &&
            (transform.position.z - player.transform.position.z) < 1.0f &&
            isChase && !attack && !isAttack) 
        {
            attack = true;
            isAttack = true;
            animator.SetTrigger("attack");
            
            Invoke("IsAttack", 0.1f);
            Invoke("NotAttack", 0.5f);
            Invoke("CanAttack", 2.0f);
            Invoke("NotWeapon", 0.3f);
        }

        //最大HPにおける現在のHPをSliderに反映
        hpSlider.value = currentHp / maxHp;

        if (currentHp <= 0.0f && !death) 
        {
            animator.SetTrigger("death");
            agent.speed = 0;
            death = true;
            Invoke("Death", 0.6f);
        }
        animator.SetFloat("EnemySpeed", speed, 0.1f, Time.deltaTime);
        hpSlider.transform.LookAt(camera.transform);
    }

    // CollisionDetectorクラスに作ったonTriggerStayEventにセットする。
    public void OnDetectObject(Collider collider)
    {
        // 検知したオブジェクトに"Player"タグが付いてれば、そのオブジェクトを追いかける
        if (collider.gameObject.tag == "Player" && !isStop) 
        {
            isChase = true;
            chaseTime = 0;
        }
        
    }

    // CollisionDetectorクラスに作ったonTriggerExitEventにセットする。 
    public void OnLoseObject(Collider collider)
    {
        // 検知したオブジェクトが範囲外から出ても、しばらく追いかけ一定秒数が経つと徘徊する
        if (collider.gameObject.tag == "Player" && !isStop) 
        {
            chaseTime = Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerController playerController;
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        //weaponタグのオブジェクトに触れると発動
        if (other.CompareTag("weapon") && !isDamage) 
        {
            isDamage = true;
            isStop = true;
            weaponCollider.enabled = false;
            //現在のHPからダメージを引く
            currentHp -= playerController.attack;
            GetComponent<Animator>().SetTrigger("damage");
            //武器ごとに被弾SEを鳴らす
            switch(playerController.weapon)
            {
                case (int)Weapon.Knife:
                    sound.PlayOneShot(knife_SE);
                    break;
                case (int)Weapon.Sword:
                    sound.PlayOneShot(sword_SE);
                    break;
                case (int)Weapon.Knuckle:
                    sound.PlayOneShot(Knuckle_SE);
                    break;
            }
            //プレイヤーの攻撃が当たると、プレイヤーの方向を向く
            transform.LookAt(player.transform);
            Invoke("NotStop", 0.5f);
            Invoke("NotDamage", 1.0f);

        }
    }

    void Death()
    {
        PlayerController playerController;
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        //死亡処理
        //KILLカウントを増やす
        playerController.kill_enemy++;
        //オブジェクトを消去する
        Destroy(gameObject);
    }

    //ダメージを受けるようになる
    void NotDamage()
    {
        isDamage = false;
    }
    //再移動可能
    void NotStop()
    {
        isStop = false;
    }
    //再攻撃可能
    void NotAttack()
    {
        isAttack = false;
    }
    //攻撃可能
    void CanAttack()
    {
        attack = false;
    }
    //武器コライダーつける
    void IsAttack()
    {
        weaponCollider.enabled = true;
    }
    //武器コライダー消す
    void NotWeapon()
    {
        weaponCollider.enabled = false;
    }

    public enum Weapon{
        Knife,
        Sword,
        Knuckle
    }
}
