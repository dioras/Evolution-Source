﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class removeRender : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
