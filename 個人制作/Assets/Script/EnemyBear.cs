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
    private int chaseTime = 0;  //追跡解除用のカウント
    private int speed;            //アニメーション管理用

    private AudioSource sound = null;

    //ダメージSE
    public AudioClip knife_SE;
    public AudioClip sword_SE;
    public AudioClip Knuckle_SE;

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
        //目的地を抽選
        destNum = Random.Range(0, 3);
        //目的地まで移動
        agent.destination = goals[destNum].position;

        Debug.Log(destNum);
    }
    private void Update()
    {
        Debug.Log(chaseTime);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //攻撃中はその場で停止
        if (isAttack)
        {
            agent.speed = 0;
        }
        //チェイス中は、移動速度と移動アニメーションを変更
        else if (isChase) 
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
        else if (agent.remainingDistance < 0.5f && !isStop) 
        {
            //徘徊時は、索敵範囲を出す
            searchArea.enabled = true;
            chaseTime = 0;
            speed = 1;
            agent.speed = 2;
            nextGoal();
        }

        //チェイススタートから一定時間が経つと、徘徊モードに戻る
        if (chaseTime >= 300)
        {
            Debug.Log("徘徊");
            isChase = false;

        }

        //プレイヤーに一定距離近づくと、攻撃する
        if ((transform.position.x - player.transform.position.x) < 1.0f &&
            (transform.position.z - player.transform.position.z) < 1.0f &&
            isChase && !attack && !isAttack) 
        {
            attack = true;
            isAttack = true;
            agent.speed = 0;
            animator.SetTrigger("attack");
            
            Invoke("IsAttack", 0.5f);
            Invoke("NotAttack", 0.5f);
            Invoke("CanAttack", 2.0f);
            Invoke("NotWeapon", 0.3f);
        }

        //最大HPにおける現在のHPをSliderに反映
        hpSlider.value = currentHp / maxHp;

        ////体力が0以下になると、死亡アニメーションを表示しオブジェクトを消去
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

    ////CollisionDetectorクラスに作ったonTriggerExitEventにセットする。 
    //public void OnLoseObject(Collider collider)
    //{
    //   // 検知したオブジェクトが範囲外から出ても、しばらく追いかけ一定秒数が経つと徘徊する
    //    if (collider.gameObject.tag == "Player" && !isStop)
    //    {

    //    }
    //}

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
