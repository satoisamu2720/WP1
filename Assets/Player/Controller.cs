
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
        FlyingPlayer,
        ThrowingHookWW
    }

    // ★追加（スパイダーフック）
    public float hookRange = 100f;
    public Transform hookTargetMark;
    public Transform hookShot;
    public bool hookShotSizeFlag;
    private Vector3 hookPoint;
    private float hookShotSize;
    private State state;
    public CharacterController con;
    public Animator anim;
    public Camera subCamera;
    public Camera mainCamera;
    public GameObject aim;


    HookLock Target;
    public float speed = 5f;          // 通常移動速度
    public float sprintSpeed = 8f;    // ダッシュ速度
    public float jump = 8f;           // ジャンプ力
    public float gravity = 9.8f;      // 重力
    public float wallClimbSpeed = 3f; // 壁登り速度
    public float rayY = 0f;

    private Vector3 moveDirection;    // 移動ベクトル
    private bool isClimbingWall = false; // 壁登り判定

    float normalSpeed = 5f; // 通常時の移動速度
    float bulletTimer = 0;
    bool moveFlag;

    public bool onGround = true;

    Vector3 moveZ;
    Vector3 moveX;


    Vector3 startPos;

    cameraChange cameraChange;

    void Start()
    {
        startPos = transform.position;
        // ★追加（スパイダーフック）
        state = State.Normal;
        hookShot.gameObject.SetActive(false);
        hookTargetMark.gameObject.SetActive(false);
        hookShotSizeFlag = false;

    }
    void PlayerMove()
    {

        Vector3 ray = new Vector3(transform.position.x, transform.position.y + rayY, transform.position.z);
        //Debug.DrawRay(ray, transform.forward * 1, Color.red, 1);
        //Debug.DrawRay(transform.position, Vector3.down * 1, Color.red, 1);
        RaycastHit hit;

       

        if (Physics.Raycast(ray, transform.forward, out hit, 1f))  // 壁を探す（前方1ユニット）
        {
            if (hit.collider.CompareTag("WallClimb"))
            {
                isClimbingWall = true;
                 // 壁に登っている状態
                 moveDirection = new Vector3(Input.GetAxis("Horizontal") * wallClimbSpeed, Input.GetAxis("Vertical") * wallClimbSpeed, 0); // 上下移動（壁登り）
                                                                                                   // 指定したベクトルだけ移動させる命令
                con.Move(moveDirection * Time.deltaTime);

                // 移動アニメーション
                anim.SetFloat("MoveSpeed", (Vector3.zero + Vector3.zero).magnitude);
            }
        }
        else
        {
            isClimbingWall = false;
        }


        if (!isClimbingWall)
        {
            // 移動速度を取得
            float currentSpeed = Input.GetButton("Fire3") ? sprintSpeed : speed;

            // カメラ向きを基準にした正面方向のベクトル
            Vector3 cameraForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 cameraRight = Vector3.Scale(mainCamera.transform.right, new Vector3(1, 0, 1)).normalized;

            // 前後左右入力（WASDキー）ベクトル計算
            moveZ = cameraForward * Input.GetAxis("Vertical") * currentSpeed;  // 前後（カメラ基準）
            moveX = cameraRight * Input.GetAxis("Horizontal") * currentSpeed;  // 左右（カメラ基準）

            // 移動アニメーション
            anim.SetFloat("MoveSpeed", (moveZ + moveX).magnitude);

            // プレイヤー向きを入力の向きに変更
            transform.LookAt(transform.position + moveZ + moveX);

            // 指定したベクトルだけ移動させる命令
            con.Move(moveDirection * Time.deltaTime);

        }
            // 地面にいるか壁に接触しているか判定
            if (con.isGrounded)
            {
                // 地面にいるとき
                moveDirection = moveZ + moveX;

                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jump;
                }

            }

            
            else
            {
                // 重力を適用
                moveDirection = moveZ + moveX + new Vector3(0, moveDirection.y, 0);
                moveDirection.y -= gravity * Time.deltaTime;
            }

        
        // WallClimbの判定を行う
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

            case State.ThrowingHookWW: // フック投げモードの時
                if (subCamera.enabled)
                {
                    HookThrowWW();
                }
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
                if (hit.collider.CompareTag("Target"))
                {
                    // Rayで特定した位置にターゲットマーク(目印）を移動させる。
                    //hookTargetMark.position = hit.point;
                    hookPoint = hit.point;

                    state = State.ThrowingHook;
                    hookShotSize = 0f;

                    //hookTargetMark.gameObject.SetActive(true);
                    hookShot.gameObject.SetActive(true);
                }
                else
                {
                    hookPoint = hit.point;
                    state = State.ThrowingHookWW;
                    hookShotSize = 0f;
                    hookShot.gameObject.SetActive(true);
                }
            }
        }
    }

    // ★追加（スパイダーフック）
    void HookFlyingMovement()
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
            //hookTargetMark.gameObject.SetActive(false);
            hookShot.gameObject.SetActive(false);
            hookShotSizeFlag = false;
        }
    }

    // ★追加（スパイダーフック）
    void HookThrow()
    {
        hookShot.LookAt(hookPoint);

        float hookShotSpeed = 30f;

        if (!hookShotSizeFlag)
        {
            hookShotSize += hookShotSpeed * Time.deltaTime;
        }

        hookShot.localScale = new Vector3(1, 1, hookShotSize);

        if (hookShotSize >= Vector3.Distance(transform.position, hookPoint))
        {
            subCamera.enabled = false;
            mainCamera.enabled = true;
            aim.SetActive(false);
            hookShotSizeFlag = true;
            // 空中移動モードに移行（空中移動を開始する）
            state = State.FlyingPlayer;
        }
    }

    void HookThrowWW()
    {
        hookShot.LookAt(hookPoint);

        float hookShotSpeed = 30f;
        if (!hookShotSizeFlag)
        {
            hookShotSize += hookShotSpeed * Time.deltaTime;
        }
        hookShot.localScale = new Vector3(1, 1, hookShotSize);

        if (hookShotSize >= Vector3.Distance(transform.position, hookPoint))
        {

            // ノーマルモードに移行する
            hookShotSizeFlag = true;
            state = State.Normal;
            //hookTargetMark.gameObject.SetActive(false);
            hookShot.gameObject.SetActive(false);
            subCamera.enabled = false;
            mainCamera.enabled = true;
            aim.SetActive(false);
            hookShotSizeFlag = false;
        }
    }

}