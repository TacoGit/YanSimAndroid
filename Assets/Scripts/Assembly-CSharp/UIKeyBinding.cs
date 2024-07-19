using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Key Binding")]
public class UIKeyBinding : MonoBehaviour
{
	public enum Action
	{
		PressAndClick = 0,
		Select = 1,
		All = 2
	}

	public enum Modifier
	{
		Any = 0,
		Shift = 1,
		Control = 2,
		Alt = 3,
		None = 4
	}

	private static List<UIKeyBinding> mList = new List<UIKeyBinding>();

	public KeyCode keyCode;

	public Modifier modifier;

	public Action action;

	[NonSerialized]
	private bool mIgnoreUp;

	[NonSerialized]
	private bool mIsInput;

	[NonSerialized]
	private bool mPress;

	public string captionText
	{
		get
		{
			string text = NGUITools.KeyToCaption(keyCode);
			if (modifier == Modifier.Alt)
			{
				return "Alt+" + text;
			}
			if (modifier == Modifier.Control)
			{
				return "Control+" + text;
			}
			if (modifier == Modifier.Shift)
			{
				return "Shift+" + text;
			}
			return text;
		}
	}

	public static bool IsBound(KeyCode key)
	{
		int i = 0;
		for (int count = mList.Count; i < count; i++)
		{
			UIKeyBinding uIKeyBinding = mList[i];
			if (uIKeyBinding != null && uIKeyBinding.keyCode == key)
			{
				return true;
			}
		}
		return false;
	}

	protected virtual void OnEnable()
	{
		mList.Add(this);
	}

	protected virtual void OnDisable()
	{
		mList.Remove(this);
	}

	protected virtual void Start()
	{
		UIInput component = GetComponent<UIInput>();
		mIsInput = component != null;
		if (component != null)
		{
			EventDelegate.Add(component.onSubmit, OnSubmit);
		}
	}

	protected virtual void OnSubmit()
	{
		if (UICamera.currentKey == keyCode && IsModifierActive())
		{
			mIgnoreUp = true;
		}
	}

	protected virtual bool IsModifierActive()
	{
		return IsModifierActive(modifier);
	}

	public static bool IsModifierActive(Modifier modifier)
	{
		switch (modifier)
		{
		case Modifier.Any:
			return true;
		case Modifier.Alt:
			if (UICamera.GetKey(KeyCode.LeftAlt) || UICamera.GetKey(KeyCode.RightAlt))
			{
				return true;
			}
			break;
		case Modifier.Control:
			if (UICamera.GetKey(KeyCode.LeftControl) || UICamera.GetKey(KeyCode.RightControl))
			{
				return true;
			}
			break;
		case Modifier.Shift:
			if (UICamera.GetKey(KeyCode.LeftShift) || UICamera.GetKey(KeyCode.RightShift))
			{
				return true;
			}
			break;
		case Modifier.None:
			return !UICamera.GetKey(KeyCode.LeftAlt) && !UICamera.GetKey(KeyCode.RightAlt) && !UICamera.GetKey(KeyCode.LeftControl) && !UICamera.GetKey(KeyCode.RightControl) && !UICamera.GetKey(KeyCode.LeftShift) && !UICamera.GetKey(KeyCode.RightShift);
		}
		return false;
	}

	protected virtual void Update()
	{
		if (UICamera.inputHasFocus || keyCode == KeyCode.None || !IsModifierActive())
		{
			return;
		}
		bool flag = UICamera.GetKeyDown(keyCode);
		bool flag2 = UICamera.GetKeyUp(keyCode);
		if (flag)
		{
			mPress = true;
		}
		if (action == Action.PressAndClick || action == Action.All)
		{
			if (flag)
			{
				UICamera.currentTouchID = -1;
				UICamera.currentKey = keyCode;
				OnBindingPress(true);
			}
			if (mPress && flag2)
			{
				UICamera.currentTouchID = -1;
				UICamera.currentKey = keyCode;
				OnBindingPress(false);
				OnBindingClick();
			}
		}
		if ((action == Action.Select || action == Action.All) && flag2)
		{
			if (mIsInput)
			{
				if (!mIgnoreUp && !UICamera.inputHasFocus && mPress)
				{
					UICamera.selectedObject = base.gameObject;
				}
				mIgnoreUp = false;
			}
			else if (mPress)
			{
				UICamera.hoveredObject = base.gameObject;
			}
		}
		if (flag2)
		{
			mPress = false;
		}
	}

	protected virtual void OnBindingPress(bool pressed)
	{
		UICamera.Notify(base.gameObject, "OnPress", pressed);
	}

	protected virtual void OnBindingClick()
	{
		UICamera.Notify(base.gameObject, "OnClick", null);
	}

	public override string ToString()
	{
		return GetString(keyCode, modifier);
	}

	public static string GetString(KeyCode keyCode, Modifier modifier)
	{
		return (modifier == Modifier.None) ? keyCode.ToString() : string.Concat(modifier, "+", keyCode);
	}

	public static bool GetKeyCode(string text, out KeyCode key, out Modifier modifier)
	{
		key = KeyCode.None;
		modifier = Modifier.None;
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		if (text.Contains("+"))
		{
			string[] array = text.Split('+');
			try
			{
				modifier = (Modifier)Enum.Parse(typeof(Modifier), array[0]);
				key = (KeyCode)Enum.Parse(typeof(KeyCode), array[1]);
			}
			catch (Exception)
			{
				return false;
			}
		}
		else
		{
			modifier = Modifier.None;
			try
			{
				key = (KeyCode)Enum.Parse(typeof(KeyCode), text);
			}
			catch (Exception)
			{
				return false;
			}
		}
		return true;
	}

	public static Modifier GetActiveModifier()
	{
		Modifier result = Modifier.None;
		if (UICamera.GetKey(KeyCode.LeftAlt) || UICamera.GetKey(KeyCode.RightAlt))
		{
			result = Modifier.Alt;
		}
		else if (UICamera.GetKey(KeyCode.LeftShift) || UICamera.GetKey(KeyCode.RightShift))
		{
			result = Modifier.Shift;
		}
		else if (UICamera.GetKey(KeyCode.LeftControl) || UICamera.GetKey(KeyCode.RightControl))
		{
			result = Modifier.Control;
		}
		return result;
	}
}
