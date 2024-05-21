using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Yootek.EntityDb;
using Yootek.Common;
using static Yootek.YootekServiceBase;
namespace Yootek.Services
{
    [AutoMap(typeof(NhaSanXuat))]
    public class NhaSanXuatDto : NhaSanXuat
    {
    }    
    public class GetAllNhaSanXuatInputDto : CommonInputDto
    {
        public FieldSortNhaSanXuat? OrderBy { get; set; }
    }
    public enum FieldSortNhaSanXuat
    {
        [FieldName("Id")]
        ID = 1,
    }
}
