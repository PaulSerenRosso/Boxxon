using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace FFDHelper
{
	public class FFD3DTestVec : MonoBehaviour
	{
		// mesh and control points
		public Mesh mesh;
		public float offsetPoint;
		public List<Vector3> meshCoordinates;
		public List<GameObject> connectors;
		public List<Vector3> controlPointsVec;

		// max and min point of mesh
		[FormerlySerializedAs("p0")] public Vector3 pointMin;
		public Vector3 pN;

		// width, depth height and number of CPS in each
		public float S, T, U;
		int CPs_s = 2;
		int CPs_t = 2;
		int CPs_u = 2;
		float[] boundBoxPosX;
		float[] boundsBoxPosY;
		float[] boundsBoxPosZ;

		// prefabs for building visual representation of control polygon
		public GameObject CPNode;

		[Header("Gizmos")] public Color gizmosColorBounds;

		void Start()
		{
			InitMesh();
			InitVecPos();
		}

		void Update()
		{
			Vector3[] verts = new Vector3[mesh.vertexCount];
			verts = mesh.vertices;

			for (int i = 0; i < mesh.vertexCount; i++)
			{
				verts[i] = ReconstructVertex(i);
			}

			// if the mesh is different move the mesh and update connectors
			if (mesh.vertices != verts)
			{
				mesh.vertices = verts;
			}
		}

		// Prepare the mesh of the model
		void InitMesh()
		{
			mesh = GetComponent<MeshFilter>().mesh;

			// get width, depth, height for scaling
			S = mesh.bounds.size.x;
			T = mesh.bounds.size.y;
			U = mesh.bounds.size.z;

			float sizeObj = 2;

			// get min and max points of the model (estimation)
			pointMin = -new Vector3(S / sizeObj, T / sizeObj, U / sizeObj) + new Vector3(0, offsetPoint, 0);
			pN = new Vector3(S / sizeObj, T / sizeObj, U / sizeObj) + new Vector3(0, offsetPoint, 0);

			// for every vertex save its s,t,u as a ratio across the lattice space
			for (int i = 0; i < mesh.vertexCount; i++)
			{
				float s = ((mesh.vertices[i].x - pointMin.x) / (pN.x - pointMin.x));
				float t = ((mesh.vertices[i].y - pointMin.y) / (pN.y - pointMin.y));
				float u = ((mesh.vertices[i].z - pointMin.z) / (pN.z - pointMin.z));
				meshCoordinates.Add(new Vector3(s, t, u));
			}
		}

		// Place control points around the object
		void InitVecPos()
		{
			int i = 0;
			float x, y, z;

			// place n control points across the object at appropriate intervals
			for (x = 0.0f; x < 1.0f; x += 1.0f / CPs_s)
			{
				for (y = 0.0f; y < 1.0f; y += 1.0f / CPs_t)
				{
					for (z = 0.0f; z < 1.0f; z += 1.0f / CPs_u, i++)
					{
						Vector3 controlPointsRef = pointMin + new Vector3(x * S, y * T, z * U) * 2;
						controlPointsVec.Add(controlPointsRef);
					}
				}
			}
		}



		//en.wikipedia.org/wiki/Bernstein_polynomial
		// for the given index compute the bernstein coefficients
		void CalculateBernsteinCoefficients(int index)
		{
			float meshCoordinatesX = meshCoordinates[index].x;
			float meshCoordinatesY = meshCoordinates[index].y;
			float meshCoordinatesZ = meshCoordinates[index].z;

			boundBoxPosX = new float[2];
			boundsBoxPosY = new float[2];
			boundsBoxPosZ = new float[2];

			//FFD 2X2X2	
			boundBoxPosX[0] = (1.0f - meshCoordinatesX);
			boundBoxPosX[1] = meshCoordinatesX;

			boundsBoxPosY[0] = (1.0f - meshCoordinatesY);
			boundsBoxPosY[1] = meshCoordinatesY;

			boundsBoxPosZ[0] = (1.0f - meshCoordinatesZ);
			boundsBoxPosZ[1] = meshCoordinatesZ;
		}

		// sum up the control points position * bernstein coefficients. return the new vertex position.
		Vector3 ReconstructVertex(int index)
		{
			CalculateBernsteinCoefficients(index);
			Vector3 point = new Vector3(0, 0, 0);
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					for (int k = 0; k < 2; k++)
					{
						point += controlPointsVec[k + (j * 2) + (i * 2 * 2)] * (boundBoxPosX[i] * boundsBoxPosY[j] * boundsBoxPosZ[k]);
					}
				}
			}
			return point;
		}

		void OnDrawGizmos()
		{
			if (Application.isPlaying)
			{
				Gizmos.color = gizmosColorBounds;
				Gizmos.DrawWireCube(mesh.bounds.center + transform.position, mesh.bounds.size);
			}
		}
	}
}