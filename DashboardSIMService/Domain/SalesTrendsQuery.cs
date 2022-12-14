using System;
using System.Collections.Generic;
using Nest;

namespace DashboardSIMService.Domain
{
    public class SalesTrendsQuery
    {
        public string FilterByProductCode { get; }
        public DateTime FilterBySalesDateStart { get; }
        public DateTime FilterBySalesDateEnd { get; }
        public TimeAggregationUnit AggregationUnit { get; }

        public SalesTrendsQuery(string filterByProductCode, DateTime filterBySalesDateStart, DateTime filterBySalesDateEnd, TimeAggregationUnit aggregationUnit)
        {
            FilterByProductCode = filterByProductCode;
            FilterBySalesDateStart = filterBySalesDateStart;
            FilterBySalesDateEnd = filterBySalesDateEnd;
            AggregationUnit = aggregationUnit;
        }
    }

    public enum TimeAggregationUnit
    {
        Day,
        Week,
        Month,
        Year
    }

    public static class TimeAggregationUnitExtensions
    {
        public static DateInterval ToDateInterval(this TimeAggregationUnit unit) => unit switch
        {
            TimeAggregationUnit.Day => DateInterval.Day,
            TimeAggregationUnit.Week => DateInterval.Week,
            TimeAggregationUnit.Month => DateInterval.Month,
            TimeAggregationUnit.Year => DateInterval.Year,
            _ => throw new ArgumentException($"Invalid value of unit {unit}")
        };

        public static TimeAggregationUnit ToTimeAggregationUnit(this DashboardSIMService.Dtos.Queries.TimeUnit unit) =>
            unit switch
            {
                DashboardSIMService.Dtos.Queries.TimeUnit.Day => TimeAggregationUnit.Day,
                DashboardSIMService.Dtos.Queries.TimeUnit.Week => TimeAggregationUnit.Week,
                DashboardSIMService.Dtos.Queries.TimeUnit.Month => TimeAggregationUnit.Month,
                DashboardSIMService.Dtos.Queries.TimeUnit.Year => TimeAggregationUnit.Year,
                _ => throw new ArgumentException($"Invalid value of unit {unit}")
    };
    }

    public class SalesTrendsResult
    {
        public IList<PeriodSales> PeriodSales { get; }

        public SalesTrendsResult()
        {
            PeriodSales = new List<PeriodSales>();
        }

        public void PeriodResult(PeriodSales periodSales) => PeriodSales.Add(periodSales);

    }

    public class PeriodSales
    {
        public DateTime PeriodDate { get; }
        public string Period { get; }
        public SalesResult Sales { get; }

        public PeriodSales(DateTime periodDate, string period, SalesResult sales)
        {
            PeriodDate = periodDate;
            Period = period;
            Sales = sales;
        }
    }
}