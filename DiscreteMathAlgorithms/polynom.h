#pragma once
int* sum_of_polynom(int* a, int deg_a, int* b, int deg_b, int q, int* sum, int* deg_sum);
int* prod_of_polynom(int* a, int deg_a, int* b, int deg_b, int q, int* prod);
void mult_polynom(int* a, int deg_a, int m, int q);
int* div_polynom(int* num, int deg_num, int* denum, int deg_denum, int q, int* quot, int* rem, int* deg_rem);
void write_polynom(int* a, int deg_a);
