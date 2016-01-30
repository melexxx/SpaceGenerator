using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class UniverseGenerator : MonoBehaviour
{
	private GameObject _universeObj;
	private Transform _parent;

	public Camera CameraObj;
	public Shader BaseShader;

	public GameObject Quad;
	public GameObject WrapSphere;

	public Color BackgroundColor = Color.black;

	public List<ScatterParams> ScatterObjects;

	void Start()
	{
		if (ScatterObjects == null)
		{
			ScatterObjects = new List<ScatterParams>();
		}
		Generate();
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Space))
		{
			Generate();
		}
	}

	public void Generate()
	{
		Destroy(_universeObj);
		_universeObj = new GameObject("UniverseObject");
		_parent = _universeObj.transform;

		CameraObj.backgroundColor = BackgroundColor;

		foreach (var sg in ScatterObjects)
		{
			Scatter(sg);
		}
	}

	private void Scatter(ScatterParams settings)
	{
		var count = Random.Range(settings.CountMin, settings.CountMax);
		for (var i = 0; i < count; i++)
		{
			var model = Instantiate<GameObject>(settings.Model);

			if (settings.RadiusMax == 0 && settings.RadiusMin == 0)
			{
				model.transform.position = Vector3.zero;
				model.transform.rotation = Random.rotation;
			}
			else {
				model.transform.position = Random.onUnitSphere * Random.Range(settings.RadiusMin, settings.RadiusMax);
				model.transform.LookAt(Vector3.zero);
				model.transform.rotation *= Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);
			}

			model.transform.localScale = Vector3.one * Random.Range(settings.ScaleMin, settings.ScaleMax);
			model.transform.SetParent(_parent);

			var tex = settings.Textures[Random.Range(0, settings.Textures.Count)];
			var colr = settings.Colors[Random.Range(0, settings.Colors.Count)].GetRandom();
			model.GetComponent<Renderer>().material = CreateMaterial(tex, colr);
		}
	}

	private Material CreateMaterial(Texture tex, Color color)
	{
		var mat = new Material(BaseShader);
		mat.SetTexture("_MainTex", tex);
		mat.SetColor("_Color", color);
		return mat;
	}
}

[System.Serializable]
public class ScatterParams
{
	public int CountMin;

	public int CountMax;

	public float ScaleMin;

	public float ScaleMax;

	public float RadiusMin;

	public float RadiusMax;

	public GameObject Model;

	public List<Texture> Textures;

	public List<ColorRange> Colors;

	public ScatterParams()
	{
		Debug.Log("La la la!");
		Colors = new List<ColorRange> { new ColorRange() };
	}
}

[System.Serializable]
public class ColorRange
{
	public Color Color1;

	public Color Color2;

	public ColorRange()
	{
		Color1 = Color.white;
		Color2 = Color.white;
	}

	public Color GetRandom()
	{
		return Utility.GetRandomColor(Color1, Color2);
	}
}