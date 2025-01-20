using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshagentを使う
using UnityEngine.UI;


public class EnemyBear : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public Collider searchArea;

    public Transform[] allGoals;  //全ての徘徊ポイント
    public Transform[] goals;     //実際に徘徊するポイント
    public int goalsNum;         //徘徊ポイントに設定するポイント
    private int setNum;           //乱数格納用
    public Transform player;      //プレイヤーの位置
    public new Transform  camera; //カメラの位置
    private int destNum = 0;　　　//向かう場所
    public Slider hpSlider;       //HPバー
    public float maxHp = 30;      //最大のHP
    public float currentHp;       //現在のHP
    bool death;                   //死亡フラグ
    bool isDamage = false;        //ダメージ中フラグ
    bool attack = false;          //攻撃フラグ
    bool isAttack = false;        //攻撃フラグ
    public bool isChase = false;  //追跡フラグ
    private int chaseTime = 0;    //追跡解除用のカウント
    private int speed;            //アニメーション管理用
    public float surpriseAttack;  //不意打ち被ダメ倍率

    private AudioSource sound = null;

    //ダメージSE
    public AudioClip knife_SE;
    public AudioClip sword_SE;
    public AudioClip Knuckle_SE;

    public Collider weaponCollider;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i > goalsNum; i++) 
        {
            setNum = Random.Range(0, 10);
            goals[i] = allGoals[setNum];
        }
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goals[destNum].position;
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        weaponCollider.enabled = false;
        speed = 0;
        death = false;
        //Sliderを満タンにする。
        hpSlider.value = 1;
        //現在の値を最大値と同じにする
        currentHp = maxHp;
    }

    void nextGoal()
    {
        //目的地を抽選
        destNum = Random.Range(0, 3);
        //目的地まで移動
        agent.destination = goals[destNum].position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //プレイヤーに一定距離近づくと、攻撃する
        if ((transform.position.x - player.transform.position.x) < 1.0f &&
            (transform.position.z - player.transform.position.z) < 1.0f &&
            isChase && !attack && !isAttack)
        {
            attack = true;
            isAttack = true;
            animator.SetTrigger("attack");

            Invoke("IsAttack", 0.5f);
            Invoke("NotAttack", 1.0f);
            Invoke("CanAttack", 2.0f);
            Invoke("NotWeapon", 0.3f);
        }
        //チェイス中は、移動速度と移動アニメーションを変更
        else if (isChase && !isAttack)
        {
            //チェイス中は、索敵範囲を消去
            searchArea.enabled = false;
            speed = 2;
            agent.speed = 15;
            chaseTime++;
            // 対象のオブジェクトを追いかける
            agent.destination = player.transform.position;
        }
        //徘徊モード
        //目的地に一定距離近づくと、再度目的地の抽選を行う
        else if (agent.remainingDistance < 2.0f)
        {
            //徘徊時は、索敵範囲を出す
            searchArea.enabled = true;
            chaseTime = 0;
            speed = 1;
            agent.speed = 2;
            nextGoal();
        }

        GameManager gameManager;
        GameObject obj = GameObject.Find("Player");
        gameManager = obj.GetComponent<GameManager>();

        if(isAttack||isDamage)
        {
            agent.speed = 0;
        }

        if (gameManager.gameOver || gameManager.gameClear)
        {
            agent.speed = 0;
            speed = 0;
        }

        //チェイススタートから一定時間が経つと、徘徊モードに戻る
        if (chaseTime >= 300)
            isChase = false;

        //最大HPにおける現在のHPをSliderに反映
        hpSlider.value = currentHp / maxHp;

        ////体力が0以下になると、死亡アニメーションを表示しオブジェクトを消去
        if (currentHp <= 0.0f && !death)
        {
            animator.SetTrigger("death");
            agent.speed = 0;
            gameManager.currentEnemy--;
            death = true;
            Invoke("Death", 0.6f);
        }
        animator.SetFloat("EnemySpeed", speed, 0.1f, Time.deltaTime);
        hpSlider.transform.LookAt(camera.transform);
    }

    // CollisionDetectorクラスに作ったonTriggerStayEventにセットする。
    public void OnDetectObject(Collider other)
    {
        // 検知したオブジェクトに"Player"タグが付いてれば、そのオブジェクトを追いかける
        if (other.CompareTag("Player")) 
        {
            isChase = true;
            chaseTime = 0;
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
            weaponCollider.enabled = false;
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

            if(!isChase)
            {
                //現在のHPからダメージを引く
                currentHp -= playerController.attack * surpriseAttack;
            }
            else
            {
                //現在のHPからダメージを引く
                currentHp -= playerController.attack;
            }

            //プレイヤーの攻撃が当たると、プレイヤーの方向を向く
            isChase =true;
            Invoke("NotDamage", 0.5f);

        }
    }

    void Death()
    {
        PlayerController playerController;
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        //死亡処理
        //KILLカウントを増やす
        playerController.killEnemy++;
        //オブジェクトを消去する
        Destroy(gameObject);
    }

    //ダメージを受けるようになる
    void NotDamage()
    {
        isDamage = false;
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
