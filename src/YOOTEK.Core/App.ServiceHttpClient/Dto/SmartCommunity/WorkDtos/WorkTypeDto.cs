#nullable enable
using Abp.Domain.Entities;
using Yootek.Common;
using System;
using System.Collections.Generic;

namespace Yootek.App.ServiceHttpClient.Dto.Yootek.SmartCommunity.WorkDtos
{
    public class WorkTypeDto : Entity<long>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreationTime { get; set; }
        public int Type { get; set; }
        public int NumberDetail { get; set; }
        public TypeStructure TypeStructure { get; set; }
        public long? UrbanId { get; set; }
        public long? BuildingId { get; set; }
        public long? OrganizationUnitId { get; set; }
    }

    public class WorkTypeDetailDto : Entity<long>
    {
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<DetailWorkDto>? WorkDetails { get; set; }
        public DateTime CreationTime { get; set; }
        public TypeStructure TypeStructure { get; set; }
        public long? UrbanId { get; set; }
        public long? BuildingId { get; set; }
        public long? OrganizationUnitId { get; set; }
    }

    public class GetWorkTypeDto : Entity<long>
    {
    }

    public class GetAllWorkTypeDto : CommonInputDto
    {
        public int? Type { get; set; }
        //public TypeStructure? TypeStructure { get; set; }
        public long? UrbanId { get; set; }
        public long? BuildingId { get; set; }
        public long? OrganizationUnitId { get; set; }
    }
    public class GetAllWorkTypeNotPagingDto : FilteredInputDto
    {
        public int? typeWork { get; set; }
    }

    public class CreateWorkTypeDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Type { get; set; }
        public List<WorkDetainInCreateWorkTypeDto>? WorkDetails { get; set; }
        public TypeStructure TypeStructure { get; set; }
        public long? UrbanId { get; set; }
        public long? BuildingId { get; set; }
        public long? OrganizationUnitId { get; set; }
    }

    public class UpdateWorkTypeDto : Entity<long>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Type { get; set; }
        public List<WorkDetainInCreateWorkTypeDto>? WorkDetails { get; set; }
        public TypeStructure TypeStructure { get; set; }
        public long? UrbanId { get; set; }
        public long? BuildingId { get; set; }
        public long? OrganizationUnitId { get; set; }
    }

    public class DeleteWorkTypeDto : Entity<long>
    {
    }
    public enum TypeWork
    {
        Normal = 0,
        DigitalServices = 1,
    }

    public enum TypeStructure
    {
        Organization = 1,
        Project = 2
    }
}
