using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //‰ñ“]‘¬“x
    public float rotationSpeed = 1f;
    //x²‰ñ“]Šp“x‚ÌÅ‘å’l
    public float max_rotation_x = 60f;
    //Œ»İ‚Ì‰ñ“]Šp“x
    private float rotation_x = 0f;
    private float rotation_y = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //‰ñ“]Šp“x‚ğ•ÏX
            rotation_y -= rotationSpeed;
            //y²‚ğ²‚É¶‰ñ‚è‚ÉrotationSpeed“x‰ñ“]
            transform.rotation = Quaternion.Euler(rotation_x, rotation_y, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //‰ñ“]Šp“x‚ğ•ÏX
            rotation_y += rotationSpeed;
            //y²‚ğ²‚É¶‰ñ‚è‚ÉrotationSpeed“x‰ñ“]
            transform.rotation = Quaternion.Euler(rotation_x, rotation_y, 0);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            //ƒJƒƒ‰‚Ìc•ûŒü‚ÌŠp“x‚Ì”ÍˆÍ‚ğw’è
            if (rotation_x < -max_rotation_x)
            {
                //”ÍˆÍŠO‚Ì‚Æ‚«return
                return;
            }
            //‰ñ“]Šp“x‚ğ•ÏX
            rotation_x -= rotationSpeed;
            //x²‚ğ²‚Éã•ûŒü‚É‰ñ“]
            transform.rotation = Quaternion.Euler(rotation_x, rotation_y, 0);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            //ƒJƒƒ‰‚Ìc•ûŒü‚ÌŠp“x‚Ì”ÍˆÍ‚ğw’è
            if (rotation_x > max_rotation_x)
            {
                //”ÍˆÍŠO‚Ì‚Æ‚«return
                return;
            }
            //‰ñ“]Šp“x‚ğ•ÏX
            rotation_x += rotationSpeed;
            //x²‚ğ²‚Éã•ûŒü‚É‰ñ“]
            transform.rotation = Quaternion.Euler(rotation_x, rotation_y, 0);
        }
    }
}