using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Android.Enums;
using OpenQA.Selenium.Appium.MultiTouch;

namespace Lab7;

[TestClass]
public class UnitTest1
{
    private AndroidDriver<AppiumWebElement> _driver;

    [TestInitialize]
    public void TestInit()
    {
        var options = new AppiumOptions();
        options.AddAdditionalCapability("platformName", "android");
        options.AddAdditionalCapability("appium:deviceName", "emulator-5554");
        options.AddAdditionalCapability("appium:appPackage", "com.bakan.universchedule");
        options.AddAdditionalCapability("appium:appActivity", ".activity.MainActivity");
        options.AddAdditionalCapability("appium:noReset", true);

        _driver = new AndroidDriver<AppiumWebElement>(new Uri("http://localhost:4723/wd/hub"), options);

        _driver.Settings = new Dictionary<string, object>()
        {
            [AutomatorSetting.WaitForIDLETimeout] = 1,
            [AutomatorSetting.WaitForSelectorTimeout] = 1
        };


        //_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
    }

    [TestMethod]
    public void AddGroup()
    {
        const string groupBtnXPath =
            "/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/androidx.drawerlayout.widget.DrawerLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.ListView/android.widget.LinearLayout";
        const string groupScheduleXPath =
            "/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/androidx.viewpager.widget.ViewPager/androidx.recyclerview.widget.RecyclerView/android.widget.FrameLayout/android.widget.LinearLayout/android.view.ViewGroup/android.widget.ListView/android.widget.LinearLayout";

        var element = _driver.FindElementByAccessibilityId("BSUIR Schedule");
        element.Click();

        var startGroupsCount = _driver.FindElementsByXPath(groupBtnXPath).Count;

        var addGroupBtn = _driver.FindElementById("com.bakan.universchedule:id/addScheduleButton");
        addGroupBtn.Click();

        var groupNameEdit = _driver.FindElementById("com.bakan.universchedule:id/searchEdit");
        groupNameEdit.SendKeys("051004");

        var groups = _driver.FindElementsByXPath(groupScheduleXPath);
        Assert.AreEqual(1, groups.Count);

        var group = groups.First();
        group.Click();

        var endGroupsCount = _driver.FindElementsByXPath(groupBtnXPath).Count;
        Assert.AreEqual(startGroupsCount + 1, endGroupsCount);

        group = _driver.FindElementsByXPath(groupBtnXPath)
            .Select(e => e.FindElementByClassName("android.widget.TextView"))
            .First(t => t.Text == "Group: 051004 Exams"); ;

        var touchAction = new TouchAction(_driver);
        touchAction.LongPress(group).Release().Perform();

        var deleteBtn = _driver.FindElementsByClassName("android.widget.TextView")
            .First(e => e.Text == "Delete");
        deleteBtn.Click();

        var deleteBtn1 = _driver.FindElementById("android:id/button1");
        deleteBtn1.Click();

        endGroupsCount = _driver.FindElementsByXPath(groupBtnXPath).Count;
        Assert.AreEqual(startGroupsCount, endGroupsCount);
    }

    [TestMethod]
    public void CheckDate()
    {
        var todayHeader = _driver.FindElementByXPath(
            "/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/androidx.drawerlayout.widget.DrawerLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout/androidx.viewpager.widget.ViewPager/android.view.ViewGroup/android.widget.TextView[1]");

        var todayText = todayHeader.Text;
        var date = todayText.Split(' ')[1];

        Assert.AreEqual(DateTime.Now.ToString("dd.MM"), date);
    }

    [TestMethod]
    public void CheckNextDate()
    {
        var todayHeader = _driver.FindElementByXPath(
            "/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/androidx.drawerlayout.widget.DrawerLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout/androidx.viewpager.widget.ViewPager/android.view.ViewGroup/android.widget.TextView[2]");

        var todayText = todayHeader.Text;
        var date = todayText.Split(' ')[1];

        Assert.AreEqual(DateTime.Now.AddDays(1).ToString("dd.MM"), date);
    }

    [TestMethod]
    public void CheckCompleteSchedule()
    {
        _driver.FindElementByAccessibilityId("Complete schedule").Click();
        var daysContainer = _driver.FindElementById("com.bakan.universchedule:id/pager_title_strip");

        var monday = daysContainer.FindElementByClassName("android.widget.TextView");

        Assert.AreEqual("MONDAY", monday.Text);
    }

    [TestMethod]
    public void CheckSubgroupOptionsCount()
    {
        _driver.FindElementByAccessibilityId("Subgroup").Click();
        var options =
            _driver.FindElementsByXPath(
                "/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.ListView/android.widget.LinearLayout");

        Assert.AreEqual(3, options.Count);
    }

    [TestMethod]
    public void OpenExamsPage()
    {
        _driver.FindElementById("com.bakan.universchedule:id/menu_current_schedule_item_exams").Click();
        var titleContainer = _driver.FindElementById("com.bakan.universchedule:id/toolbar");
        var texts = titleContainer.FindElementsByClassName("android.widget.TextView");

        Assert.AreEqual("Exams", texts[0].Text);
        Assert.AreEqual("Group: 051003", texts[1].Text);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        _driver.Quit();
    }
}