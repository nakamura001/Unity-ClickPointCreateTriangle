using UnityEngine;
using System.Collections;

public class ClickPointCreateTriangle : MonoBehaviour {
	
	public Material cubeMaterial;
	GameObject[] cubeList;
	int cubeIndex = 0;
	
	void CubePosReset() {
		Vector3 cubePos = new Vector3(0, 0, -100f);
		for (int i=0; i<cubeList.Length; i++) {
			cubeList[i].transform.position = cubePos;
		}
		cubeIndex = 0;
	}
	
	GameObject CreateCube (Vector3 pos) {
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.renderer.material = cubeMaterial;
		cube.transform.position = pos;
		float s = 0.02f;
		cube.transform.localScale = new Vector3(s, s, s);
		return cube;
	}

	void CreateTriangle ()
	{
		string objectName = "Triangle";
		Mesh mesh = new Mesh ();
		
		Vector3[] vertices = new Vector3[3];
		for (int i=0; i<vertices.Length; i++) {
			vertices [i] = cubeList [i].transform.position;
		}
		mesh.vertices = vertices;
		
		int[] triangles = new int[3];
		Vector3 v1 = mesh.vertices [1] - mesh.vertices [0];
		Vector3 v2 = mesh.vertices [2] - mesh.vertices [1];
		Vector3 crossVec = Vector3.Cross (v1, v2);
		if (Vector3.Dot (Camera.mainCamera.transform.position, crossVec) > 0) {
			triangles [0] = 0;
			triangles [1] = 1;
			triangles [2] = 2;
		} else {
			triangles [0] = 0;
			triangles [1] = 2;
			triangles [2] = 1;
		}
		mesh.triangles = triangles;

		mesh.uv = new Vector2[]{
			new Vector2 (0.0f, 1.0f),
			new Vector2 (1.0f, 1.0f),
			new Vector2 (0.0f, 0.0f)
		};		

		mesh.RecalculateNormals ();	// 法線の再計算
		mesh.RecalculateBounds ();	// バウンディングボリュームの再計算
		mesh.Optimize ();
		
		GameObject newGameobject = new GameObject (objectName);
		
		MeshRenderer meshRenderer = newGameobject.AddComponent<MeshRenderer> ();
		meshRenderer.material = new Material (Shader.Find ("Diffuse"));
		
		
		MeshFilter meshFilter = newGameobject.AddComponent<MeshFilter> ();
		meshFilter.mesh = mesh;
	}

	void Start ()
	{
		cubeList = new GameObject[3];
		Vector3 cubePos = new Vector3 (0, 0, -100f);
		for (int i=0; i<cubeList.Length; i++) {
			cubeList [i] = CreateCube (cubePos);
		}
		CubePosReset ();
	}
	
	void Update ()
	{
		if (Input.GetMouseButtonUp (0)) {
			if (cubeIndex <= cubeList.Length - 1) {
				Vector3 mousePos = Input.mousePosition;
				mousePos.z = 1f;
				cubeList [cubeIndex++].transform.position = Camera.mainCamera.ScreenToWorldPoint (mousePos);
				if (cubeIndex == cubeList.Length) {
					CreateTriangle ();
					CubePosReset ();
				}
			}
		}
	}
}
