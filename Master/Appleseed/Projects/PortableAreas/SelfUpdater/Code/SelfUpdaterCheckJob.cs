using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using Appleseed.Framework;
using Quartz;
using Quartz.Impl;
using SelfUpdater.Models;

namespace SelfUpdater.Code
{
    public class SelfUpdaterCheckJob : IJob
    {
        public void Execute(IJobExecutionContext context_)
        {
            var context = (JobExecutionContextImpl)context_;

            var executeJob = bool.Parse(context.JobDetail.JobDataMap["ExcecuteJob"].ToString());


            if(executeJob)
            {
                var projectManagers = ProjectManagerHelper.GetProjectManagers();
                var updateList = new List<InstallationState>();
                foreach (var projectManager in projectManagers)
                {
                    var availablePackages = ProjectManagerHelper.GetAvailablePackagesLatestList(projectManager);

                    var installedPackages = ProjectManagerHelper.GetInstalledPackagesLatestList(projectManager, true);


                    foreach (var installedPackage in installedPackages)
                    {
                        var update = projectManager.GetUpdatedPackage(availablePackages, installedPackage);
                        if (update != null)
                        {
                            var package = new InstallationState();
                            package.Installed = installedPackage;
                            package.Update = update;
                            package.Source = projectManager.SourceRepository.Source;

                            if (updateList.Any(d => d.Installed.Id == package.Installed.Id))
                            {
                                var addedPackage = updateList.First(d => d.Installed.Id == package.Installed.Id);
                                if (package.Update != null)
                                {
                                    if (addedPackage.Update == null || addedPackage.Update.Version < package.Update.Version)
                                    {
                                        updateList.Remove(addedPackage);
                                        updateList.Add(package);
                                    }
                                }
                            }
                            else
                            {
                                updateList.Add(package);
                            }
                        }
                    }
                }

                // UpdateList is a list with packages that has updates
                if (updateList.Any())
                {
                    var sb = new StringBuilder();
                    try
                    {
                        var entities = new SelfUpdaterEntities();

                        foreach (
                            var self in
                                updateList.Select(
                                    pack =>
                                    new SelfUpdatingPackages
                                    {
                                        PackageId = pack.Installed.Id,
                                        PackageVersion = pack.Update.Version.ToString(),
                                        Source = pack.Source,
                                        Install = false
                                    }))
                        {
                            if (!entities.SelfUpdatingPackages.Any(x => x.PackageId == self.PackageId && x.PackageVersion == self.PackageVersion))
                            {
                                entities.AddToSelfUpdatingPackages(self);
                                sb.AppendFormat("Adding package {0} version {1} to update.", self.PackageId,
                                                self.PackageVersion);
                                sb.AppendLine();
                            }
                        }

                        entities.SaveChanges();

                        var config = WebConfigurationManager.OpenWebConfiguration("~/");
                        var section = config.GetSection("system.web/httpRuntime");
                        ((HttpRuntimeSection)section).WaitChangeNotification = 123456789;
                        ((HttpRuntimeSection)section).MaxWaitChangeNotification = 123456789;
                        config.Save();


                    }
                    catch (Exception e)
                    {
                        ErrorHandler.Publish(LogLevel.Error, e);
                    }

                    try
                    {
                        var body = sb.ToString();
                        var emailDir = ConfigurationManager.AppSettings["SelfUpdaterMailconfig"];
                        if (!string.IsNullOrEmpty(body) && !string.IsNullOrEmpty(emailDir))
                        {
                            var mail = new MailMessage();
                            mail.From = new MailAddress("info@appleseedportal.net");
                            
                            mail.To.Add(new MailAddress(emailDir));
                            mail.Subject = string.Format("Updates Manager chekcker job");

                            mail.Body = body;
                            mail.IsBodyHtml = false;

                            using (var client = new SmtpClient())
                            {
                                client.Send(mail);

                            }
                        }
                    }
                    catch (Exception e)
                    {
                        ErrorHandler.Publish(LogLevel.Error, e);
                    }

                }
            }



            

           
        }

       
    }
}