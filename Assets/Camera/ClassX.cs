using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassX : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public��NestedClass�N���X��ݒ�
    public class NestedClass
    {
        public bool aimFlag;
        public bool bimFlag;
    }

    // public��NestedClass�N���X�̃C���X�^���X���쐬
    public NestedClass nestedClassInstance; //���ꂪ����̖{��

    // Awake�ŃC���X�^���X��������
    private void Awake()
    {
        nestedClassInstance = new NestedClass(); //�d�v�ȋV��
    }

}
