namespace AspNetIdentity.WebApi.Models
{
    using System;

    public class DateViewModel
    {
        public DateTime Date { get; set; }

        public int Epoch { get; set; }

        public string DateString { get; set; }

        public DateTime DateTimeFromEpoch
        {
            get
            {
                return Extensions.FromEpocToDateTime(this.Epoch);
            }
        }

        public DateTime DateTimeFromString
        {
            get
            {
                if (DateTime.TryParse(this.DateString, out var result))
                {
                    return result;
                }
                return DateTime.MinValue;
            }
        }
    }
}