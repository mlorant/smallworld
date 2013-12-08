#ifndef MAP_H
#define MAP_H

#include <stdio.h>      /* printf, scanf, puts, NULL */
#include <stdlib.h>     /* srand, rand */
#include <time.h>       /* time */

class MapGenerator {
public:
	MapGenerator();
	int* generate_map(int size);

private:
	int* _map;
	int _size;
	int _seaThickness;

	void buildBase();

	int getSeaChance(int i, int lineNumber, int* coefSeaDecrease);
	bool isThereSeaNearby(int i);
};

#endif