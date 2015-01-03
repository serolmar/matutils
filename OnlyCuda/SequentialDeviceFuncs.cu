
// Permite calcular o m�ximo divisor comum entre dois n�meros inteiros.
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

// Permite calcular o m�ximo divisor comum entre dois n�meros inteiros sem sinal.
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

// Permite calcular o m�ximo divisor comum entre dois n�meros longos.
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

// Permite calcular o m�ximo divisor comum entre dois n�meros longos sem sinal.
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