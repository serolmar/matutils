namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo que permite factorizar polinómios cujos coeficientes são elementos
    /// de um corpo finito.
    /// </summary>
    public class FiniteFieldPolFactorizationAlgorithm<T>
        : IAlgorithm<UnivariatePolynomialNormalForm<T, IField<T>>,
        Dictionary<UnivariatePolynomialNormalForm<T, IField<T>>, int>>
    {
        /// <summary>
        /// Executa o algoritmo sobre polinómios com coeficientes em corpos finitos.
        /// </summary>
        /// <param name="polynom">O polinómio a ser factorizado.</param>
        /// <returns>
        /// O dicionário que contém cada um dos factores e o respectivo grau.
        /// </returns>
        public Dictionary<UnivariatePolynomialNormalForm<T, IField<T>>, int> Run(
            UnivariatePolynomialNormalForm<T, IField<T>> polynom)
        {
            throw new NotImplementedException();
        }
    }
}
