using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balls : MonoBehaviour
{
    public GameObject Ball01;
    public GameObject Ball1;
  

    private void OnTriggerEnter(Collider other)
    {
        Ball01.SetActive(false);
        Ball1.SetActive(true);
    }

}
