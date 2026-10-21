using UnityEngine;

// S12 - 정물화용 기둥 메시 (n각기둥)
// 정점: 아래 둘레 n개 + 위 둘레 n개 + 아래 중심 1개 + 위 중심 1개 = 2n + 2개
// 삼각형: 옆면 n × 2 + 윗면 n + 아랫면 n = 4n개
// 피벗은 바닥 한가운데 (y = 0이 바닥, y = height가 윗면)
[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class S12_PillarMesh : MonoBehaviour
{
    public int segments = 8;        // 몇 각기둥인지 (3 이상)
    public float radius = 0.4f;     // 중심에서 둘레 정점까지 거리
    public float height = 2.5f;     // 기둥 높이

    void Start()
    {
        int n = Mathf.Max(3, segments);

        // ---- 정점 ----
        //   0 ~ n-1     : 아래 둘레 (y = 0)
        //   n ~ 2n-1    : 위 둘레   (y = height), i번 바로 위가 n + i번
        //   2n          : 아래 중심
        //   2n + 1      : 위 중심
        Vector3[] vertices = new Vector3[2 * n + 2];
        for (int i = 0; i < n; i++)
        {
            float angle = i * 2f * Mathf.PI / n;                 // +x에서 시작해 +z 쪽으로 돎
            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);
            vertices[i] = new Vector3(x, 0f, z);             // 아래 둘레
            vertices[n + i] = new Vector3(x, height, z);         // 위 둘레
        }
        int bottomCenter = 2 * n;
        int topCenter = 2 * n + 1;
        vertices[bottomCenter] = new Vector3(0f, 0f, 0f);
        vertices[topCenter] = new Vector3(0f, height, 0f);

        // ---- 삼각형 (바깥에서 봤을 때 시계 방향) ----
        int[] triangles = new int[4 * n * 3];
        int t = 0;
        for (int i = 0; i < n; i++)
        {
            int j = (i + 1) % n;                                 // 다음 둘레 정점 (마지막은 0으로 돌아옴)
            int bi = i, bj = j;                                  // 아래 둘레
            int ti = n + i, tj = n + j;                          // 위 둘레

            // 옆면 사각형 = 삼각형 2개
            triangles[t++] = bi; triangles[t++] = ti; triangles[t++] = bj;
            triangles[t++] = bj; triangles[t++] = ti; triangles[t++] = tj;

            // 윗면 부채꼴 (위에서 봤을 때 시계 방향)
            triangles[t++] = topCenter; triangles[t++] = tj; triangles[t++] = ti;

            // 아랫면 부채꼴 (아래에서 봤을 때 시계 방향)
            triangles[t++] = bottomCenter; triangles[t++] = bi; triangles[t++] = bj;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = mesh;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer.sharedMaterial == null)
            meshRenderer.sharedMaterial =
                new Material(Shader.Find("Universal Render Pipeline/Lit"));
    }
}