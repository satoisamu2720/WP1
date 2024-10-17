
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class playerMove : MonoBehaviour
{
    public CharacterController con;
    public Animator anim;


    float normalSpeed = 5f; // 通常時の移動速度
    float sprintSpeed = 8f; // ダッシュ時の移動速度
    float jump = 5f;        // ジャンプ力
    float gravity = 9.8f;    // 重力の大きさ
    float bulletTimer = 0;
    bool moveFlag;

    public bool onGround = true;

    Vector3 moveZ;
    Vector3 moveX;

    Vector3 moveDirection = Vector3.zero;

    Vector3 startPos;

    

    void Start()
    {
        startPos = transform.position;

    }
    void Update()
    {
        // 移動速度を取得
        float speed = Input.GetButton("Fire3") ? sprintSpeed : normalSpeed;

        // カメラ向きを基準にした正面方向のベクトル
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 前後左右入力（WASDキー）ベクトル計算
        moveZ = cameraForward * Input.GetAxis("Vertical") * speed;  //　前後（カメラ基準）　 
        moveX = Camera.main.transform.right * Input.GetAxis("Horizontal") * speed; // 左右（カメラ基準）


        //// カメラ向きを基準にした正面方向のベクトル
        //Vector3 subcameraForward = Vector3.Scale(Camera.sub.transform.forward, new Vector3(1, 0, 1)).normalized;

        //// 前後左右入力（WASDキー）ベクトル計算
        //moveZ = subcameraForward * Input.GetAxis("Vertical") * speed;  //　前後（カメラ基準）　 
        //moveX = Camera.sub.transform.right * Input.GetAxis("Horizontal") * speed; // 左右（カメラ基準）

        // 地面にいるか判定
        if (con.isGrounded)
        {
            // 地面にいるとき可能
            moveDirection = moveZ + moveX;
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jump;
            }
        }
        else
        {
            // 重力
            moveDirection = moveZ + moveX + new Vector3(0, moveDirection.y, 0);
            moveDirection.y -= gravity * Time.deltaTime;
        }
 
        // 移動アニメーション
        anim.SetFloat("MoveSpeed", (moveZ + moveX).magnitude);

        // プレイヤー向きを入力の向きに変更　
        transform.LookAt(transform.position + moveX + moveZ);

        //指定したベクトルだけ移動させる命令
        con.Move(moveDirection * Time.deltaTime);

    }


}
