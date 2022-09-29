using System;
using Unity.Mathematics;
using UnityEngine;

namespace FlexMatrixHelper
{
    [Serializable]
    public struct FlexMatrix
    {
        //matrice identity *
        // matrice rotation projection 
        public FlexMatrix(FlexMatrixLine[] _matrixLinesRow)
        {
            CheckMatrixSizeIsValid(_matrixLinesRow);
            rowLength = _matrixLinesRow[0].Values.Length;
            columnLength = _matrixLinesRow.Length;
            Rows = new FlexMatrixLine[columnLength];
            for (int i = 0; i < columnLength; i++)
            {
                Rows[i] = new FlexMatrixLine(_matrixLinesRow[i].Values);
            }

            Columns = new FlexMatrixLine[rowLength];
            for (int i = 0; i < RowLength; i++)
            {
                Columns[i] = new FlexMatrixLine(ColumnLength);
                for (int j = 0; j < Columns[i].Values.Length; j++)
                {
                    Columns[i].Values[j] = _matrixLinesRow[j].Values[i];
                }
            }
        }

        static bool CheckMatrixSizeIsValid(FlexMatrixLine[] _matrixLinesRow)
        {
            for (int i = 0; i < _matrixLinesRow.Length; i++)
            {
                for (int j = i + 1; j < _matrixLinesRow.Length; j++)
                {
                    if (_matrixLinesRow[i].Values.Length != _matrixLinesRow[j].Values.Length)
                    {
                        throw new Exception("Matrix Rows don't have the same size");
                    }
                }
            }

            return true;
        }

        public FlexMatrix(int _RowLength, int _ColumnLength)
        {
            rowLength = _RowLength;
            columnLength = _ColumnLength;
            Rows = new FlexMatrixLine[columnLength];
            Columns = new FlexMatrixLine[rowLength];
            for (int i = 0; i < RowLength; i++)
            {
                Columns[i] = new FlexMatrixLine(ColumnLength);
            }

            for (int i = 0; i < ColumnLength; i++)
            {
                Rows[i] = new FlexMatrixLine(RowLength);
            }
        }

        public void InitializeMatrixConfiguredInEditor()
        {
            rowLength = Rows[0].Values.Length;
            CheckMatrixSizeIsValid(Rows);
            Debug.Log(RowLength);
            // une fonction
            columnLength = Rows.Length;
            Columns = new FlexMatrixLine[RowLength];
            for (int i = 0; i < RowLength; i++)
            {
                Columns[i] = new FlexMatrixLine(ColumnLength);
            }

            UpdateColumns();
        }

        public void UpdateColumns()
        {
            for (int i = 0; i < RowLength; i++)
            {
                for (int j = 0; j < Columns[i].Values.Length; j++)
                {
                    Columns[i].Values[j] = Rows[j].Values[i];
                }
            }
        }

        public void UpdateRows()
        {
            for (int i = 0; i < ColumnLength; i++)
            {
                for (int j = 0; j < Rows[i].Values.Length; j++)
                {
                    Rows[i].Values[j] = Rows[j].Values[i];
                }
            }
        }

        public static implicit operator FlexMatrix(Vector3 vector)
        {
            FlexMatrix result = new FlexMatrix(new[]
            {
                new FlexMatrixLine(new[] { vector.x }),
                new FlexMatrixLine(new[] { vector.y }), new FlexMatrixLine(new[] { vector.z })
            });
            return result;
        }

        public static implicit operator FlexMatrix(Vector2 vector)
        {
            FlexMatrix result = new FlexMatrix(new[]
            {
                new FlexMatrixLine(new[] { vector.x }),
                new FlexMatrixLine(new[] { vector.y })
            });
            return result;
        }

        public static implicit operator Vector3(FlexMatrix matrix)
        {
            if (matrix.RowLength == 1 && matrix.ColumnLength == 3)
            {
                Vector3 result = new Vector3(matrix.Columns[0].Values[0],
                    matrix.Columns[0].Values[1], matrix.Columns[0].Values[2]);
                return result;
            }

            throw new Exception("matrix can't be casted");
        }

        public static implicit operator Vector2(FlexMatrix matrix)
        {
            if (matrix.RowLength == 1 && matrix.ColumnLength == 2)
            {
                Vector2 result = new Vector2(matrix.Columns[0].Values[0],
                    matrix.Columns[0].Values[1]);
                return result;
            }

            throw new Exception("matrix can't be casted");
        }

        // convert to matrix dans avec une fonction d'extension pour choisir si on souhaite exprimé en colonne ou en row
        // convert to vector et savoir check si matrice en ligne ou en colonne
        public static implicit operator FlexMatrix(float3 vector)
        {
            FlexMatrix result = new FlexMatrix(new[]
            {
                new FlexMatrixLine(new[] { vector.x }),
                new FlexMatrixLine(new[] { vector.y }), new FlexMatrixLine(new[] { vector.z })
            });
            return result;
        }

        public static implicit operator FlexMatrix(float2 vector)
        {
            FlexMatrix result = new FlexMatrix(new[]
            {
                new FlexMatrixLine(new[] { vector.x }),
                new FlexMatrixLine(new[] { vector.y })
            });
            return result;
        }

        public static implicit operator float3(FlexMatrix matrix)
        {
            if (matrix.RowLength == 1 && matrix.ColumnLength == 3)
            {
                float3 result = new float3(matrix.Columns[0].Values[0],
                    matrix.Columns[0].Values[1], matrix.Columns[0].Values[2]);
                return result;
            }

            throw new Exception("matrix can't be casted");
        }

        public static implicit operator float2(FlexMatrix matrix)
        {
            if (matrix.RowLength == 1 && matrix.ColumnLength == 2)
            {
                float2 result = new float2(matrix.Columns[0].Values[0],
                    matrix.Columns[0].Values[1]);
                return result;
            }

            throw new Exception("matrix can't be casted");
        }

        public static implicit operator FlexMatrix(float4 vector)
        {
            FlexMatrix result = new FlexMatrix(new[]
            {
                new FlexMatrixLine(new[] { vector.x }),
                new FlexMatrixLine(new[] { vector.y }), new FlexMatrixLine(new[] { vector.z }),
                new FlexMatrixLine(new[] { vector.w })
            });
            return result;
        }

        public static implicit operator float4(FlexMatrix matrix)
        {
            if (matrix.RowLength == 1 && matrix.ColumnLength == 4)
            {
                float4 result = new float4(matrix.Columns[0].Values[0],
                    matrix.Columns[0].Values[1], matrix.Columns[0].Values[2], matrix.Columns[0].Values[3]);
                return result;
            }

            throw new Exception("matrix can't be casted");
        }

        public static implicit operator FlexMatrix(Vector4 vector)
        {
            FlexMatrix result = new FlexMatrix(new[]
            {
                new FlexMatrixLine(new[] { vector.x }),
                new FlexMatrixLine(new[] { vector.y }), new FlexMatrixLine(new[] { vector.z }),
                new FlexMatrixLine(new[] { vector.w })
            });
            return result;
        }

        public static implicit operator Vector4(FlexMatrix matrix)
        {
            if (matrix.RowLength == 1 && matrix.ColumnLength == 4)
            {
                float4 result = new Vector4(matrix.Columns[0].Values[0],
                    matrix.Columns[0].Values[1], matrix.Columns[0].Values[2], matrix.Columns[0].Values[3]);
                return result;
            }

            throw new Exception("matrix can't be casted");
        }

        public int RowLength
        {
            get { return rowLength; }
        }

        public int ColumnLength
        {
            get { return columnLength; }
        }

        private int rowLength;
        private int columnLength;
        public FlexMatrixLine[] Rows;
        [HideInInspector] public FlexMatrixLine[] Columns;
    }
}
