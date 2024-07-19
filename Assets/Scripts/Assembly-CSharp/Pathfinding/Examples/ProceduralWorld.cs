using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Examples
{
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_procedural_world.php")]
	public class ProceduralWorld : MonoBehaviour
	{
		[Serializable]
		public class ProceduralPrefab
		{
			public GameObject prefab;

			public float density;

			public float perlin;

			public float perlinPower = 1f;

			public Vector2 perlinOffset = Vector2.zero;

			public float perlinScale = 1f;

			public float random = 1f;

			public bool singleFixed;
		}

		private class ProceduralTile
		{
			private int x;

			private int z;

			private System.Random rnd;

			private ProceduralWorld world;

			private Transform root;

			private IEnumerator ie;

			public bool destroyed { get; private set; }

			public ProceduralTile(ProceduralWorld world, int x, int z)
			{
				this.x = x;
				this.z = z;
				this.world = world;
				rnd = new System.Random((x * 10007) ^ (z * 36007));
			}

			public IEnumerator Generate()
			{
				ie = InternalGenerate();
				GameObject rt = new GameObject("Tile " + x + " " + z);
				root = rt.transform;
				while (ie != null && root != null && ie.MoveNext())
				{
					yield return ie.Current;
				}
				ie = null;
			}

			public void ForceFinish()
			{
				while (ie != null && root != null && ie.MoveNext())
				{
				}
				ie = null;
			}

			private Vector3 RandomInside()
			{
				Vector3 result = default(Vector3);
				result.x = ((float)x + (float)rnd.NextDouble()) * world.tileSize;
				result.z = ((float)z + (float)rnd.NextDouble()) * world.tileSize;
				return result;
			}

			private Vector3 RandomInside(float px, float pz)
			{
				Vector3 result = default(Vector3);
				result.x = (px + (float)rnd.NextDouble() / (float)world.subTiles) * world.tileSize;
				result.z = (pz + (float)rnd.NextDouble() / (float)world.subTiles) * world.tileSize;
				return result;
			}

			private Quaternion RandomYRot()
			{
				return Quaternion.Euler(360f * (float)rnd.NextDouble(), 0f, 360f * (float)rnd.NextDouble());
			}

			private IEnumerator InternalGenerate()
			{
				Debug.Log("Generating tile " + x + ", " + z);
				int counter = 0;
				float[,] ditherMap = new float[world.subTiles + 2, world.subTiles + 2];
				for (int i = 0; i < world.prefabs.Length; i++)
				{
					ProceduralPrefab pref = world.prefabs[i];
					if (pref.singleFixed)
					{
						GameObject gameObject = UnityEngine.Object.Instantiate(position: new Vector3(((float)x + 0.5f) * world.tileSize, 0f, ((float)z + 0.5f) * world.tileSize), original: pref.prefab, rotation: Quaternion.identity);
						gameObject.transform.parent = root;
						continue;
					}
					float subSize = world.tileSize / (float)world.subTiles;
					for (int k = 0; k < world.subTiles; k++)
					{
						for (int l = 0; l < world.subTiles; l++)
						{
							ditherMap[k + 1, l + 1] = 0f;
						}
					}
					for (int sx = 0; sx < world.subTiles; sx++)
					{
						for (int sz = 0; sz < world.subTiles; sz++)
						{
							float px = (float)x + (float)sx / (float)world.subTiles;
							float pz = (float)z + (float)sz / (float)world.subTiles;
							float perl = Mathf.Pow(Mathf.PerlinNoise((px + pref.perlinOffset.x) * pref.perlinScale, (pz + pref.perlinOffset.y) * pref.perlinScale), pref.perlinPower);
							float density = pref.density * Mathf.Lerp(1f, perl, pref.perlin) * Mathf.Lerp(1f, (float)rnd.NextDouble(), pref.random);
							float fcount = subSize * subSize * density + ditherMap[sx + 1, sz + 1];
							int count = Mathf.RoundToInt(fcount);
							ditherMap[sx + 1 + 1, sz + 1] += 0.4375f * (fcount - (float)count);
							ditherMap[sx + 1 - 1, sz + 1 + 1] += 0.1875f * (fcount - (float)count);
							ditherMap[sx + 1, sz + 1 + 1] += 0.3125f * (fcount - (float)count);
							ditherMap[sx + 1 + 1, sz + 1 + 1] += 0.0625f * (fcount - (float)count);
							for (int j = 0; j < count; j++)
							{
								GameObject ob = UnityEngine.Object.Instantiate(position: RandomInside(px, pz), original: pref.prefab, rotation: RandomYRot());
								ob.transform.parent = root;
								counter++;
								if (counter % 2 == 0)
								{
									yield return null;
								}
							}
						}
					}
				}
				ditherMap = null;
				yield return null;
				yield return null;
				if (Application.HasProLicense() && world.staticBatching)
				{
					StaticBatchingUtility.Combine(root.gameObject);
				}
			}

			public void Destroy()
			{
				if (root != null)
				{
					Debug.Log("Destroying tile " + x + ", " + z);
					UnityEngine.Object.Destroy(root.gameObject);
					root = null;
				}
				ie = null;
			}
		}

		public Transform target;

		public ProceduralPrefab[] prefabs;

		public int range = 1;

		public float tileSize = 100f;

		public int subTiles = 20;

		public bool staticBatching;

		private Queue<IEnumerator> tileGenerationQueue = new Queue<IEnumerator>();

		private Dictionary<Int2, ProceduralTile> tiles = new Dictionary<Int2, ProceduralTile>();

		private void Start()
		{
			Update();
			AstarPath.active.Scan();
			StartCoroutine(GenerateTiles());
		}

		private void Update()
		{
			Int2 @int = new Int2(Mathf.RoundToInt((target.position.x - tileSize * 0.5f) / tileSize), Mathf.RoundToInt((target.position.z - tileSize * 0.5f) / tileSize));
			range = ((range < 1) ? 1 : range);
			bool flag = true;
			while (flag)
			{
				flag = false;
				foreach (KeyValuePair<Int2, ProceduralTile> tile in tiles)
				{
					if (Mathf.Abs(tile.Key.x - @int.x) > range || Mathf.Abs(tile.Key.y - @int.y) > range)
					{
						tile.Value.Destroy();
						tiles.Remove(tile.Key);
						flag = true;
						break;
					}
				}
			}
			for (int i = @int.x - range; i <= @int.x + range; i++)
			{
				for (int j = @int.y - range; j <= @int.y + range; j++)
				{
					if (!tiles.ContainsKey(new Int2(i, j)))
					{
						ProceduralTile proceduralTile = new ProceduralTile(this, i, j);
						IEnumerator enumerator2 = proceduralTile.Generate();
						enumerator2.MoveNext();
						tileGenerationQueue.Enqueue(enumerator2);
						tiles.Add(new Int2(i, j), proceduralTile);
					}
				}
			}
			for (int k = @int.x - 1; k <= @int.x + 1; k++)
			{
				for (int l = @int.y - 1; l <= @int.y + 1; l++)
				{
					tiles[new Int2(k, l)].ForceFinish();
				}
			}
		}

		private IEnumerator GenerateTiles()
		{
			while (true)
			{
				if (tileGenerationQueue.Count > 0)
				{
					IEnumerator generator = tileGenerationQueue.Dequeue();
					yield return StartCoroutine(generator);
				}
				yield return null;
			}
		}
	}
}
