using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yootek.EntityDb
{
    public class Ward : Entity<string>
    {
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Code { get; set; }
        [StringLength(255)]
        public string NameEn { get; set; }
        [StringLength(255)]
        public string FullName { get; set; }
        [StringLength(255)]
        public string FullNameEn { get; set; }
        [StringLength(255)]
        public string CodeName { get; set; }
        [StringLength(20)]
        public string DistrictCode { get; set; }
        public int AdministrativeUnitId { get; set; }
    }
}
