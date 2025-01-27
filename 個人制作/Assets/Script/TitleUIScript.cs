using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUIScript : MonoBehaviour
{
    public GameObject option1Panel;
    public GameObject option2Panel;
    public bool isOption;
    public bool isControl;
    public bool isStatus;

    // Start is called before the first frame update
    void Start()
    {
        option1Panel.SetActive(false);
        option2Panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isOption)
        {
            isControl = true;
        }
        else
        {
            isControl = false;
            isStatus = false;
        }

        if (isControl)
        {
            option1Panel.SetActive(true);
        }
        else
        {
            option1Panel.SetActive(false);
        }

        if (isStatus)
        {
            option2Panel.SetActive(true);
        }
        else
        {
            option2Panel.SetActive(false);
        }
    }
}
