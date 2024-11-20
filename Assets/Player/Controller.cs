
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
        FlyingPlayer
    }

    // ���ǉ��i�X�p�C�_�[�t�b�N�j
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


    float normalSpeed = 5f; // �ʏ펞�̈ړ����x
    float sprintSpeed = 8f; // �_�b�V�����̈ړ����x
    float jump = 5f;        // �W�����v��
    float gravity = 9.8f;    // �d�͂̑傫��
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
        // ���ǉ��i�X�p�C�_�[�t�b�N�j
        state = State.Normal;
        hookShot.gameObject.SetActive(false);
        hookTargetMark.gameObject.SetActive(false);

    }
    void PlayerMove()
    {
        // �ړ����x���擾
        float speed = Input.GetButton("Fire3") ? sprintSpeed : normalSpeed;

        // �J������������ɂ������ʕ����̃x�N�g��
        Vector3 cameraForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        // �O�㍶�E���́iWASD�L�[�j�x�N�g���v�Z
        moveZ = cameraForward * Input.GetAxis("Vertical") * speed;  //�@�O��i�J������j�@ 
        moveX = mainCamera.transform.right * Input.GetAxis("Horizontal") * speed; // ���E�i�J������j


        //// �J������������ɂ������ʕ����̃x�N�g��
        //Vector3 subcameraForward = Vector3.Scale(Camera.sub.transform.forward, new Vector3(1, 0, 1)).normalized;

        //// �O�㍶�E���́iWASD�L�[�j�x�N�g���v�Z
        //moveZ = subcameraForward * Input.GetAxis("Vertical") * speed;  //�@�O��i�J������j�@ 
        //moveX = Camera.sub.transform.right * Input.GetAxis("Horizontal") * speed; // ���E�i�J������j

        // �n�ʂɂ��邩����
        if (con.isGrounded)
        {
            // �n�ʂɂ���Ƃ��\
            moveDirection = moveZ + moveX;
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jump;
            }
        }
        else
        {
            // �d��
            moveDirection = moveZ + moveX + new Vector3(0, moveDirection.y, 0);
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // �ړ��A�j���[�V����
        anim.SetFloat("MoveSpeed", (moveZ + moveX).magnitude);

        // �v���C���[��������͂̌����ɕύX�@
        transform.LookAt(transform.position + moveX + moveZ);

        //�w�肵���x�N�g�������ړ������閽��
        con.Move(moveDirection * Time.deltaTime);

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
                // Ray�œ��肵���ʒu�Ƀ^�[�Q�b�g�}�[�N(�ڈ�j���ړ�������B
                hookTargetMark.position = hit.point;
                hookPoint = hit.point;

                state = State.ThrowingHook;
                hookShotSize = 0f;

                hookTargetMark.gameObject.SetActive(true);
                hookShot.gameObject.SetActive(true);
            }
        }
    }

    // ���ǉ��i�X�p�C�_�[�t�b�N�j
    void HookFlyingMovement()
    {
        if (Target.GetFook())
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
                hookTargetMark.gameObject.SetActive(false);
                hookShot.gameObject.SetActive(false);
                subCamera.enabled = false;
                mainCamera.enabled = true;
            }
        }
        else
        {
            // �m�[�}�����[�h�Ɉڍs����
            state = State.Normal;
            hookTargetMark.gameObject.SetActive(false);
            hookShot.gameObject.SetActive(false);
            subCamera.enabled = false;
            mainCamera.enabled = true;
        }
    }

    // ���ǉ��i�X�p�C�_�[�t�b�N�j
    void HookThrow()
    {
        hookShot.LookAt(hookPoint);

        float hookShotSpeed = 50f;
        hookShotSize += hookShotSpeed * Time.deltaTime;
        hookShot.localScale = new Vector3(1, 1, hookShotSize);

        if (hookShotSize >= Vector3.Distance(transform.position, hookPoint))
        {
            // �󒆈ړ����[�h�Ɉڍs�i�󒆈ړ����J�n����j
            state = State.FlyingPlayer;
        }
    }
}