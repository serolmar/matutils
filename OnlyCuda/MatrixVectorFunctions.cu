#include "SequentialDeviceFuncs.h"

/// <summary>
/// Adição de um vector de inteiros a outro, sendo o resultado estabelecido no parâmetro a.
/// </summary>
/// <param name="a">O primeiro vector a ser adicionado.</param>
/// <param name="b">O segundo vector a ser adicionado.</param>
extern "C" __global__  void AddInt(int* a, int *b)
{
	int x = blockIdx.x * blockDim.x + threadIdx.x;
	a[x] += b[x];
}

/// <summary>
/// Adição de um vector de inteiros sem sinal a outro, sendo o resultado estabelecido no parâmetro a.
/// </summary>
/// <param name="a">O primeiro vector a ser adicionado.</param>
/// <param name="b">O segundo vector a ser adicionado.</param>
extern "C" __global__  void AddUnsignedInt(unsigned int* a, unsigned int *b)
{
	int x = blockIdx.x * blockDim.x + threadIdx.x;
	a[x] += b[x];
}

/// <summary>
/// Adição de um vector de longos a outro, sendo o resultado estabelecido no parâmetro a.
/// </summary>
/// <param name="a">O primeiro vector a ser adicionado.</param>
/// <param name="b">O segundo vector a ser adicionado.</param>
extern "C" __global__  void AddLong(long* a, long *b)
{
	int x = blockIdx.x;
	a[x] += b[x];
}

/// <summary>
/// Adição de um vector de longos sem sinal a outro, sendo o resultado estabelecido no parâmetro a.
/// </summary>
/// <param name="a">O primeiro vector a ser adicionado.</param>
/// <param name="b">O segundo vector a ser adicionado.</param>
extern "C" __global__  void AddUnsignedLong(unsigned long * a, unsigned long *b)
{
	int x = blockIdx.x;
	a[x] += b[x];
}

/// <summary>
/// Adição de um vector de vírgula flutuante de precisão simples a outro, sendo o resultado 
/// estabelecido no parâmetro a.
/// </summary>
/// <param name="a">O primeiro vector a ser adicionado.</param>
/// <param name="b">O segundo vector a ser adicionado.</param>
extern "C" __global__ void AddFloat(float *a, float *b){
	int x = blockIdx.x * blockDim.x + threadIdx.x;
	a[x] += b[x];
}

/// <summary>
/// Adição de um vector de vírgula flutuante de precisão simples a outro, sendo o resultado 
/// estabelecido no parâmetro a.
/// </summary>
/// <param name="a">O primeiro vector a ser adicionado.</param>
/// <param name="b">O segundo vector a ser adicionado.</param>
extern "C" __global__  void AddDouble(double* a, double *b)
{
	int x = blockIdx.x * blockDim.x + threadIdx.x;
	a[x] += b[x];
}

/// <summary>
/// Permite adicionar vectores de fracções, tendo em conta que cada vector contém inteiros
/// relativos ao numerador e ao denominador.
/// </summary>
/// <param name="a">O primeiro vector a ser adicionado.</param>
/// <param name="b">O segundo vector a ser adicionado.</param>
extern "C" __global__ void AddIntegerFraction(int* a, int * b){
	int gcd;
	int nextx;
	int x = (blockIdx.x * blockDim.x + threadIdx.x) << 1;
	nextx = x + 1;

	// Efectua a multiplicação simples das fracções.
	a[x] *= b[nextx];
	a[x] += b[x] * a[x];
	a[nextx] *= b[nextx];

	// Aplica o máximo divisor comum a ambos os itens.
	gcd = GreatCommonDivisorInt(a[x], a[nextx]);
	a[x] /= gcd;
	a[nextx] /= gcd;
}

/// <summary>
/// Permite adicionar vectores de fracções, tendo em conta que cada vector contém 2 * length inteiros sem sinal
/// relativos ao numerador e ao denominador.
/// </summary>
/// <param name="a">O primeiro vector a ser adicionado.</param>
/// <param name="b">O segundo vector a ser adicionado.</param>
extern "C" __global__ void AddUnsignedIntegerFraction( unsigned int* a, unsigned int* b){
	unsigned int gcd;
	int nextx;
	int x = (blockIdx.x * blockDim.x + threadIdx.x) << 1;
	nextx = x + 1;

	// Efectua a multiplicação simples das fracções.
	a[x] *= b[nextx];
	a[x] += b[x] * a[x];
	a[nextx] *= b[nextx];

	// Aplica o máximo divisor comum a ambos os itens.
	gcd = GreatCommonDivisorUnsignedInt(a[x], a[nextx]);
	a[x] /= gcd;
	a[nextx] /= gcd;
}

/// <summary>
/// Permite adicionar vectores de fracções, tendo em conta que cada vector contém 2 * length longos
/// relativos ao numerador e ao denominador.
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
extern "C" __global__ void AddLongFraction(long* a, long* b){
	long gcd;
	long nextx;
	int x = (blockIdx.x * blockDim.x + threadIdx.x) << 1;
	nextx = x + 1;

	// Efectua a multiplicação simples das fracções.
	a[x] *= b[nextx];
	a[x] += b[x] * a[x];
	a[nextx] *= b[nextx];

	// Aplica o máximo divisor comum a ambos os itens.
	gcd = GreatCommonDivisorLong(a[x], a[nextx]);
	a[x] /= gcd;
	a[nextx] /= gcd;
}

/// <summary>
/// Permite adicionar vectores de fracções, tendo em conta que cada vector contém 2 * length longos sem sinal
/// relativos ao numerador e ao denominador.
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
extern "C" __global__ void AddUnsignedLongFraction(unsigned long* a, unsigned long* b){
	unsigned long gcd;
	unsigned long nextx;
	int x = (blockIdx.x * blockDim.x + threadIdx.x) << 1;
	nextx = x + 1;

	// Efectua a multiplicação simples das fracções.
	a[x] *= b[nextx];
	a[x] += b[x] * a[x];
	a[nextx] *= b[nextx];

	// Aplica o máximo divisor comum a ambos os itens.
	gcd = GreatCommonDivisorLong(a[x], a[nextx]);
	a[x] /= gcd;
	a[nextx] /= gcd;
}

/// <summary>
/// Permite calcular o produto escalar de tantas secções de dois vectores quantos os blocos de chamada.
/// </summary>
/// <remarks>
/// O processo de determinação do produto escalar baseia-se na redução por blocos e o tamanho dos vectores
/// terá de ser uma potência de dois.
/// Ver http://developer.download.nvidia.com/assets/cuda/files/reduction.pdf
/// </remarks>
/// <param name="a"></param>
/// <param name="b"></param>
/// <param name="c"></param>
extern "C" __global__ void InnerProdIntegerVectorRed(int* a, int* b, int* c){
	// A memória estática terá de ser reservada no código de anfitrião de modo a poder ser utilizada
	extern __shared__ int* innera;
	extern __shared__ int* innerb;

	unsigned int tid = threadIdx.x;
	unsigned int i = blockIdx.x * blockDim.x + threadIdx.x;
	innera[tid] = a[tid];
	innerb[tid] = b[tid];
	__syncthreads();
}