using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Camera_Tracking : MonoBehaviour
{
    public GameObject playerObject;   // �ǔ��I�u�W�F�N�g�i�v���C���[�j

    public Vector2 rotationSpeed;           // ��]���x
    private Vector3 lastMousePosition;      // �Ō�̃}�E�X���W

    public bool mainCameraFlag = true;
    public bool sabCameraFlag = false;

    public Camera subCamera;               // �T�u�J�����i�Ǐ]����J�����j
    public Camera mainCamera;
    private float xRotation = 0f;          // �㉺�̉�]
    private float yRotation = 0f;          // ���E�̉�]

    public float mouseSensitivity = 100f;  // �}�E�X���x
    public float upDownLimit = 60f;        // �㉺��]�̐����i�p�x�j

    // �J�����ʒu�𒲐����邽�߂̃I�t�Z�b�g
    public Vector3 cameraOffset = new Vector3(0f, 2.0f, 0f); // �����ʒu

    void Start()
    {
        lastMousePosition = Input.mousePosition;
        if (mainCamera.enabled)
        {
            // �T�u�J�����̓v���C���[�̈ʒu�ɌŒ�
           
            subCamera.transform.position = playerObject.transform.position + cameraOffset;
            subCamera.transform.rotation = playerObject.transform.rotation;
        }

        // �J�[�\�����B���ă��b�N
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (subCamera.enabled)
        {
            // �T�u�J��������l�̎��_�ɐݒ�
            RotateCameraWithMouse();
            FollowPlayer();
            AdjustCameraPosition(); // �J�����ʒu�����̏���

        }
        if (mainCamera.enabled)
        {
            FacePlayerDirection(); // �v���C���[�̌���������
        
        }
    }

    void RotateCameraWithMouse()
    {
        // �}�E�X�̓����ɂ���]����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // ���E�̓���
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; // �㉺�̓���

        // �㉺��]
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -upDownLimit, upDownLimit); // �㉺�̉�]����

        // ���E��]
        yRotation += mouseX;

        // �v���C���[�̉�]���X�V�i�T�u�J�����̉�]���X�V�j
        subCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void FollowPlayer()
    {
        // �T�u�J�����̈ʒu�̓v���C���[�̈ʒu�ɌŒ�
        subCamera.transform.position = playerObject.transform.position + cameraOffset;
    }

    void AdjustCameraPosition()
    {
        // ���[�U�[���͂ŃJ�����ʒu�𒲐�
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 2f;  // ���E
        float moveY = Input.GetAxis("Vertical") * Time.deltaTime * 2f;    // �O��

        // �ʒu������ɍēx�J�����ʒu��ݒ�
        subCamera.transform.position = playerObject.transform.position + cameraOffset;
    }

    void FacePlayerDirection()
    {
        // �v���C���[�̌����ɍ��킹�ăJ��������]����悤�ɂ���
        Vector3 direction = playerObject.transform.forward; // �v���C���[�̌���
        Quaternion targetRotation = Quaternion.LookRotation(direction); // �v���C���[�������Ă������������

        // �J�������v���C���[�̌����ɍ��킹��
        subCamera.transform.rotation = Quaternion.Slerp(subCamera.transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

}