
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    public bool noai;
    public int nextMove;           
    public float monsterspeed;
    public float range;
    public int monsterjump;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke("Think", 2);
        Invoke("Jump", 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(noai)
        {
            return;
        }
        rigid.velocity = new Vector2(nextMove * monsterspeed, rigid.velocity.y);
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * monsterspeed * range, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));      
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("land"));
        if (rayHit.collider == null)
        {
            Turn();

        }
    }

    void Jump()
    {
        if(nextMove != 0)
        {
            rigid.AddForce(Vector2.up * monsterjump, ForceMode2D.Impulse);
        }      
        Invoke("Jump", 1);
    }
    void Think()
    {
        nextMove = Random.Range(-1, 2);
        if (nextMove != 0)
        {
            anim.SetBool("walking", true);
            spriteRenderer.flipX = (nextMove == 1);
        }
        else
        {
            anim.SetBool("walking", false);
        }
        float nextThinkTime = Random.Range(2f, 5f);    
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove = nextMove * (-1);
        spriteRenderer.flipX = (nextMove == 1); 
        CancelInvoke();
        Invoke("Think", 2);
        Invoke("Jump", 1);
    }
}
