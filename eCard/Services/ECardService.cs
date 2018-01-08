﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eCard.Models;
using System.Data.Entity;

namespace eCard.Services
{
    public class ECardService
    {
        public static List<MotoRequestModel> GetDuplicate(string _reloc, out string message)
        {
            try
            {
                message = "";

                using (var db = new eCardEntities())
                {
                    var qDB = new QuickipediaEntities();

                    var moto = db.MotoRequest.Where(r => r.RecordLocator == _reloc).ToList();

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
                        moto = db.MotoRequest.Where(r => (r.Status == "P" || r.Status == "F") && r.RequestedBy == UniversalService.CurrentUser.ID
                            && r.Date > date).ToList();
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
                            RecordLocator = _request.RecordLocator,
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

                        //******************EMAIL NOTIFICATION***************
                        //if (_request.Status == "P")
                        //{
                        //    string text = "\n Credit Card Moto Request" +
                        //        "\n\n Record Locator :" + _request.RecordLocator +
                        //        "\n Total Amount: " + _request.Currency + " " + string.Format("{0:0.00}", newMoto.Total) +
                        //        "\n Requested By: " + UniversalService.CurrentUser.Firstname + " " + UniversalService.CurrentUser.LastName;

                        //    var approverEmail = db.UserAccount.Where(r => r.Type == "APR").ToList();

                        //    approverEmail.ForEach(item =>
                        //    {
                        //        EmailService.SendEmail("eCard // Moto Request // " +_request.RecordLocator, text, item.Email);
                        //    });
                        //}
                    }
                    else
                    {
                        var moto = db.MotoRequest.FirstOrDefault(r => r.ID == _request.ID);

                        if(moto != null)
                        {
                            moto.ClientCode = _request.ClientCode;

                            moto.Company = _request.Company;

                            moto.PaxName = _request.PaxName.ToUpper();

                            moto.RecordLocator = _request.RecordLocator;

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
                                moto.ApprovedDate = DateTime.Now;

                            db.Entry(moto).State = EntityState.Modified;

                            db.SaveChanges();

                            //*******************EMAIL NOTIFICATION***************
                            //if(_request.Status == "X")
                            //{
                            //    string text = "\n Credit Card Moto Request - Has been Canceled" +
                            //    "\n\n Record Locator :" + _request.RecordLocator +
                            //    "\n Total Amount: " + _request.Currency + " " + string.Format("{0:0.00}", _request.Total) +
                            //    "\n Requested By: " + UniversalService.CurrentUser.Firstname + " " + UniversalService.CurrentUser.LastName;

                            //    var approverEmail = db.UserAccount.Where(r => r.Type == "APR").ToList();

                            //    approverEmail.ForEach(item =>
                            //    {
                            //        EmailService.SendEmail("eCard // Moto Request // " + _request.RecordLocator + "\\ Canceled", text, item.Email);
                            //    });
                            //}
                            //else if(_request.Status == "A")
                            //{
                            //    string text = "\n Credit Card Moto Request - Has been Approved" +
                            //    "\n\n Record Locator :" + _request.RecordLocator +
                            //    "\n Total Amount: " + _request.Currency + " " + string.Format("{0:0.00}", _request.Total) +
                            //    "\n Requested By: " + UniversalService.GetRequestor(_request.RequestedBy, out message) +
                            //    "\n Approval Code: " + _request.ApprovalCode;

                            //    var approverEmail = db.UserAccount.Where(r => r.Type == "APR").ToList();

                            //    approverEmail.ForEach(item =>
                            //    {
                            //        EmailService.SendEmail("eCard // Moto Request //" + _request.RecordLocator + "\\ Approved", text, item.Email);
                            //    });
                            //}
                            //else if(_request.Status == "D")
                            //{
                            //    string text = "\n Credit Card Moto Request - Has been Declined" +
                            //    "\n\n Record Locator :" + _request.RecordLocator +
                            //    "\n Total Amount: " + _request.Currency + " " + string.Format("{0:0.00}", _request.Total) +
                            //    "\n Requested By: " + UniversalService.GetRequestor(_request.RequestedBy, out message)
                            //    + "\n Declined Reason: " + _request.DeclinedReason;

                            //    var approverEmail = db.UserAccount.Where(r => r.Type == "APR").ToList();

                            //    approverEmail.ForEach(item =>
                            //    {
                            //        EmailService.SendEmail("eCard // Moto Request // " + _request.RecordLocator + "\\ Declined", text, item.Email);
                            //    });
                            //}
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