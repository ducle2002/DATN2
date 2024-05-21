using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace YOOTEK.EntityDb.SmartCommunity.Budget
{
    [Table("Budget", Schema = "ioc_rental")]
    public class Budget : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public double Electricity { get; set; }
        public double Water { get; set; }
        public double Contractors { get; set; }
        public double Elevator { get; set; }
        public double Materials { get; set; }
        public double AirConditionerRepair { get; set; }
        public long BuildingId { get; set; }
        public long UrbanId { get; set; }
        public BudgetType Type { get; set; }
    }
    public enum BudgetType
    {
        ESTIMATE = 1,
        REALITY = 2,
    }
}
