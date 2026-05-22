using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp.Application.Domains;

namespace WebApp.Tests.Application.Domains;

[TestClass]
public class DepartmentTests
{

    [TestMethod("部署名が21文字以上のときは例外になる")]
    public void Constructor_WithTooLongName_ThrowsExceptionWithExpectedMessage()
    {
        Exception exception;
        try
        {
            Department _ = new(id: 1, name: new string(c: 'a', count: 21));
            Assert.Fail();
            return;
        }
        catch (Exception caught)
        {
            exception = caught;
        }

        Assert.AreEqual("部署名は20文字以内で入力してください", exception.Message);
    }

    [TestMethod("部署名を空白で変更しようとすると例外になる")]
    public void ChangeName_WithWhitespace_ThrowsExceptionWithExpectedMessage()
    {
        Department department = new(id: 1, name: "総務部");

        Exception exception;
        try
        {
            department.ChangeName(" ");
            Assert.Fail();
            return;
        }
        catch (Exception caught)
        {
            exception = caught;
        }

        Assert.AreEqual("部署名は必須です", exception.Message);
    }
}
