using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Entity : MonoBehaviour
{
	#region Variables
	public Rigidbody rb;
    public Transform target;
    public Transform[] checkpoints;
    public float timeLimit;

    public float distanceFromTarget;

    //All directions to move
    public int whatDirectionStep;
    public float[] timeToWaitBetweenDirectionSteps;

    public float[] howMuchToMoveX; //How much to move X this generation
    public float[] howMuchToMoveZ; //How much to move Z this generation
    public float[] didMoveX;       //How much was moved X last generation
    public float[] didMoveZ;       //How much was moved Z last generation
    public float[] howOftenToJump; //How often to jump

    public bool isGrounded;
    public Transform groundCheck;
    public float checkerSize;
    public LayerMask whatLayer;

    public Slider timeSlider;

    public bool hasCompletedTask;
    bool timeRanOut;
    bool hitDangerObject;

	#region UI Elements
	public TMP_Text distanceText;
    public TMP_Text moveXText;
    public TMP_Text moveZText;
    public TMP_Text didMoveXText;
    public TMP_Text didMoveZText;
    public TMP_Text completedTaskText;
    public TMP_Text countdownText;
    public TMP_Text generationNoText;
    #endregion

    int whatGeneration;
    int amntOfCheckpointsReached;

    public Transform spawn;
    #endregion

    void Start()
	{
        //Makes sure the values aren't 0 when they start
        if (PlayerPrefs.GetFloat("setStartingFloats") != 1 && PlayerPrefs.GetFloat("hasCompleted") == 0)
        {
            PlayerPrefs.SetFloat("didMoveXMIN", -5);
            PlayerPrefs.SetFloat("didMoveXMAX", 5);
            PlayerPrefs.SetFloat("didMoveZMIN", -5);
            PlayerPrefs.SetFloat("didMoveZMAX", 5);
            PlayerPrefs.SetFloat("lastDistanceFromTarget", 10000);
            //Ends loop
            PlayerPrefs.SetFloat("setStartingFloats", 1);

            if(PlayerPrefs.GetFloat("setStartingFloats") != 1)
			{
                Debug.LogError("Line 53");
			}
        }

        if (PlayerPrefs.GetFloat("hasCompleted") == 0) //Checks if the task hasn't been completed
        {
			for (int i = 0; i < timeToWaitBetweenDirectionSteps.Length; i++)
			{
                howMuchToMoveX[i] = PlayerPrefs.GetFloat("didMoveX" + i) + Random.Range(-2, 2);
				while (howMuchToMoveX[i] == PlayerPrefs.GetFloat("blackListX" + i))
				{
					howMuchToMoveX[i] = PlayerPrefs.GetFloat("didMoveX" + i) + Random.Range(-2, 2);
				}

				howMuchToMoveZ[i] = PlayerPrefs.GetFloat("didMoveZ" + i) + Random.Range(-2, 2);
				while (howMuchToMoveZ[i] == PlayerPrefs.GetFloat("blackListZ" + i))
				{
					howMuchToMoveZ[i] = PlayerPrefs.GetFloat("didMoveZ" + i) + Random.Range(-2, 2);
				}
			}

            PlayerPrefs.SetFloat("oftenToJumprand", PlayerPrefs.GetFloat("didJumpTime") + Random.Range(-1, 1));
		}
		else
		{
            for (int i = 0; i < timeToWaitBetweenDirectionSteps.Length; i++)
            {
                howMuchToMoveX[i] = PlayerPrefs.GetFloat("didMoveX" + i);

                howMuchToMoveZ[i] = PlayerPrefs.GetFloat("didMoveZ" + i);
            }

            PlayerPrefs.SetFloat("oftenToJumprand", PlayerPrefs.GetFloat("didJumpTime"));
        }

        whatGeneration = PlayerPrefs.GetInt("generationNo");

        for (int i = 0; i < 1; i++)
        {
            PlayerPrefs.SetInt("generationNo", whatGeneration + 1);
        }
    }

    void Update()
    {
        #region Debug and/or background processees
        isGrounded = Physics.CheckSphere(groundCheck.position, checkerSize, whatLayer);
        groundCheck.transform.parent.up = Vector3.up;

        didMoveX[whatDirectionStep] = PlayerPrefs.GetFloat("didMoveX" + whatDirectionStep);
        didMoveZ[whatDirectionStep] = PlayerPrefs.GetFloat("didMoveZ" + whatDirectionStep);

        Time.timeScale = timeSlider.value;

        distanceText.text = distanceFromTarget.ToString();
        moveXText.text = howMuchToMoveX[whatDirectionStep].ToString();
        moveZText.text = howMuchToMoveZ[whatDirectionStep].ToString();
        didMoveXText.text = didMoveX[whatDirectionStep].ToString();
        didMoveZText.text = didMoveZ[whatDirectionStep].ToString();
        countdownText.text = timeLimit.ToString();
        generationNoText.text = PlayerPrefs.GetInt("generationNo").ToString();

        if (hasCompletedTask)
        {
            completedTaskText.text = "yes";
        }
        else
        {
            completedTaskText.text = "no";
        }
        #endregion

        if (timeToWaitBetweenDirectionSteps.Length > whatDirectionStep)
        {
            //Checks if the time is up for that life
            if (timeLimit <= 0)
            {
                timeRanOut = true;
                StartNextCycle();
            }
            else
            {
                timeLimit -= Time.deltaTime;

                //Checks the distance from the target
                distanceFromTarget = Vector3.Distance(transform.position, checkpoints[0].position) + Vector3.Distance(transform.position, checkpoints[1].position) + Vector3.Distance(transform.position, checkpoints[2].position);

                if (PlayerPrefs.GetFloat("hasCompleted") == 0) //Checks if the task hasn't been completed
                {
                    if (timeToWaitBetweenDirectionSteps[whatDirectionStep] <= 0)
                    {
                        howOftenToJump[whatDirectionStep] = PlayerPrefs.GetFloat("oftenToJumprand");

                        whatDirectionStep++;
                    }
                    else
                    {
                        timeToWaitBetweenDirectionSteps[whatDirectionStep] -= Time.deltaTime;
                    }
                }
                else
                {
                    if (timeToWaitBetweenDirectionSteps[whatDirectionStep] <= 0)
                    {
                        hasCompletedTask = true;
                        howMuchToMoveX[whatDirectionStep] = PlayerPrefs.GetFloat("didMoveX" + whatDirectionStep);
                        howMuchToMoveZ[whatDirectionStep] = PlayerPrefs.GetFloat("didMoveZ" + whatDirectionStep);
                        howOftenToJump[whatDirectionStep] = PlayerPrefs.GetFloat("oftenToJumprand");
                        whatDirectionStep++;
                    }
                    else
                    {
                        timeToWaitBetweenDirectionSteps[whatDirectionStep] -= Time.deltaTime;
                    }
                }

                //Sets the velocity to the desired X and Y mutated values
                rb.velocity = new Vector3(howMuchToMoveX[whatDirectionStep], rb.velocity.y, howMuchToMoveZ[whatDirectionStep]);

                //Jump
                if (isGrounded && howOftenToJump[whatDirectionStep] <= 0)
                {
                    rb.AddForce(new Vector3(0, 1, 0), ForceMode.Impulse);
                }

            }
        }
		else
		{
            StartNextCycle();
		}
    }

    void StartNextCycle()
	{
        if(distanceFromTarget < PlayerPrefs.GetFloat("lastDistanceFromTarget") && PlayerPrefs.GetFloat("hasCompleted") == 0)
		{
            //These set the playerprefs to the values they were this generation, if it fared better than the best so far
            PlayerPrefs.SetFloat("lastDistanceFromTarget", distanceFromTarget);

			for (int i = 0; i < whatDirectionStep; i++)
			{
                PlayerPrefs.SetFloat("didMoveX" + i + amntOfCheckpointsReached, howMuchToMoveX[i]);
                PlayerPrefs.SetFloat("didMoveZ" + i + amntOfCheckpointsReached, howMuchToMoveZ[i]);
            }

            if (hitDangerObject)
            {
				PlayerPrefs.SetFloat("blackListCount", PlayerPrefs.GetFloat("blackListCount") + 1);
				PlayerPrefs.SetFloat("blackListX" + whatDirectionStep, howMuchToMoveX[whatDirectionStep]);
				PlayerPrefs.SetFloat("blackListZ" + whatDirectionStep, howMuchToMoveZ[whatDirectionStep]);
			}

            PlayerPrefs.SetFloat("howOftenToJump", howOftenToJump[whatDirectionStep]);
            PlayerPrefs.SetFloat("timeSliderLastValue", timeSlider.value);
        }
		else if(PlayerPrefs.GetFloat("hasCompleted") == 1)
        {
			for (int i = 0; i < whatDirectionStep; i++)
			{
                PlayerPrefs.SetFloat("didMoveX" + i, howMuchToMoveX[i]);
                PlayerPrefs.SetFloat("didMoveZ" + i, howMuchToMoveZ[i]);
            }
            
            PlayerPrefs.SetFloat("howOftenToJump", howOftenToJump[whatDirectionStep]);
        }

        //Restarts
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.tag == "Danger")
        {
            hitDangerObject = true;
            StartNextCycle();
        }
        else if (collision.gameObject.tag == "Target")
        {
            PlayerPrefs.SetFloat("hasCompleted", 1);
            StartNextCycle();
        }
        else if (collision.gameObject.tag == "Checkpoint")
        {
            Destroy(collision.gameObject.GetComponent<BoxCollider>());
            amntOfCheckpointsReached++;
        }
    }

    void OnDrawGizmos()
	{
        //Gizmos.DrawWireSphere(transform.TransformPoint(-Vector3.up * 0.5f), checkerSize);
        Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(groundCheck.position, checkerSize);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, target.position);

		foreach (Transform point in checkpoints)
		{
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, point.position);
        }

		for (int i = 0; i < PlayerPrefs.GetFloat("blackListCount"); i++)
		{
            float radius = 0.5f;

            Gizmos.DrawWireSphere(new Vector3(PlayerPrefs.GetFloat("blackListX" + i), 1, PlayerPrefs.GetFloat("blackListZ" + i)), radius);
        }
    }
}
