using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //アイテム生成時に地面に埋まるのを防ぐ
        transform.position += new Vector3(0, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
