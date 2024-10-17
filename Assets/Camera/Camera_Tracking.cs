using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Camera_Tracking : MonoBehaviour
{
    public GameObject playerObject;   //�ǔ� �I�u�W�F�N�g
    
    public Vector2 rotationSpeed;           //��]���x
    private Vector3 lastMousePosition;      //�Ō�̃}�E�X���W
    private Vector3 lastTargetPosition;     //�Ō�̒ǔ��I�u�W�F�N�g�̍��W


    private float zoom;
    public bool mainCameraFlag = true;
    public bool sabCameraFlag = false;


    void Start()
    {
        zoom = 0.0f;
        lastMousePosition = Input.mousePosition;
        if (mainCameraFlag)
        {
            lastTargetPosition = playerObject.transform.position;
        }
    }

    void Update()
    {

        Rotate();
        Zoom();

        if (Input.GetKeyDown(KeyCode.R))
        {
            sabCameraFlag = true;
            mainCameraFlag = false;
        }
    }

    void Rotate()
    {
        transform.position += playerObject.transform.position - lastTargetPosition;
        lastTargetPosition = playerObject.transform.position;

        if (Input.GetMouseButton(1))
        {

            Vector3 nowMouseValue = Input.mousePosition - lastMousePosition;

            var newAngle = Vector3.zero;
            newAngle.x = rotationSpeed.x * nowMouseValue.x;
            newAngle.y = rotationSpeed.y * nowMouseValue.y;

            transform.RotateAround(playerObject.transform.position, Vector3.up, newAngle.x);
            transform.RotateAround(playerObject.transform.position, transform.right, -newAngle.y);
        }

        lastMousePosition = Input.mousePosition;
    }

    //�g��k��
    void Zoom()
    {
        zoom = Input.GetAxis("Mouse ScrollWheel");
        Vector3 offset = new Vector3(0, 0, 0);
        Vector3 pos = playerObject.transform.position - transform.position;

        if (zoom > 0)
        {
            offset = pos.normalized * 1;
        }
        else if (zoom < 0)
        {
            offset = -pos.normalized * 1;

        }
        transform.position = transform.position + offset;
    }

}
