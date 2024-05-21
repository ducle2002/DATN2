using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Yootek.EntityDb;
using Yootek.Common;
using static Yootek.YootekServiceBase;
namespace Yootek.Services
{
    [AutoMap(typeof(DonViTaiSan))]
    public class DonViTaiSanDto : DonViTaiSan
    {
    }    
    public class GetAllDonViTaiSanInputDto : CommonInputDto
    {
        public FieldSortDonViTaiSan? OrderBy { get; set; }
    }
    public enum FieldSortDonViTaiSan
    {
        [FieldName("Id")]
        ID = 1,
    }
}
