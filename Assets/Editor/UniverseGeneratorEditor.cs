using System.Collections.Generic;
using System.Linq;
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
			universeGen.CubemapShader = EditorExtensions.ObjectField<Shader>("Background Shader", universeGen.CubemapShader, false);
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

					so.LookAtCenter = GUILayout.Toggle(so.LookAtCenter, "Look at Centre");
					
					// Materials
					so.UseMaterials = GUILayout.Toggle(so.UseMaterials, "Use materials");
					if (so.UseMaterials)
					{
						so.Materials = EditorExtensions.GameObjectList<Material>("Materials", so.Materials, false);
					}
					else
					{
						// Textures
						so.Textures = EditorExtensions.GameObjectList<Texture>("Textures", so.Textures, false);

						// Colours
						if (so.Colors == null)
						{
							so.Colors = new List<ColorRange>();
						}
						for (var j = 0; j < so.Colors.Count; j++)
						{
							var clr = so.Colors[j];
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.PrefixLabel("Color");
							clr.Color1 = EditorGUILayout.ColorField(clr.Color1);
							clr.Color2 = EditorGUILayout.ColorField(clr.Color2);
							if (GUILayout.Button("X"))
							{
								so.Colors.RemoveAt(j);
							}
							EditorGUILayout.EndHorizontal();
						}
						if (GUILayout.Button("Add Color"))
						{
							so.Colors.Add(new ColorRange());
						}
					}
				}
				if (GUILayout.Button("Remove"))
				{
					universeGen.ScatterObjects.RemoveAt(i);
				}
			}

			EditorGUILayout.Separator();
			if (GUILayout.Button("Add Scatter Group"))
			{
				universeGen.ScatterObjects.Add(new ScatterParams());
			}
		}
	}
}
