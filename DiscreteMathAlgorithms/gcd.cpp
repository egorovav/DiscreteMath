#include "stdafx.h"
#include "gcd.h"
#include "polynom.h"


extern "C" __declspec(dllexport) int get_gcd(int a, int b) {
	a = abs(a);
	b = abs(b);
	int r1 = a > b ? a : b;
	int r2 = a + b - r1;
	int temp = 0;

	while (r2 != 0)
	{
	    temp = r2;
	    r2 = r1 % r2;
	    r1 = temp;
	}

	return r1;
}

int get_gcd_uv(int a, int b, int* u, int* v) {
	int r1 = b;
	int r = a % b;
	int q = a / b;
	int u0 = 1;
	int u1 = 0;
	int v0 = -q;
	int v1 = 1;
	while (r != 0) {
		q = r1 / r;
		int temp = r;
		r = r1 % r;
		r1 = temp;
		temp = u0;
		u0 = u1 - u0 * q;
		u1 = temp;
		temp = v0;
		v0 = v1 - v0 * q;
		v1 = temp;
	}

	*u = u1;
	*v = v1;
	return r1;
}

int* get_gcd_p(int* a, int deg_a, int* b, int deg_b, int q, int* gcd, int* deg_gcd) {

	int deg_r = deg_b;
	int* r = (int*)calloc(deg_b + 1, sizeof(int));
	memcpy(r, b, (deg_b + 1) * sizeof(int));

	int deg_r1 = deg_a;
	memcpy(gcd, a, (deg_a + 1) * sizeof(int));

	int* quot = (int*)calloc(deg_a + 1, sizeof(int));
	int deg_rem = deg_a;
	int* rem = (int*)calloc(deg_a + 1, sizeof(int));
	//int* temp = 0;

	while (deg_r >= 0) {
		div_polynom(gcd, deg_r1, r, deg_r, q, quot, rem, &deg_rem);
		deg_r1 = deg_r;
		memcpy(gcd, r, (deg_r + 1) * sizeof(int));
		//temp = gcd;
		//gcd = r;
		deg_r = deg_rem;
		memcpy(r, rem, (deg_r + 1) * sizeof(int));
		//r = rem;
		//rem = temp;
	}

	free(r);
	free(rem);
	free(quot);

	*deg_gcd = deg_r1;
	return gcd;
}

int reverse(int a, int q) {
	if (a >= q) {
		return 0;
	}

	int u, v;
	int* pu = &u;
	int* pv = &v;
	int gcd = get_gcd_uv(a, q, pu, pv);
	int r = *pu > 0 ? *pu : q + *pu;
	return r;
}