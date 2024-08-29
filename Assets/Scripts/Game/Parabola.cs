using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parabola : MonoBehaviour
{
    public Rigidbody Object;
    public Transform target, target2, target3, target4, target5;


    public float h = 25;
    public float gravity = -18;

    private bool seepath = true;

    private LineRenderer lineR;

    
    public GameObject soundPrefab;
    private LayerMask enemyMask;
    private RaycastHit hitinfo;
    private Ray rayinfo;
    

    void Start()
    {
        Object.useGravity = false;
        lineR = GetComponent<LineRenderer>();
        lineR.gameObject.GetComponent<LineRenderer>().enabled = false;
    }

    void Update()
    {

        if (seepath)
        {
            DrawPath();
        }

        /*
        if (Input.GetMouseButtonDown(0))
        {
            rayinfo = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayinfo, out hitinfo))
            {
                GameObject newSound = Instantiate(soundPrefab, hitinfo.point, Quaternion.identity);
                Destroy(newSound, 2);

                Collider[] allEnemies = Physics.OverlapSphere(hitinfo.point, 15, enemyMask);
                for (int i = 0; i < allEnemies.Length; i++)
                {
                    if (allEnemies[i].GetComponent<Parabola>() != null)
                    {
                        //Mover el eneimgo al punto del objeto
                        //allEnemies[i].GetComponent<Parabola>().GetF
                    }
                }
            }
        }
        */
        

    }

    public void Launch()
    {
        Physics.gravity = Vector3.up * gravity;
        Object.useGravity = true;
        Object.velocity = CalculateLaunchData().initialVelocity;
    }

    LaunchData CalculateLaunchData()
    {
        float displacementY = target.position.y - Object.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - Object.position.x, 0, target.position.z - Object.position.z);
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }

    void DrawPath()
    {

        LaunchData launchData = CalculateLaunchData();
        Vector3 previousDrawPoint = Object.position;


        int resolution = 30;
        for (int i = 1; i <= resolution; i++)
        {

            float simulationTime = i / (float)resolution * launchData.timeToTarget;
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint = Object.position + displacement;
            Debug.DrawLine(previousDrawPoint, drawPoint, Color.red);
            previousDrawPoint = drawPoint;

            lineR.SetPosition(0, transform.position);
            lineR.SetPosition(1, target2.transform.position);
            lineR.SetPosition(2, target3.transform.position);
            lineR.SetPosition(3, target4.transform.position);
            lineR.SetPosition(4, target5.transform.position);
            lineR.SetPosition(5, drawPoint);

        }
    }


    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        //B(t) = (1-t) * (1-t) * P0 +2 * t * (1 - t) * P1 + t * t * P2
        //t va de 0 a 1
        float u = 1 - t;

        //B(t) = u * u * P0 +2 * t * u * P1 + t * t * P2
        Vector3 point = u * u * p0;
        point += 2 * t * u * p1;
        point += t * t * p2;

        return point;
    }



    struct LaunchData
    {
        public Vector3 initialVelocity;
        public float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }

    }


}
