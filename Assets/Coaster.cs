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
        
    }

    void MoveAlongCurve(float dt)
    {
        
    }
}
