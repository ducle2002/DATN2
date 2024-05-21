using System;
using System.Collections.Generic;
using System.Linq;
using Abp.AutoMapper;
using Yootek.Common;
using Yootek.Common.Enum;
using Yootek.EntityDb;
using Yootek.Organizations.Interface;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using static Yootek.YootekServiceBase;
using Yootek.Services.SmartCommunity.ExcelBill.Dto;

namespace Yootek.Services.Dto
{
    public class GetAllBillConfigDto
    {

        public BillType? BillType { get; set; }
        public BillProperites? Properties { get; set; }
    }
    
    public class BillConfigProperties
    {
        public BillType? BillType { get; set; }
        public BillProperites Properties { get; set; }
    }
}