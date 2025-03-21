using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spline : MonoBehaviour
{
    public List<CubicBezierCurve> curves;
    [SerializeField] GameObject curvePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FixControlPoints();
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
        }
    }

    



}
