using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //��]���x
    public float rotationSpeed = 1f;
    //x����]�p�x�̍ő�l
    public float max_rotation_x = 60f;
    //���݂̉�]�p�x
    private float rotation_x = 0f;
    private float rotation_y = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //��]�p�x��ύX
            rotation_y -= rotationSpeed;
            //y�������ɍ�����rotationSpeed�x��]
            transform.rotation = Quaternion.Euler(rotation_x, rotation_y, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //��]�p�x��ύX
            rotation_y += rotationSpeed;
            //y�������ɍ�����rotationSpeed�x��]
            transform.rotation = Quaternion.Euler(rotation_x, rotation_y, 0);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            //�J�����̏c�����̊p�x�͈̔͂��w��
            if (rotation_x < -max_rotation_x)
            {
                //�͈͊O�̂Ƃ�return
                return;
            }
            //��]�p�x��ύX
            rotation_x -= rotationSpeed;
            //x�������ɏ�����ɉ�]
            transform.rotation = Quaternion.Euler(rotation_x, rotation_y, 0);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            //�J�����̏c�����̊p�x�͈̔͂��w��
            if (rotation_x > max_rotation_x)
            {
                //�͈͊O�̂Ƃ�return
                return;
            }
            //��]�p�x��ύX
            rotation_x += rotationSpeed;
            //x�������ɏ�����ɉ�]
            transform.rotation = Quaternion.Euler(rotation_x, rotation_y, 0);
        }
    }
}