using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCard.Models
{
    public class ECardModel
    {
    }

    public class ApprovedReportModel
    {
        public Guid ID { get; set; }
        public DateTime? Date { get; set; }
        public decimal LogID { get; set; }
        public string ShowLogID
        {
            get
            {
                string ret = LogID.ToString();

                while (LogID.ToString().Length <= 5)
                {
                    ret = "0" + ret;
                }

                return ret;
            }
        }

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

        public string FirstName { get; set; }
        public string LastName { get; set; }
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
        public DateTime? ApprovedDate { get; set; }
        public string ShowApprovedDate
        {
            get
            {
                if (ApprovedDate != null)
                    return DateTime.Parse(ApprovedDate.ToString()).ToShortDateString();
                else
                    return "";
            }
        }
        public Guid? ApprovedBy { get; set; }
        public string ShowApprovedBy { get; set; }
    }

    public class MotoRequestModel
    {
        public Guid ID { get; set; }
        public DateTime Date { get; set; }
        public decimal LogID { get; set; }
        public string ShowLogID
        {
            get
            {
                string ret = LogID.ToString();

                while (ret.Length <= 5)
                {
                    ret = "0" + ret;
                }

                return ret;
            }
        }
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
        public string ShowStatus
        {
            get
            {
                if (Status == "P")
                    return "Pending";
                else if (Status == "X")
                    return "Canceled";
                else if (Status == "A")
                    return "Approved";
                else if (Status == "D")
                    return "Declined";
                else if (Status == "F")
                    return "For Void";
                else if (Status == "V")
                    return "Voided";
                else
                    return "";

            }
        }
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

        public Guid? ApprovedBy { get; set; }
        public string ShowApprovedBy { get; set; }
    }
}