using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDetect : MonoBehaviour
{
    private EnemyControl _manager;
    void Start()
    {
        _manager = transform.parent.GetComponent<EnemyControl>();
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

