using System.Reflection;
using UnityEngine;

public class ScriptSetter : MonoBehaviour
{
	public StudentScript OldStudent;

	public StudentScript NewStudent;

	private void Start()
	{
		Component[] components = GetComponents(typeof(Component));
		Component[] array = components;
		foreach (Component component in array)
		{
			Debug.Log(string.Concat("name ", component.name, " type ", component.GetType(), " basetype ", component.GetType().BaseType));
			FieldInfo[] fields = component.GetType().GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				object obj = component;
				Debug.Log(fieldInfo.Name + " value is: " + fieldInfo.GetValue(obj));
			}
		}
	}
}
