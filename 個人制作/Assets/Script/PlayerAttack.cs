using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //weapon�^�O�̃I�u�W�F�N�g�ɐG���Ɣ���
        if (other.CompareTag("Enemy"))
        {
            hit = true;
        }
        else
        {
            hit = false;
        }
    }
}
