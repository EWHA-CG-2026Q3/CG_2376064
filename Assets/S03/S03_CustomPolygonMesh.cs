using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S03_CustomPolygonMesh : MonoBehaviour
{
    void Start()
    {
        // 정육각형: 맨 위 꼭짓점에서 시작해 시계방향으로 배치
        // 각도 = 90도 - i * 60도, 반지름 1
        Vector3[] vertices = new Vector3[]
        {
            new Vector3( 0f,      1f,     0f), // 0  맨 위
            new Vector3( 0.8660f, 0.5f,   0f), // 1  오른쪽 위
            new Vector3( 0.8660f,-0.5f,   0f), // 2  오른쪽 아래
            new Vector3( 0f,     -1f,     0f), // 3  맨 아래
            new Vector3(-0.8660f,-0.5f,   0f), // 4  왼쪽 아래
            new Vector3(-0.8660f, 0.5f,   0f), // 5  왼쪽 위
        };

        // 부채꼴 분할: 정점 0을 축으로 6-2 = 4개의 삼각형
        int[] triangles = new int[]
        {
            0, 1, 2,
            0, 2, 3,
            0, 3, 4,
            0, 4, 5,
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().sharedMaterial =
            new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }
}