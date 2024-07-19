using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HighlightingSystem
{
	public class Highlighter : MonoBehaviour
	{
		private class HighlightingRendererCache
		{
			public Renderer rendererCached;

			public GameObject goCached;

			private Material[] sourceMaterials;

			private Material[] replacementMaterials;

			private List<int> transparentMaterialIndexes;

			private int layer;

			public HighlightingRendererCache(Renderer rend, Material[] mats, Material sharedOpaqueMaterial, int renderQueue, float zTest, float stencilRef)
			{
				rendererCached = rend;
				goCached = rend.gameObject;
				sourceMaterials = mats;
				replacementMaterials = new Material[mats.Length];
				transparentMaterialIndexes = new List<int>();
				for (int i = 0; i < mats.Length; i++)
				{
					Material material = mats[i];
					if (material == null)
					{
						continue;
					}
					string tag = material.GetTag("RenderType", true, "Opaque");
					if (tag == "Transparent" || tag == "TransparentCutout")
					{
						Material material2 = new Material(transparentShader);
						material2.renderQueue = renderQueue;
						material2.SetFloat(ShaderPropertyID._ZTest, zTest);
						material2.SetFloat(ShaderPropertyID._StencilRef, stencilRef);
						if (material.HasProperty(ShaderPropertyID._MainTex))
						{
							material2.SetTexture(ShaderPropertyID._MainTex, material.mainTexture);
							material2.SetTextureOffset("_MainTex", material.mainTextureOffset);
							material2.SetTextureScale("_MainTex", material.mainTextureScale);
						}
						int cutoff = ShaderPropertyID._Cutoff;
						material2.SetFloat(cutoff, (!material.HasProperty(cutoff)) ? transparentCutoff : material.GetFloat(cutoff));
						replacementMaterials[i] = material2;
						transparentMaterialIndexes.Add(i);
					}
					else
					{
						replacementMaterials[i] = sharedOpaqueMaterial;
					}
				}
			}

			public void SetState(bool state)
			{
				if (state)
				{
					layer = goCached.layer;
					goCached.layer = 7;
					rendererCached.sharedMaterials = replacementMaterials;
				}
				else
				{
					goCached.layer = layer;
					rendererCached.sharedMaterials = sourceMaterials;
				}
			}

			public void SetColorForTransparent(Color clr)
			{
				for (int i = 0; i < transparentMaterialIndexes.Count; i++)
				{
					replacementMaterials[transparentMaterialIndexes[i]].SetColor(ShaderPropertyID._Outline, clr);
				}
			}

			public void SetRenderQueueForTransparent(int renderQueue)
			{
				for (int i = 0; i < transparentMaterialIndexes.Count; i++)
				{
					replacementMaterials[transparentMaterialIndexes[i]].renderQueue = renderQueue;
				}
			}

			public void SetZTestForTransparent(float zTest)
			{
				for (int i = 0; i < transparentMaterialIndexes.Count; i++)
				{
					replacementMaterials[transparentMaterialIndexes[i]].SetFloat(ShaderPropertyID._ZTest, zTest);
				}
			}

			public void SetStencilRefForTransparent(float stencilRef)
			{
				for (int i = 0; i < transparentMaterialIndexes.Count; i++)
				{
					replacementMaterials[transparentMaterialIndexes[i]].SetFloat(ShaderPropertyID._StencilRef, stencilRef);
				}
			}
		}

		private static float constantOnSpeed = 4.5f;

		private static float constantOffSpeed = 4f;

		private static float transparentCutoff = 0.5f;

		public const int highlightingLayer = 7;

		public static readonly List<Type> types = new List<Type>
		{
			typeof(MeshRenderer),
			typeof(SkinnedMeshRenderer),
			typeof(SpriteRenderer)
		};

		private const float doublePI = (float)Math.PI * 2f;

		private readonly Color occluderColor = new Color(0f, 0f, 0f, 0f);

		private const int renderQueueGeometry = 2000;

		private const int renderQueueOverlay = 2001;

		private const float zTestLessEqual = 4f;

		private const float zTestAlways = 8f;

		private static float zWrite = -1f;

		private static float offsetFactor = float.NaN;

		private static float offsetUnits = float.NaN;

		private Transform tr;

		private List<HighlightingRendererCache> highlightableRenderers;

		private bool materialsIsDirty = true;

		private bool currentState;

		private Color currentColor;

		private bool transitionActive;

		private float transitionValue;

		private float flashingFreq = 2f;

		private int _once;

		private Color onceColor = Color.red;

		private bool flashing;

		private Color flashingColorMin = new Color(0f, 1f, 1f, 0f);

		private Color flashingColorMax = new Color(0f, 1f, 1f, 1f);

		private bool constantly;

		private Color constantColor = Color.yellow;

		private bool occluder;

		private bool seeThrough = true;

		private bool renderQueue = true;

		private bool zTest = true;

		private bool stencilRef = true;

		private static Shader _opaqueShader;

		private static Shader _transparentShader;

		private Material _opaqueMaterial;

		private HighlightingBlitter Blitter;

		private bool once
		{
			get
			{
				return _once == Time.frameCount;
			}
			set
			{
				_once = (value ? Time.frameCount : 0);
			}
		}

		private int renderQueueInt
		{
			get
			{
				return (!renderQueue) ? 2000 : 2001;
			}
		}

		private float zTestFloat
		{
			get
			{
				return (!zTest) ? 4f : 8f;
			}
		}

		private float stencilRefFloat
		{
			get
			{
				return (!stencilRef) ? 0f : 1f;
			}
		}

		public static Shader opaqueShader
		{
			get
			{
				if (_opaqueShader == null)
				{
					_opaqueShader = Shader.Find("Hidden/Highlighted/Opaque");
				}
				return _opaqueShader;
			}
		}

		public static Shader transparentShader
		{
			get
			{
				if (_transparentShader == null)
				{
					_transparentShader = Shader.Find("Hidden/Highlighted/Transparent");
				}
				return _transparentShader;
			}
		}

		private Material opaqueMaterial
		{
			get
			{
				if (_opaqueMaterial == null)
				{
					_opaqueMaterial = new Material(opaqueShader);
					_opaqueMaterial.hideFlags = HideFlags.HideAndDontSave;
					ShaderPropertyID.Initialize();
					_opaqueMaterial.renderQueue = renderQueueInt;
					_opaqueMaterial.SetFloat(ShaderPropertyID._ZTest, zTestFloat);
					_opaqueMaterial.SetFloat(ShaderPropertyID._StencilRef, stencilRefFloat);
				}
				return _opaqueMaterial;
			}
		}

		public void ReinitMaterials()
		{
			materialsIsDirty = true;
		}

		public void OnParams(Color color)
		{
			onceColor = color;
		}

		public void On()
		{
			once = true;
		}

		public void On(Color color)
		{
			onceColor = color;
			On();
		}

		public void FlashingParams(Color color1, Color color2, float freq)
		{
			flashingColorMin = color1;
			flashingColorMax = color2;
			flashingFreq = freq;
		}

		public void FlashingOn()
		{
			flashing = true;
		}

		public void FlashingOn(Color color1, Color color2)
		{
			flashingColorMin = color1;
			flashingColorMax = color2;
			FlashingOn();
		}

		public void FlashingOn(Color color1, Color color2, float freq)
		{
			flashingFreq = freq;
			FlashingOn(color1, color2);
		}

		public void FlashingOn(float freq)
		{
			flashingFreq = freq;
			FlashingOn();
		}

		public void FlashingOff()
		{
			flashing = false;
		}

		public void FlashingSwitch()
		{
			flashing = !flashing;
		}

		public void ConstantParams(Color color)
		{
			constantColor = color;
		}

		public void ConstantOn()
		{
			constantly = true;
			transitionActive = true;
		}

		public void ConstantOn(Color color)
		{
			constantColor = color;
			ConstantOn();
		}

		public void ConstantOff()
		{
			constantly = false;
			transitionActive = true;
		}

		public void ConstantSwitch()
		{
			constantly = !constantly;
			transitionActive = true;
		}

		public void ConstantOnImmediate()
		{
			constantly = true;
			transitionValue = 1f;
			transitionActive = false;
		}

		public void ConstantOnImmediate(Color color)
		{
			constantColor = color;
			ConstantOnImmediate();
		}

		public void ConstantOffImmediate()
		{
			constantly = false;
			transitionValue = 0f;
			transitionActive = false;
		}

		public void ConstantSwitchImmediate()
		{
			constantly = !constantly;
			transitionValue = ((!constantly) ? 0f : 1f);
			transitionActive = false;
		}

		public void SeeThroughOn()
		{
			seeThrough = true;
		}

		public void SeeThroughOff()
		{
			seeThrough = false;
		}

		public void SeeThroughSwitch()
		{
			seeThrough = !seeThrough;
		}

		public void OccluderOn()
		{
			occluder = true;
		}

		public void OccluderOff()
		{
			occluder = false;
		}

		public void OccluderSwitch()
		{
			occluder = !occluder;
		}

		public void Off()
		{
			once = false;
			flashing = false;
			constantly = false;
			transitionValue = 0f;
			transitionActive = false;
		}

		public void Die()
		{
			UnityEngine.Object.Destroy(this);
		}

		private void Awake()
		{
			Blitter = Camera.main.GetComponent<HighlightingBlitter>();
			tr = GetComponent<Transform>();
			ShaderPropertyID.Initialize();
		}

		private void OnEnable()
		{
			if (CheckInstance())
			{
				HighlightingBase.AddHighlighter(this);
			}
		}

		private void OnDisable()
		{
			HighlightingBase.RemoveHighlighter(this);
			if (highlightableRenderers != null)
			{
				highlightableRenderers.Clear();
			}
			materialsIsDirty = true;
			currentState = false;
			currentColor = Color.clear;
			transitionActive = false;
			transitionValue = 0f;
			once = false;
			flashing = false;
			constantly = false;
			occluder = false;
			seeThrough = false;
		}

		private void Update()
		{
			PerformTransition();
		}

		private bool CheckInstance()
		{
			Highlighter[] components = GetComponents<Highlighter>();
			if (components.Length > 1 && components[0] != this)
			{
				base.enabled = false;
				Debug.LogWarning("HighlightingSystem : Multiple Highlighter components on a single GameObject is not allowed! Highlighter has been disabled on a GameObject with name '" + base.gameObject.name + "'.");
				return false;
			}
			return true;
		}

		private void InitMaterials()
		{
			highlightableRenderers = new List<HighlightingRendererCache>();
			List<Renderer> renderers = new List<Renderer>();
			GrabRenderers(tr, ref renderers);
			CacheRenderers(renderers);
			currentState = false;
			materialsIsDirty = false;
			currentColor = Color.clear;
		}

		private void GrabRenderers(Transform t, ref List<Renderer> renderers)
		{
			GameObject gameObject = t.gameObject;
			IEnumerator enumerator;
			for (int i = 0; i < types.Count; i++)
			{
				Component[] components = gameObject.GetComponents(types[i]);
				enumerator = components.GetEnumerator();
				while (enumerator.MoveNext())
				{
					renderers.Add(enumerator.Current as Renderer);
				}
			}
			if (t.childCount == 0)
			{
				return;
			}
			enumerator = t.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Transform transform = enumerator.Current as Transform;
				GameObject gameObject2 = transform.gameObject;
				Highlighter component = gameObject2.GetComponent<Highlighter>();
				if (!(component != null))
				{
					GrabRenderers(transform, ref renderers);
				}
			}
		}

		private void CacheRenderers(List<Renderer> renderers)
		{
			int count = renderers.Count;
			for (int i = 0; i < count; i++)
			{
				Material[] sharedMaterials = renderers[i].sharedMaterials;
				if (sharedMaterials != null)
				{
					highlightableRenderers.Add(new HighlightingRendererCache(renderers[i], sharedMaterials, opaqueMaterial, renderQueueInt, zTestFloat, stencilRefFloat));
				}
			}
		}

		public bool Highlight()
		{
			if (materialsIsDirty)
			{
				InitMaterials();
			}
			currentState = once || flashing || constantly || transitionActive;
			if (HighlightingBase.current.isDepthAvailable || currentState)
			{
				UpdateShaderParams(seeThrough, seeThrough, true);
			}
			else if (occluder)
			{
				UpdateShaderParams(false, false, false);
				currentState = true;
			}
			if (currentState)
			{
				bool flag = false;
				for (int num = highlightableRenderers.Count - 1; num >= 0; num--)
				{
					HighlightingRendererCache highlightingRendererCache = highlightableRenderers[num];
					if (highlightingRendererCache.goCached == null || highlightingRendererCache.rendererCached == null)
					{
						highlightableRenderers.RemoveAt(num);
					}
					else if (highlightingRendererCache.rendererCached.isVisible)
					{
						flag = flag || true;
					}
				}
				if (!flag)
				{
					currentState = false;
					return false;
				}
				UpdateColors();
				if (highlightableRenderers != null)
				{
					for (int num2 = highlightableRenderers.Count - 1; num2 >= 0; num2--)
					{
						highlightableRenderers[num2].SetState(true);
					}
					return true;
				}
				return false;
			}
			return false;
		}

		public void Extinguish()
		{
			if (currentState && highlightableRenderers != null)
			{
				for (int i = 0; i < highlightableRenderers.Count; i++)
				{
					highlightableRenderers[i].SetState(false);
				}
			}
		}

		private void UpdateShaderParams(bool rq, bool zt, bool sr)
		{
			if (renderQueue != rq)
			{
				renderQueue = rq;
				int renderQueueForTransparent = renderQueueInt;
				opaqueMaterial.renderQueue = renderQueueForTransparent;
				for (int i = 0; i < highlightableRenderers.Count; i++)
				{
					highlightableRenderers[i].SetRenderQueueForTransparent(renderQueueForTransparent);
				}
			}
			if (zTest != zt)
			{
				zTest = zt;
				float num = zTestFloat;
				opaqueMaterial.SetFloat(ShaderPropertyID._ZTest, num);
				for (int j = 0; j < highlightableRenderers.Count; j++)
				{
					highlightableRenderers[j].SetZTestForTransparent(num);
				}
			}
			if (stencilRef != sr)
			{
				stencilRef = sr;
				float num2 = stencilRefFloat;
				opaqueMaterial.SetFloat(ShaderPropertyID._StencilRef, num2);
				for (int k = 0; k < highlightableRenderers.Count; k++)
				{
					highlightableRenderers[k].SetStencilRefForTransparent(num2);
				}
			}
		}

		private void UpdateColors()
		{
			if (once)
			{
				SetColor(onceColor);
			}
			else if (flashing)
			{
				Color color = Color.Lerp(flashingColorMin, flashingColorMax, 0.5f * Mathf.Sin(Time.realtimeSinceStartup * flashingFreq * ((float)Math.PI * 2f)) + 0.5f);
				SetColor(color);
			}
			else if (transitionActive)
			{
				Color color2 = new Color(constantColor.r, constantColor.g, constantColor.b, constantColor.a * transitionValue);
				SetColor(color2);
			}
			else if (constantly)
			{
				SetColor(constantColor);
			}
			else if (occluder)
			{
				SetColor(occluderColor);
			}
		}

		private void SetColor(Color value)
		{
			if (!(currentColor == value))
			{
				currentColor = value;
				opaqueMaterial.SetColor(ShaderPropertyID._Outline, currentColor);
				for (int i = 0; i < highlightableRenderers.Count; i++)
				{
					highlightableRenderers[i].SetColorForTransparent(currentColor);
				}
			}
		}

		private void PerformTransition()
		{
			if (transitionActive)
			{
				float num = ((!constantly) ? 0f : 1f);
				if (transitionValue == num)
				{
					transitionActive = false;
				}
				else if (Time.timeScale != 0f)
				{
					float num2 = Time.deltaTime / Time.timeScale;
					transitionValue = Mathf.Clamp01(transitionValue + ((!constantly) ? (0f - constantOffSpeed) : constantOnSpeed) * num2);
				}
			}
		}

		public static void SetZWrite(float value)
		{
			if (zWrite != value)
			{
				zWrite = value;
				Shader.SetGlobalFloat(ShaderPropertyID._HighlightingZWrite, zWrite);
			}
		}

		public static void SetOffsetFactor(float value)
		{
			if (offsetFactor != value)
			{
				offsetFactor = value;
				Shader.SetGlobalFloat(ShaderPropertyID._HighlightingOffsetFactor, offsetFactor);
			}
		}

		public static void SetOffsetUnits(float value)
		{
			if (offsetUnits != value)
			{
				offsetUnits = value;
				Shader.SetGlobalFloat(ShaderPropertyID._HighlightingOffsetUnits, offsetUnits);
			}
		}
	}
}
