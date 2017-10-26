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
        public static List<MotoRequestModel> GetAllMoto(string _status, out string message)
        {
            try
            {
                message = "";

                using (var db = new eCardEntities())
                {
                    using (var qDB = new QuickipediaEntities())
                    {
                        var moto = db.MotoRequest.Where(r => r.Status == _status).ToList();

                        var motoClientCode = moto.Select(r => r.ClientCode);

                        var quicki = qDB.ClientProfile.Where(r => motoClientCode.Contains(r.ClientCode)).ToList();

                        var join = from m in moto
                                   join c in quicki on m.ClientCode equals c.ClientCode
                                   join u in db.UserAccount on m.RequestedBy equals u.ID
                                   where m.Date >= DateTime.Now.AddDays(-3)
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
                                       PaxName = m.PaxName,
                                       RecordLocator = m.RecordLocator,
                                       Remarks = m.Remarks,
                                       Status = m.Status,
                                       Total = m.Total,
                                       RequestedBy = m.RequestedBy,
                                       ShowRequestedBy = u.FirstName + " " + u.LastName
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

                    var moto = db.MotoRequest.Where(r => r.Status == _status && r.RequestedBy == UniversalService.CurrentUser.ID).ToList();

                    var motoClientCode = moto.Select(r => r.ClientCode);

                    var quicki = qDB.ClientProfile.Where(r => motoClientCode.Contains(r.ClientCode)).ToList();

                    var join = from m in moto
                               join c in quicki on m.ClientCode equals c.ClientCode
                               join u in db.UserAccount on m.RequestedBy equals u.ID
                               where m.Date >= DateTime.Now.AddDays(-3)
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
                                   PaxName = m.PaxName,
                                   RecordLocator = m.RecordLocator,
                                   Remarks = m.Remarks,
                                   Status = m.Status,
                                   Total = m.Total,
                                   RequestedBy = m.RequestedBy,
                                   ShowRequestedBy = u.FirstName + " " + u.LastName
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
                            PaxName = _request.PaxName,
                            RecordLocator = _request.RecordLocator,
                            Currency = _request.Currency,
                            Amount = _request.Amount,
                            Others = _request.Others,
                            BCDFee = _request.BCDFee,
                            AdminFee = _request.AdminFee,
                            Total = _request.Total,
                            OptionTime = _request.OptionTime,
                            ApprovalCode = _request.ApprovalCode,
                            Remarks = _request.Remarks,
                            Invoice = _request.Invoice,
                            Status = _request.Status
                        };

                        db.Entry(newMoto).State = EntityState.Added;
                    }
                    else
                    {
                        var moto = db.MotoRequest.FirstOrDefault(r => r.ID == _request.ID);

                        if(moto != null)
                        {
                            moto.ClientCode = _request.ClientCode;

                            moto.Company = _request.Company;

                            moto.PaxName = _request.PaxName;

                            moto.RecordLocator = _request.RecordLocator;

                            moto.Currency = _request.Currency;

                            moto.AdminFee = _request.AdminFee;

                            moto.Total = _request.Total;

                            moto.OptionTime = _request.OptionTime;

                            moto.ApprovalCode = _request.ApprovalCode;

                            moto.Remarks = _request.Remarks;

                            moto.Invoice = _request.Invoice;

                            moto.Status = _request.Status;

                            db.Entry(moto).State = EntityState.Modified;
                        }
                    }

                    db.SaveChanges();
                }
            }
            catch(Exception error)
            {
                message = error.Message;
            }
        }
    }
}