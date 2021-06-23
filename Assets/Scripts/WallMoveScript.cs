using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMoveScript : MonoBehaviour
{
    public float moveSpeed;
    void Update()
    {
        transform.position += Vector3.right * (Time.deltaTime * moveSpeed);
		//moveSpeed += Time.deltaTime / 5;
	}
}
