using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline : MonoBehaviour {

	public List<Vector3> points;


	void Start()
	{

	}

	void AddPoint(Vector3 position)
	{
		// Control Points
		//points.Add()
		//points.Add()

		// Anchor Point
		points.Add(position);
	}

	Vector3 GetPoint(Vector3[] pts, float t)
	{
		Vector3 a = Vector3.Lerp(pts[0], pts[1], t);
		Vector3 b = Vector3.Lerp(pts[1], pts[2], t);
		Vector3 c = Vector3.Lerp(pts[2], pts[3], t);
		Vector3 d = Vector3.Lerp(a, b, t);
		Vector3 e = Vector3.Lerp(b, c, t);
		return Vector3.Lerp(d, e, t);
	}

	Vector3 GetPointOptimized(Vector3[] pts, float t)
	{
		float omt = 1f-t;
		float omt2 = omt*omt;
		float t2 = t*t;
		return  pts[0] * ( omt2 * omt ) +
				pts[1] * ( 3f * omt2 * t ) +
				pts[2] * ( 3f * omt * t2 ) +
				pts[3] * ( t2 * t );
	}

	Vector3 GetTangent(Vector3[] pts, float t)
	{
		Vector3 a = Vector3.Lerp(pts[0], pts[1], t);
		Vector3 b = Vector3.Lerp(pts[1], pts[2], t);
		Vector3 c = Vector3.Lerp(pts[2], pts[3], t);
		Vector3 d = Vector3.Lerp(a, b, t);
		Vector3 e = Vector3.Lerp(b, c, t);
		return (e-d).normalized;
	}

	Vector3 GetTangentOptimized(Vector3[] pts, float t)
	{
		float omt = 1f-t;
		float omt2 = omt*omt;
		float t2 = t*t;
		Vector3 tanget = 
			pts[0] * ( -omt2 ) +
			pts[1] * ( 3f * omt2 - 2f * omt ) +
			pts[2] * ( -3f * t2 + 2f * t2 ) +
			pts[3] * ( t2 );
		return tanget.normalized;
	}

	Vector3 GetNormal2D(Vector3[] pts, float t)
	{
		Vector3 tangent = GetTangent(pts, t);
		return new Vector3( -tangent.y, tangent.x, 0f);
	}

	Vector3 GetNormal3D(Vector3[] pts, float t, Vector3 up)
	{
		Vector3 tangent = GetTangent(pts, t);
		Vector3 binormal = Vector3.Cross(up, tangent).normalized;
		return Vector3.Cross(tangent, binormal);
	}

	Quaternion GetOrientation2D( Vector3[] pts, float t)
	{
		Vector3 tangent = GetTangent( pts, t);
		Vector3 normal = GetNormal2D( pts, t);
		return Quaternion.LookRotation(tangent, normal); 
	}

	Quaternion GetOrientation3D( Vector3[] pts, float t, Vector3 up)
	{
		Vector3 tangent = GetTangent( pts, t);
		Vector3 normal = GetNormal3D( pts, t, up);
		return Quaternion.LookRotation(tangent, normal); 
	}
}
