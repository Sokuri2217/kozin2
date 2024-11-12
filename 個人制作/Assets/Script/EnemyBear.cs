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

    public float maxHp = 100;     //最大のHP
    public float currentHp;       //現在のHP

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goals[destNum].position;
        animator = GetComponent<Animator>();
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

    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == "player")
        {
            transform.LookAt(player);
            transform.Translate(0.1f, 0, 0.1f);
        }
    }
}
