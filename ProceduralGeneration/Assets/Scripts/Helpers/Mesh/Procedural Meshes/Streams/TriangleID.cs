using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace MeshGenerator {

	[StructLayout(LayoutKind.Sequential)]
	public struct TriangleID {

		public ushort A, B, C;
		public static implicit operator TriangleID (int3 _t) => new TriangleID {
			A = (ushort)_t.x,
			B = (ushort)_t.y,
			C = (ushort)_t.z
		};
	}
}