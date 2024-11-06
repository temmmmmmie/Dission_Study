using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class inventory : MonoBehaviour
{
    public static bool inving;
    public bool holdingitem;
    public int current;
    public GameObject inv;
    public GameObject[] invs = new GameObject[16]; //img 0 num 1
    public GameObject tempimg;
    public GameObject tooltipobj;
    public KeyCode invkey;
    [Space(40)]
    [Header("Magnet")]
    public Vector2 pos;
    public Vector2 size;

    [Space(40)]
    [Header("Current")]
    public string[] item = new string[16];
    public int[] num = new int[16];
    [Space(40)]
    [Header("Itemdic")]
    public string[] itemname;
    public string[] itemdesc;
    public Sprite[] img;

    string itemtemp;
    int numtemp;
    bool tooltipon;
    int holditemindex;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x + pos.x, gameObject.transform.position.y + pos.y), size);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "item")
        {
            for (int i = 0; i < item.Length; i++)
            {
                if (item[i] == collision.gameObject.name)
                {
                    num[i]++;
                    goto end;
                }
            }
            for (int i = 0; i < item.Length; i++) //new
            {
                if (item[i] == "")
                {
                    item[i] = collision.gameObject.name;
                    num[i]++;
                    goto end;
                }
            }
            full();
            return;
        end:
            Destroy(collision.gameObject);
        }
    }
    public void full()
    {
        print("아이템이 가득참");
    }

    private void Update()
    {
        if(Input.GetKeyDown(invkey))
        {
            if(inving == true)
            {
                if(holdingitem)//중간에 닫음
                {
                    item[holditemindex] = itemtemp;
                    num[holditemindex] = numtemp;
                }
                inv.SetActive(false);
                inving = false;
                holdingitem = false;
                tempimg.GetComponent<RectTransform>().anchoredPosition = new Vector2(-253, -172);
                tooltipoff();
            }
            else
            {
                inv.SetActive(true);
                showitem();
                inving = true;
            }
        }

        var detect = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x + pos.x, gameObject.transform.position.y + pos.y), size, 0, LayerMask.GetMask("item"));
        if(detect)
        {
            detect.gameObject.GetComponent<item>().anim = false;
            detect.transform.position = Vector3.MoveTowards(detect.transform.position, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 1.2f), 0.1f);
        }

        if (holdingitem)
        {
            var m = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tempimg.transform.position = new Vector3(m.x, m.y, m.z +10);
        }

        if (tooltipon)
        {
            var m = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tooltipobj.transform.position = new Vector3(m.x + 1.1f, m.y - 1.1f, m.z + 10);
        }

        if(tooltipon && Input.GetMouseButtonDown(1))
        {
            useitem();
        }

        if (itemanim.inout && Input.GetMouseButtonDown(0))
        {
            moveitem(current);
        }

    }
    public void showitem()
    {
        for(int i = 0; i < item.Length; i++)
        {
            for(int a = 0; a < itemname.Length; a++)
            {
                if (item[i] == itemname[a] && num[i] > 0)
                {
                    invs[i].SetActive(true);
                    invs[i].transform.GetChild(0).GetComponent<Image>().sprite = img[a];
                    var n = invs[i].transform.GetChild(1);
                    n.GetComponent<TextMeshProUGUI>().text = num[i].ToString();
                    if (num[a] < 10)
                    {
                        n.GetComponent<RectTransform>().anchoredPosition = new Vector2(3, 0);
                    }
                    else if (num[a] > 10 && num[a] < 100)
                    {
                        n.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                    }
                    break;
                }
                else if(item[i] == "" || num[i] <= 0)
                {
                    invs[i].SetActive(false);
                    item[i] = "";
                    tooltipoff();
                }
            }
        }

    }
    public void useitem()
    {
        num[current]--;
        showitem();
    }
    public void moveitem(int i)
    {
        if(holdingitem)
        {
            if(item[i] != "")//아이템을 바꿈
            {
                var itemp = item[i];
                var ntemp = num[i];
                item[i] = itemtemp;
                num[i] = numtemp;
                itemtemp = itemp;
                numtemp = ntemp;
                tempimg.GetComponent<Image>().sprite = img[whatitem(itemtemp)];
                tooltip(current);
                showitem();
            }
            else//아이템을 놓음
            {
                current = i;
                item[i] = itemtemp;
                num[i] = numtemp;
                tooltip(current);
                showitem();
                holdingitem = false;
                tempimg.GetComponent<RectTransform>().anchoredPosition = new Vector2(-253, -172);
                tooltipon = true;
            }
        }
        else //아이템을 들음
        {
            if (item[i] == "")
            {
                return;
            }
            holditemindex = i;
            itemtemp = item[i];
            numtemp = num[i];
            item[i] = "";
            num[i] = 0;
            tempimg.GetComponent<Image>().sprite = img[whatitem(itemtemp)];
            showitem();
            holdingitem = true;
        }
    }

    public void tooltip(int c)
    {
        current = c;
        if (item[c] != "")
        {
            var name = tooltipobj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            var desc = tooltipobj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            var itemtemp = item[c];
            name.text = itemname[ whatitem(itemtemp) ];
            desc.text = itemdesc[ whatitem(itemtemp) ];
            tooltipon = true;
        }
    }
    public void tooltipoff()
    {
        tooltipon = false;
        tooltipobj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-272.3f, -113f);
    }

    private int whatitem(string itemtemp)
    {
        int i = 0;
        for (int a = 0; a < itemname.Length; a++)
        {
            if (itemtemp == itemname[a])
            {
                i = a;
                break;
            }
        }
        return i;
    }
}
