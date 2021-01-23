#include "RBF.h"

RBF::RBF() {

}
RBF::RBF(vector<vector<double>> trainingInputs, int centroidsNb, int classesNb, int maxKMeans, vector<double> yArray, vector<vector<double>> testX, vector<double> testY, float pGamma) {
	numberOfCentroids = centroidsNb;
	numberOfClasses = classesNb;
	inputs = trainingInputs;
	maxIterationsInKMeans = maxKMeans;
	outputs = yArray;
	KMeans kMeans();
	testInputs = testX;
	testOutputs = testY;
	gamma = pGamma;
}

RBF::RBF(vector<vector<double>> trainingInputs, vector<double> yArray, vector<vector<double>> pCentroids, float pGamma, int classesNb) {
	centroids = pCentroids;
	numberOfClasses = classesNb;
	inputs = trainingInputs;
	outputs = yArray;
	KMeans kMeans();
	gamma = pGamma;
}
RBF::RBF(vector<vector<double>> trainingInputs, int centroidsNb, int maxKMeans) {
	numberOfCentroids = centroidsNb;
	inputs = trainingInputs;
	maxIterationsInKMeans = maxKMeans;
	KMeans kMeans();
}

void RBF::TrainRBFWhole() {
	cout << "In train" << endl;
	centroids = kMeans.runkMeans(inputs,numberOfCentroids,maxIterationsInKMeans);
	cout << "In train" << endl;
	gamma = 0.000001f;
	cout << "In train" << endl;

	/*for (int i = 0; i < numberOfCentroids; ++i) {
		vector<float> currentMatrix;
		for (int j = 0; j < inputs.size(); ++j) {
			currentMatrix[j] = exp(gamma * -1 * pow(Utils::getDistance(inputs[j],centroids[i]),2)));
		}
	}*/
	vector<vector<double>> RBF_X = GetAsRbfList(inputs, centroids, gamma);

	cout << "In train" << endl;
	vector<vector<double>> hot_tr_y = Utils::convert_to_one_hot(outputs, numberOfClasses);
	cout << "In train" << endl;
	vector<vector<double>> RBF_X_T = Utils::matT(RBF_X);
	cout << "In train" << endl;
	weights	= Utils::matDot(Utils::matDot(Utils::invert(Utils::matDot(RBF_X_T, RBF_X)), RBF_X_T), hot_tr_y);
	cout << "In train " << testInputs.size() << " " << testOutputs.size() << endl;
	cout << "In train " << testInputs[0][0] << " " << testOutputs[0] << endl;
	accuracy = getAccuracy(testInputs, testOutputs, weights, centroids, gamma);
	cout << "accuracy : " << accuracy << endl;
}

double* RBF::trainWeights(){
	vector<vector<double>> RBF_X = GetAsRbfList(inputs, centroids, gamma);
	vector<vector<double>> hot_tr_y = Utils::convert_to_one_hot(outputs, numberOfClasses);
	vector<vector<double>> RBF_X_T = Utils::matT(RBF_X);

	weights = Utils::matDot(Utils::matDot(Utils::invert(Utils::matDot(RBF_X_T, RBF_X)), RBF_X_T), hot_tr_y);
	
	double* result = new double[weights.size()*weights[0].size()];
	for (int i = 0; i < weights.size();++i) {
		int maxJ = weights[i].size();
		for (int j = 0; j < maxJ;++j) {
			result[i * maxJ + j] = weights[i][j];
		}
	}

	return result;
}

double* RBF::getCentroids(){
	centroids = kMeans.runkMeans(inputs, numberOfCentroids, maxIterationsInKMeans);
	double* result = new double[centroids.size() * centroids[0].size()];
	for (int i = 0; i < centroids.size(); ++i) {
		int maxJ = centroids[i].size();
		for (int j = 0; j < maxJ; ++j) {
			result[i * maxJ + j] = centroids[i][j];
		}
	}
	return result;
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

double* RBF::getResult(vector<vector<double>> X, int outputSize, vector<vector<double>>w, vector<vector<double>>centroids, double gamma) {
	vector<vector<double>> ts_rbf_list = GetAsRbfList(X, centroids, gamma);
	vector<vector<double>> pred_test_y_one_hot = Utils::matDot(ts_rbf_list, w);
	vector<double> pred_test_y;
	double* predResult = new double[outputSize];
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
	return exp(s * (distance * distance));
}

vector<vector<double>> RBF::GetAsRbfList(vector<vector<double>> X, vector<vector<double>> centroids, double std) {
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
	cout << "calc gamma" << endl;
	float maxDist = 0;
	for (int i = 0; i < numberOfCentroids; ++i) {
		for (int j = 0; j < numberOfCentroids; ++j) {
			float currentDist = Utils::getDistance(centroids[i], centroids[j]);
			if (currentDist > maxDist) {
				maxDist = currentDist;
			}
		}
	}
	cout << "calc gamma" << endl;

	return sqrt(2 * numberOfCentroids) / maxDist;
}