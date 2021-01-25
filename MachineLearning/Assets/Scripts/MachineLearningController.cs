using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Games.Global.Weapons;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class MachineLearningController : MonoBehaviour
{
    [SerializeField] private ObjectPooler spherePooler;
    
    [SerializeField] private TestCaseOption testCaseOption;
    [SerializeField] private AlgoUsed algoUsed;
    [SerializeField] private int epochs = 100000;
    [SerializeField] private double learningRate = 0.1;

    [SerializeField] private double gamma = 0.1;
    [SerializeField] private int nbCentroid = 12;

    private List<GameObject> activateGameObjects;

    private TestCaseParameters simulateTestCaseParameters;

    public int totalWantedImages;
    public int totalWantedImagesToTest;
    
    private List<double> images;
    private List<double> imageOutputs;
    private int nbPixelInImage;
    public int nbImagesLoaded;

    private int nbImagesToTestLoaded;
    private List<double> imagesToTest;
    
    public void LoadImagesClasses()
    {
        Debug.Log("Load Images");

        nbImagesToTestLoaded = 0;
        nbImagesLoaded = 0;
        images = new List<double>();
        imageOutputs = new List<double>();
        imagesToTest = new List<double>();

        int defaultValue = testCaseOption == TestCaseOption.USE_IMAGES ? 0 : -1;

        Texture2D[] druids = Resources.LoadAll<Texture2D>("train/druid32x32");
        nbPixelInImage = druids[0].GetPixels().Length;

        foreach (var image in druids)
        {
            if (nbImagesLoaded < totalWantedImages / 2)
            {
                imageOutputs.Add(defaultValue);
                nbImagesLoaded++;
                foreach (Color pixel in image.GetPixels())
                {
                    images.Add(pixel.r);
                }
            } else if (nbImagesToTestLoaded < totalWantedImagesToTest / 2)
            {
                Debug.Log("Want to find Druid");
                nbImagesToTestLoaded++;
                foreach (Color pixel in image.GetPixels())
                {
                    imagesToTest.Add(pixel.r);
                }
            }
            else
            {
                break;
            }
        }

        Texture2D[] paladins = Resources.LoadAll<Texture2D>("train/paladin32x32");
        nbPixelInImage = paladins[0].GetPixels().Length;
        foreach (var image in paladins)
        {
            if (nbImagesLoaded < totalWantedImages)
            {
                imageOutputs.Add(1);
                nbImagesLoaded++;
                foreach (Color pixel in image.GetPixels())
                {
                    images.Add(pixel.r);
                }
            } 
            else if (nbImagesToTestLoaded < totalWantedImagesToTest)
            {
                Debug.Log("Want to find Paladin");
                nbImagesToTestLoaded++;
                foreach (Color pixel in image.GetPixels())
                {
                    imagesToTest.Add(pixel.r);
                }
            }
            else
            {
                break;
            }
        }
        Debug.Log("done");
    }

    public void GenerateWeights()
    {
        Debug.Log("Start Generate weights");
        if (images == null && images.Count > 0)
        {
            return;
        }

        //List<double> output = new List<double> {0, 1, 1, 1, 0};
        //TestCaseParameters test = MachineLearningTestCase.GetTestOption(TestCaseOption.ImagesTest);

        RBFController.StartGenerationRBFModel(this, images, nbPixelInImage,nbImagesLoaded , imageOutputs, 6,2,1000,0.01f);
        //RBFController.TrainRBFodel(this, test.X, test.nplSize,test.sampleSize , test.Y, 2,2,100,0.1f);
    }

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
        double[] resultMLTestCase;

        if (testCaseOption == TestCaseOption.USE_IMAGES && (images == null || images.Count == 0))
        {
            Debug.LogError("Images not loaded");
        }

        if (totalWantedImagesToTest == 0)
        {
            imagesToTest = null;
        }
        
        TestCaseParameters param = MachineLearningTestCase.GetTestOption(testCaseOption, algoUsed);

        if (testCaseOption == TestCaseOption.USE_IMAGES)
        {
            param.neuronsPerLayer[0] = nbPixelInImage;
            param.neuronsPerLayer[param.neuronsPerLayer.Count - 1] = 1;
            param.X = images;
            param.Y = imageOutputs;
            param.sampleSize = nbImagesLoaded;
            param.gamma = (float)gamma;
            param.nbCentroids = nbCentroid;
        }

        if (algoUsed == AlgoUsed.RBF)
        {
            resultMLTestCase = RBFController.TrainRBFodel(this, param, imagesToTest, nbImagesToTestLoaded);
        }
        else
        {
            resultMLTestCase = MachineLearningTestCase.RunMachineLearningTestCase(algoUsed == AlgoUsed.LINEAR, param, epochs, learningRate, simulateTestCaseParameters, algoUsed, imagesToTest, nbImagesToTestLoaded);
        }

        if (testCaseOption != TestCaseOption.USE_IMAGES && testCaseOption != TestCaseOption.IMAGES_TEST)
        {
            DisplayOutput(resultMLTestCase);
        }
        else
        {
            foreach (var data in resultMLTestCase)
            {
                Debug.Log(data < 0.1 ? "Druid" : "Paladin");
            }
        }

        simulateTestCaseParameters = null;
    }

    public void SimulateResultTest()
    {
        simulateTestCaseParameters = MachineLearningTestCase.GetTestOption(testCaseOption, algoUsed);
        MachineLearningTestCase.lastTestCaseParameters = simulateTestCaseParameters;

        if (testCaseOption != TestCaseOption.IMAGES_TEST && testCaseOption != TestCaseOption.USE_IMAGES)
        {
            InstantiateSpheresInScene(simulateTestCaseParameters.X, simulateTestCaseParameters.sampleSize, simulateTestCaseParameters.Y);
        }
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
        }
        
        if (expectedOutputSimulation != null)
        {
            DisplayOutput(expectedOutputSimulation.ToArray());
        }
    }

    private void DisplayOutput(double[] resultMLTestCase)
    {
        if (resultMLTestCase == null || resultMLTestCase.Length == 0)
        {
            Debug.Log("Doesn't work");
        }
        else if (MachineLearningTestCase.lastTestCaseParameters != null && MachineLearningTestCase.lastTestCaseParameters.isClassification)
        {
            Debug.Log("Nb result : " + resultMLTestCase.Length);
            for (int i = 0; i < resultMLTestCase.Length; ++i)
            {
                if (testCaseOption == TestCaseOption.MULTI_LINEAR_3_CLASSES || testCaseOption == TestCaseOption.MULTI_CROSS)
                {
                    ColorMultiCrossSphere(i / 3, resultMLTestCase[i++], resultMLTestCase[i++], resultMLTestCase[i]);
                }
                else
                {
                    Debug.Log(resultMLTestCase[i]);
                    ColorSphere(i, resultMLTestCase[i]);
                }
            }
        }
        else
        {
            for (int i = 0; i < resultMLTestCase.Length; ++i)
            {
                ReplaceSphere(i, resultMLTestCase[i], MachineLearningTestCase.lastTestCaseParameters.neuronsPerLayer[0] + 1);
            }
        }
    }

    public void ResetSphere()
    {
        images?.Clear();
        imageOutputs?.Clear();
        nbPixelInImage = 0;
        nbImagesLoaded = 0;

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

    private void ReplaceSphere(int index, double newPos, int indexPos)
    {
        GameObject currentSphere = activateGameObjects[index];
        
        Vector3 pos = currentSphere.transform.position;
        if (indexPos == 1)
        {
            pos.x = (float) newPos * 10;
        } 
        else if (indexPos == 2)
        {
            pos.y = (float) newPos * 10;
        }
        else
        {
            pos.z = (float) newPos * 10;
        }

        currentSphere.transform.position = pos;
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
