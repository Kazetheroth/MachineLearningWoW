using System.Runtime.InteropServices;
using DefaultNamespace;

public static class CppImporter
{
//    [DllImport("MlDll", EntryPoint = "my_add")]
//    public static extern double MyAdd(double a, double b);
//    
//    [DllImport("MlDll", EntryPoint = "create_linear_model")]
//    public static extern System.IntPtr CreateLinearModel(int inputSize);
//    
//    [DllImport("MlDll", EntryPoint = "predict_linear_model_classification")]
//    public static extern double PredictLinearModelClassification(System.IntPtr model,
//        double[] inputs, int inputSize);
//    
//    [DllImport("MlDll", EntryPoint = "predict_linear_model_multiclass_classification")]
//    public static extern System.IntPtr PredictLinearModelMultiClassClassification(System.IntPtr model,
//        double[] inputs, int inputSize, int classCount);
//
//    [DllImport("MlDll", EntryPoint = "delete_linear_model")]
//    public static extern void DeleteLinearModel(System.IntPtr model);

    [DllImport("MlDll", EntryPoint = "train_mlp_model")]
    public static extern System.IntPtr trainMLPModel(int[] neuronsPerLayer, int nplSize, double[] X, double[] Y, int sampleSize, int epochs, double learningRate, bool isClassification);

    [DllImport("MlDll", EntryPoint = "train_linear_model")]
    public static extern System.IntPtr trainLinearModel(double[] X, int inputCount, double[] Y, int outputCount, int sampleSize, int epochs, double learningRate, bool isClassification);

    [DllImport("MlDll", EntryPoint = "train_rbf_model")]
    public static extern System.IntPtr trainRBFModel(double[] X, int nbImages, int nbPixelInImage, double[] Y, int nbCentroid, int nbClasses, int maxKmeans, double[] testX, double[] testY, float gamma);
    
    [DllImport("MlDll", EntryPoint = "train_rbf_model_get_centroids")]
    public static extern System.IntPtr trainRBFModelGetCentroids(double[] X, int nbImages, int nbPixelInImage, int nbCentroid, int maxKmeans);
    
    [DllImport("MlDll", EntryPoint = "train_rbf_model_get_weigths")]
    public static extern System.IntPtr trainRbfModelGetWeigths(double[] X, int nbImages, int nbPixelInImage, double[] Y, int nbCentroid, int nbClasses, int maxKmeans, float gamma, double[] centroids);
    
    [DllImport("MlDll", EntryPoint = "get_rbf_result")]
    public static extern System.IntPtr getRbfResult(double[] X, int nbImages, int nbPixelInImage, double[] Y,double[] centroids, int nbCentroid, int nbClasses, double[] weights, int nbWeights, float gamma);

}
