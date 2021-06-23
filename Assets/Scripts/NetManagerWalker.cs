using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetManagerWalker : MonoBehaviour
{
    public GameObject entityPrefab;
    public GameObject target;
    public GameObject theWall;
    private Vector3 wallOriginalPosition;

    private bool isTraning = false;
    public int populationSize = 20;
    private int generationNumber = 0;
    public int[] layers = new int[] { 7, 20, 20, 4 }; //No. of inputs and No. of outputs
    private List<NeuralNetwork> nets;
    [HideInInspector]
    public List<WalkerScript> entityList = null;

    public int amntLeft;

    public TMP_Text generationText;

    [Range(1f, 8)]
    public float timeScale = 1;

    public float topDistance;

    public Material[] placingMaterials;

    public float timer;

    public bool runEffectiveLearning;

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

        Time.timeScale = timeScale;

        if(entityList != null)
        {
            foreach (WalkerScript scrpt in entityList)
            {
                if (scrpt.distanceTravelled > topDistance && !scrpt.failed && scrpt != null)
                {
                    topDistance = scrpt.distanceTravelled;
                    //scrpt.GetComponent<MeshRenderer>().material = placingMaterials[1];
                    cameraObj.GetComponent<CameraFollow>().target = scrpt.gameObject.transform.GetChild(0).transform;
				}

				if(scrpt.failed)
				{
                    if(scrpt.distanceTravelled >= topDistance)
					{
                        topDistance = 0;
                        scrpt.distanceTravelled = 0;
                        //cameraObj.GetComponent<CameraFollow>().target = theWall.transform;
                    }
                }

				if(scrpt.distanceTravelled < topDistance)
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
					for (int i = populationSize / 2; i < populationSize - 4; i++) //Gathers all but best 2 nets
					{
						nets[i] = new NeuralNetwork(nets[populationSize - 1]);
						nets[i].Mutate();
					}
				}

                if (runEffectiveLearning)
                {
                    //for (int i = 0; i < (populationSize - 2) / 2; i++) //Gathers all but best 2 nets
                    //{
                    //    nets[i] = new NeuralNetwork(nets[i + (populationSize - 2) / 2]);     //Copies weight values from top half networks to worst half
                    //    nets[i].Mutate();                                                    //Mutates new entities

                    //    nets[i + (populationSize - 2) / 2] = new NeuralNetwork(nets[populationSize - 1]);
                    //    nets[i + (populationSize - 2) / 2].Mutate();

                    //    nets[populationSize - 1] = new NeuralNetwork(nets[populationSize - 1]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
                    //    nets[populationSize - 2] = new NeuralNetwork(nets[populationSize - 2]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
                    //}

                    for (int i = 2; i < (populationSize - 2) / 2; i++) //Gathers all but best 2 nets
                    {
                        nets[i] = new NeuralNetwork(nets[populationSize - 2]);
                        nets[i].Mutate();                                                    //Mutates new entities

                        nets[populationSize - 1] = new NeuralNetwork(nets[populationSize - 1]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
                        nets[populationSize - 2] = new NeuralNetwork(nets[populationSize - 2]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
                        nets[populationSize - 2].Mutate();
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
            Invoke("Timer", 15);
            timer = 15;
            CreateEntityBodies();
            theWall.transform.position = wallOriginalPosition;
            theWall.GetComponent<WallMoveScript>().moveSpeed = 2;
		}

        amntLeft = populationSize;
		foreach (WalkerScript emt in entityList)
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
            timer = 15;
            //cameraObj.GetComponent<CameraFollow>().target = theWall.transform;
        }

        if(timer > 0)
		{
            timer -= Time.deltaTime;
		}
    }

    void Timer()
    {
        if(timer <= 0)
        {
            isTraning = false;
            cameraObj.GetComponent<CameraFollow>().target = theWall.transform;
        }
		else
		{
            Invoke("Timer", 15);
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

        entityList = new List<WalkerScript>();

        for (int i = 0; i < populationSize; i++)
        {
            WalkerScript walkerScript = ((GameObject)Instantiate(entityPrefab, new Vector3(0, 0.27f, 0), entityPrefab.transform.rotation)).GetComponent<WalkerScript>();
            walkerScript.Init(nets[i], target.transform);
            entityList.Add(walkerScript);
        }

    }

    void InitEntityNeuralNetworks()
    {
        //population must be even, just setting it to 20 incase it's not
        if (populationSize % 2 != 0)
        {
            populationSize = populationSize + 1;
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
