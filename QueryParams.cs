using System.ComponentModel.DataAnnotations;

namespace Service
{
    public class QueryParams
    {
        [RegularExpression("^((25[0-5]|(2[0-4]|1\\d|[1-9]|)\\d)\\.?\\b){4}$")]
        public string IpAddress { get; set; }
    }
}
