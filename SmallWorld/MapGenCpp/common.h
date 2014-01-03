#ifndef COMMON_H
#define COMMON_H

#define NB_TILE_TYPES 5

enum CaseType
{
	Desert = 0,
	Forest,
	Mountain,
	Plain,
	Sea
};


enum UnitType
{
	None, 
	Viking,
	Gallic,
	Dwarf
};

struct Point {
	int x;
	int y;
};

#endif