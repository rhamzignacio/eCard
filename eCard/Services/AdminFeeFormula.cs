using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eCard.Models;

namespace eCard.Services
{
    public class AdminFeeFormula
    {
        public static double GetAdminFeeFromDB(MotoRequestModel _moto, out string message)
        {
            try
            {
                message = "";

                double _airFare, _serviceFee, _others;

                if (_moto.Amount == null)
                    _airFare = 0;
                else
                    _airFare = double.Parse(_moto.Amount.ToString());

                if (_moto.BCDFee == null)
                    _serviceFee = 0;
                else
                    _serviceFee = double.Parse(_moto.BCDFee.ToString());

                if (_moto.Others == null)
                    _others = 0;
                else
                    _others = double.Parse(_moto.Others.ToString());

                if (_moto.Currency == "" || _moto.Currency == null)
                {
                    message = "Currency is required to auto compute admin fee";

                    return 0;
                }

                using (var db = new QuickipediaEntities())
                {
                    var admFormula = db.EcardAdminFee.FirstOrDefault(r => r.ClientCode == _moto.ClientCode);

                    if (admFormula != null)
                    {
                        double total = 0;

                        if (_moto.Currency == "PHP")
                        {
                            if (admFormula.AirFareFlag)
                                total += _airFare;

                            if (admFormula.ServiceFeeFlag)
                                total += _serviceFee;

                            if (admFormula.OtherFeeFlag)
                                total += _others;

                            if (admFormula.Divide > 0)
                                total = total / double.Parse(admFormula.Divide.ToString());

                            if (admFormula.Multiply > 0)
                                total = total * double.Parse(admFormula.Multiply.ToString());
                        }
                        else
                        {
                            if (admFormula.AirFareUSD)
                                total += _airFare;

                            if (admFormula.ServiceFeeUSD)
                                total += _serviceFee;

                            if (admFormula.OthersUSD)
                                total += _others;

                            if (admFormula.DivideUSD > 0)
                                total = total / double.Parse(admFormula.DivideUSD.ToString());

                            if (admFormula.MultiplyUSD > 0)
                                total = total * double.Parse(admFormula.MultiplyUSD.ToString());
                        }

                        return total;
                    }
                    else
                    {
                        message = "Admin Fee Profile not Found (You need to compute admin fee manually)";

                        return 0;
                    }
                }
            }
            catch (Exception error)
            {
                message = error.Message;

                return 0;
            }
        }
        public static double GetAdminFee(string _clientCode, double? airFare, double? serviceFee,
            double? others, out string message)
        {
            try
            {
                message = "";

                double  _airFare, _serviceFee, _others;

                if (airFare == null)
                    _airFare = 0;
                else
                    _airFare = double.Parse(airFare.ToString());

                if (serviceFee == null)
                    _serviceFee = 0;
                else
                    _serviceFee = double.Parse(serviceFee.ToString());

                if (others == null)
                    _others = 0;
                else
                    _others = double.Parse(others.ToString());

                if (_clientCode == "200CL00043")//Nestle Business
                    return (_airFare + _serviceFee + _others) / 0.972 * 0.028;

                else if (_clientCode == "200CL00177" || _clientCode == "200CL00178" || _clientCode == "200CL00176") //AIA Philam
                    return (_airFare + _serviceFee + _others) / 0.972 * 0.028;

                else if (_clientCode == "200CL00044")//Nestle BPI
                    return (_airFare + _others) * 0.0235;

                else if (_clientCode == "200CL00150") //Allergan Healthcare Phil Inc
                    return (_airFare + _serviceFee + _others) / 0.972 * 0.028;

                else if (_clientCode == "200CL00158") //Astrazeneca
                    return (_airFare + _serviceFee + _others) / 0.972 * 0.028;

                else if (_clientCode == "200CL00121") //Autodesk
                    return (_airFare + _serviceFee + _others) / 0.972 * 0.028;

                else if (_clientCode == "200CL00162") //Cardinal
                    return (_airFare + _serviceFee + _others) / 0.972 * 0.028;

                else if (_clientCode == "200CL00160") //Energizer
                    return (_airFare + _serviceFee + _others) / 0.972 * 0.028;

                else if (_clientCode == "200CL00129" || _clientCode == "200CL00031") //Hitafchi
                    return (_airFare + _serviceFee + _others) / 0.972 * 0.028;

                else if (_clientCode == "200CL00135" || _clientCode == "200CL00131" || _clientCode == "200CL00133"
                    || _clientCode == "200CL00125")
                    return (_airFare + _serviceFee + _others) / 0.972 * 0.028;

                else if (_clientCode == "200CL00100" || _clientCode == "200CL00091") //Mondelez
                    return (_airFare + _serviceFee + _others) / 0.972 * 0.028;

                else if (_clientCode == "200CL00181") //Nike Philippines
                    return (_airFare + _serviceFee + _others) / 0.972 * 0.028;

                else if (_clientCode == "200CL00106" || _clientCode == "200CL00105" || _clientCode == "200CL00161") //OPTUM
                    return (_airFare + _serviceFee + _others) / 0.972 * 0.028;

                else if (_clientCode == "200CL00153" || _clientCode == "200CL00154") //PPG
                    return (_airFare + _serviceFee + _others) / 0.972 * 0.028;

                else if (_clientCode == "200CL00145") //UNIFY
                    return (_airFare + _serviceFee + _others) / 0.972 * 0.028;

                else if (_clientCode == "300CL00070") //WYETH
                    return (_airFare + _others) / 0.972 * 0.028;

                else if (_clientCode == "200CL00076" || _clientCode == "200CL00074" || _clientCode == "200CL00075")//MAXIM
                    return (_airFare + _serviceFee + _others) / 0.97 * 0.03;

                else if (_clientCode == "200CL00047" || _clientCode == "200CL00045" || _clientCode == "200CL00046")//PFIZER
                    return (_airFare + _serviceFee + _others) * 0.03;

                else if (_clientCode == "200CL00174") //BNP Paribas
                    return (_airFare + _serviceFee + _others) / 0.965 * 0.035;

                else if (_clientCode == "200CL00072" || _clientCode == "200CL00064" || _clientCode == "200CL00117"
                    || _clientCode == "200CL00071")//Cognizant
                    return (_airFare + _serviceFee + _others) / 0.965 * 0.035;

                else if (_clientCode == "200CL00136")//INFINEON
                    return (_airFare + _serviceFee + _others) / 0.965 * 0.035;

                else if (_clientCode == "200CL00156" || _clientCode == "200CL00042")//NCR
                    return (_airFare + _serviceFee + _others) / 0.965 * 0.035;

                else if (_clientCode == "200CL00132") //Northen
                    return (_airFare + _serviceFee + _others) / 0.965 * 0.035;

                else if (_clientCode == "200CL00048")//SAP
                    return (_airFare + _serviceFee + _others) / 0.965 * 0.035;

                else if (_clientCode == "300CL00101" || _clientCode == "300CL00102")//Shipserve
                    return (_airFare + _serviceFee + _others) / 0.965 * 0.035;

                else if (_clientCode == "200CL00057") //Warner
                    return (_airFare + _serviceFee + _others) / 0.965 * 0.035;


                //======AFTER INVOICE=========

                else if (_clientCode == "200CL00040") //Misys
                    return (_airFare + _serviceFee + _others) / 0.972 * 0.028;

                else if (_clientCode == "200CL00063") //BE AEROSPACE
                    return (_airFare + _serviceFee + _others) / 0.97 * 0.03;

                else if (_clientCode == "200CL00078") //HUSKY
                    return (_airFare + _serviceFee + _others) / 0.97 * 0.03;

                else if (_clientCode == "200CL00166") //DEUTSCHE 
                    return (_airFare + _serviceFee + _others) / 0.965 * 0.035;

                else if (_clientCode == "200CL00098") //INFOR
                    return (_airFare + _serviceFee + _others) / 0.965 * 0.035;

                else if (_clientCode == "200CL00123" || _clientCode == "200CL00124") //Marsh Mercer
                    return (_airFare + _serviceFee + _others) / 0.965 * 0.035;

                else if (_clientCode == "200CL00122" || _clientCode == "200CL00112") //MSD
                    return (_airFare + _serviceFee + _others) / 0.965 * 0.035;

                else if (_clientCode == "200CL00058") //Wartsila
                    return (_airFare + _serviceFee + _others) / 0.965 * 0.035;

                else if (_clientCode == "200CL00183") //RHM and HAAS
                    return (_airFare + _serviceFee + _others) / 0.972 * 0.028;

                else if (_clientCode == "200CL00155" || _clientCode == "300CL00010" || _clientCode == "300CL0009")
                    return (_airFare + _serviceFee + _others) / 0.972 * 0.028;

                else
                {
                    message = "No Admin Fee Profile Found";
                    return 0;
                }
            }
            catch (Exception error)
            {
                message = error.Message;

                return 0;
            }
        }
    }
}