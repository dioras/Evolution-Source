using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPivotsMove : MonoBehaviour
{
    public GameObject[] pivots;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public GameObject GetPivot()
    {
      return pivots[Random.Range(0, pivots.Length)];
    }
}
