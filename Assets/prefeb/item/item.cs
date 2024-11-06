using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public string itemname;
    Vector2 init;
    public bool anim;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (anim)
        {
            gameObject.transform.position = new Vector2(init.x, init.y + (Mathf.Sin(Time.time) + 0.5f) * 0.1f);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "land" || collision.transform.tag == "Player")
        {
            gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
            Destroy(gameObject.GetComponent<Rigidbody2D>());
            init = gameObject.transform.position;
            anim = true;

        }
    }
}
