using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	public class RecastBBTree
	{
		private RecastBBTreeBox root;

		public void QueryInBounds(Rect bounds, List<RecastMeshObj> buffer)
		{
			if (root != null)
			{
				QueryBoxInBounds(root, bounds, buffer);
			}
		}

		private void QueryBoxInBounds(RecastBBTreeBox box, Rect bounds, List<RecastMeshObj> boxes)
		{
			if (box.mesh != null)
			{
				if (RectIntersectsRect(box.rect, bounds))
				{
					boxes.Add(box.mesh);
				}
				return;
			}
			if (RectIntersectsRect(box.c1.rect, bounds))
			{
				QueryBoxInBounds(box.c1, bounds, boxes);
			}
			if (RectIntersectsRect(box.c2.rect, bounds))
			{
				QueryBoxInBounds(box.c2, bounds, boxes);
			}
		}

		public bool Remove(RecastMeshObj mesh)
		{
			if (mesh == null)
			{
				throw new ArgumentNullException("mesh");
			}
			if (root == null)
			{
				return false;
			}
			bool found = false;
			Bounds bounds = mesh.GetBounds();
			Rect bounds2 = Rect.MinMaxRect(bounds.min.x, bounds.min.z, bounds.max.x, bounds.max.z);
			root = RemoveBox(root, mesh, bounds2, ref found);
			return found;
		}

		private RecastBBTreeBox RemoveBox(RecastBBTreeBox c, RecastMeshObj mesh, Rect bounds, ref bool found)
		{
			if (!RectIntersectsRect(c.rect, bounds))
			{
				return c;
			}
			if (c.mesh == mesh)
			{
				found = true;
				return null;
			}
			if (c.mesh == null && !found)
			{
				c.c1 = RemoveBox(c.c1, mesh, bounds, ref found);
				if (c.c1 == null)
				{
					return c.c2;
				}
				if (!found)
				{
					c.c2 = RemoveBox(c.c2, mesh, bounds, ref found);
					if (c.c2 == null)
					{
						return c.c1;
					}
				}
				if (found)
				{
					c.rect = ExpandToContain(c.c1.rect, c.c2.rect);
				}
			}
			return c;
		}

		public void Insert(RecastMeshObj mesh)
		{
			RecastBBTreeBox recastBBTreeBox = new RecastBBTreeBox(mesh);
			if (root == null)
			{
				root = recastBBTreeBox;
				return;
			}
			RecastBBTreeBox recastBBTreeBox2 = root;
			while (true)
			{
				recastBBTreeBox2.rect = ExpandToContain(recastBBTreeBox2.rect, recastBBTreeBox.rect);
				if (recastBBTreeBox2.mesh != null)
				{
					break;
				}
				float num = ExpansionRequired(recastBBTreeBox2.c1.rect, recastBBTreeBox.rect);
				float num2 = ExpansionRequired(recastBBTreeBox2.c2.rect, recastBBTreeBox.rect);
				recastBBTreeBox2 = ((!(num < num2)) ? ((!(num2 < num)) ? ((!(RectArea(recastBBTreeBox2.c1.rect) < RectArea(recastBBTreeBox2.c2.rect))) ? recastBBTreeBox2.c2 : recastBBTreeBox2.c1) : recastBBTreeBox2.c2) : recastBBTreeBox2.c1);
			}
			recastBBTreeBox2.c1 = recastBBTreeBox;
			RecastBBTreeBox c = new RecastBBTreeBox(recastBBTreeBox2.mesh);
			recastBBTreeBox2.c2 = c;
			recastBBTreeBox2.mesh = null;
		}

		private static bool RectIntersectsRect(Rect r, Rect r2)
		{
			return r.xMax > r2.xMin && r.yMax > r2.yMin && r2.xMax > r.xMin && r2.yMax > r.yMin;
		}

		private static float ExpansionRequired(Rect r, Rect r2)
		{
			float num = Mathf.Min(r.xMin, r2.xMin);
			float num2 = Mathf.Max(r.xMax, r2.xMax);
			float num3 = Mathf.Min(r.yMin, r2.yMin);
			float num4 = Mathf.Max(r.yMax, r2.yMax);
			return (num2 - num) * (num4 - num3) - RectArea(r);
		}

		private static Rect ExpandToContain(Rect r, Rect r2)
		{
			float xmin = Mathf.Min(r.xMin, r2.xMin);
			float xmax = Mathf.Max(r.xMax, r2.xMax);
			float ymin = Mathf.Min(r.yMin, r2.yMin);
			float ymax = Mathf.Max(r.yMax, r2.yMax);
			return Rect.MinMaxRect(xmin, ymin, xmax, ymax);
		}

		private static float RectArea(Rect r)
		{
			return r.width * r.height;
		}
	}
}
