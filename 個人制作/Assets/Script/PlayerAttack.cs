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
        //weaponタグのオブジェクトに触れると発動
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
