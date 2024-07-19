using UnityEngine;

[ExecuteInEditMode]
public class SelectiveGrayscale : MonoBehaviour
{
	public Color filterColor = Color.red;

	[Range(0f, 1f)]
	public float desaturation;

	[Range(0f, 1f)]
	public float sensitivity;

	[Range(0f, 1f)]
	public float tolerance;

	public Material mat;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		mat.SetColor("_FilterColor", filterColor);
		mat.SetFloat("_Desaturation", desaturation);
		mat.SetFloat("_Sensitivity", sensitivity);
		mat.SetFloat("_Tolerance", tolerance);
		Graphics.Blit(source, destination, mat);
	}
}
