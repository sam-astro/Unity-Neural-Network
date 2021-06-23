using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BirdScript : MonoBehaviour
{
    float timeBtwnFlaps;
	public float timeElapsed;
	public bool failed;

	private NeuralNetwork net;

	public Transform sensorA, sensorB, sensorC;
	public float howFarAwayA, howFarAwayB;

	public float nextPipeHeight;
	public float nextPipeDistance;

	public LayerMask dangerLayer;
	public LayerMask pipeLayer;

	public TMP_Text scoreText;
	public TMP_Text outputText;

	Vector3 dirA, dirB, dirC;

	public int pressingButton;

	float fixPressingButton = 0.25f;

	float internalTimer = 1f;

	void Update()
    {
		#region sensors
		dirA = (this.transform.position - sensorA.position).normalized;
		dirB = (this.transform.position - sensorB.position).normalized;
		dirC = (this.transform.position - sensorC.position).normalized;

		if(Physics2D.Raycast(transform.position, transform.position + dirA, Mathf.Infinity, dangerLayer))
		{
			howFarAwayA = Vector2.Distance(transform.position, Physics2D.Raycast(transform.position, transform.position + dirA, Mathf.Infinity, dangerLayer).point);
		}

		if(Physics2D.Raycast(transform.position, transform.position + dirB, Mathf.Infinity, dangerLayer))
		{
			howFarAwayB = Vector2.Distance(transform.position, Physics2D.Raycast(transform.position, transform.position + dirB, Mathf.Infinity, dangerLayer).point);
		}

		if(Physics2D.Raycast(transform.position, transform.position - dirC, Mathf.Infinity, pipeLayer).collider)
		{
			nextPipeHeight = Physics2D.Raycast(transform.position, transform.position - dirC, Mathf.Infinity, pipeLayer).collider.gameObject.transform.parent.transform.position.y;
			nextPipeDistance = Vector2.Distance(transform.position, Physics2D.Raycast(transform.position, transform.position - dirC, Mathf.Infinity, pipeLayer).point);
		}
		else
		{
			nextPipeHeight = 0;
		}

		#endregion

		if (pressingButton == 1)
		{
			fixPressingButton -= Time.deltaTime;
		}

		if (fixPressingButton <= 0)
		{
			pressingButton = 0;
			fixPressingButton = 0.25f;
		}

		if (GetComponent<Rigidbody2D>().velocity.y > 0)
		{
			if (transform.rotation.z < 90)
			{
				transform.eulerAngles = new Vector3(0, 0, Time.deltaTime * Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) * 350);
			}
		}
		else if (GetComponent<Rigidbody2D>().velocity.y < 0)
		{
			if (transform.rotation.z > -90)
			{
				transform.eulerAngles = new Vector3(0, 0, -Time.deltaTime * Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) * 350);
			}
		}

		if (!failed)
		{
			if (internalTimer > 0)
			{
				internalTimer -= Time.deltaTime;
			}
			else
			{
				internalTimer = 1;
			}

			timeElapsed += Time.deltaTime;
			//scoreText.text = Mathf.Round(timeElapsed).ToString();
			scoreText.text = " ";

			float[] inputs = new float[7];

			inputs[0] = nextPipeDistance;
			inputs[1] = (1.6f + nextPipeHeight) - transform.position.y - 0.28f;
			inputs[2] = (-1.6f + nextPipeHeight) - transform.position.y + 0.28f;
			inputs[3] = GetComponent<Rigidbody2D>().velocity.y;
			inputs[4] = howFarAwayA;
			inputs[5] = howFarAwayB;
			inputs[6] = internalTimer;

			float[] output = net.FeedForward(inputs);

			//outputText.text = output[0].ToString();
			outputText.text = " ";

			if (output[0] > 0.5f && output[0] < 1.5f)
			{
				pressingButton = 1;
			}

			if (timeBtwnFlaps <= 0)
			{
				if (output[0] > 0.5f && output[0] < 1.5f)
				{
					//GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5, ForceMode2D.Impulse);
					GetComponent<Rigidbody2D>().velocity = Vector2.up * 10;
					timeBtwnFlaps = 0.25f;
				}
			}
			else
			{
				timeBtwnFlaps -= Time.deltaTime;
			}

			net.SetFitness((timeElapsed - Vector2.Distance(transform.position, Physics2D.Raycast(transform.position, transform.position - dirC, Mathf.Infinity, pipeLayer).point)) + 10);
		}
		else
		{
			GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.1f);
			GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
		}
	}

	public void Init(NeuralNetwork net, Transform target)
	{
		this.net = net;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Danger")
		{
			GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
			failed = true;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawLine(transform.position, transform.position + dirA * 10);
		Gizmos.DrawLine(transform.position, transform.position + dirB * 10);
		Gizmos.DrawLine(transform.position, transform.position - dirC * 10);
	}
}
