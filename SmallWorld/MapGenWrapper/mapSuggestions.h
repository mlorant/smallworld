// mapWrapper.h

#pragma once
#include "api.h"
#include "common.h"
#include "suggestions.h"
#include <vector>
#include <iostream>

using namespace System;
using namespace System::Collections::Generic;

namespace mapWrapper {

	/*!
	 * Map Tile Suggestion wrapper
	 */
	public ref class WrapperMapSuggestion
	{
		int mapSize;  /*!< Map size of the current game */

	public:
		/*!
		 * Initialize the engine with map given
		 */
		WrapperMapSuggestion(List<int>^ map, int mapSize) {

			this->mapSize = mapSize;

			// Transform map from int to CaseType
			CaseType** mapEnum = new CaseType*[mapSize];
			for(int i = 0; i < mapSize; i++) {
				mapEnum[i] = new CaseType[mapSize];
				for(int j = 0; j < mapSize; j++) {
					mapEnum[i][j] = static_cast<CaseType>(map[i*mapSize + j]);
				}
			}

			init_map_suggestion(mapEnum, mapSize);

			// Delete of the mapEnum is managed in the Suggestion class
		}


		/*!
		 * Suggest positions, according to the current game
		 */
		List<Tuple<int, int>^>^ get_tiles_suggested(List<int>^ units, int currentX, int currentY, double ptDepl, int currentNation) {

			// Transform int to UnitType array
			UnitType** unitsEnum = new UnitType*[mapSize];
			for(int i = 0; i < mapSize; i++) {
				unitsEnum[i] = new UnitType[mapSize];
				for(int j = 0; j < mapSize; j++) {
					unitsEnum[i][j] = static_cast<UnitType>(units[i*mapSize + j]);
				}
			}
			
			// Get tiles suggestions
			UnitType nation = static_cast<UnitType>(currentNation);
			std::vector<std::pair<int, int>> tilesSuggested = api_get_tiles_suggestion(unitsEnum, currentX, currentY, ptDepl, nation);
	
			// Format suggestion into list of tuples
			List<Tuple<int, int>^>^ suggestions = gcnew List<Tuple<int, int>^>();
			for(int i = 0; i < tilesSuggested.size(); i++) {
				Tuple<int, int>^ t = gcnew Tuple<int, int>(tilesSuggested[i].first, tilesSuggested[i].second);
				suggestions->Add(t);
			}

			// Free the units enum array
			for(int i = 0; i < mapSize; i++)
				delete[] unitsEnum[i];
			delete[] unitsEnum;

			return suggestions;
		}
	};
}