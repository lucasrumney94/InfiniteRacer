﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CurvePath {

	[SerializeField, HideInInspector]
	List<Vector2> points;
	[SerializeField, HideInInspector]
	bool isClosed;
	[SerializeField, HideInInspector]
	bool autoSetControlPoints;

	public CurvePath(Vector2 center)
	{
		points = new List<Vector2>
		{
			center+Vector2.left,
			center+(Vector2.left+Vector2.up)*0.5f,
			center+(Vector2.right-Vector2.up)*0.5f,
			center+Vector2.right,
		};
	}

	public Vector2 this[int i]
	{
		get
		{
			return points[i];
		}
	}

	public bool IsClosed
	{
		get
		{
			return isClosed;
		}
		set
		{
			if (isClosed != value)
			{
				isClosed = value;
				
				if (isClosed)
				{
					points.Add(points[points.Count-1]*2-points[points.Count-2]);
					points.Add(points[0]*2-points[1]);
					if (AutoSetControlPoints)
					{
						AutoSetAnchorControlPoints(0);
						AutoSetAnchorControlPoints(points.Count-3);
					}

				}
				else
				{
					if (AutoSetControlPoints)
					{
						AutoSetSetStartAndEndControls();
					}
					points.RemoveRange(points.Count-2, 2);
				}
			}
		}
	}

	public bool AutoSetControlPoints
	{
		get
		{
			return autoSetControlPoints;
		}
		set
		{
			if (autoSetControlPoints != value)
			{
				autoSetControlPoints = value;
				if (autoSetControlPoints)
				{
					AutoSetAllControlPoints();
				}
			}
		}
	}

	public int NumPoints 
	{
		get
		{
			return points.Count;
		}
	}

	public int NumSegments
	{
		get
		{
			return points.Count/3;
		}
	}

	public void AddSegment(Vector2 anchorPos)
	{
		points.Add(points[points.Count-1]*2-points[points.Count-2]);
		points.Add((points[points.Count-1]+anchorPos)*0.5f);
		points.Add(anchorPos);

		if (AutoSetControlPoints)
		{
			AutoSetAllAffectedControlPoints(points.Count-1);
		}
	}

	public void SplitSegment(Vector2 anchorPosition, int segmentIndex)
	{
		points.InsertRange(segmentIndex*3+2, new Vector2[]{Vector2.zero, anchorPosition, Vector2.zero});
		if (autoSetControlPoints)
		{
			AutoSetAllAffectedControlPoints(segmentIndex*3+3);

		}
		else 
		{
			AutoSetAnchorControlPoints(segmentIndex*3+3);
		}
	}

	public void DeleteSegment(int anchorIndex)
	{
		if (NumSegments>2 || !isClosed && NumSegments > 1)
		{
			if (anchorIndex == 0)
			{
				if (isClosed)
				{
					points[points.Count-1] = points[2];
				}
				points.RemoveRange(0,3);
			}
			else if (anchorIndex == points.Count-1 && !isClosed)
			{
				points.RemoveRange(anchorIndex-2, 3);
			}
			else
			{
				points.RemoveRange(anchorIndex-1, 3);
			}
		}
	}

	public Vector2[] GetPointsInSegment(int i)
	{
		return new Vector2[]{points[i*3], points[i*3+1], points[i*3+2], points[LoopIndex(i*3+3)]};
	}

	public void MovePoint(int i, Vector2 pos)
	{
		Vector2 deltaMove = pos-points[i];

		if (i%3==0 || !autoSetControlPoints)
		{
			points[i] = pos;

			if (autoSetControlPoints)
			{
				AutoSetAllAffectedControlPoints(i);
			}
			else
			{
				//Point is an Anchor Point
				if (i%3 == 0)
				{
					if (i+1 < points.Count || isClosed)
					{
						points[LoopIndex(i+1)]+=deltaMove;
					}
					if (i-1 >= 0 || isClosed)
					{
						points[LoopIndex(i-1)]+=deltaMove;
					}
				}
				//Point is not an Anchor Point
				else 
				{
					bool nexPointIsAnchor = (i+1)%3 == 0;
					int correspondingControlIndex = (nexPointIsAnchor) ? i + 2 : i - 2;
					int anchorIndex = (nexPointIsAnchor) ? i + 1 : i - 1;

					if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count || isClosed)
					{
						float dst = (points[LoopIndex(anchorIndex)]-points[LoopIndex(correspondingControlIndex)]).magnitude;
						Vector2 dir = (points[LoopIndex(anchorIndex)] - pos).normalized;
						points[LoopIndex(correspondingControlIndex)] = points[LoopIndex(anchorIndex)] + dir*dst;
					}
				}
			}
		}	
	}

	public Vector2[] CalculateEvenlySpacedPoints(float spacing, float resolution = 1)
	{
		List<Vector2> evenlySpacedPoints = new List<Vector2>();
		evenlySpacedPoints.Add(points[0]);
		Vector2 previousPoint = points[0];
		float dstSinceLastEvenPoint = 0;

		for (int segmentIndex = 0; segmentIndex < NumSegments; segmentIndex++)
		{
			Vector2[] p = GetPointsInSegment(segmentIndex);
			float controlNetLength = Vector2.Distance(p[0],p[1])+Vector2.Distance(p[1],p[2])+Vector2.Distance(p[2],p[3]);
			float estimatedCurveLength = Vector2.Distance(p[0], p[3])+controlNetLength/2f;
			int divisions = Mathf.CeilToInt(estimatedCurveLength * resolution * 10);

			float t = 0;
			while (t<=1)
			{
				t += 1f/divisions;
				Vector2 pointOnCurve = Bezier.EvaluateCubic(p[0],p[1],p[2],p[3],t);
				dstSinceLastEvenPoint += Vector2.Distance(previousPoint, pointOnCurve);

				while (dstSinceLastEvenPoint >= spacing)
				{
					float overshootDst = dstSinceLastEvenPoint - spacing;
					Vector2 newEvenlySpacedPoint = pointOnCurve + (previousPoint-pointOnCurve).normalized*overshootDst;
					evenlySpacedPoints.Add(newEvenlySpacedPoint);	
					dstSinceLastEvenPoint = overshootDst;
					previousPoint = newEvenlySpacedPoint;
				}

				previousPoint = pointOnCurve;
			}
		}
		return evenlySpacedPoints.ToArray();
	}

	public Vector2 CalculatePointOnCurve(int segmentIndex, float t)
	{
		Vector2[] p = GetPointsInSegment(segmentIndex);
		return Bezier.EvaluateCubic(p[0],p[1],p[2],p[3],t);
	}

	public Vector2 CalculateTangentOnCurve(int segmentIndex, float t)
	{
		Vector2[] pts = GetPointsInSegment(segmentIndex);
	
		Vector3 a = Vector3.Lerp(pts[0], pts[1], t);
		Vector3 b = Vector3.Lerp(pts[1], pts[2], t);
		Vector3 c = Vector3.Lerp(pts[2], pts[3], t);
		Vector3 d = Vector3.Lerp(a, b, t);
		Vector3 e = Vector3.Lerp(b, c, t);
		return (e-d).normalized;
	
	}

	void AutoSetAllAffectedControlPoints(int updatedAnchorIndex)
	{
		for (int i = updatedAnchorIndex-3; i <= updatedAnchorIndex+3; i+=3)
		{
			if (i >= 0 && i < points.Count || isClosed)
			{
				AutoSetAnchorControlPoints(LoopIndex(i));
			}
		}

		AutoSetSetStartAndEndControls();
	}

	void AutoSetAllControlPoints()
	{
		for (int i = 0; i < points.Count; i+=3)
		{
			AutoSetAnchorControlPoints(i);
		}

		AutoSetSetStartAndEndControls();
	}

	void AutoSetAnchorControlPoints(int anchorIndex)
	{
		Vector2 anchorPos = points[anchorIndex];
		Vector2 dir = Vector2.zero;
		float[] neighborDistances = new float[2];

		if (anchorIndex-3 >= 0 || isClosed)
		{
			Vector2 offset = points[LoopIndex(anchorIndex-3)] - anchorPos;
			dir += offset.normalized;
			neighborDistances[0] = offset.magnitude;
		}
		if (anchorIndex+3 >= 0 || isClosed)
		{
			Vector2 offset = points[LoopIndex(anchorIndex+3)] - anchorPos;
			dir -= offset.normalized;
			neighborDistances[1] = -offset.magnitude;
		}

		dir.Normalize();

		for (int i = 0; i < 2; i++)
		{
			int controlIndex = anchorIndex + i * 2 - 1;
			if (controlIndex >= 0 && controlIndex < points.Count || isClosed)
			{
				points[LoopIndex(controlIndex)] = anchorPos + dir*neighborDistances[i]*0.5f;
			}
		} 
	}

	void AutoSetSetStartAndEndControls()
	{
		if (!isClosed)
		{
			points[1] = (points[0]+points[2]) * 0.5f;
			points[points.Count-2] = (points[points.Count-1]+points[points.Count-3]) * 0.5f;
		}
	}

	int LoopIndex(int i)
	{
		return (i+points.Count)%points.Count;
	}
}
