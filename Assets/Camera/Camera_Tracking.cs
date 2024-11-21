using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Camera_Tracking : MonoBehaviour
{
    public GameObject playerObject;   // 追尾オブジェクト（プレイヤー）

    public Vector2 rotationSpeed;           // 回転速度
    private Vector3 lastMousePosition;      // 最後のマウス座標

    public bool mainCameraFlag = true;
    public bool sabCameraFlag = false;

    public Camera subCamera;               // サブカメラ（追従するカメラ）
    public Camera mainCamera;
    private float xRotation = 0f;          // 上下の回転
    private float yRotation = 0f;          // 左右の回転

    public float mouseSensitivity = 100f;  // マウス感度
    public float upDownLimit = 60f;        // 上下回転の制限（角度）

    // カメラ位置を調整するためのオフセット
    public Vector3 cameraOffset = new Vector3(0f, 2.0f, 0f); // 初期位置

    void Start()
    {
        lastMousePosition = Input.mousePosition;
        if (mainCamera.enabled)
        {
            // サブカメラはプレイヤーの位置に固定
           
            subCamera.transform.position = playerObject.transform.position + cameraOffset;
            subCamera.transform.rotation = playerObject.transform.rotation;
        }

        // カーソルを隠してロック
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (subCamera.enabled)
        {
            // サブカメラを一人称視点に設定
            RotateCameraWithMouse();
            FollowPlayer();
            AdjustCameraPosition(); // カメラ位置調整の処理

        }
        if (mainCamera.enabled)
        {
            FacePlayerDirection(); // プレイヤーの向きを向く
        
        }
    }

    void RotateCameraWithMouse()
    {
        // マウスの動きによる回転処理
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // 左右の動き
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; // 上下の動き

        // 上下回転
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -upDownLimit, upDownLimit); // 上下の回転制限

        // 左右回転
        yRotation += mouseX;

        // プレイヤーの回転を更新（サブカメラの回転も更新）
        subCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void FollowPlayer()
    {
        // サブカメラの位置はプレイヤーの位置に固定
        subCamera.transform.position = playerObject.transform.position + cameraOffset;
    }

    void AdjustCameraPosition()
    {
        // ユーザー入力でカメラ位置を調整
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 2f;  // 左右
        float moveY = Input.GetAxis("Vertical") * Time.deltaTime * 2f;    // 前後

        // 位置調整後に再度カメラ位置を設定
        subCamera.transform.position = playerObject.transform.position + cameraOffset;
    }

    void FacePlayerDirection()
    {
        // プレイヤーの向きに合わせてカメラも回転するようにする
        Vector3 direction = playerObject.transform.forward; // プレイヤーの向き
        Quaternion targetRotation = Quaternion.LookRotation(direction); // プレイヤーが向いている方向を向く

        // カメラをプレイヤーの向きに合わせる
        subCamera.transform.rotation = Quaternion.Slerp(subCamera.transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

}