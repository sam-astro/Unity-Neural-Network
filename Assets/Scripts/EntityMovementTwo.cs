using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EntityMovementTwo : MonoBehaviour
{
    private NeuralNetwork net;
    private Rigidbody rb;

    public Transform target;
    public bool failed;

    public Transform sensorA, sensorB, sensorC, sensorD, sensorE;
    [HideInInspector]
    public float howFarAwayA, howFarAwayB, howFarAwayC, howFarAwayD, howFarAwayE;
    public LayerMask senseLayer;

    Vector3 positionSecondsAgo;
    float timer = 2f;

    float timeToComplete;
    bool completedCourse;

    float countDown = 60;

    public float distanceTravelled;

    float direction;

    public Material normalMat;
    public Material failedMat;

    float startDistanceFromTarget;
    float topSpeed = 0;

    void Awake()
	{
        rb = GetComponent<Rigidbody>();
        failed = false;
        topSpeed = 0;
	}

    void FixedUpdate()
    {
        #region Sensors
        direction = transform.rotation.y;

		if (Physics.Linecast(transform.position, sensorA.position, out RaycastHit sensedInfoA, senseLayer))
		{
            howFarAwayA = Vector3.Distance(sensedInfoA.point, transform.position);
		}
		else
		{
            howFarAwayA = 6f;
        }
        
        if(Physics.Linecast(transform.position, sensorB.position, out RaycastHit sensedInfoB, senseLayer))
		{
            howFarAwayB = Vector3.Distance(sensedInfoB.point, transform.position);
		}
		else
		{
            howFarAwayB = 6f;
        }
        
        if(Physics.Linecast(transform.position, sensorC.position, out RaycastHit sensedInfoC, senseLayer))
		{
            howFarAwayC = Vector3.Distance(sensedInfoC.point, transform.position);
		}
		else
		{
            howFarAwayC = 6f;
        }
        
        if(Physics.Linecast(transform.position, sensorD.position, out RaycastHit sensedInfoD, senseLayer))
		{
            howFarAwayD = Vector3.Distance(sensedInfoD.point, transform.position);
		}
		else
		{
            howFarAwayD = 6f;
        }

		if (Physics.Linecast(transform.position, sensorE.position, out RaycastHit sensedInfoE, senseLayer))
		{
            howFarAwayE = Vector3.Distance(sensedInfoE.point, transform.position);
		}
		else
		{
            howFarAwayE = 6f;
        }
		#endregion

        distanceTravelled = timeToComplete * ((Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z)) / 2);

        if (timer <= 0)
		{
            if(Vector3.Distance(transform.position, positionSecondsAgo) <= 3.5f)
			{
                failed = true;
			}
            positionSecondsAgo = transform.position;
            timer = 2f;
		}
		else
		{
            timer -= Time.deltaTime;
		}

        if (!failed)
        {
			float[] inputs = new float[7];

			inputs[0] = howFarAwayA;
			inputs[1] = howFarAwayB;
			inputs[2] = howFarAwayC;
			inputs[3] = howFarAwayD;

			inputs[4] = howFarAwayE;

			if (completedCourse)
			{
				inputs[5] = 100;
			}
			else
			{
				inputs[5] = (howFarAwayA) + (howFarAwayB) + (howFarAwayC) + (howFarAwayD) + (howFarAwayE);
				//inputs[4] = (-howFarAwayA) + (-howFarAwayB) + (howFarAwayC) + (howFarAwayD);
			}

            inputs[6] = direction;

            float[] output = net.FeedForward(inputs);

            rb.angularVelocity = new Vector3(0, 500f, 0) * output[0];
            //rb.angularVelocity = new Vector3(0, 500f, 0) * Mathf.Clamp(((output[0]) + (output[1]) + (output[2]) + (output[3]) + (output[4])), -1, 1);
            rb.velocity = 6f * transform.forward * ((output[1] * 2) + 1);
		}
		
        if(failed)
		{
            GetComponent<MeshRenderer>().material = failedMat;
		}
		else
		{
            GetComponent<MeshRenderer>().material = normalMat;
        }

		if (!completedCourse)
		{
            timeToComplete += Time.deltaTime;
            countDown -= Time.deltaTime;
            //net.AddFitness(-timeToComplete / 10);

            if((rb.velocity.x + rb.velocity.y + rb.velocity.z) / 3 > topSpeed)
			{
                topSpeed = (rb.velocity.x + rb.velocity.y + rb.velocity.z) / 3;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.tag == "Danger")
        {
            failed = true;
            FinalizeFitnessFail();
        }
        else if (collision.gameObject.tag == "CourseComplete")
        {
            failed = true;
            completedCourse = true;
            FinalizeFitnessSucceed();
        }
    }

    void FinalizeFitnessFail()
	{
        net.SetFitness(distanceTravelled * 5 + net.GetFitness() + (startDistanceFromTarget - Vector3.Distance(transform.position, target.transform.position)) + topSpeed * 10 - (60 - timeToComplete * 4));
        net.SetFitness((net.GetFitness() * 0.75f));
    }
    
    void FinalizeFitnessSucceed()
	{
        net.SetFitness(distanceTravelled * 5 + net.GetFitness() + (startDistanceFromTarget - Vector3.Distance(transform.position, target.transform.position)) + topSpeed * 10 - (60 - timeToComplete * 4));
        net.AddFitness(Mathf.Abs(60 - timeToComplete));
    }

    public void Init(NeuralNetwork net, Transform target)
    {
        this.net = net;
        this.target = target;
        startDistanceFromTarget = Vector3.Distance(transform.position, target.transform.position);
    }

    void OnDrawGizmos()
    {
		if (!failed)
        {
            if (Physics.Linecast(transform.position, sensorA.position, out RaycastHit sensedInfoA, senseLayer))
            {
                Gizmos.color = Color.black;
                Gizmos.DrawIcon(sensedInfoA.point, "collidePointIcon.png");
                Gizmos.color = Color.red;
                Gizmos.DrawLine(sensedInfoA.point, transform.position);
            }

            if (Physics.Linecast(transform.position, sensorB.position, out RaycastHit sensedInfoB, senseLayer))
            {
                Gizmos.color = Color.black;
                Gizmos.DrawIcon(sensedInfoB.point, "collidePointIcon.png");
                Gizmos.color = Color.red;
                Gizmos.DrawLine(sensedInfoB.point, transform.position);
            }

            if (Physics.Linecast(transform.position, sensorC.position, out RaycastHit sensedInfoC, senseLayer))
            {
                Gizmos.color = Color.black;
                Gizmos.DrawIcon(sensedInfoC.point, "collidePointIcon.png");
                Gizmos.color = Color.red;
                Gizmos.DrawLine(sensedInfoC.point, transform.position);
            }

            if (Physics.Linecast(transform.position, sensorD.position, out RaycastHit sensedInfoD, senseLayer))
            {
                Gizmos.color = Color.black;
                Gizmos.DrawIcon(sensedInfoD.point, "collidePointIcon.png");
                Gizmos.color = Color.red;
                Gizmos.DrawLine(sensedInfoD.point, transform.position);
            }

            if (Physics.Linecast(transform.position, sensorE.position, out RaycastHit sensedInfoE, senseLayer))
            {
                Gizmos.color = Color.black;
                Gizmos.DrawIcon(sensedInfoE.point, "collidePointIcon.png");
                Gizmos.color = Color.red;
                Gizmos.DrawLine(sensedInfoE.point, transform.position);
            }
        }
    }
}

