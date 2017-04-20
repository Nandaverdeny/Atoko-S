using ATokoEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace AToko.Models
{
    public static class Date
    {
        public static string IsMockDate
        {
            get
            {
                return ConfigurationManager.AppSettings["IsMockDate"].ToString();
            }
        }
        public static string ShowMockDate
        {
            get
            {
                return ConfigurationManager.AppSettings["ShowMockDate"].ToString();
            }
        }
        public static DateTime getDate()
        {
            if (IsMockDate == "1")
            {
                string day = ConfigurationManager.AppSettings["Day"].ToString().Trim();
                string month = ConfigurationManager.AppSettings["Month"].ToString().Trim();
                string year = ConfigurationManager.AppSettings["Year"].ToString().Trim();

                int _day = 1;
                int _month = 1;
                int _year = 1990;

                if (day != "")
                {
                    _day = int.Parse(day);
                }
                else
                {
                    _day = DateTime.Today.Day;
                }

                if (month != "")
                {
                    _month = int.Parse(month);
                }
                else
                {
                    _month = DateTime.Today.Month;
                }

                if (year != "")
                {
                    _year = int.Parse(year);
                }
                else
                {
                    _year = DateTime.Today.Year;
                }

                return new DateTime(_year, _month, _day);
            }
            else
            {
                return DateTime.Today;
            }

        }

    }
}