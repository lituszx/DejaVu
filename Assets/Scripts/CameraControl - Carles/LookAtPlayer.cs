using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private GameObject target;
    private CameraController gm;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        gm = transform.parent.parent.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.transform);
    }
    public void OnParentTriggerEnter(Collider other)
    {
      
    }
    public void OnParentTriggerExit(Collider other)
    {

    }
}
