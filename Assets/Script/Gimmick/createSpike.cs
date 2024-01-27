using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createSpike : MonoBehaviour
{
    [SerializeField]
    private GameObject area;
    [SerializeField]
    private GameObject spike;

    private void Awake()
    {
        area = GetComponent<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Player")) 
        {
            spike.SetActive(true);
        }
    }
}
