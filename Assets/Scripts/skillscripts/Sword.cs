using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public bool turn;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(move(0.5f));
    }
    IEnumerator move(float a)
    {
        if (turn)
        {
            while(a > 0)
            {
                gameObject.transform.Translate(Vector2.left* 25 * Time.deltaTime);
                yield return null;
                a -= 0.01f;
                if (a < 0.1f)
                {
                    gameObject.GetComponent<Animator>().SetTrigger("end");

                }
            }

        }
        else
        {
            while(a > 0)
            {
                gameObject.transform.Translate(Vector2.right* 25 * Time.deltaTime);
                yield return null;
                a -= 0.01f;
                if(a < 0.1f)
                {
                    gameObject.GetComponent<Animator>().SetTrigger("end");

                }
            }

        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "monster")
        {
            var crit = Dods_ChanceMaker.GetThisChanceResult_Percentage(Attack.instance.critchance);
            collision.GetComponent<Enemy>().hit(Skill.instence.damage[6], Skill.instence.knockback[6], crit);
        }
    }
}
