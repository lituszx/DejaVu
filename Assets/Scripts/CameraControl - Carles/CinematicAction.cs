using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicAction : MonoBehaviour
{
    public int idCinematica;
    public GameObject cameraPoint;
    private void Start()
    {
        cameraPoint = transform.GetChild(0).gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponentInChildren<CameraPlayer>().cinematic = true;
            other.GetComponentInChildren<CameraPlayer>().Cinematica(idCinematica, cameraPoint);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponentInChildren<CameraPlayer>().transform.localPosition = other.GetComponentInChildren<CameraPlayer>().initPos;
            other.GetComponentInChildren<CameraPlayer>().transform.localRotation = other.GetComponentInChildren<CameraPlayer>().initRot;
            other.GetComponentInChildren<CameraPlayer>().cinematic = false;
        }
    }
}
