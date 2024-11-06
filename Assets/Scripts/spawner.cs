using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    GameObject folder;
    public GameObject prefeb;
    public float time;

    private void Start()
    {
        folder = GameObject.Find("Monsters");
        StartCoroutine("spawn");
    }
    IEnumerator spawn()
    {
        while(true)
        {
            var a = Instantiate(prefeb, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity, folder.transform);
            yield return new WaitForSeconds(time);

        }
    }
}
