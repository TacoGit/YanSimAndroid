using System.Collections.Generic;
using UnityEngine;

namespace HighlightingSystem
{
	[RequireComponent(typeof(Camera))]
	public class HighlightingBase : MonoBehaviour
	{
		public static HighlightingBase current;

		protected static List<Highlighter> highlighters;

		public float offsetFactor;

		public float offsetUnits;

		public int _downsampleFactor = 4;

		public int iterations = 2;

		public float blurMinSpread = 0.65f;

		public float blurSpread = 0.25f;

		public float _blurIntensity = 0.3f;

		protected int layerMask = 128;

		private const int D3D9 = 0;

		private const int D3D11 = 1;

		private const int OGL = 2;

		private int graphicsDeviceVersion;

		protected GameObject shaderCameraGO;

		protected Camera shaderCamera;

		protected RenderTexture highlightingBuffer;

		protected GameObject go;

		protected Camera refCam;

		protected bool isSupported;

		protected bool _isDepthAvailable = true;

		protected static Shader _blitShader;

		protected static Shader _blurShader;

		protected static Shader _cutShader;

		protected static Shader _compShader;

		protected static Material _blitMaterial;

		protected static Material _blurMaterial;

		protected static Material _cutMaterial;

		protected static Material _compMaterial;

		public bool isDepthAvailable
		{
			get
			{
				return _isDepthAvailable;
			}
			protected set
			{
				_isDepthAvailable = value;
			}
		}

		protected static Shader blitShader
		{
			get
			{
				if (_blitShader == null)
				{
					_blitShader = Shader.Find("Hidden/Highlighted/Blit");
				}
				return _blitShader;
			}
		}

		protected static Shader blurShader
		{
			get
			{
				if (_blurShader == null)
				{
					_blurShader = Shader.Find("Hidden/Highlighted/Blur");
				}
				return _blurShader;
			}
		}

		protected static Shader cutShader
		{
			get
			{
				if (_cutShader == null)
				{
					_cutShader = Shader.Find("Hidden/Highlighted/Cut");
				}
				return _cutShader;
			}
		}

		protected static Shader compShader
		{
			get
			{
				if (_compShader == null)
				{
					_compShader = Shader.Find("Hidden/Highlighted/Composite");
				}
				return _compShader;
			}
		}

		protected static Material blitMaterial
		{
			get
			{
				if (_blitMaterial == null)
				{
					_blitMaterial = new Material(blitShader);
					_blitMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				return _blitMaterial;
			}
		}

		protected static Material blurMaterial
		{
			get
			{
				if (_blurMaterial == null)
				{
					_blurMaterial = new Material(blurShader);
					_blurMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				return _blurMaterial;
			}
		}

		protected static Material cutMaterial
		{
			get
			{
				if (_cutMaterial == null)
				{
					_cutMaterial = new Material(cutShader);
					_cutMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				return _cutMaterial;
			}
		}

		protected static Material compMaterial
		{
			get
			{
				if (_compMaterial == null)
				{
					_compMaterial = new Material(compShader);
					_compMaterial.hideFlags = HideFlags.HideAndDontSave;
				}
				return _compMaterial;
			}
		}

		protected virtual void Awake()
		{
			ShaderPropertyID.Initialize();
			go = base.gameObject;
			refCam = GetComponent<Camera>();
			if (highlighters == null)
			{
				highlighters = new List<Highlighter>();
			}
			string text = SystemInfo.graphicsDeviceVersion.ToLower();
			if (text.StartsWith("direct3d 11"))
			{
				graphicsDeviceVersion = 1;
			}
			else if (text.StartsWith("opengl"))
			{
				graphicsDeviceVersion = 2;
			}
			else
			{
				graphicsDeviceVersion = 0;
			}
		}

		protected virtual void OnEnable()
		{
			if (CheckInstance())
			{
				isSupported = CheckSupported();
				if (isSupported)
				{
					blurMaterial.SetFloat(ShaderPropertyID._Intensity, _blurIntensity);
					return;
				}
				base.enabled = false;
				Debug.LogWarning("HighlightingSystem : Highlighting System has been disabled due to unsupported Unity features on the current platform!");
			}
		}

		protected virtual void OnDisable()
		{
			if (shaderCameraGO != null)
			{
				Object.DestroyImmediate(shaderCameraGO);
			}
			if (highlightingBuffer != null)
			{
				RenderTexture.ReleaseTemporary(highlightingBuffer);
				highlightingBuffer = null;
			}
		}

		protected virtual void OnDestroy()
		{
			if (_blitMaterial != null)
			{
				Object.DestroyImmediate(_blitMaterial);
			}
			if (_blurMaterial != null)
			{
				Object.DestroyImmediate(_blurMaterial);
			}
			if (_cutMaterial != null)
			{
				Object.DestroyImmediate(_cutMaterial);
			}
			if (_compMaterial != null)
			{
				Object.DestroyImmediate(_compMaterial);
			}
		}

		public bool CheckInstance()
		{
			HighlightingBase[] components = GetComponents<HighlightingBase>();
			if (components.Length > 1 && components[0] != this)
			{
				base.enabled = false;
				string arg = GetType().ToString();
				Debug.LogWarning(string.Format("HighlightingSystem : Only single instance of HighlightingRenderer / HighlightingMobile components is allowed on a single Gameobject! {0} has been disabled on GameObject with name '{1}'.", arg, base.name));
				return false;
			}
			return true;
		}

		protected bool CheckSupported()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				Debug.LogWarning("HighlightingSystem : Image effects is not supported on this platform!");
				return false;
			}
			if (!SystemInfo.supportsRenderTextures)
			{
				Debug.LogWarning("HighlightingSystem : RenderTextures is not supported on this platform!");
				return false;
			}
			if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32))
			{
				Debug.LogWarning("HighlightingSystem : RenderTextureFormat.ARGB32 is not supported on this platform!");
				return false;
			}
			if (!Highlighter.opaqueShader.isSupported)
			{
				Debug.LogWarning("HighlightingSystem : HighlightingOpaque shader is not supported on this platform!");
				return false;
			}
			if (!Highlighter.transparentShader.isSupported)
			{
				Debug.LogWarning("HighlightingSystem : HighlightingTransparent shader is not supported on this platform!");
				return false;
			}
			if (!blitShader.isSupported)
			{
				Debug.LogWarning("HighlightingSystem : HighlightingBlit shader is not supported on this platform!");
				return false;
			}
			if (!blurShader.isSupported)
			{
				Debug.LogWarning("HighlightingSystem : HighlightingBlur shader is not supported on this platform!");
				return false;
			}
			if (!cutShader.isSupported)
			{
				Debug.LogWarning("HighlightingSystem : HighlightingCut shader is not supported on this platform!");
				return false;
			}
			if (!compShader.isSupported)
			{
				Debug.LogWarning("HighlightingSystem : HighlightingComposite shader is not supported on this platform!");
				return false;
			}
			return true;
		}

		public void RenderHighlighting(RenderTexture frameBuffer)
		{
			if (highlightingBuffer != null)
			{
				RenderTexture.ReleaseTemporary(highlightingBuffer);
				highlightingBuffer = null;
			}
			if (!isSupported || !base.enabled || !go.activeInHierarchy)
			{
				return;
			}
			int num = QualitySettings.antiAliasing;
			if (num == 0)
			{
				num = 1;
			}
			bool flag = true;
			if (frameBuffer == null || frameBuffer.depth < 24)
			{
				flag = false;
			}
			if (refCam.actualRenderingPath == RenderingPath.DeferredLighting)
			{
				num = 1;
			}
			else if (num > 1)
			{
				flag = false;
			}
			if (isDepthAvailable != flag)
			{
				isDepthAvailable = flag;
				Highlighter.SetZWrite((!isDepthAvailable) ? 1f : 0f);
				if (isDepthAvailable)
				{
					Debug.LogWarning("HighlightingSystem : Framebuffer depth data is available back again and will be used to occlude highlighting. Highlighting occluders disabled.");
				}
				else
				{
					Debug.LogWarning("HighlightingSystem : Framebuffer depth data is not available and can't be used to occlude highlighting. Highlighting occluders enabled.");
				}
			}
			Highlighter.SetOffsetFactor(offsetFactor);
			Highlighter.SetOffsetUnits(offsetUnits);
			current = this;
			int num2 = 0;
			for (int i = 0; i < highlighters.Count; i++)
			{
				if (highlighters[i].Highlight())
				{
					num2++;
				}
			}
			if (num2 == 0)
			{
				current = null;
				return;
			}
			int width = Screen.width;
			int height = Screen.height;
			int depthBuffer = 24;
			if (isDepthAvailable)
			{
				width = frameBuffer.width;
				height = frameBuffer.height;
				depthBuffer = 0;
			}
			highlightingBuffer = RenderTexture.GetTemporary(width, height, depthBuffer, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, num);
			if (!highlightingBuffer.IsCreated())
			{
				highlightingBuffer.filterMode = FilterMode.Point;
				highlightingBuffer.useMipMap = false;
				highlightingBuffer.wrapMode = TextureWrapMode.Clamp;
			}
			RenderTexture.active = highlightingBuffer;
			GL.Clear((!isDepthAvailable) ? true : false, true, Color.clear);
			RenderBuffer depthBuffer2 = ((!isDepthAvailable) ? highlightingBuffer.depthBuffer : frameBuffer.depthBuffer);
			if (!shaderCameraGO)
			{
				shaderCameraGO = new GameObject("HighlightingCamera");
				shaderCameraGO.hideFlags = HideFlags.HideAndDontSave;
				shaderCamera = shaderCameraGO.AddComponent<Camera>();
				shaderCamera.enabled = false;
			}
			shaderCamera.CopyFrom(refCam);
			shaderCamera.cullingMask = layerMask;
			shaderCamera.rect = new Rect(0f, 0f, 1f, 1f);
			shaderCamera.renderingPath = RenderingPath.Forward;
			shaderCamera.depthTextureMode = DepthTextureMode.None;
			shaderCamera.allowHDR = false;
			shaderCamera.useOcclusionCulling = false;
			shaderCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
			shaderCamera.clearFlags = CameraClearFlags.Nothing;
			shaderCamera.SetTargetBuffers(highlightingBuffer.colorBuffer, depthBuffer2);
			frameBuffer.MarkRestoreExpected();
			shaderCamera.Render();
			for (int j = 0; j < highlighters.Count; j++)
			{
				highlighters[j].Extinguish();
			}
			current = null;
			int width2 = highlightingBuffer.width / _downsampleFactor;
			int height2 = highlightingBuffer.height / _downsampleFactor;
			RenderTexture temporary = RenderTexture.GetTemporary(width2, height2, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
			RenderTexture temporary2 = RenderTexture.GetTemporary(width2, height2, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 1);
			if (!temporary.IsCreated())
			{
				temporary.useMipMap = false;
				temporary.wrapMode = TextureWrapMode.Clamp;
			}
			if (!temporary2.IsCreated())
			{
				temporary2.useMipMap = false;
				temporary2.wrapMode = TextureWrapMode.Clamp;
			}
			Graphics.Blit(highlightingBuffer, temporary, blitMaterial);
			bool flag2 = true;
			for (int k = 0; k < iterations; k++)
			{
				if (flag2)
				{
					FourTapCone(temporary, temporary2, k);
				}
				else
				{
					FourTapCone(temporary2, temporary, k);
				}
				flag2 = !flag2;
			}
			Graphics.SetRenderTarget(highlightingBuffer.colorBuffer, depthBuffer2);
			cutMaterial.SetTexture(ShaderPropertyID._MainTex, (!flag2) ? temporary2 : temporary);
			DoubleBlit(cutMaterial, 0, cutMaterial, 1);
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
		}

		protected void FourTapCone(RenderTexture src, RenderTexture dst, int iteration)
		{
			float value = blurMinSpread + (float)iteration * blurSpread;
			blurMaterial.SetFloat(ShaderPropertyID._OffsetScale, value);
			Graphics.Blit(src, dst, blurMaterial);
			src.DiscardContents();
		}

		protected void DoubleBlit(Material mat1, int pass1, Material mat2, int pass2, float texelSize1 = 1f)
		{
			float y = 0f;
			float y2 = 1f;
			if (texelSize1 < 0f)
			{
				if (graphicsDeviceVersion == 0)
				{
					y = 1f - texelSize1;
					y2 = 0f - texelSize1;
				}
				else
				{
					y = 1f;
					y2 = 0f;
				}
			}
			float z = 0f;
			GL.PushMatrix();
			GL.LoadOrtho();
			mat1.SetPass(pass1);
			GL.Begin(7);
			GL.TexCoord2(0f, y);
			GL.Vertex3(0f, 0f, z);
			GL.TexCoord2(0f, y2);
			GL.Vertex3(0f, 1f, z);
			GL.TexCoord2(1f, y2);
			GL.Vertex3(1f, 1f, z);
			GL.TexCoord2(1f, y);
			GL.Vertex3(1f, 0f, z);
			GL.End();
			mat2.SetPass(pass2);
			GL.Begin(7);
			GL.TexCoord2(0f, 0f);
			GL.Vertex3(0f, 0f, z);
			GL.TexCoord2(0f, 1f);
			GL.Vertex3(0f, 1f, z);
			GL.TexCoord2(1f, 1f);
			GL.Vertex3(1f, 1f, z);
			GL.TexCoord2(1f, 0f);
			GL.Vertex3(1f, 0f, z);
			GL.End();
			GL.PopMatrix();
		}

		public void BlitHighlighting(RenderTexture src, RenderTexture dst)
		{
			if (highlightingBuffer == null)
			{
				Graphics.Blit(src, dst, blitMaterial);
				return;
			}
			Graphics.SetRenderTarget(dst);
			blitMaterial.SetTexture(ShaderPropertyID._MainTex, src);
			compMaterial.SetTexture(ShaderPropertyID._MainTex, highlightingBuffer);
			DoubleBlit(blitMaterial, 0, compMaterial, 0, src.texelSize.y);
			RenderTexture.ReleaseTemporary(highlightingBuffer);
			highlightingBuffer = null;
		}

		public static void AddHighlighter(Highlighter h)
		{
			if (highlighters == null)
			{
				highlighters = new List<Highlighter>();
			}
			highlighters.Add(h);
		}

		public static void RemoveHighlighter(Highlighter h)
		{
			if (highlighters != null)
			{
				int num = highlighters.IndexOf(h);
				if (num != -1)
				{
					highlighters.RemoveAt(num);
				}
			}
		}
	}
}
