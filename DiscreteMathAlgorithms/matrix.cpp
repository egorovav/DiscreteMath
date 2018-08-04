#include "stdafx.h"
#include "matrix.h"

double get_element(double* matrix, size_t matrix_size, int i, int j) {
	return matrix[i * matrix_size + j];
}

double get_determinant_by_gauss(double* matrix, size_t matrix_size) {
	double _determinant = 1;

	for (int i = 0; i < matrix_size; ++i) {

		double _diag_element = get_element(matrix, matrix_size, i, i);
		if (_diag_element == 0)
		{
			return 0;
			//continue;
		}
		_determinant *= _diag_element;

		for (int k = i + 1; k < matrix_size; ++k) {

			double _under_diag = get_element(matrix, matrix_size, k, i);
			double _mult = _under_diag / _diag_element;

			for (int j = 0; j < matrix_size; ++j) {

				double _first_row_element = get_element(matrix, matrix_size, i, j);
				double _second_row_element = get_element(matrix, matrix_size, k, j);
				matrix[k * matrix_size + j] = _second_row_element - _first_row_element * _mult;
			}
		}

		//print_matrix(matrix, matrix_size);
	}

	return _determinant;
}

double* get_minor(double* matrix, size_t matrix_size, int row_index, int col_index) {

	size_t _minor_length = (matrix_size - 1) * (matrix_size - 1);
	double* _minor = (double*)calloc(_minor_length, sizeof(double));
	int _minor_index = 0;
	for (int i = 0; i < matrix_size; ++i) {
		for (int j = 0; j < matrix_size; ++j) {
			if (i == row_index) {
				continue;
			}

			if (j == col_index) {
				continue;
			}

			_minor[_minor_index] = get_element(matrix, matrix_size, i, j);
			_minor_index++;
		}
	}

	return _minor;
}

double get_determinant_by_rec(double* matrix, size_t matrix_size) {

	if (matrix_size == 1) {
		return matrix[0];
	}

	double _determinant = 0;

	for (int j = 0; j < matrix_size; ++j) {
		double* _minor = get_minor(matrix, matrix_size, 0, j);
		double _minor_det = get_determinant_by_rec(_minor, matrix_size - 1);
		double _evenly = 1 - ((j % 2) * 2);
		_determinant += _minor_det * _evenly * get_element(matrix, matrix_size, 0, j);
	}

	return _determinant;
}