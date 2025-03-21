using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coaster : MonoBehaviour
{
    public GameObject splineGO;
    Spline spline;
    int currentCurve;
    float t;

    // Start is called before the first frame update
    void Start()
    {
        spline = splineGO.GetComponent<Spline>();
        currentCurve = 0;
        t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        MoveAlongSpline(Time.deltaTime);
    }

    public void MoveAlongSpline(float dt)
    {
        t += dt;
        if (t > 1)
        {
            t--;
            currentCurve++;
            if (currentCurve >= spline.curves.Count)
            {
                currentCurve = 0;
            }
        }
        transform.position = spline.curves[currentCurve].CubicBezierPoint(t);
        transform.rotation = Quaternion.LookRotation(spline.curves[currentCurve].CalcDerivative(t), Vector3.up);
    }
}
