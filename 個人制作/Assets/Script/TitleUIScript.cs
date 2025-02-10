using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUIScript : MonoBehaviour
{
    //操作説明画面に移行するボタンの画像
    public Image option;
    public Sprite openOption;
    public Sprite closeOption;

    //各確認画面のパネル
    public GameObject option1Panel;
    public GameObject option2Panel;

    //表示フラグ
    public bool isOption;
    public bool isControl;
    public bool isStatus;

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        option1Panel.SetActive(false);
        option2Panel.SetActive(false);
        option.sprite = openOption;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOption)
        {
            //ボタンの画像変更
            option.sprite = closeOption;

            //1ページ目を表示
            if (!isControl && !isStatus) 
            {
                isControl = true;
            }
            //1ページ目を非表示
            else if(isStatus)
            {
                isControl = false;
            }
        }
        else
        {
            //画像を元に戻す
            option.sprite = openOption;

            //全ページを非表示
            isControl = false;
            isStatus = false;
        }

        if (isControl)
        {
            //1ページ目を表示
            option1Panel.SetActive(true);
        }
        else
        {
            //1ページ目を非表示
            option1Panel.SetActive(false);
        }

        if (isStatus)
        {
            //2ページ目を表示
            option2Panel.SetActive(true);
        }
        else
        {
            //2ページ目を非表示
            option2Panel.SetActive(false);
        }
    }
}
