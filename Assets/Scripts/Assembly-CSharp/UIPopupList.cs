using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Popup List")]
public class UIPopupList : UIWidgetContainer
{
	public enum Position
	{
		Auto = 0,
		Above = 1,
		Below = 2
	}

	public enum Selection
	{
		OnPress = 0,
		OnClick = 1
	}

	public enum OpenOn
	{
		ClickOrTap = 0,
		RightClick = 1,
		DoubleClick = 2,
		Manual = 3
	}

	public delegate void LegacyEvent(string val);

	public static UIPopupList current;

	protected static GameObject mChild;

	protected static float mFadeOutComplete;

	private const float animSpeed = 0.15f;

	public UIAtlas atlas;

	public UIFont bitmapFont;

	public Font trueTypeFont;

	public int fontSize = 16;

	public FontStyle fontStyle;

	public string backgroundSprite;

	public string highlightSprite;

	public Sprite background2DSprite;

	public Sprite highlight2DSprite;

	public Position position;

	public Selection selection;

	public NGUIText.Alignment alignment = NGUIText.Alignment.Left;

	public List<string> items = new List<string>();

	public List<object> itemData = new List<object>();

	public Vector2 padding = new Vector3(4f, 4f);

	public Color textColor = Color.white;

	public Color backgroundColor = Color.white;

	public Color highlightColor = new Color(0.88235295f, 40f / 51f, 0.5882353f, 1f);

	public bool isAnimated = true;

	public bool isLocalized;

	public UILabel.Modifier textModifier;

	public bool separatePanel = true;

	public int overlap;

	public OpenOn openOn;

	public List<EventDelegate> onChange = new List<EventDelegate>();

	[HideInInspector]
	[SerializeField]
	protected string mSelectedItem;

	[HideInInspector]
	[SerializeField]
	protected UIPanel mPanel;

	[HideInInspector]
	[SerializeField]
	protected UIBasicSprite mBackground;

	[HideInInspector]
	[SerializeField]
	protected UIBasicSprite mHighlight;

	[HideInInspector]
	[SerializeField]
	protected UILabel mHighlightedLabel;

	[HideInInspector]
	[SerializeField]
	protected List<UILabel> mLabelList = new List<UILabel>();

	[HideInInspector]
	[SerializeField]
	protected float mBgBorder;

	[Tooltip("Whether the selection will be persistent even after the popup list is closed. By default the selection is cleared when the popup is closed so that the same selection can be chosen again the next time the popup list is opened. If enabled, the selection will persist, but selecting the same choice in succession will not result in the onChange notification being triggered more than once.")]
	public bool keepValue;

	[NonSerialized]
	protected GameObject mSelection;

	[NonSerialized]
	protected int mOpenFrame;

	[HideInInspector]
	[SerializeField]
	private GameObject eventReceiver;

	[HideInInspector]
	[SerializeField]
	private string functionName = "OnSelectionChange";

	[HideInInspector]
	[SerializeField]
	private float textScale;

	[HideInInspector]
	[SerializeField]
	private UIFont font;

	[HideInInspector]
	[SerializeField]
	private UILabel textLabel;

	[NonSerialized]
	public Vector3 startingPosition;

	private LegacyEvent mLegacyEvent;

	[NonSerialized]
	protected bool mExecuting;

	protected bool mUseDynamicFont;

	[NonSerialized]
	protected bool mStarted;

	protected bool mTweening;

	public GameObject source;

	public UnityEngine.Object ambigiousFont
	{
		get
		{
			if (trueTypeFont != null)
			{
				return trueTypeFont;
			}
			if (bitmapFont != null)
			{
				return bitmapFont;
			}
			return font;
		}
		set
		{
			if (value is Font)
			{
				trueTypeFont = value as Font;
				bitmapFont = null;
				font = null;
			}
			else if (value is UIFont)
			{
				bitmapFont = value as UIFont;
				trueTypeFont = null;
				font = null;
			}
		}
	}

	[Obsolete("Use EventDelegate.Add(popup.onChange, YourCallback) instead, and UIPopupList.current.value to determine the state")]
	public LegacyEvent onSelectionChange
	{
		get
		{
			return mLegacyEvent;
		}
		set
		{
			mLegacyEvent = value;
		}
	}

	public static bool isOpen
	{
		get
		{
			return current != null && (mChild != null || mFadeOutComplete > Time.unscaledTime);
		}
	}

	public virtual string value
	{
		get
		{
			return mSelectedItem;
		}
		set
		{
			Set(value);
		}
	}

	public virtual object data
	{
		get
		{
			int num = items.IndexOf(mSelectedItem);
			return (num <= -1 || num >= itemData.Count) ? null : itemData[num];
		}
	}

	public bool isColliderEnabled
	{
		get
		{
			Collider component = GetComponent<Collider>();
			if (component != null)
			{
				return component.enabled;
			}
			Collider2D component2 = GetComponent<Collider2D>();
			return component2 != null && component2.enabled;
		}
	}

	protected bool isValid
	{
		get
		{
			return bitmapFont != null || trueTypeFont != null;
		}
	}

	protected int activeFontSize
	{
		get
		{
			return (!(trueTypeFont != null) && !(bitmapFont == null)) ? bitmapFont.defaultSize : fontSize;
		}
	}

	protected float activeFontScale
	{
		get
		{
			return (!(trueTypeFont != null) && !(bitmapFont == null)) ? ((float)fontSize / (float)bitmapFont.defaultSize) : 1f;
		}
	}

	public void Set(string value, bool notify = true)
	{
		if (!(mSelectedItem != value))
		{
			return;
		}
		mSelectedItem = value;
		if (mSelectedItem != null)
		{
			if (notify && mSelectedItem != null)
			{
				TriggerCallbacks();
			}
			if (!keepValue)
			{
				mSelectedItem = null;
			}
		}
	}

	public virtual void Clear()
	{
		items.Clear();
		itemData.Clear();
	}

	public virtual void AddItem(string text)
	{
		items.Add(text);
		itemData.Add(text);
	}

	public virtual void AddItem(string text, object data)
	{
		items.Add(text);
		itemData.Add(data);
	}

	public virtual void RemoveItem(string text)
	{
		int num = items.IndexOf(text);
		if (num != -1)
		{
			items.RemoveAt(num);
			itemData.RemoveAt(num);
		}
	}

	public virtual void RemoveItemByData(object data)
	{
		int num = itemData.IndexOf(data);
		if (num != -1)
		{
			items.RemoveAt(num);
			itemData.RemoveAt(num);
		}
	}

	protected void TriggerCallbacks()
	{
		if (!mExecuting)
		{
			mExecuting = true;
			UIPopupList uIPopupList = current;
			current = this;
			if (mLegacyEvent != null)
			{
				mLegacyEvent(mSelectedItem);
			}
			if (EventDelegate.IsValid(onChange))
			{
				EventDelegate.Execute(onChange);
			}
			else if (eventReceiver != null && !string.IsNullOrEmpty(functionName))
			{
				eventReceiver.SendMessage(functionName, mSelectedItem, SendMessageOptions.DontRequireReceiver);
			}
			current = uIPopupList;
			mExecuting = false;
		}
	}

	protected virtual void OnEnable()
	{
		if (EventDelegate.IsValid(onChange))
		{
			eventReceiver = null;
			functionName = null;
		}
		if (font != null)
		{
			if (font.isDynamic)
			{
				trueTypeFont = font.dynamicFont;
				fontStyle = font.dynamicFontStyle;
				mUseDynamicFont = true;
			}
			else if (bitmapFont == null)
			{
				bitmapFont = font;
				mUseDynamicFont = false;
			}
			font = null;
		}
		if (textScale != 0f)
		{
			fontSize = ((!(bitmapFont != null)) ? 16 : Mathf.RoundToInt((float)bitmapFont.defaultSize * textScale));
			textScale = 0f;
		}
		if (trueTypeFont == null && bitmapFont != null && bitmapFont.isDynamic)
		{
			trueTypeFont = bitmapFont.dynamicFont;
			bitmapFont = null;
		}
	}

	protected virtual void OnValidate()
	{
		Font font = trueTypeFont;
		UIFont uIFont = bitmapFont;
		bitmapFont = null;
		trueTypeFont = null;
		if (font != null && (uIFont == null || !mUseDynamicFont))
		{
			bitmapFont = null;
			trueTypeFont = font;
			mUseDynamicFont = true;
		}
		else if (uIFont != null)
		{
			if (uIFont.isDynamic)
			{
				trueTypeFont = uIFont.dynamicFont;
				fontStyle = uIFont.dynamicFontStyle;
				fontSize = uIFont.defaultSize;
				mUseDynamicFont = true;
			}
			else
			{
				bitmapFont = uIFont;
				mUseDynamicFont = false;
			}
		}
		else
		{
			trueTypeFont = font;
			mUseDynamicFont = true;
		}
	}

	public virtual void Start()
	{
		if (!mStarted)
		{
			mStarted = true;
			if (keepValue)
			{
				string text = mSelectedItem;
				mSelectedItem = null;
				value = text;
			}
			else
			{
				mSelectedItem = null;
			}
			if (textLabel != null)
			{
				EventDelegate.Add(onChange, textLabel.SetCurrentSelection);
				textLabel = null;
			}
		}
	}

	protected virtual void OnLocalize()
	{
		if (isLocalized)
		{
			TriggerCallbacks();
		}
	}

	protected virtual void Highlight(UILabel lbl, bool instant)
	{
		if (!(mHighlight != null))
		{
			return;
		}
		mHighlightedLabel = lbl;
		Vector3 highlightPosition = GetHighlightPosition();
		if (!instant && isAnimated)
		{
			TweenPosition.Begin(mHighlight.gameObject, 0.1f, highlightPosition).method = UITweener.Method.EaseOut;
			if (!mTweening)
			{
				mTweening = true;
				StartCoroutine("UpdateTweenPosition");
			}
		}
		else
		{
			mHighlight.cachedTransform.localPosition = highlightPosition;
		}
	}

	protected virtual Vector3 GetHighlightPosition()
	{
		if (mHighlightedLabel == null || mHighlight == null)
		{
			return Vector3.zero;
		}
		Vector4 border = mHighlight.border;
		float num = ((!(atlas != null)) ? 1f : atlas.pixelSize);
		float num2 = border.x * num;
		float y = border.w * num;
		return mHighlightedLabel.cachedTransform.localPosition + new Vector3(0f - num2, y, 1f);
	}

	protected virtual IEnumerator UpdateTweenPosition()
	{
		if (mHighlight != null && mHighlightedLabel != null)
		{
			TweenPosition tp = mHighlight.GetComponent<TweenPosition>();
			while (tp != null && tp.enabled)
			{
				tp.to = GetHighlightPosition();
				yield return null;
			}
		}
		mTweening = false;
	}

	protected virtual void OnItemHover(GameObject go, bool isOver)
	{
		if (isOver)
		{
			UILabel component = go.GetComponent<UILabel>();
			Highlight(component, false);
		}
	}

	protected virtual void OnItemPress(GameObject go, bool isPressed)
	{
		if (isPressed && selection == Selection.OnPress)
		{
			OnItemClick(go);
		}
	}

	protected virtual void OnItemClick(GameObject go)
	{
		Select(go.GetComponent<UILabel>(), true);
		UIEventListener component = go.GetComponent<UIEventListener>();
		value = component.parameter as string;
		UIPlaySound[] components = GetComponents<UIPlaySound>();
		int i = 0;
		for (int num = components.Length; i < num; i++)
		{
			UIPlaySound uIPlaySound = components[i];
			if (uIPlaySound.trigger == UIPlaySound.Trigger.OnClick)
			{
				NGUITools.PlaySound(uIPlaySound.audioClip, uIPlaySound.volume, 1f);
			}
		}
		CloseSelf();
	}

	private void Select(UILabel lbl, bool instant)
	{
		Highlight(lbl, instant);
	}

	protected virtual void OnNavigate(KeyCode key)
	{
		if (!base.enabled || !(current == this))
		{
			return;
		}
		int num = mLabelList.IndexOf(mHighlightedLabel);
		if (num == -1)
		{
			num = 0;
		}
		switch (key)
		{
		case KeyCode.UpArrow:
			if (num > 0)
			{
				Select(mLabelList[--num], false);
			}
			break;
		case KeyCode.DownArrow:
			if (num + 1 < mLabelList.Count)
			{
				Select(mLabelList[++num], false);
			}
			break;
		}
	}

	protected virtual void OnKey(KeyCode key)
	{
		if (base.enabled && current == this && (key == UICamera.current.cancelKey0 || key == UICamera.current.cancelKey1))
		{
			OnSelect(false);
		}
	}

	protected virtual void OnDisable()
	{
		CloseSelf();
	}

	protected virtual void OnSelect(bool isSelected)
	{
		if (!isSelected)
		{
			GameObject selectedObject = UICamera.selectedObject;
			if (selectedObject == null || (!(selectedObject == mChild) && (!(mChild != null) || !(selectedObject != null) || !NGUITools.IsChild(mChild.transform, selectedObject.transform))))
			{
				CloseSelf();
			}
		}
	}

	public static void Close()
	{
		if (current != null)
		{
			current.CloseSelf();
			current = null;
		}
	}

	public virtual void CloseSelf()
	{
		if (!(mChild != null) || !(current == this))
		{
			return;
		}
		StopCoroutine("CloseIfUnselected");
		mSelection = null;
		mLabelList.Clear();
		if (isAnimated)
		{
			UIWidget[] componentsInChildren = mChild.GetComponentsInChildren<UIWidget>();
			int i = 0;
			for (int num = componentsInChildren.Length; i < num; i++)
			{
				UIWidget uIWidget = componentsInChildren[i];
				Color color = uIWidget.color;
				color.a = 0f;
				TweenColor.Begin(uIWidget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
			}
			Collider[] componentsInChildren2 = mChild.GetComponentsInChildren<Collider>();
			int j = 0;
			for (int num2 = componentsInChildren2.Length; j < num2; j++)
			{
				componentsInChildren2[j].enabled = false;
			}
			UnityEngine.Object.Destroy(mChild, 0.15f);
			mFadeOutComplete = Time.unscaledTime + Mathf.Max(0.1f, 0.15f);
		}
		else
		{
			UnityEngine.Object.Destroy(mChild);
			mFadeOutComplete = Time.unscaledTime + 0.1f;
		}
		mBackground = null;
		mHighlight = null;
		mChild = null;
		current = null;
	}

	protected virtual void AnimateColor(UIWidget widget)
	{
		Color color = widget.color;
		widget.color = new Color(color.r, color.g, color.b, 0f);
		TweenColor.Begin(widget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
	}

	protected virtual void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
	{
		Vector3 localPosition = widget.cachedTransform.localPosition;
		Vector3 localPosition2 = ((!placeAbove) ? new Vector3(localPosition.x, 0f, localPosition.z) : new Vector3(localPosition.x, bottom, localPosition.z));
		widget.cachedTransform.localPosition = localPosition2;
		GameObject go = widget.gameObject;
		TweenPosition.Begin(go, 0.15f, localPosition).method = UITweener.Method.EaseOut;
	}

	protected virtual void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
	{
		GameObject go = widget.gameObject;
		Transform cachedTransform = widget.cachedTransform;
		float num = (float)activeFontSize * activeFontScale + mBgBorder * 2f;
		cachedTransform.localScale = new Vector3(1f, num / (float)widget.height, 1f);
		TweenScale.Begin(go, 0.15f, Vector3.one).method = UITweener.Method.EaseOut;
		if (placeAbove)
		{
			Vector3 localPosition = cachedTransform.localPosition;
			cachedTransform.localPosition = new Vector3(localPosition.x, localPosition.y - (float)widget.height + num, localPosition.z);
			TweenPosition.Begin(go, 0.15f, localPosition).method = UITweener.Method.EaseOut;
		}
	}

	protected void Animate(UIWidget widget, bool placeAbove, float bottom)
	{
		AnimateColor(widget);
		AnimatePosition(widget, placeAbove, bottom);
	}

	protected virtual void OnClick()
	{
		if (mOpenFrame == Time.frameCount)
		{
			return;
		}
		if (mChild == null)
		{
			if (openOn != OpenOn.DoubleClick && openOn != OpenOn.Manual && (openOn != OpenOn.RightClick || UICamera.currentTouchID == -2))
			{
				Show();
			}
		}
		else if (mHighlightedLabel != null)
		{
			OnItemPress(mHighlightedLabel.gameObject, true);
		}
	}

	protected virtual void OnDoubleClick()
	{
		if (openOn == OpenOn.DoubleClick)
		{
			Show();
		}
	}

	private IEnumerator CloseIfUnselected()
	{
		GameObject sel;
		do
		{
			yield return null;
			sel = UICamera.selectedObject;
		}
		while (!(sel != mSelection) || (!(sel == null) && (sel == mChild || NGUITools.IsChild(mChild.transform, sel.transform))));
		CloseSelf();
	}

	public virtual void Show()
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && mChild == null && isValid && items.Count > 0)
		{
			mLabelList.Clear();
			StopCoroutine("CloseIfUnselected");
			UICamera.selectedObject = UICamera.hoveredObject ?? base.gameObject;
			mSelection = UICamera.selectedObject;
			source = mSelection;
			if (source == null)
			{
				Debug.LogError("Popup list needs a source object...");
				return;
			}
			mOpenFrame = Time.frameCount;
			if (mPanel == null)
			{
				mPanel = UIPanel.Find(base.transform);
				if (mPanel == null)
				{
					return;
				}
			}
			mChild = new GameObject("Drop-down List");
			mChild.layer = base.gameObject.layer;
			if (separatePanel)
			{
				if (GetComponent<Collider>() != null)
				{
					Rigidbody rigidbody = mChild.AddComponent<Rigidbody>();
					rigidbody.isKinematic = true;
				}
				else if (GetComponent<Collider2D>() != null)
				{
					Rigidbody2D rigidbody2D = mChild.AddComponent<Rigidbody2D>();
					rigidbody2D.isKinematic = true;
				}
				UIPanel uIPanel = mChild.AddComponent<UIPanel>();
				uIPanel.depth = 1000000;
				uIPanel.sortingOrder = mPanel.sortingOrder;
			}
			current = this;
			Transform transform = ((!separatePanel) ? mPanel.cachedTransform : ((Component)(((object)mPanel.GetComponentInParent<UIRoot>()) ?? ((object)mPanel))).transform);
			Transform transform2 = mChild.transform;
			transform2.parent = transform;
			Vector3 vector;
			Vector3 vector2;
			if (openOn == OpenOn.Manual && mSelection != base.gameObject)
			{
				startingPosition = UICamera.lastEventPosition;
				vector = transform.InverseTransformPoint(mPanel.anchorCamera.ScreenToWorldPoint(startingPosition));
				vector2 = vector;
				transform2.localPosition = vector;
				startingPosition = transform2.position;
			}
			else
			{
				Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform, base.transform, false, false);
				vector = bounds.min;
				vector2 = bounds.max;
				transform2.localPosition = vector;
				startingPosition = transform2.position;
			}
			StartCoroutine("CloseIfUnselected");
			transform2.localRotation = Quaternion.identity;
			transform2.localScale = Vector3.one;
			int num = ((!separatePanel) ? NGUITools.CalculateNextDepth(mPanel.gameObject) : 0);
			if (background2DSprite != null)
			{
				UI2DSprite uI2DSprite = mChild.AddWidget<UI2DSprite>(num);
				uI2DSprite.sprite2D = background2DSprite;
				mBackground = uI2DSprite;
			}
			else
			{
				if (!(atlas != null))
				{
					return;
				}
				mBackground = mChild.AddSprite(atlas, backgroundSprite, num);
			}
			bool flag = position == Position.Above;
			if (position == Position.Auto)
			{
				UICamera uICamera = UICamera.FindCameraForLayer(mSelection.layer);
				if (uICamera != null)
				{
					flag = uICamera.cachedCamera.WorldToViewportPoint(startingPosition).y < 0.5f;
				}
			}
			mBackground.pivot = UIWidget.Pivot.TopLeft;
			mBackground.color = backgroundColor;
			Vector4 border = mBackground.border;
			mBgBorder = border.y;
			mBackground.cachedTransform.localPosition = new Vector3(0f, (!flag) ? ((float)overlap) : (border.y * 2f - (float)overlap), 0f);
			if (highlight2DSprite != null)
			{
				UI2DSprite uI2DSprite2 = mChild.AddWidget<UI2DSprite>(++num);
				uI2DSprite2.sprite2D = highlight2DSprite;
				mHighlight = uI2DSprite2;
			}
			else
			{
				if (!(atlas != null))
				{
					return;
				}
				mHighlight = mChild.AddSprite(atlas, highlightSprite, ++num);
			}
			float num2 = 0f;
			float num3 = 0f;
			if (mHighlight.hasBorder)
			{
				num2 = mHighlight.border.w;
				num3 = mHighlight.border.x;
			}
			mHighlight.pivot = UIWidget.Pivot.TopLeft;
			mHighlight.color = highlightColor;
			float num4 = activeFontSize;
			float num5 = activeFontScale;
			float num6 = num4 * num5;
			float num7 = num6 + padding.y;
			float a = 0f;
			float num8 = ((!flag) ? (0f - padding.y - border.y + (float)overlap) : (border.y - padding.y - (float)overlap));
			float num9 = border.y * 2f + padding.y;
			List<UILabel> list = new List<UILabel>();
			if (!items.Contains(mSelectedItem))
			{
				mSelectedItem = null;
			}
			int i = 0;
			for (int count = items.Count; i < count; i++)
			{
				string text = items[i];
				UILabel uILabel = mChild.AddWidget<UILabel>(mBackground.depth + 2);
				uILabel.name = i.ToString();
				uILabel.pivot = UIWidget.Pivot.TopLeft;
				uILabel.bitmapFont = bitmapFont;
				uILabel.trueTypeFont = trueTypeFont;
				uILabel.fontSize = fontSize;
				uILabel.fontStyle = fontStyle;
				uILabel.text = ((!isLocalized) ? text : Localization.Get(text));
				uILabel.modifier = textModifier;
				uILabel.color = textColor;
				uILabel.cachedTransform.localPosition = new Vector3(border.x + padding.x - uILabel.pivotOffset.x, num8, -1f);
				uILabel.overflowMethod = UILabel.Overflow.ResizeFreely;
				uILabel.alignment = alignment;
				list.Add(uILabel);
				num9 += num7;
				num8 -= num7;
				a = Mathf.Max(a, uILabel.printedSize.x);
				UIEventListener uIEventListener = UIEventListener.Get(uILabel.gameObject);
				uIEventListener.onHover = OnItemHover;
				uIEventListener.onPress = OnItemPress;
				uIEventListener.onClick = OnItemClick;
				uIEventListener.parameter = text;
				if (mSelectedItem == text || (i == 0 && string.IsNullOrEmpty(mSelectedItem)))
				{
					Highlight(uILabel, true);
				}
				mLabelList.Add(uILabel);
			}
			a = Mathf.Max(a, vector2.x - vector.x - (border.x + padding.x) * 2f);
			float num10 = a;
			Vector3 vector3 = new Vector3(num10 * 0.5f, (0f - num6) * 0.5f, 0f);
			Vector3 vector4 = new Vector3(num10, num6 + padding.y, 1f);
			int j = 0;
			for (int count2 = list.Count; j < count2; j++)
			{
				UILabel uILabel2 = list[j];
				NGUITools.AddWidgetCollider(uILabel2.gameObject);
				uILabel2.autoResizeBoxCollider = false;
				BoxCollider component = uILabel2.GetComponent<BoxCollider>();
				if (component != null)
				{
					vector3.z = component.center.z;
					component.center = vector3;
					component.size = vector4;
				}
				else
				{
					BoxCollider2D component2 = uILabel2.GetComponent<BoxCollider2D>();
					component2.offset = vector3;
					component2.size = vector4;
				}
			}
			int width = Mathf.RoundToInt(a);
			a += (border.x + padding.x) * 2f;
			num8 -= border.y;
			mBackground.width = Mathf.RoundToInt(a);
			mBackground.height = Mathf.RoundToInt(num9);
			int k = 0;
			for (int count3 = list.Count; k < count3; k++)
			{
				UILabel uILabel3 = list[k];
				uILabel3.overflowMethod = UILabel.Overflow.ShrinkContent;
				uILabel3.width = width;
			}
			float num11 = ((!(atlas != null)) ? 2f : (2f * atlas.pixelSize));
			float f = a - (border.x + padding.x) * 2f + num3 * num11;
			float f2 = num6 + num2 * num11;
			mHighlight.width = Mathf.RoundToInt(f);
			mHighlight.height = Mathf.RoundToInt(f2);
			if (isAnimated)
			{
				AnimateColor(mBackground);
				if (Time.timeScale == 0f || Time.timeScale >= 0.1f)
				{
					float bottom = num8 + num6;
					Animate(mHighlight, flag, bottom);
					int l = 0;
					for (int count4 = list.Count; l < count4; l++)
					{
						Animate(list[l], flag, bottom);
					}
					AnimateScale(mBackground, flag, bottom);
				}
			}
			if (flag)
			{
				vector.y = vector2.y - border.y;
				vector2.y = vector.y + (float)mBackground.height;
				vector2.x = vector.x + (float)mBackground.width;
				transform2.localPosition = new Vector3(vector.x, vector2.y - border.y, vector.z);
			}
			else
			{
				vector2.y = vector.y + border.y;
				vector.y = vector2.y - (float)mBackground.height;
				vector2.x = vector.x + (float)mBackground.width;
			}
			Transform parent = mPanel.cachedTransform.parent;
			if (parent != null)
			{
				vector = mPanel.cachedTransform.TransformPoint(vector);
				vector2 = mPanel.cachedTransform.TransformPoint(vector2);
				vector = parent.InverseTransformPoint(vector);
				vector2 = parent.InverseTransformPoint(vector2);
				float pixelSizeAdjustment = UIRoot.GetPixelSizeAdjustment(base.gameObject);
				vector /= pixelSizeAdjustment;
				vector2 /= pixelSizeAdjustment;
			}
			Vector3 vector5 = ((!mPanel.hasClipping) ? mPanel.CalculateConstrainOffset(vector, vector2) : Vector3.zero);
			Vector3 localPosition = transform2.localPosition + vector5;
			localPosition.x = Mathf.Round(localPosition.x);
			localPosition.y = Mathf.Round(localPosition.y);
			transform2.localPosition = localPosition;
		}
		else
		{
			OnSelect(false);
		}
	}
}
