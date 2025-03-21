using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spline : MonoBehaviour
{
    public List<CubicBezierCurve> curves;
    [SerializeField] GameObject curvePrefab;
    public int numSegments;
    float[] LUT;

    // Start is called before the first frame update
    void Start()
    {
        numSegments = 20;
    }

    // Update is called once per frame
    void Update()
    {
        FixControlPoints();
        CalcArcLength();
    }

    public void AddCurve()
    {
        GameObject curveGO = Instantiate(curvePrefab);
        CubicBezierCurve curve = curveGO.GetComponent<CubicBezierCurve>();
        curve.knot1.transform.position = curves[curves.Count - 1].knot2.transform.position;
        curve.c1.transform.position = curve.knot1.transform.position + (curves[curves.Count - 1].c2.transform.position - curve.knot1.transform.position) * -1;
        curves.Add(curve);
    }

    public void AddCurve(CubicBezierCurve curve)
    {
        curves.Add(curve);
    }

    public void AddCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        CubicBezierCurve newCurve = new CubicBezierCurve(p0, p1, p2, p3);
    }

    public void AddCurve(GameObject p0, GameObject p1, GameObject p2, GameObject p3)
    {
        CubicBezierCurve newCurve = new CubicBezierCurve(p0, p1, p2, p3);
    }

    public void RemoveCurve(CubicBezierCurve curve)
    {
        curves.Remove(curve);
    }

    public void FixControlPoints()
    {
        for (int i = 1; i < curves.Count; i++)
        {
            curves[i].c1.transform.position = curves[i].knot1.transform.position + (curves[i - 1].c2.transform.position - curves[i].knot1.transform.position) * -1;
            curves[i].knot1.transform.position = curves[i - 1].knot2.transform.position;
        }
    }

    //public float CalcArcLength()
    //{
    //    float sum = 0f;
        
    //    LUT = new float[numSegments * curves.Count];
    //    for (float i = 1; i <= numSegments * curves.Count - 1; i++)
    //    {
    //        float t1 = i / (float)numSegments;
    //        t1 *= curves.Count;
    //        LUT[(int)i - 1] = sum;
    //        Vector3 pos1 = curves[(int)t1].CubicBezierPoint(t1 - Mathf.Floor(t1));
    //        float t2 = (i - 1f) / (float)numSegments;
    //        t2 *= curves.Count;
    //        Vector3 pos2 = curves[(int)t2].CubicBezierPoint(t2 - Mathf.Floor(t2));

    //        sum += Vector3.Distance(pos1, pos2);
    //        Debug.DrawLine(pos1, pos1 + Vector3.up * (sum - LUT[(int)i - 1]));
    //        //Debug.Log(i / (float)numSegments);
    //    }
    //    return sum;
    //}

    public float CalcArcLength()
    {
        float sum = 0;
        LUT = new float[numSegments * curves.Count];
        for (int i = 0; i < curves.Count; i++)
        {
            sum += curves[i].CalcArcLength();
        }
        for (int i = 0; i < curves.Count; i++)
        {
            for (int j = 0; j < numSegments; j++)
            {
                LUT[j + i * numSegments] = curves[i].LUT[j];
            }
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
                    return Remap(dist, LUT[i], LUT[i + 1], i / (n - 1f), (i + 1) / (n - 1f));

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
