using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSample : MonoBehaviour
{

    private GameObject player;   //�v���C���[���i�[�p
    private Vector3 offset;      //���΋����擾�p

    // Use this for initialization
    void Start()
    {

        //�v���C���[�̏����擾
        this.player = GameObject.Find("Player");

        // MainCamera(�������g)��player�Ƃ̑��΋��������߂�
        offset = transform.position - player.transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        //�V�����g�����X�t�H�[���̒l��������
        transform.position = player.transform.position + offset;


        //���j�e�B�����̌����Ɠ����悤�ɃJ�����̌�����ύX����
       // transform.rotation = player.transform.rotation;
    }
}