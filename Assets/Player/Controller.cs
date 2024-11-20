
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class playerMove : MonoBehaviour
{


    // ★追加（スパイダーフック）
    private enum State
    {
        Normal,
        ThrowingHook,
        FlyingPlayer
    }

    // ★追加（スパイダーフック）
    public float hookRange = 100f;
    public Transform hookTargetMark;
    public Transform hookShot;
    private Vector3 hookPoint;
    private float hookShotSize;
    private State state;
    public CharacterController con;
    public Animator anim;
    public Camera subCamera;
    public Camera mainCamera;

    cameraChange cameraChange;
    HookLock Target;


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
        // ★追加（スパイダーフック）
        state = State.Normal;
        hookShot.gameObject.SetActive(false);
        hookTargetMark.gameObject.SetActive(false);

    }
    void PlayerMove()
    {
        // 移動速度を取得
        float speed = Input.GetButton("Fire3") ? sprintSpeed : normalSpeed;

        // カメラ向きを基準にした正面方向のベクトル
        Vector3 cameraForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 前後左右入力（WASDキー）ベクトル計算
        moveZ = cameraForward * Input.GetAxis("Vertical") * speed;  //　前後（カメラ基準）　 
        moveX = mainCamera.transform.right * Input.GetAxis("Horizontal") * speed; // 左右（カメラ基準）


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

    private void Update()
    {
        // ★改良＆追加（スパイダーフック）
        // （テクニック）
        // enumとswitch文を組み合わせることで、『この状態（モード）の時は、このメソッドを実行する』という仕組みを作れる。
        switch (state)
        {
            default:
            case State.Normal: // ノーマルモードの時
                if (mainCamera.enabled)
                {
                    PlayerMove();
                }
                if (subCamera.enabled)
                {
                    HookStart();
                }
                break;

            case State.ThrowingHook: // フック投げモードの時
                if (subCamera.enabled)
                {
                    HookThrow();
                }
                break;

            case State.FlyingPlayer: // プレーヤーが空中移動の時
                
                    HookFlyingMovement();
                break;
        }
    }

    // ★追加（スパイダーフック）
    void HookStart()
    {
        // マウスの右ボタンを推した時
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;

            if (Physics.Raycast(subCamera.transform.position, subCamera.transform.forward, out hit, hookRange))
            {
                // Rayで特定した位置にターゲットマーク(目印）を移動させる。
                hookTargetMark.position = hit.point;
                hookPoint = hit.point;

                state = State.ThrowingHook;
                hookShotSize = 0f;

                hookTargetMark.gameObject.SetActive(true);
                hookShot.gameObject.SetActive(true);
            }
        }
    }

    // ★追加（スパイダーフック）
    void HookFlyingMovement()
    {
        if (Target.GetFook())
        {
            Vector3 moveDir = (hookPoint - transform.position).normalized;
            // （テクニック）
            // 現在地と目的地の距離が遠いほど移動速度が早い（近くなるにつれて減速）
            float flyingSpeed = Vector3.Distance(transform.position, hookPoint) * 2f;

            con.Move(moveDir * flyingSpeed * Time.deltaTime);

            // 目標地点の近くまで来るとフックショットを自動的に隠す
            if (Vector3.Distance(transform.position, hookPoint) < 2f)
            {
                // ノーマルモードに移行する
                state = State.Normal;
                hookTargetMark.gameObject.SetActive(false);
                hookShot.gameObject.SetActive(false);
                subCamera.enabled = false;
                mainCamera.enabled = true;
            }
        }
        else
        {
            // ノーマルモードに移行する
            state = State.Normal;
            hookTargetMark.gameObject.SetActive(false);
            hookShot.gameObject.SetActive(false);
            subCamera.enabled = false;
            mainCamera.enabled = true;
        }
    }

    // ★追加（スパイダーフック）
    void HookThrow()
    {
        hookShot.LookAt(hookPoint);

        float hookShotSpeed = 50f;
        hookShotSize += hookShotSpeed * Time.deltaTime;
        hookShot.localScale = new Vector3(1, 1, hookShotSize);

        if (hookShotSize >= Vector3.Distance(transform.position, hookPoint))
        {
            // 空中移動モードに移行（空中移動を開始する）
            state = State.FlyingPlayer;
        }
    }
}