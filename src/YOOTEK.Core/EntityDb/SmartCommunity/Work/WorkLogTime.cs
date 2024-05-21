using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;

namespace Yootek.EntityDb
{
    [Table("WorkLogTimes")]
    public class WorkLogTime : FullAuditedEntity<long>, IMayHaveTenant
    {
        public long WorkId { get; set; }
        public long WorkDetailId { get; set; }
        public LogTimeStatus Status { get; set; }
        public string? Note { get; set; }
        public List<string>? ImageUrls { get; set; }
        public ReadState ReadState { get; set; }
        public int TurnNumber { get; set; }
        public DateTime WorkDate { get; set; }
        public int? TenantId { get; set; }
        public EWorkQuality? QualityWork { get; set; } //Chất lượng công việc
    }

    public enum LogTimeStatus
    {
        NOT_STARTED = 1,  // chưa bắt đầu
        IN_PROGRESS = 2,  // đang thực hiện
        PAUSED = 3,  // tạm dừng
        COMPLETED = 4,  // hoàn thành
        CANCELLED = 5,  // hủy bỏ
        PENDING_APPROVAL = 6,  // chờ duyệt
        EXPIRED = 7,  // hết hạn
    }

    //Chất lượng công việc
    public enum EWorkQuality
    {
        GOOD = 1, // Tốt
        NOTGOOD = 2, // Không tốt
    }
}
