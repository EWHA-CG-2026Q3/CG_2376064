using System.Collections.Generic;
using UnityEngine;

// 중간 프로젝트 - 반딧불이 지나간 자리를 GL 선으로 이음 (OpenGL 프로그래밍)
// 자식 반딧불마다 최근 위치 trailLength개를 기억해 GL.LINE_STRIP으로 그림
// 최근 위치일수록 밝게, 오래된 위치일수록 어둡게
public class FireflyTrailGL : MonoBehaviour
{
    [SerializeField] Material glMaterial;                    // S06_MyGLMaterial (정점 색을 그대로 쓰는 셰이더)
    [SerializeField] int trailLength = 40;                   // 기억할 위치 수
    [SerializeField] float recordInterval = 0.03f;           // 위치를 기록하는 간격 (초)
    [SerializeField] Color headColor = new Color(0.9f, 1f, 0.4f, 1f);
    [SerializeField] Color tailColor = new Color(0.02f, 0.04f, 0.02f, 1f);

    List<Vector3>[] trails;
    float timer;

    void Start()
    {
        trails = new List<Vector3>[transform.childCount];
        for (int i = 0; i < trails.Length; i++)
            trails[i] = new List<Vector3>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < recordInterval) return;
        timer = 0f;

        for (int i = 0; i < trails.Length; i++)
        {
            trails[i].Insert(0, transform.GetChild(i).position);   // 맨 앞 = 지금 위치
            if (trails[i].Count > trailLength)
                trails[i].RemoveAt(trails[i].Count - 1);           // 가장 오래된 위치 버림
        }
    }

    void OnRenderObject()
    {
        if (glMaterial == null || trails == null) return;

        glMaterial.SetPass(0);
        foreach (List<Vector3> trail in trails)
        {
            if (trail.Count < 2) continue;
            GL.Begin(GL.LINE_STRIP);                                // 정점을 차례로 이어 선 하나로
            for (int j = 0; j < trail.Count; j++)
            {
                GL.Color(Color.Lerp(headColor, tailColor, (float)j / (trail.Count - 1)));
                GL.Vertex(trail[j]);
            }
            GL.End();
        }
    }
}