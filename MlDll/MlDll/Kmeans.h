#pragma once

#include <iostream>
#include <vector>
#include <math.h>
#include <time.h>

using namespace std;

class Kmeans
{
public:
	float getDistance(vector<double> x1, vector<double> x2);
	vector<double> getAveragePointInCluster(vector<vector<double>> cluster);
	vector<vector<vector<double>>> kMeans(vector<vector<double>> X, int k, int maxIterations);
};

