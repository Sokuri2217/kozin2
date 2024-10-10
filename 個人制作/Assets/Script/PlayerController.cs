using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        //�R���|�[�l���g�֘A�t��
        TryGetComponent(out animator);
    }

    // Update is called once per frame
    void Update()
    {
        //���̓x�N�g���̎擾
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var velocity = new Vector3(horizontal, 0, vertical).normalized;
        var speed = Input.GetKey(KeyCode.LeftShift) ? 1 : 0.5f;

        //�ړ�����������
        if(velocity.magnitude>0.5f)
        {
            transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
        }

        //�ړ����x��Animator�ɔ��f
        animator.SetFloat("Speed", velocity.magnitude * speed, 0.1f, Time.deltaTime);

    }
}
