using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace pacman {
    class matrix {
        public float[,] data;
        public int columns, rows;

        public matrix(int rows, int columns) {
            this.columns = columns;
            this.rows = rows;
            this.data = new float[rows, columns];
        }

        public void randomize(Random rand) {

            for (int i = 0; i < this.rows; i++) {
                for (int j = 0; j < this.columns; j++) {
                    this.data[i, j] = rand.Next(10);
                }
            }
        }

        public void print() {
            for (int i = 0; i < this.rows; i++) {
                for (int j = 0; j < this.columns; j++) {
                    Debug.Write(this.data[i, j] + " ");
                }
                Debug.WriteLine("");

            }
            Debug.WriteLine("");
        }

        public void transpose() {
            matrix result = new matrix(this.columns, this.rows);
            for (int i = 0; i < this.rows; i++) {
                for (int j = 0; j < this.columns; j++) {
                    result.data[j, i] = this.data[i, j];
                }
            }
            this.data = result.data;
        }

        public matrix add(float term) {
            matrix result = new matrix(this.rows, this.columns);
            for (int i = 0; i < result.rows; i++) {
                for (int j = 0; j < result.columns; j++) {
                    result.data[i, j] = this.data[i, j] + term;
                }
            }
            return result;
        }

        public matrix add(matrix m) {
            if (m.rows == this.rows && m.columns == this.columns) {
                matrix result = new matrix(this.rows, this.columns);
                for (int i = 0; i < m.rows; i++) {
                    for (int j = 0; j < m.columns; j++) {
                        result.data[i, j] = this.data[i, j] + m.data[i, j];
                    }
                }
                return result;
            } else {
                Console.WriteLine("WRONG DIMENSIONS!");
                return this;
            }
        }

        public matrix sub(float term) {
            matrix result = new matrix(this.rows, this.columns);
            for (int i = 0; i < result.rows; i++) {
                for (int j = 0; j < result.columns; j++) {
                    result.data[i, j] = this.data[i, j] - term;
                }
            }
            return result;
        }

        public matrix mult(float term) {
            matrix result = new matrix(this.rows, this.columns);
            for (int i = 0; i < result.rows; i++) {
                for (int j = 0; j < result.columns; j++) {
                    result.data[i, j] = this.data[i, j] * term;
                }
            }
            return result;
        }

        public matrix mult(matrix m) {
            if (m.rows == this.rows && m.columns == this.columns) {
                matrix result = new matrix(this.rows, this.columns);
                for (int i = 0; i < m.rows; i++) {
                    for (int j = 0; j < m.columns; j++) {
                        result.data[i, j] = this.data[i, j] * m.data[i, j];
                    }
                }
                return result;
            } else if (this.columns == m.rows) {
                matrix result = new matrix(this.rows, m.columns);
                result.print();
                for (int i = 0; i < this.rows; i++) {
                    for (int j = 0; j < m.columns; j++) {
                        float temp = 0;
                        for (int k = 0; k < this.columns; k++) {
                            temp += this.data[i, k] * m.data[k, j];
                        }
                        result.data[i, j] = temp;
                    }
                }
                return result;
            } else {
                Debug.WriteLine("WRONG DIMENSIONS!");
                return this;
            }
        }

        public matrix div(float term) {
            matrix result = new matrix(this.rows, this.columns);
            for (int i = 0; i < result.rows; i++) {
                for (int j = 0; j < result.columns; j++) {
                    result.data[i, j] = this.data[i, j] / term;
                }
            }
            return result;
        }
    }
}
