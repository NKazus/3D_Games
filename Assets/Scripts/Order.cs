public struct Order
{
    private int misses;

    public int tools;
    public int food;
    public int fuel;
    public int health;

    public override bool Equals(object obj)
    {
        misses = 0;
        if(tools != ((Order)obj).tools)
        {
            misses++;
        }
        if (food != ((Order)obj).food)
        {
            misses++;
        }
        if (fuel != ((Order)obj).fuel)
        {
            misses++;
        }
        if (health != ((Order)obj).health)
        {
            misses++;
        }
        return misses == 0;
    }

    public int GetMisses()
    {
        return misses;
    }
}
