#pragma once

#include <iostream>
#include <vector>
#include <cstdlib>
#include <math.h>
#include <time.h>

using namespace std;

class MLP {
public:
	MLP(int* _neuronsPerLayer, int _neuronsPerLayerSize);
	int getNbWeights();
	int getNbInputs();

	void train(double allInputs[], double allExpectedOutputs[], int sampleCount, bool isClassification, int epochs, double learningRate);
	void forwardPass(vector<double> _inputs, bool isClassification);

	double getAndDisplayInput();
	void displayWeights();

	void testChangeInput();

private:
	int* neuronsPerLayer;
	int nbLayers;

	vector<vector<vector<double>>> weights;
	vector<vector<double>> inputs;
	vector<vector<double>> deltas;
};