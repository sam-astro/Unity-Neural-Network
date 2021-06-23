using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyNetworkManagers : MonoBehaviour
{
    public GameObject entityPrefab;
    public GameObject target;

    private bool isTraning = false;
    public int populationSize;
    private int generationNumber = 0;
    public int[] layers = new int[] { 7, 20, 20, 2 }; //No. of inputs and No. of outputs
    private List<NeuralNetwork> nets;
    private bool leftMouseDown = false;
    private List<EntityMovementTwo> entityList = null;

    public int amntLeft;

    public TMP_Text generationText;

    [Range(1f, 8)]
    public float timeScale = 1;

    public float topDistance;

    public Material[] placingMaterials;

    float timer;

    public bool runEffectiveLearning = true;

    public Slider populationSlider;
    public Toggle learnMethodToggle;

    void Update()
    {
        generationText.text = generationNumber.ToString();
        populationSize = Mathf.RoundToInt(populationSlider.value);
        runEffectiveLearning = learnMethodToggle;

        Time.timeScale = timeScale;

        if(entityList != null)
        {
            foreach (EntityMovementTwo scrpt in entityList)
            {
                if (scrpt.distanceTravelled > topDistance && !scrpt.failed && scrpt != null)
                {
                    topDistance = scrpt.distanceTravelled;
                    //scrpt.GetComponent<MeshRenderer>().material = placingMaterials[1];
                    //Camera.main.GetComponent<CameraFollow>().target = scrpt.transform;
                }

				if(scrpt.failed)
				{
                    if(scrpt.distanceTravelled >= topDistance)
					{
                        topDistance = 0;
                    }
                    //Camera.main.GetComponent<CameraFollow>().target = entityList[0].transform;
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
                    //nets[0].topFitness = true;
                    for (int i = populationSize / 2; i < populationSize - 1; i++)
                    {
                        //nets[i] = new NeuralNetwork(nets[i + (populationSize / 2)]);
                        nets[i] = nets[populationSize - 1];
                        //nets[i].Mutate();
                        nets[i - populationSize / 2].Mutate();

                        //nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
                    }
                    //nets[0] = nets[populationSize - 1];
                }

                if (runEffectiveLearning)
                {
                    for (int i = 0; i < populationSize / 2; i++)
                    {
                        nets[i] = new NeuralNetwork(nets[i + (populationSize / 2)]);
                        nets[i + (populationSize / 2)].Mutate();                                  //Mutates best entities

                        nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
                    }
                }

                for (int i = 0; i < populationSize; i++)
                {
                    nets[i].SetFitness(0f);
                }
            }

            generationNumber++;
            topDistance = 0;
            isTraning = true;
            Invoke("Timer", 30);
            timer = 30;
            CreateEntityBodies();
            //Camera.main.GetComponent<CameraFollow>().target = entityList[0].transform;
        }

        amntLeft = populationSize;
		foreach (EntityMovementTwo emt in entityList)
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
            timer = 30;
            //Camera.main.GetComponent<CameraFollow>().target = entityList[0].transform;
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

        entityList = new List<EntityMovementTwo>();

        for (int i = 0; i < populationSize; i++)
        {
            EntityMovementTwo entityMovement = ((GameObject)Instantiate(entityPrefab, new Vector3(0, 0, 0), entityPrefab.transform.rotation)).GetComponent<EntityMovementTwo>();
            entityMovement.Init(nets[i], target.transform);
            entityList.Add(entityMovement);
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
