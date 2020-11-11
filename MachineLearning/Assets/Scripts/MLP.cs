using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MLP
{
    private List<int> neuronsPerLayer;
    private int nbLayers;

    private double[] weights;
    private double[] inputs;
    private double[] deltas;
    
    MLP(List<int> npl)
    {
        neuronsPerLayer = npl;
        nbLayers = neuronsPerLayer.Count;

        int nbWeights = GetNbWeights();
        int nbInputs = GetNbInputs();
        
        var calculatedWeight = CppImporter.createWeights(nbWeights);
        var calculatedInputs = CppImporter.createInputs(nbInputs);
        var calculatedLabels = CppImporter.createInputs(nbInputs);

        Marshal.Copy(calculatedWeight, weights, 0, nbWeights);
        Marshal.Copy(calculatedInputs, inputs, 0, nbInputs);
        Marshal.Copy(calculatedLabels, deltas, 0, nbInputs);
    }

    private int GetNbWeights()
    {
        int nbWeight = 0;

        for (int i = 1; i < nbLayers; ++i) {
            nbWeight += (neuronsPerLayer[i - 1] + 1) * (neuronsPerLayer[i] + 1);
        }

        return nbWeight;
    }

    private int GetNbInputs()
    {
        int nbInputs = 0;

        for (int i = 1; i < nbLayers; ++i) {
            nbInputs += neuronsPerLayer[i] + 1;
        }

        return nbInputs;
    }
}
