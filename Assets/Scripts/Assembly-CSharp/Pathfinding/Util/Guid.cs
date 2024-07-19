using System;
using System.Text;

namespace Pathfinding.Util
{
	public struct Guid
	{
		private const string hex = "0123456789ABCDEF";

		public static readonly Guid zero = new Guid(new byte[16]);

		public static readonly string zeroString = new Guid(new byte[16]).ToString();

		private readonly ulong _a;

		private readonly ulong _b;

		private static Random random = new Random();

		private static StringBuilder text;

		public Guid(byte[] bytes)
		{
			ulong num = ((ulong)bytes[0] << 0) | ((ulong)bytes[1] << 8) | ((ulong)bytes[2] << 16) | ((ulong)bytes[3] << 24) | ((ulong)bytes[4] << 32) | ((ulong)bytes[5] << 40) | ((ulong)bytes[6] << 48) | ((ulong)bytes[7] << 56);
			ulong num2 = ((ulong)bytes[8] << 0) | ((ulong)bytes[9] << 8) | ((ulong)bytes[10] << 16) | ((ulong)bytes[11] << 24) | ((ulong)bytes[12] << 32) | ((ulong)bytes[13] << 40) | ((ulong)bytes[14] << 48) | ((ulong)bytes[15] << 56);
			_a = ((!BitConverter.IsLittleEndian) ? SwapEndianness(num) : num);
			_b = ((!BitConverter.IsLittleEndian) ? SwapEndianness(num2) : num2);
		}

		public Guid(string str)
		{
			_a = 0uL;
			_b = 0uL;
			if (str.Length < 32)
			{
				throw new FormatException("Invalid Guid format");
			}
			int num = 0;
			int num2 = 0;
			int num3 = 60;
			while (num < 16)
			{
				if (num2 >= str.Length)
				{
					throw new FormatException("Invalid Guid format. String too short");
				}
				char c = str[num2];
				if (c != '-')
				{
					int num4 = "0123456789ABCDEF".IndexOf(char.ToUpperInvariant(c));
					if (num4 == -1)
					{
						throw new FormatException("Invalid Guid format : " + c + " is not a hexadecimal character");
					}
					_a |= (ulong)((long)num4 << num3);
					num3 -= 4;
					num++;
				}
				num2++;
			}
			num3 = 60;
			while (num < 32)
			{
				if (num2 >= str.Length)
				{
					throw new FormatException("Invalid Guid format. String too short");
				}
				char c2 = str[num2];
				if (c2 != '-')
				{
					int num5 = "0123456789ABCDEF".IndexOf(char.ToUpperInvariant(c2));
					if (num5 == -1)
					{
						throw new FormatException("Invalid Guid format : " + c2 + " is not a hexadecimal character");
					}
					_b |= (ulong)((long)num5 << num3);
					num3 -= 4;
					num++;
				}
				num2++;
			}
		}

		public static Guid Parse(string input)
		{
			return new Guid(input);
		}

		private static ulong SwapEndianness(ulong value)
		{
			ulong num = (value >> 0) & 0xFF;
			ulong num2 = (value >> 8) & 0xFF;
			ulong num3 = (value >> 16) & 0xFF;
			ulong num4 = (value >> 24) & 0xFF;
			ulong num5 = (value >> 32) & 0xFF;
			ulong num6 = (value >> 40) & 0xFF;
			ulong num7 = (value >> 48) & 0xFF;
			ulong num8 = (value >> 56) & 0xFF;
			return (num << 56) | (num2 << 48) | (num3 << 40) | (num4 << 32) | (num5 << 24) | (num6 << 16) | (num7 << 8) | (num8 << 0);
		}

		public byte[] ToByteArray()
		{
			byte[] array = new byte[16];
			byte[] bytes = BitConverter.GetBytes(BitConverter.IsLittleEndian ? _a : SwapEndianness(_a));
			byte[] bytes2 = BitConverter.GetBytes(BitConverter.IsLittleEndian ? _b : SwapEndianness(_b));
			for (int i = 0; i < 8; i++)
			{
				array[i] = bytes[i];
				array[i + 8] = bytes2[i];
			}
			return array;
		}

		public static Guid NewGuid()
		{
			byte[] array = new byte[16];
			random.NextBytes(array);
			return new Guid(array);
		}

		public static bool operator ==(Guid lhs, Guid rhs)
		{
			return lhs._a == rhs._a && lhs._b == rhs._b;
		}

		public static bool operator !=(Guid lhs, Guid rhs)
		{
			return lhs._a != rhs._a || lhs._b != rhs._b;
		}

		public override bool Equals(object _rhs)
		{
			if (!(_rhs is Guid))
			{
				return false;
			}
			Guid guid = (Guid)_rhs;
			return _a == guid._a && _b == guid._b;
		}

		public override int GetHashCode()
		{
			ulong num = _a ^ _b;
			return (int)(num >> 32) ^ (int)num;
		}

		public override string ToString()
		{
			if (text == null)
			{
				text = new StringBuilder();
			}
			lock (text)
			{
				text.Length = 0;
				text.Append(_a.ToString("x16")).Append('-').Append(_b.ToString("x16"));
				return text.ToString();
			}
		}
	}
}
