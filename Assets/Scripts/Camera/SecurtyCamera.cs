using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurtyCamera : MonoBehaviour
{
    GameObject playerDetect = null;
    public bool rotate;
    public float speed;
    public int angle;
    void Update()
    {      
        if (!rotate)
        {
            transform.Rotate(Vector3.up * speed * Time.deltaTime);
            if (transform.rotation == Quaternion.Euler(0, angle, 0))
            {
                rotate = true;
            }
        }
        else
        {
            transform.Rotate(Vector3.down * speed * Time.deltaTime);
            if (transform.rotation == Quaternion.Euler(0, -angle, 0))
            {
                rotate = false;
            }
        }            
    }
    public void OnChildTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerDetect = other.gameObject;
            print("Detectado!");
        }
    }
    public void OnChildTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerDetect = null;
        }
    }
}
