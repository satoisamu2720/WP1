using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    float defaultY;     // 扉の初期のY座標
    float openY = 5f;   // 扉のオープン時のY座標
    float speed = 1f;   // 扉の開閉のスピード

    public bool isOpen; // 扉を開けるか閉めるかのフラグ

    void Start()
    {
        defaultY = transform.position.y;
    }

    void Update()
    {
        if (isOpen && transform.position.y < openY)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
        else if (!isOpen && transform.position.y > defaultY)
        {
            transform.position -= Vector3.up * speed * Time.deltaTime;
        }
    }
}