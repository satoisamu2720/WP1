using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TreeEditor.TreeGroup;

public class HookLock : MonoBehaviour
{
    public static HookLock instance;
    public bool hookFlag;
    // Start is called before the first frame update
    void Start()
    {
        hookFlag = false;
    }
    public bool GetFook()
    {
        return hookFlag;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("HookLock"))
    //    {
    //        Debug.Log("地面と当たった！");
    //        hookFlag = true;
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ("HookLock"))
        {
            Debug.Log("地面と当たった！");
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("HookLock"))
    //    {
    //        hookFlag = false;
    //    }
    //}
}
