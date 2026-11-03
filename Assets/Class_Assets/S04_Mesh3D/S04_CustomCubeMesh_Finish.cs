using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S04_CustomCubeMesh_Finish : MonoBehaviour
{
    void OnEnable()
    {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0f, 0f, 0f), // 0
            new Vector3(1f, 0f, 0f), // 1
            new Vector3(1f, 1f, 0f), // 2
            new Vector3(0f, 1f, 0f), // 3
            new Vector3(0f, 0f, 1f), // 4
            new Vector3(1f, 0f, 1f), // 5
            new Vector3(1f, 1f, 1f), // 6
            new Vector3(0f, 1f, 1f), // 7
        };

        int[] triangles = new int[]
        {
            0,2,1, 0,3,2, // z=0 면
            4,5,6, 4,6,7, // z=1 면
            0,1,5, 0,5,4, // y=0 면
            3,6,2, 3,7,6, // y=1 면
            0,7,3, 0,4,7, // x=0 면
            1,2,6, 1,6,5, // x=1 면
        };

        // 잘못된 예시 — 6개 면 모두 정점 순서가 반대로 뒤집혀 있음
        //int[] triangles = new int[]
        //{
        //    0,1,2, 0,2,3, // z=0 면 (원래: 0,2,1, 0,3,2)
        //    4,6,5, 4,7,6, // z=1 면 (원래: 4,5,6, 4,6,7)
        //    0,5,1, 0,4,5, // y=0 면 (원래: 0,1,5, 0,5,4)
        //    3,2,6, 3,6,7, // y=1 면 (원래: 3,6,2, 3,7,6)
        //    0,3,7, 0,7,4, // x=0 면 (원래: 0,7,3, 0,4,7)
        //    1,6,2, 1,5,6, // x=1 면 (원래: 1,2,6, 1,6,5)
        //};

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }
}