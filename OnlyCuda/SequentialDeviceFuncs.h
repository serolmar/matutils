#ifndef SEQUENTIALDEVICEFUNCS_H
#define SEQUENTIALDEVICEFUNCS_H

/*
 * Declara��o das fun��es auxiliares de dispositivo que poder�o ser chamadas a partir
 * de outros m�dulos.
*/

// Permite calcular o m�ximo divisor comum entre dois n�meros inteiros.
extern "C" __device__ int GreatCommonDivisorInt(int x, int y);

// Permite calcular o m�ximo divisor comum entre dois n�meros inteiros sem sinal.
extern "C" __device__ unsigned int GreatCommonDivisorUnsignedInt(unsigned int x, unsigned int y);

// Permite calcular o m�ximo divisor comum entre dois n�meros longos.
extern "C" __device__ long GreatCommonDivisorLong(long x, long y);

// Permite calcular o m�ximo divisor comum entre dois n�meros longos sem sinal.
extern "C" __device__ long GreatCommonDivisorUnsignedLong(unsigned long x, unsigned long y);

#endif