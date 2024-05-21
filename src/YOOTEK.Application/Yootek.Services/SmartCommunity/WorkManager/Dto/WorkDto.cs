using Abp.Application.Services.Dto;
using Yootek.App.ServiceHttpClient.Dto.Yootek.SmartCommunity.WorkDtos;
using Yootek.Common;

namespace Yootek.Services
{
    public class GetListWorkInput : CommonInputDto
    {
        public EWorkStatus? Status { get; set; }
        public FormIdWork? FormId { get; set; }
        public long? WorkTypeId { get; set; }
        public long? UrbanId { get; set; }
        public long? BuildingId {  get; set; }
        public long? OrganizationUnitId { get; set; }
        public TypeStructure? TypeStructure { get; set; }
        public int? Type { get; set;  }
    }
}
