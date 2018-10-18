namespace DotNetContrib.AspNetCore.Initialization
{
    public class InitializationContext
    {
        public InitializationContext()
        {
            Status = new InitializationStatus
            {
                Message = "Loading..."
            };
        }
        
        public InitializationStatus Status { get; }
    }
}