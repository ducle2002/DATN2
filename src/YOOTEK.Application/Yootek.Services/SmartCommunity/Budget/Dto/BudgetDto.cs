using System;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Yootek.Common;
using YOOTEK.EntityDb.SmartCommunity.Budget;

namespace Yootek.Services
{
    [AutoMap(typeof(Budget))]
    public class CreateBudgetDto
    {
        public double Electricity { get; set; }
        public double Water { get; set; }
        public double Contractors { get; set; }
        public double Elevator { get; set; }
        public double Materials { get; set; }
        public double AirConditionerRepair { get; set; }
        public long BuildingId { get; set; }
        public long UrbanId { get; set; }
        public DateTime CreationTime { get; set; }
    }
    public class UpdateBudgetDto
    {
        public double Electricity { get; set; }
        public double Water { get; set; }
        public double Contractors { get; set; }
        public double Elevator { get; set; }
        public double Materials { get; set; }
        public double AirConditionerRepair { get; set; }
        public long Id { get; set; }
    }
    public class GetAllBudget : Entity<long>
    {
        public DateTime CreationTime { get; set; }
        public double Electricity { get; set; }
        public double Water { get; set; }
        public double Contractors { get; set; }
        public double Elevator { get; set; }
        public double Materials { get; set; }
        public double AirConditionerRepair { get; set; }
        public long BuildingId { get; set; }
        public long UrbanId { get; set; }
        public string BuildingName { get; set; }
        public string UrbanName { get; set; }
        public double Total { get; set; }
        public string Data { get; set; }
        public int ViewQuarterly { get; set; }
        public DateTime ViewYear { get; set; }
        public BudgetType Type { get; set; }
    }

    public class BudgetInput : CommonInputDto
    {
        public DateTime? CreationTime { get; set; }
        public long? BuildingId { get; set; }
        public long? UrbanId { get; set; }
        public BudgetType Type { get; set; }

    }
    public class QuarterlyAndAnnuallyInput : CommonInputDto
    {
        public int Quarterly { get; set; }
        public long? BuildingId { get; set; }
        public long? UrbanId { get; set; }
        public DateTime Year { get; set; }
        public BudgetType Type { get; set; }

    }
    public class ImportBudgetRealityExcelInput
    {
        public IFormFile File { get; set; }
    }
}
