namespace ClearBank.DeveloperTest.Domain
{
    public interface IRule
    {
        string Message { get; }
        bool IsBroken();
    }
}
