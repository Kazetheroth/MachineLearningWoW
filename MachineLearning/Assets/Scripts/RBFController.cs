using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class RBFController
{
    public static void TrainRBFodel(MachineLearningController controller, List<double> images, int nbPixelInImage,
        int nbImages, List<double> output, int nbCentroid, int nbClasses, int maxKmeans, float gamma)
    {
        IntPtr rawResut;
        rawResut = CppImporter.trainRBFModel(
            images.ToArray(),
            nbImages,
            nbPixelInImage,
            output.ToArray(), 
            nbCentroid, 
            nbClasses, 
            maxKmeans, 
            images.ToArray(), 
            output.ToArray(),
            gamma);

        double[] result = new double[output.Count];
        Marshal.Copy(rawResut, result, 0, output.Count);
        
        Debug.Log("Result rbf");
        Debug.Log(result.ToList().Count);
        foreach (var data in result)
        {
            Debug.Log(data);
        }
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
        
        Debug.Log("Result centroids ");
        foreach (var data in result)
        {
            Debug.Log(data);
        }

        return result;
    }

    public static void ComputeAndSaveWeights(List<double> images, int nbPixelInImage,
        int nbImages, List<double> output, int nbCentroid, int nbClasses, int maxKmeans, float gamma, List<double> centroids)
    {
        IntPtr rawResut;
        rawResut = CppImporter.trainRbfModelGetWeigths(
            images.ToArray(),
            nbImages,
            nbPixelInImage,
            output.ToArray(),
            nbCentroid,
            nbClasses,
            maxKmeans,
            gamma,
            centroids.ToArray());

        double[] result = new double[output.Count];
        Marshal.Copy(rawResut, result, 0, output.Count);
        
        Debug.Log("Result weights");
        Debug.Log(result.ToList().Count);
        foreach (var data in result)
        {
            Debug.Log(data);
        }
    }
}