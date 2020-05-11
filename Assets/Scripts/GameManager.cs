using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider slider;
    public Text spacing;
    public ObjectGenerator objectGenerator;

    void Start()
    {
        
    }

    void Update()
    {
        spacing.text = slider.value.ToString();
        objectGenerator.spacing = slider.value;
    }

    public void ButtonGenerate()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");

        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
        }

        objectGenerator.Start();
    }
}
