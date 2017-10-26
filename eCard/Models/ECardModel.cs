using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCard.Models
{
    public class ECardModel
    {
    }

    public class MotoRequestModel
    {
        public Guid ID { get; set; }
        public DateTime? Date { get; set; }
        public string ShowDate
        {
            get
            {
                if (Date != null)
                    return DateTime.Parse(Date.ToString()).ToShortDateString();
                else
                    return "";
            }
        }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public string Company { get; set; }
        public string PaxName { get; set; }
        public string RecordLocator { get; set; }
        public string Currency { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Others { get; set; }
        public decimal? BCDFee { get; set; }
        public decimal? AdminFee { get; set; }
        public decimal? Total { get; set; }
        public string OptionTime { get; set; }
        public string ApprovalCode { get; set; }
        public string Remarks { get; set; }
        public string Invoice { get; set; }
        public string Status { get; set; }

        public Guid? RequestedBy { get; set; }
        public string ShowRequestedBy { get; set; }
    }
}