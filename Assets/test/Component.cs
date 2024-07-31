using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Component : MonoBehaviour
{
    public Animator anim;
    public CharacterController con;

    private void Awake()
    {
        TryGetComponent(out anim);
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var velocity = new Vector3(horizontal, 0, vertical).normalized;
        var speed = Input.GetKey(KeyCode.LeftShift) ? 5 : 3;
        var gravity = 10f;    // d—Í‚Ì‘å‚«‚³

        Vector3 moveDirection = Vector3.zero;

        if (con.isGrounded)
        {
        }
        else
        {
            // d—Í‚ðŒø‚©‚¹‚é
           
            velocity.y -= gravity * Time.deltaTime;
        }


        if (velocity.magnitude > 0.5f)
        { 
            transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
        }

        anim.SetFloat("MoveSpeed", velocity.magnitude * speed, 0.1f, Time.deltaTime);

        con.Move(velocity * Time.deltaTime);
    }
}
