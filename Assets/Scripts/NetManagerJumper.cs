using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetManagerJumper : MonoBehaviour
{
    public GameObject entityPrefab;
    public GameObject target;
    public GameObject theWall;
    private Vector3 wallOriginalPosition;

    private bool isTraning = false;
    public int populationSize = 20;
    private int generationNumber = 0;
    public int[] layers = new int[] { 7, 10, 10, 6 }; //No. of inputs and No. of outputs
    private List<NeuralNetwork> nets;
    private bool leftMouseDown = false;
    [HideInInspector]
    public List<JumperScript> entityList = null;

    public int amntLeft;

    public TMP_Text generationText;

    [Range(1f, 8)]
    public float timeScale = 1;

    public float topDistance;

    public Material[] placingMaterials;

    public float timer;

    public bool runEffectiveLearning = true;

    public Slider populationSlider;
    public Toggle learnMethodToggle;

    public GameObject cameraObj;

    void Awake()
    {
        wallOriginalPosition = theWall.transform.position;
    }

    void Update()
    {
        generationText.text = generationNumber.ToString();
        populationSize = Mathf.RoundToInt(populationSlider.value);
        runEffectiveLearning = learnMethodToggle;

        Time.timeScale = timeScale;

        if (entityList != null)
        {
            foreach (JumperScript scrpt in entityList)
            {
                if (scrpt.distanceTravelled > topDistance && !scrpt.failed && scrpt != null)
                {
                    topDistance = scrpt.distanceTravelled;
                    //scrpt.GetComponent<MeshRenderer>().material = placingMaterials[1];
                    cameraObj.GetComponent<CameraFollow>().target = scrpt.gameObject.transform.GetChild(0).transform;
                }

                if (scrpt.failed)
                {
                    if (scrpt.distanceTravelled >= topDistance)
                    {
                        topDistance = 0;
                        scrpt.distanceTravelled = 0;
                        //cameraObj.GetComponent<CameraFollow>().target = theWall.transform;
                    }
                }

                if (scrpt.distanceTravelled < topDistance)
                {
                    //scrpt.GetComponent<MeshRenderer>().material = placingMaterials[0];
                }
            }
        }

        if (isTraning == false)
        {
            amntLeft = populationSize;

            if (generationNumber == 0)
            {
                InitEntityNeuralNetworks();
            }
            else
            {
                nets.Sort();
                GameObject.Find("Window_Graph").GetComponent<WindowGraph>().valueList.Add(nets[populationSize - 1].fitness);
                GameObject.Find("Window_Graph").GetComponent<WindowGraph>().NewEntry();
                if (!runEffectiveLearning)
                {
                    for (int i = 0; i < populationSize / 2; i++)
                    {
                        nets[i] = new NeuralNetwork(nets[i + (populationSize / 2)]);
                        nets[i].Mutate();

                        nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
                    }
                }

                if (runEffectiveLearning)
                {
                    for (int i = 0; i < (populationSize - 2) / 2; i++) //Gathers all but best 2 nets
                    {
                        nets[i] = new NeuralNetwork(nets[i + (populationSize - 2) / 2]);     //Copies weight values from top half networks to worst half
                        nets[i].Mutate();                                                    //Mutates new entities

                        nets[i + (populationSize - 2) / 2] = new NeuralNetwork(nets[populationSize - 1]);
                        nets[i + (populationSize - 2) / 2].Mutate();

                        nets[populationSize - 1] = new NeuralNetwork(nets[populationSize - 1]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
                        nets[populationSize - 2] = new NeuralNetwork(nets[populationSize - 2]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
                    }
                }


                for (int i = 0; i < populationSize; i++)
                {
                    nets[i].SetFitness(0f);
                }
            }

            //cameraObj.GetComponent<CameraFollow>().target = theWall.transform;
            generationNumber++;
            topDistance = 0;
            isTraning = true;
            Invoke("Timer", 5);
            timer = 5;
            CreateEntityBodies();
            theWall.transform.position = wallOriginalPosition;
            //theWall.GetComponent<WallMoveScript>().moveSpeed = 2;
        }

        amntLeft = populationSize;
        foreach (JumperScript emt in entityList)
        {
            if (emt.failed)
            {
                amntLeft--;
            }
        }

        if (amntLeft <= 0)
        {
            isTraning = false;
            amntLeft = populationSize;
            timer = 5;
            //cameraObj.GetComponent<CameraFollow>().target = theWall.transform;
        }

        if (Input.GetMouseButtonDown(0))
        {
            leftMouseDown = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            leftMouseDown = false;
        }

        if (leftMouseDown == true)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.transform.position = mousePosition;
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    void Timer()
    {
        if (timer <= 0)
        {
            isTraning = false;
            cameraObj.GetComponent<CameraFollow>().target = theWall.transform;
		}
		else
		{
            Invoke("Timer", 5);
        }
    }

    private void CreateEntityBodies()
    {
        if (entityList != null)
        {
            for (int i = 0; i < entityList.Count; i++)
            {
                GameObject.Destroy(entityList[i].gameObject);
            }
        }

        entityList = new List<JumperScript>();

        for (int i = 0; i < populationSize; i++)
        {
            JumperScript JumperScript = ((GameObject)Instantiate(entityPrefab, new Vector3(0, 0.27f, 0), entityPrefab.transform.rotation)).GetComponent<JumperScript>();
            JumperScript.Init(nets[i], target.transform);
            entityList.Add(JumperScript);
        }

    }

    void InitEntityNeuralNetworks()
    {
        //population must be even, just setting it to 20 incase it's not
        if (populationSize % 2 != 0)
        {
            populationSize = populationSize - 1;
        }

        nets = new List<NeuralNetwork>();

        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            net.Mutate();
            nets.Add(net);
        }
    }
}
