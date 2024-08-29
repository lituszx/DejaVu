using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public List<GameObject> allCameras;
    public GameObject activeCamera;
    private void Awake()
    {
        allCameras = new List<GameObject>(GameObject.FindGameObjectsWithTag("MainCamera"));
    }
    void Start()
    {
        for (int i = 0; i < allCameras.Count; i++)
        {
            allCameras[i].GetComponent<Camera>().enabled = false;
            allCameras[i].GetComponent<AudioListener>().enabled = false;
        }
        allCameras[0].GetComponent<Camera>().enabled = true;
        allCameras[0].GetComponent<AudioListener>().enabled = true;
        activeCamera = allCameras[0].gameObject;
    }
}
