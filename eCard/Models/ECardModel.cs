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
        public DateTime Date { get; set; }
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

        private decimal? _amount;
        public decimal? Amount
        {
            get
            {
                if (_amount != null)
                    return _amount;
                else
                    return 0;
            }
            set
            {
                _amount = value;
            }
        }

        private decimal? _others;
        public decimal? Others
        {
            get
            {
                if (_others != null)
                    return _others;
                else
                    return 0;
            }
            set
            {
                _others = value;
            }
        }

        private decimal? _bcdFee { get; set; }
        public decimal? BCDFee
        {
            get
            {
                if (_bcdFee != null)
                    return _bcdFee;
                else
                    return 0;
            }
            set
            {
                _bcdFee = value;
            }
        }

        private decimal? _adminFee;
        public decimal? AdminFee
        {
            get
            {
                if (_adminFee != null)
                    return _adminFee;
                else
                    return 0;
            }
            set
            {
                _adminFee = value;
            }
        }

        public decimal? Total { get; set; }
        public string OptionTime { get; set; }
        public string ApprovalCode { get; set; }
        public string Remarks { get; set; }
        public string Invoice { get; set; }
        public string Status { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string CanVoid
        {
            get
            {
                if (ApprovedDate != null)
                {
                    if (DateTime.Now <= DateTime.Parse(ApprovedDate.ToString()).Date.AddDays(1).AddHours(9))
                    {
                        return "Y";
                    }
                    else
                    {
                        return "N";
                    }
                }
                else
                    return "N";
            }
        }
        public string DeclinedReason { get; set; }

        public Guid? RequestedBy { get; set; }
        public string ShowRequestedBy { get; set; }
    }
}