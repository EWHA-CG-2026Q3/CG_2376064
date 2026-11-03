using System.Collections.Generic;
using UnityEngine;

// 중간 프로젝트 - 연꽃 등 메시 (반복문으로 만든 새로운 형태)
// 꽃잎: 겹 layerCount개 × 겹마다 petalCount장
//   꽃잎 p의 방향 각도 = 360° / n × (p + 엇갈림), 홀수 겹은 반 장씩 엇갈림
//   꽃잎 한 장 = 길이 방향 rows칸 × 폭 방향 cols칸 격자, 앞면과 뒷면을 따로 만듦
// 받침: 가운데 점 + 원 위의 점 2n개로 채운 판
// 초: 원기둥 (submesh 1 → Mesh Renderer의 두 번째 재질)
// Inspector에서 값을 바꾸면 바로 다시 만듦 (Play 중에도)
[ExecuteAlways]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LotusLanternMesh : MonoBehaviour
{
    [Header("꽃잎")]
    [Range(3, 16)] public int petalCount = 8;    // 겹마다 꽃잎 수
    [Range(1, 5)] public int layerCount = 3;     // 겹 수
    public float innerAngle = 20f;               // 안쪽 겹이 수직에서 벌어진 각도 (도)
    public float outerAngle = 70f;               // 바깥 겹이 수직에서 벌어진 각도 (도)
    public float innerLength = 0.16f;            // 안쪽 겹 꽃잎 길이
    public float outerLength = 0.22f;            // 바깥 겹 꽃잎 길이
    public float petalWidth = 0.09f;             // 꽃잎 가장 넓은 곳의 폭
    public float curl = 0.06f;                   // 꽃잎 끝이 위로 말려 올라가는 높이
    public float cup = 0.02f;                    // 꽃잎 가장자리가 안쪽으로 오므라드는 양
    [Range(2, 12)] public int rows = 6;          // 꽃잎 길이 방향 칸 수
    [Range(2, 8)] public int cols = 4;           // 꽃잎 폭 방향 칸 수

    [Header("받침과 초")]
    public float baseRadius = 0.06f;
    public float candleRadius = 0.025f;
    public float candleHeight = 0.08f;
    [Range(6, 32)] public int candleSegments = 16;

    Mesh mesh;
    bool dirty = true;

    void OnEnable() { Build(); dirty = false; }   // 씬을 열거나 Play를 누르면 바로 만듦
    void OnValidate() { dirty = true; }           // Inspector 값이 바뀌면 다시 만들도록 표시

    void Update()
    {
        if (!dirty) return;
        Build();
        dirty = false;
    }

    void Build()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> petalTriangles = new List<int>();    // submesh 0: 꽃잎 + 받침
        List<int> candleTriangles = new List<int>();   // submesh 1: 초

        // ---- 1. 꽃잎: 겹마다, 장마다 ----
        for (int layer = 0; layer < layerCount; layer++)
        {
            float k = (layerCount == 1) ? 0f : (float)layer / (layerCount - 1);   // 0 안쪽 ~ 1 바깥
            float open = Mathf.Lerp(innerAngle, outerAngle, k) * Mathf.Deg2Rad;
            float length = Mathf.Lerp(innerLength, outerLength, k);
            float width = petalWidth * Mathf.Lerp(0.8f, 1.2f, k);               // 바깥 겹일수록 넓게
            float stagger = (layer % 2) * 0.5f;                                    // 겹마다 반 장씩 엇갈림

            for (int p = 0; p < petalCount; p++)
            {
                float angle = 2f * Mathf.PI / petalCount * (p + stagger);
                AddPetal(vertices, petalTriangles, angle, open, length, width);
            }
        }

        // ---- 2. 받침: 가운데 점 + 원 위의 점 ring개 ----
        int ring = petalCount * 2;
        int center = vertices.Count;
        vertices.Add(Vector3.zero);
        for (int i = 0; i < ring; i++)
        {
            float a = 2f * Mathf.PI / ring * i;
            vertices.Add(new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a)) * baseRadius * 1.3f);
        }
        for (int i = 0; i < ring; i++)
        {
            int j = (i + 1) % ring;                              // 마지막 점은 0번으로 돌아옴
            petalTriangles.Add(center);
            petalTriangles.Add(center + 1 + j);
            petalTriangles.Add(center + 1 + i);                  // 위에서 볼 때 시계 방향
        }

        // ---- 3. 초: 원기둥 (S12 기둥과 같은 순서) ----
        int n = candleSegments;
        int bottom = vertices.Count;          // bottom + i : 아래 둘레
        for (int i = 0; i < n; i++)
        {
            float a = 2f * Mathf.PI / n * i;
            vertices.Add(new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a)) * candleRadius);
        }
        int top = vertices.Count;             // top + i : 위 둘레
        for (int i = 0; i < n; i++)
        {
            float a = 2f * Mathf.PI / n * i;
            vertices.Add(new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a)) * candleRadius + Vector3.up * candleHeight);
        }
        int topCenter = vertices.Count;
        vertices.Add(Vector3.up * candleHeight);

        for (int i = 0; i < n; i++)
        {
            int j = (i + 1) % n;
            // 옆면 사각형 = 삼각형 2개
            candleTriangles.Add(bottom + i); candleTriangles.Add(top + i); candleTriangles.Add(bottom + j);
            candleTriangles.Add(bottom + j); candleTriangles.Add(top + i); candleTriangles.Add(top + j);
            // 윗면 부채꼴
            candleTriangles.Add(topCenter); candleTriangles.Add(top + j); candleTriangles.Add(top + i);
        }

        // ---- 4. 메시에 넣기 ----
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.name = "LotusLantern";
        }
        mesh.Clear();
        mesh.SetVertices(vertices);
        mesh.subMeshCount = 2;
        mesh.SetTriangles(petalTriangles, 0);
        mesh.SetTriangles(candleTriangles, 1);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    // 꽃잎 한 장: 밑동(s = 0)에서 끝(s = 1)까지, 왼쪽(u = -1)에서 오른쪽(u = 1)까지 격자
    void AddPetal(List<Vector3> vertices, List<int> triangles,
                  float angle, float open, float length, float width)
    {
        Vector3 outward = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));   // 꽃잎이 뻗는 방향
        Vector3 side = new Vector3(-Mathf.Sin(angle), 0f, Mathf.Cos(angle));     // 꽃잎의 폭 방향
        float r0 = baseRadius * 0.8f;                                            // 꽃잎이 시작하는 반지름

        // 앞면 정점
        int front = vertices.Count;
        for (int r = 0; r <= rows; r++)
        {
            float s = (float)r / rows;
            float radial = r0 + length * Mathf.Sin(open) * s;                    // 바깥으로 벌어짐
            float height = length * Mathf.Cos(open) * s + curl * s * s;          // 위로 올라가고 끝이 말림
            float halfWidth = width * 0.5f * Mathf.Sin(Mathf.PI * (0.15f + 0.85f * s));   // 밑동은 좁게, 끝은 뾰족하게
            for (int c = 0; c <= cols; c++)
            {
                float u = (float)c / cols * 2f - 1f;
                float inward = cup * u * u * Mathf.Sin(Mathf.PI * s);            // 가장자리를 안쪽으로 오므림
                vertices.Add(outward * (radial - inward) + side * (halfWidth * u) + Vector3.up * height);
            }
        }

        // 뒷면 정점: 같은 위치, 노멀만 반대가 되도록 따로 둠
        int count = vertices.Count - front;
        for (int i = 0; i < count; i++)
            vertices.Add(vertices[front + i]);
        int back = front + count;

        // 격자 칸마다 삼각형 2개, 뒷면은 감는 순서를 뒤집음
        int w = cols + 1;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int a = r * w + c, b = a + 1, d = a + w, e = d + 1;
                triangles.Add(front + a); triangles.Add(front + d); triangles.Add(front + b);
                triangles.Add(front + b); triangles.Add(front + d); triangles.Add(front + e);

                triangles.Add(back + a); triangles.Add(back + b); triangles.Add(back + d);
                triangles.Add(back + b); triangles.Add(back + e); triangles.Add(back + d);
            }
        }
    }
}