using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicBezierCurve : MonoBehaviour
{
    [SerializeField] GameObject c1;
    [SerializeField] GameObject c2;
    [SerializeField] GameObject knot1;
    [SerializeField] GameObject knot2;

    [SerializeField] int numSegments;
    public Vector3[] points;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 Lerp(Vector3 p0, Vector3 p1, float t)
    {
        return p0 * (1 - t) + t * p1;
    }

    Vector3 CubicBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 a = Lerp(p0, p1, t);
        Vector3 b = Lerp(p1, p2, t);
        Vector3 c = Lerp(p2, p3, t);
        Vector3 d = Lerp(a, b, t);
        Vector3 e = Lerp(b, c, t);
        return Lerp(d, e, t);
    }
}
