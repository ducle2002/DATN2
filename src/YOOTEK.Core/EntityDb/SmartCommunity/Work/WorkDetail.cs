using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yootek.EntityDb
{
    [Table("WorkDetails")]
    public class WorkDetail : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long WorkTypeId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Frequency { get; set; }
        public int? FrequencyTimes { get; set; }
        public string? FrequencyOption { get; set; }
        public int? DayOfMonth { get; set; }
        public List<bool> Checks { get; set; }
    }
}
