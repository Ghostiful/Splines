using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coaster : MonoBehaviour
{
    public GameObject splineGO;
    Spline spline;
    int currentCurve;
    //float t;
    float dist;
    int currSegment;
    public float mult;

    // Start is called before the first frame update
    void Start()
    {
        spline = splineGO.GetComponent<Spline>();
        currentCurve = 0;
        currSegment = 0;
        //t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        MoveAlongSpline(Time.deltaTime);
        //MoveAlongFullSpline(Time.deltaTime);
    }

    public void MoveAlongSpline(float dt)
    {
        dist += dt * mult;
        if (dist >= spline.curves[currentCurve].CalcArcLength())
        {
            dist -= spline.curves[currentCurve].CalcArcLength();
            currentCurve++;
            if (currentCurve >= spline.curves.Count)
            {
                currentCurve = 0;
            }
        }
        float t = spline.curves[currentCurve].DistToT(dist);
        transform.position = spline.curves[currentCurve].CubicBezierPoint(t);
        transform.rotation = Quaternion.LookRotation(spline.curves[currentCurve].CalcDerivative(t), Vector3.up);
        
    }


}
