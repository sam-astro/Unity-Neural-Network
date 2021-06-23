using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ThreeDSnake : MonoBehaviour
{
    [HideInInspector]
    public NeuralNetwork net;
    private Rigidbody2D rb;

    private Transform target;
    float lowestDistanceFromTarget = 100000;
    public bool failed;

    public Transform sensorA, sensorB, sensorC, sensorD, sensorE, sensorF, sensorG, sensorH, sensorI, sensorJ;
    float distanceFromSensorA, distanceFromSensorB, distanceFromSensorC, distanceFromSensorD, distanceFromSensorE, distanceFromSensorF, distanceFromSensorG, distanceFromSensorH, distanceFromSensorI, distanceFromSensorJ;

    public LayerMask groundLayer;

    float countDown = 60;

    public float distanceTravelled;

    float direction;

    public GameObject head, upperBody, midBody, lowerBody;
    public GameObject neckJoint, midJoint, endJoint;

    float timeToComplete;

    float motorSpeedMultiplier = 400;

    public bool stayUpright;
    float uprightAmount;

    public Material failedMaterial;
    public Material normalMaterial;

    float internalTimer = 1f;

    void Awake()
    {
        failed = false;
        lowestDistanceFromTarget = 100000;

        //head.GetComponent<MeshRenderer>().material = normalMaterial;
        //upperBody.GetComponent<MeshRenderer>().material = normalMaterial;
        //midBody.GetComponent<MeshRenderer>().material = normalMaterial;
        //lowerBody.GetComponent<MeshRenderer>().material = normalMaterial;
    }

    void FixedUpdate()
    {
		#region Sensors

		if (Physics.Linecast(head.transform.position, sensorA.position, out RaycastHit sensedInfoA, groundLayer))
		{
			distanceFromSensorA = Vector3.Distance(sensedInfoA.point, head.transform.position);
		}
		else
		{
			distanceFromSensorA = 20f;
		}

		if (Physics.Linecast(head.transform.position, sensorB.position, out RaycastHit sensedInfoB, groundLayer))
		{
			distanceFromSensorB = Vector3.Distance(sensedInfoB.point, head.transform.position);
		}
		else
		{
			distanceFromSensorB = 20f;
		}
        
		if (Physics.Linecast(head.transform.position, sensorC.position, out RaycastHit sensedInfoC, groundLayer))
		{
			distanceFromSensorC = Vector3.Distance(sensedInfoC.point, head.transform.position);
		}
		else
		{
			distanceFromSensorC = 20f;
		}
        
		if (Physics.Linecast(head.transform.position, sensorD.position, out RaycastHit sensedInfoD, groundLayer))
		{
			distanceFromSensorD = Vector3.Distance(sensedInfoD.point, head.transform.position);
		}
		else
		{
			distanceFromSensorE = 20f;
		}
        
		if (Physics.Linecast(head.transform.position, sensorF.position, out RaycastHit sensedInfoF, groundLayer))
		{
			distanceFromSensorF = Vector3.Distance(sensedInfoF.point, head.transform.position);
		}
		else
		{
			distanceFromSensorF = 20f;
		}
        
		if (Physics.Linecast(head.transform.position, sensorG.position, out RaycastHit sensedInfoG, groundLayer))
		{
			distanceFromSensorG = Vector3.Distance(sensedInfoG.point, head.transform.position);
		}
		else
		{
			distanceFromSensorG = 20f;
		}
        
		if (Physics.Linecast(head.transform.position, sensorH.position, out RaycastHit sensedInfoH, groundLayer))
		{
			distanceFromSensorH = Vector3.Distance(sensedInfoH.point, head.transform.position);
		}
		else
		{
			distanceFromSensorH = 20f;
		}
        
		if (Physics.Linecast(head.transform.position, sensorI.position, out RaycastHit sensedInfoI, groundLayer))
		{
			distanceFromSensorI = Vector3.Distance(sensedInfoI.point, head.transform.position);
		}
		else
		{
			distanceFromSensorI = 20f;
		}
        
		if (Physics.Linecast(head.transform.position, sensorJ.position, out RaycastHit sensedInfoJ, groundLayer))
		{
			distanceFromSensorJ = Vector3.Distance(sensedInfoJ.point, head.transform.position);
		}
		else
		{
			distanceFromSensorJ = 20f;
		}

		#endregion

        //distanceTravelled = timeToComplete * body.GetComponent<Rigidbody2D>().velocity.x;

        if (!failed)
        {
            if(internalTimer > 0)
            {
                internalTimer -= Time.deltaTime;
            }
			else
			{
                internalTimer = 1;
			}

            if(Vector3.Distance(head.transform.position, target.position) < lowestDistanceFromTarget)
			{
                lowestDistanceFromTarget = Vector3.Distance(head.transform.position, target.position);
            }

            timeToComplete += Time.deltaTime;

            float[] inputs = new float[31];

            inputs[0] = neckJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[1] = neckJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[2] = neckJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[0] = midJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[1] = midJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[2] = midJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[0] = endJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[1] = endJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[2] = endJoint.GetComponent<Rigidbody>().rotation.z / 360;

			inputs[3] = distanceFromSensorA / 20;
            inputs[4] = distanceFromSensorB / 20;
            inputs[5] = distanceFromSensorC / 20;
            inputs[6] = distanceFromSensorD / 20;
            inputs[7] = distanceFromSensorE / 20;
            inputs[8] = distanceFromSensorF / 20;
            inputs[9] = distanceFromSensorG / 20;
            inputs[10] = distanceFromSensorH / 20;
            inputs[11] = distanceFromSensorI / 20;
            inputs[12] = distanceFromSensorJ / 20;

			inputs[13] = Mathf.Clamp((head.transform.position.x + head.transform.position.z) / 2000, 0, 1);
			//inputs[13] = Mathf.Clamp(head.transform.position.x / 1000, 0, 1);

			inputs[14] = head.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[15] = upperBody.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[16] = midBody.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[17] = lowerBody.GetComponent<LimbsCollidingOrNot>().collidingInt;

            inputs[18] = head.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[19] = head.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[20] = head.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[21] = upperBody.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[22] = upperBody.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[23] = upperBody.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[24] = midBody.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[25] = midBody.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[26] = midBody.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[27] = lowerBody.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[28] = lowerBody.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[29] = lowerBody.GetComponent<Rigidbody>().rotation.z / 360;

            inputs[30] = internalTimer;

            float[] output = net.FeedForward(inputs);

			neckJoint.GetComponent<Rigidbody>().MoveRotation(neckJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[0] * motorSpeedMultiplier, output[1] * motorSpeedMultiplier, output[2] * motorSpeedMultiplier) * Time.deltaTime));
			midJoint.GetComponent<Rigidbody>().MoveRotation(midJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[3] * motorSpeedMultiplier, output[4] * motorSpeedMultiplier, output[5] * motorSpeedMultiplier) * Time.deltaTime));
			endJoint.GetComponent<Rigidbody>().MoveRotation(endJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[6] * motorSpeedMultiplier, output[7] * motorSpeedMultiplier, output[8] * motorSpeedMultiplier) * Time.deltaTime));

			//net.SetFitness(timeToComplete);
			net.SetFitness((Mathf.Abs(head.transform.position.x) + Mathf.Abs(head.transform.position.z)) / 2);
			//net.SetFitness(Mathf.Abs(head.transform.position.x));
			//net.SetFitness(distanceTravelled + uprightAmount * 10);
			//uprightAmount += 180 - ((upperBody.transform.rotation.x + 180) / 2);
			//distanceTravelled = upperBody.transform.position.z * -1;
			distanceTravelled = net.GetFitness();

            //net.SetFitness(200 - Vector3.Distance(upperBody.transform.position, target.position) + timeToComplete * 10 + uprightAmount * 10);
            //net.SetFitness(100 - Vector3.Distance(upperBody.transform.position, target.position));
        }
		else
		{
            //head.GetComponent<MeshRenderer>().material = failedMaterial;
            //upperBody.GetComponent<MeshRenderer>().material = failedMaterial;
            //rUpperArm.GetComponent<MeshRenderer>().material = failedMaterial;
            //rLowerArm.GetComponent<MeshRenderer>().material = failedMaterial;
            //rHand.GetComponent<MeshRenderer>().material = failedMaterial;
            //lUpperArm.GetComponent<MeshRenderer>().material = failedMaterial;
            //lLowerArm.GetComponent<MeshRenderer>().material = failedMaterial;
            //lHand.GetComponent<MeshRenderer>().material = failedMaterial;
            //midBody.GetComponent<MeshRenderer>().material = failedMaterial;
            //midBodyB.GetComponent<MeshRenderer>().material = failedMaterial;
            //lowerBody.GetComponent<MeshRenderer>().material = failedMaterial;
            //topLeftLeg.GetComponent<MeshRenderer>().material = failedMaterial;
            //bottomLeftLeg.GetComponent<MeshRenderer>().material = failedMaterial;
            //leftFoot.GetComponent<MeshRenderer>().material = failedMaterial;
            //topRightLeg.GetComponent<MeshRenderer>().material = failedMaterial;
            //bottomRightLeg.GetComponent<MeshRenderer>().material = failedMaterial;
            //rightFoot.GetComponent<MeshRenderer>().material = failedMaterial;
            //lShoulderJoint.GetComponent<MeshRenderer>().material = failedMaterial;
            //rShoulderJoint.GetComponent<MeshRenderer>().material = failedMaterial;
            //lElbowJoint.GetComponent<MeshRenderer>().material = failedMaterial;
            //rElbowJoint.GetComponent<MeshRenderer>().material = failedMaterial;
            //lWristJoint.GetComponent<MeshRenderer>().material = failedMaterial;
            //rWristJoint.GetComponent<MeshRenderer>().material = failedMaterial;
            //uTorsoJoint.GetComponent<MeshRenderer>().material = failedMaterial;
            //mTorsoJoint.GetComponent<MeshRenderer>().material = failedMaterial;
            //lTorsoJoint.GetComponent<MeshRenderer>().material = failedMaterial;
            //lLegJoint.GetComponent<MeshRenderer>().material = failedMaterial;
            //rLegJoint.GetComponent<MeshRenderer>().material = failedMaterial;
            //lKneeJoint.GetComponent<MeshRenderer>().material = failedMaterial;
            //rKneeJoint.GetComponent<MeshRenderer>().material = failedMaterial;
            //lAnkleJoint.GetComponent<MeshRenderer>().material = failedMaterial;
            //rAnkleJoint.GetComponent<MeshRenderer>().material = failedMaterial;

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "Danger")
        //{
        //    failed = true;
        //    net.AddFitness((net.GetFitness() / 2) * -1);
        //}
    }

    public void Init(NeuralNetwork net, Transform target, bool stayUpright)
    {
        this.net = net;
        this.target = target;
        this.stayUpright = stayUpright;
    }

    void OnDrawGizmos()
	{
        Gizmos.color = Color.red;
		//Gizmos.DrawLine(wallSensor.position, body.transform.position);
	}
}

