using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	[Serializable]
	[AddComponentMenu("Pathfinding/Modifiers/Advanced Smooth")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_advanced_smooth.php")]
	public class AdvancedSmooth : MonoModifier
	{
		[Serializable]
		public class MaxTurn : TurnConstructor
		{
			private Vector3 preRightCircleCenter = Vector3.zero;

			private Vector3 preLeftCircleCenter = Vector3.zero;

			private Vector3 rightCircleCenter;

			private Vector3 leftCircleCenter;

			private double vaRight;

			private double vaLeft;

			private double preVaLeft;

			private double preVaRight;

			private double gammaLeft;

			private double gammaRight;

			private double betaRightRight;

			private double betaRightLeft;

			private double betaLeftRight;

			private double betaLeftLeft;

			private double deltaRightLeft;

			private double deltaLeftRight;

			private double alfaRightRight;

			private double alfaLeftLeft;

			private double alfaRightLeft;

			private double alfaLeftRight;

			public override void OnTangentUpdate()
			{
				rightCircleCenter = TurnConstructor.current + TurnConstructor.normal * TurnConstructor.turningRadius;
				leftCircleCenter = TurnConstructor.current - TurnConstructor.normal * TurnConstructor.turningRadius;
				vaRight = Atan2(TurnConstructor.current - rightCircleCenter);
				vaLeft = vaRight + Math.PI;
			}

			public override void Prepare(int i, Vector3[] vectorPath)
			{
				preRightCircleCenter = rightCircleCenter;
				preLeftCircleCenter = leftCircleCenter;
				rightCircleCenter = TurnConstructor.current + TurnConstructor.normal * TurnConstructor.turningRadius;
				leftCircleCenter = TurnConstructor.current - TurnConstructor.normal * TurnConstructor.turningRadius;
				preVaRight = vaRight;
				preVaLeft = vaLeft;
				vaRight = Atan2(TurnConstructor.current - rightCircleCenter);
				vaLeft = vaRight + Math.PI;
			}

			public override void TangentToTangent(List<Turn> turnList)
			{
				alfaRightRight = Atan2(rightCircleCenter - preRightCircleCenter);
				alfaLeftLeft = Atan2(leftCircleCenter - preLeftCircleCenter);
				alfaRightLeft = Atan2(leftCircleCenter - preRightCircleCenter);
				alfaLeftRight = Atan2(rightCircleCenter - preLeftCircleCenter);
				double num = (leftCircleCenter - preRightCircleCenter).magnitude;
				double num2 = (rightCircleCenter - preLeftCircleCenter).magnitude;
				bool flag = false;
				bool flag2 = false;
				if (num < (double)(TurnConstructor.turningRadius * 2f))
				{
					num = TurnConstructor.turningRadius * 2f;
					flag = true;
				}
				if (num2 < (double)(TurnConstructor.turningRadius * 2f))
				{
					num2 = TurnConstructor.turningRadius * 2f;
					flag2 = true;
				}
				deltaRightLeft = ((!flag) ? (Math.PI / 2.0 - Math.Asin((double)(TurnConstructor.turningRadius * 2f) / num)) : 0.0);
				deltaLeftRight = ((!flag2) ? (Math.PI / 2.0 - Math.Asin((double)(TurnConstructor.turningRadius * 2f) / num2)) : 0.0);
				betaRightRight = ClockwiseAngle(preVaRight, alfaRightRight - Math.PI / 2.0);
				betaRightLeft = ClockwiseAngle(preVaRight, alfaRightLeft - deltaRightLeft);
				betaLeftRight = CounterClockwiseAngle(preVaLeft, alfaLeftRight + deltaLeftRight);
				betaLeftLeft = CounterClockwiseAngle(preVaLeft, alfaLeftLeft + Math.PI / 2.0);
				betaRightRight += ClockwiseAngle(alfaRightRight - Math.PI / 2.0, vaRight);
				betaRightLeft += CounterClockwiseAngle(alfaRightLeft + deltaRightLeft, vaLeft);
				betaLeftRight += ClockwiseAngle(alfaLeftRight - deltaLeftRight, vaRight);
				betaLeftLeft += CounterClockwiseAngle(alfaLeftLeft + Math.PI / 2.0, vaLeft);
				betaRightRight = GetLengthFromAngle(betaRightRight, TurnConstructor.turningRadius);
				betaRightLeft = GetLengthFromAngle(betaRightLeft, TurnConstructor.turningRadius);
				betaLeftRight = GetLengthFromAngle(betaLeftRight, TurnConstructor.turningRadius);
				betaLeftLeft = GetLengthFromAngle(betaLeftLeft, TurnConstructor.turningRadius);
				Vector3 vector = AngleToVector(alfaRightRight - Math.PI / 2.0) * TurnConstructor.turningRadius + preRightCircleCenter;
				Vector3 vector2 = AngleToVector(alfaRightLeft - deltaRightLeft) * TurnConstructor.turningRadius + preRightCircleCenter;
				Vector3 vector3 = AngleToVector(alfaLeftRight + deltaLeftRight) * TurnConstructor.turningRadius + preLeftCircleCenter;
				Vector3 vector4 = AngleToVector(alfaLeftLeft + Math.PI / 2.0) * TurnConstructor.turningRadius + preLeftCircleCenter;
				Vector3 vector5 = AngleToVector(alfaRightRight - Math.PI / 2.0) * TurnConstructor.turningRadius + rightCircleCenter;
				Vector3 vector6 = AngleToVector(alfaRightLeft - deltaRightLeft + Math.PI) * TurnConstructor.turningRadius + leftCircleCenter;
				Vector3 vector7 = AngleToVector(alfaLeftRight + deltaLeftRight + Math.PI) * TurnConstructor.turningRadius + rightCircleCenter;
				Vector3 vector8 = AngleToVector(alfaLeftLeft + Math.PI / 2.0) * TurnConstructor.turningRadius + leftCircleCenter;
				betaRightRight += (vector - vector5).magnitude;
				betaRightLeft += (vector2 - vector6).magnitude;
				betaLeftRight += (vector3 - vector7).magnitude;
				betaLeftLeft += (vector4 - vector8).magnitude;
				if (flag)
				{
					betaRightLeft += 10000000.0;
				}
				if (flag2)
				{
					betaLeftRight += 10000000.0;
				}
				turnList.Add(new Turn((float)betaRightRight, this, 2));
				turnList.Add(new Turn((float)betaRightLeft, this, 3));
				turnList.Add(new Turn((float)betaLeftRight, this, 4));
				turnList.Add(new Turn((float)betaLeftLeft, this, 5));
			}

			public override void PointToTangent(List<Turn> turnList)
			{
				bool flag = false;
				bool flag2 = false;
				float magnitude = (TurnConstructor.prev - rightCircleCenter).magnitude;
				float magnitude2 = (TurnConstructor.prev - leftCircleCenter).magnitude;
				if (magnitude < TurnConstructor.turningRadius)
				{
					flag = true;
				}
				if (magnitude2 < TurnConstructor.turningRadius)
				{
					flag2 = true;
				}
				double num = ((!flag) ? Atan2(TurnConstructor.prev - rightCircleCenter) : 0.0);
				double num2 = ((!flag) ? (Math.PI / 2.0 - Math.Asin(TurnConstructor.turningRadius / (TurnConstructor.prev - rightCircleCenter).magnitude)) : 0.0);
				gammaRight = num + num2;
				double num3 = ((!flag) ? ClockwiseAngle(gammaRight, vaRight) : 0.0);
				double num4 = ((!flag2) ? Atan2(TurnConstructor.prev - leftCircleCenter) : 0.0);
				double num5 = ((!flag2) ? (Math.PI / 2.0 - Math.Asin(TurnConstructor.turningRadius / (TurnConstructor.prev - leftCircleCenter).magnitude)) : 0.0);
				gammaLeft = num4 - num5;
				double num6 = ((!flag2) ? CounterClockwiseAngle(gammaLeft, vaLeft) : 0.0);
				if (!flag)
				{
					turnList.Add(new Turn((float)num3, this));
				}
				if (!flag2)
				{
					turnList.Add(new Turn((float)num6, this, 1));
				}
			}

			public override void TangentToPoint(List<Turn> turnList)
			{
				bool flag = false;
				bool flag2 = false;
				float magnitude = (TurnConstructor.next - rightCircleCenter).magnitude;
				float magnitude2 = (TurnConstructor.next - leftCircleCenter).magnitude;
				if (magnitude < TurnConstructor.turningRadius)
				{
					flag = true;
				}
				if (magnitude2 < TurnConstructor.turningRadius)
				{
					flag2 = true;
				}
				if (!flag)
				{
					double num = Atan2(TurnConstructor.next - rightCircleCenter);
					double num2 = Math.PI / 2.0 - Math.Asin(TurnConstructor.turningRadius / magnitude);
					gammaRight = num - num2;
					double num3 = ClockwiseAngle(vaRight, gammaRight);
					turnList.Add(new Turn((float)num3, this, 6));
				}
				if (!flag2)
				{
					double num4 = Atan2(TurnConstructor.next - leftCircleCenter);
					double num5 = Math.PI / 2.0 - Math.Asin(TurnConstructor.turningRadius / magnitude2);
					gammaLeft = num4 + num5;
					double num6 = CounterClockwiseAngle(vaLeft, gammaLeft);
					turnList.Add(new Turn((float)num6, this, 7));
				}
			}

			public override void GetPath(Turn turn, List<Vector3> output)
			{
				switch (turn.id)
				{
				case 0:
					AddCircleSegment(gammaRight, vaRight, true, rightCircleCenter, output, TurnConstructor.turningRadius);
					break;
				case 1:
					AddCircleSegment(gammaLeft, vaLeft, false, leftCircleCenter, output, TurnConstructor.turningRadius);
					break;
				case 2:
					AddCircleSegment(preVaRight, alfaRightRight - Math.PI / 2.0, true, preRightCircleCenter, output, TurnConstructor.turningRadius);
					AddCircleSegment(alfaRightRight - Math.PI / 2.0, vaRight, true, rightCircleCenter, output, TurnConstructor.turningRadius);
					break;
				case 3:
					AddCircleSegment(preVaRight, alfaRightLeft - deltaRightLeft, true, preRightCircleCenter, output, TurnConstructor.turningRadius);
					AddCircleSegment(alfaRightLeft - deltaRightLeft + Math.PI, vaLeft, false, leftCircleCenter, output, TurnConstructor.turningRadius);
					break;
				case 4:
					AddCircleSegment(preVaLeft, alfaLeftRight + deltaLeftRight, false, preLeftCircleCenter, output, TurnConstructor.turningRadius);
					AddCircleSegment(alfaLeftRight + deltaLeftRight + Math.PI, vaRight, true, rightCircleCenter, output, TurnConstructor.turningRadius);
					break;
				case 5:
					AddCircleSegment(preVaLeft, alfaLeftLeft + Math.PI / 2.0, false, preLeftCircleCenter, output, TurnConstructor.turningRadius);
					AddCircleSegment(alfaLeftLeft + Math.PI / 2.0, vaLeft, false, leftCircleCenter, output, TurnConstructor.turningRadius);
					break;
				case 6:
					AddCircleSegment(vaRight, gammaRight, true, rightCircleCenter, output, TurnConstructor.turningRadius);
					break;
				case 7:
					AddCircleSegment(vaLeft, gammaLeft, false, leftCircleCenter, output, TurnConstructor.turningRadius);
					break;
				}
			}
		}

		[Serializable]
		public class ConstantTurn : TurnConstructor
		{
			private Vector3 circleCenter;

			private double gamma1;

			private double gamma2;

			private bool clockwise;

			public override void Prepare(int i, Vector3[] vectorPath)
			{
			}

			public override void TangentToTangent(List<Turn> turnList)
			{
				Vector3 dir = Vector3.Cross(TurnConstructor.t1, Vector3.up);
				Vector3 vector = TurnConstructor.current - TurnConstructor.prev;
				Vector3 start = vector * 0.5f + TurnConstructor.prev;
				vector = Vector3.Cross(vector, Vector3.up);
				bool intersects;
				circleCenter = VectorMath.LineDirIntersectionPointXZ(TurnConstructor.prev, dir, start, vector, out intersects);
				if (intersects)
				{
					gamma1 = Atan2(TurnConstructor.prev - circleCenter);
					gamma2 = Atan2(TurnConstructor.current - circleCenter);
					clockwise = !VectorMath.RightOrColinearXZ(circleCenter, TurnConstructor.prev, TurnConstructor.prev + TurnConstructor.t1);
					double angle = ((!clockwise) ? CounterClockwiseAngle(gamma1, gamma2) : ClockwiseAngle(gamma1, gamma2));
					angle = GetLengthFromAngle(angle, (circleCenter - TurnConstructor.current).magnitude);
					turnList.Add(new Turn((float)angle, this));
				}
			}

			public override void GetPath(Turn turn, List<Vector3> output)
			{
				AddCircleSegment(gamma1, gamma2, clockwise, circleCenter, output, (circleCenter - TurnConstructor.current).magnitude);
				TurnConstructor.normal = (TurnConstructor.current - circleCenter).normalized;
				TurnConstructor.t2 = Vector3.Cross(TurnConstructor.normal, Vector3.up).normalized;
				TurnConstructor.normal = -TurnConstructor.normal;
				if (!clockwise)
				{
					TurnConstructor.t2 = -TurnConstructor.t2;
					TurnConstructor.normal = -TurnConstructor.normal;
				}
				TurnConstructor.changedPreviousTangent = true;
			}
		}

		public abstract class TurnConstructor
		{
			public float constantBias;

			public float factorBias = 1f;

			public static float turningRadius = 1f;

			public const double ThreeSixtyRadians = Math.PI * 2.0;

			public static Vector3 prev;

			public static Vector3 current;

			public static Vector3 next;

			public static Vector3 t1;

			public static Vector3 t2;

			public static Vector3 normal;

			public static Vector3 prevNormal;

			public static bool changedPreviousTangent;

			public abstract void Prepare(int i, Vector3[] vectorPath);

			public virtual void OnTangentUpdate()
			{
			}

			public virtual void PointToTangent(List<Turn> turnList)
			{
			}

			public virtual void TangentToPoint(List<Turn> turnList)
			{
			}

			public virtual void TangentToTangent(List<Turn> turnList)
			{
			}

			public abstract void GetPath(Turn turn, List<Vector3> output);

			public static void Setup(int i, Vector3[] vectorPath)
			{
				current = vectorPath[i];
				prev = vectorPath[i - 1];
				next = vectorPath[i + 1];
				prev.y = current.y;
				next.y = current.y;
				t1 = t2;
				t2 = (next - current).normalized - (prev - current).normalized;
				t2 = t2.normalized;
				prevNormal = normal;
				normal = Vector3.Cross(t2, Vector3.up);
				normal = normal.normalized;
			}

			public static void PostPrepare()
			{
				changedPreviousTangent = false;
			}

			public void AddCircleSegment(double startAngle, double endAngle, bool clockwise, Vector3 center, List<Vector3> output, float radius)
			{
				double num = Math.PI / 50.0;
				if (clockwise)
				{
					while (endAngle > startAngle + Math.PI * 2.0)
					{
						endAngle -= Math.PI * 2.0;
					}
					while (endAngle < startAngle)
					{
						endAngle += Math.PI * 2.0;
					}
				}
				else
				{
					while (endAngle < startAngle - Math.PI * 2.0)
					{
						endAngle += Math.PI * 2.0;
					}
					while (endAngle > startAngle)
					{
						endAngle -= Math.PI * 2.0;
					}
				}
				if (clockwise)
				{
					for (double num2 = startAngle; num2 < endAngle; num2 += num)
					{
						output.Add(AngleToVector(num2) * radius + center);
					}
				}
				else
				{
					for (double num3 = startAngle; num3 > endAngle; num3 -= num)
					{
						output.Add(AngleToVector(num3) * radius + center);
					}
				}
				output.Add(AngleToVector(endAngle) * radius + center);
			}

			public void DebugCircleSegment(Vector3 center, double startAngle, double endAngle, double radius, Color color)
			{
				double num = Math.PI / 50.0;
				while (endAngle < startAngle)
				{
					endAngle += Math.PI * 2.0;
				}
				Vector3 start = AngleToVector(startAngle) * (float)radius + center;
				for (double num2 = startAngle + num; num2 < endAngle; num2 += num)
				{
					Debug.DrawLine(start, AngleToVector(num2) * (float)radius + center);
				}
				Debug.DrawLine(start, AngleToVector(endAngle) * (float)radius + center);
			}

			public void DebugCircle(Vector3 center, double radius, Color color)
			{
				double num = Math.PI / 50.0;
				Vector3 start = AngleToVector(0.0 - num) * (float)radius + center;
				for (double num2 = 0.0; num2 < Math.PI * 2.0; num2 += num)
				{
					Vector3 vector = AngleToVector(num2) * (float)radius + center;
					Debug.DrawLine(start, vector, color);
					start = vector;
				}
			}

			public double GetLengthFromAngle(double angle, double radius)
			{
				return radius * angle;
			}

			public double ClockwiseAngle(double from, double to)
			{
				return ClampAngle(to - from);
			}

			public double CounterClockwiseAngle(double from, double to)
			{
				return ClampAngle(from - to);
			}

			public Vector3 AngleToVector(double a)
			{
				return new Vector3((float)Math.Cos(a), 0f, (float)Math.Sin(a));
			}

			public double ToDegrees(double rad)
			{
				return rad * 57.295780181884766;
			}

			public double ClampAngle(double a)
			{
				while (a < 0.0)
				{
					a += Math.PI * 2.0;
				}
				while (a > Math.PI * 2.0)
				{
					a -= Math.PI * 2.0;
				}
				return a;
			}

			public double Atan2(Vector3 v)
			{
				return Math.Atan2(v.z, v.x);
			}
		}

		public struct Turn : IComparable<Turn>
		{
			public float length;

			public int id;

			public TurnConstructor constructor;

			public float score
			{
				get
				{
					return length * constructor.factorBias + constructor.constantBias;
				}
			}

			public Turn(float length, TurnConstructor constructor, int id = 0)
			{
				this.length = length;
				this.id = id;
				this.constructor = constructor;
			}

			public void GetPath(List<Vector3> output)
			{
				constructor.GetPath(this, output);
			}

			public int CompareTo(Turn t)
			{
				return (t.score > score) ? (-1) : ((t.score < score) ? 1 : 0);
			}

			public static bool operator <(Turn lhs, Turn rhs)
			{
				return lhs.score < rhs.score;
			}

			public static bool operator >(Turn lhs, Turn rhs)
			{
				return lhs.score > rhs.score;
			}
		}

		public float turningRadius = 1f;

		public MaxTurn turnConstruct1 = new MaxTurn();

		public ConstantTurn turnConstruct2 = new ConstantTurn();

		public override int Order
		{
			get
			{
				return 40;
			}
		}

		public override void Apply(Path p)
		{
			Vector3[] array = p.vectorPath.ToArray();
			if (array == null || array.Length <= 2)
			{
				return;
			}
			List<Vector3> list = new List<Vector3>();
			list.Add(array[0]);
			TurnConstructor.turningRadius = turningRadius;
			for (int i = 1; i < array.Length - 1; i++)
			{
				List<Turn> turnList = new List<Turn>();
				TurnConstructor.Setup(i, array);
				turnConstruct1.Prepare(i, array);
				turnConstruct2.Prepare(i, array);
				TurnConstructor.PostPrepare();
				if (i == 1)
				{
					turnConstruct1.PointToTangent(turnList);
					turnConstruct2.PointToTangent(turnList);
				}
				else
				{
					turnConstruct1.TangentToTangent(turnList);
					turnConstruct2.TangentToTangent(turnList);
				}
				EvaluatePaths(turnList, list);
				if (i == array.Length - 2)
				{
					turnConstruct1.TangentToPoint(turnList);
					turnConstruct2.TangentToPoint(turnList);
				}
				EvaluatePaths(turnList, list);
			}
			list.Add(array[array.Length - 1]);
			p.vectorPath = list;
		}

		private void EvaluatePaths(List<Turn> turnList, List<Vector3> output)
		{
			turnList.Sort();
			for (int i = 0; i < turnList.Count; i++)
			{
				if (i == 0)
				{
					turnList[i].GetPath(output);
				}
			}
			turnList.Clear();
			if (TurnConstructor.changedPreviousTangent)
			{
				turnConstruct1.OnTangentUpdate();
				turnConstruct2.OnTangentUpdate();
			}
		}
	}
}
