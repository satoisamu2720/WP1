using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    public DoorScript door;
    public SwitchScript switchOne;
    public SwitchScript switchTwo;
    public SwitchScript switchThree;

    public bool OpenFlagOne = false;
    public bool OpenFlagTwo = false;
    public bool OpenFlagThree = false;
    float one;
    float two;
    float three;
    float bottomY = -0.1f;

    public float count = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        one = switchOne.transform.position.y;
        two = switchTwo.transform.position.y;
        three = switchThree.transform.position.y;

        //一個目
        if (!OpenFlagOne && one <= bottomY)
        {
            
            if(!switchTwo.onFlag && !switchThree.onFlag) {
                OpenFlagOne = true; 
            }
        }

        //二個目
        if (!OpenFlagTwo && two <= bottomY)
        {
            if (switchOne.onFlag && !switchThree.onFlag)
            {
                OpenFlagTwo = true;
            }
        }
  
        //三個目
        if (!OpenFlagThree && three <= bottomY)
        {
            if (switchOne.onFlag && switchTwo.onFlag)
            {
                OpenFlagThree = true;
            }
        }

      
        
        //三個押しているか
        if(switchOne.active && switchTwo.active && switchThree.active)
        {
            //順番道理なら扉が開く
            if (OpenFlagOne && OpenFlagTwo && OpenFlagThree)
            {
                door.isOpen = true;
            }//順番道理ではなかったらリセット
            else if( three <= bottomY)
            {
                switchOne.active = false;
                switchTwo.active = false;
                switchThree.active = false;

                switchOne.onFlag = false;
                switchTwo.onFlag = false;
                switchThree.onFlag = false;

                OpenFlagOne = false;
                OpenFlagTwo = false;
                OpenFlagThree = false;
                one = 0.15f;
                two = 0.15f;
                three = 0.15f;
            }
         
        }


    }
}
