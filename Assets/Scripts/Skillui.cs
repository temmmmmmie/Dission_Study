using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skillui : MonoBehaviour
{
    public GameObject[] skills;
    public GameObject[] ani;
    private Move m;
    // Start is called before the first frame update
    void Start()
    {
        m = Attack.instance.player.GetComponent<Move>();
    }

    // Update is called once per frame
    void Update()
    {
        skills[0].GetComponent<Image>().fillAmount = m.curcool / m.dashcool;
        if(m.curcool <= 0 )
        {
            ani[0].GetComponent<Animator>().SetBool("complete", true);
        }
        else
        {
            ani[0].GetComponent<Animator>().SetBool("complete", false);
        }
        for(int i = 1; i < skills.Length; i++)
        {
            skills[i].GetComponent<Image>().fillAmount = Skill.instence.curcool[i-1] / Skill.instence.cooldown[i-1];
            if(Skill.instence.curcool[i - 1] <= 0)
            {
                ani[i].GetComponent<Animator>().SetBool("complete", true);
            }
            else
            {
                ani[i].GetComponent<Animator>().SetBool("complete", false);
            }
        }
    }
}
