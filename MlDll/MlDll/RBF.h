#pragma once

#include <iostream>
#include <vector>
#include <cstdlib>
#include <math.h>
#include <time.h>

using namespace std;

class RBF
{
public:
	

private:
	int* neuronsPerLayer;
	int nbLayers;

	int nbClasses;
	int k;
	bool stdFromCluster;

	vector<vector<vector<double>>> weights;
	vector<vector<double>> inputs;
	vector<vector<double>> deltas;
};

