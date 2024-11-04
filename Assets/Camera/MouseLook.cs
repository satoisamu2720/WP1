using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity;  //���x
    public Transform player;
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        // �J�[�\���L�[���\���ɂ���B
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // ���E�̓���
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime; // �㉺�̓���

        // �㉺�^��
        xRotation -= mouseY;
        yRotation += mouseX;
        

        // �㉺�^���i�ړ��ł���p�x�j�ɐ�����������B
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        //yRotation = Mathf.Clamp(-90f, yRotation, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // ���E�^��
        //player.Rotate(Vector3.up * mouseX);
    }
}