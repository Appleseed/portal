using System;
using System.Web;

using System.Collections.Specialized;

using System.Configuration;
using Appleseed.Framework;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace SelfUpdater.Code
{
    public class SelfUpdaterManager {
    
        
        private static ISchedulerFactory _sf;
        private static IScheduler _sched;


        public SelfUpdaterManager()
        {
            //instancio el schedule
            if (_sf == null) {
                var properties = new NameValueCollection();

                properties["quartz.scheduler.instanceName"] = "SelfUpdaterScheduler";
                properties["quartz.scheduler.instanceId"] = "SelfUpdaterinstance_one";
                properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
                properties["quartz.threadPool.threadCount"] = "1";
                properties["quartz.threadPool.threadPriority"] = "Normal";

                _sf = new StdSchedulerFactory(properties);
                
            }
        }

        public virtual void Start() {
            _sched = _sf.GetScheduler();
            _sched.Start();
            
        }

        public virtual void AddSelfUpdaterCheckJob()
        {
            try {
                if (ConfigurationManager.AppSettings["SelfUpdaterCronTrigger"] != null)
                {
                    try
                    {
                        var jobDetail = new JobDetailImpl("SelfUpdaterChecker", null, typeof(SelfUpdaterCheckJob));
                        HttpContext.Current.Application.Lock();
                        var executeJob = !bool.Parse(HttpContext.Current.Application["NugetSelfUpdatesToInstall"].ToString());
                        HttpContext.Current.Application.UnLock();
                        jobDetail.JobDataMap.Add("ExcecuteJob", executeJob);
                        var cronExpression = ConfigurationManager.AppSettings["SelfUpdaterCronTrigger"];
                        var trigger = new CronTriggerImpl("TriggerSelfUpdaterChecker", "SelfUpdaterChecker", cronExpression);
                        _sched.ScheduleJob(jobDetail, trigger);


                    }
                    catch (Exception e)
                    {
                        ErrorHandler.Publish(LogLevel.Error, "Failed to add selfUpdater job", e);
                    }
                }
            }
            catch (Exception e) {
                ErrorHandler.Publish(LogLevel.Error, "Failed to add selfUpdater job", e);                
            }
            
        }

 

        //public virtual void PauseEmailSenderTrigger() {
        //    _sched.PauseTrigger("TriggerBirthDayChecker", "BirthdayChecker");
        //}

        //public virtual void ResumeEmailSenderTrigger() {
        //    _sched.ResumeTrigger("TriggerBirthDayChecker", "BirthdayChecker");            
        //}

        //public virtual bool isTriggerEmailSenderPaused() {
        //    return _sched.GetTriggerState("TriggerBirthDayChecker", "BirthdayChecker").Equals(TriggerState.Paused);
        //}

        //public virtual bool ChequearEstadoSchedule() { 
        //    return _sched.IsStarted;
        //}
    }
}



    
