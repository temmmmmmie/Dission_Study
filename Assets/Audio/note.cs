using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class note : MonoBehaviour
{
    public float speed;
    private void Start()
    {
        StartCoroutine("selfdes");
    }
    void Update()
    {
        transform.Translate(Vector2.right * speed);       
    }
    IEnumerator selfdes()
    {
        yield return new WaitForSeconds(0.5f);
        print("miss");
        Tilemove.t.misseff();
        Destroy(gameObject);
    }
}
