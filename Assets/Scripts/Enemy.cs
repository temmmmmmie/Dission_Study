
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public static bool invin;
    private GameObject player;
    private Rigidbody2D rigid;
    private SpriteRenderer healthimage;
    public GameObject effect;
    public GameObject healthbar;
    public GameObject showdamage;
    public GameObject particle;
    [Space(50)]
    [Header("health")]
    public float maxhealth;
    public float health;
    [Space(50)]
    [Header("stats")]
    public float damage;
    public float knockback;
    private float dir;
    [Space(50)]
    [Header("item")]
    public GameObject[] item;
    public float[] percent;

    private void Start()
    {
       health = maxhealth;
       player = GameObject.FindGameObjectWithTag("Player");
       rigid = gameObject.GetComponent<Rigidbody2D>();
       healthimage = healthbar.GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        dir = gameObject.transform.position.x - player.transform.position.x;
    }
    private void OnCollisionEnter2D(Collision2D collision) //플레이어 맞음
    {
        if(invin)
        {
            return;
        }
        if(collision.transform.tag == "Player")
        {
            var attack = player.GetComponent<Attack>();
            attack.curhealth -= damage;
            attack.cor();
            if (dir < 0)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(knockback, knockback), ForceMode2D.Impulse);
            }
            else
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-knockback, knockback), ForceMode2D.Impulse);
            }
        }
    }

    public void hit(float damage, float knockback, bool iscrit) //몬스터 맞음
    {
        if(iscrit)
        {
            health -= damage * 2;
        }
        else
        {
            health -= damage;
        }
        healthimage.size = new Vector2( health / maxhealth, 1);
        StartCoroutine("stop");
        if(iscrit)
        {
            Attack.instance.shake.Camerashake();    

        }
        effect.GetComponent<Animation>().Play();
        var dam = Instantiate(showdamage, gameObject.transform);
        var damtext = dam.GetComponent<TextMeshPro>();
        if(iscrit)
        {
            string what = "Crit! ";
            damtext.faceColor = Color.red;
            damtext.text = what + damage.ToString();

        }
        else
        {
            damtext.text = damage.ToString();
        }
        dam.GetComponent<Animation>().Play();
        Destroy(dam, 3);
        if (health <= 0)
        {
            var par = Instantiate(particle, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
            par.GetComponent<ParticleSystem>().Emit(10);
            Destroy(gameObject);
            if(item.Length > 0)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    var per = Dods_ChanceMaker.GetThisChanceResult_Percentage(percent[i]);
                    if (per)
                    {
                        var tem = Instantiate(item[i], new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1), Quaternion.identity);
                        var name = tem.GetComponent<item>().itemname;
                        tem.name = name;
                        tem.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-2, 2) * 100, 200));
                    }
                }
            }

            Destroy(par, 0.5f);
        }
        var healthanim = healthbar.GetComponent<Animation>();
        healthanim.Play();
        if (dir < 0)
        {
            rigid.AddForce(new Vector2(-knockback, knockback * 0.5f), ForceMode2D.Impulse);

        }
        else
        {
            rigid.AddForce(new Vector2(knockback, knockback * 0.5f), ForceMode2D.Impulse);
        }
    }

    IEnumerator stop()
    {
        if(gameObject.GetComponent<EnemyMove>() != null)
        {
            var move = gameObject.GetComponent<EnemyMove>();
            move.enabled = false;
            yield return new WaitForSeconds(1);
            move.enabled = true;
        }
        else if(gameObject.GetComponent<EnemyFollow>() != null)
        {
            var follow = gameObject.GetComponent<EnemyFollow>();
            follow.enabled = false;
            yield return new WaitForSeconds(1);
            follow.enabled = true;
        }
    }
}
