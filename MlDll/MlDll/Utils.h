#pragma once

#include <iostream>
#include <vector>

using namespace std;


static class Utils {
public:
	static vector<double> createVector(double* X, int indexStart, int indexEnd);
};