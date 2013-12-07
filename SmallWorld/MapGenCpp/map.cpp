#include "map.h"
#include <iostream>

enum CaseType
{
	Desert = 0,
	Forest,
	Mountain,
	Plain,
	Sea
};

/*!
 * \brief Generate a map with random cases
 *
 * Generate an array of numbers, which represents squares types.
 * Representation of numbers are given by the `CaseType` enum.
 *
 * \param size : number of squares of the map (side length)
 * \return array of number between 0 and 4, with a length of size^2
 */
int* MapGenerator::generate_map(int size) {
	
	// Init random seed and map array
	srand (time(NULL));
	int* map = (int*) malloc(size * size * sizeof(int*));
	
	int i;
	// TODO: Implement map generation algorithm. Idea : Perlin Noise
	for(i = 0; i < size * size; i++) {
		int proba = rand() % 100; // Between 0 and 99

		if(proba < 15) {				// 15% of chance of a Desert
			map[i] = Desert;
		}
		else if(proba < 30) {			// 15% of chance of a Forest
			map[i] = Forest;
		}
		else if(proba < 40) {			// 10% of chance of a Mountain
			map[i] = Mountain;
		}
		else if(proba < 50) {			// 10% of chance of a Sea
			map[i] = Sea;
		}
		else {							// 50% of chance of a Plain
			map[i] = Plain;
		}
	}
	
	return map;
}

