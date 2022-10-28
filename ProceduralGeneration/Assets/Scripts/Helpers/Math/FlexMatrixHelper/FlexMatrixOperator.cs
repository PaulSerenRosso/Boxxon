using System;
using Unity.VisualScripting;
using UnityEngine;

namespace FlexMatrixHelper
{
    public static class FlexMatrixOperator
    {
        public static FlexMatrix Multiply(this FlexMatrix _firstMatrix, FlexMatrix _secondMatrix)
        {
            if (_firstMatrix.RowLength == _secondMatrix.ColumnLength)
            {
                FlexMatrix result = new FlexMatrix(_secondMatrix.RowLength, _firstMatrix.ColumnLength);
                for (int i = 0; i < _firstMatrix.ColumnLength; i++)
                {
                    FlexMatrixLine row = _firstMatrix.Rows[i];
                    for (int j = 0; j < _secondMatrix.RowLength; j++)
                    {
                        FlexMatrixLine column = _secondMatrix.Columns[j];
                        for (int k = 0; k < _firstMatrix.RowLength; k++)
                        {
                            result.Rows[i].Values[j] += row.Values[k] * column.Values[k];
                        }

                        result.UpdateColumns();
                    }
                }

                return result;
            }

            throw new Exception("Matrixes are not compatibles");
        }

        public static FlexMatrix Add(this FlexMatrix _firstMatrix, FlexMatrix _secondMatrix)
        {
            if (_firstMatrix.RowLength == _secondMatrix.RowLength &&
                _firstMatrix.ColumnLength == _secondMatrix.ColumnLength)
            {
                FlexMatrix result = new FlexMatrix(_firstMatrix.RowLength, _firstMatrix.ColumnLength);
                for (int i = 0; i < _firstMatrix.ColumnLength; i++)
                {
                    for (int j = 0; j < _firstMatrix.RowLength; j++)
                    {
                        result.Rows[i].Values[j] = _firstMatrix.Rows[i].Values[j] + _secondMatrix.Rows[i].Values[j];
                    }
                }

                return result;
            }

            throw new Exception("Matrixes are not compatibles");
        }


        public static FlexMatrix Subtract(this FlexMatrix _firstMatrix, FlexMatrix _secondMatrix)
        {
            if (_firstMatrix.RowLength == _secondMatrix.RowLength &&
                _firstMatrix.ColumnLength == _secondMatrix.ColumnLength)
            {
                FlexMatrix result = new FlexMatrix(_firstMatrix.RowLength, _firstMatrix.ColumnLength);
                for (int i = 0; i < _firstMatrix.ColumnLength; i++)
                {
                    for (int j = 0; j < _firstMatrix.RowLength; j++)
                    {
                        result.Rows[i].Values[j] = _firstMatrix.Rows[i].Values[j] - _secondMatrix.Rows[i].Values[j];
                    }
                }

                return result;
            }

            throw new Exception("Matrixes are not compatibles");
        }

        public static FlexMatrix GetIdentityMatrix(int _size)
        {
            FlexMatrix result = new FlexMatrix(_size, _size);
            for (int i = 0; i < result.Rows.Length; i++)
            {
                result.Rows[i].Values[i] = 1;
            }

            return result;
        }

        public static bool IsSquared(this FlexMatrix _matrix)
        {
            if (_matrix.ColumnLength == _matrix.RowLength)
            {
                return true;
            }

            return false;
        }

        public static float GetDeterminant(this FlexMatrix _matrix)
        {
            float determinant = 0;
            switch (_matrix.ColumnLength, _matrix.RowLength)
            {
                case (2, 2):
                {
                    FlexMatrixLine firstRow = _matrix.Rows[0]; 
                    FlexMatrixLine secondRow = _matrix.Rows[1];
                    determinant = firstRow.Values[0] * secondRow.Values[1] -
                                  firstRow.Values[1] * secondRow.Values[0];
                    break;
                }
                case (3, 3):
                {
                    FlexMatrixLine firstRow = _matrix.Rows[0]; 
                    FlexMatrixLine secondRow = _matrix.Rows[1];
                    FlexMatrixLine thirdRow = _matrix.Rows[2];
                    determinant = firstRow.Values[0] * secondRow.Values[1] * thirdRow.Values[2]
                                  + firstRow.Values[1] * secondRow.Values[2] * thirdRow.Values[0]
                                  + firstRow.Values[2] * secondRow.Values[0] * thirdRow.Values[1]
                                  - firstRow.Values[2] * secondRow.Values[1] * thirdRow.Values[0]
                                  + firstRow.Values[1] * secondRow.Values[0] * thirdRow.Values[2]
                                  + firstRow.Values[0] * secondRow.Values[2] * thirdRow.Values[1];
                    break;
                }
                default:
                {
                    throw new Exception("Matrix must be equal to 2x2 or 3x3");
                }
            }
            return determinant; 
        }

        public static FlexMatrix Inverse(this FlexMatrix _matrix)
        {
            float determinant = GetDeterminant(_matrix);
            if (determinant == 0)
            {
                throw new Exception("Matrix must be valid");
            }
            
            switch (_matrix.ColumnLength, _matrix.RowLength)
            {
                case (2, 2):
                {
                    float firstRowFirstElement = _matrix.Rows[1].Values[1]/determinant;
                    float firstRowSecondElement = -_matrix.Rows[0].Values[1]/determinant ;
                    float secondRowFirstElement = -_matrix.Rows[1].Values[0]/determinant ;
                    float secondRowSecondElement = _matrix.Rows[0].Values[0]/determinant ;
                    _matrix.Rows[0].Values[0] = firstRowFirstElement;
                    _matrix.Rows[0].Values[1] = firstRowSecondElement;
                    _matrix.Rows[1].Values[0] = secondRowFirstElement;
                    _matrix.Rows[1].Values[1] = secondRowSecondElement;
                    _matrix.UpdateColumns();
                    break;
                }
                default:
                {
                    throw new Exception("Matrix must be equal to 2x2");
                    break;
                }
            }
            
            return _matrix;
        }
    }
}