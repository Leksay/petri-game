using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Petri.Formula
{
    public class FormulaConstructor
    {
        // Formula = Queue of Reagents
        // Reagen = Modifier + ConnectionType
        // Modifier = List of Parameters
        // Parameter = ApplyType + OperationType + FormulaData's Property + Value

        private FormulaData _formulaData;
        private int _sizeX, _sizeY;

        private Reagent[,] _reagentsMatrix;
        private FormulaChain[] _chains;
        private ConnectionTypes[,] _connectionMatrix;
        private FormulaNode[,] _nodeMatrix;

        public FormulaConstructor CreateFormula(int sizeX, int sizeY)
        {
            _formulaData = new FormulaData();
            _reagentsMatrix = new Reagent[sizeX, sizeY];
            _connectionMatrix = new ConnectionTypes[sizeX, sizeY];
            _nodeMatrix = new FormulaNode[sizeX, sizeY];

            _chains = new FormulaChain[sizeX];

            for (var i = 0; i < _chains.Length; i++)
            {
                _chains[i] = new FormulaChain();
            }

            _sizeX = sizeX;
            _sizeY = sizeY;

            return this;
        }

        public void AddReagent(Reagent reagent, int x, int y)
        {
            _reagentsMatrix[x, y] = reagent;
            RecalculateChains();
            RecalculateConnectionMatrix();
        }

        public void RemoveReagent(int x, int y)
        {
            _reagentsMatrix[x, y] = null;
            _connectionMatrix[x, y] = ConnectionTypes.None;
            RecalculateChains();
            RecalculateConnectionMatrix();
        }

        private void RecalculateConnectionMatrix()
        {
            var newArray = new ConnectionTypes[_sizeX, _sizeY];
            foreach (var chain in _chains)
            {
                foreach (var node in chain.AllNodes)
                {
                    var connectionTypes = ConnectionTypes.None;
                    foreach (var inputNode in node.Input)
                    {
                        connectionTypes |= inputNode.Reagent.ConnectionType.Inverse();
                    }

                    if (node.Output != null)
                    {
                        newArray[node.Position.x, node.Position.y] |= connectionTypes;
                    }

                    newArray[node.Position.x, node.Position.y] = connectionTypes;
                }
            }
            
            _connectionMatrix = newArray;
        }
        
        private void RecalculateChains()
        {
            _nodeMatrix = new FormulaNode[_sizeX, _sizeY];
            for (var x = 0; x < _chains.Length; x++)
            {
                var chain = _chains[x];
                chain.Clear();

                var startNodeReagent = _reagentsMatrix[x, 0];
                if (startNodeReagent == null)
                {
                    continue;
                }

                var currentPosition = new Vector2Int(x, 0);
                var currentNode = GetNode(currentPosition);
                chain.StartNode = currentNode;
                chain.AllNodes.Add(currentNode);

                var chainFinished = false;
                while (!chainFinished)
                {
                    if (!TryGetNextReagent(currentPosition.x, currentPosition.y, out var nextPosition))
                    {
                        chainFinished = true;
                        chain.EndNode = currentNode;
                        continue;
                    }

                    if(nextPosition == currentNode.Position)
                    {
                        chainFinished = true;
                        chain.EndNode = currentNode;
                        chain.EndNode.IsBoundNode = true;
                        continue;
                    }

                    var nextNode = GetNode(nextPosition);
                    currentPosition = nextPosition;

                    nextNode.Input.Add(currentNode);
                    currentNode.Output = nextNode;

                    var nextNodeInChain = IsReagentInChain(nextPosition);
                    if (nextNodeInChain)
                    {
                        chainFinished = true;
                        chain.EndNode = currentNode;
                        chain.EndsInOtherChain = true;

                        currentNode.IsPointsToOtherChain = true;
                        var nodeInChain = GetNode(nextPosition);
                        nodeInChain.Input.Add(currentNode);

                        continue;
                    }

                    currentNode = nextNode;
                    chain.AllNodes.Add(currentNode);
                }
            }

#if UNITY_EDITOR
            var node = _chains[0].StartNode;
            var reagents = string.Empty;

            if (node != null)
            {
                while (node != null)
                {
                    if (node.Input.Count > 0)
                    {
                        reagents += "->";
                    }
                    reagents += $"{node.Reagent.Modifier.Parameter.ReagentGroup.ToString().Replace("Group", "")}";
                    node = node.Output;
                }
            }

            Debug.Log(reagents);
#endif
        }

        private bool TryGetNextReagent(int x, int y, out Vector2Int nextReagentXY)
        {
            nextReagentXY = Vector2Int.zero;

            var currentReagent = _reagentsMatrix[x, y];
            if (currentReagent == null)
            {
                return false;
            }

            switch (currentReagent.ConnectionType)
            {
                case ConnectionTypes.Up:
                    nextReagentXY = new Vector2Int(x, y - 1);
                    break;
                case ConnectionTypes.Right:
                    nextReagentXY = new Vector2Int(x + 1, y);
                    break;
                case ConnectionTypes.Down:
                    nextReagentXY = new Vector2Int(x, y + 1);
                    break;
                case ConnectionTypes.Left:
                    nextReagentXY = new Vector2Int(x - 1, y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            nextReagentXY.x = Mathf.Clamp(nextReagentXY.x, 0, _sizeX - 1);
            nextReagentXY.y = Mathf.Clamp(nextReagentXY.y, 0, _sizeY - 1);

            var nextReagent = _reagentsMatrix[nextReagentXY.x, nextReagentXY.y];
            return nextReagent != null;
        }

        public ConnectionTypes[,] GetConnectionMatrix()
        {
            var newArray = new ConnectionTypes[_sizeX, _sizeY];
            Array.Copy(_connectionMatrix, newArray, _connectionMatrix.Length);

            return newArray;
        }

        /// <summary>
        /// Get matrix of bools. True, if reagent is in chain.
        /// </summary>
        /// <returns>Matrix of bools</returns>
        public bool[,] GetChainedCellsMatrix()
        {
            var matrix = new bool[_sizeX, _sizeY];

            foreach (var chain in _chains)
            {
                foreach (var node in chain.AllNodes)
                {
                    matrix[node.Position.x, node.Position.y] = true;
                }
            }

            return matrix;
        }
        
        public List<Vector2Int?> GetChainsTails() => _chains.Select(x => x.EndsInOtherChain ? null : x.EndNode?.Position).ToList();
        
        public bool IsReagentInChain(int x, int y) => IsReagentInChain(new Vector2Int(x, y));

        public bool IsReagentInChain(Vector2Int position) => _chains.Any(chain => chain.AllNodes.Any(node => node.Position == position));

        public FormulaNode GetNode(int x, int y) => GetNode(new Vector2Int(x, y));

        public FormulaNode GetNode(Vector2Int position)
        {
            if (position.x < 0 || position.y < 0 || position.x >= _sizeX || position.y >= _sizeY)
            {
                Debug.LogError($"Can't get node with {position}");
                return null;
            }

            var node = _nodeMatrix[position.x, position.y];
            if (node != null)
            {
                return node;
            }

            return _nodeMatrix[position.x, position.y] = new FormulaNode(_reagentsMatrix[position.x, position.y], position);
        }
    }
}