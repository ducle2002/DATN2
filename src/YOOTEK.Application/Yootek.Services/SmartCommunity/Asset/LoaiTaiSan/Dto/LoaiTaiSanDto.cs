using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Yootek.EntityDb;
using Yootek.Common;
using static Yootek.YootekServiceBase;
namespace Yootek.Services
{
    [AutoMap(typeof(LoaiTaiSan))]
    public class LoaiTaiSanDto : LoaiTaiSan
    {
    }    
    public class GetAllLoaiTaiSanInputDto : CommonInputDto
    {
        public FieldSortLoaiTaiSan? OrderBy { get; set; }
    }
    public enum FieldSortLoaiTaiSan
    {
        [FieldName("Id")]
        ID = 1,
    }
}
