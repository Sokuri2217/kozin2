using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeScript : MonoBehaviour
{
    public AudioSource se;  //SE関連

    //プレイヤー
    public AudioClip playerDamageSe;      //被弾
    public AudioClip playerPowerDamageSe; //被ダメージ上昇中の被弾
    public AudioClip itemGet;             //アイテム獲得
    public bool playerIsSound;

    //敵
    public AudioClip[] enemyDamageSe;      //被弾
    public AudioClip[] enemyPowerDamageSe; //被ダメージ上昇中の被弾
    public bool enemyIsSound;
    //熊
    public AudioClip BearDeathSe; //死亡
    //ゴーレム
    public AudioClip GolemDeathSe; //死亡

    //UI
    public AudioClip changeConsentSe; //ゲージがたまったとき
    private bool consent;             //たまったときのSEが複数回ならないようにする
    public AudioClip changeSe;        //ゲージを消費したとき
    private bool use;                 //消費したときのSEが複数回ならないようにする

    //スクリプト取得
    public PlayerController playerController;
    public PlayerAttack playerAttack;
    public GameManager gameManager;
    public MainUIScript mainUI;

    // Start is called before the first frame update
    void Start()
    {
        enemyIsSound = false;
        playerIsSound = false;
    }

    // Update is called once per frame
    void Update()
    {
        //敵に攻撃が当たったときに、SEを鳴らす
        if (playerAttack.hit)  
        {
            if(!enemyIsSound)
            {
                //武器ごとに被弾SEを鳴らす
                if (playerController.attack > playerController.firstAttack)
                {
                    //プレイヤーの攻撃力が上昇しているとき
                    se.PlayOneShot(enemyPowerDamageSe[playerController.weapon]);
                }
                else
                {
                    //プレイヤーの攻撃力が変化していないとき
                    se.PlayOneShot(enemyDamageSe[playerController.weapon]);
                }

                enemyIsSound = true;
            }
        }

        //敵が死亡するとき
        //熊
        //if(enemyBear.death)
        //{
        //    se.PlayOneShot(BearDeathSe);
        //}
        //if(golemController.death)
        //{
        //    se.PlayOneShot(GolemDeathSe);
        //}

        //敵の攻撃に当たったとき
        if(playerController.isDamage)
        {
            if (!playerIsSound)
            {
                //ダメージSEを鳴らす
                if (playerController.damage > playerController.firstDamage)
                {
                    se.PlayOneShot(playerPowerDamageSe);
                }
                else
                {
                    se.PlayOneShot(playerDamageSe);
                }
                playerIsSound = true;
            }
        }
        else
        {
            playerIsSound = false;
        }

        //アイテム取ったとき
        if (playerController.item)
        {
            //取得音を鳴らす
            se.PlayOneShot(itemGet);
            playerController.item = false;
        }

        //ゲージたまったとき
        if (mainUI.changeConsent)
        {
            if (!consent)
            {
                se.PlayOneShot(changeConsentSe);
                consent = true;
            }
        }
        //ゲージ消費したとき
        else if (mainUI.changeInput) 
        {
            if (!use)
            {
                se.PlayOneShot(changeSe);
                use = true;
                mainUI.changeInput = false;
            }
        }
        else
        {
            consent = false;
            use = false;
        }
    }
}
