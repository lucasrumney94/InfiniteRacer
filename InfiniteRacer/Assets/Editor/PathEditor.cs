using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor 
{
	PathCreator creator;
	CurvePath curvePath;


	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EditorGUI.BeginChangeCheck();
		if (GUILayout.Button("Create New"))
		{
			Undo.RecordObject(creator, "Create New");
			
			creator.CreatePath();
			curvePath = creator.curvePath;


		}

		if (GUILayout.Button("Toggle Closed"))
		{
			Undo.RecordObject(creator, "Toggle Closed");			
			curvePath.ToggleClosed();

		}

		bool autoSetControlPoints = GUILayout.Toggle(curvePath.AutoSetControlPoints, "Auto Set Control Points");
		if (autoSetControlPoints != curvePath.AutoSetControlPoints)
		{
			Undo.RecordObject(creator, "Toggle Auto Set Controls");
			curvePath.AutoSetControlPoints = autoSetControlPoints;
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
			Undo.RecordObject(creator, "Add Segment");
			curvePath.AddSegment(mousePos);
		}

		if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
		{
			float minDstToAnchor = 0.05f;
			int closestAnchorIndex = -1;

			for (int i = 0; i < curvePath.NumPoints; i+=3)
			{
				float dst = Vector2.Distance(mousePos, curvePath[i]);
				if (dst < minDstToAnchor)
				{
					minDstToAnchor = dst;
					closestAnchorIndex = i;
				}
			}

			if (closestAnchorIndex != -1)
			{
				Undo.RecordObject(creator, "Delete Segment");
				curvePath.DeleteSegment(closestAnchorIndex);
			}
		}
	}


	void Draw()
	{

		for (int i = 0; i< curvePath.NumSegments; i++)
		{
			Vector2[] points = curvePath.GetPointsInSegment(i);
			Handles.color = Color.black;
			Handles.DrawLine(points[1], points[0]);
			Handles.DrawLine(points[2], points[3]);	
			Handles.DrawBezier(points[0],points[3],points[1],points[2], Color.green, null, 2);
		}

		Handles.color = Color.red;

		for (int i = 0; i < curvePath.NumPoints; i++)
		{
			Vector2 newPos = Handles.FreeMoveHandle(curvePath[i],Quaternion.identity, 0.1f, Vector2.zero, Handles.CylinderHandleCap);
			if (curvePath[i] != newPos)
			{
				Undo.RecordObject(creator, "Move point");
				curvePath.MovePoint(i,newPos);

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
		curvePath = creator.curvePath;
	}

}
