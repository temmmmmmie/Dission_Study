using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemanim : MonoBehaviour
{
    RectTransform trans;
    public static bool inout;
    private void Start()
    {
       trans = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
    }
    public void mouseup()
    {   
        if(trans)
        {
            trans.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
    }
    public void mousedown()
    {
        if(trans)
        {
            trans.localScale = new Vector3(1, 1, 1);

        }
    }

    public void inmouse()
    {
        inout = true;
        int i = 0;
        switch (gameObject.name)
        {
            case "showitem1":
                i = 0;
                break;
            case "showitem2":
                i = 1;
                break;
            case "showitem3":
                i = 2;
                break;
            case "showitem4":
                i = 3;
                break;
            case "showitem5":
                i = 4;
                break;
            case "showitem6":
                i = 5;
                break;
            case "showitem7":
                i = 6;
                break;
            case "showitem8":
                i = 7;
                break;
            case "showitem9":
                i = 8;
                break;
            case "showitem10":
                i = 9;
                break;
            case "showitem11":
                i = 10;
                break;
            case "showitem12":
                i = 11;
                break;
            case "showitem13":
                i = 12;
                break;
            case "showitem14":
                i = 13;
                break;
            case "showitem15":
                i = 14;
                break;
            case "showitem16":
                i = 15;
                break;
        }
        Attack.instance.player.GetComponent<inventory>().current = i;
        if (trans)
        {           
            Attack.instance.player.GetComponent<inventory>().tooltip(i);
        }
    }
    public void outmouse()
    {
        inout = false;
        Attack.instance.player.GetComponent<inventory>().tooltipoff();
    }
}
