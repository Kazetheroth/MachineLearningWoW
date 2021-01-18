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
	RBF(vector<vector<double>> trainingInputs, int centroidsNb, int classesNb, int maxKMeans);
	void TrainRBF();
	float CalculateGamma();
	void runRBF();

private:
	vector<vector<double>> inputs;
	int numberOfCentroids;
	vector<vector<double>> centroids;
	vector<vector<double>> weights;
	int numberOfClasses;
	int maxIterationsInKMeans;
	KMeans kMeans;
	float gamma;
};

