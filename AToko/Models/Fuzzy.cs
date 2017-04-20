using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AToko.Models
{
    public class Fuzzy
    {
        
        public decimal MaxNumber { get; set; }
        public decimal MinNumber { get; set; }
        public decimal MedianNumber { get; set; }
        public decimal ResultLinearUp { get; set; }
        public decimal ResultLinearDown { get; set; }
        public decimal Input { get; set; }
        public bool IsMax { get; set; }
        public decimal Range 
        { 
            get 
            {
                return MaxNumber - MinNumber;
            }
            set
            {
                Range = value;
            }
        }


        public int OrderStatus { get; set; }
        public bool InputSalesStatusIsUp { get; set; }
        public bool InputStockStatusIsUp { get; set; }

        public decimal InputSales { get; set; }
        public decimal InputStock { get; set; }

        public decimal Rule()
        {
            return Math.Min(InputSales, InputStock);
        }

        /// <summary>
        /// Sales Down and Stock Up then Order Down
        /// </summary>
        public decimal Rule1;

        /// <summary>
        /// Sales Down and Stock Down then Order Down
        /// </summary>
        public decimal Rule2;

        /// <summary>
        /// Sales Up and Stock Up then Order Up
        /// </summary>
        public decimal Rule3;
    
        /// <summary>
        /// Sales Up and Stock Down then Order Up
        /// </summary>
        public decimal Rule4;

        public decimal UpResult;
        public decimal DownResult;

        public decimal OrderMax;
        public decimal OrderMin;

        public decimal OrderRange 
        { 
            get 
            {
                return OrderMax - OrderMin;
            }
            set
            {
                OrderRange = value;
            }
        }

        public decimal Result { get; set; }

        public Fuzzy()
        {
        }

        public Fuzzy(decimal _MinNumber,decimal _MaxNumber,decimal _Input,bool _IsMax)
        {
            MinNumber = _MinNumber;
            MaxNumber = _MaxNumber;
            Input = _Input;
        }

        public Fuzzy(decimal _MinNumber,decimal _MaxNumber,decimal _Input)
        {
            MinNumber = _MinNumber;
            MaxNumber = _MaxNumber;
            Input = _Input;
        }

        public Fuzzy(decimal _MinNumber,decimal _MaxNumber)
        {
            MinNumber = _MinNumber;
            MaxNumber = _MaxNumber;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal CalculateLinearUp()
        {
            if (Input <= MinNumber)
            {
                ResultLinearUp = 0;
            }
            else if (MinNumber <= Input && Input <= MaxNumber)
            {
                ResultLinearUp = (Input - MinNumber) / Range;
            }
            else if (Input >= MaxNumber)
            {
                ResultLinearUp = 1;
            }

            return ResultLinearUp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal CalculateLinearDown()
        {
            if (Input <= MinNumber)
            {
                ResultLinearDown = 1;
            }
            else if (MinNumber <= Input && Input <= MaxNumber)
            {
                ResultLinearDown = (MaxNumber - Input) / Range;
            }
            else if (Input >= MaxNumber)
            {
                ResultLinearDown = 0;
            }
            
            return ResultLinearDown;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal ImplicationFunction()
        {
            return Rule();
            //return isMax ? Math.Max(InputDemand, InputProduction) : Math.Min(InputDemand, InputProduction);
        }

        /// <summary>
        /// Calculate Rule Composition And Grouping By Order Status,
        /// then evaluate Max value by Order Status
        /// </summary>
        /// <returns>DownResult And UpResult</returns>
        public void RuleComposition()
        {
            DownResult = Math.Max(Rule1, Rule2);
            UpResult = Math.Max(Rule3, Rule4);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void Membership()
        {
            if (OrderStatus == 1) // Up
            {
                if (Input <= OrderMin)
                {
                    Result = UpResult;
                }
                else if (OrderMin <= Input && Input <= OrderMax)
                {
                    Result = (Input - MinNumber) / Range;
                }
                else if (Input >= OrderMax)
                {
                    Result = DownResult;
                }
            }
            else if (OrderStatus == 2) // Down
            {
                if (Input <= OrderMin)
                {
                    Result = UpResult;
                }
                else if (OrderMin <= Input && Input <= OrderMax)
                {
                    Result = (MaxNumber - Input) / Range;
                }
                else if (Input >= OrderMax)
                {
                    Result = DownResult;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal CentroidM1()
        {
            decimal result = 0;

            decimal a = DownResult / 2;

            decimal z2 = OrderMin * OrderMin;

            result = a * z2;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal CentroidM2()
        {
            decimal result = 0;

            if (OrderStatus == 1) // Up
            {
                decimal a = (1 / Range) / 3;
                decimal b = (MinNumber / Range) / 2;

                decimal z3up = (OrderMax * OrderMax * OrderMax);
                decimal z2up = (OrderMax * OrderMax);

                decimal z3down = (OrderMin * OrderMin * OrderMin);
                decimal z2down = (OrderMin * OrderMin);

                result = ((a * z3up) - (b * z2up)) - ((a * z3down) - (b * z2down));
            }
            else if (OrderStatus == 2) // Down
            {
                decimal a = (MaxNumber / Range) / 2;
                decimal b = (1 / Range) / 3;

                decimal z3up = (OrderMax * OrderMax * OrderMax);
                decimal z2up = (OrderMax * OrderMax);

                decimal z3down = (OrderMin * OrderMin * OrderMin);
                decimal z2down = (OrderMin * OrderMin);

                result = ((a * z2up) - (b * z3up)) - ((a * z2down) - (b * z3down));
            }
            

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal CentroidM3()
        {
            decimal result = 0;

            decimal a = UpResult / 2;

            decimal z2up = MaxNumber * MaxNumber;
            decimal z2down = OrderMax * OrderMax;
       
             result = (a * z2up) - (a * z2down);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal CalculateAreaWidth()
        {
            decimal a1 = OrderMin * DownResult;
            decimal a2 = ((UpResult + DownResult) * (OrderMin + OrderMax)) / 2;
            decimal a3 = MaxNumber - OrderMax;

            return a1 + a2 + a3;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public decimal Defuzzyfication()
        {
            decimal result = 0;
            decimal m1 = CentroidM1();
            decimal m2 = CentroidM2();
            decimal m3 = CentroidM3();
            decimal area = CalculateAreaWidth();

            if (area != 0)
            {
                result = (m1 + m2 + m3) / area;
            }


            return result;
        }

    }

    
}