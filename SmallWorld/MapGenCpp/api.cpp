#include "api.h"
#include "map.h"

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
