using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Audiomanager : MonoBehaviour
{
    public bool test;
    [Space(20)]
    public float time;
    public static Audiomanager instance;
    public AudioSource a;
    public Slider s;
    public TMP_Dropdown d;
    public TextMeshProUGUI t;
    public GameObject h;
    public GameObject prefeb;
    public GameObject stamps;
    public AudioClip[] clips;
    public List<string> optionList = new List<string>();
    public double[] times1;
    public double[] times2;
    public double[] times3;
    public double[] times4;
    public double[] times5;
    bool p;
    private void Awake()
    {
        Audiomanager.instance = this;
    }
    private void Start()
    {
        a = gameObject.GetComponent<AudioSource>();
        if (test)
        {
            return;
        }
        d.ClearOptions();
        for(int i = 0; i<clips.Length; i++)
        {
            optionList.Add(clips[i].name);
        }
        d.AddOptions(optionList);

        s.maxValue = a.clip.length;
        
    }
    private void Update()
    {

        time = a.time;
        if (test)
        {
            return;
        }
        s.value = a.time;
        t.text = ((int)a.time).ToString();
    }

    public void plus()
    {
        a.time += 5;
    }
    public void minus()
    {
        if(a.time >= 5)
        {
            a.time -= 5;
        }
    }
    public void change()
    {
        for (int i = 0; i < stamps.transform.childCount; i++)
        {
            Destroy(stamps.transform.GetChild(i).gameObject);
        }
        a.clip = clips[d.value];
        s.maxValue = a.clip.length;
        a.time = 0;
    }
    public void play()
    {
        a.Play();
    }
    public void stop()
    {        
        if(p)
        {
            a.UnPause();
            p = false;
        }
        else
        {
            a.Pause();
            p = true;
        }
    }
    public void tostart()
    {
        a.Stop();
        a.time = 0;
    }

    public void maketime()
    {
        Instantiate(prefeb, new Vector2(h.transform.position.x, 0), Quaternion.identity, stamps.transform);
        switch (d.value + 1)
        {
            case 1:
                Array.Resize(ref times1, times1.Length + 1);
                times1[times1.Length - 1] = Time.time;
                break;
            case 2:
                Array.Resize(ref times2, times2.Length + 1);
                times2[times2.Length - 1] = Time.time;
                break;
            case 3:
                Array.Resize(ref times3, times3.Length + 1);
                times3[times3.Length - 1] = Time.time;
                break;
            case 4:
                Array.Resize(ref times4, times4.Length + 1);
                times4[times4.Length - 1] = Time.time;
                break;
            case 5:
                Array.Resize(ref times5, times5.Length + 1);
                times5[times5.Length - 1] = Time.time;
                break;

        }

    }

    public void re()
    {
        switch (d.value + 1)
        {
            case 1:
                Array.Resize(ref times1, 0);
                break;
            case 2:
                Array.Resize(ref times2, 0);
                break;
            case 3:
                Array.Resize(ref times3, 0);
                break;
            case 4:
                Array.Resize(ref times4, 0);
                break;
            case 5:
                Array.Resize(ref times5, 0);
                break;

        }
        for (int i = 0; i < stamps.transform.childCount; i++)
        {
            Destroy(stamps.transform.GetChild(i).gameObject);
        }
        a.Stop();
        a.time = 0;
    }

    public void togame()
    {
        a.clip = clips[0];
        a.Stop();
        a.time = 0;
        SceneManager.LoadScene("Tile");
    }

}
