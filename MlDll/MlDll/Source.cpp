#include <stdio.h>      /* printf */
#include <math.h>       /* tanh, log */
#include <cstdlib>

#include "Utils.h"
#include "MLP.h"
#include "RBF.h"

extern "C" {

	/* ====================== LINEAR ====================== */
	__declspec(dllexport) double* create_linear_model(int inputs_count) {
		auto weights = new double[inputs_count + 1];
		srand(time(NULL));

		for (auto i = 0; i < inputs_count + 1; i++) {
			weights[i] = rand() / (double)RAND_MAX * 2.0 - 1.0;
		}

		return weights;
	}

	__declspec(dllexport) double predict_linear_model_classification(double* model, vector<double> inputs, int inputs_count) {
		double sum = model[0];

		for (int i = 0; i < inputs_count; ++i) {
            sum += inputs[i] * model[i + 1];
        }

		return Utils::sigmoid(sum) < 0.5 ? -1 : 1;
	}

    __declspec(dllexport) double predict_linear_model_regression(double* model, vector<double> inputs, int inputs_count) {
		double sum = model[0];

		for (int i = 0; i < inputs_count; ++i) {
			sum += inputs[i] * model[i + 1];
		}

		return Utils::sigmoid(sum) < 0.5 ? -1 : 1;
    }

	__declspec(dllexport) double* predict_linear_model_multiclass_classification(double* model, double inputs[], int inputs_count,
		int class_count) {
		// TODO
		return new double[3] {1.0, -1.0, 1.0};
	}

	__declspec(dllexport) void train_linear_model_rosenblatt(double* model, double all_inputs[], int inputs_count, int sample_count,
		double all_expected_outputs[], int expected_output_size, int epochs, double learning_rate, bool isClassification) {

		vector<double> x;
		double outputExpected;
		double predictResult;

		srand(time(NULL));

		for (int it = 0; it < epochs; ++it) {
			int k = rand() % sample_count;

			x.clear();
			x.resize(inputs_count);

			for (int i = 0; i < inputs_count; ++i) {
				x[i] = (all_inputs[inputs_count * k + i]);
			}

			outputExpected = all_expected_outputs[k];

			if (isClassification) {
				predictResult = predict_linear_model_classification(model, x, inputs_count);
			}
			else {
				predictResult = predict_linear_model_regression(model, x, inputs_count);
			}

			model[0] += learning_rate * (outputExpected - predictResult);
			for (int i = 1; i < inputs_count + 1; ++i) {
				model[i] += learning_rate * (outputExpected - predictResult) * x[i - 1];
			}
		}

		return;
	}

	__declspec(dllexport) void delete_linear_model(double* model) {
		delete[] model;
	}

	__declspec(dllexport) double* train_linear_model(double* inputs, int inputsCount, double* outputs, int outputsCount, int sampleSize, int epochs, double learningRate, bool isClassification) {
		double* model = create_linear_model(inputsCount);
		double* result = new double[outputsCount * sampleSize];

		train_linear_model_rosenblatt(model, inputs, inputsCount, sampleSize, outputs, outputsCount, epochs, learningRate, isClassification);

		cout << "AFTER TRAINING" << endl;
		for (int k = 0; k < outputsCount * sampleSize; ++k) {
			if (isClassification) {
				result[k] = predict_linear_model_classification(model, Utils::createVector(inputs, k * inputsCount, inputsCount * (k + 1)), inputsCount);
			}
			else {
				result[k] = predict_linear_model_regression(model, Utils::createVector(inputs, k * inputsCount, inputsCount * (k + 1)), inputsCount);
			}
		}

		delete_linear_model(model);
		return result;
	}

	/* ====================== MLP ====================== */
	__declspec(dllexport) double* train_mlp_model(int* neuronsPerLayer, int nplSize, double* X, double* Y, int sampleSize, int epochs, double learningRate, bool isClassification) {
		MLP* mlp = new MLP(neuronsPerLayer, nplSize);
		double* result = new double[sampleSize * neuronsPerLayer[nplSize - 1]];

		cout << "BEFORE TRAINING " << sampleSize * neuronsPerLayer[nplSize - 1] << endl;
		/*for (int k = 0; k < sampleSize; ++k) {
			mlp->forwardPass(Utils::createVector(X, k * 2, 2 * (k + 1)), isClassification);
			mlp->displayInput();
		}*/

		mlp->train(X, Y, sampleSize, isClassification, epochs, learningRate);

		cout << "AFTER TRAINING" << endl;
		for (int k = 0; k < sampleSize; ++k) {
			mlp->forwardPass(Utils::createVector(X, k * neuronsPerLayer[0], neuronsPerLayer[0] * (k + 1)), isClassification);
			mlp->fillInputsResult(result, k);
		}

		return result;
	}
	
	/* ====================== RBF ====================== */
	__declspec(dllexport) double* train_rbf_model(double* X, int nbImages, int XSeparation, double* Y, int centroidTaMere, int maxClasses, int maxKMeans, double* XpetiteNuanceSurLaVariable, double* YEtVoila) {
		cout << "Hey" << endl;
		vector<double> output;
		vector<vector<double>> entries;
		cout << "Hey" << endl;
		for (int i = 0; i < nbImages; ++i) {
			vector<double> pixels;
			for (int j = 0; j < XSeparation; ++j) {
				int index = (i * XSeparation) + j;

				pixels.push_back(X[index]);
			}

			entries.push_back(pixels);

			output.push_back(Y[i]);
		}

		vector<double> outputTest;
		vector<vector<double>> entriesTest;

		for (int i = 0; i < nbImages; ++i) {
			vector<double> pixels;
			for (int j = 0; j < XSeparation; ++j) {
				int index = (i * XSeparation) + j;

				pixels.push_back(XpetiteNuanceSurLaVariable[index]);
			}

			entriesTest.push_back(pixels);

			outputTest.push_back(YEtVoila[i]);
		}

		RBF* rbf = new RBF(entries, centroidTaMere, maxClasses, maxKMeans, output, entriesTest, outputTest);

		rbf->TrainRBF();

		double* result = rbf->getResult(entriesTest,outputTest,rbf->weights,rbf->centroids,rbf->gamma);
		
		return result;
	}
}