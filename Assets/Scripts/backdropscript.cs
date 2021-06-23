using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backdropscript : MonoBehaviour
{
    public float moveSpeed;

    void Update()
    {
        transform.position += new Vector3(Time.deltaTime * moveSpeed, 0, 0);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Remove")
        {
            transform.position = transform.position + new Vector3(22, 0, 0);
        }
    }
}
