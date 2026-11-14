export module score;
export int limit_score(int value)
{
    if (value < 0) return 0;
    if (value > 100) return 100;
    return value;
}
