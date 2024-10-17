using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class SwitchScript : MonoBehaviour
{
    float bottomY = -0.1f;
    float speed = 0.5f;
    float defaultY;     // ”à‚Ì‰Šú‚ÌYÀ•W
    public bool active;
    public bool onFlag = false;

    void Start()
    {
        defaultY = transform.position.y;
    }

    void Update()
    {
        if(active && transform.position.y > bottomY)
        {
            transform.position -= Vector3.up * speed * Time.deltaTime;

        }
       
        if (!active && transform.position.y < defaultY)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if(!active && other.CompareTag("Player"))
        {
            active = true;
            onFlag = true;
        }
    }
}