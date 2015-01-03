
// Permite calcular o máximo divisor comum entre dois números inteiros.
extern "C" __device__ int GreatCommonDivisorInt(int x, int y){
	int innerx = x, innery = y, aux = 0;
	while (innery != 1)
	{
		aux = innery;
		innery = innerx % innery;
		innerx = aux;
	}

	return innerx;
}

// Permite calcular o máximo divisor comum entre dois números inteiros sem sinal.
extern "C" __device__ unsigned int GreatCommonDivisorUnsignedInt(unsigned int x, unsigned int y){
	int innerx = x, innery = y, aux = 0;
	while (innery != 1)
	{
		aux = innery;
		innery = innerx % innery;
		innerx = aux;
	}

	return innerx;
}

// Permite calcular o máximo divisor comum entre dois números longos.
extern "C" __device__ long GreatCommonDivisorLong(long x, long y){
	int innerx = x, innery = y, aux = 0;
	while (innery != 1)
	{
		aux = innery;
		innery = innerx % innery;
		innerx = aux;
	}

	return innerx;
}

// Permite calcular o máximo divisor comum entre dois números longos sem sinal.
extern "C" __device__ unsigned long GreatCommonDivisorUnsignedLong(unsigned long x, unsigned long y){
	int innerx = x, innery = y, aux = 0;
	while (innery != 1)
	{
		aux = innery;
		innery = innerx % innery;
		innerx = aux;
	}

	return innerx;
}