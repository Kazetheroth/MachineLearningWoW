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
    
    [SerializeField] private bool useLinearModel;
    [SerializeField] private TestCaseOption testCaseOption;
    [SerializeField] private int epochs = 100000;
    [SerializeField] private double learningRate = 0.1;

    private List<GameObject> activateGameObjects;

    private TestCaseParameters simulateTestCaseParameters;

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
        double[] resultXorTest = MachineLearningTestCase.RunMachineLearningTestCase(useLinearModel, testCaseOption, epochs, learningRate, simulateTestCaseParameters);

        simulateTestCaseParameters = null;
        
        if (resultXorTest == null || resultXorTest.Length == 0)
        {
            Debug.Log("Doesn't work");
        }
        else
        {
            Debug.Log("Nb result : " + resultXorTest.Length);
            for (int i = 0; i < resultXorTest.Length; ++i)
            {
                if (testCaseOption == TestCaseOption.MULTI_LINEAR_3_CLASSES)
                {
                    ColorMultiCrossSphere(i / 3, resultXorTest[i++], resultXorTest[i++], resultXorTest[i]);
                }
                else
                {
                    Debug.Log(resultXorTest[i]);
                    ColorSphere(i, resultXorTest[i]);
                }
            }
        }
    }

    public void SimulateResultTest()
    {
        simulateTestCaseParameters = MachineLearningTestCase.GetTestOption(testCaseOption);

        InstantiateSpheresInScene(simulateTestCaseParameters.X, simulateTestCaseParameters.sampleSize, simulateTestCaseParameters.Y);
    }

    public void InstantiateSpheresInScene(List<double> samples, int sampleSize, List<double> expectedOutputSimulation)
    {
        ResetSphere();

        int allSample = samples.Count;
        int nbParametersInSample = allSample / sampleSize;

        int index = 0;
        
        for (int i = 0; i < allSample; i += nbParametersInSample)
        {
            PlaceSphere(samples[i], nbParametersInSample > 1 ? samples[i + 1] : 0, nbParametersInSample > 2 ? samples[i + 2] : 0);

            if (expectedOutputSimulation != null)
            {
                if (testCaseOption == TestCaseOption.MULTI_LINEAR_3_CLASSES)
                {
                    ColorMultiCrossSphere(index / 3, expectedOutputSimulation[index++], expectedOutputSimulation[index++], expectedOutputSimulation[index++]);
                }
                else
                {
                    ColorSphere(i / nbParametersInSample, expectedOutputSimulation[i / nbParametersInSample]);
                }
            }
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

    private void ColorMultiCrossSphere(int index, double result1, double result2, double result3)
    {
        Renderer renderer = activateGameObjects[index].GetComponent<Renderer>();

        if (result1 > 0.4 && result1 > result2 && result1 > result3)
        {
            Material greenMat = new Material(renderer.sharedMaterial) {color = Color.green};
            renderer.sharedMaterial = greenMat;
        } else if (result2 > 0.4 && result2 > result1 && result2 > result3)
        {
            Material redMat = new Material(renderer.sharedMaterial) {color = Color.red};
            renderer.sharedMaterial = redMat;
        } else if (result3 > 0.4 && result3 > result1 && result3 > result2)
        {
            Material blueMat = new Material(renderer.sharedMaterial) {color = Color.blue};
            renderer.sharedMaterial = blueMat;
        }
        else
        {
            Material blueMat = new Material(renderer.sharedMaterial) {color = Color.white};
            renderer.sharedMaterial = blueMat;
        }
    }
    
    private void ColorSphere(int index, double result)
    {
        Renderer renderer = activateGameObjects[index].GetComponent<Renderer>();

        if (result >= 0.8)
        {
            Material greenMat = new Material(renderer.sharedMaterial) {color = Color.green};
            renderer.sharedMaterial = greenMat;
        }
        else if (result <= -0.8)
        {
            Material redMat = new Material(renderer.sharedMaterial) {color = Color.red};
            renderer.sharedMaterial = redMat;
        } else {
            Material blueMat = new Material(renderer.sharedMaterial) {color = Color.white};
            renderer.sharedMaterial = blueMat;
        }
    }
}
