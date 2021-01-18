#include "RBF.h"

RBF::RBF(vector<vector<double>> trainingInputs, int centroidsNb, int classesNb, int maxKMeans) {
	numberOfCentroids = centroidsNb;
	numberOfClasses = classesNb;
	inputs = trainingInputs;
	maxIterationsInKMeans = maxKMeans;
	KMeans kMeans();
}

void RBF::TrainRBF() {
	centroids = kMeans.runkMeans(inputs,numberOfCentroids,maxIterationsInKMeans);
	gamma = CalculateGamma();

	for (int i = 0; i < numberOfCentroids; ++i) {
		vector<float> currentMatrix;
		for (int j = 0; j < inputs.size(); ++j) {
			currentMatrix[j] = exp(gamma * -1 * pow(Utils::getDistance(inputs[j],centroids[i]),2)));
		}
	}
}

float RBF::CalculateGamma() {
	float maxDist = 0;
	for (int i = 0; i < numberOfCentroids; ++i) {
		for (int j = 0; j < numberOfCentroids; ++i) {
			float currentDist = Utils::getDistance(centroids[i], centroids[j]);
			if (currentDist > maxDist) {
				maxDist = currentDist;
			}
		}
	}

	return sqrt(2 * numberOfCentroids) / maxDist;
}