#pragma once

#include <iostream>
#include <vector>

using namespace std;


static class Utils {
public:
	static vector<double> createVector(double* X, int indexStart, int indexEnd);
	static double sigmoid(double x);
	static float getDistance(vector<double> x1, vector<double> x2);
};