#include "map.h"
#include <iostream>
#include <cmath> /* round */

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
MapGenerator::MapGenerator() {
	// Init random seed and map array
	srand (time(0));
}

int* MapGenerator::generate_map(int size) {
	
	// Init general parameters for the map generation
	this->_size = size;
	this->_seaThickness = (int) _size*0.15; // Create random seas around the map, 20% max

	this->_map = (int*) malloc(size * size * sizeof(int*));
	this->buildBase();
	int i;

	// TODO: Implement map generation algorithm. Idea : Perlin Noise
	for(i = size*_seaThickness; i < size * (size-_seaThickness); i++) {
		
		// Skip borders
		if((i % _size) <= _seaThickness || (i % _size) >= (_size-_seaThickness)) 
			continue;

		int proba = rand() % 100; // Between 0 and 99

		if(proba < 15) {				// 15% of chance of a Desert
			this->_map[i] = Desert;
		}
		else if(proba < 30) {			// 15% of chance of a Forest
			this->_map[i] = Forest;
		}
		else if(proba < 45) {			// 10% of chance of a Mountain
			this->_map[i] = Mountain;
		}
		else if(proba < 50) {			// 5% of chance of a Sea
			this->_map[i] = Sea;
		}
		else {							// 50% of chance of a Plain
			this->_map[i] = Plain;
		}
	}
	
	return this->_map;
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

	// Limit the number of Sea possible side by side
	int coefSeaDecrease = 0;

	// First lines
	for(int i = 0; i < _size*_seaThickness; i++) {
		int line = i / _size; // x coordinates to lowered the number of sea in the center
		int coef = this->getSeaChance(i, line, &coefSeaDecrease);
		this->_map[i] = (rand() % 100 < coef) ? Sea : Plain;
	}

	// Last line (last tile to the center)
	for(int i = _size*_size; i > (_size*_size) - (_size*_seaThickness); i--) {
		int line = _size - (i / _size);
		int coef = this->getSeaChance(i, line, &coefSeaDecrease);
		this->_map[i] = (rand() % 100 < coef) ? Sea : Plain;
	}
	
	// Sides
	for(int i = 0; i < _size*_size; i++) {
		if((i % _size) > _seaThickness &&  (i % _size) < (_size-_seaThickness)) 
			continue;

		int column = (i > (_size-_seaThickness)) ? _size - (i % _size) : i % _size;
		int coef = this->getSeaChance(i, column, &coefSeaDecrease);
		this->_map[i] = (rand() % 100 < coef) ? Sea : Plain;
	}

}

int MapGenerator::getSeaChance(int i, int lineNumber, int* coefSeaDecrease) {
	int coef;
	if(this->isThereSeaNearby(i)) {
		coef = 75 - 20*(*coefSeaDecrease);
		*coefSeaDecrease++;
	}
	else {
		coef = 50;
		*coefSeaDecrease = 0;
	}
	
	coef *= (0.5 - 0.25*(lineNumber - 2)); // Decreasing as in that approach the center
	return coef;
}

bool MapGenerator::isThereSeaNearby(int i) {
	// Left
	if((i % _size) > 0 && this->_map[i-1] == Sea)
		return true;
	// Right
	if((i % _size) < (_size-1) && this->_map[i+1] == Sea)
		return true;

	// Up	
	if(i > _size && this->_map[i-_size] == Sea)
		return true;
	// Down
	if(i < _size*(_size-1) && this->_map[i+_size] == Sea)
		return true;

	return false;
}

