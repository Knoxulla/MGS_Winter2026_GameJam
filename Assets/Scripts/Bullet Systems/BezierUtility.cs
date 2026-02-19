//using UnityEngine;
//using System.Collections.Generic;

//public class BezierUtility : MonoBehaviour
//{
//    // Method to evaluate a cubic Bezier curve at time t
//    public static Vector3 EvaluateCubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
//    {
//        // Ensure t is within bounds [0, 1]
//        t = Mathf.Clamp01(t);

//        // Cubic Bezier formula:
//        // B(t) = (1 - t)^3 * P0 + 3 * (1 - t)^2 * t * P1 + 3 * (1 - t) * t^2 * P2 + t^3 * P3
//        float u = 1 - t;
//        float tt = t * t;
//        float uu = u * u;
//        float uuu = uu * u;
//        float ttt = tt * t;

//        Vector3 p = uuu * p0; // (1 - t)^3 * P0
//        p += 3 * uu * t * p1; // 3 * (1 - t)^2 * t * P1
//        p += 3 * u * tt * p2; // 3 * (1 - t) * t^2 * P2
//        p += ttt * p3;         // t^3 * P3

//        return p;
//    }

//    // Method to generate a path of points based on a cubic Bezier curve
//    public static List<Vector3> GenerateBezierPath(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int numPoints)
//    {
//        List<Vector3> pathPoints = new List<Vector3>();

//        for (int i = 0; i <= numPoints; i++)
//        {
//            float t = i / (float)numPoints;
//            pathPoints.Add(EvaluateCubicBezier(p0, p1, p2, p3, t));
//        }

//        return pathPoints;
//    }

//    // Method to calculate the first derivative (tangent) of the Bezier curve at time t
//    public static Vector3 EvaluateCubicBezierDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
//    {
//        // First derivative of the cubic Bezier formula:
//        // B'(t) = 3 * (1 - t)^2 * (P1 - P0) + 6 * (1 - t) * t * (P2 - P1) + 3 * t^2 * (P3 - P2)
//        float u = 1 - t;
//        float tt = t * t;
//        float uu = u * u;

//        Vector3 derivative = 3 * uu * (p1 - p0); // 3 * (1 - t)^2 * (P1 - P0)
//        derivative += 6 * u * t * (p2 - p1);    // 6 * (1 - t) * t * (P2 - P1)
//        derivative += 3 * tt * (p3 - p2);        // 3 * t^2 * (P3 - P2)

//        return derivative;
//    }

//    // Method to get a list of tangents (first derivative) for the Bezier path
//    public static List<Vector3> GenerateBezierTangents(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int numPoints)
//    {
//        List<Vector3> tangents = new List<Vector3>();

//        for (int i = 0; i <= numPoints; i++)
//        {
//            float t = i / (float)numPoints;
//            tangents.Add(EvaluateCubicBezierDerivative(p0, p1, p2, p3, t));
//        }

//        return tangents;
//    }

//    // Method to visualize a cubic Bezier curve in the editor (optional for debugging)
//    public static void DrawBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int numPoints)
//    {
//        List<Vector3> path = GenerateBezierPath(p0, p1, p2, p3, numPoints);

//        for (int i = 0; i < path.Count - 1; i++)
//        {
//            Debug.DrawLine(path[i], path[i + 1], Color.green);
//        }
//    }


//    // Calculate point on a cubic Bezier curve for a given time t (0 <= t <= 1)
//    public static Vector3 GetPointOnCurve(Vector3[] controlPoints, float t)
//    {
//        // Use De Casteljau's algorithm (akak painful math but neccessary) for quadratic or cubic Bezier curves
//        if (controlPoints.Length == 4) // Cubic Bezier
//        {
//            Vector3 p0 = controlPoints[0];
//            Vector3 p1 = controlPoints[1];
//            Vector3 p2 = controlPoints[2];
//            Vector3 p3 = controlPoints[3];

//            Vector3 q0 = Vector3.Lerp(p0, p1, t);
//            Vector3 q1 = Vector3.Lerp(p1, p2, t);
//            Vector3 q2 = Vector3.Lerp(p2, p3, t);

//            Vector3 r0 = Vector3.Lerp(q0, q1, t);
//            Vector3 r1 = Vector3.Lerp(q1, q2, t);

//            return Vector3.Lerp(r0, r1, t); // Final point on the curve
//        }
//        else if (controlPoints.Length == 3) // Quadratic Bezier
//        {
//            Vector3 p0 = controlPoints[0];
//            Vector3 p1 = controlPoints[1];
//            Vector3 p2 = controlPoints[2];

//            Vector3 q0 = Vector3.Lerp(p0, p1, t);
//            Vector3 q1 = Vector3.Lerp(p1, p2, t);

//            return Vector3.Lerp(q0, q1, t); // Final point on the curve
//        }
//        else
//        {
//            Debug.LogError("Bezier path must have 3 (quadratic) or 4 (cubic) control points.");
//            return Vector3.zero;
//        }
//    }


//}
