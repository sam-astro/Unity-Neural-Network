using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
    public float moveSpeed;

    void Update()
    {
        transform.position += new Vector3(Time.deltaTime * moveSpeed, 0, 0);
    }

    void OnCollisionStay2D(Collision2D collision)
	{
        if(collision.gameObject.tag == "Remove")
		{
            transform.position = new Vector2(19f, Random.Range(0.5f, 4.5f));
		}
	}
}
