#pragma once

extern "C" __declspec(dllexport) int get_gcd(int a, int b);
int get_gcd_uv(int a, int b, int* u, int* v);
int* get_gcd_p(int* a, int deg_a, int* b, int deg_b, int q, int* gcd, int* deg_gcd);
int reverse(int a, int q);
