using UnityEngine;

// S04 - 다이아몬드(팔면체) 메시 만들기
// 정점 6개 / 삼각형 8개
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S04_DiamondMesh : MonoBehaviour
{
    void Start()
    {
        // ── S03에서 정의했던 정점 8개 (0과 1로 구성된 단위 정육면체) ──
        //   0: (0,0,0)   1: (1,0,0)   2: (1,1,0)   3: (0,1,0)
        //   4: (0,0,1)   5: (1,0,1)   6: (1,1,1)   7: (0,1,1)
        //
        // 이 중 y = 0 인 바닥면 4개(S03의 0, 1, 5, 4번)를 다이아몬드의 허리로 쓰고,
        // 위/아래 꼭짓점(apex) 2개를 새로 추가한다.  →  4 + 2 = 총 6개

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0f,  0f, 0f),      // 0  (S03 #0) 허리
            new Vector3(1f,  0f, 0f),      // 1  (S03 #1) 허리
            new Vector3(1f,  0f, 1f),      // 2  (S03 #5) 허리
            new Vector3(0f,  0f, 1f),      // 3  (S03 #4) 허리

            new Vector3(0.5f,  1f, 0.5f),  // 4  새 정점 - 위 꼭짓점
            new Vector3(0.5f, -1f, 0.5f),  // 5  새 정점 - 아래 꼭짓점
        };

        // TODO: 삼각형 8개(인덱스 24개)를 winding order에 맞게 채우기
        int[] triangles = new int[]
{
            // ── 위쪽 4면 (위 꼭짓점 = 4번) ──
            1, 0, 4,
            2, 1, 4,
            3, 2, 4,
            0, 3, 4,

            // ── 아래쪽 4면 (아래 꼭짓점 = 5번) ──
            0, 1, 5,
            1, 2, 5,
            2, 3, 5,
            3, 0, 5,
};

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }
}