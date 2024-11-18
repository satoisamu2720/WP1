using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockOn : MonoBehaviour
{
    public float lockRange = 50f;
    public Image aimImage;
    private Camera FPSCamera;
    private Color originalColor;

    void Start()
    {
        FPSCamera = GetComponent<Camera>();
        originalColor = aimImage.color;
    }

    void Update()
    {
        Vector3 rayOrigin = FPSCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, FPSCamera.transform.forward, out hit, lockRange))
        {
            string hitTag = hit.transform.gameObject.tag;

            //if (hitTag == "Target")
            //{

            //}
            //else
            //{
            //    aimImage.color = originalColor;
            //}
            if (hit.collider.tag == "Target")
            {
                // 赤色に変更
                aimImage.color = new Color(1.0f, 0f, 0f, 1.0f);
               
            }
            if (hit.collider.tag != "Target")
            {
                // 赤色に変更
                aimImage.color = originalColor;

            }

        }
    }
}
