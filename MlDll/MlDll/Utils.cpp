#include "Utils.h"

vector<double> Utils::createVector(double* X, int indexStart, int indexEnd) {
    vector<double> nVector;

    for (int i = indexStart; i < indexEnd; ++i) {
        nVector.push_back(X[i]);
    }

    return nVector;
}

double Utils::sigmoid(double x) {
    return 1 / (1 + exp(-x));
}

float Utils::getDistance(vector<double> x1, vector<double> x2) {
	if (x1.size() != x2.size()) {
		return NULL;
	}
	float sum = 0;
	for (int i = 0; i < x1.size(); ++i) {
		sum += pow(x1[i] - x2[i], 2);
	}
	return sqrt(sum);
}

vector<vector<double>> Utils::convert_to_one_hot(vector<double> vec, int numberOfClasses) {
	vector<vector<double>> A;

	for (int i = 0; i < vec.size(); i++)
	{
		vector<double> row(numberOfClasses, 0.0);
		row[(int)vec[i]] = 1.0;
		A.push_back(row);
	}
	return A;
}

vector<vector<double>> Utils::matT(vector<vector<double>> a) {
	vector<vector<double>> aT;

	for (int i = 0; i < a[0].size(); i++)
	{
		vector<double> row;
		for (int j = 0; j < a.size(); j++)
		{
			row.push_back(a[j][i]);
		}
		aT.push_back(row);
	}

	return aT;
}

vector<vector<double>> Utils::matDot(vector<vector<double>> a, vector<vector<double>> b) {
	vector<vector<double>> dMat;
	vector<vector<double>> bT = Utils::matT(b);
	for (int i = 0; i < a.size(); i++)
	{
		vector<double> n_row;
		vector<double> row = a[i];
		for (int j = 0; j < bT.size(); j++)
		{
			vector<double> col = bT[j];

			double sum = 0;
			for (int k = 0; k < row.size(); k++)
				sum += (row[k] * col[k]);

			n_row.push_back(sum);
		}
		dMat.push_back(n_row);
	}
	return dMat;
}

vector<vector<double>> Utils::invert(vector<vector<double>> a) {
	int n = a.size(), i = 0, j = 0, k = 0;

	vector<vector<double>> b;
	vector<double> p(n, 0.0);

	for (i = 0; i < n; i++) {
		vector<double> row(n, 0.0);
		b.push_back(row);
	}


	for (i = 0; i < n; i++)
	{
		for (j = 0; j < n; j++)
		{
			if (i == j)
			{
				b[i][j] = 1;
			}
			else
			{
				b[i][j] = 0;
			}
		}
	}

	for (i = 0; i < n; i++)
	{
		p[i] = a[i][i];
		for (j = 0; j < n; j++)
		{
			b[i][j] = b[i][j] / p[i];
			a[i][j] = a[i][j] / p[i];
		}
		for (j = 0; j < n; j++)
		{
			for (k = 0; k < n; k++)
			{
				if (j != i)
				{
					p[j] = a[j][i];
					b[j][k] -= b[i][k] * p[j];
				}
			}
		}
		for (j = 0; j < n; j++)
		{
			for (k = 0; k < n; k++)
			{
				if (j != i)
				{
					a[j][k] -= a[i][k] * p[j];
				}
			}
		}
	}
	return b;
}

double Utils::argMax(vector<double> a) {
	double maxE = -999, maxIdx = 0;
	for (int i = 0; i < a.size(); i++) {
		if (a[i] > maxE) {
			maxE = a[i];
			maxIdx = i;
		}
	}
	return maxIdx;
}