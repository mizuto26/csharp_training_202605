using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp.Application.Domains;

namespace MSTest_WebApp.Application.Domains;

[TestClass]
public class EmployeeTests
{
    [TestMethod]
    public void Constructor_WithBlankName_ThrowsExceptionWithExpectedMessage()
    {
        Exception exception;
        try
        {
            Employee _ = new(id: 1, name: " ", department: null);
            Assert.Fail();
            return;
        }
        catch (Exception caught)
        {
            exception = caught;
        }

        Assert.AreEqual("氏名は必須です", exception.Message);
    }

    [TestMethod]
    public void ChangeName_WithTooLongName_ThrowsExceptionWithExpectedMessage()
    {
        Employee employee = new(id: 1, name: "山田太郎", department: null);

        Exception exception;
        try
        {
            employee.ChangeName(new string('a', 21));
            Assert.Fail();
            return;
        }
        catch (Exception caught)
        {
            exception = caught;
        }

        Assert.AreEqual("氏名は20文字以内で入力してください", exception.Message);
    }
}
