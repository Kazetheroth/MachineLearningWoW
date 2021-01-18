#include "Kmeans.h"

float Kmeans::getDistance(vector<double> x1, vector<double> x2) {
	if (x1.size() != x2.size()) {
		return NULL;
	}
	float sum = 0;
	for (int i = 0; i < x1.size(); ++i) {
		sum += pow(x1[i] - x2[i], 2);
	}
	return sqrt(sum);
}

vector<double> Kmeans::getAveragePointInCluster(vector<vector<double>> cluster) {
	vector<double> newAveragePoint;

	for (int i = 0; i < cluster[0].size(); ++i) {
		int sum = 0;
		for (int j = 0; j < cluster.size(); ++j) {
			sum += cluster[j][i];
		}
		newAveragePoint.push_back(sum / cluster.size());
	}
	return newAveragePoint;
}

vector<vector<vector<double>>> Kmeans::kMeans(vector<vector<double>> X, int k, int maxIterations) {
	vector<vector<double>> centroids;
	vector<vector<double>> oldCentroids;
	vector<vector<vector<double>>> clusters;
	int iterations = 0;

	srand(time(NULL));

	for (int i = 0; i < k; ++i) {
		int randomIndex = rand() % X.size();
		centroids.push_back(X[randomIndex]);
		vector<vector<double>> row;
		clusters.push_back(row);
		clusters[i].push_back(X[randomIndex]);
		X.erase(X.begin() + randomIndex);
	}
	
	while (iterations < maxIterations) {
		for (int i = 0; i < X.size(); ++i) {
			float minDist = 1000;
			int currentIndex = -1;
			for (int j = 0; j < k; ++j) {
				float currentDist = getDistance(X[i], centroids[j]);
				if (currentDist < minDist) {
					minDist = currentDist;
					currentIndex = j;
				}
			}
			if (currentIndex != -1) {
				clusters[currentIndex].push_back(X[i]);
			}
		}

		bool shouldBreak = false;
		for (int i = 0; i < k; ++i) {
			copy(centroids.begin(), centroids.end(), back_inserter(oldCentroids));
			centroids[i] = getAveragePointInCluster(clusters[i]);
			/*if (std::equal(centroids.begin(), centroids().end(), oldCentroids.begin())) {
				shouldBreak = true;
			}*/
		}
		if (shouldBreak) {
			break;
		}
		++iterations;
	}
	return clusters;
}