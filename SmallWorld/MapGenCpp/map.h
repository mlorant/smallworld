#ifndef MAP_H
#define MAP_H

#include <stdio.h>      /* printf, scanf, puts, NULL */
#include <stdlib.h>     /* srand, rand */
#include <time.h>       /* time */

#define NB_TILE_TYPES 5

enum CaseType
{
	Desert = 0,
	Forest,
	Mountain,
	Plain,
	Sea
};

/*! \class MapGenerator
 * \brief Generate random map with patterns 
 *
 * Generate random map with natural elements (Forest, Mountain, ...)
 * and try to make patterns to have a continental map.
 */
class MapGenerator {
public:
	/*!
     * Initialize random seed and private attributes
     */
	MapGenerator();

	/*!
     * \brief Return a random map
     *
     * Generate and returns a new map array of the size desired
     */
	int* generate_map(int size);

private:
	int* _map; /*!< Map array, representing each tile type */
	int _size; /*!< Map size, corresponding to the width of one side */
	bool tilesUsed[NB_TILE_TYPES]; /*!< Array of tile already used (for the random with patterns) */

	/*!
     * \brief Generate a (very) random map 
     *
     * Modify the map to be random: put a random tile type on
	 * each index of the map array. Plain has more chance to 
	 * appears than other tile.
     *
     */
	void pureRandom();

	/*!
     * \brief Generate a random map with predefined patterns
     *
     * Modify the map to be random, but with patterns. Add
	 * many patterns on the map, of different tile type, which
	 * produces circles, squares, etc.
     *
     */
	void randomWithPatterns();

	/*!
     * \brief Fill the whole map of Plain
     *
     * Fill the whole map of Plain
     *
     */
	void buildBase();

	/*!
     * \brief Return a tile type index
     *
     * Return a random tile type, except Plain.
     *
     */
	int getRandomSpecialTile();
};





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

#endif