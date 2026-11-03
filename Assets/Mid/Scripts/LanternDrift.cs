using UnityEngine;

// 중간 프로젝트 - 연꽃 등이 연못 위를 떠다니게 함 (우리가 만든 행렬)
// 부모(연못 가운데)를 기준으로 원을 따라 돌고, 위아래로 출렁이고, 앞뒤·좌우로 기울고, 제자리에서 천천히 돔
//   M = T(원 위의 위치 + 출렁임) × Ry(제자리 회전) × Rx(앞뒤 기울기) × Rz(좌우 기울기)
// M의 4열(위치)과 회전을 이 오브젝트의 Transform에 넣음 → 자식(꽃, 불꽃, 빛)이 함께 움직임 (부모-자식)
public class LanternDrift : MonoBehaviour
{
    public float orbitRadius = 1.0f;     // 연못 가운데에서의 거리 (m)
    public float orbitSpeed = 8f;        // 연못을 도는 속도 (도/초)
    public float phase = 0f;             // 시작 각도 (도), 등불마다 다르게
    public float radiusWobble = 0.15f;   // 반지름이 흔들리는 양, 원이 너무 반듯하지 않게
    public float bobHeight = 0.03f;      // 위아래 출렁임 (m)
    public float bobSpeed = 1.2f;        // 출렁임 빠르기
    public float tiltAngle = 6f;         // 최대 기울기 (도)
    public float spinSpeed = 10f;        // 제자리 회전 (도/초)

    void Update()
    {
        float t = Time.time;
        float p = phase * Mathf.Deg2Rad;

        // 원 위의 위치 + 출렁임
        float orbit = orbitSpeed * Mathf.Deg2Rad * t + p;
        float r = orbitRadius + radiusWobble * Mathf.Sin(0.7f * t + p);
        Vector3 position = new Vector3(
            r * Mathf.Cos(orbit),
            bobHeight * Mathf.Sin(bobSpeed * t + p),
            r * Mathf.Sin(orbit));

        // 오른쪽 행렬부터 정점에 적용됨: 기울이고 → 돌리고 → 옮김
        Matrix4x4 M = MyMatrix.Translation(position)
                    * MyMatrix.RotationY(spinSpeed * t + phase)
                    * MyMatrix.RotationX(tiltAngle * Mathf.Sin(bobSpeed * t + p + 1f))
                    * MyMatrix.RotationZ(tiltAngle * Mathf.Sin(bobSpeed * 0.8f * t + p));

        transform.localPosition = M.GetColumn(3);   // 4열 = 위치
        transform.localRotation = M.rotation;        // 1–3열 = 회전
    }
}