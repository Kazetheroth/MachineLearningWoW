#include "MLP.h"

MLP::MLP(int* _neuronsPerLayer, int _neuronsPerLayerSize) {
	neuronsPerLayer = _neuronsPerLayer;
	nbLayers = _neuronsPerLayerSize - 1;

	vector<vector<double>> subLayer;
	vector<double> layerWeight;

	srand(time(NULL));

	for (int i = 1; i < _neuronsPerLayerSize; ++i) {
		subLayer.clear();

		for (int j = 0; j < neuronsPerLayer[i - 1] + 1; ++j) {
			layerWeight.clear();

			for (int k = 0; k < neuronsPerLayer[i] + 1; ++k) {
				layerWeight.push_back(rand() / (double)RAND_MAX * 2.0 - 1.0);
			}

			subLayer.push_back(layerWeight);
		}

		weights.push_back(subLayer);
	}

	vector<double> layer;
	for (int i = 0; i < _neuronsPerLayerSize; ++i) {
		layer.clear();
		
		for (int j = 0; j < neuronsPerLayer[i] + 1; ++j) {
			if (j == 0) {
				layer.push_back(1.0);
			}
			else {
				layer.push_back(0.0);
			}
		}

		deltas.push_back(layer);
		inputs.push_back(layer);
	}
}

int MLP::getNbWeights() {
	int nbWeight = 0;

	for (int i = 1; i < nbLayers; ++i) {
		nbWeight += (neuronsPerLayer[i - 1] + 1) * (neuronsPerLayer[i] + 1);
	}

	return nbWeight;
}

int MLP::getNbInputs() {
	int nbInputs = 0;

	for (int i = 1; i < nbLayers; ++i) {
		nbInputs += neuronsPerLayer[i] + 1;
	}

	return nbInputs;
}

void MLP::forwardPass(vector<double> _inputs, bool isClassification) {
	for (int j = 0; j < neuronsPerLayer[0]; ++j) {
		inputs[0][j + 1] = _inputs[j];
	}

	for (int l = 1; l < nbLayers + 1; ++l) {
		for (int j = 1; j < neuronsPerLayer[l] + 1; ++j) {
			double sum = 0.0;

			for (int i = 0; i < neuronsPerLayer[l - 1] + 1; ++i) {
				sum += inputs[l - 1][i] * weights[l - 1][i][j];
			}

			if (l == nbLayers && !isClassification) {
				inputs[l][j] = sum;
			}
			else {
				inputs[l][j] = tanh(sum);
			}
		}
	}
}

void MLP::train(double allInputs[], double allExpectedOutputs[], int sampleCount, bool isClassification, int epochs, double learningRate) {
	int inputsSize = neuronsPerLayer[0];
	int outputsSize = neuronsPerLayer[nbLayers];

	vector<double> x_k;
	vector<double> y_k;

	srand(time(NULL));

	for (int it = 0; it < epochs; ++it) {
		int k = rand() % sampleCount;

		x_k.clear();
		y_k.clear();
		x_k.resize(inputsSize);
		y_k.resize(outputsSize);

		for (int i = 0; i < inputsSize; ++i) {
			x_k[i] = (allInputs[inputsSize * k + i]);
		}

		for (int i = 0; i < outputsSize; ++i) {
			y_k[i] = (allExpectedOutputs[outputsSize * k + i]);
		}

		forwardPass(x_k, isClassification);

		for (int j = 1; j < outputsSize + 1; ++j) {
			deltas[nbLayers][j] = inputs[nbLayers][j] - y_k[j - 1];

			if (isClassification) {
				deltas[nbLayers][j] *= 1 - pow(inputs[nbLayers][j], 2);
			}
		}

		for (int l = nbLayers; l >= 2; --l) {
			for (int i = 0; i < neuronsPerLayer[l - 1] + 1; ++i) {
				double sum = 0.0;

				for (int j = 1; j < neuronsPerLayer[l] + 1; ++j) {
					sum += weights[l - 1][i][j] * deltas[l][j];
				}

				deltas[l - 1][i] = (1 - pow(inputs[l - 1][i], 2)) * sum;
			}
		}

		for (int l = 1; l < nbLayers + 1; ++l) {
			for (int i = 0; i < neuronsPerLayer[l - 1] + 1; ++i) {
				for (int j = 1; j < neuronsPerLayer[l] + 1; ++j) {
					weights[l - 1][i][j] -= learningRate * inputs[l - 1][i] * deltas[l][j];
				}
			}
		}
	}
}

void MLP::displayInput() {
	int inputSize = inputs[nbLayers].size();
	for (int i = 1; i < inputSize; ++i) {
		cout << inputs[nbLayers][i] << endl;
	}
}

void MLP::fillInputsResult(double* outputResult, int index) {
	int inputSize = inputs[nbLayers].size();

	for (int i = 1; i < inputSize; ++i) {
		outputResult[(inputSize - 1) * index + (i - 1)] = inputs[nbLayers][i];
	}
}

void MLP::displayWeights() {
	int iSize = weights.size();

	for (int i = 0; i < iSize; ++i) {
		cout << "For i == " << i << endl;
		int jSize = weights[i].size();

		for (int j = 0; j < jSize; ++j) {
			cout << "\tFor j == " << j << endl;
			int kSize = weights[i][j].size();
			
			for (int k = 0; k < kSize; ++k) {
				cout << "\t\tFor k == " << k << " value = " << weights[i][j][k] << endl;
			}
		}
	}
}

void MLP::testChangeInput() {
	inputs[nbLayers][1] = 259;
}