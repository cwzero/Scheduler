using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scheduler;
using System.Net.Http;
using System.Threading.Tasks;

namespace SchedulerTests
{
    [TestClass]
    public class ServiceTests
    {
        private SchedulerService scheduler = new SchedulerService();

        public ServiceTests()
        {

        }

        [TestMethod]
        public async Task TestServices()
        {
            await TestUserService();
            await TestEventService();
            await TestGroupService();
        }

        public async Task TestEventService()
        {
            Service<Event> service = scheduler.eventService;
            Event e = new Event();

            e.date = new DateTime(2017, 1, 1, 0, 0, 0);
            e.desc = "Test Event";
            e.name = "Test Event";
            e.reoccurType = Event.ReoccurType.DEFAULT;

            var response = await service.createItem(e);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
        
        public async Task TestGroupService()
        {
            Service<Group> service = scheduler.groupService;

            Group g = new Group();
            g.desc = "Test Group";
            g.name = "Test Group";
            g.owner = "testuser";
            g.users.Add("testuser");
            g.events.Add("Test Event");

            var response = await service.createItem(g);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
        
        public async Task TestUserService()
        {
            Service<User> service = scheduler.userService;

            User u = new User();
            u.name = "testuser";
            u.password = "testpassword";

            var response = await service.createItem(u);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
