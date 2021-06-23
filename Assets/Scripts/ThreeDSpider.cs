using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ThreeDSpider : MonoBehaviour
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

    public GameObject head, oneLeg, twoLeg, threeLeg, fourLeg, oneLowerLeg, twoLowerLeg, threeLowerLeg, fourLowerLeg;
    public GameObject uOneJoint, uTwoJoint, uThreeJoint, uFourJoint, lOneJoint, lTwoJoint, lThreeJoint, lFourJoint;

    float timeToComplete;

    float motorSpeedMultiplier = 400;

    public bool stayUpright;
    float uprightAmount;

    public Material failedMaterial;

    float internalTimer = 1f;

    void Awake()
    {
        failed = false;
        lowestDistanceFromTarget = 100000;
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

        if (stayUpright)
        {
            if (head.GetComponent<LimbsCollidingOrNot>().collidingInt == 1)
            {
                failed = true;
            }
        }

        //distanceTravelled = timeToComplete * body.GetComponent<Rigidbody2D>().velocity.x;

        if (!failed)
        {
            if(internalTimer < 1)
            {
                internalTimer += Time.deltaTime;
            }
			else
			{
                internalTimer = 0;
			}

            timeToComplete += Time.fixedDeltaTime;

            float[] inputs = new float[45];

            inputs[0] = uOneJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[1] = uOneJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[2] = uOneJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[3] = uTwoJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[4] = uTwoJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[5] = uTwoJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[6] = uThreeJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[7] = uThreeJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[8] = uThreeJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[9] = uFourJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[10] = uFourJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[11] = uFourJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[12] = lOneJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[13] = lOneJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[14] = lOneJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[15] = lTwoJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[16] = lTwoJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[17] = lTwoJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[18] = lThreeJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[19] = lThreeJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[20] = lThreeJoint.GetComponent<Rigidbody>().rotation.z / 360;
            inputs[21] = lFourJoint.GetComponent<Rigidbody>().rotation.x / 360;
            inputs[22] = lFourJoint.GetComponent<Rigidbody>().rotation.y / 360;
            inputs[23] = lFourJoint.GetComponent<Rigidbody>().rotation.z / 360;

			inputs[24] = distanceFromSensorA / 20;
            inputs[25] = distanceFromSensorB / 20;
            inputs[26] = distanceFromSensorC / 20;
            inputs[27] = distanceFromSensorD / 20;
            inputs[28] = distanceFromSensorE / 20;
            inputs[29] = distanceFromSensorF / 20;
            inputs[30] = distanceFromSensorG / 20;
            inputs[31] = distanceFromSensorH / 20;
            inputs[32] = distanceFromSensorI / 20;
            inputs[33] = distanceFromSensorJ / 20;

			inputs[34] = Mathf.Clamp((head.transform.position.x + head.transform.position.z) / 1000, 0, 1);
			//inputs[13] = Mathf.Clamp(head.transform.position.x / 1000, 0, 1);

			inputs[35] = oneLeg.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[36] = twoLeg.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[37] = threeLeg.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[38] = fourLeg.GetComponent<LimbsCollidingOrNot>().collidingInt;
			inputs[39] = oneLowerLeg.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[40] = twoLowerLeg.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[41] = threeLowerLeg.GetComponent<LimbsCollidingOrNot>().collidingInt;
            inputs[42] = fourLowerLeg.GetComponent<LimbsCollidingOrNot>().collidingInt;

            inputs[43] = Mathf.Clamp(internalTimer, 0.01f, 1);

			inputs[44] = (360 - Mathf.Abs(head.transform.rotation.x + head.transform.rotation.z)) / 100;

			float[] output = net.FeedForward(inputs);

			uOneJoint.GetComponent<Rigidbody>().MoveRotation(uOneJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[0] * motorSpeedMultiplier, output[1] * motorSpeedMultiplier, output[2] * motorSpeedMultiplier) * Time.deltaTime));
			uTwoJoint.GetComponent<Rigidbody>().MoveRotation(uTwoJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[3] * motorSpeedMultiplier, output[4] * motorSpeedMultiplier, output[5] * motorSpeedMultiplier) * Time.deltaTime));
			uThreeJoint.GetComponent<Rigidbody>().MoveRotation(uThreeJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[6] * motorSpeedMultiplier, output[7] * motorSpeedMultiplier, output[8] * motorSpeedMultiplier) * Time.deltaTime));
            uFourJoint.GetComponent<Rigidbody>().MoveRotation(uFourJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[9] * motorSpeedMultiplier, output[10] * motorSpeedMultiplier, output[11] * motorSpeedMultiplier) * Time.deltaTime));
			lOneJoint.GetComponent<Rigidbody>().MoveRotation(lOneJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[12] * motorSpeedMultiplier, output[13] * motorSpeedMultiplier, output[14] * motorSpeedMultiplier) * Time.deltaTime));
			lTwoJoint.GetComponent<Rigidbody>().MoveRotation(lTwoJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[15] * motorSpeedMultiplier, output[16] * motorSpeedMultiplier, output[17] * motorSpeedMultiplier) * Time.deltaTime));
            lThreeJoint.GetComponent<Rigidbody>().MoveRotation(lThreeJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[18] * motorSpeedMultiplier, output[19] * motorSpeedMultiplier, output[20] * motorSpeedMultiplier) * Time.deltaTime));
			lFourJoint.GetComponent<Rigidbody>().MoveRotation(lFourJoint.GetComponent<Rigidbody>().rotation * Quaternion.Euler(new Vector3(output[21] * motorSpeedMultiplier, output[22] * motorSpeedMultiplier, output[23] * motorSpeedMultiplier) * Time.deltaTime));

			//net.SetFitness(timeToComplete);
			net.SetFitness(((Mathf.Abs(head.transform.position.x) + Mathf.Abs(head.transform.position.z)) / 2) + timeToComplete + uprightAmount);
			//net.SetFitness(Mathf.Abs(head.transform.position.x));
			//net.SetFitness(distanceTravelled + uprightAmount * 10);
			//uprightAmount += (360 - Mathf.Abs(head.transform.rotation.x + head.transform.rotation.z)) / 1000;
			//distanceTravelled = upperBody.transform.position.z * -1;
			distanceTravelled = net.GetFitness();

            //net.SetFitness(200 - Vector3.Distance(upperBody.transform.position, target.position) + timeToComplete * 10 + uprightAmount * 10);
            //net.SetFitness(100 - Vector3.Distance(upperBody.transform.position, target.position));
        }
		else
		{
			head.GetComponent<MeshRenderer>().material = failedMaterial;
			oneLeg.GetComponent<MeshRenderer>().material = failedMaterial;
			twoLeg.GetComponent<MeshRenderer>().material = failedMaterial;
			threeLeg.GetComponent<MeshRenderer>().material = failedMaterial;
			fourLeg.GetComponent<MeshRenderer>().material = failedMaterial;
			oneLowerLeg.GetComponent<MeshRenderer>().material = failedMaterial;
			twoLowerLeg.GetComponent<MeshRenderer>().material = failedMaterial;
			threeLowerLeg.GetComponent<MeshRenderer>().material = failedMaterial;
			fourLowerLeg.GetComponent<MeshRenderer>().material = failedMaterial;
			uOneJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			uTwoJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			uThreeJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			uFourJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			lOneJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			lTwoJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			lThreeJoint.GetComponent<MeshRenderer>().material = failedMaterial;
			lFourJoint.GetComponent<MeshRenderer>().material = failedMaterial;
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

