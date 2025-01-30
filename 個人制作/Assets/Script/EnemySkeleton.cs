using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshagent‚ðŽg‚¤
using UnityEngine.UI;

public class EnemySkeleton : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject damageeffect;
    private RaycastHit hit;

    void Start()
    {
    }

    void Update()
    {

        // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 0.3f);

        //  transform.position += transform.forward * this.speed;


        // rigid.MovePosition(transform.position + transform.forward * Time.deltaTime);
        // transform.rotation = Quaternion.LookRotation(transform.position - SlimePos);

        //  SlimePos = transform.position;
        //transform.rotation = Quaternion.LookRotation(new Vector3(0, 90, 0));
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject Target = GameObject.Find("Player");

            var diff = Target.transform.position - transform.position;
            var distance = diff.magnitude;
            var direction = diff.normalized;

            if (Physics.Raycast(transform.position, direction, out hit, distance))
            {
                if (hit.transform.gameObject == Target)
                {
                    agent.isStopped = false;
                    agent.destination = Target.transform.position;
                }
                else
                {
                    agent.isStopped = true;
                }

            }

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {

            foreach (ContactPoint point in other.contacts)
            {
                GameObject effect = Instantiate(damageeffect) as GameObject;
                effect.transform.position = (Vector3)point.point;

            }

        }
    }
}
