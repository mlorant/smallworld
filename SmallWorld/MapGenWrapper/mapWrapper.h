// mapWrapper.h

#pragma once
#include "api.h"
#include "common.h"
#include <vector>
#include <iostream>

using namespace System;
using namespace System::Collections::Generic;

namespace mapWrapper {
	public ref class WrapperMapGenerator
	{
	public:

		List<int>^ generate_map(int size) {

			List<int>^ mapGen = gcnew List<int>();
			int* mapNative = api_generate_map(size);
	
			int i;
			for(i = 0; i < size * size; i++) {
				mapGen->Add(mapNative[i]);
			}

			return mapGen;
		}
	};



}
