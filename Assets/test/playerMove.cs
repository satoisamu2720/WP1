using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class playerMove : MonoBehaviour
{
    public CharacterController con;
    public Animator anim;
  

    float normalSpeed = 3f; // �ʏ펞�̈ړ����x
    float sprintSpeed = 5f; // �_�b�V�����̈ړ����x
    float jump = 10f;        // �W�����v��
    float gravity = 10f;    // �d�͂̑傫��
    float bulletTimer = 0;
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

        // �J�����̌�������ɂ������ʕ����̃x�N�g��
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // �O�㍶�E�̓��́iWASD�L�[�j����A�ړ��̂��߂̃x�N�g�����v�Z
        // Input.GetAxis("Vertical") �͑O��iWS�L�[�j�̓��͒l
        // Input.GetAxis("Horizontal") �͍��E�iAD�L�[�j�̓��͒l
        moveZ = cameraForward * Input.GetAxis("Vertical") * speed;  //�@�O��i�J������j�@ 
        moveX = Camera.main.transform.right * Input.GetAxis("Horizontal") * speed; // ���E�i�J������j

        // isGrounded �͒n�ʂɂ��邩�ǂ����𔻒肵�܂�
        // �n�ʂɂ���Ƃ��̓W�����v���\��
        if (con.isGrounded)
        {
            moveDirection = moveX + moveZ;


            //if (Input.GetButtonDown("Jump"))
            //{
            //   moveDirection.y = jump;
            //}
        }
        else
        {
            // �d�͂���������
            moveDirection = moveX + moveZ + new Vector3(0, moveDirection.y, 0);
            moveDirection.y -= gravity * Time.deltaTime;

            
        }

        // �ړ��̃A�j���[�V����
        anim.SetFloat("MoveSpeed", (moveX + moveZ).magnitude);

        // �v���C���[�̌�������͂̌����ɕύX�@
        transform.LookAt(transform.position + moveX + moveZ);

        // Move �͎w�肵���x�N�g�������ړ������閽��
        con.Move(moveDirection * Time.deltaTime);

    }
}
