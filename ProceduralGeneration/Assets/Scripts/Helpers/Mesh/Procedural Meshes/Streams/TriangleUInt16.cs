using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace ProceduralMeshes.Streams {

	[StructLayout(LayoutKind.Sequential)]
	public struct TriangleUInt16 {

		public ushort A, B, C;

		public static implicit operator TriangleUInt16 (int3 _t) => new TriangleUInt16 {
			A = (ushort)_t.x,
			B = (ushort)_t.y,
			C = (ushort)_t.z
		};
	}
}