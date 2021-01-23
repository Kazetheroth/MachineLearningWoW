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
        
        Debug.Log("Result mothafucka");
        Debug.Log(result.ToList().Count);
        foreach (var data in result)
        {
            Debug.Log(data);
        }
    }
}