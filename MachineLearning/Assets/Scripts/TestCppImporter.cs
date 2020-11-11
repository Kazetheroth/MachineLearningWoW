using System.Runtime.InteropServices;
using UnityEngine;

public class TestCppImporter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(CppImporter.MyAdd(42.0, 51.0));
    }

    // Update is called once per frame
    void Update()
    {
        var model = CppImporter.CreateLinearModel(4096);
        var inputs = new double[] {1.0, 2.0, 3.0};
        var result = CppImporter.PredictLinearModelMultiClassClassification(model,
            inputs, 3, 3);
        
        var result_managed = new double[3];
        
        Marshal.Copy(result, result_managed, 0, 3);

        foreach (var rslt in result_managed)
        {
            Debug.Log(rslt);
        }
        
        CppImporter.DeleteLinearModel(result);
        
        CppImporter.DeleteLinearModel(model);
    }
}
