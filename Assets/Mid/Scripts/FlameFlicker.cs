using UnityEngine;

// 중간 프로젝트 - 등불 안의 불꽃이 일렁임 (자식 오브젝트의 움직임)
// 부모(등불)가 떠다니는 동안, 자식인 불꽃은 따로 위아래로 늘었다 줄었다 하고 빛의 세기도 같이 바뀜
public class FlameFlicker : MonoBehaviour
{
    public Light flameLight;              // 같은 등불의 Point Light
    public float flickerAmount = 0.15f;   // 늘고 주는 정도 (0.15 = ±15%)
    public float flickerSpeed = 8f;       // 일렁이는 빠르기
    public float seed = 0f;               // 등불마다 다르게 (0, 10, 20, ...)

    Vector3 baseScale;
    float baseIntensity;

    void Start()
    {
        baseScale = transform.localScale;
        if (flameLight != null) baseIntensity = flameLight.intensity;
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(seed, Time.time * flickerSpeed);   // 0 ~ 1
        float k = 1f + flickerAmount * (noise * 2f - 1f);                  // 0.85 ~ 1.15

        // 위로 늘어나면 옆으로는 줄어듦 (부피가 비슷하게)
        float side = 1f / Mathf.Sqrt(k);
        transform.localScale = new Vector3(baseScale.x * side, baseScale.y * k, baseScale.z * side);

        if (flameLight != null) flameLight.intensity = baseIntensity * k;
    }
}