using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraChange : MonoBehaviour
{
    public Camera mainCamera;
    public Camera subCamera;

    void Start()
    {
        mainCamera.enabled = true;
        subCamera.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            mainCamera.enabled = !mainCamera.enabled;
            subCamera.enabled = !subCamera.enabled;
        }
    }
}