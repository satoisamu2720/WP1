
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class playerMove : MonoBehaviour
{
    public CharacterController con;
    public Animator anim;


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

    }
    void Update()
    {
        // �ړ����x���擾
        float speed = Input.GetButton("Fire3") ? sprintSpeed : normalSpeed;

        // �J������������ɂ������ʕ����̃x�N�g��
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // �O�㍶�E���́iWASD�L�[�j�x�N�g���v�Z
        moveZ = cameraForward * Input.GetAxis("Vertical") * speed;  //�@�O��i�J������j�@ 
        moveX = Camera.main.transform.right * Input.GetAxis("Horizontal") * speed; // ���E�i�J������j


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


}
