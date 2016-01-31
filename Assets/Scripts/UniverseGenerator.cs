using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class UniverseGenerator : MonoBehaviour
{
	#region Private Members

	private GameObject _universeObj;

	private Transform _parent;

	private Camera _renderCamera;

	#endregion

	#region Public Variables

	public int FlatResolution = 2048;

	public Shader BaseShader;

	public Shader CubemapShader;

	public GameObject Quad;

	public GameObject WrapSphere;

	public Color BackgroundColor = Color.black;

	public List<ScatterSettings> ScatterObjects;

	#endregion

	public UniverseGenerator()
	{
		if (ScatterObjects == null)
		{
			ScatterObjects = new List<ScatterSettings>();
		}
	}

	void Start()
	{
		if (ScatterObjects == null)
		{
			ScatterObjects = new List<ScatterSettings>();
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

	#region Public Methods

	public void Generate()
	{
		Destroy(_universeObj);
		_universeObj = new GameObject("UniverseObject");
		_parent = _universeObj.transform;
		
		var camObj = new GameObject("BackgroundCamera");
		_renderCamera = camObj.AddComponent<Camera>();
		// VERY IMPORTANT - must set clear to color before creating universe, then set to skybox after clearing
		_renderCamera.clearFlags = CameraClearFlags.Color;
		camObj.transform.parent = _parent;
		
		_renderCamera.backgroundColor = BackgroundColor;

		foreach (var sg in ScatterObjects)
		{
			Scatter(sg);
		}

		Flatten();
	}

	public void Flatten()
	{
		var renderTexture = new RenderTexture(FlatResolution, FlatResolution, 24);
		renderTexture.wrapMode = TextureWrapMode.Repeat;
		renderTexture.antiAliasing = 2;
		renderTexture.anisoLevel = 9;
		renderTexture.filterMode = FilterMode.Trilinear;
		renderTexture.generateMips = false;
		renderTexture.isCubemap = true;

		var bgMaterial = new Material(CubemapShader);
		bgMaterial.SetTexture("_Tex", renderTexture);
		Destroy(_parent.gameObject);

		RenderSettings.skybox = bgMaterial;

		_renderCamera.RenderToCubemap(renderTexture, 63);

		_renderCamera.clearFlags = CameraClearFlags.Skybox;
		_renderCamera.enabled = false;
	}

	#endregion

	#region Private Methods

	private void Scatter(ScatterSettings settings)
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

				if (settings.LookAtCenter)
				{
					model.transform.LookAt(Vector3.zero);
					model.transform.rotation *= Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);
				}
				else
				{
					model.transform.rotation = Random.rotation;
				}
			}

			model.transform.localScale = Vector3.one * Random.Range(settings.ScaleMin, settings.ScaleMax);
			model.transform.SetParent(_parent);

			if (settings.UseMaterials)
			{
				model.GetComponent<Renderer>().material = settings.Materials[Random.Range(0, settings.Materials.Count)];
			}
			else {
				if (settings.Textures != null && settings.Textures.Any())
				{
					var tex = settings.Textures[Random.Range(0, settings.Textures.Count)];
					var colr = settings.Colors[Random.Range(0, settings.Colors.Count)].GetRandom();
					model.GetComponent<Renderer>().material = CreateMaterial(tex, colr);
				}
			}
		}
	}

	private Material CreateMaterial(Texture tex, Color color)
	{
		var mat = new Material(BaseShader);
		mat.SetTexture("_MainTex", tex);
		mat.SetColor("_Color", color);
		return mat;
	}

	#endregion
}

[System.Serializable]
public class ScatterSettings
{
	public int CountMin;

	public int CountMax;

	public float ScaleMin;

	public float ScaleMax;

	public float RadiusMin;

	public float RadiusMax;

	public GameObject Model;

	public bool LookAtCenter;

	public List<Texture> Textures;

	public List<ColorRange> Colors;

	public bool UseMaterials;

	public List<Material> Materials;

	public ScatterSettings()
	{
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