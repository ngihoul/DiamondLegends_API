
namespace DiamondLegends.BLL.Services
{
    [Serializable]
    public class GameToBePlayedException : Exception
    {
        public GameToBePlayedException()
        {
        }

        public GameToBePlayedException(string? message) : base(message)
        {
        }

        public GameToBePlayedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}