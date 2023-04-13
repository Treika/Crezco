using System.ComponentModel.DataAnnotations;

namespace Service
{
    public class QueryParams
    {
        [RegularExpression("^(?:[0-9]{1,3}\\.){3}[0-9]{1,3}$")]
        public string IpAddress { get; set; }
    }
}
