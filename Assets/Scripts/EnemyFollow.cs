using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public GameObject player;
    public int monsterspeed;
    public int monstermaxspeed;
    public float dir;
    private Vector2 dirvec;
    public static bool detect;
    Rigidbody2D rigid;
    SpriteRenderer rend;
    Animator anim;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigid = gameObject.GetComponent<Rigidbody2D>();
        rend = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        dir = Mathf.Abs(gameObject.transform.position.x) - Mathf.Abs(player.transform.position.x);
        if (dir >= 0) //왼쪽
        {
            dirvec = Vector2.left * monsterspeed;
            rend.flipX = false;
        }
        else //오른쪽
        {
            dirvec = Vector2.right * monsterspeed;
            rend.flipX = true;
        }
        if(rigid.velocity.x >= monstermaxspeed)
        {
            rigid.velocity = new Vector2(monstermaxspeed, rigid.velocity.y);
        }

        if(detect)
        {
            anim.SetBool("walking", true);
            rigid.AddForce(dirvec);
        }
        else
        {
            anim.SetBool("walking", false);
        }
    }
}
