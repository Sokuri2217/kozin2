using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject player;

    private float rotation_hor;
    private float rotation_ver;
    private float distance_base;
    private Vector3 playertrack;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        rotation_hor = 0f;
        rotation_ver = 0f;
        distance_base = 5.0f;
        playertrack = Vector3.zero;

        //cursor lock : Esc to exit
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        distance_base -= Input.mouseScrollDelta.y * 0.5f;
        if (distance_base < 1.0f) distance_base = 1.0f;
    }

    void FixedUpdate()
    {
        //camera rotation
        rotation_hor += Input.GetAxis("Mouse X") * 3;
        rotation_ver -= Input.GetAxis("Mouse Y") * 1.5f;

        //restrict vertical angle to -90 ~ +90
        if (Mathf.Abs(rotation_ver) > 90) rotation_ver = Mathf.Sign(rotation_ver) * 90;

        //base vector to rotate
        var rotation = Vector3.Normalize(new Vector3(0, 0.2f, -5)); //base(normalized)
        rotation = Quaternion.Euler(rotation_ver, rotation_hor, 0) * rotation; //rotate vector

        //stop at floor-obstacle layer
        RaycastHit hit;
        int layermask = 1 << 6; //1のビットを6レイヤー分(Floor_obstacleがある場所)だけ左シフト
        float distance = distance_base; //copy default(mouseScroll zoom)

        if (Physics.SphereCast(playertrack + Vector3.up * 1.7f, 0.5f,
        rotation, out hit, distance, layermask))
        {
            distance = hit.distance; //overwrite copy
        }

        //turn self
        transform.rotation = Quaternion.Euler(rotation_ver, rotation_hor, 0); //Quaternion IN!!

        //turn around + zoom
        transform.position = rotation * distance;

        //rotation center to neck-level
        var necklevel = Vector3.up * 1.7f;
        transform.position += necklevel;

        //track
        playertrack = Vector3.Lerp(
            playertrack, player.transform.position, Time.deltaTime * 10);
        transform.position += playertrack;
    }
}
