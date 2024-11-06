using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public GameObject player;
    public ParticleSystem particle;
    private Attack p;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        p =  player.GetComponent<Attack>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "monster")
        {
            bool iscrit = Dods_ChanceMaker.GetThisChanceResult_Percentage(p.critchance);
            collision.GetComponent<Enemy>().hit(p.bulletdam, p.knockback, iscrit);
            particle.Emit(20);
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        var eff = Instantiate( Attack.instance.bulleteffect[2], new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
        Destroy(eff, 0.3f);

    }
}
