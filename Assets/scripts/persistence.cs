using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class persistence : MonoBehaviour
{
    


    void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject); // Prevents duplicates
            return;
        }

        DontDestroyOnLoad(gameObject);
    }


}
