using UnityEngine;
using UnityEngine.Rendering;

namespace XftWeapon
{
	public class VertexPool
	{
		public class VertexSegment
		{
			public int VertStart;

			public int IndexStart;

			public int VertCount;

			public int IndexCount;

			public VertexPool Pool;

			public VertexSegment(int start, int count, int istart, int icount, VertexPool pool)
			{
				VertStart = start;
				VertCount = count;
				IndexCount = icount;
				IndexStart = istart;
				Pool = pool;
			}

			public void ClearIndices()
			{
				for (int i = IndexStart; i < IndexStart + IndexCount; i++)
				{
					Pool.Indices[i] = 0;
				}
				Pool.IndiceChanged = true;
			}
		}

		public Vector3[] Vertices;

		public int[] Indices;

		public Vector2[] UVs;

		public Color[] Colors;

		public bool IndiceChanged;

		public bool ColorChanged;

		public bool UVChanged;

		public bool VertChanged;

		public bool UV2Changed;

		protected int VertexTotal;

		protected int VertexUsed;

		protected int IndexTotal;

		protected int IndexUsed;

		public bool FirstUpdate = true;

		protected bool VertCountChanged;

		public const int BlockSize = 108;

		public float BoundsScheduleTime = 1f;

		public float ElapsedTime;

		protected XWeaponTrail _owner;

		protected MeshFilter _meshFilter;

		protected Mesh _mesh2d;

		protected Material _material;

		public Mesh MyMesh
		{
			get
			{
				if (!_owner.UseWith2D)
				{
					return _mesh2d;
				}
				if (_meshFilter == null || _meshFilter.gameObject == null)
				{
					return null;
				}
				return _meshFilter.sharedMesh;
			}
		}

		public VertexPool(Material material, XWeaponTrail owner)
		{
			VertexTotal = (VertexUsed = 0);
			VertCountChanged = false;
			_owner = owner;
			if (owner.UseWith2D)
			{
				CreateMeshObj(owner, material);
			}
			else
			{
				_mesh2d = new Mesh();
			}
			_material = material;
			InitArrays();
			IndiceChanged = (ColorChanged = (UVChanged = (UV2Changed = (VertChanged = true))));
		}

		public void RecalculateBounds()
		{
			MyMesh.RecalculateBounds();
		}

		public void SetMeshObjectActive(bool flag)
		{
			if (!(_meshFilter == null))
			{
				_meshFilter.gameObject.SetActive(flag);
			}
		}

		private void CreateMeshObj(XWeaponTrail owner, Material material)
		{
			GameObject gameObject = new GameObject("_XWeaponTrailMesh:|material:" + material.name);
			gameObject.layer = owner.gameObject.layer;
			gameObject.AddComponent<MeshFilter>();
			gameObject.AddComponent<MeshRenderer>();
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			_meshFilter = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));
			MeshRenderer meshRenderer = (MeshRenderer)gameObject.GetComponent(typeof(MeshRenderer));
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.receiveShadows = false;
			meshRenderer.GetComponent<Renderer>().sharedMaterial = material;
			meshRenderer.sortingLayerName = _owner.SortingLayerName;
			meshRenderer.sortingOrder = _owner.SortingOrder;
			_meshFilter.sharedMesh = new Mesh();
		}

		public void Destroy()
		{
			if (!_owner.UseWith2D)
			{
				Object.DestroyImmediate(_mesh2d);
			}
			else if (_meshFilter != null)
			{
				Object.Destroy(_meshFilter.gameObject);
			}
		}

		public VertexSegment GetVertices(int vcount, int icount)
		{
			int num = 0;
			int num2 = 0;
			if (VertexUsed + vcount >= VertexTotal)
			{
				num = (vcount / 108 + 1) * 108;
			}
			if (IndexUsed + icount >= IndexTotal)
			{
				num2 = (icount / 108 + 1) * 108;
			}
			VertexUsed += vcount;
			IndexUsed += icount;
			if (num != 0 || num2 != 0)
			{
				EnlargeArrays(num, num2);
				VertexTotal += num;
				IndexTotal += num2;
			}
			return new VertexSegment(VertexUsed - vcount, vcount, IndexUsed - icount, icount, this);
		}

		protected void InitArrays()
		{
			Vertices = new Vector3[4];
			UVs = new Vector2[4];
			Colors = new Color[4];
			Indices = new int[6];
			VertexTotal = 4;
			IndexTotal = 6;
		}

		public void EnlargeArrays(int count, int icount)
		{
			Vector3[] vertices = Vertices;
			Vertices = new Vector3[Vertices.Length + count];
			vertices.CopyTo(Vertices, 0);
			Vector2[] uVs = UVs;
			UVs = new Vector2[UVs.Length + count];
			uVs.CopyTo(UVs, 0);
			Color[] colors = Colors;
			Colors = new Color[Colors.Length + count];
			colors.CopyTo(Colors, 0);
			int[] indices = Indices;
			Indices = new int[Indices.Length + icount];
			indices.CopyTo(Indices, 0);
			VertCountChanged = true;
			IndiceChanged = true;
			ColorChanged = true;
			UVChanged = true;
			VertChanged = true;
			UV2Changed = true;
		}

		public void LateUpdate()
		{
			if (VertCountChanged)
			{
				MyMesh.Clear();
			}
			MyMesh.vertices = Vertices;
			if (UVChanged)
			{
				MyMesh.uv = UVs;
			}
			if (ColorChanged)
			{
				MyMesh.colors = Colors;
			}
			if (IndiceChanged)
			{
				MyMesh.triangles = Indices;
			}
			ElapsedTime += Time.deltaTime;
			if (ElapsedTime > BoundsScheduleTime || FirstUpdate)
			{
				RecalculateBounds();
				ElapsedTime = 0f;
			}
			if (ElapsedTime > BoundsScheduleTime)
			{
				FirstUpdate = false;
			}
			VertCountChanged = false;
			IndiceChanged = false;
			ColorChanged = false;
			UVChanged = false;
			UV2Changed = false;
			VertChanged = false;
			if (!_owner.UseWith2D)
			{
				Graphics.DrawMesh(MyMesh, Matrix4x4.identity, _material, _owner.gameObject.layer, null, 0, null, false, false);
			}
		}
	}
}
