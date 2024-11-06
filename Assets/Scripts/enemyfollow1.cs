using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyfollow1 : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            EnemyFollow.detect = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyFollow.detect = false;
    }
}
