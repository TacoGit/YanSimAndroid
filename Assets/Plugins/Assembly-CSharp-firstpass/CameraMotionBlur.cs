using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Camera/Camera Motion Blur")]
public class CameraMotionBlur : PostEffectsBase
{
	public enum MotionBlurFilter
	{
		CameraMotion = 0,
		LocalBlur = 1,
		Reconstruction = 2,
		ReconstructionDX11 = 3,
		ReconstructionDisc = 4
	}

	private const float MAX_RADIUS = 10f;

	public MotionBlurFilter filterType = MotionBlurFilter.Reconstruction;

	public bool preview;

	public Vector3 previewScale = Vector3.one;

	public float movementScale;

	public float rotationScale = 1f;

	public float maxVelocity = 8f;

	public float minVelocity = 0.1f;

	public float velocityScale = 0.375f;

	public float softZDistance = 0.005f;

	public int velocityDownsample = 1;

	public LayerMask excludeLayers = 0;

	private GameObject tmpCam;

	public Shader shader;

	public Shader dx11MotionBlurShader;

	public Shader replacementClear;

	private Material motionBlurMaterial;

	private Material dx11MotionBlurMaterial;

	public Texture2D noiseTexture;

	public float jitter = 0.05f;

	public bool showVelocity;

	public float showVelocityScale = 1f;

	private Matrix4x4 currentViewProjMat;

	private Matrix4x4 prevViewProjMat;

	private int prevFrameCount;

	private bool wasActive;

	private Vector3 prevFrameForward = Vector3.forward;

	private Vector3 prevFrameRight = Vector3.right;

	private Vector3 prevFrameUp = Vector3.up;

	private Vector3 prevFramePos = Vector3.zero;

	private void CalculateViewProjection()
	{
		Camera component = GetComponent<Camera>();
		Matrix4x4 worldToCameraMatrix = component.worldToCameraMatrix;
		Matrix4x4 gPUProjectionMatrix = GL.GetGPUProjectionMatrix(component.projectionMatrix, true);
		currentViewProjMat = gPUProjectionMatrix * worldToCameraMatrix;
	}

	protected override void Start()
	{
		CheckResources();
		wasActive = base.gameObject.activeInHierarchy;
		CalculateViewProjection();
		Remember();
		wasActive = false;
	}

	private void OnEnable()
	{
		GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
	}

	private void OnDisable()
	{
		if (null != motionBlurMaterial)
		{
			Object.DestroyImmediate(motionBlurMaterial);
			motionBlurMaterial = null;
		}
		if (null != dx11MotionBlurMaterial)
		{
			Object.DestroyImmediate(dx11MotionBlurMaterial);
			dx11MotionBlurMaterial = null;
		}
		if (null != tmpCam)
		{
			Object.DestroyImmediate(tmpCam);
			tmpCam = null;
		}
	}

	protected override bool CheckResources()
	{
		CheckSupport(true, true);
		motionBlurMaterial = CheckShaderAndCreateMaterial(shader, motionBlurMaterial);
		if (supportDX11 && filterType == MotionBlurFilter.ReconstructionDX11)
		{
			dx11MotionBlurMaterial = CheckShaderAndCreateMaterial(dx11MotionBlurShader, dx11MotionBlurMaterial);
		}
		if (!isSupported)
		{
			ReportAutoDisable();
		}
		return isSupported;
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!CheckResources())
		{
			Graphics.Blit(source, destination);
			return;
		}
		if (filterType == MotionBlurFilter.CameraMotion)
		{
			StartFrame();
		}
		RenderTextureFormat format = ((!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf)) ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.RGHalf);
		RenderTexture temporary = RenderTexture.GetTemporary(divRoundUp(source.width, velocityDownsample), divRoundUp(source.height, velocityDownsample), 0, format);
		int num = 1;
		int num2 = 1;
		maxVelocity = Mathf.Max(2f, maxVelocity);
		float num3 = maxVelocity;
		bool flag = false;
		if (filterType == MotionBlurFilter.ReconstructionDX11 && dx11MotionBlurMaterial == null)
		{
			flag = true;
		}
		if (filterType == MotionBlurFilter.Reconstruction || flag || filterType == MotionBlurFilter.ReconstructionDisc)
		{
			maxVelocity = Mathf.Min(maxVelocity, 10f);
			num = divRoundUp(temporary.width, (int)maxVelocity);
			num2 = divRoundUp(temporary.height, (int)maxVelocity);
			num3 = temporary.width / num;
		}
		else
		{
			num = divRoundUp(temporary.width, (int)maxVelocity);
			num2 = divRoundUp(temporary.height, (int)maxVelocity);
			num3 = temporary.width / num;
		}
		RenderTexture temporary2 = RenderTexture.GetTemporary(num, num2, 0, format);
		RenderTexture temporary3 = RenderTexture.GetTemporary(num, num2, 0, format);
		temporary.filterMode = FilterMode.Point;
		temporary2.filterMode = FilterMode.Point;
		temporary3.filterMode = FilterMode.Point;
		if ((bool)noiseTexture)
		{
			noiseTexture.filterMode = FilterMode.Point;
		}
		source.wrapMode = TextureWrapMode.Clamp;
		temporary.wrapMode = TextureWrapMode.Clamp;
		temporary3.wrapMode = TextureWrapMode.Clamp;
		temporary2.wrapMode = TextureWrapMode.Clamp;
		CalculateViewProjection();
		if (base.gameObject.activeInHierarchy && !wasActive)
		{
			Remember();
		}
		wasActive = base.gameObject.activeInHierarchy;
		Matrix4x4 matrix4x = Matrix4x4.Inverse(currentViewProjMat);
		motionBlurMaterial.SetMatrix("_InvViewProj", matrix4x);
		motionBlurMaterial.SetMatrix("_PrevViewProj", prevViewProjMat);
		motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", prevViewProjMat * matrix4x);
		motionBlurMaterial.SetFloat("_MaxVelocity", num3);
		motionBlurMaterial.SetFloat("_MaxRadiusOrKInPaper", num3);
		motionBlurMaterial.SetFloat("_MinVelocity", minVelocity);
		motionBlurMaterial.SetFloat("_VelocityScale", velocityScale);
		motionBlurMaterial.SetFloat("_Jitter", jitter);
		motionBlurMaterial.SetTexture("_NoiseTex", noiseTexture);
		motionBlurMaterial.SetTexture("_VelTex", temporary);
		motionBlurMaterial.SetTexture("_NeighbourMaxTex", temporary3);
		motionBlurMaterial.SetTexture("_TileTexDebug", temporary2);
		Camera component = GetComponent<Camera>();
		if (preview)
		{
			Matrix4x4 worldToCameraMatrix = component.worldToCameraMatrix;
			Matrix4x4 identity = Matrix4x4.identity;
			identity.SetTRS(previewScale * (1f / 3f), Quaternion.identity, Vector3.one);
			Matrix4x4 gPUProjectionMatrix = GL.GetGPUProjectionMatrix(component.projectionMatrix, true);
			prevViewProjMat = gPUProjectionMatrix * identity * worldToCameraMatrix;
			motionBlurMaterial.SetMatrix("_PrevViewProj", prevViewProjMat);
			motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", prevViewProjMat * matrix4x);
		}
		if (filterType == MotionBlurFilter.CameraMotion)
		{
			Vector4 zero = Vector4.zero;
			float num4 = Vector3.Dot(base.transform.up, Vector3.up);
			Vector3 rhs = prevFramePos - base.transform.position;
			float magnitude = rhs.magnitude;
			float num5 = 1f;
			num5 = Vector3.Angle(base.transform.up, prevFrameUp) / component.fieldOfView * ((float)source.width * 0.75f);
			zero.x = rotationScale * num5;
			num5 = Vector3.Angle(base.transform.forward, prevFrameForward) / component.fieldOfView * ((float)source.width * 0.75f);
			zero.y = rotationScale * num4 * num5;
			num5 = Vector3.Angle(base.transform.forward, prevFrameForward) / component.fieldOfView * ((float)source.width * 0.75f);
			zero.z = rotationScale * (1f - num4) * num5;
			if (magnitude > Mathf.Epsilon && movementScale > Mathf.Epsilon)
			{
				zero.w = movementScale * Vector3.Dot(base.transform.forward, rhs) * ((float)source.width * 0.5f);
				zero.x += movementScale * Vector3.Dot(base.transform.up, rhs) * ((float)source.width * 0.5f);
				zero.y += movementScale * Vector3.Dot(base.transform.right, rhs) * ((float)source.width * 0.5f);
			}
			if (preview)
			{
				motionBlurMaterial.SetVector("_BlurDirectionPacked", new Vector4(previewScale.y, previewScale.x, 0f, previewScale.z) * (0.5f * component.fieldOfView));
			}
			else
			{
				motionBlurMaterial.SetVector("_BlurDirectionPacked", zero);
			}
		}
		else
		{
			Graphics.Blit(source, temporary, motionBlurMaterial, 0);
			Camera camera = null;
			if (excludeLayers.value != 0)
			{
				camera = GetTmpCam();
			}
			if ((bool)camera && excludeLayers.value != 0 && (bool)replacementClear && replacementClear.isSupported)
			{
				camera.targetTexture = temporary;
				camera.cullingMask = excludeLayers;
				camera.RenderWithShader(replacementClear, string.Empty);
			}
		}
		if (!preview && Time.frameCount != prevFrameCount)
		{
			prevFrameCount = Time.frameCount;
			Remember();
		}
		source.filterMode = FilterMode.Bilinear;
		if (showVelocity)
		{
			motionBlurMaterial.SetFloat("_DisplayVelocityScale", showVelocityScale);
			Graphics.Blit(temporary, destination, motionBlurMaterial, 1);
		}
		else if (filterType == MotionBlurFilter.ReconstructionDX11 && !flag)
		{
			dx11MotionBlurMaterial.SetFloat("_MinVelocity", minVelocity);
			dx11MotionBlurMaterial.SetFloat("_VelocityScale", velocityScale);
			dx11MotionBlurMaterial.SetFloat("_Jitter", jitter);
			dx11MotionBlurMaterial.SetTexture("_NoiseTex", noiseTexture);
			dx11MotionBlurMaterial.SetTexture("_VelTex", temporary);
			dx11MotionBlurMaterial.SetTexture("_NeighbourMaxTex", temporary3);
			dx11MotionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, softZDistance));
			dx11MotionBlurMaterial.SetFloat("_MaxRadiusOrKInPaper", num3);
			Graphics.Blit(temporary, temporary2, dx11MotionBlurMaterial, 0);
			Graphics.Blit(temporary2, temporary3, dx11MotionBlurMaterial, 1);
			Graphics.Blit(source, destination, dx11MotionBlurMaterial, 2);
		}
		else if (filterType == MotionBlurFilter.Reconstruction || flag)
		{
			motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, softZDistance));
			Graphics.Blit(temporary, temporary2, motionBlurMaterial, 2);
			Graphics.Blit(temporary2, temporary3, motionBlurMaterial, 3);
			Graphics.Blit(source, destination, motionBlurMaterial, 4);
		}
		else if (filterType == MotionBlurFilter.CameraMotion)
		{
			Graphics.Blit(source, destination, motionBlurMaterial, 6);
		}
		else if (filterType == MotionBlurFilter.ReconstructionDisc)
		{
			motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, softZDistance));
			Graphics.Blit(temporary, temporary2, motionBlurMaterial, 2);
			Graphics.Blit(temporary2, temporary3, motionBlurMaterial, 3);
			Graphics.Blit(source, destination, motionBlurMaterial, 7);
		}
		else
		{
			Graphics.Blit(source, destination, motionBlurMaterial, 5);
		}
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
		RenderTexture.ReleaseTemporary(temporary3);
	}

	private void Remember()
	{
		prevViewProjMat = currentViewProjMat;
		prevFrameForward = base.transform.forward;
		prevFrameRight = base.transform.right;
		prevFrameUp = base.transform.up;
		prevFramePos = base.transform.position;
	}

	private Camera GetTmpCam()
	{
		Camera component = GetComponent<Camera>();
		if (tmpCam == null)
		{
			string text = "_" + component.name + "_MotionBlurTmpCam";
			GameObject gameObject = GameObject.Find(text);
			if (null == gameObject)
			{
				tmpCam = new GameObject(text, typeof(Camera));
			}
			else
			{
				tmpCam = gameObject;
			}
		}
		tmpCam.hideFlags = HideFlags.DontSave;
		tmpCam.transform.position = component.transform.position;
		tmpCam.transform.rotation = component.transform.rotation;
		tmpCam.transform.localScale = component.transform.localScale;
		tmpCam.GetComponent<Camera>().CopyFrom(component);
		tmpCam.GetComponent<Camera>().enabled = false;
		tmpCam.GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
		tmpCam.GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
		return tmpCam.GetComponent<Camera>();
	}

	private void StartFrame()
	{
		prevFramePos = Vector3.Slerp(prevFramePos, base.transform.position, 0.75f);
	}

	private int divRoundUp(int x, int d)
	{
		return (x + d - 1) / d;
	}
}
