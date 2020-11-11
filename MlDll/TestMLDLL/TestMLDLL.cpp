// TestMLDLL.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>

#include "MLP.h";

using namespace std;

extern "C"
{
    __declspec(dllimport) double my_add(double a, double b);
}

vector<double> createVector(double* X, int indexStart, int indexEnd) {
    vector<double> nVector;

    for (int i = indexStart; i < indexEnd; ++i) {
        nVector.push_back(X[i]);
    }

    return nVector;
}

int main()
{
    int* neuronsPerLayer = new int[] {
        2, 3, 1
    };

    MLP* mlp = new MLP(neuronsPerLayer, 3);
    //mlp->displayWeights();

    double* X = new double[] {
        0, 0,
            0, 1,
            1, 0,
            1, 1
    };

    double* Y = new double[] {
        -1,
            1,
            1,
            -1
    };

    int sampleSize = 4;
    cout << "BEFORE TRAINING" << endl;

    for (int k = 0; k < sampleSize; ++k) {
        mlp->forwardPass(createVector(X, k * 2, 2 * (k + 1)), true);
        mlp->displayInputs();
    }

    mlp->train(X, Y, sampleSize, true, 100000, 0.1);

    cout << "AFTER TRAINING" << endl;
    for (int k = 0; k < sampleSize; ++k) {
        mlp->forwardPass(createVector(X, k * 2, 2 * (k + 1)), true);
        mlp->displayInputs();
    }
}
