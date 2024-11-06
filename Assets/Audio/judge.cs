using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class judge : MonoBehaviour
{
    public static judge instance;
    float[][] chart;
    public int chartCount; // 차트 포인터?

    // 시간 관리
    public float time;
    public float systemTime;

    // 게임 점수 관리
    public int perfect;
    public int good;
    public int bad;
    public int miss;

    // 판정 범위 관리
    public float perfectRange = 0.05f;
    public float goodRange = 0.1f;
    public float badRange = 0.3f;
    public float missRange = 0.5f;


    // Start is called before the first frame update
    private void Awake()
    {
        judge.instance = this;
    }
    void Start()
    {
        var a = GameObject.Find("audio").GetComponent<Audiomanager>();
        var t = GameObject.Find("tiledata").GetComponent<Maketile>();
        chartCount = 0;
        // 시간 초기화
        time = 0;
        systemTime = 0;

        // 게임 점수 초기화
        perfect = 0;
        good = 0;
        bad = 0;
        miss = 0;
        chart = new float[500][];

        chart[0] = new float[3] { 0.703f, 0, 0 };
        chart[1] = new float[3] { 1.194f, 1, 0 };
        chart[2] = new float[3] { 1.407f, 2, 0 };
        chart[3] = new float[3] { 1.663f, 3, 0 };
        chart[4] = new float[3] { 1.855f, 4, 0 };
        chart[5] = new float[3] { 2.239f, 5, 0 };
        chart[6] = new float[3] { 2.773f, 6, 0 };
        chart[7] = new float[3] { 2.965f, 7, 0 };
        chart[8] = new float[3] { 3.157f, 8, 0 };
        chart[9] = new float[3] { 3.370f, 9, 0 };
        chart[10] = new float[3] { 3.711f, 10, 0 };
        chart[11] = new float[3] { 4.202f, 11, 0 };
        chart[12] = new float[3] { 4.415f, 12, 0 };
        chart[13] = new float[3] { 4.586f, 13, 0 };
        chart[14] = new float[3] { 4.799f, 14, 0 };
        chart[15] = new float[3] { 5.183f, 15, 0 };

        //for (int i = 0; i < a.times1.Length; i++)
        //{
        //    chart[i] = new float[3]{ (float)a.times1[i], t.boxpos[i].x, t.boxpos[i].y };
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if(Tilemove.t.stop)
        {
            return;
        }
        // 시간 처리
        time += Time.deltaTime;
        systemTime = Time.time;

        // miss 처리
        if (chartCount < chart.Length && // 채보 안 데이터가
            Time.time > (chart[chartCount][0] + 1 + badRange)) // 현재 시간이 data time 이후 badRange(0.5초)를 지나면 miss 처리
        {
            chartCount++;
            miss++;
            Tilemove.t.misseff();
        }

    }

    public void Judge(float time, float x, float y)
    {
        // 변수
        // time: player 이동 시간, x: player x좌표, y: player y좌표
        // chart[i][0]: note 판정 시간, chart[i][1]: note x좌표, chart[i][2]: note y좌표, i: chartCount ~ chartCount + 9

        // 판정 이후로 10개만 좌표 맞는 데이터배열 찾기
        for (int i = chartCount; i < chartCount + 10; i++)
        {
            if (i > chart.Length - 1) break; // 마지막 노트보다 크면 break
            if (x == chart[i][1] && y == chart[i][2]) // 좌표 일치하면
            {
                print("좌표 일치");
                if (time < (chart[i][0] + 1 + perfectRange) && time > (chart[i][0] + 1 - perfectRange)) // 이동 시간이 
                {
                    perfect++;
                    chartCount++;
                    Tilemove.t.perfecteff();
                    break;
                }
                else if (time < (chart[i][0] + 1 + goodRange) && time > (chart[i][0] + 1 - goodRange)) // 시간 일치하면
                {
                    good++;
                    chartCount++;
                    Tilemove.t.goodeff();
                    break;
                }
                else if (time < (chart[i][0] + 1 + badRange) && time > (chart[i][0] + 1 - badRange)) // 시간 일치하면
                {
                    bad++;
                    chartCount++;
                    Tilemove.t.okeff();
                    break;
                }
                else // miss
                {
                    miss++;
                    chartCount++;
                    Tilemove.t.misseff();
                    break;
                }
            }
        }
    }
}
