using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        //コンポーネント関連付け
        TryGetComponent(out animator);
    }

    // Update is called once per frame
    void Update()
    {
        //入力ベクトルの取得
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var velocity = new Vector3(horizontal, 0, vertical).normalized;
        var speed = Input.GetKey(KeyCode.LeftShift) ? 1 : 0.5f;

        //移動方向を向く
        if(velocity.magnitude>0.5f)
        {
            transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
        }

        //移動速度をAnimatorに反映
        animator.SetFloat("Speed", velocity.magnitude * speed, 0.1f, Time.deltaTime);

    }
}
