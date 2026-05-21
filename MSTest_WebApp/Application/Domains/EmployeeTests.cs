using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp.Application.Domains;

namespace MSTest_WebApp.Application.Domains;

[TestClass]
public class EmployeeTests
{
    [TestMethod("氏名が空白の場合")]
    public void Constructor_WithBlankName_ThrowsException()
    {
        Exception exception;
        try
        {
            Employee _ = new(id: 1, name: " ", phone: "", mail: "", department: null);
            return;
        }
        catch (Exception caught)
        {
            exception = caught;
        }

        Assert.AreEqual("氏名は必須です", exception.Message);
    }

    [TestMethod("氏名が20文字を超えている場合")]
    public void ChangeName_WithTooLongName_ThrowsException()
    {
        Employee employee = new(id: 1, name: "山田太郎", phone: null, mail: null, department: null);

        Exception exception;
        try
        {
            employee.ChangeName(new string(c: 'c', count: 21));
            return;
        }
        catch (Exception caught)
        {
            exception = caught;
        }

        Assert.AreEqual("氏名は20文字以内で入力してください", exception.Message);
    }
}
