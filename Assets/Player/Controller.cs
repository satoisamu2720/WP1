using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float jogSpeed = 7f;
    [SerializeField] float runSpeed = 10f;
    private Rigidbody playerRb;
    public Animator _animator;
    private bool walkInput = false;
    private bool runOn = false;
    private bool jogOn = false;
    private bool flying = false;
    public bool onGround = true;
    private bool jumpOn = false;
    private const float RotateSpeed = 900f;
    public GroundCheck3D rightGround, leftGround;
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        // 接地状態でのみ速度の切り替えやモーション変更を行う
        if (onGround)
        {
            if ((horizontalInput == 0.0f) && (verticalInput == 0.0f))
            {
                walkInput = false;
            }
            else
            {
                walkInput = true;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = runSpeed;
                runOn = true;
            }
            else
            {
                moveSpeed = walkSpeed;
                if (jogOn)
                {
                    moveSpeed = jogSpeed;
                }
                runOn = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (!jogOn)
                {
                    jogOn = true;
                    moveSpeed = jogSpeed;
                }
                else
                {
                    jogOn = false;
                    moveSpeed = walkSpeed;
                }
            }

            if (Input.GetButtonDown("Jump"))
            {
                playerRb.velocity = Vector3.up * 10;
                //_animator.SetBool("jumpOn", true);
                jumpOn = true;
                walkInput = false;
                onGround = false;
            }
        }
    }

    void FixedUpdate()
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * verticalInput + Camera.main.transform.right * horizontalInput;
        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        playerRb.velocity = moveForward * moveSpeed + new Vector3(0, playerRb.velocity.y, 0);

        //_animator.SetBool("input", walkInput);
        //_animator.SetBool("jogOn", jogOn);
        //_animator.SetBool("runOn", runOn);
        //_animator.SetBool("jumpOn", jumpOn);
        //_animator.SetBool("flying", flying);
        //_animator.SetBool("onGround", onGround);

        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            Quaternion from = transform.rotation;
            Quaternion to = Quaternion.LookRotation(moveForward);
            transform.rotation = Quaternion.RotateTowards(from, to, RotateSpeed * Time.deltaTime);
        }
    }
    /*
    Colliderに触れた時のみRaycastで接地判定を行う
    */
    private void OnCollisionEnter(Collision other)
    {
        // Colliderが地面に触れた時
        //if (other.gameObject.CompareTag("Ground"))
       
            if (rightGround.CheckGroundStatus() || leftGround.CheckGroundStatus())
            {
                // 左右の足どちらかが接地判定になった場合jumpモーションをやめ接地する
                jumpOn = false;
                flying = false;
                onGround = true;
            }
            else
            {
                // Colliderが地面に触れても接地判定でなければ接地しない
                onGround = false;
                flying = true;
            }


        
    }
}