#ifndef SEQUENTIALDEVICEFUNCS_H
#define SEQUENTIALDEVICEFUNCS_H

/*
 * Declaração das funções auxiliares de dispositivo que poderão ser chamadas a partir
 * de outros módulos.
*/

// Permite calcular o máximo divisor comum entre dois números inteiros.
extern "C" __device__ int GreatCommonDivisorInt(int x, int y);

// Permite calcular o máximo divisor comum entre dois números inteiros sem sinal.
extern "C" __device__ unsigned int GreatCommonDivisorUnsignedInt(unsigned int x, unsigned int y);

// Permite calcular o máximo divisor comum entre dois números longos.
extern "C" __device__ long GreatCommonDivisorLong(long x, long y);

// Permite calcular o máximo divisor comum entre dois números longos sem sinal.
extern "C" __device__ long GreatCommonDivisorUnsignedLong(unsigned long x, unsigned long y);

#endif