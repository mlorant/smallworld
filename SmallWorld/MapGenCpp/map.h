#ifndef MAP_H
#define MAP_H

#include <stdio.h>      /* printf, scanf, puts, NULL */
#include <stdlib.h>     /* srand, rand */
#include <time.h>       /* time */

class MapGenerator {
public:
	int* generate_map(int size);
};

#endif