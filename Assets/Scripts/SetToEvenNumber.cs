using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetToEvenNumber : MonoBehaviour
{
    public Slider slider;

    [ExecuteInEditMode]
    void Update()
    {
        if(slider.value % 2 != 0)
		{
            slider.value = slider.value - 1;
		}
    }
}
