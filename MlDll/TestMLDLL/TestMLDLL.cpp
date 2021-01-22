// TestMLDLL.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <time.h>

using namespace std;

extern "C"
{
    __declspec(dllimport) double* train_linear_model(double* inputs, int inputsCount, double* outputs, int outputsCount, int sampleSize, int epochs, double learningRate);
    __declspec(dllimport) double* train_mlp_model(int* neuronsPerLayer, int nplSize, double* X, double* Y, int sampleSize, int epochs, double learningRate, bool isClassification);
    __declspec(dllimport) double* train_rbf_model(double* X, int nbImages, int XSeparation, double* Y, int centroidTaMere, int maxClasses, int maxKMeans, double* XpetiteNuanceSurLaVariable, double* YEtVoila);
}

int* neuronsPerLayer;
int nplSize;
double* X;
double* Y;
double* Xtest;
double* Ytest;
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
    LINEAR_SIMPLE_2D,
    LINEAR_TRICKY_3D
};

void generateParameter(TestCaseOption testCaseOption) {
    srand(time(NULL));

    switch (testCaseOption) {
    case LINEAR_SIMPLE:
        neuronsPerLayer = new int[2] {
            2, 1
        };

        nplSize = 2;
        inputsCount = 2;
        outputsCount = 1;

        X = new double[6] {
            1, 1,
            2, 3,
            3, 3
        };

        Y = new double[3] {
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

        neuronsPerLayer = new int[2] {
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
        neuronsPerLayer = new int[3] {
            2, 3, 1
        };

        nplSize = 3;
        inputsCount = 2;
        outputsCount = 1;

        X = new double[8] {
            0, 0,
                0, 1,
                1, 0,
                1, 1
        };

        Y = new double[4] {
            1,
                2,
                2,
                1
        };
		
		Xtest = new double[8] {
			0, 2,
				2, 2,
				1, 0,
				0, 0
        };

        Ytest = new double[4] {
            1,
                2,
                2,
                1
        };

        sampleSize = 4;
        break;
    case CROSS:
        break;
    case MULTI_CROSS:
        neuronsPerLayer = new int[2] { 2, 3 };
        nplSize = 2;
        sampleSize = 500;
        inputsCount = 2;
        outputsCount = 3;

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
        inputsCount = 1;
        outputsCount = 1;
        isClassification = false;
        neuronsPerLayer = new int[2] {
            1, 1
        };

        nplSize = 2;

        X = new double[2] {
            1, 2
        };

        Y = new double[2] {
            2, 3
        };

        sampleSize = 2;
        break;
    case LINEAR_TRICKY_3D:
        inputsCount = 2;
        outputsCount = 1;
        isClassification = false;
        neuronsPerLayer = new int[2] {
            2, 1
        };

        nplSize = 2;

        X = new double[6] {
            1, 1, 2, 2, 3, 3
        };

        Y = new double[3] {
            1, 2, 3
        };
		
		Xtest = new double[6] {
            0,0,2,2,4,4
        };

        Ytest = new double[3] {
            2, 2, 2
        };

        sampleSize = 3;
        break;
    }
}

int main() 
{
    generateParameter(XOR);

    bool useLinearModel = false;
    bool useRBF = true;
    double* result;

    int epochs = 1000;
    double learningRate = 0.1;

    if (useLinearModel) {
        result = train_linear_model(X, inputsCount, Y, outputsCount, sampleSize, epochs, learningRate);
	}
	else if (useRBF) {
		result = train_rbf_model(X,4,2,Y,4,3,1000, Xtest,Ytest);
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
