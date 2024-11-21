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

    // publicでNestedClassクラスを設定
    public class NestedClass
    {
        public bool aimFlag;
        public bool bimFlag;
    }

    // publicでNestedClassクラスのインスタンスを作成
    public NestedClass nestedClassInstance; //これが今回の本丸

    // Awakeでインスタンスを初期化
    private void Awake()
    {
        nestedClassInstance = new NestedClass(); //重要な儀式
    }

}
