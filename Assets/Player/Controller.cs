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
        // �ڒn��Ԃł̂ݑ��x�̐؂�ւ��⃂�[�V�����ύX���s��
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
        // �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
        Vector3 moveForward = cameraForward * verticalInput + Camera.main.transform.right * horizontalInput;
        // �ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂��B
        playerRb.velocity = moveForward * moveSpeed + new Vector3(0, playerRb.velocity.y, 0);

        //_animator.SetBool("input", walkInput);
        //_animator.SetBool("jogOn", jogOn);
        //_animator.SetBool("runOn", runOn);
        //_animator.SetBool("jumpOn", jumpOn);
        //_animator.SetBool("flying", flying);
        //_animator.SetBool("onGround", onGround);

        // �L�����N�^�[�̌�����i�s������
        if (moveForward != Vector3.zero)
        {
            Quaternion from = transform.rotation;
            Quaternion to = Quaternion.LookRotation(moveForward);
            transform.rotation = Quaternion.RotateTowards(from, to, RotateSpeed * Time.deltaTime);
        }
    }
    /*
    Collider�ɐG�ꂽ���̂�Raycast�Őڒn������s��
    */
    private void OnCollisionEnter(Collision other)
    {
        // Collider���n�ʂɐG�ꂽ��
        //if (other.gameObject.CompareTag("Ground"))
       
            if (rightGround.CheckGroundStatus() || leftGround.CheckGroundStatus())
            {
                // ���E�̑��ǂ��炩���ڒn����ɂȂ����ꍇjump���[�V��������ߐڒn����
                jumpOn = false;
                flying = false;
                onGround = true;
            }
            else
            {
                // Collider���n�ʂɐG��Ă��ڒn����łȂ���ΐڒn���Ȃ�
                onGround = false;
                flying = true;
            }


        
    }
}