#include "map.h"

int* MapGenerator::generate_map(int size) {
	
	// Init randomness
	srand (time(NULL));

	int* map = (int*) malloc(size * size * sizeof(int*));
	
	int i, j;
	for(i = 0; i < size * size; i++) {
			map[i] = rand() % 5;
	}

	return map;
}