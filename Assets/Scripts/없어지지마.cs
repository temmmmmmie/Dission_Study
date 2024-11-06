using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 없어지지마 : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
