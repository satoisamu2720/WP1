using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

    public class OrbitalTest : CinemachineExtension
    {
        private const float RotateSpeed = 2f;

        private Vector2 _currentRotate;
        private Transform _lookAt;

        private CinemachineFreeLook _virtualCamera;

        private void Update()
        {
            // �ȈՓI�ɑ�����󂯕t���Ă݂�
            if (Input.GetKey(KeyCode.LeftArrow)) Rotation(new Vector2(1f, 0f));

            if (Input.GetKey(KeyCode.RightArrow)) Rotation(new Vector2(-1f, 0f));

            if (Input.GetKey(KeyCode.UpArrow)) Rotation(new Vector2(0f, 1f));

            if (Input.GetKey(KeyCode.DownArrow)) Rotation(new Vector2(0f, -1f));

            // ���E�㉺��]����
            void Rotation(Vector2 diffDelta)
            {
                diffDelta *= RotateSpeed * Time.deltaTime * 30;
                _currentRotate.x += diffDelta.y;
                _currentRotate.y += diffDelta.x;
            }
        }

        protected override void ConnectToVcam(bool connect)
        {
            base.ConnectToVcam(connect);
            if (connect == false) return;

            // ����������
            // VirtualCamera��LookAt�Ώۂ𐶐����Ă����B�킴�킴�������Ȃ��Ă����ڑΏە���ݒ���B
            _lookAt = GameObject.Find("LookPos")?.transform;
            if (_lookAt == null)
            {
                _lookAt = new GameObject("LookPos").transform;
                _lookAt.position = new Vector3(0f, 0.5f, 0f);
            }

            _virtualCamera = VirtualCamera as CinemachineFreeLook;
            _virtualCamera.LookAt = _lookAt;
        }

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            var (pos, rotate) = GetTransform(_virtualCamera.LookAt.localToWorldMatrix);
            state.RawPosition = pos;
            state.RawOrientation = rotate;
        }

        /// <summary>
        /// �J������Transform���擾
        /// </summary>
        private (Vector3 pos, Quaternion rotate) GetTransform(Matrix4x4 baseCenterMatrix)
        {
            // ���S�_����]������
            var centerMatrix = baseCenterMatrix * Matrix4x4.Rotate(Quaternion.AngleAxis(_currentRotate.y, Vector3.up) *
                                                                   Quaternion.AngleAxis(_currentRotate.x, Vector3.right));

            // ��]�Ɋ�Â����J�����̈ʒu���擾
            var distanceVector = new Vector3(0f, 0f, -3f);
            var pos = centerMatrix.MultiplyPoint3x4(distanceVector);

            return (pos, centerMatrix.rotation);
        }
    }
