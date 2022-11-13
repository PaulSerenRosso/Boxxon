using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FFD3D : MonoBehaviour {
	// mesh and control points
	public Mesh mesh;
	public bool useControlPoints;
	public float offsetPoint;
	public List<Vector3> meshCoordinates;
	public List<GameObject> connectors;
	public List<GameObject> controlPointsGO;
	public List<Vector3> controlPointsVec;

	// max and min point of mesh
	public Vector3 p0;
	public Vector3 pN;

	// width, depth height and number of CPS in each
	public float S,T,U;
	int CPs_s = 2;
	int CPs_t = 2;
	int CPs_u = 2;
	float [] Bs;
	float [] Bt;
	float [] Bu;

	// prefabs for building visual representation of control polygon
	public GameObject CPNode;

	[Header("Gizmos")] 
	public Color gizmosColorBounds;

	void Start ()
	{
		InitMesh();
		InitVecPos ();
	}

	void Update () {
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
	void InitMesh(){
		mesh = GetComponent<MeshFilter>().mesh;

		// get width, depth, height for scaling
		S = mesh.bounds.size.x;
		T = mesh.bounds.size.y;
		U = mesh.bounds.size.z;

		float sizeObj = 2;
		
		// get min and max points of the model (estimation)
		p0 = - new Vector3 (S/sizeObj, T/sizeObj, U/sizeObj) + new Vector3(0,offsetPoint,0);
		pN = new Vector3 (S/sizeObj, T/sizeObj, U/sizeObj) + new Vector3(0,offsetPoint,0);

		// for every vertex save its s,t,u as a ratio across the lattice space
		for (int i = 0; i < mesh.vertexCount; i++){
			float s = ((mesh.vertices[i].x - p0.x) / (pN.x - p0.x));
			float t = ((mesh.vertices[i].y - p0.y) / (pN.y - p0.y));
			float u = ((mesh.vertices[i].z - p0.z) / (pN.z - p0.z));
			meshCoordinates.Add(new Vector3(s,t,u));
		}
	}

	// Place control points around the object
	void InitVecPos(){
		int i = 0;
		float x,y,z;

		// place n control points across the object at appropriate intervals
		for (x = 0.0f; x < 1.0f; x += 1.0f/CPs_s) {
			for (y = 0.0f; y < 1.0f; y += 1.0f / CPs_t)
			{
				for (z = 0.0f; z < 1.0f; z += 1.0f / CPs_u, i++)
				{
					if (!useControlPoints)
					{
						Vector3 controlPointsRef = p0 + new Vector3(x * S, y * T, z * U) * 2;
						controlPointsVec.Add(controlPointsRef);
					}
					else
					{
						GameObject Node = Instantiate(CPNode, transform.position, Quaternion.identity) as GameObject;
						Node.transform.parent = transform;
						Node.transform.localPosition = (p0 + new Vector3(x*S,y*T ,z*U) * 2 ); // position is min node + % across the object * scale
						controlPointsGO.Add(Node);
					}
				}
			}
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
		
		//Bs[0] = (1.0f - s) * (1.0f - s) * (1.0f - s);
		//Bs[1] = 3.0f * s * (1.0f - s) * (1.0f - s);
		//Bs[2] = 3.0f * s * s * (1.0f - s);
		//Bs[3] = s * s * s;
		
		//3x4x4
		
		//Bs[0] = (1.0f - s) * (1.0f - s);
		//Bs[1] = 2.0f * s * (1.0f - s);
		//Bs[2] = s * s ;
		//
		//Bt[0] = (1.0f - t) * (1.0f - t) * (1.0f - t);
		//Bt[1] = 3.0f * t * (1.0f - t) * (1.0f - t);
		//Bt[2] = 3.0f * t * t * (1.0f - t);
		//Bt[3] = t * t * t;
		//
		//Bu[0] = (1.0f - u) * (1.0f - u) * (1.0f - u);
		//Bu[1] = 3.0f * u * (1.0f - u) * (1.0f - u);
		//Bu[2] = 3.0f * u * u * (1.0f - u);
		//Bu[3] = u * u * u;
		
		//3X3X3
		
		//Bs[0] = (1.0f - s) * (1.0f - s);
		//Bs[1] = 2.0f * s * (1.0f - s);
		//Bs[2] = s * s ;
		//
		//Bt[0] =  (1.0f - t) * (1.0f - t);
		//Bt[1] = 2.0f * t * (1.0f - t);
		//Bt[2] = t * t;
		//
		//Bu[0] = (1.0f - u) * (1.0f - u);
		//Bu[1] = 2.0f * u * (1.0f - u);
		//Bu[2] = u * u;
		
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
					point += controlPointsGO[k+(j*2)+ (i*2*2)].transform.localPosition * (Bs[i]*Bt[j]*Bu[k]);
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