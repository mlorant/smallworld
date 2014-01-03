#include "map.h"
#include <iostream>
#include <cmath> /* round */



#define NB_PATTERNS 5
#define PATTERN_WIDTH 5
#define PATTERN_HEIGHT 5

/*
 * Define many patterns used to make medium and big random
 * map. Each patterns is composed of 0 and 1, where 1 designed
 * a tile which will be special (Forest, Mountain, ...) and 0
 * will be a Plain OR an element of the pattern overlapped.
 */
int patterns[NB_PATTERNS][PATTERN_HEIGHT][PATTERN_WIDTH] = {
	{
		{0, 0, 0, 0, 0},
		{0, 0, 1, 0, 0},
		{0, 1, 1, 1, 0},
		{0, 0, 1, 0, 0},
		{0, 0, 0, 0, 0}
	},
	{
		{0, 1, 0, 0, 0},
		{0, 1, 1, 0, 0},
		{0, 1, 1, 1, 0},
		{0, 1, 1, 0, 0},
		{0, 1, 0, 0, 0}
	},
	{
		{0, 0, 1, 0, 0},
		{0, 1, 1, 1, 0},
		{1, 1, 1, 1, 1},
		{0, 1, 1, 1, 0},
		{0, 0, 1, 0, 0}
	},	
	{
		{0, 0, 0, 0, 0},
		{1, 1, 0, 1, 1},
		{1, 1, 0, 1, 1},
		{1, 1, 0, 1, 1},
		{0, 0, 0, 0, 0}
	},
	{
		{0, 1, 1, 1, 0},
		{0, 1, 0, 1, 0},
		{0, 1, 0, 1, 0},
		{0, 1, 1, 1, 0},
		{0, 0, 0, 0, 0}
	}
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
MapGenerator::MapGenerator() {
	// Init random seed and map array
	srand (time(0));

	for(int i = 0; i < NB_TILE_TYPES; i++) {
		tilesUsed[i] = false;
	}
}


int rand_between(int min, int max) {
	return min + (rand() % (int)(max - min + 1));
}

int MapGenerator::getRandomSpecialTile() {

	int start = rand() % NB_TILE_TYPES;
	
	int i = 0;
	while(tilesUsed[start] == true && static_cast<CaseType>(start) != Plain) {
		start = (start + 1) % NB_TILE_TYPES;

		// If we have use every tile types, reset the bool array
		if(i++ == NB_TILE_TYPES) {
			for(int i = 0; i < NB_TILE_TYPES; i++) {
				tilesUsed[i] = false;
			}
		}
	}
	
	return start;
}

int* MapGenerator::generate_map(int size) {
	

	// Init the base of the map, full of Plain.
	this->_size = size;
	this->_map = (int*) malloc(size * size * sizeof(int*));
	this->buildBase();

	// Strategy (depend of the map size)
	if(size <= 7) {
		this->pureRandom();
	}
	else {
		this->randomWithPatterns();
	}
	return this->_map;
}


void MapGenerator::pureRandom() {
	
	for(int i = 0; i < _size * _size; i++) {

		int proba = rand() % 100;

		if(proba < 20) {				// 20% of chance of a Desert
			this->_map[i] = Desert;
		}
		else if(proba < 40) {			// 20% of chance of a Forest
			this->_map[i] = Forest;
		}
		else if(proba < 58) {			// 18% of chance of a Mountain
			this->_map[i] = Mountain;
		}
		else if(proba < 73) {			// 15% of chance of a Sea
			this->_map[i] = Sea;
		}
	}
}

void MapGenerator::randomWithPatterns() {

	int quarter = this->_size/4;
	int points[4];

	int mid = this->_size / 2;
	
	for(int i = 0; i < 45; i++) {
		int tileOrigin = rand() % (_size*_size);
		int xOrigin = tileOrigin % _size;
		int yOrigin = tileOrigin / _size;

		// Get random patterns
		int patNumber = rand() % NB_PATTERNS;
		int tileType = this->getRandomSpecialTile();

		for(int j = 0; j < PATTERN_HEIGHT; j++) {
			// If we hit the bottom on the map, we stop the pattern
			if(yOrigin + j >= _size)
				break;

			// Draw the line number j of the pattern
			for(int k = 0; k < PATTERN_WIDTH; k++) {

				// If we hit the right side of map, stop the line
				if(xOrigin + k >= _size) 
					break;

				if(patterns[patNumber][j][k] == 1)
					this->_map[tileOrigin + j*_size + k] = tileType;
				
			}
		}
	}

}


/*!
 * \brief Build border of the map
 *
 * Generate random seas around the map, limited to 20% of the width
 * maximum. There's more chance to get a sea if there's another sea 
 * nearby during the construction.
 *
 */
void MapGenerator::buildBase() {
	int i;
	for(i = 0; i < this->_size*this->_size; i++) {
		this->_map[i] = Plain;
	}
}



int** MapGenerator::getAccessMatrix() {
	int** matrix = (int**) malloc(_size * sizeof(int*));

	for(int i = 0; i < _size; i++) {
		matrix[i] = (int*) malloc(_size * sizeof(int));
		for(int j = 0; j < _size; j++) {
			if(this->_map[i*_size + j] == Sea)
				matrix[i][j] = 0;
			else
				matrix[i][j] = 1;
		}
	}

	return matrix;
}

/*
void MapGenerator::warshall() {
    
	int** matrix = this->getAccessMatrix();

	int** 

    int k,i,j;
    int hopdist;
 
    for( k = 0; k < _size; k ++ ) {
 
        for( i = 0; i < _size; i ++ ) {   // row index
 
            for( j = 0; j < _size; j ++ ) {   // column index
                 
                if(matrix[i][k] == 0 || matrix[k][j] == 0 )
                    hopdist = 0;
                else                    // can hop to k
                    hopdist = 1;
                 
                if( hopdist < gr[i][j]) { // if hopping to k is better
                    first_hop[i][j] = k;        // if you want to go from i to j then first go to k
                    gr[i][j] = hopdist;
                }
            }
        }
    }
}
*/

/*
	Check if the map generated can be traversed
*//*
bool MapGenerator::hasGoodPath() {

	int** matrix = this->getAccessMatrix();

	for(int i = 0; i < _size; i++)
        if(hasPathHelper(m, v, i, 0))
            return true;

    return false;


	return true;
}
*/
