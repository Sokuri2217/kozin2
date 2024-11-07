using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshagent‚ðŽg‚¤

public class EnemyMove : MonoBehaviour
{
    public Transform[] goals;
    private int destNum = 0;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goals[destNum].position;
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
        // Debug.Log(agent.remainingDistance);
        if (agent.remainingDistance < 0.5f)
        {
            nextGoal();
        }

    }
}

