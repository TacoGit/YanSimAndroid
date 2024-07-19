using System.Collections.Generic;
using UnityEngine;

public static class FTransformMethods
{
	public static Transform FindChildByNameInDepth(string name, Transform transform)
	{
		if (transform.name == name)
		{
			return transform;
		}
		Transform[] componentsInChildren = transform.GetComponentsInChildren<Transform>();
		foreach (Transform transform2 in componentsInChildren)
		{
			if (transform2.name == name)
			{
				return transform2;
			}
		}
		return null;
	}

	public static List<T> FindComponentsInAllChildren<T>(Transform transformToSearchIn) where T : Component
	{
		List<T> list = new List<T>();
		Transform[] componentsInChildren = transformToSearchIn.GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			T component = transform.GetComponent<T>();
			if ((bool)(Object)component)
			{
				list.Add(component);
			}
		}
		return list;
	}

	public static T FindComponentInAllChildren<T>(Transform transformToSearchIn) where T : Component
	{
		Transform[] componentsInChildren = transformToSearchIn.GetComponentsInChildren<Transform>();
		foreach (Transform transform in componentsInChildren)
		{
			T component = transform.GetComponent<T>();
			if ((bool)(Object)component)
			{
				return component;
			}
		}
		return (T)null;
	}

	public static T FindComponentInAllParents<T>(Transform transformToSearchIn) where T : Component
	{
		Transform parent = transformToSearchIn.parent;
		for (int i = 0; i < 100; i++)
		{
			T component = parent.GetComponent<T>();
			if ((bool)(Object)component)
			{
				return component;
			}
			parent = parent.parent;
			if (parent == null)
			{
				return (T)null;
			}
		}
		return (T)null;
	}

	public static void ChangeActiveChildrenInside(Transform parentOfThem, bool active)
	{
		for (int i = 0; i < parentOfThem.childCount; i++)
		{
			parentOfThem.GetChild(i).gameObject.SetActive(active);
		}
	}

	public static void ChangeActiveThroughParentTo(Transform start, Transform end, bool active, bool changeParentsChildrenActivation = false)
	{
		start.gameObject.SetActive(active);
		Transform parent = start.parent;
		for (int i = 0; i < 100; i++)
		{
			if (parent == end)
			{
				break;
			}
			if (parent == null)
			{
				break;
			}
			if (changeParentsChildrenActivation)
			{
				ChangeActiveChildrenInside(parent, active);
			}
			parent = parent.parent;
		}
	}
}
