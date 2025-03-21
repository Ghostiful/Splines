using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class CubicBezierCurve : MonoBehaviour
{
    public GameObject c1;
    public GameObject c2;
    public GameObject knot1;
    public GameObject knot2;

    public int numSegments;
    public Vector3[] points;

    public float[] LUT;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalcArcLength();
    }

    public CubicBezierCurve(GameObject p0, GameObject p1, GameObject p2, GameObject p3)
    {
        points = new Vector3[numSegments];
        CalculatePoints(p0.transform.position, p1.transform.position, p2.transform.position, p3.transform.position);

    }

    public CubicBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        points = new Vector3[numSegments];
        CalculatePoints(p0, p1, p2, p3);
    }

    private void OnDrawGizmos()
    {
        points = new Vector3[numSegments];
        CalculatePoints(knot1.transform.position, c1.transform.position, c2.transform.position, knot2.transform.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawLineStrip(points, false);
    }

    Vector3 Lerp(Vector3 p0, Vector3 p1, float t)
    {
        return p0 * (1 - t) + t * p1;
    }

    public Vector3 CubicBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 a = Lerp(p0, p1, t);
        Vector3 b = Lerp(p1, p2, t);
        Vector3 c = Lerp(p2, p3, t);
        Vector3 d = Lerp(a, b, t);
        Vector3 e = Lerp(b, c, t);
        return Lerp(d, e, t);
    }
    public Vector3 CubicBezierPoint(float t)
    {
        Vector3 a = Lerp(knot1.transform.position, c1.transform.position, t);
        Vector3 b = Lerp(c1.transform.position, c2.transform.position, t);
        Vector3 c = Lerp(c2.transform.position, knot2.transform.position, t);
        Vector3 d = Lerp(a, b, t);
        Vector3 e = Lerp(b, c, t);
        return Lerp(d, e, t);
    }

    void CalculatePoints(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        for (float i = 0; i < numSegments; i++)
        {
            points[(int)i] = CubicBezierPoint(p0, p1, p2, p3, (1.0f / ((float)numSegments - 1)) * i);
        }
    }

    public Vector3 CalcDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return p0 * (-3 * Mathf.Pow(t, 2) + 6 * t - 3) +
            p1 * (9 * Mathf.Pow(t, 2) - 12 * t + 3) +
            p2 * (-9 * Mathf.Pow(t, 2) + 6 * t) +
            p3 * 3 * Mathf.Pow(t, 2);
    }
    
    public Vector3 CalcDerivative(float t)
    {
        return knot1.transform.position * (-3 * Mathf.Pow(t, 2) + 6 * t - 3) +
            c1.transform.position * (9 * Mathf.Pow(t, 2) - 12 * t + 3) +
            c2.transform.position * (-9 * Mathf.Pow(t, 2) + 6 * t) +
            knot2.transform.position * 3 * Mathf.Pow(t, 2);
    }

    public float CalcArcLength()
    {
        float sum = 0f;
        LUT = new float[numSegments];
        for (float i = 1; i <= numSegments; i++)
        {
            LUT[(int)i - 1] = sum;
            if (i != numSegments)
            {
                sum += Vector3.Distance(CubicBezierPoint(i / (float)numSegments), CubicBezierPoint((i - 1) / (float)numSegments));
            }
            
            //Debug.DrawLine(CubicBezierPoint(i / (float)numSegments), CubicBezierPoint(i / (float)numSegments) + Vector3.up * (sum - LUT[(int)i-1]));
            //Debug.Log(i / (float)numSegments);
        }
        return sum;
    }

    public float DistToT(float dist)
    {
        float arcLength = LUT[LUT.Length - 1];
        int n = LUT.Length;
        if (dist >= 0 && dist <= arcLength)
        {
            for (int i = 0; i < n - 1; i++)
            {
                if (dist >= LUT[i] && dist <= LUT[i + 1])
                {
                    return Remap(dist, LUT[i], LUT[i + 1], (float)i / (n - 1f), ((float)i + 1) / (n - 1f));

                }
            }
        }

        return dist / arcLength;
    }

    float Remap(float x, float a1, float b1, float a2, float b2)
    {
        float t = Mathf.InverseLerp(a1, b1, x);
        return Mathf.Lerp(a2, b2, t);
    }

}
