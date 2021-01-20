#pragma once

#include <iostream>
#include <vector>

using namespace std;


static class Utils {
public:
	static vector<double> createVector(double* X, int indexStart, int indexEnd);
	static double sigmoid(double x);
	static float getDistance(vector<double> x1, vector<double> x2);
	static vector<vector<double>> convert_to_one_hot(vector<double> vec, int numberOfClasses);
	static vector<vector<double>> matT(vector<vector<double>> a);
	static vector<vector<double>> matDot(vector<vector<double>> a, vector<vector<double>> b);
	static vector<vector<double>> invert(vector<vector<double>> a);
	static double argMax(vector<double> a);
};