#ifndef API_H
#define API_H

#define EXTERNC extern "C"
#ifdef MAP_DLL_EXPORT
	#define DLL __declspec(dllexport)
#else
	#define DLL __declspec(dllimport)
#endif


EXTERNC DLL int* api_generate_map(int size);

#endif