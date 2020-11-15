// TestMLDLL.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <time.h>

using namespace std;

extern "C"
{
    __declspec(dllimport) double* train_linear_model(double* inputs, int inputsCount, double* outputs, int outputsCount, int sampleSize, int epochs, double learningRate);
    __declspec(dllimport) double* train_mlp_model(int* neuronsPerLayer, int nplSize, double* X, double* Y, int sampleSize, int epochs, double learningRate, bool isClassification);
}

int* neuronsPerLayer;
int nplSize;
double* X;
double* Y;
int sampleSize;
bool isClassification = true;
int inputsCount;
int outputsCount;

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
        inputsCount = 2;
        outputsCount = 1;

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
        inputsCount = 2;
        outputsCount = 1;

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
        inputsCount = 2;
        outputsCount = 1;

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
        neuronsPerLayer = new int[] { 2, 3 };
        nplSize = 2;
        sampleSize = 500;

        X = new double[sampleSize * 2];
        Y = new double[sampleSize * 3];

        for (int i = 0; i < 500; ++i)
        {
            X[i * 2] = rand() / (double)RAND_MAX * 2 - 1;
            X[i * 2 + 1] = rand() / (double)RAND_MAX * 2 - 1;

            int xIndex = i * 2;
            int yIndex = i * 2 + 1;

            if (-X[xIndex] - X[yIndex] - 0.5 > 0 && X[yIndex] < 0 &&
                X[xIndex] - X[yIndex] - 0.5 < 0)
            {
                Y[i * 3] = 1;
                Y[i * 3 + 1] = 0;
                Y[i * 3 + 2] = 0;
            }
            else if (-X[xIndex] - X[yIndex] - 0.5 < 0 && X[yIndex] > 0 &&
                X[xIndex] - X[yIndex] - 0.5 < 0)
            {
                Y[i * 3] = 0;
                Y[i * 3 + 1] = 1;
                Y[i * 3 + 2] = 0;
            }
            else if (-X[xIndex] - X[yIndex] - 0.5 < 0 && X[yIndex] < 0 &&
                X[xIndex] - X[yIndex] - 0.5 > 0)
            {
                Y[i * 3] = 0;
                Y[i * 3 + 1] = 0;
                Y[i * 3 + 2] = 1;
            }
            else
            {
                Y[i * 3] = 0;
                Y[i * 3 + 1] = 0;
                Y[i * 3 + 2] = 0;
            }
        }

        isClassification = true;
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
    generateParameter(LINEAR_MULTIPLE);

    bool useLinearModel = true;
    double* result;

    int epochs = 1000;
    double learningRate = 0.1;

    if (useLinearModel) {
        result = train_linear_model(X, inputsCount, Y, outputsCount, sampleSize, epochs, learningRate);
    }
    else {
        result = train_mlp_model(neuronsPerLayer, nplSize, X, Y, sampleSize, epochs, learningRate, isClassification);
    }

    int nbResult = sampleSize * neuronsPerLayer[nplSize - 1];
    cout << "Nombre de resultat : " << nbResult << " Resultat obtenu" << endl;

    for (int i = 0; i < nbResult; ++i) {
        cout << result[i] << endl;
    }
}
