﻿namespace PROG_P1.Models
{
    public class Claims
    {
        public int ClaimsID { get; set; }
        public string Name { get; set; }
        public string Module { get; set; }
        public int HoursWorked { get; set; }
        public double HourlyWage { get; set; }
        public double TotalEarnings
        {
            get
            {
                return HoursWorked * HourlyWage;
            }
        }
        // public byte[]? Document { get; set; }

    }
}
