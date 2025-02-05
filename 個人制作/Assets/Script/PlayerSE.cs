using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSE : SeScript
{
    //アイテム取得音
    public AudioClip itemGet;
    public bool isItemGet;

    //被弾音
    public AudioClip damage;      //通常
    public AudioClip powerDamage; //被ダメ増加
    public AudioClip downDamage; //被ダメ減少

    // Start is called before the first frame update
    new void Start()
    {
        isItemGet = false;
        isHit = false;
    }

    // Update is called once per frame
    new void Update()
    {
        //アイテム取得
        if(playerController.item)
        {
            //複数鳴らないようにする
            if (!isItemGet)
            {
                se.PlayOneShot(itemGet);
                isItemGet = true;
            }
        }
        else
        {
            isItemGet = false;
        }

        //被弾
        if(playerController.isDamage)
        {
            //複数鳴らないようにする
            if(!isHit)
            {
                //プレイヤーのダメージ量によって変える
                if (playerController.damage > playerController.firstDamage) //被ダメ増加
                {
                    se.PlayOneShot(powerDamage);
                }
                else if (playerController.damage < playerController.firstDamage) //被ダメ減少
                {

                }
                else //通常
                {
                    se.PlayOneShot(damage);
                }

                isHit = true;
            }
        }
        else
        {
            isHit = false;
        }
    }
}
