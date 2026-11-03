using UnityEngine;

// 중간 프로젝트 - 반딧불이 정자 앞을 맴돔 (등불과 다른 움직임)
// 리사주 곡선 p(t) = (ax·sin(fx·t + φ), ay·sin(fy·t + 2φ), az·sin(fz·t + 3φ))
// 곡선 전체를 y축으로 천천히 돌림: M = Ry(회전 속도 × t) × T(p(t))
// 밝기 대신 크기를 주기적으로 바꿔 깜빡이는 느낌을 냄
public class FireflyOrbit : MonoBehaviour
{
    public Vector3 amplitude = new Vector3(2.5f, 0.5f, 1.5f);   // 축마다 움직이는 폭 (m)
    public Vector3 frequency = new Vector3(0.5f, 1.3f, 0.7f);   // 축마다 빠르기
    public float turnSpeed = 15f;                                // 곡선 전체가 도는 속도 (도/초)
    public float phase = 0f;                                     // 반딧불마다 다르게 (도)
    public float pulseSpeed = 3f;                                // 깜빡임 빠르기
    public float pulseAmount = 0.4f;                             // 깜빡임 크기 (0.4 = ±40%)

    Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        float t = Time.time;
        float p = phase * Mathf.Deg2Rad;

        Vector3 lissajous = new Vector3(
            amplitude.x * Mathf.Sin(frequency.x * t + p),
            amplitude.y * Mathf.Sin(frequency.y * t + 2f * p),
            amplitude.z * Mathf.Sin(frequency.z * t + 3f * p));

        Matrix4x4 M = MyMatrix.RotationY(turnSpeed * t + phase) * MyMatrix.Translation(lissajous);
        transform.localPosition = M.GetColumn(3);

        float k = 1f + pulseAmount * Mathf.Sin(pulseSpeed * t + p);
        transform.localScale = baseScale * k;
    }
}