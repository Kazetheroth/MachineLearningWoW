using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Games.Global.Weapons;
using UnityEngine;

[ExecuteInEditMode]
public class MachineLearningController : MonoBehaviour
{
    [SerializeField] private ObjectPooler spherePooler;
    
    [SerializeField] private TestCaseOption testCaseOption;
    [SerializeField] private int epochs = 100000;
    [SerializeField] private double learningRate = 0.1;
    [SerializeField] private bool isClassification = true;

    private List<GameObject> activateGameObjects;

    private void Update()
    {
        if (!Application.isPlaying)
        {
            if (MachineLearningTestCase.mlcInstance == null)
            {
                InitMLController();
            }
        }
    }

    private void InitMLController()
    {
        Debug.Log("Init ML controller");
        activateGameObjects = new List<GameObject>();
        MachineLearningTestCase.mlcInstance = this;
    }

    public void RunMachineLearningTestCase()
    {
        double[] resultXorTest = MachineLearningTestCase.RunMachineLearningTestCase(testCaseOption, epochs, learningRate, isClassification);

        if (resultXorTest == null || resultXorTest.Length == 0)
        {
            Debug.Log("Doesn't work");
        }
        else
        {
            for (int i = 0; i < resultXorTest.Length; ++i)
            {
                Debug.Log(resultXorTest[i]);
                ColorSphere(i, resultXorTest[i]);
            }
        }
    }

    public void InstantiateSpheresInScene(List<double> samples, int sampleSize)
    {
        ResetSphere();

        int allSample = samples.Count;
        int nbParametersInSample = allSample / sampleSize;
        
        Debug.Log(allSample);
        Debug.Log(nbParametersInSample);
        Debug.Log(sampleSize);

        for (int i = 0; i < allSample; i += nbParametersInSample)
        {
            PlaceSphere(samples[i], nbParametersInSample > 1 ? samples[i + 1] : 0, nbParametersInSample > 2 ? samples[i + 2] : 0);
        }
    }

    public void ResetSphere()
    {
        if (activateGameObjects == null)
        {
            return;
        }

        foreach (GameObject activateGameObject in activateGameObjects)
        {
            activateGameObject.SetActive(false);
        }
        
        activateGameObjects.Clear();
    }

    private void PlaceSphere(double x, double y, double z)
    {
        GameObject newSphere = spherePooler.GetPooledObject(0);

        Vector3 pos = newSphere.transform.position;
        pos.x = (float) x * 10;
        pos.y = (float) y * 10;
        pos.z = (float) z * 10;
        newSphere.transform.position = pos;
        
        newSphere.SetActive(true);
        activateGameObjects.Add(newSphere);
    }

    private void ColorSphere(int index, double result)
    {
        Renderer renderer = activateGameObjects[index].GetComponent<Renderer>();

        if (result >= 0.6)
        {
            Material greenMat = new Material(renderer.sharedMaterial) {color = Color.green};
            renderer.sharedMaterial = greenMat;
        }
        else if (result <= -0.6)
        {
            Material redMat = new Material(renderer.sharedMaterial) {color = Color.red};
            renderer.sharedMaterial = redMat;
        }
    }
}
