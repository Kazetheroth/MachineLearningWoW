#pragma once

#include <iostream>
#include <vector>
#include <cstdlib>
#include <math.h>
#include <time.h>

#include "KMeans.h"

using namespace std;

class RBF
{
public:
	RBF();
	RBF(vector<vector<double>> trainingInputs, int centroidsNb, int classesNb, int maxKMeans, vector<double> yArray, vector<vector<double>> testX, vector<double> testY, float pGamma);
	RBF(vector<vector<double>> trainingInputs, vector<double> yArray, vector<vector<double>> centroids, float pGamma, int classesNb);
	RBF(vector<vector<double>> trainingInputs, int centroidsNb, int maxKMeans);
	void TrainRBFWhole();
	float CalculateGamma();
	void RunRBF();
	double GetRbf(vector<double> x, vector<double> c, double s);
	vector<vector<double>> GetAsRbfList(vector<vector<double>> X, vector<vector<double>> centroids, double std);
	double getAccuracy(vector<vector<double>> X, vector<double> y, vector<vector<double>>w, vector<vector<double>>centroids, double gamma);
	double* getResult(vector<vector<double>> X, int outputSize, vector<vector<double>>w, vector<vector<double>>centroids, double gamma);
	double* getCentroids();
	double* trainWeights();
	vector<vector<double>> centroids;
	vector<vector<double>> weights;
	float gamma;

private:
	vector<vector<double>> inputs;
	vector<double> outputs;
	int numberOfCentroids;
	int numberOfClasses;
	int maxIterationsInKMeans;
	KMeans kMeans;
	double accuracy;
	vector<vector<double>> testInputs;
	vector<double> testOutputs;
};

