using System;
using NUnit.Framework;
using Unmanaged;

namespace PowerManagementTest
{
    [TestFixture]
    public class PowerManagerTests
    {
        private static PowerManager _powerManager;

        [SetUp]
        public void Initialize()
        {
            _powerManager = new PowerManager();
        }

        [Test]
        public void LastSleepTimeTest()
        {
            Console.WriteLine("Last sleep time: {0}", _powerManager.GetLastSleepTime());
        }

        [Test]
        public void LastWakeTimeTest()
        {
            Console.WriteLine("Last wake time: {0}", _powerManager.GetLastWakeTime());
        }

        [Test]
        public void SystemBatteryStateTest()
        {
            var state = _powerManager.GetSystemBatteryState();

            Console.WriteLine("Battery Present: {0}", state.BatteryPresent);
            Console.WriteLine("Remaining Capacity: {0}", state.RemainingCapacity);
        }

        [Test]
        public void SystemPowerInformationTest()
        {
            var info = _powerManager.GetSystemPowerInformation();

            var time = new TimeSpan(0, 0, (int)info.TimeRemaining);

            Console.WriteLine($"Remaning time: {time.Days} day(s), {time.Minutes} minute(s), {time.Seconds} second(s)");
            //0 - active, 1 - passive, 2- invalid
            Console.WriteLine("Current system cooling mode: {0}", info.CoolingMode);
        }

        [Test]
        public void SuspendSystemTest()
        {
            Assert.Fail("Comment fail assert to enter sleep mode");
            _powerManager.SuspendSystem();
        }

        [Test]
        public void HibernationFileTest()
        {
            _powerManager.ManageHibernationFile(true);
            var dummyToBreakPoint = 0;
            _powerManager.ManageHibernationFile(false);
        }
    }
}
