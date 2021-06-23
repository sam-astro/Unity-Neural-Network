using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbsCollidingOrNot : MonoBehaviour
{
    public bool isColliding;
    public bool failed;

    public string tagName;

	public int collidingInt;

    void OnCollisionEnter(Collision collision)
	{
        if(collision.gameObject.tag == tagName)
		{
			isColliding = true;
			collidingInt = 1;
		}
		else if (collision.gameObject.tag == "Danger")
		{
			failed = true;
		}
	}

    void OnCollisionExit(Collision collision)
	{
        if(collision.gameObject.tag == tagName)
		{
			isColliding = false;
			collidingInt = 0;
		}
	}
}
