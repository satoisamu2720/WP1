using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObjScript : MonoBehaviour
{
    GameObject targetObj;
    Vector3 targetPos;

    void Start()
    {
        targetObj = GameObject.Find("TargetGameObject");
        targetPos = targetObj.transform.position;
    }

    void Update()
    {
        // target�̈ړ��ʕ��A�����i�J�����j���ړ�����
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;

        // �}�E�X�̉E�N���b�N�������Ă����
        if (Input.GetMouseButton(1))
        {
            // �}�E�X�̈ړ���
            float mouseInputX = Input.GetAxis("Mouse X");
            float mouseInputY = Input.GetAxis("Mouse Y");
            // target�̈ʒu��Y���𒆐S�ɁA��]�i���]�j����
            transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * 200f);
            // �J�����̐����ړ��i���p�x�����Ȃ��A�K�v��������΃R�����g�A�E�g�j
            //transform.RotateAround(targetPos, transform.right, mouseInputY * Time.deltaTime * 200f);
        }
    }
}
