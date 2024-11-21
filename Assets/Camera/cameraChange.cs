using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraChange : MonoBehaviour
{
    public Camera mainCamera;
    public Camera subCamera;
    
    public GameObject aim;
    

    void Start()
    {
        mainCamera.enabled = true;
        subCamera.enabled = false;
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.R))
        {
            
            mainCamera.enabled = false;
            subCamera.enabled = true;
            aim.SetActive(true);
           
        }
        if (Input.GetKeyDown(KeyCode.E))
        {

            mainCamera.enabled = true;
            subCamera.enabled = false;
            aim.SetActive(false);
           
        }
    }
}