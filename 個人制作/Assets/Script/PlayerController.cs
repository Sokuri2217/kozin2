using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    
    Quaternion targetRotation;
    Vector3 pos;

    void Awake()
    {
        //コンポーネント関連付け
        TryGetComponent(out animator);

        //初期化
        targetRotation = transform.rotation;
        pos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        //入力ベクトルの取得
        var horizontal = Input.GetAxis("Horizontal");
        var vertical   = Input.GetAxis("Vertical");
        var velocity = new Vector3(horizontal, 0, vertical).normalized;
        var speed = Input.GetKey(KeyCode.LeftShift) ? 1.5f : 0.75f;
        var rotationSpeed = 600 * Time.deltaTime;

        pos.x += horizontal / 60 * speed;
        pos.z += vertical / 60 * speed;

        transform.position = pos;

        if (velocity.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

        //移動速度をAnimatorに反映
        animator.SetFloat("Speed", velocity.magnitude * speed, 0.1f, Time.deltaTime);
    }
}
