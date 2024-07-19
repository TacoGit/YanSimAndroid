using UnityEngine;

public class SmartphoneScript : MonoBehaviour
{
	public Texture SmashedTexture;

	public GameObject PhoneSmash;

	public Renderer MyRenderer;

	public PromptScript Prompt;

	public MeshFilter MyMesh;

	public Mesh SmashedMesh;

	private void Update()
	{
		if (Prompt.Circle[0].fillAmount == 0f)
		{
			base.transform.localEulerAngles = new Vector3(base.transform.localEulerAngles.x, base.transform.localEulerAngles.y, 0f);
			Object.Instantiate(PhoneSmash, base.transform.position, Quaternion.identity);
			Prompt.Yandere.Police.PhotoEvidence--;
			MyRenderer.material.mainTexture = SmashedTexture;
			MyMesh.mesh = SmashedMesh;
			Prompt.Hide();
			Prompt.enabled = false;
			base.enabled = false;
		}
	}
}
