using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    private Camera cam;
    private Rigidbody2D rigid;
    private Vector3 dir;
    public float delay;
    public AnimationClip[] animc;
    private bool c = false;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        rigid =  gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine("move");
    }
    IEnumerator move()
    {
        
        yield return new WaitForSeconds(delay);
        dir = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, -10, -cam.transform.position.z));
        while(!c)
        {
            yield return null;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, dir, 0.5f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "land")
        {
            c = true;
            StopCoroutine("move");
            Attack.instance.player.GetComponent<Skill>().impect(0);
            var anim = gameObject.GetComponent<Animation>();
            anim.clip = animc[1];
            anim.Play();
            gameObject.GetComponentInChildren<ParticleSystem>().Emit(200);
            Attack.instance.shake.Camerashake();
            Destroy(gameObject, 1);
            Move.freeze = false;
        }
    }
}
