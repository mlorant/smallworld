#include "api.h"
#include "map.h"

int* api_generate_map(int size) {
	MapGenerator gen;
	return gen.generate_map(size);
}
