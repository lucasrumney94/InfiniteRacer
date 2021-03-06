﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor 
{
	PathCreator creator;
	CurvePath CurvePath
	{
		get 
		{
			return creator.curvePath;
		}
	}

	const float segmentSelectionDistanceThreshold = 0.1f;
	int selectedSegmentIndex = -1;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EditorGUI.BeginChangeCheck();
		if (GUILayout.Button("Create New"))
		{
			Undo.RecordObject(creator, "Create New");
			
			creator.CreatePath();
		}

		bool isClosed = GUILayout.Toggle(CurvePath.IsClosed, "Closed");
		if (isClosed != CurvePath.IsClosed)
		{
			Undo.RecordObject(creator, "Toggle Closed");			
			CurvePath.IsClosed = isClosed;
		}

		bool autoSetControlPoints = GUILayout.Toggle(CurvePath.AutoSetControlPoints, "Auto Set Control Points");
		if (autoSetControlPoints != CurvePath.AutoSetControlPoints)
		{
			Undo.RecordObject(creator, "Toggle Auto Set Controls");
			CurvePath.AutoSetControlPoints = autoSetControlPoints;
		}

		if (EditorGUI.EndChangeCheck())
		{
			SceneView.RepaintAll();
		}
	}

	void OnSceneGUI()
	{
		Input();
		Draw();
	}

	void Input()
	{
		Event guiEvent = Event.current;
		Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

		if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
		{
			if (selectedSegmentIndex != -1)
			{
				Undo.RecordObject(creator, "Split Segment");
				CurvePath.SplitSegment(mousePos, selectedSegmentIndex);
			}
			else if (!CurvePath.IsClosed)
			{
				Undo.RecordObject(creator, "Add Segment");
				CurvePath.AddSegment(mousePos);
			}
		}


		if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
		{
			//Debug.Log("Right Click!");

			float minDstToAnchor = creator.anchorDiameter*0.5f;
			int closestAnchorIndex = -1;

			for (int i = 0; i < CurvePath.NumPoints; i+=3)
			{
				float dst = Vector2.Distance(mousePos, CurvePath[i]);
				if (dst < minDstToAnchor)
				{
					minDstToAnchor = dst;
					closestAnchorIndex = i;
				}
			}

			//Debug.Log("Closest Anchor Index is " + closestAnchorIndex.ToString());

			if (closestAnchorIndex != -1)
			{
				Undo.RecordObject(creator, "Delete Segment");
				CurvePath.DeleteSegment(closestAnchorIndex);
			}
		}

		if (guiEvent.type == EventType.MouseMove)
		{
			float minDistToSegment = segmentSelectionDistanceThreshold;
			int newSelectedSegmentIndex = -1;

			for (int i = 0; i < CurvePath.NumSegments; i++)
			{
				Vector2[] points = CurvePath.GetPointsInSegment(i);
				float dst = HandleUtility.DistancePointBezier(mousePos, points[0], points[3], points[1], points[2]);
				if (dst< minDistToSegment)
				{
					minDistToSegment = dst;
					newSelectedSegmentIndex = i;
				}
			}

			if (newSelectedSegmentIndex != selectedSegmentIndex)
			{
				selectedSegmentIndex = newSelectedSegmentIndex;
				HandleUtility.Repaint();
			}
		}

		HandleUtility.AddDefaultControl(0);
	}


	void Draw()
	{

		for (int i = 0; i< CurvePath.NumSegments; i++)
		{
			Vector2[] points = CurvePath.GetPointsInSegment(i);

			if (creator.displayControlPoints)
			{
				Handles.color = Color.black;
				Handles.DrawLine(points[1], points[0]);
				Handles.DrawLine(points[2], points[3]);
			}

			Color segmentCol = (i == selectedSegmentIndex && Event.current.shift) ? creator.selectedSegmentCol: creator.segmentCol;

			Handles.DrawBezier(points[0],points[3],points[1],points[2], segmentCol, null, 2);
		}
 

		for (int i = 0; i < CurvePath.NumPoints; i++)
		{
			if (i%3==0 || creator.displayControlPoints)
			{
				Handles.color = (i % 3 == 0) ? creator.anchorCol : creator.controlCol;
				float handleSize = (i % 3 == 0) ? creator.anchorDiameter : creator.controlDiameter;

				Vector2 newPos = Handles.FreeMoveHandle(CurvePath[i],Quaternion.identity, handleSize, Vector2.zero, Handles.CylinderHandleCap);
				if (CurvePath[i] != newPos)
				{
					Undo.RecordObject(creator, "Move point");
					CurvePath.MovePoint(i,newPos);
				}
			}
		}
	}

	void OnEnable()
	{
		creator = (PathCreator)target;
		if (creator.curvePath == null)
		{
			creator.CreatePath();
		}
	}

}
