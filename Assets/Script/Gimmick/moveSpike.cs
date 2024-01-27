using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveSpike : MonoBehaviour
{
    [SerializeField]
    private GameObject spike;

    [SerializeField]
    private Transform startPos;
    [SerializeField]
    private Transform endPos;

    [SerializeField]
    private float speed;

    private bool isStart = false;

    void Start()
    {
        spike.transform.position = startPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart) return;
        if(spike != endPos)
        {
            //spike.position = Vector2.Lerp(startPos.position, endPos.position, 1);
            spike.transform.position = Vector3.Lerp(spike.transform.position, endPos.position, speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name.Equals("Player") &&!isStart)
        {
            isStart = true;
            spike.SetActive(true);
        }
    }
}
