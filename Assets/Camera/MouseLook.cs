using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity;  //感度
    public Transform player;
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        // カーソルキーを非表示にする。
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // 左右の動き
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; // 上下の動き

        // 上下運動
        xRotation -= mouseY;
        yRotation += mouseX;
        

        // 上下運動（移動できる角度）に制限を加える。
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        //yRotation = Mathf.Clamp(-90f, yRotation, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // 左右運動
        //player.Rotate(Vector3.up * mouseX);
    }
}