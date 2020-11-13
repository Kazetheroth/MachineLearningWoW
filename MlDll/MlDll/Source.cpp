#include <stdio.h>      /* printf */
#include <math.h>       /* tanh, log */
#include <cstdlib>

#include "Utils.h"
#include "MLP.h"

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

	__declspec(dllexport) double predict_linear_model_classification(double* model, double inputs[], int inputs_count) {
		double sum = model[0];
        for (int i = 0; i < inputs_count; ++i) {
            sum += inputs[i] * model[i + 1];
        }

		return sum < 0.5 ? -1 : 1;
	}

    __declspec(dllexport) double predict_linear_model_regression(double* model, double inputs[], int inputs_count) {
		double sum = model[0];
		for (int i = 0; i < inputs_count; ++i) {
			sum += inputs[i] * model[i + 1];
		}

		return sum < 0.5 ? -1 : 1;
    }

	__declspec(dllexport) double* predict_linear_model_multiclass_classification(double* model, double inputs[], int inputs_count,
		int class_count) {
		// TODO
		return new double[3] {1.0, -1.0, 1.0};
	}

	__declspec(dllexport) void train_linear_model_rosenblatt(double* model, double all_inputs[], int inputs_count, int sample_count,
		double all_expected_outputs[], int expected_output_size, int epochs, double learning_rate) {
		// TODO
		return;
	}

	__declspec(dllexport) void delete_linear_model(double* model) {
		delete[] model;
	}

	__declspec(dllexport) double my_add(double a, double b) {
		return a + b + 2;
	}

	/* ====================== MLP ====================== */
	__declspec(dllexport) double* train_mlp_model(int* neuronsPerLayer, int nplSize, double* X, double* Y, int sampleSize, int epochs, double learningRate, bool isClassification) {
		MLP* mlp = new MLP(neuronsPerLayer, nplSize);
		double* result = new double[sampleSize * neuronsPerLayer[nplSize - 1]];

		cout << "BEFORE TRAINING " << sampleSize * neuronsPerLayer[nplSize - 1] << endl;
		for (int k = 0; k < sampleSize; ++k) {
			mlp->forwardPass(Utils::createVector(X, k * 2, 2 * (k + 1)), isClassification);
			mlp->displayInput();
		}

		mlp->train(X, Y, sampleSize, isClassification, epochs, learningRate);

		cout << "AFTER TRAINING" << endl;
		for (int k = 0; k < sampleSize; ++k) {
			mlp->forwardPass(Utils::createVector(X, k * 2, 2 * (k + 1)), isClassification);
			mlp->fillInputsResult(result, k);
		}

		return result;
	}
}