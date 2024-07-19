using System.Collections.Generic;
using UnityEngine;

namespace XftWeapon
{
	public class XWeaponTrail : MonoBehaviour
	{
		public class Element
		{
			public Vector3 PointStart;

			public Vector3 PointEnd;

			public Vector3 Pos
			{
				get
				{
					return (PointStart + PointEnd) / 2f;
				}
			}

			public Element(Vector3 start, Vector3 end)
			{
				PointStart = start;
				PointEnd = end;
			}

			public Element()
			{
			}
		}

		public class ElementPool
		{
			private readonly Stack<Element> _stack = new Stack<Element>();

			public int CountAll { get; private set; }

			public int CountActive
			{
				get
				{
					return CountAll - CountInactive;
				}
			}

			public int CountInactive
			{
				get
				{
					return _stack.Count;
				}
			}

			public ElementPool(int preCount)
			{
				for (int i = 0; i < preCount; i++)
				{
					Element t = new Element();
					_stack.Push(t);
					CountAll++;
				}
			}

			public Element Get()
			{
				Element result;
				if (_stack.Count == 0)
				{
					result = new Element();
					CountAll++;
				}
				else
				{
					result = _stack.Pop();
				}
				return result;
			}

			public void Release(Element element)
			{
				if (_stack.Count > 0 && object.ReferenceEquals(_stack.Peek(), element))
				{
					Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
				}
				_stack.Push(element);
			}
		}

		public static string Version = "1.2.0";

		public bool UseWith2D;

		public string SortingLayerName;

		public int SortingOrder;

		public Transform PointStart;

		public Transform PointEnd;

		public int MaxFrame = 14;

		public int Granularity = 60;

		public Color MyColor = Color.white;

		public Material MyMaterial;

		protected float mTrailWidth;

		protected Element mHeadElem = new Element();

		protected List<Element> mSnapshotList = new List<Element>();

		protected ElementPool mElemPool;

		protected Spline mSpline = new Spline();

		protected float mFadeT = 1f;

		protected bool mIsFading;

		protected float mFadeTime = 1f;

		protected float mElapsedTime;

		protected float mFadeElapsedime;

		protected GameObject mMeshObj;

		protected VertexPool mVertexPool;

		protected VertexPool.VertexSegment mVertexSegment;

		protected bool mInited;

		public Vector3 CurHeadPos
		{
			get
			{
				return (PointStart.position + PointEnd.position) / 2f;
			}
		}

		public float TrailWidth
		{
			get
			{
				return mTrailWidth;
			}
		}

		public void Init()
		{
			if (!mInited)
			{
				mElemPool = new ElementPool(MaxFrame);
				mTrailWidth = (PointStart.position - PointEnd.position).magnitude;
				InitMeshObj();
				InitOriginalElements();
				InitSpline();
				mInited = true;
			}
		}

		public void Activate()
		{
			Init();
			base.gameObject.SetActive(true);
			mVertexPool.SetMeshObjectActive(true);
			mFadeT = 1f;
			mIsFading = false;
			mFadeTime = 1f;
			mFadeElapsedime = 0f;
			mElapsedTime = 0f;
			for (int i = 0; i < mSnapshotList.Count; i++)
			{
				mSnapshotList[i].PointStart = PointStart.position;
				mSnapshotList[i].PointEnd = PointEnd.position;
				mSpline.ControlPoints[i].Position = mSnapshotList[i].Pos;
				mSpline.ControlPoints[i].Normal = mSnapshotList[i].PointEnd - mSnapshotList[i].PointStart;
			}
			RefreshSpline();
			UpdateVertex();
		}

		public void Deactivate()
		{
			base.gameObject.SetActive(false);
			mVertexPool.SetMeshObjectActive(false);
		}

		public void StopSmoothly(float fadeTime)
		{
			mIsFading = true;
			mFadeTime = fadeTime;
		}

		private void Update()
		{
			if (mInited)
			{
				UpdateHeadElem();
				RecordCurElem();
				RefreshSpline();
				UpdateFade();
				UpdateVertex();
			}
		}

		private void LateUpdate()
		{
			if (mInited)
			{
				mVertexPool.LateUpdate();
			}
		}

		private void OnDestroy()
		{
			if (mInited && mVertexPool != null)
			{
				mVertexPool.Destroy();
			}
		}

		private void Start()
		{
			mInited = false;
			Init();
		}

		private void OnDrawGizmos()
		{
			if (!(PointEnd == null) && !(PointStart == null))
			{
				float magnitude = (PointStart.position - PointEnd.position).magnitude;
				if (!(magnitude < Mathf.Epsilon))
				{
					Gizmos.color = Color.red;
					Gizmos.DrawSphere(PointStart.position, magnitude * 0.04f);
					Gizmos.color = Color.blue;
					Gizmos.DrawSphere(PointEnd.position, magnitude * 0.04f);
				}
			}
		}

		private void InitSpline()
		{
			mSpline.Granularity = Granularity;
			mSpline.Clear();
			for (int i = 0; i < MaxFrame; i++)
			{
				mSpline.AddControlPoint(CurHeadPos, PointStart.position - PointEnd.position);
			}
		}

		private void RefreshSpline()
		{
			for (int i = 0; i < mSnapshotList.Count; i++)
			{
				mSpline.ControlPoints[i].Position = mSnapshotList[i].Pos;
				mSpline.ControlPoints[i].Normal = mSnapshotList[i].PointEnd - mSnapshotList[i].PointStart;
			}
			mSpline.RefreshSpline();
		}

		private void UpdateVertex()
		{
			VertexPool pool = mVertexSegment.Pool;
			for (int i = 0; i < Granularity; i++)
			{
				int num = mVertexSegment.VertStart + i * 3;
				float num2 = (float)i / (float)Granularity;
				float tl = num2 * mFadeT;
				Vector2 zero = Vector2.zero;
				Vector3 vector = mSpline.InterpolateByLen(tl);
				Vector3 vector2 = mSpline.InterpolateNormalByLen(tl);
				Vector3 vector3 = vector + vector2.normalized * mTrailWidth * 0.5f;
				Vector3 vector4 = vector - vector2.normalized * mTrailWidth * 0.5f;
				pool.Vertices[num] = vector3;
				pool.Colors[num] = MyColor;
				zero.x = 0f;
				zero.y = num2;
				pool.UVs[num] = zero;
				pool.Vertices[num + 1] = vector;
				pool.Colors[num + 1] = MyColor;
				zero.x = 0.5f;
				zero.y = num2;
				pool.UVs[num + 1] = zero;
				pool.Vertices[num + 2] = vector4;
				pool.Colors[num + 2] = MyColor;
				zero.x = 1f;
				zero.y = num2;
				pool.UVs[num + 2] = zero;
			}
			mVertexSegment.Pool.UVChanged = true;
			mVertexSegment.Pool.VertChanged = true;
			mVertexSegment.Pool.ColorChanged = true;
		}

		private void UpdateIndices()
		{
			VertexPool pool = mVertexSegment.Pool;
			for (int i = 0; i < Granularity - 1; i++)
			{
				int num = mVertexSegment.VertStart + i * 3;
				int num2 = mVertexSegment.VertStart + (i + 1) * 3;
				int num3 = mVertexSegment.IndexStart + i * 12;
				pool.Indices[num3] = num2;
				pool.Indices[num3 + 1] = num2 + 1;
				pool.Indices[num3 + 2] = num;
				pool.Indices[num3 + 3] = num2 + 1;
				pool.Indices[num3 + 4] = num + 1;
				pool.Indices[num3 + 5] = num;
				pool.Indices[num3 + 6] = num2 + 1;
				pool.Indices[num3 + 7] = num2 + 2;
				pool.Indices[num3 + 8] = num + 1;
				pool.Indices[num3 + 9] = num2 + 2;
				pool.Indices[num3 + 10] = num + 2;
				pool.Indices[num3 + 11] = num + 1;
			}
			pool.IndiceChanged = true;
		}

		private void UpdateHeadElem()
		{
			mSnapshotList[0].PointStart = PointStart.position;
			mSnapshotList[0].PointEnd = PointEnd.position;
		}

		private void UpdateFade()
		{
			if (mIsFading)
			{
				mFadeElapsedime += Time.deltaTime;
				float num = mFadeElapsedime / mFadeTime;
				mFadeT = 1f - num;
				if (mFadeT < 0f)
				{
					Deactivate();
				}
			}
		}

		private void RecordCurElem()
		{
			Element element = mElemPool.Get();
			element.PointStart = PointStart.position;
			element.PointEnd = PointEnd.position;
			if (mSnapshotList.Count < MaxFrame)
			{
				mSnapshotList.Insert(1, element);
				return;
			}
			mElemPool.Release(mSnapshotList[mSnapshotList.Count - 1]);
			mSnapshotList.RemoveAt(mSnapshotList.Count - 1);
			mSnapshotList.Insert(1, element);
		}

		private void InitOriginalElements()
		{
			mSnapshotList.Clear();
			mSnapshotList.Add(new Element(PointStart.position, PointEnd.position));
			mSnapshotList.Add(new Element(PointStart.position, PointEnd.position));
		}

		private void InitMeshObj()
		{
			mVertexPool = new VertexPool(MyMaterial, this);
			mVertexSegment = mVertexPool.GetVertices(Granularity * 3, (Granularity - 1) * 12);
			UpdateIndices();
		}
	}
}
