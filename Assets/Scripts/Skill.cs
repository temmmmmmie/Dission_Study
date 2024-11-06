
using Spine.Unity;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public static Skill instence;
    public Vector3 mospos;
    [Header("정보")]
    public string[] skillname;
    public GameObject[] prefeb;
    public GameObject[] followobj;
    public KeyCode[] skillkey;
    [Space(40)]
    [Header("능력치")]
    public float[] damage;
    public float[] knockback;
    public float[] cooldown;
    public float[] curcool;
    public float[] mana;
    [Space(40)]
    [Header("범위")]
    public Vector2[] pos;
    public Vector2[] range;
    public Color[] gizcolor;

    float angle;
    Vector2 mouse;
    private Animation lanim;
    private Animator anim;
    private Rigidbody2D rigid;
    private Attack a;
    private void Awake()
    {
        Skill.instence = this;
    }

    private void Start()
    {
        lanim = gameObject.GetComponentInChildren<Animation>();
        anim = gameObject.GetComponent<Animator>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        a = Attack.instance;
    }
    private void Update()
    {
        if(inventory.inving)
        {
            return;
        }
        mospos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKey(skillkey[0]) && curcool[0] <= 0)
        {
            usemana();
            Move.freeze = true;
            lanim.Play();
            rigid.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
            anim.SetTrigger("Spear");
            curcool[0] = cooldown[0];
            StartCoroutine("cool0");
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            angle = Mathf.Atan2(gameObject.transform.position.y + 5 - mouse.y , gameObject.transform.position.x - mouse.x) * Mathf.Rad2Deg;
            Invoke("delay", 0.3f);
        }

        if(Input.GetKey(skillkey[1]) && curcool[1] <= 0)
        {
            usemana();
            curcool[1] = cooldown[1];
            anim.SetBool("fly", true);
            gameObject.transform.GetChild(6).GetComponent<Animation>().Play();
            Enemy.invin = true;
            Move.nodash = true;
            StartCoroutine(impect1(0));
        }

        if (Input.GetKey(skillkey[2]) && curcool[2] <= 0)
        {
            Move.freeze = true;
            anim.SetBool("holding", true);
        }
        if(Input.GetKeyUp(skillkey[2]) && curcool[2] <= 0)
        {
            usemana();
            Move.freeze = false;
            curcool[2] = cooldown[2];
            anim.SetBool("holding", false);
            StartCoroutine("cool2");
            impect(2);
        }
        if(Move.turn)
        {
            pos[2].x = -4;
        }
        else
        {
            pos[2].x = 4;
        }

        if(Input.GetAxis("Mouse ScrollWheel") < 0 && curcool[3] <= 0)
        {
            usemana();
            curcool[3] = cooldown[3];
            anim.SetTrigger("punish");
            var p =Instantiate(prefeb[1], new Vector2(mospos.x, 4), Quaternion.identity);
            followobj[3] = p;
            StartCoroutine("cool3");
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && curcool[4] <= 0)
        {
            usemana();
            curcool[4] = cooldown[4];
            anim.SetTrigger("spike");
            GameObject p;
            if(Move.turn)
            {
                p = Instantiate(prefeb[2], new Vector2(gameObject.transform.position.x - 4f, -2.5f), Quaternion.identity);
                p.GetComponent<SkeletonAnimation>().skeleton.ScaleX = -1;
            }
            else
            {
                p = Instantiate(prefeb[2], new Vector2(gameObject.transform.position.x + 4f, -2.5f), Quaternion.identity);
            }
            followobj[4] = p;
            StartCoroutine("cool4");
            impect(4);
            Destroy(p, 0.5f);
        }

        if (Input.GetKey(skillkey[5]) && curcool[5] <= 0)
        {
            usemana();
            curcool[5] = cooldown[5];
            StartCoroutine("cool5");
            anim.SetTrigger("spike2");
            Invoke("delay3", 0.45f);
        }
        if (Move.turn)
        {
            pos[5].x = -3;
        }
        else
        {
            pos[5].x = 3;
        }

        if (Input.GetKey(skillkey[6]) && curcool[6] <= 0)
        {
            usemana();
            curcool[6] = cooldown[6];
            StartCoroutine("cool6");
            anim.SetTrigger("sword");
            Invoke("delay2", 0.45f);
        }
    }
    void delay3()
    {
        impect(5);
    }
    void delay2()
    {
        GameObject s;
        if (Move.turn)
        {
            s = Instantiate(prefeb[3], new Vector2(gameObject.transform.position.x - 1, -4.6f), quaternion.identity);
            s.GetComponent<Sword>().turn = true;
            s.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            s = Instantiate(prefeb[3], new Vector2(gameObject.transform.position.x + 1, -4.6f), quaternion.identity);
        }
        followobj[6] = s;
    }
    void delay()
    {
        if (Move.turn)
        {
            followobj[0] = Instantiate(prefeb[0], new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 4), Quaternion.AngleAxis(angle - 90, Vector3.forward));
            pos[0].x = -1.26f;
        }
        else
        {
            followobj[0] = Instantiate(prefeb[0], new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 4), Quaternion.AngleAxis(angle - 90, Vector3.forward));
            pos[0].x = 1.86f;
        }
    }
    private void OnDrawGizmos()
    {
        if (followobj[0] != null)
        {
            Gizmos.color = gizcolor[0];
            Gizmos.DrawWireCube(new Vector3(followobj[0].transform.position.x + pos[0].x, followobj[0].transform.position.y + pos[0].y), range[0]);
        }
        if (followobj[3] != null)
        {
            Gizmos.color = gizcolor[3];
            Gizmos.DrawWireCube(new Vector3(followobj[3].transform.position.x + pos[3].x, followobj[3].transform.position.y + pos[3].y), range[3]);
        }
        if (followobj[4] != null)
        {
            Gizmos.color = gizcolor[4];
            Gizmos.DrawWireCube(new Vector3(followobj[4].transform.position.x + pos[4].x, followobj[4].transform.position.y + pos[4].y), range[4]);
        }
        if (followobj[6] != null)
        {
            Gizmos.color = gizcolor[6];
            Gizmos.DrawWireCube(new Vector3(followobj[6].transform.position.x + pos[6].x, followobj[6].transform.position.y + pos[6].y), range[6]);
        }
        Gizmos.color = gizcolor[1];
        Gizmos.DrawWireCube(new Vector3(followobj[1].transform.position.x + pos[1].x, followobj[1].transform.position.y + pos[1].y), range[1]);
        Gizmos.color = gizcolor[2];
        Gizmos.DrawWireCube(new Vector3(followobj[2].transform.position.x + pos[2].x, followobj[2].transform.position.y + pos[2].y), range[2]);
        Gizmos.color = gizcolor[5];
        Gizmos.DrawWireCube(new Vector3(followobj[5].transform.position.x + pos[5].x, followobj[5].transform.position.y + pos[5].y), range[5]);
    }

    public void impect(int a)
    {
        var target = Physics2D.OverlapBoxAll(new Vector3(followobj[a].transform.position.x + pos[a].x, followobj[a].transform.position.y + pos[a].y), range[a], 0);
        {
            for(int i = 0; i < target.Length; i++)
            {
                if (target[i].transform.tag == "monster")
                {
                    var crit = Dods_ChanceMaker.GetThisChanceResult_Percentage(Attack.instance.critchance);
                    target[i].GetComponent<Enemy>().hit(damage[a], knockback[a], crit);
                }
            }
        }
    }
    IEnumerator impect1(float a)
    {
        var target = Physics2D.OverlapBoxAll(new Vector3(followobj[1].transform.position.x + pos[1].x, followobj[1].transform.position.y + pos[1].y), range[1], 0);
        for (int i = 0; i < target.Length; i++)
        {
            if (target[i].transform.tag == "monster")
            {
                var crit = Dods_ChanceMaker.GetThisChanceResult_Percentage(Attack.instance.critchance);
                target[i].GetComponent<Enemy>().hit(damage[1], knockback[1], crit);
            }
        }
        yield return new WaitForSeconds(0.1f);
        a = a + 0.1f;
        if(a < 5)
        {
            StartCoroutine(impect1(a));
        }
        else
        {
            anim.SetBool("fly", false);
            Enemy.invin = false;
            Move.nodash = false;
            StartCoroutine("cool1");
        }
    }
    void usemana()
    {
        a.curmana -= mana[0];
        a.mcor();
    }
    #region cool
    IEnumerator cool0()
    {
        while(curcool[0] > 0)
        {
            yield return new WaitForSeconds(0.1f);
            curcool[0] = curcool[0] - 0.1f;
        }
    }
    IEnumerator cool1()
    {
        while (curcool[1] > 0)
        {
            yield return new WaitForSeconds(0.1f);
            curcool[1] = curcool[1] - 0.1f;
        }
    }
    IEnumerator cool2()
    {
        while (curcool[2] > 0)
        {
            yield return new WaitForSeconds(0.1f);
            curcool[2] = curcool[2] - 0.1f;
        }
    }
    IEnumerator cool3()
    {
        while (curcool[3] > 0)
        {
            yield return new WaitForSeconds(0.1f);
            curcool[3] = curcool[3] - 0.1f;
        }
    }

    IEnumerator cool4()
    {
        while (curcool[4] > 0)
        {
            yield return new WaitForSeconds(0.1f);
            curcool[4] = curcool[4] - 0.1f;
        }
    }

    IEnumerator cool5()
    {
        while (curcool[5] > 0)
        {
            yield return new WaitForSeconds(0.1f);
            curcool[5] = curcool[5] - 0.1f;
        }
    }

    IEnumerator cool6()
    {
        while (curcool[6] > 0)
        {
            yield return new WaitForSeconds(0.1f);
            curcool[6] = curcool[6] - 0.1f;
        }
    }
    #endregion
}
