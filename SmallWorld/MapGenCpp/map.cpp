#include "map.h"

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
		map[i] = rand() % 5;
	}
	
	return map;
}