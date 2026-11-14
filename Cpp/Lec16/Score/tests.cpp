import score;
int main()
{
    return limit_score(-1) == 0 && limit_score(0) == 0
        && limit_score(50) == 50 && limit_score(100) == 100
        && limit_score(101) == 100 ? 0 : 1;
}
