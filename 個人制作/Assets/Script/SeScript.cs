using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeScript : MonoBehaviour
{
    //スクリプト取得
    public PlayerController playerController;
    public GameManager gameManager;

    //コンポーネント取得
    public AudioSource se;

    public bool isHit; //連続再生防止

    // Start is called before the first frame update
    public void Start()
    {
        isHit = false;
    }
}
