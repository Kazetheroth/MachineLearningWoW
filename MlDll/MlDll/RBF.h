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
	RBF(vector<vector<double>> trainingInputs, int centroidsNb, int classesNb, int maxKMeans, vector<double> yArray, vector<vector<double>> pTsX, vector<double> pTsY);
	void TrainRBF();
	float CalculateGamma();
	void RunRBF();
	double GetRbf(vector<double> x, vector<double> c, double s);
	vector<vector<double>> GetAsRbfList(vector<vector<double>> X, vector<vector<double>> centroids, double std);
	double getAccuracy(vector<vector<double>> X, vector<double> y, vector<vector<double>>w, vector<vector<double>>centroids, double gamma);
	double* getResult(vector<vector<double>> X, vector<double> y, vector<vector<double>>w, vector<vector<double>>centroids, double gamma);
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

