using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshagent���g��
using UnityEngine.UI;


public class EnemyBear : MonoBehaviour
{
    public Transform[] goals;
    public Transform player;
    private int destNum = 0;
    private NavMeshAgent agent;

    public Slider hpSlider;       //HP�o�[

    public float maxHp = 100;     //�ő��HP
    public float currentHp;       //���݂�HP

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goals[destNum].position;
    }

    void nextGoal()
    {

        destNum += 1;
        if (destNum == 3)
        {
            destNum = 0;
        }

        agent.destination = goals[destNum].position;

        Debug.Log(destNum);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(agent.remainingDistance);
        if (agent.remainingDistance < 0.5f)
        {
            nextGoal();
        }

        //�ő�HP�ɂ����錻�݂�HP��Slider�ɔ��f
        hpSlider.value = currentHp / maxHp;

    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == "player")
        {
            transform.LookAt(player);
            transform.Translate(0, 0, 0.1f);
        }
    }
}
