using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmScript : MonoBehaviour
{
    public AudioSource main_Bgm;

    private AudioSource result_Bgm;
    public AudioClip clear_Bgm;
    public AudioClip over_Bgm;

    bool bgm;

    // Start is called before the first frame update
    void Start()
    {
        
        bgm = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameManager gameManager = GetComponent<GameManager>();

        
    }
}
