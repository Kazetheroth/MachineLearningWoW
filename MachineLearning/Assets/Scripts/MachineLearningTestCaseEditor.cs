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
        }
    }
}