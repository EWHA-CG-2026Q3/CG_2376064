using UnityEngine;

// 동차좌표와 4×4 행렬로 기울이기(shear)를 적용하는 스크립트
// 높이(y)에 비례해 x축 방향으로 밀림: (x, y, z) → (x + k·y, y, z)
// Unity의 Matrix4x4 타입 없이 float[4,4] 배열만으로 계산함
[ExecuteAlways]
[RequireComponent(typeof(DiamondMesh))]
public class S09_Shear : MonoBehaviour
{
    [SerializeField] float k = 1f;   // 기울이기 정도. 학번 끝자리 4 → k = (4 + 1) ÷ 5 = 1

    DiamondMesh diamondMesh;

    void OnEnable()
    {
        diamondMesh = GetComponent<DiamondMesh>();
    }

    void Update()
    {
        if (diamondMesh == null || diamondMesh.BaseVertices == null) return;

        Vector3[] verts = ApplyShear_Raw(diamondMesh.BaseVertices, k);
        diamondMesh.SetVertices(verts);
    }

    // ---------- 행렬 빌더 ----------

    // 1열: e₁ 그대로 / 2열: e₂ → (k, 1, 0) / 3열: e₃ 그대로 / 4열: 원점 그대로
    float[,] ShearMatrixRaw(float k)
    {
        return new float[,] {
            { 1f, k,  0f, 0f },
            { 0f, 1f, 0f, 0f },
            { 0f, 0f, 1f, 0f },
            { 0f, 0f, 0f, 1f }
        };
    }

    // ---------- 동차좌표 ----------

    // 정점에 네 번째 성분 1을 붙임
    Vector4 ToHomogeneous(Vector3 v)
    {
        return new Vector4(v.x, v.y, v.z, 1f);
    }

    // 계산이 끝난 뒤 네 번째 성분을 떼어냄
    Vector3 FromHomogeneous(Vector4 h)
    {
        return new Vector3(h.x, h.y, h.z);
    }

    // 4×4 행렬-벡터 곱: MultiplyMatrixVector3x3과 구조가 같고 반복 횟수만 4
    Vector4 MultiplyMatrixVectorRaw(float[,] M, Vector4 v)
    {
        float[] input = { v.x, v.y, v.z, v.w };
        float[] result = new float[4];
        for (int row = 0; row < 4; row++)
            for (int col = 0; col < 4; col++)
                result[row] += M[row, col] * input[col]; // 행과 벡터의 내적
        return new Vector4(result[0], result[1], result[2], result[3]);
    }

    // ---------- 적용 ----------

    // 모든 정점에 기울이기 행렬을 적용
    Vector3[] ApplyShear_Raw(Vector3[] baseVertices, float k)
    {
        float[,] Sh = ShearMatrixRaw(k);

        Vector3[] verts = new Vector3[baseVertices.Length];
        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector4 h = ToHomogeneous(baseVertices[i]);
            h = MultiplyMatrixVectorRaw(Sh, h);
            verts[i] = FromHomogeneous(h);
        }
        return verts;
    }
}