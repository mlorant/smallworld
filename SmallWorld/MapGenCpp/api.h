#ifndef API_H
#define API_H
#include "common.h"
#include <vector>

#define EXTERNC extern "C"
#ifdef MAP_DLL_EXPORT
	#define DLL __declspec(dllexport)
#else
	#define DLL __declspec(dllimport)
#endif


DLL int* api_generate_map(int size);

DLL void init_map_suggestion(CaseType** map, int mapSize);
DLL std::vector<std::pair<int, int>> api_get_tiles_suggestion(UnitType** units, int currentX, int currentY, double ptDepl, UnitType currentNation);

#endif