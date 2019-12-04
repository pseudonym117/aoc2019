#include <chrono>
#include <cstdio>
#include <fstream>

void correct_pwd_min(char*);
void correct_pwd_max(char*);
bool part_1_is_valid_pwd(const char* pwd);
bool part_2_is_valid_pwd(const char* pwd);

int main()
{
    auto start = std::chrono::high_resolution_clock::now();

    char min[7]{};
    char max[7]{};
    
    {
        std::ifstream in("input.txt", std::ios::binary);
        if (!in)
        {
            printf("could not find input.txt");
            return -1;
        }
        in.read(min, 6);
        in.seekg(7);
        in.read(max, 6);
    }

    auto file_read = std::chrono::high_resolution_clock::now();

    printf("min: %s, max: %s\n", min, max);

    correct_pwd_min(min);
    correct_pwd_max(max);

    auto part_1_count = 0;
    auto part_2_count = 0;
    for (; min[0] <= max[0]; ++min[0])
    {
        for (; min[1] <= '9'; ++min[1])
        {
            if (min[0] == max[0] && min[1] > max[1]) break;

            for (; min[2] <= '9'; ++min[2])
            {
                if (min[0] == max[0] && min[1] == max[1] && min[2] > max[2]) break;

                for (; min[3] <= '9'; ++min[3])
                {
                    if (min[0] == max[0] && min[1] == max[1] && min[2] == max[2] && min[3] > max[3]) break;

                    for (; min[4] <= '9'; ++min[4])
                    {
                        if (min[0] == max[0] && min[1] == max[1] && min[2] == max[2] && min[3] == max[3] && min[4] > max[4]) break;

                        for (; min[5] <= '9'; ++min[5])
                        {
                            if (min[0] == max[0] && min[1] == max[1] && min[2] == max[2] && min[3] == max[3] && min[4] == max[4] && min[5] > max[5]) break;

                            if (part_1_is_valid_pwd(min))
                            {
                                ++part_1_count;
                            }

                            if (part_2_is_valid_pwd(min))
                            {
                                ++part_2_count;
                            }
                        }

                        min[5] = min[4];
                    }

                    min[5] = min[4] = min[3];
                }

                min[5] = min[4] = min[3] = min[2];
            }

            min[5] = min[4] = min[3] = min[2] = min[1];
        }

        min[5] = min[4] = min[3] = min[2] = min[1] = min[0];
    }

    auto finished = std::chrono::high_resolution_clock::now();

    printf("part 1: %d valid passwords\n", part_1_count);
    printf("part 2: %d valid passwords\n", part_2_count);

    auto total_time = finished - start;
    auto file_read_time = file_read - start;
    auto algo_time = finished - file_read;

    printf(
        "\nPerformance:\n Overall: %f ms\n    File: %f ms\n    Algo: %f ms\n",
        total_time.count() / 1000. / 1000,
        file_read_time.count() / 1000. / 1000,
        algo_time.count() / 1000. / 1000
    );
}

void correct_pwd_min(char* pwd)
{
    for (auto i = 1; i < 6; ++i)
    {
        if (pwd[i] < pwd[i - 1])
        {
            pwd[i] = pwd[i - 1];
        }
    }
}

void correct_pwd_max(char* pwd)
{
    auto from_index = 1;
    for (; from_index < 6; ++from_index)
    {
        if (pwd[from_index] < pwd[from_index - 1])
        {
            break;
        }
    }

    if (from_index < 6)
    {
        --pwd[from_index - 1];
        for (auto i = from_index; i < 6; ++i)
        {
            pwd[i] = '9';
        }
    }
}

bool part_1_is_valid_pwd(const char* pwd)
{
    // length assumed to be 6, because reasons
    bool double_char = false;

    for (auto i = 1; i < 6; ++i)
    {
        if (pwd[i - 1] > pwd[i])
        {
            return false;
        }

        if (pwd[i - 1] == pwd[i])
        {
            double_char = true;
        }
    }

    return double_char;
}

bool part_2_is_valid_pwd(const char* pwd)
{
    // length assumed to be 6, because reasons
    char counts[10]{};

    counts[pwd[0] - '0'] = 1;
    for (auto i = 1; i < 6; ++i)
    {
        if (pwd[i - 1] > pwd[i])
        {
            return false;
        }

        ++counts[pwd[i] - '0'];
    }

    for (auto i = 0; i < sizeof(counts); ++i)
    {
        if (counts[i] == 2)
        {
            return true;
        }
    }

    return false;
}


