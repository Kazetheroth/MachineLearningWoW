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

float Utils::getDistance(vector<double> x1, vector<double> x2) {
	if (x1.size() != x2.size()) {
		return NULL;
	}
	float sum = 0;
	for (int i = 0; i < x1.size(); ++i) {
		sum += pow(x1[i] - x2[i], 2);
	}
	return sqrt(sum);
}