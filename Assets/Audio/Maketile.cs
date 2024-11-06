using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Maketile : MonoBehaviour
{
    public GameObject pointer;
    public GameObject prefeb;
    public Vector3 mospos;
    public int index;
    [Space(20)]
    public Vector2[] boxpos;
    bool dontdo;
    // Start is called before the first frame update
    void Start()
    {
        index = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Audiomanager.instance.test || dontdo)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("gamescene");
            dontdo = true;
        }
        mospos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mospos.y < 0 && mospos.x > 0)
        {
            pointer.transform.position = new Vector2((int)mospos.x + 0.5f, (int)mospos.y - 0.5f);
        }
        else if(mospos.y > 0 && mospos.x < 0)
        {
            pointer.transform.position = new Vector2((int)mospos.x - 0.5f, (int)mospos.y + 0.5f);
        }
        else if(mospos.y < 0 && mospos.x < 0)
        {
            pointer.transform.position = new Vector2((int)mospos.x - 0.5f, (int)mospos.y - 0.5f);
        }
        else
        {
            pointer.transform.position = new Vector2((int)mospos.x + 0.5f, (int)mospos.y + 0.5f);
        }

        if(Input.GetMouseButtonDown(0))
        {
            var t = Physics2D.Raycast(mospos, Vector3.back);
            if(t && t.collider.tag == "tile")
            {
                Destroy(t.collider.gameObject);
            }
            Array.Resize(ref boxpos, boxpos.Length + 1);
            boxpos[boxpos.Length - 1] = pointer.transform.position;
            var p = Instantiate(prefeb, new Vector2(pointer.transform.position.x, pointer.transform.position.y), Quaternion.identity);
            p.GetComponentInChildren<TextMeshPro>().text = index.ToString();
            index++;
        }
            
    }
}
