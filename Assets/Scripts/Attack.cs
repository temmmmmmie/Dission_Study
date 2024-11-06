using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    public static Attack instance;
    public GameObject player;
    public GameObject healthui;
    public GameObject manaui;
    public GameObject bullet;
    public GameObject bulleyfac;
    public GameObject[] bulleteffect;
    private Animator anim;
    [HideInInspector]
    public SimpleCameraShakeInCinemachine shake;
    public KeyCode attackkey;
    public Vector2 boxpos;
    public Vector2 boxsize;
    [Space(50)]
    [Header("Sword")]
    public float damage;
    public float knockback;
    public float critchance;
    public float speed;
    public float curspeed;
    [Space(50)]
    [Header("Bullet")]
    public float bulletdam;
    public KeyCode shootkey;
    public float bulletpower;
    public float bulletspeed;
    public float bulletcurspeed;
    public float destroytiime;
    [Space(20)]
    [Header("health")]
    public float maxhealth;
    public float curhealth;
    private float temp;
    [Header("mana")]
    public float maxmana;
    public float curmana;
    private float mtemp;

    private void Awake()
    {
        Attack.instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        shake = gameObject.GetComponent<SimpleCameraShakeInCinemachine>();
        curhealth = maxhealth;
        curmana = maxmana;
        temp = maxhealth;
        mtemp = maxmana;
    }

    // Update is called once per frame
    void Update()
    {
        var detect = Physics2D.OverlapBoxAll(new Vector2(gameObject.transform.position.x + boxpos.x, gameObject.transform.position.y + boxpos.y), boxsize, 0);
        if (Input.GetKey(attackkey))
        {
            if (curspeed <= 0)
            {
                anim.SetBool("attack", true);
                for (int i = 0; i < detect.Length; i++) //타격
                {
                    StartCoroutine(delay(detect[i]));
                }
                Invoke("holy", 0.4f);
                curspeed = speed;
                StartCoroutine("timer");
            }
        }
        if (Input.GetKey(shootkey))
        {
            anim.SetBool("shooting", true);
            Move.freeze = true;
            StartCoroutine("shootdelay");
        }
        if(Input.GetKeyUp(shootkey))
        {
            StopCoroutine("shootdelay");
            anim.SetBool("shooting", false);
            Move.freeze = false;
        }
    }

    public void simpatk()
    {
        var detect = Physics2D.OverlapBoxAll(new Vector2(gameObject.transform.position.x + boxpos.x, gameObject.transform.position.y + boxpos.y), boxsize, 0);
        if (curspeed <= 0)
        {
            anim.SetBool("attack", true);
            for (int i = 0; i < detect.Length; i++) //타격
            {
                StartCoroutine(delay(detect[i]));
            }
            Invoke("holy", 0.4f);
            curspeed = speed;
            StartCoroutine("timer");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x + boxpos.x, gameObject.transform.position.y + boxpos.y), boxsize);
    }
    IEnumerator timer()
    {
        while(curspeed > 0)
        {
            yield return new WaitForSeconds(0.1f);
            curspeed -= 0.1f;
        }
    }
    IEnumerator btimer()
    {
        while (bulletcurspeed > 0)
        {
            yield return new WaitForSeconds(0.1f);
            bulletcurspeed -= 0.1f;
        }
    }
    void holy()
    {
        anim.SetBool("attack", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "monster")
        {
            shake.Camerashake();
        }
    }

    IEnumerator delay(Collider2D detect)
    {
        if (detect.gameObject.tag == "monster")
        {
            var crit = Dods_ChanceMaker.GetThisChanceResult_Percentage(critchance);
            yield return new WaitForSeconds(0.4f);
            detect.gameObject.GetComponent<Enemy>().hit(damage, knockback, crit);
        };
    }

    IEnumerator shootdelay()
    {
        yield return new WaitForSeconds(0.3f);
        if (bulletcurspeed <= 0)
        {
            var particle = bulleteffect[0].GetComponent<ParticleSystem>();
            var b = Instantiate(bullet, new Vector3(bulleyfac.transform.position.x, bulleyfac.transform.position.y, bulleyfac.transform.position.z), Quaternion.identity, bulleyfac.transform);
            bulletcurspeed = bulletspeed;
            StartCoroutine("btimer");
            if (Move.turn)
            {
                bulleteffect[0].transform.localPosition = new Vector2(-2.57f, 1.25f);
                bulleyfac.transform.localPosition = new Vector2(-3.8f, 1.25f);
                b.GetComponent<SkeletonAnimation>().skeleton.ScaleX = 1;
                b.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletpower, ForceMode2D.Impulse);

            }
            else
            {
                bulleteffect[0].transform.localPosition = new Vector2(2.57f, 1.25f);
                bulleyfac.transform.localPosition = new Vector2(3.8f, 1.25f);
                b.GetComponent<SkeletonAnimation>().skeleton.ScaleX = -1;
                b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletpower, ForceMode2D.Impulse);

            }
            particle.Emit(20);
            yield return new WaitForSeconds(0.1f);
            Destroy(b, destroytiime);
        }
    }
    public void cor()
    {
        StartCoroutine("healthanim");
    }
    IEnumerator healthanim()
    {
        while(!Mathf.Approximately(temp, curhealth))
        {
            temp = Mathf.Lerp(curhealth, temp, 0.1f);
            healthui.GetComponent<Image>().fillAmount = temp / maxhealth;
            yield return new WaitForSeconds(1);
        }
    }
    public void mcor()
    {
        StartCoroutine("manaanim");
    }
    IEnumerator manaanim()
    {
        while (!Mathf.Approximately(temp, curmana))
        {
            mtemp = Mathf.Lerp(curmana, mtemp, 0.1f);
            manaui.GetComponent<Image>().fillAmount = mtemp / maxmana;
            yield return new WaitForSeconds(1);
        }
    }
}
