using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDownDamage : MonoBehaviour
{

    public float heigt = 10;
    private float dir = -1;
    public float speed = 1;
    private float currMove = 0;

    private void Update()
    {
        float delata = Time.deltaTime * speed;
        transform.position = new Vector3(transform.position.x, transform.position.y + delata * dir);
        currMove += delata;
        if(currMove >= heigt)
        {
            currMove = 0;
            dir *= -1;
        }
    }

    IEnumerator Move()
    {
        for (int i = 0; i < speed; ++i)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - dir * heigt / speed);
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        dir *= -1;
    }
}
