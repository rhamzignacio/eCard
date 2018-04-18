
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eCard.Models;
using System.Data.Entity;

namespace eCard.Services
{
    public class ECardService
    {
        public static List<ApprovedReportModel> GetAllMotoReport(DateTime start, DateTime end, out string message)
        {
            try
            {
                message = "";

                using (var db = new eCardEntities())
                {
                    var qDB = new QuickipediaEntities();

                    var moto = db.v_MotoRequest.Where(r => r.ApprovedDate >= start && r.ApprovedDate <= end);

                    var motoClientCode = moto.Select(r => r.ClientCode);

                    var quicki = qDB.ClientProfile.Where(r => motoClientCode.Contains(r.ClientCode)).ToList();

                    var join = from a in moto
                               join c in quicki on a.ClientCode equals c.ClientCode
                               join u in db.UserAccount on a.RequestedBy equals u.ID
                               join usr in db.UserAccount on a.ApprovedBy equals usr.ID into qUsr
                               from ap in qUsr.DefaultIfEmpty()
                               orderby a.Date ascending
                               select new ApprovedReportModel
                               {
                                   ID = a.ID,
                                   Date = a.Date,
                                   FirstName = a.FirstName,
                                   LastName = a.LastName,
                                   ClientCode = a.ClientCode,
                                   Company = a.Company,
                                   PaxName = a.PaxName,
                                   RecordLocator = a.RecordLocator,
                                   Currency = a.Currency,
                                   Amount = a.Amount,
                                   Others = a.Others,
                                   BCDFee = a.BCDFee,
                                   AdminFee = a.AdminFee,
                                   Total = a.Total,
                                   OptionTime = a.OptionTime,
                                   ApprovalCode = a.ApprovalCode,
                                   Remarks = a.Remarks,
                                   ApprovedDate = a.ApprovedDate,
                                   ApprovedBy = ap.ID,
                                   ShowApprovedBy = ap.FirstName + " " + ap.LastName
                               };

                    return join.ToList();
                }
            }
            catch (Exception error)
            {
                message = error.Message;

                return null;
            }
        }

        public static List<ApprovedReportModel> GetAllApprovedReport(DateTime start, DateTime end, out string message)
        {
            try
            {
                message = "";

                using (var db = new eCardEntities())
                {
                    var qDB = new QuickipediaEntities();

                    var moto = db.vw_Approved.Where(r => r.ApprovedDate >= start && r.ApprovedDate <= end && r.Status == "A").ToList();

                    var motoClientCode = moto.Select(r => r.ClientCode);

                    var quicki = qDB.ClientProfile.Where(r => motoClientCode.Contains(r.ClientCode)).ToList();

                    var join = from a in moto
                               join c in quicki on a.ClientCode equals c.ClientCode
                               join u in db.UserAccount on a.RequestedBy equals u.ID
                               join usr in db.UserAccount   on a.ApprovedBy equals usr.ID into qUsr
                               from ap in qUsr.DefaultIfEmpty()
                               orderby a.Date ascending
                               select new ApprovedReportModel
                               {
                                   ID = a.ID,
                                   Date = a.Date,
                                   FirstName = a.FirstName,
                                   LastName = a.LastName,
                                   ClientName = c.ClientName,
                                   ClientCode = a.ClientCode,
                                   Company = a.Company,
                                   PaxName = a.PaxName,
                                   RecordLocator = a.RecordLocator,
                                   Currency = a.Currency,
                                   Amount = a.Amount,
                                   Others = a.Others,
                                   BCDFee = a.BCDFee,
                                   AdminFee = a.AdminFee,
                                   Total = a.Total,
                                   OptionTime = a.OptionTime,
                                   ApprovalCode = a.ApprovalCode,
                                   Remarks = a.Remarks,
                                   ApprovedDate = a.ApprovedDate,
                                   ApprovedBy =  ap.ID,
                                   ShowApprovedBy = ap.FirstName + " " + ap.LastName
                               };

                    return join.ToList();
                }
            }
            catch (Exception error)
            {
                message = error.Message;

                return null;
            }
        }

        public static List<MotoRequestModel> GetDuplicate(MotoRequestModel _model, out string message)
        {
            try
            {
                message = "";

                using (var db = new eCardEntities())
                {
                    var qDB = new QuickipediaEntities();

                    var moto = db.MotoRequest.Where(r => r.RecordLocator.ToLower() == _model.RecordLocator.ToLower()
                        && r.PaxName.ToLower() == _model.PaxName.ToLower()).ToList();

                    var motoClientCode = moto.Select(r => r.ClientCode);

                    var quicki = qDB.ClientProfile.Where(r => motoClientCode.Contains(r.ClientCode)).ToList();

                    var join = from m in moto
                               join c in quicki on m.ClientCode equals c.ClientCode
                               join u in db.UserAccount on m.RequestedBy equals u.ID
                               orderby m.Date ascending
                               select new MotoRequestModel
                               {
                                   ClientCode = m.ClientCode,
                                   ClientName = c.ClientName,
                                   AdminFee = m.AdminFee,
                                   Amount = m.Amount,
                                   ApprovalCode = m.ApprovalCode,
                                   BCDFee = m.BCDFee,
                                   Company = m.Company,
                                   Currency = m.Currency,
                                   Date = m.Date,
                                   ID = m.ID,
                                   Invoice = m.Invoice,
                                   OptionTime = m.OptionTime,
                                   Others = m.Others,
                                   PaxName = m.PaxName.ToUpper(),
                                   RecordLocator = m.RecordLocator,
                                   Remarks = m.Remarks,
                                   Status = m.Status,
                                   Total = m.Total,
                                   RequestedBy = m.RequestedBy,
                                   ShowRequestedBy = u.FirstName + " " + u.LastName,
                                   DeclinedReason = m.DeclinedReason,
                                   ApprovedDate = m.ApprovedDate
                               };

                    return join.ToList();
                }
            }
            catch(Exception error)
            {
                message = error.Message;

                return null;
            }
        }

        public static List<MotoRequestModel> GetAllMoto(string _status, out string message)
        {
            try
            {
                message = "";

                using (var db = new eCardEntities())
                {
                    using (var qDB = new QuickipediaEntities())
                    {
                        var date = DateTime.Now.AddDays(-7);

                        List<MotoRequest> moto;

                        if (_status == "P")
                        {
                            moto = db.MotoRequest.Where(r => (r.Status == "P" || r.Status == "F")).ToList();
                        }
                        else
                        {
                            moto = db.MotoRequest.Where(r => r.Status == _status 
                                && r.Date > date).ToList();
                        }


                        var motoClientCode = moto.Select(r => r.ClientCode);

                        var quicki = qDB.ClientProfile.Where(r => motoClientCode.Contains(r.ClientCode)).ToList();

                        var join = from m in moto
                                   join c in quicki on m.ClientCode equals c.ClientCode
                                   join u in db.UserAccount on m.RequestedBy equals u.ID
                                   orderby m.Date ascending
                                   select new MotoRequestModel
                                   {
                                       ClientCode = m.ClientCode,
                                       ClientName = c.ClientName,
                                       AdminFee = m.AdminFee,
                                       Amount = m.Amount,
                                       ApprovalCode = m.ApprovalCode,
                                       BCDFee = m.BCDFee,
                                       Company = m.Company,
                                       Currency = m.Currency,
                                       Date = m.Date,
                                       ID = m.ID,
                                       Invoice = m.Invoice,
                                       OptionTime = m.OptionTime,
                                       Others = m.Others,
                                       PaxName = m.PaxName.ToUpper(),
                                       RecordLocator = m.RecordLocator,
                                       Remarks = m.Remarks,
                                       Status = m.Status,
                                       Total = m.Total,
                                       RequestedBy = m.RequestedBy,
                                       ShowRequestedBy = u.FirstName + " " + u.LastName,
                                       DeclinedReason = m.DeclinedReason,
                                       ApprovedDate = m.ApprovedDate
                                   };

                        return join.ToList();
                    }
                }
            }
            catch(Exception error)
            {
                message = error.Message;

                return null;
            }
        }

        public static List<MotoRequestModel> GetAllMotoPerUser(string _status, out string message)
        {
            try
            {
                message = "";

                using (var db = new eCardEntities())
                {
                    var qDB = new QuickipediaEntities();

                    var date = DateTime.Now.AddDays(-7);

                    List<MotoRequest> moto;

                    if (_status == "P")
                    {
                        moto = db.MotoRequest.Where(r => (r.Status == "P" || r.Status == "F") && r.RequestedBy == UniversalService.CurrentUser.ID).ToList();
                    }
                    else
                    {
                        moto = db.MotoRequest.Where(r => r.Status == _status && r.RequestedBy == UniversalService.CurrentUser.ID
                            && r.Date > date).ToList();
                    }

                    var motoClientCode = moto.Select(r => r.ClientCode);

                    var quicki = qDB.ClientProfile.Where(r => motoClientCode.Contains(r.ClientCode)).ToList();

                    var join = from m in moto
                               join c in quicki on m.ClientCode equals c.ClientCode
                               join u in db.UserAccount on m.RequestedBy equals u.ID
                               orderby m.Date ascending
                               select new MotoRequestModel
                               {
                                   ClientCode = m.ClientCode,
                                   ClientName = c.ClientName,
                                   AdminFee = m.AdminFee,
                                   Amount = m.Amount,
                                   ApprovalCode = m.ApprovalCode,
                                   BCDFee = m.BCDFee,
                                   Company = m.Company,
                                   Currency = m.Currency,
                                   Date = m.Date,
                                   ID = m.ID,
                                   Invoice = m.Invoice,
                                   OptionTime = m.OptionTime,
                                   Others = m.Others,
                                   PaxName = m.PaxName.ToUpper(),
                                   RecordLocator = m.RecordLocator,
                                   Remarks = m.Remarks,
                                   Status = m.Status,
                                   Total = m.Total,
                                   RequestedBy = m.RequestedBy,
                                   ShowRequestedBy = u.FirstName + " " + u.LastName,
                                   DeclinedReason = m.DeclinedReason,
                                   ApprovedDate = m.ApprovedDate
                               };

                    return join.ToList();
                }
            }
            catch (Exception error)
            {
                message = error.Message;

                return null;
            }
        }
          

        public static void SaveMotoRequest(MotoRequestModel _request, out string message)
        {
            try
            {
                message = "";

                using (var db = new eCardEntities())
                {
                    if(_request.ID == Guid.Empty) //NEW
                    {
                        MotoRequest newMoto = new MotoRequest
                        {
                            ID = Guid.NewGuid(),
                            Date = DateTime.Now,
                            RequestedBy = UniversalService.CurrentUser.ID,
                            ClientCode = _request.ClientCode,
                            Company = _request.Company,
                            PaxName = _request.PaxName.ToUpper(),
                            RecordLocator = _request.RecordLocator.ToUpper(),
                            Currency = _request.Currency,
                            Amount = _request.Amount,
                            Others = _request.Others,
                            BCDFee = _request.BCDFee,
                            AdminFee = _request.AdminFee,
                            Total = _request.Amount + _request.BCDFee + _request.Others + _request.AdminFee,
                            OptionTime = _request.OptionTime,
                            ApprovalCode = _request.ApprovalCode,
                            Remarks = _request.Remarks,
                            Invoice = _request.Invoice,
                            Status = _request.Status,
                            DeclinedReason = _request.DeclinedReason
                        };

                        db.Entry(newMoto).State = EntityState.Added;

                        db.SaveChanges();
                    }
                    else
                    {
                        var moto = db.MotoRequest.FirstOrDefault(r => r.ID == _request.ID);

                        if(moto != null)
                        {
                            moto.ClientCode = _request.ClientCode;

                            moto.Company = _request.Company;

                            moto.PaxName = _request.PaxName.ToUpper();

                            moto.RecordLocator = _request.RecordLocator.ToUpper();

                            moto.Currency = _request.Currency;

                            moto.AdminFee = _request.AdminFee;

                            moto.Total = _request.Amount + _request.Others + _request.BCDFee + _request.AdminFee;

                            moto.OptionTime = _request.OptionTime;

                            moto.ApprovalCode = _request.ApprovalCode;

                            moto.Remarks = _request.Remarks;

                            moto.Invoice = _request.Invoice;

                            moto.Status = _request.Status;

                            moto.DeclinedReason = _request.DeclinedReason;

                            if (_request.Status == "A")
                            {
                                moto.ApprovedDate = DateTime.Now;

                                moto.ApprovedBy = UniversalService.CurrentUser.ID;

                            }

                            db.Entry(moto).State = EntityState.Modified;

                            db.SaveChanges();

                        }
                    }
                }
            }
            catch(Exception error)
            {
                message = error.Message;
            }
        }
    }
}