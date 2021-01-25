using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class RBFController
{
    public static double[] TrainRBFodel(MachineLearningController controller, TestCaseParameters param, List<double> outputToTest = null, int outputSize = 0)
    {
        MachineLearningTestCase.lastTestCaseParameters = param;
        if (param.neuronsPerLayer[0] < 3)
        {
            controller.InstantiateSpheresInScene(param.X, param.sampleSize, null);
        }

        int eachOutputSize = param.neuronsPerLayer[param.neuronsPerLayer.Count - 1];
        List<double> outputTest = (outputToTest == null || outputToTest.Count == 0) ? param.X : outputToTest;
        int outputTotalSize = (outputToTest == null || outputToTest.Count == 0) ? param.sampleSize * eachOutputSize: outputSize;

        IntPtr rawResut;
        rawResut = CppImporter.trainRBFModel(
            param.X.ToArray(),
            param.sampleSize,
            param.neuronsPerLayer[0],
            param.Y.ToArray(),
            param.nbCentroids,
            param.nbClasses,
            param.maxKmeans,
            outputTest.ToArray(),
            param.Y.ToArray(),
            param.gamma,
            outputTotalSize);

        
        double[] result = new double[outputTotalSize];
        Marshal.Copy(rawResut, result, 0, outputTotalSize);
        
        Debug.Log("Result rbf ");

        return result;
    }

    public static void StartGenerationRBFModel(MachineLearningController controller, List<double> images, int nbPixelInImage,
        int nbImages, List<double> output, int nbCentroid, int nbClasses, int maxKmeans, float gamma)
    {
        double[] centroid = ComputeAndSaveCentroids(images, nbImages, nbPixelInImage, nbCentroid, maxKmeans);
        ComputeAndSaveWeights(images, nbPixelInImage, nbImages, output, nbCentroid, nbClasses, maxKmeans, gamma, centroid.ToList());
    }

    public static double[] ComputeAndSaveCentroids(List<double> X, int nbImages, int nbPixelInImage, int nbCentroid, int maxKmeans)
    {
        IntPtr rawResut;
        rawResut = CppImporter.trainRBFModelGetCentroids(X.ToArray(), nbImages, nbPixelInImage, nbCentroid, maxKmeans);
        
        double[] result = new double[nbCentroid * nbPixelInImage];
        Marshal.Copy(rawResut, result, 0, nbCentroid * nbPixelInImage);

        return result;
    }

    public static void ComputeAndSaveWeights(List<double> images, int nbPixelInImage,
        int nbImages, List<double> output, int nbCentroid, int nbClasses, int maxKmeans, float gamma, List<double> centroids)
    {
        IntPtr rawResut;
        rawResut = CppImporter.trainRbfModelGetWeights(
            images.ToArray(),
            nbImages,
            nbPixelInImage,
            output.ToArray(),
            nbCentroid,
            nbClasses,
            maxKmeans,
            gamma,
            centroids.ToArray());

//        double[] nbWeigths = new double[1];
//        Marshal.Copy(rawResut, nbWeigths, 0, 1);
//        
//        Debug.Log("Size " + nbWeigths[0]);
    }
}