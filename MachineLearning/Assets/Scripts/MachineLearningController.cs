using System;
using System.Collections.Generic;
using DefaultNamespace;
using Games.Global.Weapons;
using UnityEngine;

public class MachineLearningController : MonoBehaviour
{
    [SerializeField] private ObjectPooler spherePooler;
    
    [SerializeField] private TestCaseOption testCaseOption;
    [SerializeField] private int epochs = 100000;
    [SerializeField] private double learningRate = 0.1;
    [SerializeField] private bool isClassification = true;

    private List<GameObject> activateGameObjects = new List<GameObject>();

    public void RunMachineLearningTestCase()
    {
        ResetSphere();
        
        double[] resultXorTest = MachineLearningTestCase.RunMachineLearningTestCase(testCaseOption, epochs, learningRate, isClassification);

        if (resultXorTest == null)
        {
            Debug.Log("Doesn't work");
        }
        else
        {
            for (int i = 0; i < resultXorTest.Length; ++i)
            {
                Debug.Log(resultXorTest[i]);
                PlaceSphere(i, resultXorTest[i]);
            }
        }
    }

    private void ResetSphere()
    {
        foreach (GameObject activateGameObject in activateGameObjects)
        {
            activateGameObject.SetActive(false);
        }
    }

    private void PlaceSphere(int index, double result)
    {
        GameObject newSphere = spherePooler.GetPooledObject(0);

        Vector3 pos = newSphere.transform.position;
        pos.x = index * 2;
        pos.y = (float) result;
        newSphere.transform.position = pos;
        
        newSphere.SetActive(true);
        activateGameObjects.Add(newSphere);
    }
}
