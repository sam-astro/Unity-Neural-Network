using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingWallOrApple : MonoBehaviour
{
	[HideInInspector]
    public bool touchingWall;
	[HideInInspector]
	public bool touchingApple;

    void OnCollisionStay2D(Collision2D collision)
	{
        if(collision.gameObject.tag == "Danger")
		{
			touchingWall = true;
			touchingApple = false;
		}
		else if (collision.gameObject.tag == "Apple")
		{
			touchingWall = false;
			touchingApple = true;
		}
		else if (collision.gameObject.tag == "Body")
		{
			if (collision.gameObject.transform.parent == transform.parent)
			{
				touchingWall = true;
				touchingApple = false;
			}
		}
	}
}
