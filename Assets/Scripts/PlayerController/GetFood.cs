using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFood : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("tigger" + other.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision" + collision.other.name);
    }
}
