using Yootek.Common.Enum;
using Yootek.EntityDb;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Yootek.Services.SmartCommunity.ExcelBill.Dto
{
    public class BillProperites
    {
        public string customerName { get; set; }
        public long[] formulas { get; set; }
        public BillConfig[] formulaDetails { get; set; }
        public long? pricesType { get; set; }
        public BillConfig vehicleFormulaDetail { get; set; }


    }
}
