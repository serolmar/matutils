﻿namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite resolver o problema do transporte.
    /// </summary>
    public class TransportationProblemDouble
    {
        /// <summary>
        /// A quantidade da oferta.
        /// </summary>
        private int supplyNumber;

        /// <summary>
        /// A quantidade da procura.
        /// </summary>
        private int demandNumber;

        /// <summary>
        /// O vector com os valores das ofertas.
        /// </summary>
        private int[] supply;

        /// <summary>
        /// O vector com os valores das procuras.
        /// </summary>
        private int[] demand;

        /// <summary>
        /// A matriz com o custo do transporte.
        /// </summary>
        double[,] transportationCost;

        /// <summary>
        /// A matriz resultado.
        /// </summary>
        private int[,] resultMatrix;

        /// <summary>
        /// O vector auxiliar u.
        /// </summary>
        private double[] u;

        /// <summary>
        /// O vector auxiliar v.
        /// </summary>
        private double[] v;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="TransportationProblemDouble"/>.
        /// </summary>
        /// <param name="supply">Os valores da oferta.</param>
        /// <param name="demand">Os valores da procura.</param>
        /// <param name="transportationCost">O custo do transporte.</param>
        public TransportationProblemDouble(int[] supply, int[] demand, double[,] transportationCost)
        {
            this.CheckSupplyAndDemand(supply, demand);
            this.supply = supply;
            this.demand = demand;
            this.transportationCost = transportationCost;
            this.supplyNumber = supply.Length;
            this.demandNumber = demand.Length;
            this.InitVariables();
        }

        /// <summary>
        /// Obtém o valor da função objectivo.
        /// </summary>
        /// <value>
        /// O valor.
        /// </value>
        public double ObjectiveFunctionValue
        {
            get
            {
                double result = 0;
                for (int i = 0; i < this.supplyNumber;++i )
                {
                    for (int j = 0; j < this.demandNumber; ++j)
                    {
                        if (this.resultMatrix[i, j] > 0)
                        {
                            result += this.resultMatrix[i, j] * this.transportationCost[i, j];
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém os resultados.
        /// </summary>
        /// <value>
        /// Os resultados.
        /// </value>
        public int[,] Results
        {
            get
            {
                int[,] results = new int[this.supplyNumber, this.demandNumber];
                for (int i = 0; i < this.supplyNumber; ++i)
                {
                    for (int j = 0; j < this.demandNumber; ++j)
                    {
                        if (this.resultMatrix[i, j] > 0)
                        {
                            results[i, j] = this.resultMatrix[i, j];
                        }
                    }
                }

                return results;
            }
        }

        /// <summary>
        /// Executa o algoritmo.
        /// </summary>
        public void Run()
        {
            this.ApplyRussels();
            this.ComputeDeltaVectors();
            while (this.ChainReaction())
            {
                this.ComputeDeltaVectors();
            }
        }

        /// <summary>
        /// Inicializa as variáveis.
        /// </summary>
        private void InitVariables()
        {
            this.resultMatrix = new int[this.supplyNumber, this.demandNumber];
            for (int i = 0; i < this.supplyNumber; ++i)
            {
                for (int j = 0; j < this.demandNumber; ++j)
                {
                    this.resultMatrix[i, j] = -1;
                }
            }

            this.u = new double[this.supplyNumber];
            this.v = new double[this.demandNumber];
        }

        /// <summary>
        /// Aplica a inicialização do tipo de norte-oeste.
        /// </summary>
        private void ApplyNorthWest()
        {
            int i = 0;
            int j = 0;
            while (i < this.supplyNumber && j < this.demandNumber)
            {
                if (supply[i] <= demand[j])
                {
                    resultMatrix[i, j] = supply[i];
                    demand[j] -= supply[i];
                    supply[i] = 0;
                    ++i;
                }
                else
                {
                    resultMatrix[i, j] = demand[j];
                    supply[i] -= demand[j];
                    demand[j] = 0;
                    ++j;
                }
            }
        }

        /// <summary>
        /// Aplica a inicialização de Vogels.
        /// </summary>
        private void ApplyVorgels()
        {
            bool[] isRowEliminated = new bool[supplyNumber];
            bool[] isColumnEliminated = new bool[demandNumber];
            int numberOfRemainingRows = supplyNumber;
            int numberOfRemainingColumns = demandNumber;
            double temporary;
            bool isMaximumDifferenceInRow = true;
            double maxDifference = -double.MaxValue;
            int rowOrColumn = 0;
            for (int i = 0; i < this.supplyNumber; ++i)
            {
                int firstMinimum = 0;
                int secondMinimum = 1;

                for (int j = secondMinimum; j < demandNumber; ++j)
                {
                    if (this.transportationCost[i, firstMinimum] > this.transportationCost[i, j])
                    {
                        secondMinimum = firstMinimum;
                        firstMinimum = j;
                    }
                    else if (this.transportationCost[i, secondMinimum] > this.transportationCost[i, j])
                    {
                        secondMinimum = j;
                    }
                }

                temporary = this.transportationCost[i, secondMinimum] - this.transportationCost[i, firstMinimum];
                if (temporary > maxDifference)
                {
                    maxDifference = temporary;
                    rowOrColumn = i;
                }
            }

            for (int i = 0; i < this.demandNumber; ++i)
            {
                int firstMinimum = 0;
                int secondMinimum = 1;

                for (int j = secondMinimum; j < this.supplyNumber; ++j)
                {
                    if (this.transportationCost[firstMinimum, i] > this.transportationCost[j, i])
                    {
                        secondMinimum = firstMinimum;
                        firstMinimum = j;
                    }
                    else if (this.transportationCost[secondMinimum, i] > this.transportationCost[j, i])
                    {
                        secondMinimum = j;
                    }
                }

                temporary = this.transportationCost[secondMinimum, i] - this.transportationCost[firstMinimum, i];
                if (temporary > maxDifference)
                {
                    maxDifference = temporary;
                    rowOrColumn = i;
                    isMaximumDifferenceInRow = false;
                }
            }

            while (numberOfRemainingRows > 0 && numberOfRemainingColumns > 0)
            {
                if (isMaximumDifferenceInRow)
                {
                    temporary = double.MaxValue;
                    int j = 0;
                    for (int i = 0; i < this.demandNumber; ++i)
                    {
                        if (this.transportationCost[rowOrColumn, i] < temporary && !isColumnEliminated[i])
                        {
                            temporary = this.transportationCost[rowOrColumn, i];
                            j = i;
                        }
                    }

                    if (this.supply[rowOrColumn] <= demand[j])
                    {
                        this.resultMatrix[rowOrColumn, j] = this.supply[rowOrColumn];
                        this.demand[j] -= this.supply[rowOrColumn];
                        this.supply[rowOrColumn] = 0;
                        isRowEliminated[rowOrColumn] = true;
                        --numberOfRemainingRows;
                    }
                    else
                    {
                        this.resultMatrix[rowOrColumn, j] = demand[j];
                        this.supply[rowOrColumn] -= demand[j];
                        isColumnEliminated[j] = true;
                        --numberOfRemainingColumns;
                    }
                }
                else
                {
                    temporary = double.MaxValue;
                    int i = 0;
                    for (int j = 0; j < this.supplyNumber; ++j)
                    {
                        if (this.transportationCost[j, rowOrColumn] < temporary && !isRowEliminated[j])
                        {
                            temporary = this.transportationCost[j, rowOrColumn];
                            i = j;
                        }
                    }

                    if (this.supply[i] <= this.demand[rowOrColumn])
                    {
                        this.resultMatrix[i, rowOrColumn] = this.supply[i];
                        this.demand[rowOrColumn] -= this.supply[i];
                        this.supply[i] = 0;
                        isRowEliminated[i] = true;
                        --numberOfRemainingRows;
                    }
                    else
                    {
                        this.resultMatrix[i, rowOrColumn] = this.demand[rowOrColumn];
                        this.supply[i] -= this.demand[rowOrColumn];
                        isColumnEliminated[rowOrColumn] = true;
                        --numberOfRemainingColumns;
                    }
                }

                if (numberOfRemainingRows == 1)
                {
                    int j = -1;
                    for (int i = 0; i < this.supplyNumber; ++i)
                    {
                        if (!isRowEliminated[i])
                        {
                            j = i;
                            i = this.supplyNumber;
                        }
                    }

                    // j contains the line number that remains
                    for (int i = 0; i < this.demandNumber; ++i)
                    {
                        if (!isColumnEliminated[i])
                        {
                            this.resultMatrix[j, i] = this.demand[i];
                        }
                    }

                    numberOfRemainingRows = 0;
                }
                else if (numberOfRemainingColumns == 1)
                {
                    int j = -1;
                    for (int i = 0; i < this.demandNumber; ++i)
                    {
                        if (!isColumnEliminated[i])
                        {
                            j = i;
                            i = this.demandNumber;
                        }
                    }

                    // j contains the line number that remains
                    for (int i = 0; i < this.supplyNumber; ++i)
                    {
                        if (!isRowEliminated[i])
                        {
                            this.resultMatrix[i, j] = supply[i];
                        }
                    }

                    numberOfRemainingColumns = 0;
                }
                else
                {
                    isMaximumDifferenceInRow = true;
                    maxDifference = -double.MaxValue;
                    rowOrColumn = -1;
                    for (int i = 0; i < this.supplyNumber; ++i)
                    {
                        if (!isRowEliminated[i])
                        {
                            int firstMinimum = -1;
                            int secondMinimum = -1;

                            for (int j = 0; j < this.demandNumber; ++j)
                            {
                                if (!isColumnEliminated[j])
                                {
                                    firstMinimum = j;
                                    j = demandNumber;
                                }
                            }

                            for (int j = firstMinimum + 1; j < this.demandNumber; ++j)
                            {
                                if (!isColumnEliminated[j])
                                {
                                    secondMinimum = j;
                                    j = this.demandNumber;
                                }
                            }

                            for (int j = secondMinimum; j < this.demandNumber; ++j)
                            {
                                if (this.transportationCost[i, firstMinimum] > this.transportationCost[i, j] && !isColumnEliminated[j])
                                {
                                    secondMinimum = firstMinimum;
                                    firstMinimum = j;
                                }
                                else if (this.transportationCost[i, secondMinimum] > this.transportationCost[i, j] && !isColumnEliminated[j])
                                {
                                    secondMinimum = j;
                                }
                            }

                            temporary = this.transportationCost[i, secondMinimum] - this.transportationCost[i, firstMinimum];
                            if (temporary > maxDifference)
                            {
                                maxDifference = temporary;
                                rowOrColumn = i;
                            }
                        }
                    }

                    for (int i = 0; i < this.demandNumber; ++i)
                    {
                        if (!isColumnEliminated[i])
                        {
                            int firstMinimum = -1;
                            int secondMinimum = -1;

                            for (int j = 0; j < this.supplyNumber; ++j)
                            {
                                if (!isRowEliminated[j])
                                {
                                    firstMinimum = j;
                                    j = supplyNumber;
                                }
                            }

                            for (int j = firstMinimum + 1; j < this.supplyNumber; ++j)
                            {
                                if (!isRowEliminated[j])
                                {
                                    secondMinimum = j;
                                    j = this.supplyNumber;
                                }
                            }

                            for (int j = secondMinimum; j < this.supplyNumber; ++j)
                            {
                                if (this.transportationCost[firstMinimum, i] > this.transportationCost[j, i] && !isRowEliminated[j])
                                {
                                    secondMinimum = firstMinimum;
                                    firstMinimum = j;
                                }
                                else if (this.transportationCost[secondMinimum, i] > this.transportationCost[j, i] && !isRowEliminated[j])
                                {
                                    secondMinimum = j;
                                }
                            }

                            temporary = this.transportationCost[secondMinimum, i] - this.transportationCost[firstMinimum, i];
                            if (temporary > maxDifference)
                            {
                                maxDifference = temporary;
                                rowOrColumn = i;
                                isMaximumDifferenceInRow = false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Aplica a incialização de Russels.
        /// </summary>
        private void ApplyRussels()
        {
            TransportationMaxDoubleNumberField[] largestRowsCosts = new TransportationMaxDoubleNumberField[supplyNumber];
            TransportationMaxDoubleNumberField[] largestColumnsCosts = new TransportationMaxDoubleNumberField[demandNumber];
            bool[] isRowEliminated = new bool[this.supplyNumber];
            bool[] isColumnEliminated = new bool[this.demandNumber];
            TransportationMaxDoubleNumberField temporary;
            for (int i = 0; i < this.supplyNumber; ++i)
            {
                largestRowsCosts[i] = new TransportationMaxDoubleNumberField(this.transportationCost[i, 0]);
            }

            for (int i = 0; i < this.demandNumber; ++i)
            {
                largestColumnsCosts[i] = new TransportationMaxDoubleNumberField(this.transportationCost[0, i]);
            }

            for (int i = 0; i < this.supplyNumber; ++i)
            {
                for (int j = 0; j < this.demandNumber; ++j)
                {
                    if (largestRowsCosts[i] < this.transportationCost[i, j])
                    {
                        largestRowsCosts[i] = new TransportationMaxDoubleNumberField(this.transportationCost[i, j]);
                    }

                    if (largestColumnsCosts[j] < this.transportationCost[i, j])
                    {
                        largestColumnsCosts[j] = new TransportationMaxDoubleNumberField(this.transportationCost[i, j]);
                    }
                }
            }

            int chosenLine = -1;
            int chosenColumn = -1;
            TransportationMaxDoubleNumberField minimumDelta = new TransportationMaxDoubleNumberField(this.transportationCost[0, 0]);
            for (int i = 0; i < this.supplyNumber; ++i)
            {
                for (int j = 0; j < this.demandNumber; ++j)
                {
                    temporary = this.transportationCost[i, j] - largestColumnsCosts[j] - largestRowsCosts[i];
                    if (temporary < minimumDelta)
                    {
                        minimumDelta = temporary;
                        chosenLine = i;
                        chosenColumn = j;
                    }
                }
            }

            int numberOfRemainingRows = this.supplyNumber;
            int numberOfRemainingColumns = this.demandNumber;
            bool state = true;
            while (state)
            {
                bool rowHasBeenEliminated = true;
                if (this.supply[chosenLine] <= this.demand[chosenColumn])
                {
                    this.resultMatrix[chosenLine, chosenColumn] = this.supply[chosenLine];
                    this.demand[chosenColumn] -= this.supply[chosenLine];
                    this.supply[chosenLine] = 0;
                    isRowEliminated[chosenLine] = true;
                    --numberOfRemainingRows;
                }
                else
                {
                    this.resultMatrix[chosenLine, chosenColumn] = this.demand[chosenColumn];
                    this.supply[chosenLine] -= this.demand[chosenColumn];
                    isColumnEliminated[chosenColumn] = true;
                    --numberOfRemainingColumns;
                    rowHasBeenEliminated = false;
                }

                if (numberOfRemainingRows == 0)
                {
                    state = false;
                }
                else if (numberOfRemainingColumns == 0)
                {
                    state = false;
                }
                else if (rowHasBeenEliminated)
                {
                    int i = 0;
                    int j = 0;
                    while (isRowEliminated[i]) ++i;
                    while (isColumnEliminated[j]) ++j;
                    for (; j < this.demandNumber; ++j)
                    {
                        if (!isColumnEliminated[j])
                        {
                            largestColumnsCosts[j] = new TransportationMaxDoubleNumberField(this.transportationCost[i, j]);
                        }
                    }
                }
                else
                {
                    int i = 0;
                    int j = 0;
                    while (isRowEliminated[i]) ++i;
                    while (isColumnEliminated[j]) ++j;
                    for (; i < this.supplyNumber; ++i)
                    {
                        if (!isRowEliminated[i])
                        {
                            largestRowsCosts[i] = new TransportationMaxDoubleNumberField(this.transportationCost[i, j]);
                        }
                    }
                }

                for (int i = 0; i < this.supplyNumber; ++i)
                {
                    if (!isRowEliminated[i])
                    {
                        for (int j = 0; j < this.demandNumber; ++j)
                        {
                            if (!isColumnEliminated[j])
                            {
                                if (largestRowsCosts[i] < this.transportationCost[i, j])
                                {
                                    largestRowsCosts[i] = new TransportationMaxDoubleNumberField(this.transportationCost[i, j]);
                                }

                                if (largestColumnsCosts[j] < this.transportationCost[i, j])
                                {
                                    largestColumnsCosts[j] = new TransportationMaxDoubleNumberField(this.transportationCost[i, j]);
                                }
                            }
                        }
                    }
                }

                chosenLine = -1;
                chosenColumn = -1;
                minimumDelta = new TransportationMaxDoubleNumberField(double.MaxValue);
                for (int i = 0; i < this.supplyNumber; ++i)
                {
                    if (!isRowEliminated[i])
                    {
                        for (int j = 0; j < this.demandNumber; ++j)
                        {
                            if (!isColumnEliminated[j])
                            {
                                temporary = this.transportationCost[i, j] - largestColumnsCosts[j] - largestRowsCosts[i];
                                if (temporary < minimumDelta)
                                {
                                    minimumDelta = temporary;
                                    chosenLine = i;
                                    chosenColumn = j;
                                }
                            }
                        }
                    }
                }
            }  //While
        }

        /// <summary>
        /// Calcula valores delta.
        /// </summary>
        private void ComputeDeltaVectors()
        {
            bool isToComputeLine = true;
            bool[] isRowEliminated = new bool[this.supplyNumber];
            bool[] isColumnEliminated = new bool[this.demandNumber];
            Stack<int> lineStack = new Stack<int>();
            Stack<int> columnStack = new Stack<int>();
            lineStack.Push(this.ChooseStartLine());
            while (lineStack.Count != 0 || columnStack.Count != 0)
            {
                if (isToComputeLine)
                {
                    if (lineStack.Count == 0)
                    {
                        isToComputeLine = false;
                    }
                    else
                    {
                        int line = lineStack.Pop();
                        isRowEliminated[line] = true;
                        for (int i = 0; i < this.demandNumber; ++i)
                        {
                            if (!isColumnEliminated[i])
                            {
                                if (this.resultMatrix[line, i] != -1)
                                {
                                    columnStack.Push(i);
                                    this.v[i] = this.transportationCost[line, i] - this.u[line];
                                    isColumnEliminated[i] = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (columnStack.Count == 0)
                    {
                        isToComputeLine = true;
                    }
                    else
                    {
                        int column = columnStack.Pop();
                        isColumnEliminated[column] = true;
                        for (int i = 0; i < this.supplyNumber; ++i)
                        {
                            if (!isRowEliminated[i])
                            {
                                if (this.resultMatrix[i, column] != -1)
                                {
                                    lineStack.Push(i);
                                    this.u[i] = this.transportationCost[i, column] - this.v[column];
                                    isRowEliminated[i] = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Escolhe a linha para iniciar as computações dos deltas.
        /// </summary>
        /// <returns>O índice da linha de início.</returns>
        private int ChooseStartLine()
        {
            int chosenLine = -1;
            chosenLine = 0;
            int numberOfAllocationsInLine = 0;
            for (int i = 0; i < this.demandNumber; ++i)
            {
                if (this.resultMatrix[0, i] != -1)
                {
                    ++numberOfAllocationsInLine;
                }
            }

            for (int i = 1; i < this.supplyNumber; ++i)
            {
                int k = 0;
                for (int j = 0; j < this.demandNumber; ++j)
                {
                    if (this.resultMatrix[i, j] != -1)
                    {
                        ++k;
                    }
                }

                if (k > numberOfAllocationsInLine)
                {
                    numberOfAllocationsInLine = k;
                    chosenLine = i;
                }
            }

            return chosenLine;
        }

        /// <summary>
        /// Aplica a acção em cadeia.
        /// </summary>
        /// <returns>Verdadeiro caso uma alteração tenha sido processada e falso caso contrário.</returns>
        /// <exception cref="MathematicsException">Em caso de erro interno.</exception>
        private bool ChainReaction()
        {
            double minimumCost = 0;
            int chosenLine = -1;
            int chosenColumn = -1;
            for (int i = 0; i < this.supplyNumber; ++i)
            {
                for (int j = 0; j < this.demandNumber; ++j)
                {
                    double temporary = this.transportationCost[i, j] - u[i] - v[j];
                    if (temporary < minimumCost)
                    {
                        minimumCost = temporary;
                        chosenLine = i;
                        chosenColumn = j;
                    }
                }
            }

            if (chosenLine == -1 || chosenColumn == -1)
            {
                return false;
            }

            bool[] isRowEliminated = new bool[this.supplyNumber];
            bool[] isColumnEliminated = new bool[this.demandNumber];
            Queue<TransportationMatrixTreeNode> nodeQueue = new Queue<TransportationMatrixTreeNode>();
            var startNode = new TransportationMatrixTreeNode()
            {
                Line = chosenLine,
                Column = chosenColumn
            };

            for (int i = 0; i < this.demandNumber; ++i)
            {
                if (this.resultMatrix[chosenLine, i] != -1 && i != chosenColumn)
                {
                    nodeQueue.Enqueue(new TransportationMatrixTreeNode()
                    {
                        Line = chosenLine,
                        Column = i,
                    });
                }
            }

            isRowEliminated[chosenLine] = true;

            while (nodeQueue.Count != 0)
            {
                var currentNode = nodeQueue.Dequeue();
                if (!isRowEliminated[currentNode.Line])
                {
                    for (int i = 0; i < this.demandNumber; ++i)
                    {
                        if (currentNode.Line == startNode.Line &&
                            i == startNode.Column)
                        {
                            startNode.Next = currentNode;
                            this.InnerChainReaction(startNode);
                            return true;
                        }

                        if (this.resultMatrix[currentNode.Line, i] != -1 &&
                            i != currentNode.Column &&
                            !isColumnEliminated[i])
                        {
                            if (!nodeQueue.Any(n => n.Line == currentNode.Line &&
                                n.Column == i))
                            {
                                nodeQueue.Enqueue(new TransportationMatrixTreeNode()
                                {
                                    Line = currentNode.Line,
                                    Column = i,
                                    Next = currentNode
                                });
                            }
                        }
                    }

                    isRowEliminated[currentNode.Line] = true;
                }

                if (!isColumnEliminated[currentNode.Column])
                {
                    for (int i = 0; i < this.supplyNumber; ++i)
                    {
                        if (i == startNode.Line &&
                            currentNode.Column == startNode.Column)
                        {
                            startNode.Next = currentNode;
                            this.InnerChainReaction(startNode);
                            return true;
                        }

                        if (this.resultMatrix[i, currentNode.Column] != -1 &&
                            i != currentNode.Line &&
                            !isRowEliminated[i])
                        {
                            if (!nodeQueue.Any(n => n.Line == i &&
                                n.Column == currentNode.Column))
                            {
                                nodeQueue.Enqueue(new TransportationMatrixTreeNode()
                                {
                                    Line = i,
                                    Column = currentNode.Column,
                                    Next = currentNode
                                });
                            }
                        }
                    }

                    isColumnEliminated[currentNode.Column] = true;
                }
            }

            throw new MathematicsException("An error occured while processing chain reacion.");
        }

        /// <summary>
        /// Auxilia a acçáo em cadeia.
        /// </summary>
        /// <param name="startNode">O nó de início.</param>
        private void InnerChainReaction(TransportationMatrixTreeNode startNode)
        {
            var currentNode = startNode.Next;
            if (currentNode == null)
            {
                return;
            }

            var minimumAllocation = this.resultMatrix[currentNode.Line, currentNode.Column];
            var toRemoveNode = currentNode;
            currentNode = currentNode.Next;
            bool status = false;
            while (currentNode != null)
            {
                if (status)
                {
                    var temp = resultMatrix[currentNode.Line, currentNode.Column];
                    if (temp < minimumAllocation)
                    {
                        minimumAllocation = temp;
                        toRemoveNode = currentNode;
                    }
                }

                currentNode = currentNode.Next;
                status = !status;
            }

            currentNode = startNode;
            while (currentNode != null)
            {
                resultMatrix[currentNode.Line, currentNode.Column] += minimumAllocation;
                currentNode = currentNode.Next;
                resultMatrix[currentNode.Line, currentNode.Column] -= minimumAllocation;
                currentNode = currentNode.Next;
            }

            resultMatrix[toRemoveNode.Line, toRemoveNode.Column] = -1;
            resultMatrix[startNode.Line, startNode.Column] = minimumAllocation;
        }

        /// <summary>
        /// Verifica a integridade dos dados da oferta e da procura.
        /// </summary>
        /// <param name="supply">A oferta.</param>
        /// <param name="demand">A procura.</param>
        /// <exception cref="MathematicsException">
        /// Se a soma da oferta não coincidir com a soma da procura.
        /// </exception>
        private void CheckSupplyAndDemand(int[] supply, int[] demand)
        {
            int supplySum = 0;
            foreach (var supplyNumber in supply)
            {
                supplySum += supplyNumber;
            }

            int demandSum = 0;
            foreach (var demandNumber in demand)
            {
                demandSum += demandNumber;
            }

            if (supplySum != demandSum)
            {
                throw new MathematicsException("The sum of the supplies must equal the sum of the demands.");
            }
        }
    }
}
