
public class ResurrectionSource
{
    private int maxAmount = 0;
    private int spent = 0;

    public int getRemaining() => maxAmount - spent;
    public void setMax(int value) => maxAmount = value;

    public bool spend()
    {
        if (getRemaining() <= 0) return false;
        spent++;
        return true;
    }

    public void reset()
    {
        maxAmount = 0;
        spent = 0;
    }
    
}
