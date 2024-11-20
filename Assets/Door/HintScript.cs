using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static TreeEditor.TreeGroup;

public class HintScript : MonoBehaviour
{
    //public GameObject yellowText;
    //public GameObject buleText;
    //public GameObject redText;
    //public GameObject rightOneText;
    //public GameObject rightTwoText;
    //public Collider Box;
   
    public bool onFlag;
   // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            Debug.Log("地面と当たった！");
        }
    }

    //private void On

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        //yellowText.SetActive(false);
    //        //buleText.SetActive(false);
    //        //redText.SetActive(false);
    //        //rightOneText.SetActive(false);
    //        //rightTwoText.SetActive(false);
    //    }
    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Debug.Log("地面と当たった！");
    //        onFlag = true;
    //    }
    //}
}
