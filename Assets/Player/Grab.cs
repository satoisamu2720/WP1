using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField] private Transform grabPoint;
    [SerializeField] private Transform rayPoint;
    private float rayDistance = 0.2f;
    private GameObject grabObj;
    RaycastHit2D hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (grabObj == null)
            {
                
                if (hit.collider != null && hit.collider.tag == "Box")
                {
                    grabObj = hit.collider.gameObject;
                    grabObj.GetComponent<Rigidbody2D>().isKinematic = true;
                    grabObj.transform.position = grabPoint.position;
                    grabObj.transform.SetParent(transform);
                }
            }
            else
            {
                grabObj.GetComponent<Rigidbody2D>().isKinematic = false;
                grabObj.transform.SetParent(null);
                grabObj = null;
            }
        }
    }
}

