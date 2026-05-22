using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp.Application.Domains;

namespace MSTest_WebApp.Application.Domains;

[TestClass]
public class EmployeeTests
{
    [TestMethod("氏名が空白のときは例外になる")]
    public void Constructor_WithBlankName_ThrowsExceptionWithExpectedMessage()
    {
        Exception exception;
        try
        {
            Employee _ = new(
                id: 1,
                name: " ",
                email: "yamada@example.com",
                phone: "03-1234-5678",
                department: null
            );

            Assert.Fail();
            return;
        }
        catch (Exception caught)
        {
            exception = caught;
        }

        Assert.AreEqual("氏名は必須です", exception.Message);
    }

    [TestMethod("氏名を21文字以上に変更しようとすると例外になる")]
    public void ChangeName_WithTooLongName_ThrowsExceptionWithExpectedMessage()
    {
        Employee employee = new(
            id: 1,
            name: "山田太郎",
            email: "yamada@example.com",
            phone: "03-1234-5678",
            department: null
        );

        Exception exception;
        try
        {
            employee.ChangeName(new string(c: 'c', count: 21));
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
