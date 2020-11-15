#include "Utils.h"

vector<double> Utils::createVector(double* X, int indexStart, int indexEnd) {
    vector<double> nVector;

    for (int i = indexStart; i < indexEnd; ++i) {
        nVector.push_back(X[i]);
    }

    return nVector;
}

double Utils::sigmoid(double x) {
    return 1 / (1 + exp(-x));
}