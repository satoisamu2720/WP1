
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class playerMove : MonoBehaviour
{


    // ���ǉ��i�X�p�C�_�[�t�b�N�j
    private enum State
    {
        Normal,
        ThrowingHook,
        FlyingPlayer,
        ThrowingHookWW
    }

    // ���ǉ��i�X�p�C�_�[�t�b�N�j
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
    public float speed = 5f;          // �ʏ�ړ����x
    public float sprintSpeed = 8f;    // �_�b�V�����x
    public float jump = 8f;           // �W�����v��
    public float gravity = 9.8f;      // �d��
    public float wallClimbSpeed = 3f; // �Ǔo�葬�x
    public float rayY = 0f;

    private Vector3 moveDirection;    // �ړ��x�N�g��
    private bool isClimbingWall = false; // �Ǔo�蔻��

    float normalSpeed = 5f; // �ʏ펞�̈ړ����x
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
        // ���ǉ��i�X�p�C�_�[�t�b�N�j
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

       

        if (Physics.Raycast(ray, transform.forward, out hit, 1f))  // �ǂ�T���i�O��1���j�b�g�j
        {
            if (hit.collider.CompareTag("WallClimb"))
            {
                isClimbingWall = true;
                 // �ǂɓo���Ă�����
                 moveDirection = new Vector3(Input.GetAxis("Horizontal") * wallClimbSpeed, Input.GetAxis("Vertical") * wallClimbSpeed, 0); // �㉺�ړ��i�Ǔo��j
                                                                                                   // �w�肵���x�N�g�������ړ������閽��
                con.Move(moveDirection * Time.deltaTime);

                // �ړ��A�j���[�V����
                anim.SetFloat("MoveSpeed", (Vector3.zero + Vector3.zero).magnitude);
            }
        }
        else
        {
            isClimbingWall = false;
        }


        if (!isClimbingWall)
        {
            // �ړ����x���擾
            float currentSpeed = Input.GetButton("Fire3") ? sprintSpeed : speed;

            // �J������������ɂ������ʕ����̃x�N�g��
            Vector3 cameraForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 cameraRight = Vector3.Scale(mainCamera.transform.right, new Vector3(1, 0, 1)).normalized;

            // �O�㍶�E���́iWASD�L�[�j�x�N�g���v�Z
            moveZ = cameraForward * Input.GetAxis("Vertical") * currentSpeed;  // �O��i�J������j
            moveX = cameraRight * Input.GetAxis("Horizontal") * currentSpeed;  // ���E�i�J������j

            // �ړ��A�j���[�V����
            anim.SetFloat("MoveSpeed", (moveZ + moveX).magnitude);

            // �v���C���[��������͂̌����ɕύX
            transform.LookAt(transform.position + moveZ + moveX);

            // �w�肵���x�N�g�������ړ������閽��
            con.Move(moveDirection * Time.deltaTime);

        }
            // �n�ʂɂ��邩�ǂɐڐG���Ă��邩����
            if (con.isGrounded)
            {
                // �n�ʂɂ���Ƃ�
                moveDirection = moveZ + moveX;

                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jump;
                }

            }

            
            else
            {
                // �d�͂�K�p
                moveDirection = moveZ + moveX + new Vector3(0, moveDirection.y, 0);
                moveDirection.y -= gravity * Time.deltaTime;
            }

        
        // WallClimb�̔�����s��
    }
    private void Update()
    {

        // �����ǁ��ǉ��i�X�p�C�_�[�t�b�N�j
        // �i�e�N�j�b�N�j
        // enum��switch����g�ݍ��킹�邱�ƂŁA�w���̏�ԁi���[�h�j�̎��́A���̃��\�b�h�����s����x�Ƃ����d�g�݂�����B
        switch (state)
        {
            default:
            case State.Normal: // �m�[�}�����[�h�̎�
                if (mainCamera.enabled)
                {
                    PlayerMove();
                }
                if (subCamera.enabled)
                {
                    HookStart();
                }
                break;

            case State.ThrowingHook: // �t�b�N�������[�h�̎�
                if (subCamera.enabled)
                {
                    HookThrow();
                }
                break;

            case State.FlyingPlayer: // �v���[���[���󒆈ړ��̎�

                HookFlyingMovement();
                break;

            case State.ThrowingHookWW: // �t�b�N�������[�h�̎�
                if (subCamera.enabled)
                {
                    HookThrowWW();
                }
                break;
        }
    }

    // ���ǉ��i�X�p�C�_�[�t�b�N�j
    void HookStart()
    {
        // �}�E�X�̉E�{�^���𐄂�����
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;

            if (Physics.Raycast(subCamera.transform.position, subCamera.transform.forward, out hit, hookRange))
            {
                if (hit.collider.CompareTag("Target"))
                {
                    // Ray�œ��肵���ʒu�Ƀ^�[�Q�b�g�}�[�N(�ڈ�j���ړ�������B
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

    // ���ǉ��i�X�p�C�_�[�t�b�N�j
    void HookFlyingMovement()
    {



        Vector3 moveDir = (hookPoint - transform.position).normalized;
        // �i�e�N�j�b�N�j
        // ���ݒn�ƖړI�n�̋����������قǈړ����x�������i�߂��Ȃ�ɂ�Č����j
        float flyingSpeed = Vector3.Distance(transform.position, hookPoint) * 2f;

        con.Move(moveDir * flyingSpeed * Time.deltaTime);

        // �ڕW�n�_�̋߂��܂ŗ���ƃt�b�N�V���b�g�������I�ɉB��
        if (Vector3.Distance(transform.position, hookPoint) < 2f)
        {
            // �m�[�}�����[�h�Ɉڍs����
            state = State.Normal;
            //hookTargetMark.gameObject.SetActive(false);
            hookShot.gameObject.SetActive(false);
            hookShotSizeFlag = false;
        }
    }

    // ���ǉ��i�X�p�C�_�[�t�b�N�j
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
            // �󒆈ړ����[�h�Ɉڍs�i�󒆈ړ����J�n����j
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

            // �m�[�}�����[�h�Ɉڍs����
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