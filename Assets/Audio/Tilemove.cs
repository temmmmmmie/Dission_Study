using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Tilemove : MonoBehaviour
{
    public static Tilemove t;
    [Header("판정")]
    public Vector2 perfect;
    public Vector2 good;
    public Vector2 ok;
    public Vector2 miss;
    [Space(30)]
    public Vector2 perfectran;
    public Vector2 goodran;
    public Vector2 okran;
    public Vector2 missran;
    [Space(50)]
    public TextMeshProUGUI ui;
    public TMP_Dropdown d;
    public GameObject note;
    public GameObject noteeff;
    public TextMeshProUGUI judget;
    public Animation judgetani;
    public Light2D judgelight;
    public Image progressbar;
    public TextMeshProUGUI scoret;
    public Animation eff1;
    [Space(50)]
    public int score;
    public float notethrehold;
    public float boxthrehold;
    public KeyCode up;
    public KeyCode down;
    public KeyCode right;
    public KeyCode left;
    public bool stop;
    public double[] playerinput;
    int track;
    public int i = 0;
    public int s = 0;
    public Maketile tile;
    private void Awake()
    {
        Tilemove.t = this;
    }

    private void Start()
    {
        score = 1000000;
        d.ClearOptions();
        d.AddOptions(Audiomanager.instance.optionList);
        tile =GameObject.Find("tiledata").GetComponent<Maketile>();
        stop = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(perfect, perfectran);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(good, goodran);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(ok, okran);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(miss, missran);
    }
    public void change()
    {
        Audiomanager.instance.a.clip = Audiomanager.instance.clips[d.value];
        track = d.value;
    }
    public void startgame()
    {       
        ui.text = "";
        StartCoroutine("count");
    }
    // Update is called once per frame
    void Update()
    {
        if(stop)
        {
            return;
        }
        progressbar.fillAmount = Audiomanager.instance.a.time / Audiomanager.instance.clips[track].length;
        scoret.text = score.ToString();
        if (Input.GetKeyDown(up))
        {
            Array.Resize(ref playerinput, playerinput.Length + 1);
            playerinput[playerinput.Length - 1] = Audiomanager.instance.a.time;
            var ray = Physics2D.Raycast(gameObject.transform.position, Vector2.up, 1);
            if (ray && ray.collider.tag == "wall")
            {
                return;
            }
            else
            {
                gameObject.transform.position += new Vector3(0, 1);
            }
            judge.instance.Judge(Time.time, gameObject.transform.position.x, gameObject.transform.position.y);

        }
        if (Input.GetKeyDown(down))
        {
            Array.Resize(ref playerinput, playerinput.Length + 1);
            playerinput[playerinput.Length - 1] = Audiomanager.instance.a.time;
            var ray = Physics2D.Raycast(gameObject.transform.position, Vector2.down, 1);
            if (ray && ray.collider.tag == "wall")
            {
                return;
            }
            else
            {
                gameObject.transform.position += new Vector3(0, -1);
            }
            judge.instance.Judge(Time.time, gameObject.transform.position.x, gameObject.transform.position.y);


        }
        if (Input.GetKeyDown(right))
        {
            Array.Resize(ref playerinput, playerinput.Length + 1);
            playerinput[playerinput.Length - 1] = Audiomanager.instance.a.time;
            var ray = Physics2D.Raycast(gameObject.transform.position, Vector2.right, 1);
            if (ray && ray.collider.tag == "wall")
            {
                return;
            }
            else
            {
                gameObject.transform.position += new Vector3(1, 0);
            }
            judge.instance.Judge(Time.time, gameObject.transform.position.x, gameObject.transform.position.y);

        }
        if (Input.GetKeyDown(left))
        {
            Array.Resize(ref playerinput, playerinput.Length + 1);
            playerinput[playerinput.Length - 1] = Audiomanager.instance.a.time;
            var ray = Physics2D.Raycast(gameObject.transform.position, Vector2.left, 1);
            if(ray && ray.collider.tag == "wall")
            {
                return;
            }
            else
            {
                gameObject.transform.position += new Vector3(-1, 0);
            }
            judge.instance.Judge(Time.time, gameObject.transform.position.x, gameObject.transform.position.y);

        }
       
    }
    IEnumerator count()
    {
        ui.text = "3";
        yield return new WaitForSeconds(1);
        ui.text = "2";
        yield return new WaitForSeconds(1);
        ui.text = "1";
        yield return new WaitForSeconds(1);
        ui.text = "Go!";
        yield return new WaitForSeconds(1);
        ui.text = "";
        stop = false;
        StartCoroutine("eff");
        Audiomanager.instance.play();
    }
    IEnumerator eff()
    {
        switch (track)
        {
            case 0:
                while (i <= Audiomanager.instance.times1.Length)
                {
                    if (Audiomanager.instance.times1[s] - boxthrehold < Audiomanager.instance.a.time && Audiomanager.instance.a.time < Audiomanager.instance.times1[s] + boxthrehold)
                    {
                        //Instantiate(noteeff, new Vector2(tile.boxpos[i].x, tile.boxpos[i].y), Quaternion.identity);
                        s++;
                    }
                    if (Audiomanager.instance.times1[i] - notethrehold < Audiomanager.instance.a.time && Audiomanager.instance.a.time < Audiomanager.instance.times1[i] + notethrehold)
                    {
                        Instantiate(note);
                        i++;
                    }
                    yield return null;
                }
                break;
            case 1:
                while (i < Audiomanager.instance.times2.Length)
                {
                    if (Audiomanager.instance.times2[i] - notethrehold < Audiomanager.instance.a.time && Audiomanager.instance.a.time < Audiomanager.instance.times2[i] + notethrehold)
                    {
                        Instantiate(note);
                        i++;
                    }
                    yield return null;
                }
                break;
            case 2:
                while (i < Audiomanager.instance.times3.Length)
                {
                    if (Audiomanager.instance.times3[i] - notethrehold < Audiomanager.instance.a.time && Audiomanager.instance.a.time < Audiomanager.instance.times3[i] + notethrehold)
                    {
                        Instantiate(note);
                        i++;
                    }
                    yield return null;
                }
                break;
            case 3:
                while (i < Audiomanager.instance.times4.Length)
                {
                    if (Audiomanager.instance.times4[i] - notethrehold < Audiomanager.instance.a.time && Audiomanager.instance.a.time < Audiomanager.instance.times4[i] + notethrehold)
                    {
                        Instantiate(note);
                        i++;
                    }
                    yield return null;
                }
                break;
            case 4:
                while (i < Audiomanager.instance.times5.Length)
                {
                    if (Audiomanager.instance.times5[i] - notethrehold < Audiomanager.instance.a.time && Audiomanager.instance.a.time < Audiomanager.instance.times5[i] + notethrehold)
                    {
                        Instantiate(note);
                        i++;
                    }
                    yield return null;
                }
                break;
        }
    }

    public void perfecteff()
    {
        judget.text = "Perfect!";
        judget.color = Color.yellow;
        judgelight.color = Color.yellow;
        judgetani.Play();
        score += 1000;
        eff1.Play();
    }
    public void goodeff()
    {
        judget.text = "Good";
        judget.color = Color.blue;
        judgelight.color = Color.blue;
        judgetani.Play();
        score += 800;
    }
    public void okeff()
    {
        judget.text = "Ok";
        judget.color = Color.green;
        judgelight.color = Color.green;
        judgetani.Play();
        score += 500;
    }
    public void misseff()
    {
        judget.text = "Miss";
        judget.color = Color.gray;
        judgelight.color = Color.gray;
        judgetani.Play();
    }
}
