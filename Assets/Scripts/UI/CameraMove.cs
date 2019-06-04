using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject player;

    private Vector3 dir;

    private float delta_x;

    private void Awake()
    {
        delta_x = transform.position.x - player.transform.position.x;
    }

    private void LateUpdate()
    {
        dir = player.transform.position - transform.position;
        if (dir.x >= 4)
        {
            SmoothMove();
        }
    }

    private void SmoothMove()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        for(int i = 0; i < 100; ++i)
        {
            transform.position = new Vector3(transform.position.x + delta_x / 100, transform.position.y, transform.position.z);
            yield return null;
        }
    }
}
