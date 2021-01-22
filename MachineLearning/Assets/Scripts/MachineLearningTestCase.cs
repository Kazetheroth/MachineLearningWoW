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
        MULTI_LINEAR_3_CLASSES,
        MULTI_CROSS,
        LINEAR_SIMPLE_2D,
        NON_LINEAR_SIMPLE_2D,
        LINEAR_SIMPLE_3D,
        LINEAR_TRICKY_3D,
        NON_LINEAR_SIMPLE_3D
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
        public static TestCaseParameters lastTestCaseParameters;
        
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
                        Y = new List<double> {1, 2, 2, 1},
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
                case TestCaseOption.MULTI_CROSS:
                    xArray = new List<double>();
                    yArray = new List<double>();

                    for (int i = 0; i < 1000; ++i)
                    {
                        xArray.Add(Random.Range(-1.0f, 1.0f));
                        xArray.Add(Random.Range(-1.0f, 1.0f));

                        int xIndex = i * 2;
                        int yIndex = i * 2 + 1;

                        if (Math.Abs(xArray[xIndex]) % 0.5 <= 0.25 && Math.Abs(xArray[yIndex]) % 0.5 > 0.25)
                        {
                            yArray.Add(1);
                            yArray.Add(0);
                            yArray.Add(0);
                        } else if (Math.Abs(xArray[xIndex]) % 0.5 > 0.25 && Math.Abs(xArray[yIndex]) % 0.5 <= 0.25)
                        {
                            yArray.Add(0);
                            yArray.Add(1);
                            yArray.Add(0);
                        } else
                        {
                            yArray.Add(0);
                            yArray.Add(0);
                            yArray.Add(1);
                        }
                    }
                    
                    return new TestCaseParameters
                    {
                        neuronsPerLayer = new List<int> {2, 3, 3, 3},
                        nplSize = 4,
                        X = xArray,
                        Y = yArray,
                        sampleSize = 1000,
                        isClassification = true
                    };
                case TestCaseOption.LINEAR_SIMPLE_2D:
                    return new TestCaseParameters
                    {
                        neuronsPerLayer = new List<int> {1, 1},
                        nplSize = 2,
                        X = new List<double> {1, 2},
                        Y = new List<double> {2, 3},
                        sampleSize = 2,
                        isClassification = false
                    };
                case TestCaseOption.NON_LINEAR_SIMPLE_2D:
                    return new TestCaseParameters
                    {
                        neuronsPerLayer = new List<int> {1, 3, 1},
                        nplSize = 3,
                        X = new List<double> {1, 2, 3},
                        Y = new List<double> {2, 3, 2.5},
                        sampleSize = 3,
                        isClassification = false
                    };
                case TestCaseOption.LINEAR_SIMPLE_3D:
                    return new TestCaseParameters
                    {
                        neuronsPerLayer = new List<int> {2, 1},
                        nplSize = 2,
                        X = new List<double> {1, 1, 2, 2, 3, 1},
                        Y = new List<double> {2, 3, 2.5},
                        sampleSize = 3,
                        isClassification = false
                    };
                case TestCaseOption.LINEAR_TRICKY_3D:
                    return new TestCaseParameters
                    {
                        neuronsPerLayer = new List<int> {2, 1},
                        nplSize = 2,
                        X = new List<double> {1, 1, 2, 2, 3, 3},
                        Y = new List<double> {1, 2, 3},
                        sampleSize = 3,
                        isClassification = false
                    };
                case TestCaseOption.NON_LINEAR_SIMPLE_3D:
                    return new TestCaseParameters
                    {
                        neuronsPerLayer = new List<int> {2, 2, 1},
                        nplSize = 3,
                        X = new List<double> {1, 0, 0, 1, 1, 1, 0, 0},
                        Y = new List<double> {2, 1, -2, -1},
                        sampleSize = 4,
                        isClassification = false
                    };
            }

            return testCaseParameters;
        }

        public static double[] RunMachineLearningTestCase(bool useLinearModel, TestCaseOption testCaseOption, int epochs, double learningRate, TestCaseParameters simulateTestCaseParameters)
        {
            lastTestCaseParameters = simulateTestCaseParameters ?? GetTestOption(testCaseOption);

            if (simulateTestCaseParameters == null)
            {
                Debug.Log("Generate new test case");
            }
            
            if (lastTestCaseParameters == null)
            {
                return null;
            }

            mlcInstance.InstantiateSpheresInScene(lastTestCaseParameters.X, lastTestCaseParameters.sampleSize, null);

            double[] result = new double[lastTestCaseParameters.sampleSize * lastTestCaseParameters.neuronsPerLayer[lastTestCaseParameters.nplSize - 1]];

            IntPtr rawResut;
            if (useLinearModel)
            {
                rawResut = CppImporter.trainLinearModel(
                    lastTestCaseParameters.X.ToArray(),
                    lastTestCaseParameters.neuronsPerLayer[0],
                    lastTestCaseParameters.Y.ToArray(),
                    lastTestCaseParameters.neuronsPerLayer[lastTestCaseParameters.nplSize - 1],
                    lastTestCaseParameters.sampleSize,
                    epochs,
                    learningRate,
                    lastTestCaseParameters.isClassification
                );
                
            }
            else
            {
                rawResut = CppImporter.trainMLPModel(
                    lastTestCaseParameters.neuronsPerLayer.ToArray(),
                    lastTestCaseParameters.nplSize,
                    lastTestCaseParameters.X.ToArray(),
                    lastTestCaseParameters.Y.ToArray(),
                    lastTestCaseParameters.sampleSize,
                    epochs,
                    learningRate,
                    lastTestCaseParameters.isClassification
                );
            }
            Marshal.Copy(rawResut, result, 0, lastTestCaseParameters.sampleSize);

            return result;
        }
    }
}