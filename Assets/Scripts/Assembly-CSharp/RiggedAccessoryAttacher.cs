using UnityEngine;

public class RiggedAccessoryAttacher : MonoBehaviour
{
	public StudentScript Student;

	public GameObject root;

	public GameObject accessory;

	public Material[] accessoryMaterials;

	public Material[] okaMaterials;

	public Material[] ribaruMaterials;

	public Material[] defaultMaterials;

	public Material[] painterMaterials;

	public Material[] painterMaterialsFlipped;

	public GameObject[] Panties;

	public Material[] PantyMaterials;

	public SkinnedMeshRenderer newRenderer;

	public bool Initialized;

	public bool CookingClub;

	public bool ScienceClub;

	public bool ArtClub;

	public bool Gentle;

	public int PantyID;

	public int ID;

	public void Start()
	{
		Initialized = true;
		if (PantyID == 99)
		{
			PantyID = PlayerGlobals.PantiesEquipped;
		}
		if (CookingClub)
		{
			if (Student.Male)
			{
				accessory = GameObject.Find("MaleCookingApron");
			}
			else
			{
				accessory = GameObject.Find("FemaleCookingApron");
			}
		}
		else if (ArtClub)
		{
			if (Student.Male)
			{
				accessory = GameObject.Find("PainterApron");
				accessoryMaterials = painterMaterials;
			}
			else
			{
				accessory = GameObject.Find("PainterApronFemale");
				accessoryMaterials = painterMaterials;
			}
		}
		else if (Gentle)
		{
			accessory = GameObject.Find("GentleEyes");
			accessoryMaterials = defaultMaterials;
		}
		else
		{
			if (ID == 1)
			{
				accessory = GameObject.Find("LabcoatFemale");
			}
			if (ID == 2)
			{
				accessory = GameObject.Find("LabcoatMale");
			}
			if (ID == 26)
			{
				accessory = GameObject.Find("OkaBlazer");
				accessoryMaterials = okaMaterials;
			}
			if (ID == 100)
			{
				accessory = Panties[PantyID];
				accessoryMaterials[0] = PantyMaterials[PantyID];
			}
		}
		AttachAccessory();
	}

	public void AttachAccessory()
	{
		AddLimb(accessory, root, accessoryMaterials);
		if (ID == 100)
		{
			newRenderer.updateWhenOffscreen = true;
		}
	}

	public void RemoveAccessory()
	{
		Object.Destroy(newRenderer);
	}

	private void AddLimb(GameObject bonedObj, GameObject rootObj, Material[] bonedObjMaterials)
	{
		SkinnedMeshRenderer[] componentsInChildren = bonedObj.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		SkinnedMeshRenderer[] array = componentsInChildren;
		foreach (SkinnedMeshRenderer thisRenderer in array)
		{
			ProcessBonedObject(thisRenderer, rootObj, bonedObjMaterials);
		}
	}

	private void ProcessBonedObject(SkinnedMeshRenderer thisRenderer, GameObject rootObj, Material[] thisRendererMaterials)
	{
		GameObject gameObject = new GameObject(thisRenderer.gameObject.name);
		gameObject.transform.parent = rootObj.transform;
		gameObject.layer = rootObj.layer;
		gameObject.AddComponent<SkinnedMeshRenderer>();
		newRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
		Transform[] array = new Transform[thisRenderer.bones.Length];
		for (int i = 0; i < thisRenderer.bones.Length; i++)
		{
			array[i] = FindChildByName(thisRenderer.bones[i].name, rootObj.transform);
		}
		newRenderer.bones = array;
		newRenderer.sharedMesh = thisRenderer.sharedMesh;
		if (thisRendererMaterials == null)
		{
			newRenderer.materials = thisRenderer.sharedMaterials;
		}
		else
		{
			newRenderer.materials = thisRendererMaterials;
		}
	}

	private Transform FindChildByName(string thisName, Transform thisGameObj)
	{
		if (thisGameObj.name == thisName)
		{
			return thisGameObj.transform;
		}
		foreach (Transform item in thisGameObj)
		{
			Transform transform = FindChildByName(thisName, item);
			if ((bool)transform)
			{
				return transform;
			}
		}
		return null;
	}
}
