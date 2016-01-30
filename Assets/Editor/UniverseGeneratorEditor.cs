using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UniverseGenerator))]
public class UniverseGeneratorEditor : Editor
{
	void OnEnable()
	{
		hideFlags = HideFlags.HideAndDontSave;
		Debug.Log("On Enabled!");
	}

	public override void OnInspectorGUI()
	{
		var universeGen = (UniverseGenerator)target;

		if (universeGen != null)
		{
			universeGen.BaseShader = EditorExtensions.ObjectField<Shader>("Base Shader", universeGen.BaseShader, false);
			universeGen.CameraObj = EditorExtensions.ObjectField<Camera>("Camera", universeGen.CameraObj, true);
			universeGen.BackgroundColor = EditorGUILayout.ColorField("Background Color", universeGen.BackgroundColor);

			EditorGUILayout.LabelField("Scatter Groups", EditorStyles.boldLabel);
			for (var i = 0; i < universeGen.ScatterObjects.Count; i++)
			{
				EditorGUILayout.LabelField("Scatter Group " + (i + 1));

				var so = universeGen.ScatterObjects[i];

				if (so != null)
				{
					//so.CountMin = EditorGUILayout.IntField("ggg", so.CountMin);
					so.Model = EditorExtensions.ObjectField<GameObject>("Model", so.Model, false);

					// Radius
					var radius = EditorExtensions.FloatRange("Radius", so.RadiusMin, so.RadiusMax);
					so.RadiusMin = radius.Min;
					so.RadiusMax = radius.Max;

					// Count
					var count = EditorExtensions.IntRange("Count", so.CountMin, so.CountMax);
					so.CountMin = count.Min;
					so.CountMax = count.Max;

					// Scale
					var scale = EditorExtensions.FloatRange("Scale", so.ScaleMin, so.ScaleMax);
					so.ScaleMin = scale.Min;
					so.ScaleMax = scale.Max;

					// Textures
					so.Textures = EditorExtensions.GameObjectList<Texture>("Textures", so.Textures, false);
				}
				else
				{
					EditorGUILayout.LabelField("- NULL -");
				}
				if (GUILayout.Button("Remove"))
				{
					universeGen.ScatterObjects.RemoveAt(i);
				}
				//so.Colors = new List<ColorRange> { new ColorRange() };
			}
			if (GUILayout.Button("Add Scatter Group"))
			{
				//universeGen.ScatterObjects.Add(null);
				universeGen.ScatterObjects.Add(new ScatterParams());
			}
		}
	}
}
