using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [CustomEditor(typeof(MachineLearningController))]
    public class MachineLearningTestCaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MachineLearningController mlc = (MachineLearningController) target;
            if (GUILayout.Button("Lancer le training"))
            {
                mlc.RunMachineLearningTestCase();
            }

            if (GUILayout.Button("Clear la scene"))
            {
                mlc.ResetSphere();
            }

            if (GUILayout.Button("Simulate test"))
            {
                mlc.SimulateResultTest();
            }

            if (GUILayout.Button("Load images"))
            {
                //mlc.LoadImagesSimple();
                mlc.LoadImagesClasses();
            }
            
//            if (GUILayout.Button("CIOMPUTE images"))
//            {
//                mlc.CiomputeImage();
//            }
//            
//            if (GUILayout.Button("Generate and save rbf model"))
//            {
//                mlc.GenerateWeights();
//            }
            
//            if (GUILayout.Button("Tester une image"))
//            {
//                mlc.ChooseImage();
//            }
        }
    }
}