using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeScript : MonoBehaviour
{
    //�X�N���v�g�擾
    public PlayerController playerController;
    public GameManager gameManager;

    //�R���|�[�l���g�擾
    public AudioSource se;

    public bool isHit; //�A���Đ��h�~

    // Start is called before the first frame update
    public void Start()
    {
        isHit = false;
    }
}
