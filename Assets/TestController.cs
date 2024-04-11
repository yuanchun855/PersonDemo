using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    // Start is called before the first frame update
    private float _speed = 2.5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {

            transform.Translate(Vector3.left * Time.deltaTime * _speed);
        }

        //向右
        if (Input.GetKey(KeyCode.D))
        {

            transform.Translate(Vector3.right * Time.deltaTime * _speed);
        }
        //向前
        if (Input.GetKey(KeyCode.W))
        {

            transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        }
        //向后
        if (Input.GetKey(KeyCode.S))
        {

            transform.Translate(Vector3.back * Time.deltaTime * _speed);
        }
    }
}
