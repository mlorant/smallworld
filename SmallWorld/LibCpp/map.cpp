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
	if(size <= 7)
		this->pureRandom();
	else
		this->randomWithPatterns();
	

	this->removeWaterFromSides();

	return this->_map;
}


void MapGenerator::pureRandom() {
	
	int maxNumberTiles = (_size*_size) / NB_TILE_TYPES;

	// Create the same number of tile type
	for(int i = 0; i < NB_TILE_TYPES; i++) 
		for(int j = 0; j < maxNumberTiles; j++)
			this->_map[i*maxNumberTiles + j] = i;

	std::random_shuffle(&this->_map[0], &this->_map[(_size*_size)-1]);
}

void MapGenerator::randomWithPatterns() {

	int uses[NB_TILE_TYPES] = {0};
	uses[Plain] = _size*_size;
	int maxNumberTiles = ((_size*_size) / NB_TILE_TYPES) * 1.2;


	int nbIterations = (_size*_size)*0.33;
	for(int i = 0; i < nbIterations; i++) {
		int tileOrigin = rand() % (_size*_size);
		int xOrigin = tileOrigin % _size;
		int yOrigin = tileOrigin / _size;

		// Get random patterns
		int patNumber = rand() % NB_PATTERNS;

		int tileType = this->getRandomSpecialTile();
		while(uses[tileType] > maxNumberTiles) {
			tileType = this->getRandomSpecialTile();
		}

		for(int j = 0; j < PATTERN_HEIGHT; j++) {
			// If we hit the bottom on the map, we stop the pattern
			if(yOrigin + j >= _size)
				break;

			// Draw the line number j of the pattern
			for(int k = 0; k < PATTERN_WIDTH; k++) {

				// If we hit the right side of map, stop the line
				if(xOrigin + k >= _size) 
					break;

				if(patterns[patNumber][j][k] == 1) {
					uses[this->_map[tileOrigin + j*_size + k]]--;
					uses[tileType]++;
					this->_map[tileOrigin + j*_size + k] = tileType;
				}
				
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

/*!
 * \brief Remove water from sides to avoid map unplayable
 *
 */
void MapGenerator::removeWaterFromSides() {

	// Avoid on top side
	for(int i = 0; i < _size; i++) {
		if(this->_map[i] ==  Sea) {
			int index = i + _size; // means y++ on the grid
			// Increment until the swapping tile isn't sea
			while(this->_map[index] == Sea && index < _size * _size) { index++; }
			// Swap values
			this->_map[i] = this->_map[index];
			this->_map[index] = Sea;
		}
	}

	// Avoid water on the right side
	for(int i = (_size-1); i < _size*_size; i += _size) {
		if(this->_map[i] ==  Sea) {
			int index = i - 1;
			while(this->_map[index] == Sea && index < _size * _size) { index--; }

			this->_map[i] = this->_map[index];
			this->_map[index] = Sea;
		}
	}
}