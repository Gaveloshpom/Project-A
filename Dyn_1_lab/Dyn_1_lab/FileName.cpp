#include <iostream>
#include <chrono>
#include <vector>
#include<windows.h>

using namespace std;
using namespace std::chrono;

unsigned long long fibonacciRecursive(int n) {
    if (n <= 1) {
        return 1;
    }
    return fibonacciRecursive(n - 1) + fibonacciRecursive(n - 2);
}

unsigned long long fibonacciMemoization(int n, vector<unsigned long long>& A) {
    if (A[n] != -1) {
        return A[n];
    }

    if (n < 2) {
        return 1;
    }
    else {
        A[n] = fibonacciMemoization(n - 1, A) + fibonacciMemoization(n - 2, A);
        return A[n];
    }
}

unsigned long long fibonacciIterative(int n) {
    if (n <= 1) {
        return 1;
    }
    vector<unsigned long long> F(n + 1);
    F[0] = 1;
    F[1] = 1;
    for (int i = 2; i <= n; ++i) {
        F[i] = F[i - 1] + F[i - 2];
    }
    return F[n];
}

int main() {
    SetConsoleOutputCP(1251);
    SetConsoleCP(1251);
    int N;
    cout << "¬вед≥ть число N: ";
    cin >> N;

    // 1 cпос≥б
    auto start = high_resolution_clock::now();
    unsigned long long fibRec = fibonacciRecursive(N);
    auto stop = high_resolution_clock::now();
    auto durationRec = duration_cast<milliseconds>(stop - start);
    cout << "–екурсивний метод: " << fibRec << ", „ас: " << durationRec.count() << endl;

    // 2 cпос≥б
    vector<unsigned long long> A(N + 1, -1);
    start = high_resolution_clock::now();
    unsigned long long fibMemo = fibonacciMemoization(N, A);
    stop = high_resolution_clock::now();
    auto durationMemo = duration_cast<milliseconds>(stop - start);
    cout << "ћетод з меморизац≥Їю: " << fibMemo << ", „ас: " << durationMemo.count() << endl;

    // 3 cпос≥б
    start = high_resolution_clock::now();
    unsigned long long fibIter = fibonacciIterative(N);
    stop = high_resolution_clock::now();
    auto durationIter = duration_cast<milliseconds>(stop - start);
    cout << "÷икл≥чний метод: " << fibIter << ", „ас: " << durationIter.count() << endl;

    return 0;
}
