using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFinder : MonoBehaviour
{
    #region  Singleton

        public static playerFinder instance;
        void Awake()
        {
            instance = this;
        }
    
    #endregion

    public GameObject player;
}
