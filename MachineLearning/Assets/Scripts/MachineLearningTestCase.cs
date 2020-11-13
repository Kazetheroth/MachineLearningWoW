using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public enum TestCaseOption
    {
        LINEAR_SIMPLE,
        LINEAR_MULTIPLE,
        XOR,
        CROSS,
        MULTI_CROSS
    }
    
    public class TestCaseParameters
    {
        public List<int> neuronsPerLayer;
        public int nplSize;
        public List<double> X;
        public List<double> Y;
        public int sampleSize;
    }

    public static class MachineLearningTestCase
    {
        public static TestCaseParameters GetTestOption(TestCaseOption testCaseOption)
        {
            TestCaseParameters testCaseParameters = null;
            
            switch (testCaseOption)
            {
                case TestCaseOption.LINEAR_SIMPLE:
                    return new TestCaseParameters
                    {
                        neuronsPerLayer = new List<int> {2, 1},
                        nplSize = 2,
                        X = new List<double> {1, 1, 2, 3, 3, 3},
                        Y = new List<double> {1, -1, -1},
                        sampleSize = 3
                    };
                case TestCaseOption.LINEAR_MULTIPLE:
                    List<double> Xarray = new List<double>();
                    List<double> Yarray = new List<double>();

                    for (int i = 0; i < 100; ++i)
                    {
                        Xarray.Add(Random.Range(0, 1) + (i < 50 ? 0 : 2));
                        Xarray.Add(Random.Range(0, 1) + (i < 50 ? 0 : 2));
                        Yarray.Add(i < 50 ? 1 : -1);
                    }

                    return new TestCaseParameters
                    {
                        neuronsPerLayer = new List<int> {2, 1},
                        nplSize = 2,
                        X = Xarray,
                        Y = Yarray,
                        sampleSize = 100
                    };
                case TestCaseOption.XOR:
                    return new TestCaseParameters
                    {
                        neuronsPerLayer = new List<int> {2, 3, 1},
                        nplSize = 3,
                        X = new List<double> {0, 0, 0, 1, 1, 0, 1, 1},
                        Y = new List<double> {-1, 1, 1, -1},
                        sampleSize = 4
                    };
                case TestCaseOption.CROSS:
                    return new TestCaseParameters
                    {

                    };
                case TestCaseOption.MULTI_CROSS:
                    return new TestCaseParameters
                    {

                    };
            }

            return testCaseParameters;
        }

        public static double[] RunMachineLearningTestCase(TestCaseOption testCaseOption, int epochs, double learningRate, bool isClassification)
        {
            TestCaseParameters testCaseParameters = GetTestOption(testCaseOption);

            if (testCaseParameters == null)
            {
                return null;
            }
            
            double[] result = new double[testCaseParameters.sampleSize];

            IntPtr rawResut = CppImporter.trainMLPModel(
                testCaseParameters.neuronsPerLayer.ToArray(),
                testCaseParameters.nplSize,
                testCaseParameters.X.ToArray(),
                testCaseParameters.Y.ToArray(),
                testCaseParameters.sampleSize,
                epochs,
                learningRate,
                isClassification
            );
            Marshal.Copy(rawResut, result, 0, testCaseParameters.sampleSize);
  
            return result;
        }
    }
}