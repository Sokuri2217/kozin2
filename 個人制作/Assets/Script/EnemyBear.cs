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
    public bool isChase = false;  //追跡フラグ
    private int chaseTime = 0;    //追跡解除用のカウント

    private AudioSource sound = null;
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
            //Debug.Log(agent.remainingDistance);
            if (agent.remainingDistance < 0.5f && !isChase)  
            {
                nextGoal(); 
            }
        }

        if (agent.remainingDistance > 0.5f && isChase && !attack) 
        {
            attack = true;
            animator.SetTrigger("attack");
            
            Invoke("IsAttack", 0.1f);
            Invoke("NotAttack", 2.0f);
            Invoke("NotWeapon", 0.3f);
        }

        //最大HPにおける現在のHPをSliderに反映
        hpSlider.value = currentHp / maxHp;

        if (currentHp <= 0.0f && !death) 
        {
            animator.SetTrigger("death");
            death = true;
            Invoke("Death", 0.6f);
        }
        hpSlider.transform.LookAt(camera.transform);
    }

    // CollisionDetectorクラスに作ったonTriggerStayEventにセットする。
    public void OnDetectObject(Collider collider)
    {
        // 検知したオブジェクトに"Player"タグが付いてれば、そのオブジェクトを追いかける
        if (collider.gameObject.tag == "Player" && !isStop) 
        {
            // 対象のオブジェクトを追いかける
            agent.destination = collider.gameObject.transform.position;
            isChase = true;
        }
        
    }

    // CollisionDetectorクラスに作ったonTriggerExitEventにセットする。 
    public void OnLoseObject(Collider collider)
    {
        // 検知したオブジェクトに"Player"タグが付いてれば、その場で止まる
        if (collider.gameObject.tag == "Player" && !isStop) 
        {
            isChase = false;
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
            Invoke("NotStop", 0.5f);
            Invoke("NotDamage", 1.0f);

        }
    }

    void Death()
    {
        PlayerController playerController;
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        playerController.kill_enemy++;
        Destroy(gameObject);
    }

    void NotDamage()
    {
        isDamage = false;
    }

    void NotStop()
    {
        isStop = false;
    }
    void NotAttack()
    {
        attack = false;
    }
    void IsAttack()
    {
        weaponCollider.enabled = true;
    }
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
