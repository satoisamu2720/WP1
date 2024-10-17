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

        //���
        if (!OpenFlagOne && one <= bottomY)
        {
            
            if(!switchTwo.onFlag && !switchThree.onFlag) {
                OpenFlagOne = true; 
            }
        }

        //���
        if (!OpenFlagTwo && two <= bottomY)
        {
            if (switchOne.onFlag && !switchThree.onFlag)
            {
                OpenFlagTwo = true;
            }
        }
  
        //�O��
        if (!OpenFlagThree && three <= bottomY)
        {
            if (switchOne.onFlag && switchTwo.onFlag)
            {
                OpenFlagThree = true;
            }
        }

      
        
        //�O�����Ă��邩
        if(switchOne.active && switchTwo.active && switchThree.active)
        {
            //���ԓ����Ȃ�����J��
            if (OpenFlagOne && OpenFlagTwo && OpenFlagThree)
            {
                door.isOpen = true;
            }//���ԓ����ł͂Ȃ������烊�Z�b�g
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
