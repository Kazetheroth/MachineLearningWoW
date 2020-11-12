// TestMLDLL.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>

using namespace std;

extern "C"
{
    __declspec(dllimport) double my_add(double a, double b);

    __declspec(dllimport) double* train_mlp_model(int* neuronsPerLayer, int nplSize, double* X, double* Y, int sampleSize, int epochs, double learningRate, bool isClassification);
}

int main()
{
    int* neuronsPerLayer = new int[] {
        2, 3, 1
    };

    int nplSize = 3;

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

    double* result = train_mlp_model(neuronsPerLayer, nplSize, X, Y, sampleSize, 1000, 0.1, true);

    cout << "Resultat obtenu" << endl;
    for (int i = 0; i < sampleSize; ++i) {
        cout << result[i] << endl;
    }
}
