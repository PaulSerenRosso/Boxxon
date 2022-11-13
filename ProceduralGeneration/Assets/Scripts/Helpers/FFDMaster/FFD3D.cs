using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FFDHelper
{
	public class FFD3D  {
		// mesh and control points
		public Mesh mesh;
		public List<Vector3> meshCoordinates = new List<Vector3>();
		public Vector3[] controlPointsVec;
	
		// max and min point of mesh
		public Vector3 p0;
		public Vector3 pN;
	
		// width, depth height and number of CPS in each
		public float meshBoundSizeX,meshBoundSizeY,meshBoundSizeZ;
		float [] Bs;
		float [] Bt;
		float [] Bu;

		public void Launch(Mesh _mesh, Vector3[] _controlPoints,float _offset)
		{
			if (_controlPoints.Length != 8)
			{
				throw new Exception("Control point must be equal 8");
			}
			controlPointsVec = _controlPoints;
			meshCoordinates.Clear();
			InitMesh(_mesh, _offset);
			ModifyVerts();
		}
	
		void ModifyVerts () {
			Vector3 [] verts = new Vector3[mesh.vertexCount]; 
			verts = mesh.vertices;
			
			for (int i = 0; i < mesh.vertexCount; i++){
				verts[i] = ReconstructVertex(i);
			}
	
			// if the mesh is different move the mesh and update connectors
			if (mesh.vertices != verts) {
				mesh.vertices = verts;
			}
		}
	
		// Prepare the mesh of the model
		void InitMesh(Mesh _mesh, float _offset){
			mesh = _mesh;
	
			// get width, depth, height for scaling
			meshBoundSizeX = mesh.bounds.size.x;
			meshBoundSizeY = mesh.bounds.size.y;
			meshBoundSizeZ = mesh.bounds.size.z;
	
			float sizeObj = 2;
			
			// get min and max points of the model (estimation)
			p0 = - new Vector3 (meshBoundSizeX/sizeObj, meshBoundSizeY/sizeObj, meshBoundSizeZ/sizeObj) + new Vector3(0,_offset,0);
			pN = new Vector3 (meshBoundSizeX/sizeObj, meshBoundSizeY/sizeObj, meshBoundSizeZ/sizeObj) + new Vector3(0,_offset,0);
	
			// for every vertex save its s,t,u as a ratio across the lattice space
			for (int i = 0; i < mesh.vertexCount; i++){
				float s = ((mesh.vertices[i].x - p0.x) / (pN.x - p0.x));
				float t = ((mesh.vertices[i].y - p0.y) / (pN.y - p0.y));
				float u = ((mesh.vertices[i].z - p0.z) / (pN.z - p0.z));
				//Debug.Log(mesh);
				//Debug.Log( "Vertices Length " + mesh.vertices.Length);
				//Debug.Log(i);
				//Debug.Log("Vertices Count " +mesh.vertexCount);
				meshCoordinates.Add(new Vector3(s,t,u));
			}
		}
		
		//en.wikipedia.org/wiki/Bernstein_polynomial
		// for the given index compute the bernstein coefficients
		void CalculateBernsteinCoefficients(int index){
			float s = meshCoordinates[index].x;
			float t = meshCoordinates[index].y;
			float u = meshCoordinates[index].z;
			
			Bs = new float[2];
			Bt = new float[2];
			Bu = new float[2];

			//2X2X2
			
			Bs[0] = (1.0f - s);
			Bs[1] = s;
	
			Bt[0] =  (1.0f - t);
			Bt[1] = t;
			
			Bu[0] = (1.0f - u);
			Bu[1] = u;
		}
	
		// sum up the control points position * bernstein coefficients. return the new vertex position.
		Vector3 ReconstructVertex(int index){
			CalculateBernsteinCoefficients (index);
			Vector3 point = new Vector3 (0,0,0);
			for (int i = 0; i < 2; i++) {
				for (int j = 0; j < 2; j++) {
					for (int k = 0; k < 2; k++) {
						point += controlPointsVec[k+(j*2)+ (i*2*2)] * (Bs[i]*Bt[j]*Bu[k]);
					}
				}
			}
			return point;
		}
	}
}