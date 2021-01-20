#include "RBF.h"

RBF::RBF(vector<vector<double>> trainingInputs, int centroidsNb, int classesNb, int maxKMeans, vector<double> yArray, vector<vector<double>> pTsX, vector<double> pTsY) {
	numberOfCentroids = centroidsNb;
	numberOfClasses = classesNb;
	inputs = trainingInputs;
	maxIterationsInKMeans = maxKMeans;
	classes = yArray;
	KMeans kMeans();
	testInputs = pTsX;
	testClasses = pTsY;
}

void RBF::TrainRBF() {
	centroids = kMeans.runkMeans(inputs,numberOfCentroids,maxIterationsInKMeans);
	gamma = CalculateGamma();

	/*for (int i = 0; i < numberOfCentroids; ++i) {
		vector<float> currentMatrix;
		for (int j = 0; j < inputs.size(); ++j) {
			currentMatrix[j] = exp(gamma * -1 * pow(Utils::getDistance(inputs[j],centroids[i]),2)));
		}
	}*/
	vector<vector<double>> RBF_X = GetAsRbfList(inputs, centroids, gamma);

	vector<vector<double>> hot_tr_y = Utils::convert_to_one_hot(classes, numberOfClasses);

	vector<vector<double>> RBF_X_T = Utils::matT(RBF_X);

	weights	= Utils::matDot(Utils::matDot(Utils::invert(Utils::matDot(RBF_X_T, RBF_X)), RBF_X_T), hot_tr_y);

	accuracy = getAccuracy(testInputs, testClasses, weights, centroids, gamma);
}

double RBF::getAccuracy(vector<vector<double>> X, vector<double> y,	vector<vector<double>>w, vector<vector<double>>centroids, double gamma) {
	vector<vector<double>> ts_rbf_list = GetAsRbfList(X, centroids, gamma);
	vector<vector<double>> pred_test_y_one_hot = Utils::matDot(ts_rbf_list, w);
	vector<double> pred_test_y;

	for (vector<double> row : pred_test_y_one_hot)
		pred_test_y.push_back(Utils::argMax(row));

	double true_counter = 0;

	for (int i = 0; i < pred_test_y.size(); i++)
		if (pred_test_y[i] == y[i])
			true_counter++;

	return true_counter / pred_test_y.size();

}

double* RBF::getResult(vector<vector<double>> X, vector<double> y,	vector<vector<double>>w, vector<vector<double>>centroids, double gamma) {
	vector<vector<double>> ts_rbf_list = GetAsRbfList(X, centroids, gamma);
	vector<vector<double>> pred_test_y_one_hot = Utils::matDot(ts_rbf_list, w);
	vector<double> pred_test_y;
	int ySize = y.size();
	double* predResult = new double[ySize];
	int loop = 0;
	for (vector<double> row : pred_test_y_one_hot) {
		pred_test_y.push_back(Utils::argMax(row));
		predResult[loop] = pred_test_y[loop];
		++loop;
	}

	return predResult;
}

double RBF::GetRbf(vector<double> x, vector<double> c, double s) {
	double distance = Utils::getDistance(x, c);
	return 1 / exp(-distance / (s * s));
}

vector<vector<double>> RBF::GetAsRbfList(vector<vector<double>> X, vector<vector<double>> centroids, double std) {

	// get RBFs
	vector<vector<double>> rbf_list;
	for (vector<double> x : X) {
		vector<double> rbf_row;
		for (vector<double> c : centroids) {
			rbf_row.push_back(GetRbf(x, c, std));
		}
		rbf_list.push_back(rbf_row);
	}
	return rbf_list;
}

float RBF::CalculateGamma() {
	float maxDist = 0;
	for (int i = 0; i < numberOfCentroids; ++i) {
		for (int j = 0; j < numberOfCentroids; ++i) {
			float currentDist = Utils::getDistance(centroids[i], centroids[j]);
			if (currentDist > maxDist) {
				maxDist = currentDist;
			}
		}
	}

	return sqrt(2 * numberOfCentroids) / maxDist;
}