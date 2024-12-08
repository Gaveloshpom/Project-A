#include <iostream>
#include <vector>
#include <chrono>
#include<windows.h>

using namespace std;
using namespace std::chrono;

long long countSequences(int n) {
    if (n == 1) return 2;
    if (n == 2) return 3;

    vector<long long> dp(n + 1);
    dp[1] = 2;
    dp[2] = 3;

    for (int i = 3; i <= n; ++i) {
        dp[i] = dp[i - 1] + dp[i - 2];
    }

    return dp[n];
}

int main() {
    SetConsoleOutputCP(1251);
    SetConsoleCP(1251);
    int n;
    cout << "Введіть значення n: ";
    cin >> n;

    auto start = high_resolution_clock::now();

    long long result = countSequences(n);

    auto end = high_resolution_clock::now();
    auto duration = duration_cast<microseconds>(end - start);

    cout << "Кількість послідовностей довжини " << n << ": " << result << endl;
    cout << "Час виконання: " << duration.count() << " мікросекунд" << endl;

    return 0;
}

