using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshagentを使う
using UnityEngine.UI;


public class GolemController : ObjectMove
{
    //移動関連
    public Transform player; //プレイヤーの位置
    public int speed;        //移動速度

    //戦闘関連
    public GameObject shotPos;  //遠距離オブジェクトを発射する位置
    public GameObject rock;     //遠距離攻撃の岩オブジェクト
    public float rockSpeed;     //岩の速度
    public bool attack;         //攻撃フラグ
    public float attackStart;   //攻撃を始める時間
    public bool attackFar;      //遠距離攻撃
    public bool attackNear;     //近距離攻撃

    //武器の当たり判定
    public Collider nearCollider; //近距離攻撃用コライダー

    //ゴールオブジェクト
    public GameObject goal;

    //ボスUI
    public GameObject bossUi;

    //エフェクト
    public GameObject hitEffect;
    public GameObject deathEffect;

    // Start is called before the first frame update
    private new void Start()
    {
        //攻撃状態の初期化
        attack = false;
        //移動速度の設定
        agent.speed = speed;
        {
            //ボスの右手を取得（遠距離の発射位置のため）
            //shotPos = GameObject.Find("Index_Proximal_R");
        }
        //現在の値を最大値と同じにする
        currentHp = maxHp;
        //近距離攻撃の当たり判定を消す
        nearCollider.enabled = false;
        bossUi.SetActive(true);
    }


    new void Update()
    {
        move = 1.0f;

        // 対象のオブジェクトを追いかける
        agent.destination = player.transform.position;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //プレイヤーが近くにいるとき
        if ((transform.position.x - player.transform.position.x) < 0.001f &&
            (transform.position.z - player.transform.position.z) < 0.001f)
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

        if (attackNear)   
        {
            move = 0.0f;
            agent.speed = 0;
            attackNear = false;

            if (!isAttack)
            {
                transform.LookAt(player.transform);

                //近距離攻撃
                animator.SetTrigger("nearattack");
                nearCollider.enabled = true;
                {
                    ////近距離
                    //if (attackNear) 
                    //{
                    //    animator.SetTrigger("nearattack");
                    //    nearCollider.enabled = true;
                    //}
                    ////遠距離
                    //else if (attackFar)
                    //{
                    //    animator.SetTrigger("farattack");
                    //    //rock = (GameObject)Instantiate(rock, this.transform.position, Quaternion.identity);
                    //    //rock.transform.parent = shotPos.transform;
                    //    //Invoke("RockShot", 1.6f);
                    //}
                }
                isAttack = true;
                Invoke("NotWeapon", 1.5f);
            }
        }

        if (gameManager.gameOver)
        {
            agent.speed = 0;
            move = 0.0f;
        }

        //体力が0以下になると、死亡アニメーションを表示しオブジェクトを消去
        if (currentHp <= 0.0f && !death)
        {
            animator.SetTrigger("death");
            bossUi.SetActive(false);
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
        //weaponタグのオブジェクトに触れると発動
        if (other.CompareTag("weapon") && !isDamage) 
        {
            if(!isAttack)
            {
                GetComponent<Animator>().SetTrigger("damage");
            }
            //エフェクトを生成する
            GameObject effects = Instantiate(hitEffect) as GameObject;
            //エフェクトが発生する場所を決定する(敵オブジェクトの場所)
            effects.transform.position = gameObject.transform.position;
            isDamage = true;
            //現在のHPからダメージを引く
            currentHp -= playerController.attack;
            Invoke("NotDamage", 0.2f);
        }
    }

    //void RockShot()
    //{
        
    //    rock.GetComponent<Rigidbody>().velocity = shotPos.transform.forward * rockSpeed;
    //}

    void Death()
    {
        //死亡処理
        //エフェクトを生成する
        GameObject effects = Instantiate(deathEffect) as GameObject;
        //エフェクトが発生する場所を決定する(敵オブジェクトの場所)
        effects.transform.position = gameObject.transform.position;
        //その場にゴールを生成
        Instantiate(goal, transform.position, Quaternion.identity);
        //オブジェクトを消去する
        Destroy(gameObject);
    }
    //武器コライダー消す
    void NotWeapon()
    {
        isAttack = false;
        agent.speed = speed;
        nearCollider.enabled = false;
    }


}
