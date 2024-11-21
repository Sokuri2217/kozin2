using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    BoxCollider boxCol;

    void Start()
    {
        boxCol = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            boxCol.enabled = false;
        }

        else if (Input.GetMouseButtonDown(1))
        {
            boxCol.enabled = true;
        }
    }
}
