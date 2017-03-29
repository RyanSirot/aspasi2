using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZenithSociety.Models.Business;

namespace ZenithSociety.Data
{
    public class BusinessDummyData
    {
        public static void Initialize(ApplicationDbContext db)
        {
            // seed activity table
            if (!db.Activities.Any())
            {

                db.Activities.Add(new Activity()
                {
                    Description = "Playing Golf",
                    CreationDate = new DateTime(2017, 2, 1, 7, 0, 0)
                });

                db.Activities.Add(new Activity()
                {
                    Description = "Swimming",
                    CreationDate = new DateTime(2017, 1, 15, 8, 30, 0)
                });

                db.Activities.Add(new Activity()
                {
                    Description = "Leadership General Assembly meeting",
                    CreationDate = new DateTime(2017, 1, 25, 8, 30, 0)
                });

                db.Activities.Add(new Activity()
                {
                    Description = "Youth Bowling Tournament",
                    CreationDate = new DateTime(2017, 1, 25, 8, 30, 0)
                });

                db.Activities.Add(new Activity()
                {
                    Description = "Young ladies cooking lessons",
                    CreationDate = new DateTime(2017, 1, 25, 8, 30, 0)
                });

                db.Activities.Add(new Activity()
                {
                    Description = "BBQ Lunch",
                    CreationDate = new DateTime(2017, 1, 25, 8, 30, 0)
                });

                db.SaveChanges();

            }

            // seed events table
            if(!db.Events.Any())
            {
                db.Events.Add(new Event()
                {
                    EventFrom = new DateTime(2017, 3, 1, 14, 30, 0),
                    EventTo = new DateTime(2017, 3, 1, 16, 20, 0),
                    UserName = "a",
                    CreationDate = new DateTime(2017, 1, 24, 10, 30, 0),
                    IsActive = true,
                    Activity = db.Activities.Where(a => a.ActivityId == 4).FirstOrDefault()
                });

                db.Events.Add(new Event()
                {
                    EventFrom = new DateTime(2017, 3, 2, 14, 30, 0),
                    EventTo = new DateTime(2017, 3, 2, 16, 20, 0),
                    UserName = "a",
                    CreationDate = new DateTime(2017, 1, 24, 10, 30, 0),
                    IsActive = true,
                    Activity = db.Activities.Where(a => a.ActivityId == 6).FirstOrDefault()
                });

                db.Events.Add(new Event()
                {
                    EventFrom = new DateTime(2017, 1, 25, 8, 30, 0),
                    EventTo = new DateTime(2017, 1, 25, 10, 30, 0),
                    UserName = "a",
                    CreationDate = new DateTime(2017, 1, 24, 10, 30, 0),
                    IsActive = true,
                    Activity = db.Activities.Where(a => a.ActivityId == 1).FirstOrDefault()
                });

                db.Events.Add(new Event()
                {
                    EventFrom = new DateTime(2017, 3, 6, 8, 30, 0),
                    EventTo = new DateTime(2017, 3, 6, 10, 30, 0),
                    UserName = "a",
                    CreationDate = new DateTime(2017, 1, 24, 10, 30, 0),
                    IsActive = true,
                    Activity = db.Activities.Where(a => a.ActivityId == 1).FirstOrDefault()
                });

                db.Events.Add(new Event()
                {
                    EventFrom = new DateTime(2017, 3, 7, 8, 30, 0),
                    EventTo = new DateTime(2017, 3, 7, 10, 30, 0),
                    UserName = "a",
                    CreationDate = new DateTime(2017, 1, 24, 10, 30, 0),
                    IsActive = true,
                    Activity = db.Activities.Where(a => a.ActivityId == 2).FirstOrDefault()
                });

                db.Events.Add(new Event()
                {
                    EventFrom = new DateTime(2017, 3, 8, 8, 30, 0),
                    EventTo = new DateTime(2017, 3, 8, 10, 30, 0),
                    UserName = "a",
                    CreationDate = new DateTime(2017, 1, 24, 10, 30, 0),
                    IsActive = true,
                    Activity = db.Activities.Where(a => a.ActivityId == 2).FirstOrDefault()
                });

                db.Events.Add(new Event()
                {
                    EventFrom = new DateTime(2017, 3, 8, 8, 30, 0),
                    EventTo = new DateTime(2017, 3, 8, 10, 30, 0),
                    UserName = "a",
                    CreationDate = new DateTime(2017, 1, 24, 10, 30, 0),
                    IsActive = true,
                    Activity = db.Activities.Where(a => a.ActivityId == 3).FirstOrDefault()
                });

                db.Events.Add(new Event()
                {
                    EventFrom = new DateTime(2017, 3, 9, 14, 30, 0),
                    EventTo = new DateTime(2017, 3, 9, 16, 20, 0),
                    UserName = "a",
                    CreationDate = new DateTime(2017, 1, 24, 10, 30, 0),
                    IsActive = true,
                    Activity = db.Activities.Where(a => a.ActivityId == 4).FirstOrDefault()
                });

                db.Events.Add(new Event()
                {
                    EventFrom = new DateTime(2017, 3, 10, 14, 30, 0),
                    EventTo = new DateTime(2017, 3, 10, 16, 20, 0),
                    UserName = "a",
                    CreationDate = new DateTime(2017, 1, 24, 10, 30, 0),
                    IsActive = true,
                    Activity = db.Activities.Where(a => a.ActivityId == 5).FirstOrDefault()
                });

                db.Events.Add(new Event()
                {
                    EventFrom = new DateTime(2017, 3, 11, 14, 30, 0),
                    EventTo = new DateTime(2017, 3, 11, 16, 20, 0),
                    UserName = "a",
                    CreationDate = new DateTime(2017, 1, 24, 10, 30, 0),
                    IsActive = true,
                    Activity = db.Activities.Where(a => a.ActivityId == 5).FirstOrDefault()
                });

                db.Events.Add(new Event()
                {
                    EventFrom = new DateTime(2017, 3, 13, 14, 30, 0),
                    EventTo = new DateTime(2017, 3, 13, 16, 20, 0),
                    UserName = "a",
                    CreationDate = new DateTime(2017, 1, 24, 10, 30, 0),
                    IsActive = true,
                    Activity = db.Activities.Where(a => a.ActivityId == 3).FirstOrDefault()
                });

                db.Events.Add(new Event()
                {
                    EventFrom = new DateTime(2017, 3, 14, 14, 30, 0),
                    EventTo = new DateTime(2017, 3, 14, 16, 20, 0),
                    UserName = "a",
                    CreationDate = new DateTime(2017, 1, 24, 10, 30, 0),
                    IsActive = true,
                    Activity = db.Activities.Where(a => a.ActivityId == 5).FirstOrDefault()
                });

                db.Events.Add(new Event()
                {
                    EventFrom = new DateTime(2017, 3, 17, 14, 30, 0),
                    EventTo = new DateTime(2017, 3, 17, 16, 20, 0),
                    UserName = "a",
                    CreationDate = new DateTime(2017, 1, 24, 10, 30, 0),
                    IsActive = true,
                    Activity = db.Activities.Where(a => a.ActivityId == 5).FirstOrDefault()
                });

                db.Events.Add(new Event()
                {
                    EventFrom = new DateTime(2017, 3, 15, 14, 30, 0),
                    EventTo = new DateTime(2017, 3, 15, 16, 20, 0),
                    UserName = "a",
                    CreationDate = new DateTime(2017, 1, 24, 10, 30, 0),
                    IsActive = true,
                    Activity = db.Activities.Where(a => a.ActivityId == 3).FirstOrDefault()
                });

                db.SaveChanges();

            }
        }
    }
}
