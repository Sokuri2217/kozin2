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
        main_Bgm = GetComponent<AudioSource>();
        result_Bgm = GetComponent<AudioSource>();

        main_Bgm.Play();

        bgm = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameManager gameManager = GetComponent<GameManager>();

        if (gameManager.gameClear && !bgm) 
        {
            result_Bgm.PlayOneShot(clear_Bgm);
            main_Bgm.Stop();
            bgm = true;
        }
        if (gameManager.gameOver && bgm)
        {
            result_Bgm.PlayOneShot(over_Bgm);
            main_Bgm.Stop();
            bgm = true;
        }   
    }
}
