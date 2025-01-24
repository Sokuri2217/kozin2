using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshagentを使う
using UnityEngine.UI;


public class GolemController : ObjectMove
{
    //移動関連
    public Transform player;      //プレイヤーの位置

    //戦闘関連
    public bool attack = false;  //攻撃フラグ
    private float attackTime;    //攻撃するまでの時間
    public float attackStart;    //攻撃を始める時間
    public bool attackFar;       //遠距離攻撃
    public bool attackNear;      //近距離攻撃

    //ダメージSE
    public AudioClip[] damage_Se;

    //武器の当たり判定
    public Collider nearCollider;
    public Collider farCollider;

    // Start is called before the first frame update
    private new void Start()
    {
        //weaponCollider.enabled = false;
        speed = 0.0f;

        //現在の値を最大値と同じにする
        currentHp = maxHp;

        nearCollider.enabled = false;
        farCollider.enabled = false;
    }


    private new void Update()
    {
        speed = 1.0f;
        // 対象のオブジェクトを追いかける
        agent.destination = player.transform.position;

        //プレイヤーが近くにいるとき
        if ((transform.position.x - player.transform.position.x) < 7.0f &&
            (transform.position.z - player.transform.position.z) < 7.0f)
        {
            attackNear = true;
            attackFar = false;
        }
        //プレイヤーが遠くにいるとき
        else
        {
            attackFar = true;
            attackNear = false;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isAttack) 
        {
            attackTime++;
        }

        //移動開始から一定時間経つと、その場で止まりプレイヤーとの距離に適した攻撃をする
        if (attackTime >= attackStart)  
        {
            speed = 0.0f;
            agent.speed = 0;

            //近距離
            if (!isAttack && attackNear)
            {
                animator.SetTrigger("nearattack");
                nearCollider.enabled = true;
            }
            //遠距離
            else if (attackFar) 
            {
                animator.SetTrigger("farattack");
                farCollider.enabled = true;
            }
            isAttack = true;
            attackTime = 0.0f;
            Invoke("NotWeapon", 10.0f);
        }
        else
        {
            speed = 1.0f;
            agent.speed = 10;
        }

        

        GameObject obj = GameObject.Find("Player");
        gameManager = obj.GetComponent<GameManager>();

        if (gameManager.gameOver)
        {
            agent.speed = 0;
            speed = 0.0f;
        }

        //体力が0以下になると、死亡アニメーションを表示しオブジェクトを消去
        if (currentHp <= 0.0f && !death)
        {
            animator.SetTrigger("death");
            agent.speed = 0;
            death = true;
            Invoke("Death", 0.6f);
        }
        animator.SetFloat("EnemyBoss", speed, 0.1f, Time.deltaTime);

        //最大HPにおける現在のHPをSliderに反映
        hpSlider.value = currentHp / maxHp;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        //weaponタグのオブジェクトに触れると発動
        if (other.CompareTag("weapon") && !isDamage)
        {
            if(!isAttack)
            {
                GetComponent<Animator>().SetTrigger("damage");
            }
            isDamage = true;
            //武器ごとに被弾SEを鳴らす
            se.PlayOneShot(damage_Se[playerController.weapon]);
            //現在のHPからダメージを引く
            currentHp -= playerController.attack;
        }
    }

    void Death()
    {
        //死亡処理
        //オブジェクトを消去する
        Destroy(gameObject);
    }
    //武器コライダー消す
    void NotWeapon()
    {
        isAttack = false;
        nearCollider.enabled = false;
        farCollider.enabled = false;
    }


}
