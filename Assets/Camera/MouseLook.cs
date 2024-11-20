using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity;  //���x
    public Transform player;
    private float xRotation = 0f;
    private float yRotation = 0f;
    public Camera subCamera;

    void Start()
    {
        // �J�[�\���L�[���\���ɂ���B
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (subCamera.enabled)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // ���E�̓���
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; // �㉺�̓���

            // �㉺�^��
            xRotation -= mouseY;
            yRotation += mouseX;

            // �㉺�^���i�ړ��ł���p�x�j�ɐ�����������B
            xRotation = Mathf.Clamp(xRotation, -60f, 45f);
            yRotation = Mathf.Clamp(yRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }
}