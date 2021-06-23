using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SnakeScript : MonoBehaviour
{
	public float timeElapsed;
	public bool failed;

	private NeuralNetwork net;

	public Transform sensorA, sensorB, sensorC, sensorD, sensorE, sensorF, sensorG, sensorH;
	public float howFarAwayA, howFarAwayB, howFarAwayC, howFarAwayD, howFarAwayE, howFarAwayF, howFarAwayG, howFarAwayH;
	public float distanceFromBodyA, distanceFromBodyB, distanceFromBodyC, distanceFromBodyD, distanceFromBodyE, distanceFromBodyF, distanceFromBodyG, distanceFromBodyH;
	public float distanceFromAppleA, distanceFromAppleB, distanceFromAppleC, distanceFromAppleD, distanceFromAppleE, distanceFromAppleF, distanceFromAppleG, distanceFromAppleH;

	Vector3 dirA, dirB, dirC, dirD, dirE, dirF, dirG, dirH;

	public List<GameObject> bodyParts;
	public Vector3[] partLocationLastTick;

	Transform nextApple;
	public GameObject applePrefab;

	public Vector2 boardSize;

	public LayerMask dangerLayer;
	public LayerMask appleLayer;
	public LayerMask bodyLayer;

	public float maxTickSpeed = 0.7f;
	float tickSpeed = 0.01f;

	public GameObject head;

	public float headRot;

	public int movesThisTick = 1;

	SnakeManager snakeManager;

	int applesCollected;

	void Awake()
	{
		snakeManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<SnakeManager>();
	}

	void Start()
	{
		for (int i = 0; i < bodyParts.Count; i++)
		{
			partLocationLastTick[i] = bodyParts[i].transform.position;
		}
	}

	void Update()
    {
		#region sensors
		dirA = (head.transform.position - sensorA.position).normalized;
		dirB = (head.transform.position - sensorB.position).normalized;
		dirC = (head.transform.position - sensorC.position).normalized;
		dirD = (head.transform.position - sensorD.position).normalized;
		dirE = (head.transform.position - sensorE.position).normalized;
		dirF = (head.transform.position - sensorF.position).normalized;
		dirG = (head.transform.position - sensorG.position).normalized;
		dirH = (head.transform.position - sensorH.position).normalized;


		#region Wall Distance
		if(Physics2D.Raycast(head.transform.position, head.transform.position + dirA, Mathf.Infinity, dangerLayer))
		{
			if(!Physics2D.Raycast(head.transform.position, head.transform.position + dirA, Mathf.Infinity, dangerLayer).transform.gameObject.name.Contains("Creature") && !Physics2D.Raycast(head.transform.position, head.transform.position + dirA, Mathf.Infinity, dangerLayer).transform.gameObject.name.Contains("Body"))
			{
				howFarAwayA = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirA, Mathf.Infinity, dangerLayer).point);
			}
		}

		if(Physics2D.Raycast(head.transform.position, head.transform.position + dirB, Mathf.Infinity, dangerLayer))
		{
			if (!Physics2D.Raycast(head.transform.position, head.transform.position + dirB, Mathf.Infinity, dangerLayer).transform.gameObject.name.Contains("Creature") && !Physics2D.Raycast(head.transform.position, head.transform.position + dirB, Mathf.Infinity, dangerLayer).transform.gameObject.name.Contains("Body"))
			{
				howFarAwayB = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirB, Mathf.Infinity, dangerLayer).point);
			}
		}
		
		if(Physics2D.Raycast(head.transform.position, head.transform.position + dirC, Mathf.Infinity, dangerLayer))
		{
			if (!Physics2D.Raycast(head.transform.position, head.transform.position + dirC, Mathf.Infinity, dangerLayer).transform.gameObject.name.Contains("Creature") && !Physics2D.Raycast(head.transform.position, head.transform.position + dirC, Mathf.Infinity, dangerLayer).transform.gameObject.name.Contains("Body"))
			{
				howFarAwayC = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirC, Mathf.Infinity, dangerLayer).point);
			}
		}

		if(Physics2D.Raycast(head.transform.position, head.transform.position + dirD, Mathf.Infinity, dangerLayer))
		{
			if (!Physics2D.Raycast(head.transform.position, head.transform.position + dirD, Mathf.Infinity, dangerLayer).transform.gameObject.name.Contains("Creature") && !Physics2D.Raycast(head.transform.position, head.transform.position + dirD, Mathf.Infinity, dangerLayer).transform.gameObject.name.Contains("Body"))
			{
				howFarAwayD = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirD, Mathf.Infinity, dangerLayer).point);
			}
		}

		if(Physics2D.Raycast(head.transform.position, head.transform.position + dirE, Mathf.Infinity, dangerLayer))
		{
			if(!Physics2D.Raycast(head.transform.position, head.transform.position + dirE, Mathf.Infinity, dangerLayer).transform.gameObject.name.Contains("Creature") && !Physics2D.Raycast(head.transform.position, head.transform.position + dirE, Mathf.Infinity, dangerLayer).transform.gameObject.name.Contains("Body"))
			{
				howFarAwayE = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirE, Mathf.Infinity, dangerLayer).point);
			}
		}

		if(Physics2D.Raycast(head.transform.position, head.transform.position + dirF, Mathf.Infinity, dangerLayer))
		{
			if (!Physics2D.Raycast(head.transform.position, head.transform.position + dirF, Mathf.Infinity, dangerLayer).transform.gameObject.name.Contains("Creature") && !Physics2D.Raycast(head.transform.position, head.transform.position + dirF, Mathf.Infinity, dangerLayer).transform.gameObject.name.Contains("Body"))
			{
				howFarAwayF = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirF, Mathf.Infinity, dangerLayer).point);
			}
		}
		
		if(Physics2D.Raycast(head.transform.position, head.transform.position + dirG, Mathf.Infinity, dangerLayer))
		{
			if (!Physics2D.Raycast(head.transform.position, head.transform.position + dirG, Mathf.Infinity, dangerLayer).transform.gameObject.name.Contains("Creature") && !Physics2D.Raycast(head.transform.position, head.transform.position + dirG, Mathf.Infinity, dangerLayer).transform.gameObject.name.Contains("Body"))
			{
				howFarAwayG = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirG, Mathf.Infinity, dangerLayer).point);
			}
		}

		if(Physics2D.Raycast(head.transform.position, head.transform.position + dirH, Mathf.Infinity, dangerLayer))
		{
			if (!Physics2D.Raycast(head.transform.position, head.transform.position + dirH, Mathf.Infinity, dangerLayer).transform.gameObject.name.Contains("Creature") && !Physics2D.Raycast(head.transform.position, head.transform.position + dirH, Mathf.Infinity, dangerLayer).transform.gameObject.name.Contains("Body"))
			{
				howFarAwayH = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirH, Mathf.Infinity, dangerLayer).point);
			}
		}
		#endregion

		#region Apple Distance
		if (Physics2D.Raycast(head.transform.position, head.transform.position + dirA, Mathf.Infinity, appleLayer))
		{
			if(Physics2D.Raycast(head.transform.position, head.transform.position + dirA, Mathf.Infinity, appleLayer).transform.parent == transform)
			{
				distanceFromAppleA = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirA, Mathf.Infinity, appleLayer).point);
			}
			else
			{
				distanceFromAppleA = 0;
			}
		}
		else
		{
			distanceFromAppleA = 0;
		}

		if (Physics2D.Raycast(head.transform.position, head.transform.position + dirB, Mathf.Infinity, appleLayer))
		{
			if (Physics2D.Raycast(head.transform.position, head.transform.position + dirB, Mathf.Infinity, appleLayer).transform.parent == transform)
			{
				distanceFromAppleB = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirB, Mathf.Infinity, appleLayer).point);
			}
			else
			{
				distanceFromAppleB = 0;
			}
		}
		else
		{
			distanceFromAppleB = 0;
		}

		if (Physics2D.Raycast(head.transform.position, head.transform.position + dirC, Mathf.Infinity, appleLayer))
		{
			if (Physics2D.Raycast(head.transform.position, head.transform.position + dirC, Mathf.Infinity, appleLayer).transform.parent == transform)
			{
				distanceFromAppleC = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirC, Mathf.Infinity, appleLayer).point);
			}
			else
			{
				distanceFromAppleC = 0;
			}
		}
		else
		{
			distanceFromAppleC = 0;
		}

		if (Physics2D.Raycast(head.transform.position, head.transform.position + dirD, Mathf.Infinity, appleLayer))
		{
			if (Physics2D.Raycast(head.transform.position, head.transform.position + dirD, Mathf.Infinity, appleLayer).transform.parent == transform)
			{
				distanceFromAppleD = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirD, Mathf.Infinity, appleLayer).point);
			}
			else
			{
				distanceFromAppleD = 0;
			}
		}
		else
		{
			distanceFromAppleD = 0;
		}

		if (Physics2D.Raycast(head.transform.position, head.transform.position + dirE, Mathf.Infinity, appleLayer))
		{
			if(Physics2D.Raycast(head.transform.position, head.transform.position + dirE, Mathf.Infinity, appleLayer).transform.parent == transform)
			{
				distanceFromAppleE = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirE, Mathf.Infinity, appleLayer).point);
			}
			else
			{
				distanceFromAppleE = 0;
			}
		}
		else
		{
			distanceFromAppleE = 0;
		}

		if (Physics2D.Raycast(head.transform.position, head.transform.position + dirF, Mathf.Infinity, appleLayer))
		{
			if (Physics2D.Raycast(head.transform.position, head.transform.position + dirF, Mathf.Infinity, appleLayer).transform.parent == transform)
			{
				distanceFromAppleF = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirF, Mathf.Infinity, appleLayer).point);
			}
			else
			{
				distanceFromAppleF = 0;
			}
		}
		else
		{
			distanceFromAppleF = 0;
		}

		if (Physics2D.Raycast(head.transform.position, head.transform.position + dirG, Mathf.Infinity, appleLayer))
		{
			if (Physics2D.Raycast(head.transform.position, head.transform.position + dirG, Mathf.Infinity, appleLayer).transform.parent == transform)
			{
				distanceFromAppleG = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirG, Mathf.Infinity, appleLayer).point);
			}
			else
			{
				distanceFromAppleG = 0;
			}
		}
		else
		{
			distanceFromAppleG = 0;
		}

		if (Physics2D.Raycast(head.transform.position, head.transform.position + dirH, Mathf.Infinity, appleLayer))
		{
			if (Physics2D.Raycast(head.transform.position, head.transform.position + dirH, Mathf.Infinity, appleLayer).transform.parent == transform)
			{
				distanceFromAppleH = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirH, Mathf.Infinity, appleLayer).point);
			}
			else
			{
				distanceFromAppleH = 0;
			}
		}
		else
		{
			distanceFromAppleH = 0;
		}
		#endregion
		
		#region Body Distance
		if (Physics2D.Raycast(head.transform.position, head.transform.position + dirA, Mathf.Infinity, bodyLayer))
		{
			if(Physics2D.Raycast(head.transform.position, head.transform.position + dirA, Mathf.Infinity, bodyLayer).transform.parent == transform)
			{
				distanceFromBodyA = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirA, Mathf.Infinity, bodyLayer).point);
			}
			else
			{
				distanceFromBodyA = 0;
			}
		}
		else
		{
			distanceFromBodyA = 0;
		}

		if (Physics2D.Raycast(head.transform.position, head.transform.position + dirB, Mathf.Infinity, bodyLayer))
		{
			if (Physics2D.Raycast(head.transform.position, head.transform.position + dirB, Mathf.Infinity, bodyLayer).transform.parent == transform)
			{
				distanceFromBodyB = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirB, Mathf.Infinity, bodyLayer).point);
			}
			else
			{
				distanceFromBodyB = 0;
			}
		}
		else
		{
			distanceFromBodyB = 0;
		}

		if (Physics2D.Raycast(head.transform.position, head.transform.position + dirC, Mathf.Infinity, bodyLayer))
		{
			if (Physics2D.Raycast(head.transform.position, head.transform.position + dirC, Mathf.Infinity, bodyLayer).transform.parent == transform)
			{
				distanceFromBodyC = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirC, Mathf.Infinity, bodyLayer).point);
			}
			else
			{
				distanceFromBodyC = 0;
			}
		}
		else
		{
			distanceFromBodyC = 0;
		}

		if (Physics2D.Raycast(head.transform.position, head.transform.position + dirD, Mathf.Infinity, bodyLayer))
		{
			if (Physics2D.Raycast(head.transform.position, head.transform.position + dirD, Mathf.Infinity, bodyLayer).transform.parent == transform)
			{
				distanceFromBodyD = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirD, Mathf.Infinity, bodyLayer).point);
			}
			else
			{
				distanceFromBodyD = 0;
			}
		}
		else
		{
			distanceFromBodyD = 0;
		}

		if (Physics2D.Raycast(head.transform.position, head.transform.position + dirE, Mathf.Infinity, bodyLayer))
		{
			if(Physics2D.Raycast(head.transform.position, head.transform.position + dirE, Mathf.Infinity, bodyLayer).transform.parent == transform)
			{
				distanceFromBodyE = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirE, Mathf.Infinity, bodyLayer).point);
			}
			else
			{
				distanceFromBodyE = 0;
			}
		}
		else
		{
			distanceFromBodyE = 0;
		}

		if (Physics2D.Raycast(head.transform.position, head.transform.position + dirF, Mathf.Infinity, bodyLayer))
		{
			if (Physics2D.Raycast(head.transform.position, head.transform.position + dirF, Mathf.Infinity, bodyLayer).transform.parent == transform)
			{
				distanceFromBodyF = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirF, Mathf.Infinity, bodyLayer).point);
			}
			else
			{
				distanceFromBodyF = 0;
			}
		}
		else
		{
			distanceFromBodyF = 0;
		}

		if (Physics2D.Raycast(head.transform.position, head.transform.position + dirG, Mathf.Infinity, bodyLayer))
		{
			if (Physics2D.Raycast(head.transform.position, head.transform.position + dirG, Mathf.Infinity, bodyLayer).transform.parent == transform)
			{
				distanceFromBodyG = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirG, Mathf.Infinity, bodyLayer).point);
			}
			else
			{
				distanceFromBodyG = 0;
			}
		}
		else
		{
			distanceFromBodyG = 0;
		}

		if (Physics2D.Raycast(head.transform.position, head.transform.position + dirH, Mathf.Infinity, bodyLayer))
		{
			if (Physics2D.Raycast(head.transform.position, head.transform.position + dirH, Mathf.Infinity, bodyLayer).transform.parent == transform)
			{
				distanceFromBodyH = Vector2.Distance(head.transform.position, Physics2D.Raycast(head.transform.position, head.transform.position + dirH, Mathf.Infinity, bodyLayer).point);
			}
			else
			{
				distanceFromBodyH = 0;
			}
		}
		else
		{
			distanceFromBodyH = 0;
		}
		#endregion

		#endregion

		if (GameObject.FindGameObjectWithTag("Apple"))
		{
			//nextApple = GameObject.FindGameObjectWithTag("Apple").transform;

			snakeManager.applesLeft = GameObject.FindGameObjectsWithTag("Apple").Length;
		}
		else
		{
			GameObject applePos = Instantiate(applePrefab, new Vector2(Random.Range(-boardSize.x / 2, boardSize.x / 2), Random.Range(-boardSize.y / 2, boardSize.y / 2)), Quaternion.identity);
			
			if(applePos.transform.position.x % 0.3f != 0)
			{
				applePos.transform.position -= new Vector3((applePos.transform.position.x % 0.3f) * 3, 0);
			}
		}

		if (head.GetComponent<TouchingWallOrApple>().touchingApple)
		{
			applesCollected++;
			Destroy(GameObject.FindGameObjectWithTag("Apple"));
			head.GetComponent<TouchingWallOrApple>().touchingApple = false;
		}

		if (head.GetComponent<TouchingWallOrApple>().touchingWall)
		{
			failed = true;
		}

		float[] inputs = new float[25];
		inputs[0] = head.GetComponent<Rigidbody2D>().rotation / 360;
		inputs[1] = howFarAwayA;
		inputs[2] = howFarAwayB;
		inputs[3] = howFarAwayC;
		inputs[4] = howFarAwayD;
		inputs[5] = howFarAwayE;
		inputs[6] = howFarAwayF;
		inputs[7] = howFarAwayG;
		inputs[8] = howFarAwayH;
		inputs[9] = distanceFromBodyA;
		inputs[10] = distanceFromBodyB;
		inputs[11] = distanceFromBodyC;
		inputs[12] = distanceFromBodyD;
		inputs[13] = distanceFromBodyE;
		inputs[14] = distanceFromBodyF;
		inputs[15] = distanceFromBodyG;
		inputs[16] = distanceFromBodyH;
		inputs[17] = distanceFromAppleA;
		inputs[18] = distanceFromAppleB;
		inputs[19] = distanceFromAppleC;
		inputs[20] = distanceFromAppleD;
		inputs[21] = distanceFromAppleE;
		inputs[22] = distanceFromAppleF;
		inputs[23] = distanceFromAppleG;
		inputs[24] = distanceFromAppleH;

		float[] output = net.FeedForward(inputs);

		if (tickSpeed <= 0)
		{
			if (!failed)
			{
				timeElapsed += Time.deltaTime;

				for (int i = 0; i < bodyParts.Count; i++)
				{
					if(i - 1 >= 0)
					{
						bodyParts[i].transform.position = partLocationLastTick[i - 1];
					}
				}
				head.transform.position += 0.3f * head.transform.up;

				net.SetFitness(applesCollected * 100 + timeElapsed);
			}
			else
			{
				head.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.1f);
			}

			for (int i = 0; i < bodyParts.Count; i++)
			{
				partLocationLastTick[i] = bodyParts[i].transform.position;
			}

			movesThisTick = 1;
			tickSpeed = maxTickSpeed;
		}
		else
		{
			tickSpeed -= Time.deltaTime;
		}

		if (!failed)
		{
			#region Movement
			headRot = head.GetComponent<Rigidbody2D>().rotation;

			if (movesThisTick == 1)
			{
				float mult = 3;
				float subtract = 1;

				if ((output[0] * mult - subtract) > 0 && (headRot == -90))
				{
					head.transform.Rotate(new Vector3(0, 0, 90));
					movesThisTick--;
				}
				else if ((output[0] * mult - subtract) > 0 && (headRot == 90))
				{
					head.transform.Rotate(new Vector3(0, 0, -90));
					movesThisTick--;
				}

				if ((output[1] * mult - subtract) > 0 && (headRot == 0))
				{
					head.transform.Rotate(new Vector3(0, 0, -90));
					movesThisTick--;
				}
				else if ((output[1] * mult - subtract) > 0 && (headRot == -180 || headRot == 180))
				{
					head.transform.Rotate(new Vector3(0, 0, 90));
					movesThisTick--;
				}

				if ((output[2] * mult - subtract) > 0 && (headRot == -90))
				{
					head.transform.Rotate(new Vector3(0, 0, -90));
					movesThisTick--;
				}
				else if ((output[2] * mult - subtract) > 0 && (headRot == 90))
				{
					head.transform.Rotate(new Vector3(0, 0, 90));
					movesThisTick--;
				}

				if ((output[3] * mult - subtract) > 0 && (headRot == 0))
				{
					head.transform.Rotate(new Vector3(0, 0, 90));
					movesThisTick--;
				}
				else if ((output[3] * mult - subtract) > 0 && (headRot == -180 || headRot == 180))
				{
					head.transform.Rotate(new Vector3(0, 0, -90));
					movesThisTick--;
				}
			}
			#endregion
		}
	}

	public void Init(NeuralNetwork net, Transform target)
	{
		this.net = net;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		//if(collision.gameObject.tag == "Danger")
		//{
		//	GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		//	failed = true;
		//}
	}

	void OnDrawGizmos()
	{
		dirA = (head.transform.position - sensorA.position).normalized;
		dirB = (head.transform.position - sensorB.position).normalized;
		dirC = (head.transform.position - sensorC.position).normalized;
		dirD = (head.transform.position - sensorD.position).normalized;
		dirE = (head.transform.position - sensorE.position).normalized;
		dirF = (head.transform.position - sensorF.position).normalized;
		dirG = (head.transform.position - sensorG.position).normalized;
		dirH = (head.transform.position - sensorH.position).normalized;
		Gizmos.DrawLine(head.transform.position, head.transform.position + dirA * 15);
		Gizmos.DrawLine(head.transform.position, head.transform.position + dirB * 15);
		Gizmos.DrawLine(head.transform.position, head.transform.position + dirC * 15);
		Gizmos.DrawLine(head.transform.position, head.transform.position + dirD * 15);
		Gizmos.DrawLine(head.transform.position, head.transform.position + dirE * 15);
		Gizmos.DrawLine(head.transform.position, head.transform.position + dirF * 15);
		Gizmos.DrawLine(head.transform.position, head.transform.position + dirG * 15);
		Gizmos.DrawLine(head.transform.position, head.transform.position + dirH * 15);

		Gizmos.DrawWireCube(Camera.main.transform.position + new Vector3(0, 0, 10), new Vector3(boardSize.x, boardSize.y, 1));
	}
}
