using WebApplication13.Controllers;
using Xunit;

namespace Test
{
    public class Class1
    {
        [Fact]
        public void PassingTest()
        {
            HomeController controller = new HomeController();
            controller.Index();
        }
    }
}