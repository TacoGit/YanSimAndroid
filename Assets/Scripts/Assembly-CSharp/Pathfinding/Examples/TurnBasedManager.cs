using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pathfinding.Examples
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_turn_based_manager.php")]
	public class TurnBasedManager : MonoBehaviour
	{
		public enum State
		{
			SelectUnit = 0,
			SelectTarget = 1,
			Move = 2
		}

		private TurnBasedAI selected;

		public float movementSpeed;

		public GameObject nodePrefab;

		public LayerMask layerMask;

		private List<GameObject> possibleMoves = new List<GameObject>();

		private EventSystem eventSystem;

		public State state;

		private void Awake()
		{
			eventSystem = UnityEngine.Object.FindObjectOfType<EventSystem>();
		}

		private void Update()
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (eventSystem.IsPointerOverGameObject())
			{
				return;
			}
			if (state == State.SelectTarget)
			{
				HandleButtonUnderRay(ray);
			}
			if ((state == State.SelectUnit || state == State.SelectTarget) && Input.GetKeyDown(KeyCode.Mouse0))
			{
				TurnBasedAI byRay = GetByRay<TurnBasedAI>(ray);
				if (byRay != null)
				{
					Select(byRay);
					DestroyPossibleMoves();
					GeneratePossibleMoves(selected);
					state = State.SelectTarget;
				}
			}
		}

		private void HandleButtonUnderRay(Ray ray)
		{
			Astar3DButton byRay = GetByRay<Astar3DButton>(ray);
			if (byRay != null && Input.GetKeyDown(KeyCode.Mouse0))
			{
				byRay.OnClick();
				DestroyPossibleMoves();
				state = State.Move;
				StartCoroutine(MoveToNode(selected, byRay.node));
			}
		}

		private T GetByRay<T>(Ray ray) where T : class
		{
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, layerMask))
			{
				return hitInfo.transform.GetComponentInParent<T>();
			}
			return (T)null;
		}

		private void Select(TurnBasedAI unit)
		{
			selected = unit;
		}

		private IEnumerator MoveToNode(TurnBasedAI unit, GraphNode node)
		{
			ABPath path = ABPath.Construct(unit.transform.position, (Vector3)node.position);
			path.traversalProvider = unit.traversalProvider;
			AstarPath.StartPath(path);
			yield return StartCoroutine(path.WaitForPath());
			if (path.error)
			{
				Debug.LogError("Path failed:\n" + path.errorLog);
				state = State.SelectTarget;
				GeneratePossibleMoves(selected);
			}
			else
			{
				unit.targetNode = path.path[path.path.Count - 1];
				yield return StartCoroutine(MoveAlongPath(unit, path, movementSpeed));
				unit.blocker.BlockAtCurrentPosition();
				state = State.SelectUnit;
			}
		}

		private static IEnumerator MoveAlongPath(TurnBasedAI unit, ABPath path, float speed)
		{
			if (path.error || path.vectorPath.Count == 0)
			{
				throw new ArgumentException("Cannot follow an empty path");
			}
			float distanceAlongSegment = 0f;
			for (int i = 0; i < path.vectorPath.Count - 1; i++)
			{
				Vector3 p0 = path.vectorPath[Mathf.Max(i - 1, 0)];
				Vector3 p1 = path.vectorPath[i];
				Vector3 p2 = path.vectorPath[i + 1];
				Vector3 p3 = path.vectorPath[Mathf.Min(i + 2, path.vectorPath.Count - 1)];
				float segmentLength;
				for (segmentLength = Vector3.Distance(p1, p2); distanceAlongSegment < segmentLength; distanceAlongSegment += Time.deltaTime * speed)
				{
					Vector3 interpolatedPoint = AstarSplines.CatmullRom(p0, p1, p2, p3, distanceAlongSegment / segmentLength);
					unit.transform.position = interpolatedPoint;
					yield return null;
				}
				distanceAlongSegment -= segmentLength;
			}
			unit.transform.position = path.vectorPath[path.vectorPath.Count - 1];
		}

		private void DestroyPossibleMoves()
		{
			foreach (GameObject possibleMove in possibleMoves)
			{
				UnityEngine.Object.Destroy(possibleMove);
			}
			possibleMoves.Clear();
		}

		private void GeneratePossibleMoves(TurnBasedAI unit)
		{
			ConstantPath constantPath = ConstantPath.Construct(unit.transform.position, unit.movementPoints * 1000 + 1);
			constantPath.traversalProvider = unit.traversalProvider;
			AstarPath.StartPath(constantPath);
			constantPath.BlockUntilCalculated();
			foreach (GraphNode allNode in constantPath.allNodes)
			{
				if (allNode != constantPath.startNode)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(nodePrefab, (Vector3)allNode.position, Quaternion.identity);
					possibleMoves.Add(gameObject);
					gameObject.GetComponent<Astar3DButton>().node = allNode;
				}
			}
		}
	}
}
