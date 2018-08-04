#include "stdafx.h"
#include "polynom.h"
#include "gcd.h"


int* sum_of_polynom(int* a, int deg_a, int* b, int deg_b, int q, int* sum, int* deg_sum) {
	int max_deg = deg_a > deg_b ? deg_a : deg_b;
	int ai = 0;
	int bi = 0;
	*deg_sum = -1;
	for (int i = 0; i <= max_deg; ++i) {
		if (i <= deg_a)
			ai = a[i];
		if (i <= deg_b)
			bi = b[i];
		sum[i] = (ai + bi) % q;
		if (sum[i] != 0)
			*deg_sum = i;
		ai = 0;
		bi = 0;
	}
	return sum;
}

int* prod_of_polynom(int* a, int deg_a, int* b, int deg_b, int q, int* prod) {
	int deg_prod = deg_a + deg_b;

	for (int i = 0; i <= deg_prod; ++i) {
		int pi = 0;
		for (int j = 0; j <= i; j++) {
			int aj = j <= deg_a ? a[j] : 0;
			int bi_j = i - j <= deg_b ? b[i - j] : 0;
			pi += aj * bi_j;
		}

		prod[i] = pi % q;
	}
	return prod;
}

void mult_polynom(int* a, int deg_a, int m, int q) {
	for (int i = 0; i <= deg_a; ++i) {
		a[i] = (a[i] * m) % q;
	}
}

int* div_polynom(int* num, int deg_num, int* denum, int deg_denum, int q, int* quot, int* rem, int* deg_rem) {

	if (deg_num < deg_denum) {
		rem = num;
		*deg_rem = deg_num;
		return 0;
	}

	int deg_quot = deg_num - deg_denum;
	for (int i = 0; i <= deg_quot; ++i)
		quot[i] = 0;

	int deg_r = deg_num;
	memcpy(rem, num, (deg_num + 1) * sizeof(int));

	int temp = 0;
	while (deg_r >= deg_denum) {
		int c = reverse(denum[deg_denum], q);
		int p = deg_r - deg_denum;
		int* d = (int*)calloc(p + 1, sizeof(int));
		d[p] = (rem[deg_r] * c) % q;
		for (int i = 0; i < p; ++i)
			d[i] = 0;
		//write_polynom(d, p);
		sum_of_polynom(d, p, quot, deg_quot, q, quot, &temp);
		int* dd = (int*)calloc(deg_num + 1, sizeof(int));
		prod_of_polynom(denum, deg_denum, d, p, q, dd);
		free(d);
		mult_polynom(dd, deg_r, q - 1, q);
		//write_polynom(dd, deg_num);
		sum_of_polynom(rem, deg_r, dd, deg_r, q, rem, &temp);
		deg_r = temp;
		free(dd);
	}

	*deg_rem = deg_r;
	return quot;
}