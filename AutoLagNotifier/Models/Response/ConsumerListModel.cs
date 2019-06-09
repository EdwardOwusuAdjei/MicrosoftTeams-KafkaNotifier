using System.Collections.Generic;


namespace AutoLagNotifier.Models.Response
{
    public class ConsumerListModel
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Consumers { get; set; }
    }
}