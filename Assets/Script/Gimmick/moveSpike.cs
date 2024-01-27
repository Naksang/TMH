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

    void Start()
    {
        spike.transform.position = startPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(spike != endPos)
        {
            //spike.position = Vector2.Lerp(startPos.position, endPos.position, 1);
            spike.transform.position = Vector3.Lerp(spike.transform.position, endPos.position, 0.1f);
        }
     
    }
}
