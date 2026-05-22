namespace WebApp.Presentation.ViewModels;

public class EmployeeListViewModel
{
    public IReadOnlyList<EmployeeListItemViewModel> Employees { get; set; } = [];

    public override string ToString()
    {
        return $"Employees={Employees}";
    }
}