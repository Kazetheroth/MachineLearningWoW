using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public enum TestCaseOption
    {
        LINEAR_SIMPLE,
        LINEAR_MULTIPLE,
        XOR,
        CROSS,
        MULTI_LINEAR_3_CLASSES
    }
    
    public class TestCaseParameters
    {
        public List<int> neuronsPerLayer;
        public int nplSize;
        public List<double> X;
        public List<double> Y;
        public int sampleSize;
        public bool isClassification;
    }

    public static class MachineLearningTestCase
    {
        public static MachineLearningController mlcInstance;
        
        public static TestCaseParameters GetTestOption(TestCaseOption testCaseOption)
        {
            TestCaseParameters testCaseParameters = null;
            List<double> xArray;
            List<double> yArray;
            
            switch (testCaseOption)
            {
                case TestCaseOption.LINEAR_SIMPLE:
                    return new TestCaseParameters
                    {
                        neuronsPerLayer = new List<int> {2, 1},
                        nplSize = 2,
                        X = new List<double> {1, 1, 2, 3, 3, 3},
                        Y = new List<double> {1, -1, -1},
                        sampleSize = 3,
                        isClassification = true
                    };
                case TestCaseOption.LINEAR_MULTIPLE:
                    xArray = new List<double>();
                    yArray = new List<double>();

                    for (int i = 0; i < 100; ++i)
                    {
                        xArray.Add(Random.Range(0.0f, 1.0f) + (i < 50 ? 1 : 2));
                        xArray.Add(Random.Range(0.0f, 1.0f) + (i < 50 ? 1 : 2));
                        yArray.Add(i < 50 ? 1 : -1);
                    }

                    return new TestCaseParameters
                    {
                        neuronsPerLayer = new List<int> {2, 1},
                        nplSize = 2,
                        X = xArray,
                        Y = yArray,
                        sampleSize = 100,
                        isClassification = true
                    };
                case TestCaseOption.XOR:
                    return new TestCaseParameters
                    {
                        neuronsPerLayer = new List<int> {2, 3, 1},
                        nplSize = 3,
                        X = new List<double> {0, 0, 0, 1, 1, 0, 1, 1},
                        Y = new List<double> {-1, 1, 1, -1},
                        sampleSize = 4,
                        isClassification = true
                    };
                case TestCaseOption.CROSS:
                    xArray = new List<double>();
                    yArray = new List<double>();

                    for (int i = 0; i < 500; ++i)
                    {
                        xArray.Add(Random.Range(-1.0f, 1.0f));
                        xArray.Add(Random.Range(-1.0f, 1.0f));
                        
                        yArray.Add(Mathf.Abs((float)xArray[i * 2]) <= 0.3 || Mathf.Abs((float)xArray[i * 2 + 1]) <= 0.3 ? 1 : -1);
                    }

                    return new TestCaseParameters
                    {
                        neuronsPerLayer = new List<int> {2, 4, 1},
                        nplSize = 3,
                        X = xArray,
                        Y = yArray,
                        sampleSize = 500,
                        isClassification = true
                    };
                case TestCaseOption.MULTI_LINEAR_3_CLASSES:
                    xArray = new List<double>();
                    yArray = new List<double>();

                    for (int i = 0; i < 500; ++i)
                    {
                        xArray.Add(Random.Range(-1.0f, 1.0f));
                        xArray.Add(Random.Range(-1.0f, 1.0f));

                        int xIndex = i * 2;
                        int yIndex = i * 2 + 1;

                        if (-xArray[xIndex] - xArray[yIndex] - 0.5 > 0 && xArray[yIndex] < 0 &&
                            xArray[xIndex] - xArray[yIndex] - 0.5 < 0)
                        {
                            yArray.Add(1);
                            yArray.Add(0);
                            yArray.Add(0);
                        } else if (-xArray[xIndex] - xArray[yIndex] - 0.5 < 0 && xArray[yIndex] > 0 &&
                                   xArray[xIndex] - xArray[yIndex] - 0.5 < 0)
                        {
                            yArray.Add(0);
                            yArray.Add(1);
                            yArray.Add(0);
                        } else if (-xArray[xIndex] - xArray[yIndex] - 0.5 < 0 && xArray[yIndex] < 0 &&
                                   xArray[xIndex] - xArray[yIndex] - 0.5 > 0)
                        {
                            yArray.Add(0);
                            yArray.Add(0);
                            yArray.Add(1);
                        }
                        else
                        {
                            yArray.Add(0);
                            yArray.Add(0);
                            yArray.Add(0);
                        }
                    }

                    return new TestCaseParameters
                        {
                            neuronsPerLayer = new List<int> {2, 3},
                            nplSize = 2,
                            X = xArray,
                            Y = yArray,
                            sampleSize = 500,
                            isClassification = true
                    };
            }

            return testCaseParameters;
        }

        public static double[] RunMachineLearningTestCase(bool useLinearModel, TestCaseOption testCaseOption, int epochs, double learningRate, TestCaseParameters simulateTestCaseParameters)
        {
            TestCaseParameters testCaseParameters = simulateTestCaseParameters ?? GetTestOption(testCaseOption);

            if (simulateTestCaseParameters == null)
            {
                Debug.Log("Generate new test case");
            }
            
            if (testCaseParameters == null)
            {
                return null;
            }

            mlcInstance.InstantiateSpheresInScene(testCaseParameters.X, testCaseParameters.sampleSize, null);

            double[] result = new double[testCaseParameters.sampleSize * testCaseParameters.neuronsPerLayer[testCaseParameters.nplSize - 1]];

            IntPtr rawResut;
            if (useLinearModel)
            {
                rawResut = CppImporter.trainLinearModel(
                    testCaseParameters.X.ToArray(),
                    testCaseParameters.neuronsPerLayer[0],
                    testCaseParameters.Y.ToArray(),
                    testCaseParameters.neuronsPerLayer[testCaseParameters.nplSize - 1],
                    testCaseParameters.sampleSize,
                    epochs,
                    learningRate,
                    testCaseParameters.isClassification
                );
                
            }
            else
            {
                rawResut = CppImporter.trainMLPModel(
                    testCaseParameters.neuronsPerLayer.ToArray(),
                    testCaseParameters.nplSize,
                    testCaseParameters.X.ToArray(),
                    testCaseParameters.Y.ToArray(),
                    testCaseParameters.sampleSize,
                    epochs,
                    learningRate,
                    testCaseParameters.isClassification
                );
            }
            Marshal.Copy(rawResut, result, 0, testCaseParameters.sampleSize);

            return result;
        }
    }
}