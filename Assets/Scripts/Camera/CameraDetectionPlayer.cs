using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDetectionPlayer : MonoBehaviour
{
    private SecurtyCamera _manager;
    void Start()
    {
        _manager = transform.parent.GetComponent<SecurtyCamera>();
    }
    private void OnTriggerEnter(Collider other)
    {
        _manager.OnChildTriggerEnter(other);
    }
    private void OnTriggerExit(Collider other)
    {
        _manager.OnChildTriggerExit(other);
    }
}
