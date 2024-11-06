using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punish : MonoBehaviour
{
    bool c;
    void Update()
    {
        if(!c)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector2(gameObject.transform.position.x, -10), 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "land")
        {
            c = true;
        }
        Skill.instence.impect(3);
        gameObject.GetComponent<Animator>().SetTrigger("end");
        Destroy(gameObject, 0.5f);
        gameObject.transform.GetChild(5).GetComponent<ParticleSystem>().Emit(10);
    }
}
