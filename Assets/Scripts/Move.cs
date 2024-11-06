using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private Rigidbody2D rigid;
    private SkeletonAnimation spineanim;
    private Animator anim;
    public GameObject ping;
    [Space(50)]
    [Header("이동")]
    public static bool freeze;
    public static bool turn;
    public static bool nodash;
    public KeyCode right;
    public KeyCode left;
    public int speed;
    public int maxspeed;
    public float dashcool;
    public float curcool;
    [Range(-1, 1)]
    public int curstate;
    public float pingdistance;
    [Space(50)]
    [Header("점프")]
    public KeyCode jump;
    public int jumppower;
    public int jumpnum;
    public int curjump;
    public bool isjumping;
    private Vector2 boxtemp;
    private int clicknuml;
    private float firstclicktimel;
    private float secondclicktimel;
    private int clicknumr;
    private float firstclicktimer;
    private float secondclicktimer;

    private bool pingstart;
    private bool noright;
    private bool noleft;
    private void Start()
    {
        rigid = player.GetComponent<Rigidbody2D>();
        curjump = jumpnum;
        spineanim = GetComponent<SkeletonAnimation>();
        anim = player.GetComponent<Animator>();
        boxtemp.x = gameObject.GetComponent<Attack>().boxpos.x;
    }
    // Update is called once per frame
    void Update()
    {
        if(freeze)
        {
            return;
        }
        #region doubleclick
        if (Input.GetKeyDown(left) && clicknuml == 0 && nodash == false) //first
        {
            firstclicktimel = Time.time;            
            clicknuml ++;
        }
        else if(Input.GetKeyDown(left) && clicknuml > 0 && curcool <= 0)
        {
            secondclicktimel = Time.time;
            if (clicknuml == 1 && secondclicktimel - firstclicktimel < 0.7f)
            {
                rigid.AddForce(Vector2.left * speed * 2, ForceMode2D.Impulse);
                anim.SetTrigger("dash");
                gameObject.transform.GetChild(4).GetComponent<ParticleSystem>().Emit(50);
                gameObject.transform.GetChild(5).GetComponent<Animation>().Play();
                curcool = dashcool;
                StartCoroutine(dashcooltimer());
                clicknuml = 0;
                firstclicktimel = 0;
                secondclicktimel = 0;
            }
            else
            {
                clicknuml = 0;
                firstclicktimel = 0;
                secondclicktimel = 0;
            }
        }

        if (Input.GetKeyDown(right) && clicknumr == 0 && nodash == false) //first
        {
            firstclicktimer = Time.time;
            clicknumr++;
        }
        else if (Input.GetKeyDown(right) && clicknumr > 0 && curcool <= 0)
        {
            secondclicktimer = Time.time;
            if (clicknumr == 1 && secondclicktimer - firstclicktimer < 0.7f)
            {
                rigid.AddForce(Vector2.right * speed * 2, ForceMode2D.Impulse);
                anim.SetTrigger("dash");
                gameObject.transform.GetChild(4).GetComponent<ParticleSystem>().Emit(50);
                gameObject.transform.GetChild(5).GetComponent<Animation>().Play();
                curcool = dashcool;
                StartCoroutine(dashcooltimer());
                clicknumr = 0;
                firstclicktimer = 0;
                secondclicktimer = 0;
            }
            else
            {
                clicknumr = 0;
                firstclicktimer = 0;
                secondclicktimer = 0;
            }
        }
        #endregion
        if(Input.GetMouseButton(0))
        {
            var m = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ping.transform.position = new Vector2(m.x, -4.3f);
            pingstart = true;
        }
        if(pingstart)
        {
            var detect = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x + Attack.instance.boxpos.x, gameObject.transform.position.y + Attack.instance.boxpos.y), Attack.instance.boxsize, 0, LayerMask.GetMask("monster"));
            if (detect)
            {
                if(detect.transform.position.x - gameObject.transform.position.x < 0) //left
                {
                    noright = false;
                    noleft = true;
                }
                else //right
                {
                    noleft = false;
                    noright = true;
                }
                if((ping.transform.position.x - gameObject.transform.position.x > 0 && noright) || (ping.transform.position.x - gameObject.transform.position.x < 0 && noleft)) //goleft!
                {
                    Attack.instance.simpatk();
                }
                pingstart = false;
            }
            else
            {
                noleft = false;
                noright = false;
            }
            pingdistance = ping.transform.position.x - gameObject.transform.position.x;
            if (pingdistance > 2 && noright == false) //right
            {
                turn = false;
                gameObject.GetComponent<Attack>().boxpos.x = boxtemp.x;
                spineanim.skeleton.ScaleX = -1;
                anim.SetFloat("state", 1);
                rigid.AddForce(Vector2.right * speed);
                limit();
            }
            else if(pingdistance < -2 && noleft == false) //left
            {
                turn = true;
                gameObject.GetComponent<Attack>().boxpos.x = -boxtemp.x;
                spineanim.skeleton.ScaleX = 1;
                anim.SetFloat("state", 1);
                rigid.AddForce(Vector2.left * speed);
                limit();
            }
            else
            {
                pingstart = false;
                ping.transform.position = new Vector2(0.28f, -9.46f);

            }

        }
        if (Input.GetKey(left))
        {
            turn = true;
            gameObject.GetComponent<Attack>().boxpos.x = - boxtemp.x;
            spineanim.skeleton.ScaleX = 1;
            anim.SetFloat("state", 1);
            rigid.AddForce(Vector2.left * speed);
            limit();
        }
        else if (Input.GetKey(right))
        {
            turn = false;
            gameObject.GetComponent<Attack>().boxpos.x = boxtemp.x;
            spineanim.skeleton.ScaleX = -1;
            anim.SetFloat("state", 1);
            rigid.AddForce(Vector2.right * speed);
            limit();
        }
        else if(!pingstart)
        {
            anim.SetFloat("state", 0);
            curstate = 0;
        }
        if(Input.GetKeyDown(jump))
        {
            anim.SetBool("jumping", true);
            if(curjump > 0)
            {
                rigid.AddForce(Vector2.up * jumppower, ForceMode2D.Impulse);
                curjump--;
            }
        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "land" || collision.gameObject.tag == "monster")
        {
            anim.SetBool("jumping", false);
            isjumping = false;
            curjump = jumpnum;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "land" || collision.gameObject.tag == "monster")
        {
            anim.SetBool("jumping", true);
            isjumping = true;
        }
    }
    void limit()
    {
        if (rigid.velocity.x >= maxspeed)
        {
            rigid.velocity = new Vector2(maxspeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x <= -maxspeed)
        {
            rigid.velocity = new Vector2(-maxspeed, rigid.velocity.y);
        }
    }

    IEnumerator dashcooltimer()
    {
        while(curcool > 0)
        {
            yield return new WaitForSeconds(0.1f);
            curcool -= 0.1f;
        }
    }
}
