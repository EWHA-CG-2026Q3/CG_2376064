using UnityEngine;

// 중간 프로젝트 - 이동, 회전 행렬을 직접 채워 만드는 함수 모음
// Matrix4x4.TRS, Quaternion을 쓰지 않고 4×4 행렬의 칸(m행열)을 하나씩 넣음
public static class MyMatrix
{
    // 이동: 4열에 (x, y, z)
    public static Matrix4x4 Translation(Vector3 t)
    {
        Matrix4x4 m = Matrix4x4.identity;
        m.m03 = t.x;
        m.m13 = t.y;
        m.m23 = t.z;
        return m;
    }

    // x축 회전
    // | 1  0  0 |
    // | 0  c -s |
    // | 0  s  c |
    public static Matrix4x4 RotationX(float degrees)
    {
        float c = Mathf.Cos(degrees * Mathf.Deg2Rad);
        float s = Mathf.Sin(degrees * Mathf.Deg2Rad);
        Matrix4x4 m = Matrix4x4.identity;
        m.m11 = c; m.m12 = -s;
        m.m21 = s; m.m22 = c;
        return m;
    }

    // y축 회전
    // |  c  0  s |
    // |  0  1  0 |
    // | -s  0  c |
    public static Matrix4x4 RotationY(float degrees)
    {
        float c = Mathf.Cos(degrees * Mathf.Deg2Rad);
        float s = Mathf.Sin(degrees * Mathf.Deg2Rad);
        Matrix4x4 m = Matrix4x4.identity;
        m.m00 = c; m.m02 = s;
        m.m20 = -s; m.m22 = c;
        return m;
    }

    // z축 회전
    // | c -s  0 |
    // | s  c  0 |
    // | 0  0  1 |
    public static Matrix4x4 RotationZ(float degrees)
    {
        float c = Mathf.Cos(degrees * Mathf.Deg2Rad);
        float s = Mathf.Sin(degrees * Mathf.Deg2Rad);
        Matrix4x4 m = Matrix4x4.identity;
        m.m00 = c; m.m01 = -s;
        m.m10 = s; m.m11 = c;
        return m;
    }
}