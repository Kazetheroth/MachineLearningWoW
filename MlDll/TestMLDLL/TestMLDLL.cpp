// TestMLDLL.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <time.h>

using namespace std;

extern "C"
{
    __declspec(dllimport) double my_add(double a, double b);

    __declspec(dllimport) double* train_mlp_model(int* neuronsPerLayer, int nplSize, double* X, double* Y, int sampleSize, int epochs, double learningRate, bool isClassification);
}

int* neuronsPerLayer;
int nplSize;
double* X;
double* Y;
int sampleSize;
bool isClassification = true;

enum TestCaseOption
{
    LINEAR_SIMPLE,
    LINEAR_MULTIPLE,
    XOR,
    CROSS,
    MULTI_CROSS,
    LINEAR_SIMPLE_2D
};

void generateParameter(TestCaseOption testCaseOption) {
    srand(time(NULL));

    switch (testCaseOption) {
    case LINEAR_SIMPLE:
        neuronsPerLayer = new int[] {
            2, 1
        };

        nplSize = 2;

        X = new double[] {
            1, 1,
            2, 3,
            3, 3
        };

        Y = new double[] {
            1,
                -1,
                -1
        };

        sampleSize = 3;
        break;
    case LINEAR_MULTIPLE:
        sampleSize = 100;

        neuronsPerLayer = new int[] {
            2, 1
        };

        nplSize = 2;

        X = new double[sampleSize * 2];
        Y = new double[sampleSize];

        for (int i = 0; i < sampleSize; ++i) {
            X[i * 2] = rand() / (double)RAND_MAX + (i < 50 ? 1 : 2);
            X[i * 2 + 1] = rand() / (double)RAND_MAX + (i < 50 ? 1 : 2);
            Y[i] = (i < 50 ? -1 : 1);
        }

        break;
    case XOR:
        neuronsPerLayer = new int[] {
            2, 3, 1
        };

        nplSize = 3;

        X = new double[] {
            0, 0,
                0, 1,
                1, 0,
                1, 1
        };

        Y = new double[] {
            -1,
                1,
                1,
                -1
        };

        sampleSize = 4;
        break;
    case CROSS:
        break;
    case MULTI_CROSS:
        break;
    case LINEAR_SIMPLE_2D:
        isClassification = false;
        neuronsPerLayer = new int[] {
            1, 1
        };

        nplSize = 2;

        X = new double[] {
            1, 2
        };

        Y = new double[] {
            2, 3
        };

        sampleSize = 2;
        break;
    }
}

int main() 
{
    generateParameter(LINEAR_SIMPLE_2D);

    double* result = train_mlp_model(neuronsPerLayer, nplSize, X, Y, sampleSize, 100000, 0.1, isClassification);

    cout << "Resultat obtenu" << endl;
    for (int i = 0; i < sampleSize; ++i) {
        cout << result[i] << endl;
    }
}
