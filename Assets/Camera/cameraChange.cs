using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraChange : MonoBehaviour
{
    public Camera mainCamera;
    public Camera subCamera;
    public GameObject aim;
    public bool aimFlag;

    void Start()
    {
        mainCamera.enabled = true;
        subCamera.enabled = false;
        aimFlag = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            mainCamera.enabled = !mainCamera.enabled;
            subCamera.enabled = !subCamera.enabled;
            aimFlag = !aimFlag;

            aim.SetActive(aimFlag);
        }
    }
}