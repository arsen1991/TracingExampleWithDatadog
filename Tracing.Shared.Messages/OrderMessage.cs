
namespace Tracing.Shared.Messages
{
    public class OrderMessage
    {
        public static string EndPointAddress = "order";

        public  int OrderId { get; set; }

        public int OrderCount { get; set; }

        public override string ToString()
        {
            return $"Order => {OrderId} , {OrderCount}";
        }
    }
}