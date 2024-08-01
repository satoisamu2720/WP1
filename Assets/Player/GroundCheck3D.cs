using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck3D : MonoBehaviour
{
    // �ڒn������s���Ώۃ��C���[�}�X�N
    [SerializeField] LayerMask groundLayers = 0;
    // ���_���猩��Ray�̎n�_�M�邽�߂�offset
    [SerializeField] Vector3 offset = new Vector3(0, 0.1f, 0f);
    private Vector3 direction, position;
    // Ray�̒���
    [SerializeField] float distance = 0.35f;
    /*
    Bool�ŕԂ��B
    Ray�͈̔͂�groundLayers�Ŏw�肵�����C���[�����݂��邩�ǂ���
    */
    public bool CheckGroundStatus()
    {
        // Ray�̕����B�����Ȃ̂�down
        direction = Vector3.down;
        // Ray�̎n�_�B���_ + offset
        position = transform.position + offset;
        Ray ray = new Ray(position, direction);
        // Ray��Gizmo�Ŋm�F���邽�߂�DrawRay
        Debug.DrawRay(position, direction * distance, Color.red);

        return Physics.Raycast(ray, distance, groundLayers);
    }
}