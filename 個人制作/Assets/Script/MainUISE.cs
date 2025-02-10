using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUISE : SeScript
{
    //スクリプト取得
    public MainUIScript mainUI;

    //効果音
    public AudioClip consent; //ステータス変更可能
    public AudioClip change;  //ステータス変更

    //一度だけ鳴らすためのフラグ
    public bool isConsent;
    public bool isChange; 

    // Start is called before the first frame update
    new void Start()
    {
        isChange = false;
        isConsent = false;
    }

    // Update is called once per frame
    void Update()
    {
        //ステータス変更可能
        if(mainUI.changeConsent)
        {
            if(!isConsent)
            {
                se.PlayOneShot(consent);
                isConsent = true;
            }
        }
        else
        {
            isConsent = false;
        }

        //ステータス変更
        if(mainUI.changeInput)
        {
            if(!isChange)
            {
                se.PlayOneShot(change);
                isChange = true;
            }
        }
        else
        {
            isChange = false;
        }
    }
}
