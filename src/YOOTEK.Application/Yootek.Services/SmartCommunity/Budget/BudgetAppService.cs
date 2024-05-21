using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Yootek;
using Yootek.Application;
using Yootek.Common.DataResult;
using Yootek.Organizations;
using Yootek.Services;
using YOOTEK.EntityDb.SmartCommunity.Budget;

namespace YOOTEK.Yootek.Services
{
    public class BudgetAppService(IRepository<Budget, long> budgetRepository, IRepository<AppOrganizationUnit, long> organizationUnit) : YootekAppServiceBase
    {
        private readonly IRepository<Budget, long> _budgetRepository = budgetRepository;
        private readonly IRepository<AppOrganizationUnit, long> _organizationUnit = organizationUnit;
        public async Task<object> CreateBudgetEstimate(CreateBudgetDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                var budget = await _budgetRepository.FirstOrDefaultAsync(x =>
                    x.BuildingId == input.BuildingId &&
                    x.UrbanId == input.UrbanId &&
                    x.CreationTime.Year == DateTime.Now.Year &&
                    x.CreationTime.Month == DateTime.Now.Month && x.Type == BudgetType.ESTIMATE);

                if (budget != null)
                {
                    throw new UserFriendlyException("Creation failed");
                }

                budget = input.MapTo<Budget>();
                budget.TenantId = AbpSession.TenantId;
                budget.Type = BudgetType.ESTIMATE;

                var data = await _budgetRepository.InsertAsync(budget);

                mb.statisticMetris(t1, 0, "CreateBudgetService.CreateBudgetAsync");
                return DataResult.ResultSuccess(data, "Insert success");
            }
            catch (UserFriendlyException e)
            {
                Logger.Fatal(e.Message);
                throw new UserFriendlyException(e.Code, e.Message);
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                throw;
            }
        }
        public async Task<object> CreateBudgetReality(CreateBudgetDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                var budget = await _budgetRepository.FirstOrDefaultAsync(x =>
                    x.BuildingId == input.BuildingId &&
                    x.UrbanId == input.UrbanId &&
                    x.CreationTime.Year == DateTime.Now.Year &&
                    x.CreationTime.Month == DateTime.Now.Month && x.Type == BudgetType.REALITY);

                if (budget != null)
                {
                    throw new UserFriendlyException("Creation failed");
                }

                budget = input.MapTo<Budget>();
                budget.TenantId = AbpSession.TenantId;
                budget.Type = BudgetType.REALITY;
                var data = await _budgetRepository.InsertAsync(budget);

                mb.statisticMetris(t1, 0, "CreateBudgetService.CreateBudgetAsync");
                return DataResult.ResultSuccess(data, "Insert success");
            }
            catch (UserFriendlyException e)
            {
                Logger.Fatal(e.Message);
                throw new UserFriendlyException(e.Code, e.Message);
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                throw;
            }
        }

        public async Task<object> GetAllBudgetCurrentMonth(BudgetInput input)
        {
            try
            {
                IQueryable<GetAllBudget> query = (from u in _budgetRepository.GetAll()
                                                  select new GetAllBudget()
                                                  {
                                                      Id = u.Id,
                                                      CreationTime = u.CreationTime,
                                                      BuildingId = u.BuildingId,
                                                      UrbanId = u.UrbanId,
                                                      BuildingName = _organizationUnit.GetAll().Where(x => x.Id == u.BuildingId).Select(x => x.DisplayName).FirstOrDefault(),
                                                      UrbanName = _organizationUnit.GetAll().Where(x => x.Id == u.UrbanId).Select(x => x.DisplayName).FirstOrDefault(),
                                                      Electricity = u.Electricity,
                                                      Water = u.Water,
                                                      Contractors = u.Contractors,
                                                      Elevator = u.Elevator,
                                                      Materials = u.Materials,
                                                      AirConditionerRepair = u.AirConditionerRepair,
                                                      Total = u.Electricity + u.Water + u.Contractors + u.Elevator + u.Materials + u.AirConditionerRepair,
                                                      Data = u.CreationTime.ToString("MM/yyyy"),
                                                      ViewQuarterly = u.CreationTime.Month,
                                                      ViewYear = u.CreationTime,
                                                      Type = u.Type,
                                                  })
                                                  .Where(x => x.CreationTime.Year == input.CreationTime.Value.Year && x.CreationTime.Month == input.CreationTime.Value.Month)
                                                   .Where(x => x.Type == input.Type)
          .WhereIf(input.BuildingId.HasValue, x => x.BuildingId == input.BuildingId)
          .WhereIf(input.UrbanId.HasValue, x => x.UrbanId == input.UrbanId)
         .ApplySearchFilter(input.Keyword, u => u.BuildingName, u => u.UrbanName)
         .AsQueryable();
                var result = await query.PageBy(input).ToListAsync();
                var data = DataResult.ResultSuccess(result, "Get success", query.Count());

                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                throw;
            }
        }
        public async Task<object> GetAllBudgetCurrentQuarterlyDetail(QuarterlyAndAnnuallyInput input)
        {
            try
            {
                IQueryable<GetAllBudget> query = (from u in _budgetRepository.GetAll()
                                                  select new GetAllBudget()
                                                  {
                                                      Id = u.Id,
                                                      CreationTime = u.CreationTime,
                                                      BuildingId = u.BuildingId,
                                                      UrbanId = u.UrbanId,
                                                      BuildingName = _organizationUnit.GetAll().Where(x => x.Id == u.BuildingId).Select(x => x.DisplayName).FirstOrDefault(),
                                                      UrbanName = _organizationUnit.GetAll().Where(x => x.Id == u.UrbanId).Select(x => x.DisplayName).FirstOrDefault(),
                                                      Electricity = u.Electricity,
                                                      Water = u.Water,
                                                      Contractors = u.Contractors,
                                                      Elevator = u.Elevator,
                                                      Materials = u.Materials,
                                                      AirConditionerRepair = u.AirConditionerRepair,
                                                      Total = u.Electricity + u.Water + u.Contractors + u.Elevator + u.Materials + u.AirConditionerRepair,
                                                      Data = "Quý " + input.Quarterly + " " + u.CreationTime.ToString("yyyy"),
                                                      ViewQuarterly = input.Quarterly,
                                                      ViewYear = u.CreationTime,
                                                      Type = u.Type,
                                                  })
                                                  .Where(x => ((x.CreationTime.Month - 1) / 3 + 1) == input.Quarterly)
                                                  .Where(x => input.Year.Year == x.CreationTime.Year)
                                                  .Where(x => x.Type == input.Type)
                                                   .WhereIf(input.BuildingId.HasValue, x => x.BuildingId == input.BuildingId)
                                                   .WhereIf(input.UrbanId.HasValue, x => x.UrbanId == input.UrbanId)
                                                   .ApplySearchFilter(input.Keyword, u => u.BuildingName, u => u.UrbanName)
                                                   .AsQueryable();

                var result = await query.PageBy(input).ToListAsync();
                var data = DataResult.ResultSuccess(result, "Get success", query.Count());

                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                throw;
            }
        }
        public async Task<object> GetAllBudgetCurrentQuarterly(QuarterlyAndAnnuallyInput input)
        {
            try
            {
                IQueryable<GetAllBudget> query = (from u in _budgetRepository.GetAll()
                                                  select new GetAllBudget()
                                                  {
                                                      Id = u.Id,
                                                      CreationTime = u.CreationTime,
                                                      BuildingId = u.BuildingId,
                                                      UrbanId = u.UrbanId,
                                                      BuildingName = _organizationUnit.GetAll().Where(x => x.Id == u.BuildingId).Select(x => x.DisplayName).FirstOrDefault(),
                                                      UrbanName = _organizationUnit.GetAll().Where(x => x.Id == u.UrbanId).Select(x => x.DisplayName).FirstOrDefault(),
                                                      Electricity = u.Electricity,
                                                      Water = u.Water,
                                                      Contractors = u.Contractors,
                                                      Elevator = u.Elevator,
                                                      Materials = u.Materials,
                                                      AirConditionerRepair = u.AirConditionerRepair,
                                                      Total = u.Electricity + u.Water + u.Contractors + u.Elevator + u.Materials + u.AirConditionerRepair,
                                                      Data = "Quý " + input.Quarterly + " " + u.CreationTime.ToString("yyyy"),
                                                      ViewQuarterly = input.Quarterly,
                                                      ViewYear = u.CreationTime,
                                                      Type = u.Type,
                                                  })
                                                   .Where(x => ((x.CreationTime.Month - 1) / 3 + 1) == input.Quarterly)
                                                   .Where(x => x.Type == input.Type)
                                                   .Where(x => input.Year.Year == x.CreationTime.Year)
                                                   .WhereIf(input.BuildingId.HasValue, x => x.BuildingId == input.BuildingId)
                                                   .WhereIf(input.UrbanId.HasValue, x => x.UrbanId == input.UrbanId)
                                                   .ApplySearchFilter(input.Keyword, u => u.BuildingName, u => u.UrbanName)
                                                   .AsQueryable();

                var result = await query.ToListAsync();

                // Group by BuildingId and UrbanId and calculate sum of relevant properties
                var summaryList = result
                    .GroupBy(x => new { x.BuildingId, x.UrbanId, x.BuildingName, x.UrbanName })
                    .Select(group => new
                    {
                        Id = Guid.NewGuid(),
                        CreationTime = group.First().CreationTime,
                        BuildingId = group.Key.BuildingId,
                        UrbanId = group.Key.UrbanId,
                        BuildingName = group.Key.BuildingName,
                        UrbanName = group.Key.UrbanName,
                        Electricity = group.Sum(x => x.Electricity),
                        Water = group.Sum(x => x.Water),
                        Contractors = group.Sum(x => x.Contractors),
                        Elevator = group.Sum(x => x.Elevator),
                        Materials = group.Sum(x => x.Materials),
                        AirConditionerRepair = group.Sum(x => x.AirConditionerRepair),
                        Total = group.Sum(x => x.Total),
                        Data = "Quý " + input.Quarterly + " " + group.First().CreationTime.ToString("yyyy"),
                        ViewQuarterly = input.Quarterly,
                        ViewYear = group.First().CreationTime,
                    })
                    .ToList();

                var pagedSummaryList = summaryList;

                var data = DataResult.ResultSuccess(pagedSummaryList, "Get success", summaryList.Count);

                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                throw;
            }
        }

        public async Task<object> GetAllBudgetCurrentYear(QuarterlyAndAnnuallyInput input)
        {
            try
            {
                IQueryable<GetAllBudget> query = (from u in _budgetRepository.GetAll()
                                                  select new GetAllBudget()
                                                  {
                                                      Id = u.Id,
                                                      CreationTime = u.CreationTime,
                                                      BuildingId = u.BuildingId,
                                                      UrbanId = u.UrbanId,
                                                      BuildingName = _organizationUnit.GetAll().Where(x => x.Id == u.BuildingId).Select(x => x.DisplayName).FirstOrDefault(),
                                                      UrbanName = _organizationUnit.GetAll().Where(x => x.Id == u.UrbanId).Select(x => x.DisplayName).FirstOrDefault(),
                                                      Electricity = u.Electricity,
                                                      Water = u.Water,
                                                      Contractors = u.Contractors,
                                                      Elevator = u.Elevator,
                                                      Materials = u.Materials,
                                                      AirConditionerRepair = u.AirConditionerRepair,
                                                      Total = u.Electricity + u.Water + u.Contractors + u.Elevator + u.Materials + u.AirConditionerRepair,
                                                      Data = "Năm " + u.CreationTime.ToString("yyyy"),
                                                      ViewQuarterly = input.Quarterly,
                                                      ViewYear = u.CreationTime,
                                                      Type = u.Type,
                                                  })
                                                  .Where(x => input.Year.Year == x.CreationTime.Year)
                                                  .Where(x => x.Type == input.Type)
                                                   .WhereIf(input.BuildingId.HasValue, x => x.BuildingId == input.BuildingId)
                                                   .WhereIf(input.UrbanId.HasValue, x => x.UrbanId == input.UrbanId)
                                                   .ApplySearchFilter(input.Keyword, u => u.BuildingName, u => u.UrbanName)
                                                   .AsQueryable();

                var result = await query.ToListAsync();

                // Group by BuildingId and UrbanId and calculate sum of relevant properties
                var summaryList = result
                    .GroupBy(x => new { x.BuildingId, x.UrbanId, x.BuildingName, x.UrbanName })
                    .Select(group => new
                    {
                        Id = Guid.NewGuid(),
                        CreationTime = group.First().CreationTime,
                        BuildingId = group.Key.BuildingId,
                        UrbanId = group.Key.UrbanId,
                        BuildingName = group.Key.BuildingName,
                        UrbanName = group.Key.UrbanName,
                        Electricity = group.Sum(x => x.Electricity),
                        Water = group.Sum(x => x.Water),
                        Contractors = group.Sum(x => x.Contractors),
                        Elevator = group.Sum(x => x.Elevator),
                        Materials = group.Sum(x => x.Materials),
                        AirConditionerRepair = group.Sum(x => x.AirConditionerRepair),
                        Total = group.Sum(x => x.Total),
                        Data = "Năm " + group.First().CreationTime.ToString("yyyy"),
                        ViewQuarterly = input.Quarterly,
                        ViewYear = group.First().CreationTime,
                    })
                    .ToList();

                var pagedSummaryList = summaryList;

                var data = DataResult.ResultSuccess(pagedSummaryList, "Get success", summaryList.Count);

                return data;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                throw;
            }
        }
        public async Task<object> UpdateBudgetReality(UpdateBudgetDto input)
        {
            try
            {
                long t1 = TimeUtils.GetNanoseconds();

                var budget = await _budgetRepository.FirstOrDefaultAsync(x =>
                    x.Id == input.Id && x.Type == BudgetType.REALITY);

                if (budget == null)
                {
                    throw new UserFriendlyException("Update failed");
                }
                budget.AirConditionerRepair = input.AirConditionerRepair;
                budget.Contractors = input.Contractors;
                budget.Electricity = input.Electricity;
                budget.Elevator = input.Elevator;
                budget.Water = input.Water;
                budget.Materials = input.Materials;
                var data = await _budgetRepository.UpdateAsync(budget);

                mb.statisticMetris(t1, 0, "UpdateBudgetService.UpdateBudgetAsync");
                return DataResult.ResultSuccess(data, "Update success");
            }
            catch (UserFriendlyException e)
            {
                Logger.Fatal(e.Message);
                throw new UserFriendlyException(e.Code, e.Message);
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message);
                throw;
            }
        }
        public async Task<object> ImportBudgetRealityFromExcel([FromForm] ImportBudgetRealityExcelInput input)
        {
            try
            {
                const int COL_URBAN_CODE = 1;
                const int COL_BUILDING_CODE = 2;
                const int COL_ELECTRICITY = 3;
                const int COL_WATER = 4;
                const int COL_CONTRACTORS = 5;
                const int COL_ELEVATOR = 6;
                const int COL_MATERIALS = 7;
                const int COL_AIRCONDITIONERREPAIR = 8;
                IFormFile file = input.File;
                string fileName = file.FileName;
                string fileExt = Path.GetExtension(fileName);
                if (fileExt != ".xlsx" && fileExt != ".xls")
                {
                    return DataResult.ResultError("File not supported", "Error");
                }

                // Generate a unique file path
                string filePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + fileExt);

                using (FileStream stream = File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                    stream.Close();
                }

                var package = new ExcelPackage(new FileInfo(filePath));
                var worksheet = package.Workbook.Worksheets.First();
                int rowCount = worksheet.Dimension.End.Row;
                var listBudgetReality = new List<Budget>();

                for (var row = 2; row <= rowCount; row++)
                {
                    var budgetReality = new Budget();

                    //COL_URBAN_CODE
                    if (worksheet.Cells[row, COL_URBAN_CODE].Value != null)
                    {
                        var ubIDstr = worksheet.Cells[row, COL_URBAN_CODE].Value.ToString().Trim();
                        var ubObj = await _organizationUnit.FirstOrDefaultAsync(x => x.ProjectCode.ToLower() == ubIDstr.ToLower());
                        if (ubObj != null) { budgetReality.UrbanId = ubObj.Id; }
                        else
                        {
                            continue;
                        }
                    }
                    else continue;


                    //COL_BUILDING_CODE
                    if (worksheet.Cells[row, COL_BUILDING_CODE].Value != null)
                    {
                        var buildIDStr = worksheet.Cells[row, COL_BUILDING_CODE].Value.ToString().Trim();
                        var buildObj = await _organizationUnit.FirstOrDefaultAsync(x => x.ProjectCode.ToLower() == buildIDStr.ToLower() && x.ParentId != null);
                        if (buildObj != null)
                        {
                            budgetReality.BuildingId = buildObj.Id;
                        }
                        else continue;
                    }
                    else continue;
                    //COL_ELECTRICITY
                    if (worksheet.Cells[row, COL_ELECTRICITY].Value != null)
                    {
                        budgetReality.Electricity = double.Parse(worksheet.Cells[row, COL_ELECTRICITY].Value.ToString().Trim());
                    }
                    else
                    {
                        budgetReality.Electricity = 0;
                    }

                    //COL_WATER
                    if (worksheet.Cells[row, COL_WATER].Value != null)
                    {
                        budgetReality.Water = double.Parse(worksheet.Cells[row, COL_WATER].Value.ToString().Trim());
                    }
                    else
                    {
                        budgetReality.Water = 0;
                    }
                    //COL_CONTRACTORS
                    if (worksheet.Cells[row, COL_CONTRACTORS].Value != null)
                    {
                        budgetReality.Contractors = double.Parse(worksheet.Cells[row, COL_CONTRACTORS].Value.ToString().Trim());
                    }
                    else
                    {
                        budgetReality.Contractors = 0;
                    }
                    //COL_ELEVATOR
                    if (worksheet.Cells[row, COL_ELEVATOR].Value != null)
                    {
                        budgetReality.Elevator = double.Parse(worksheet.Cells[row, COL_ELEVATOR].Value.ToString().Trim());
                    }
                    else
                    {
                        budgetReality.Elevator = 0;
                    }
                    //COL_MATERIALS
                    if (worksheet.Cells[row, COL_MATERIALS].Value != null)
                    {
                        budgetReality.Materials = double.Parse(worksheet.Cells[row, COL_MATERIALS].Value.ToString().Trim());
                    }
                    else
                    {
                        budgetReality.Materials = 0;
                    }
                    //COL_AIRCONDITIONERREPAIR
                    if (worksheet.Cells[row, COL_AIRCONDITIONERREPAIR].Value != null)
                    {
                        budgetReality.AirConditionerRepair = double.Parse(worksheet.Cells[row, COL_AIRCONDITIONERREPAIR].Value.ToString().Trim());
                    }
                    else
                    {
                        budgetReality.AirConditionerRepair = 0;
                    }

                    budgetReality.TenantId = AbpSession.TenantId;


                    listBudgetReality.Add(budgetReality);

                }

                await CreateListBudgetRealityAsync(listBudgetReality);
                File.Delete(filePath);

                return DataResult.ResultSuccess(listBudgetReality, "Success");
            }

            catch (Exception ex)
            {
                var data = DataResult.ResultError(ex.Message, "Error");
                Logger.Fatal(ex.Message, ex);
                throw;
            }
        }
        private async Task CreateListBudgetRealityAsync(List<Budget> input)
        {
            try
            {
                if (input == null || !input.Any())
                {
                    return;
                }
                foreach (var item in input)
                {
                    var budget = await _budgetRepository.FirstOrDefaultAsync(x =>
                     x.BuildingId == item.BuildingId &&
                     x.UrbanId == item.UrbanId &&
                     x.CreationTime.Year == DateTime.Now.Year &&
                     x.CreationTime.Month == DateTime.Now.Month && x.Type == BudgetType.REALITY);

                    if (budget != null)
                    {
                        continue;
                    }
                    item.Type = BudgetType.REALITY;
                    var data = await _budgetRepository.InsertAsync(item);
                }
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                throw;
            }
        }

    }
}