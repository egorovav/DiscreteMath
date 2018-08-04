#pragma once

//#define _CRT_SECURE_NO_WARNINGS
#include "stdio.h"
#include "stdlib.h"
#include "math.h"

double get_determinant_by_gauss(double*, size_t);
double get_determinant_by_rec(double*, size_t);
double get_element(double*, size_t, int, int);
double* get_minor(double*, size_t, int, int);
