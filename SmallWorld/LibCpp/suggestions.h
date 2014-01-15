#ifndef SUGGESTIONS_H
#define SUGGESTIONS_H

#include "common.h"
#include <vector>

using namespace std;

enum MoveType
{
	Impossible = 0,
	Possible,
	Suggested
};


/*! \class Suggestion
 * \brief Suggest tiles where the current unit can go
 *
 * Suggest tiles where the current unit select can go,
 * according to the environnement : unit type, opponent,
 * tiles types..
 */
class Suggestion
{
	CaseType** _map;    /*!< Map array, representing each tile type */
	UnitType** _units;  /*!< Units array, representing which unit is on each tile */
	int _mapSize;       /*!< Map width */
	
	int** _sugg;        /*!< Suggestion array, to indicate where the unit can go */
	int _currentX;      /*!< Current X position of the unit */
	int _currentY;      /*!< Current Y position of the unit */
	int _movePoints;    /*!< Move points available for the unit */
	UnitType _nation;   /*!< Unit type of the current unit */

	
	/*!
     * \brief Mark every tiles around the unit as "Possible", if
	 * it isn't water
     */
	void moveAround(int x, int y, float moves);

	/*!
     * \brief Mark every mountains as "Possible"
     */
	void moveMountains();

	/*!
     * \brief Check if there's a unit around this case
     */
	bool unitNearBy(int x, int y, UnitType nation);

	/*!
     * \brief Check if there's water near the tile
     */
	bool waterNearBy(int x, int y);


public:

	Suggestion(CaseType** map, int mapSize);

	~Suggestion();

	/*!
     * \brief Returns suggestions tiles
     */
	vector<tuple<int, int, int>> suggestion(UnitType** units, int currentX,int currentY, double ptDepl, UnitType currentNation);

};

#endif