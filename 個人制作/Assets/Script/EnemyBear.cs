using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshagentを使う
using UnityEngine.UI;


public class EnemyBear : MonoBehaviour
{
    public Transform[] goals;
    public Transform player;
    private int destNum = 0;
    private NavMeshAgent agent;
    private Animator animator;


    public Slider hpSlider;       //HPバー

    public float maxHp = 50;     //最大のHP
    public float currentHp;       //現在のHP
    float currentTime = 0.0f;     //現在の時間取得

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goals[destNum].position;
        animator = GetComponent<Animator>();

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
    void Update()
    {
        //Debug.Log(agent.remainingDistance);
        if (agent.remainingDistance < 0.5f)
        {
            nextGoal();
        }

        //最大HPにおける現在のHPをSliderに反映
        hpSlider.value = currentHp / maxHp;

        if(currentHp<=0.0f)
        {
            Destroy(gameObject);
        }

        //APの自動回復
        if (currentHp < maxHp)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= 1.0f)
            {
                currentHp += 1.0f;
                currentTime = 0.0f;
            }
        }

    }

    // CollisionDetectorクラスに作ったonTriggerStayEventにセットする。
    public void OnDetectObject(Collider collider)
    {
        // 検知したオブジェクトに"Player"タグが付いてれば、そのオブジェクトを追いかける
        if (collider.gameObject.tag == "Player")
        {
            // 対象のオブジェクトを追いかける
            agent.destination = collider.gameObject.transform.position;

            if(currentHp<=currentHp/2)
            {
                animator.SetTrigger("search");

                agent.destination = collider.gameObject.transform.position;
            }
        }

    }

    // CollisionDetectorクラスに作ったonTriggerExitEventにセットする。 
    public void OnLoseObject(Collider collider)
    {
        // 検知したオブジェクトに"Player"タグが付いてれば、その場で止まる
        if (collider.gameObject.tag == "Player")
        {
            // その場で止まる（目的地を今の自分自身の場所にすることにより止めている）
            agent.destination = this.gameObject.transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerController playerController;
        GameObject obj = GameObject.Find("Player");
        playerController = obj.GetComponent<PlayerController>();

        //Enemyタグのオブジェクトに触れると発動
        if (other.CompareTag("weapon"))
        {
            //現在のHPからダメージを引く
            currentHp -= playerController.attack;
            GetComponent<Animator>().SetTrigger("damage");
        }
    }

    //void OnTriggerStay(Collider col)
    //{
    //    if (col.gameObject.name == "player")
    //    {
    //        transform.LookAt(player);
    //        transform.Translate(0.1f, 0, 0.1f);
    //    }
    //}
}
