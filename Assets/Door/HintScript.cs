using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintScript : MonoBehaviour
{
    public GameObject yellowText;
    public GameObject buleText;
    public GameObject redText;
    public GameObject rightOneText;
    public GameObject rightTwoText;
  
   
    public bool onFlag;
   // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            yellowText.SetActive(true);
            buleText.SetActive(true); 
            redText.SetActive(true);
            rightOneText.SetActive(true);
            rightTwoText.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            yellowText.SetActive(false);
            buleText.SetActive(false);
            redText.SetActive(false);
            rightOneText.SetActive(false);
            rightTwoText.SetActive(false);
        }
    }
}
