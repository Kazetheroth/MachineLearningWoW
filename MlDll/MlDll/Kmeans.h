#pragma once

#include <iostream>
#include <vector>
#include <math.h>
#include <time.h>
#include "Utils.h"

using namespace std;

class KMeans
{
public:
	KMeans();
	vector<double> getAveragePointInCluster(vector<vector<double>> cluster);
	vector<vector<double>> runkMeans(vector<vector<double>> X, int k, int maxIterations);
};

