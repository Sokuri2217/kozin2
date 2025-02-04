using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUISE : SeScript
{
    //スクリプト取得
    public MainUIScript mainUI;

    public AudioClip consent;
    public bool isConsent;
    public AudioClip change;
    public bool isChange;

    // Start is called before the first frame update
    new void Start()
    {
        isChange = false;
        isConsent = false;
    }

    // Update is called once per frame
    new void Update()
    {
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
