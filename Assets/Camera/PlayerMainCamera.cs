using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainCamera : MonoBehaviour
{
    public GameObject playerObject;         // �v���C���[�I�u�W�F�N�g
    public float followDelay = 0.2f;        // �Ǐ]�x�����ԁi�b�j
    public Vector3 positionOffset = new Vector3(0, 2, -5); // �J�����ʒu�̃I�t�Z�b�g (X, Y, Z)
    public float collisionOffset = 0.5f;    // ��Q���Ƃ̋��������i�J�����Ə�Q���Ƃ̐ڐG��h�����߂̃I�t�Z�b�g�j
    public float rayY = 0f;
    public float rayZ = 0f;
    public float rayLength = 0f;

    private Vector3 targetPosition;         // �ڕW�ʒu�i�J�������Ǐ]����ʒu�j
    private Quaternion targetRotation;      // �ڕW��]�i�v���C���[�𒍎������]�j
    private Quaternion currentRotation;     // ���݂̉�]
    private Vector3 velocity = Vector3.zero; // ��ԗp�̑��x�iSmoothDamp�p�j

    void Start()
    {
        // �����̖ڕW�ʒu���v���C���[�̃I�t�Z�b�g���l�����Đݒ�
        targetPosition = playerObject.transform.position + playerObject.transform.TransformDirection(positionOffset);
        currentRotation = transform.rotation; // �ŏ��̉�]���L�^
    }

    void LateUpdate()
    {
        FollowPosition();
        FollowRotation();
    }

    void FollowPosition()
    {
        // �v���C���[�̉�]���l�����A��Ƀv���C���[�̌��̈ʒu�ɃJ������ݒ�
        Vector3 desiredPosition = playerObject.transform.position + playerObject.transform.TransformDirection(positionOffset);

        // ��Q��������ꍇ�A�J��������Q���̎�O�ɒ���
        Vector3 ray = new Vector3(transform.position.x, transform.position.y + rayY, transform.position.z + rayZ);
        Debug.DrawRay(ray, transform.forward * rayLength, Color.red, 1);
        RaycastHit hit;
        if (Physics.Raycast(ray, (desiredPosition - playerObject.transform.position).normalized, out hit, positionOffset.magnitude))
        {
            // ��Q�����J�����̐i�s�����ɂ���ꍇ�A��Q���̎�O�ŃJ������ݒ�
            desiredPosition = hit.point - (hit.normal * collisionOffset);
        }

        // �ڕW�ʒu�� SmoothDamp �Œx���Ǐ]
        targetPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, followDelay);

        // �J�����̈ʒu���X�V
        transform.position = targetPosition;
    }

    void FollowRotation()
    {
        // �v���C���[����ɒ�������ڕW��]���v�Z
        targetRotation = Quaternion.LookRotation(playerObject.transform.position - transform.position);

        // �v���C���[�̏㉺�ipitch�j�ƍ��E�iyaw�j�𕪂���
        float targetPitch = targetRotation.eulerAngles.x;
        float targetYaw = targetRotation.eulerAngles.y;

        // ���݂̉�]���擾
        float currentPitch = transform.rotation.eulerAngles.x;
        float currentYaw = transform.rotation.eulerAngles.y;

        // �㉺�̉�]�͑����ɔ��f�i�x���Ȃ��j
        float pitch = Mathf.LerpAngle(currentPitch, targetPitch, Time.deltaTime * 10f); // 10f�̓X���[�Y�ȕ�Ԃ̃X�s�[�h����

        // ���E�̉�]�͒x����K�p
        float yaw = Mathf.LerpAngle(currentYaw, targetYaw, Time.deltaTime / followDelay);

        // �V������]��ݒ�
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}

