using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RBFController
{
    public static void TrainRBFodel(MachineLearningController controller, List<double> images, int nbPixelInImage, List<double> output)
    {
        IntPtr rawResut;
        rawResut = CppImporter.trainRBFModel(
            images.ToArray(), 
            nbPixelInImage, 
            output.ToArray(), 
            3, 
            2, 
            1000, 
            images.ToArray(), 
            output.ToArray());

        double[] result = new double[output.Count];
        Marshal.Copy(rawResut, result, 0, output.Count);
        
        Debug.Log("Result mothafucka");
        Debug.Log(result);
    }
}