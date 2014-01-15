#include "api.h"
#include "common.h"
#include "map.h"
#include "suggestions.h"
#include <vector>

/*!
 * \brief Generate a map with random cases
 *
 * Generate an array of numbers, which represents squares types.
 * Representation of numbers are given by the `CaseType` enum.
 *
 * \param size : number of squares of the map (side length)
 * \return array of number between 0 and 4, with a length of size^2
 */
int* api_generate_map(int size) {
	MapGenerator gen;
	return gen.generate_map(size);
}

Suggestion* gen;

void init_map_suggestion(CaseType** map, int mapSize) {
	gen = new Suggestion(map, mapSize);
}

std::vector<std::tuple<int, int, int>> api_get_tiles_suggestion(UnitType** units, int currentX,int currentY, double ptDepl, UnitType currentNation) {
	return gen->suggestion(units, currentX, currentY, ptDepl, currentNation);
}