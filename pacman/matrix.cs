using System;
using System.Collections.Generic;
using System.Text;

namespace pacman {
    class matrix {
        public float[,] data;
        public int columns, rows;

        public matrix(int columns, int rows) {
            this.columns = columns;
            this.rows = rows;
            this.data = new float[columns, rows];
        }

        public void randomize(Random rand) {

            for (int i = 0; i < this.columns; i++) {
                for (int j = 0; j < this.rows; j++) {
                    this.data[i, j] = rand.Next(10);
                }
            }
        }

        public void print() {
            for (int i = 0; i < this.rows; i++) {
                for (int j = 0; j < this.columns; j++) {
                    Console.Write(this.data[j, i] + " ");
                }
                Console.WriteLine("");

            }
            Console.WriteLine("");
        }

        public void transpose() {
            matrix result = new matrix(this.rows, this.columns);
            for (int i = 0; i < this.columns; i++) {
                for (int j = 0; j < this.rows; j++) {
                    result.data[j, i] = this.data[i, j];
                }
            }
            this.data = result.data;
        }

        public matrix add(float term) {
            matrix result = new matrix(this.columns, this.rows);
            for (int i = 0; i < result.columns; i++) {
                for (int j = 0; j < result.rows; j++) {
                    result.data[i, j] = this.data[i, j] + term;
                }
            }
            return result;
        }

        public matrix add(matrix m) {
            if (m.data.GetLength(0) == this.columns && m.rows == this.rows) {
                matrix result = new matrix(this.columns, this.rows);
                for (int i = 0; i < m.columns; i++) {
                    for (int j = 0; j < m.rows; j++) {
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
            matrix result = new matrix(this.columns, this.rows);
            for (int i = 0; i < result.columns; i++) {
                for (int j = 0; j < result.rows; j++) {
                    result.data[i, j] = this.data[i, j] - term;
                }
            }
            return result;
        }

        public matrix mult(float term) {
            matrix result = new matrix(this.columns, this.rows);
            for (int i = 0; i < result.columns; i++) {
                for (int j = 0; j < result.rows; j++) {
                    result.data[i, j] = this.data[i, j] * term;
                }
            }
            return result;
        }

        public matrix mult(matrix m) {
            if (m.data.GetLength(0) == this.columns && m.rows == this.rows) {
                matrix result = new matrix(this.columns, this.rows);
                for (int i = 0; i < m.columns; i++) {
                    for (int j = 0; j < m.rows; j++) {
                        result.data[i, j] = this.data[i, j] * m.data[i, j];
                    }
                }
                return result;
            } else if (this.rows == m.columns && this.columns == m.rows) {
                matrix result = new matrix(this.rows, m.columns);
                for (int i = 0; i < this.rows; i++) {
                    for (int j = 0; j < m.columns; j++) {
                        float temp = 0;
                        for (int k = 0; k < this.columns; k++) {
                            temp += this.data[k, j] * m.data[i, k];
                        }
                        result.data[i, j] = temp;
                    }
                }
                return result;
            } else {
                Console.WriteLine("WRONG DIMENSIONS!");
                return this;
            }
        }

        public matrix div(float term) {
            matrix result = new matrix(this.columns, this.rows);
            for (int i = 0; i < result.columns; i++) {
                for (int j = 0; j < result.rows; j++) {
                    result.data[i, j] = this.data[i, j] / term;
                }
            }
            return result;
        }
    }
}
