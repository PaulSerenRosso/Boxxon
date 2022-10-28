using System;
using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace MeshGenerator {

	[StructLayout(LayoutKind.Sequential)]
	public struct TriangleID {

		public ushort A, B, C;

	public	TriangleID(int3 ids)
	{
		A = (ushort) ids.x;
		B = (ushort) ids.y;
		C = (ushort) ids.z;

	}
	
	public	TriangleID(int[] ids)
	{
		if(ids.Length != 3)
			throw new Exception("Vertices count must be equal to 3");
		A = (ushort) ids[0];
		B = (ushort) ids[1];
		C = (ushort) ids[2];

	}
		public static implicit operator TriangleID (int3 _t) => new TriangleID {
			A = (ushort)_t.x,
			B = (ushort)_t.y,
			C = (ushort)_t.z
		};
	}
}