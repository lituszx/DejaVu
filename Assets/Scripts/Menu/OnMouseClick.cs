using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouseClick : MonoBehaviour
{
    GameObject img;
    Transform initPos;
    int clicks = 0;
    private void Update()
    {
        if (clicks == 1)
        {
            img.transform.localPosition = Vector3.MoveTowards(img.transform.localPosition, Vector3.zero, Time.deltaTime * 300);
            img.transform.localScale = Vector3.MoveTowards(img.transform.localScale, new Vector3(5f, 5f, 5f), Time.deltaTime * 10);
        }
        if (clicks == 2)
        {
            img.transform.localPosition = Vector3.MoveTowards(img.transform.localPosition, initPos.position, Time.deltaTime * 300);
            img.transform.localScale = Vector3.MoveTowards(img.transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10);
            if (img.transform.position == initPos.position)
                clicks = 0;
        }
    }
    public void MoveToCenter(GameObject _img)
    {
        img = _img;
        initPos = _img.transform;
        clicks++;
    }
}
