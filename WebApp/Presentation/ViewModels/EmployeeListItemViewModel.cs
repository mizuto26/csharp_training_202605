namespace WebApp.Presentation.ViewModels;

public class EmployeeListItemViewModel
{
    public int? EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int? DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"EmployeeId={EmployeeId} , EmployeeName={EmployeeName} , Email={Email} , Phone={Phone} , DepartmentId={DepartmentId} , DepartmentName={DepartmentName}";
    }
}
