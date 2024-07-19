using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag and Drop Item")]
public class UIDragDropItem : MonoBehaviour
{
	public enum Restriction
	{
		None = 0,
		Horizontal = 1,
		Vertical = 2,
		PressAndHold = 3
	}

	public Restriction restriction;

	public bool cloneOnDrag;

	[HideInInspector]
	public float pressAndHoldDelay = 1f;

	public bool interactable = true;

	[NonSerialized]
	protected Transform mTrans;

	[NonSerialized]
	protected Transform mParent;

	[NonSerialized]
	protected Collider mCollider;

	[NonSerialized]
	protected Collider2D mCollider2D;

	[NonSerialized]
	protected UIButton mButton;

	[NonSerialized]
	protected UIRoot mRoot;

	[NonSerialized]
	protected UIGrid mGrid;

	[NonSerialized]
	protected UITable mTable;

	[NonSerialized]
	protected float mDragStartTime;

	[NonSerialized]
	protected UIDragScrollView mDragScrollView;

	[NonSerialized]
	protected bool mPressed;

	[NonSerialized]
	protected bool mDragging;

	[NonSerialized]
	protected UICamera.MouseOrTouch mTouch;

	public static List<UIDragDropItem> draggedItems = new List<UIDragDropItem>();

	protected virtual void Awake()
	{
		mTrans = base.transform;
		mCollider = base.gameObject.GetComponent<Collider>();
		mCollider2D = base.gameObject.GetComponent<Collider2D>();
	}

	protected virtual void OnEnable()
	{
	}

	protected virtual void OnDisable()
	{
		if (mDragging)
		{
			StopDragging(UICamera.hoveredObject);
		}
	}

	protected virtual void Start()
	{
		mButton = GetComponent<UIButton>();
		mDragScrollView = GetComponent<UIDragScrollView>();
	}

	protected virtual void OnPress(bool isPressed)
	{
		if (!interactable || UICamera.currentTouchID == -2 || UICamera.currentTouchID == -3)
		{
			return;
		}
		if (isPressed)
		{
			if (!mPressed)
			{
				mTouch = UICamera.currentTouch;
				mDragStartTime = RealTime.time + pressAndHoldDelay;
				mPressed = true;
			}
		}
		else if (mPressed && mTouch == UICamera.currentTouch)
		{
			mPressed = false;
			mTouch = null;
		}
	}

	protected virtual void Update()
	{
		if (restriction == Restriction.PressAndHold && mPressed && !mDragging && mDragStartTime < RealTime.time)
		{
			StartDragging();
		}
	}

	protected virtual void OnDragStart()
	{
		if (!interactable || !base.enabled || mTouch != UICamera.currentTouch)
		{
			return;
		}
		if (restriction != 0)
		{
			if (restriction == Restriction.Horizontal)
			{
				Vector2 totalDelta = mTouch.totalDelta;
				if (Mathf.Abs(totalDelta.x) < Mathf.Abs(totalDelta.y))
				{
					return;
				}
			}
			else if (restriction == Restriction.Vertical)
			{
				Vector2 totalDelta2 = mTouch.totalDelta;
				if (Mathf.Abs(totalDelta2.x) > Mathf.Abs(totalDelta2.y))
				{
					return;
				}
			}
			else if (restriction == Restriction.PressAndHold)
			{
				return;
			}
		}
		StartDragging();
	}

	public virtual void StartDragging()
	{
		if (!interactable || mDragging)
		{
			return;
		}
		if (cloneOnDrag)
		{
			mPressed = false;
			GameObject gameObject = base.transform.parent.gameObject.AddChild(base.gameObject);
			gameObject.transform.localPosition = base.transform.localPosition;
			gameObject.transform.localRotation = base.transform.localRotation;
			gameObject.transform.localScale = base.transform.localScale;
			UIButtonColor component = gameObject.GetComponent<UIButtonColor>();
			if (component != null)
			{
				component.defaultColor = GetComponent<UIButtonColor>().defaultColor;
			}
			if (mTouch != null && mTouch.pressed == base.gameObject)
			{
				mTouch.current = gameObject;
				mTouch.pressed = gameObject;
				mTouch.dragged = gameObject;
				mTouch.last = gameObject;
			}
			UIDragDropItem component2 = gameObject.GetComponent<UIDragDropItem>();
			component2.mTouch = mTouch;
			component2.mPressed = true;
			component2.mDragging = true;
			component2.Start();
			component2.OnClone(base.gameObject);
			component2.OnDragDropStart();
			if (UICamera.currentTouch == null)
			{
				UICamera.currentTouch = mTouch;
			}
			mTouch = null;
			UICamera.Notify(base.gameObject, "OnPress", false);
			UICamera.Notify(base.gameObject, "OnHover", false);
		}
		else
		{
			mDragging = true;
			OnDragDropStart();
		}
	}

	protected virtual void OnClone(GameObject original)
	{
	}

	protected virtual void OnDrag(Vector2 delta)
	{
		if (interactable && mDragging && base.enabled && mTouch == UICamera.currentTouch)
		{
			if (mRoot != null)
			{
				OnDragDropMove(delta * mRoot.pixelSizeAdjustment);
			}
			else
			{
				OnDragDropMove(delta);
			}
		}
	}

	protected virtual void OnDragEnd()
	{
		if (interactable && base.enabled && mTouch == UICamera.currentTouch)
		{
			StopDragging(UICamera.hoveredObject);
		}
	}

	public void StopDragging(GameObject go = null)
	{
		if (mDragging)
		{
			mDragging = false;
			OnDragDropRelease(go);
		}
	}

	protected virtual void OnDragDropStart()
	{
		if (!draggedItems.Contains(this))
		{
			draggedItems.Add(this);
		}
		if (mDragScrollView != null)
		{
			mDragScrollView.enabled = false;
		}
		if (mButton != null)
		{
			mButton.isEnabled = false;
		}
		else if (mCollider != null)
		{
			mCollider.enabled = false;
		}
		else if (mCollider2D != null)
		{
			mCollider2D.enabled = false;
		}
		mParent = mTrans.parent;
		mRoot = NGUITools.FindInParents<UIRoot>(mParent);
		mGrid = NGUITools.FindInParents<UIGrid>(mParent);
		mTable = NGUITools.FindInParents<UITable>(mParent);
		if (UIDragDropRoot.root != null)
		{
			mTrans.parent = UIDragDropRoot.root;
		}
		Vector3 localPosition = mTrans.localPosition;
		localPosition.z = 0f;
		mTrans.localPosition = localPosition;
		TweenPosition component = GetComponent<TweenPosition>();
		if (component != null)
		{
			component.enabled = false;
		}
		SpringPosition component2 = GetComponent<SpringPosition>();
		if (component2 != null)
		{
			component2.enabled = false;
		}
		NGUITools.MarkParentAsChanged(base.gameObject);
		if (mTable != null)
		{
			mTable.repositionNow = true;
		}
		if (mGrid != null)
		{
			mGrid.repositionNow = true;
		}
	}

	protected virtual void OnDragDropMove(Vector2 delta)
	{
		mTrans.localPosition += mTrans.InverseTransformDirection(delta);
	}

	protected virtual void OnDragDropRelease(GameObject surface)
	{
		if (!cloneOnDrag)
		{
			UIDragScrollView[] componentsInChildren = GetComponentsInChildren<UIDragScrollView>();
			UIDragScrollView[] array = componentsInChildren;
			foreach (UIDragScrollView uIDragScrollView in array)
			{
				uIDragScrollView.scrollView = null;
			}
			if (mButton != null)
			{
				mButton.isEnabled = true;
			}
			else if (mCollider != null)
			{
				mCollider.enabled = true;
			}
			else if (mCollider2D != null)
			{
				mCollider2D.enabled = true;
			}
			UIDragDropContainer uIDragDropContainer = ((!surface) ? null : NGUITools.FindInParents<UIDragDropContainer>(surface));
			if (uIDragDropContainer != null)
			{
				mTrans.parent = ((!(uIDragDropContainer.reparentTarget != null)) ? uIDragDropContainer.transform : uIDragDropContainer.reparentTarget);
				Vector3 localPosition = mTrans.localPosition;
				localPosition.z = 0f;
				mTrans.localPosition = localPosition;
			}
			else
			{
				mTrans.parent = mParent;
			}
			mParent = mTrans.parent;
			mGrid = NGUITools.FindInParents<UIGrid>(mParent);
			mTable = NGUITools.FindInParents<UITable>(mParent);
			if (mDragScrollView != null)
			{
				Invoke("EnableDragScrollView", 0.001f);
			}
			NGUITools.MarkParentAsChanged(base.gameObject);
			if (mTable != null)
			{
				mTable.repositionNow = true;
			}
			if (mGrid != null)
			{
				mGrid.repositionNow = true;
			}
		}
		else
		{
			NGUITools.Destroy(base.gameObject);
		}
		OnDragDropEnd();
	}

	protected virtual void OnDragDropEnd()
	{
		draggedItems.Remove(this);
	}

	protected void EnableDragScrollView()
	{
		if (mDragScrollView != null)
		{
			mDragScrollView.enabled = true;
		}
	}

	protected void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			StopDragging();
		}
	}
}
