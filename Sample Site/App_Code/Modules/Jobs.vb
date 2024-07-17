Imports System
Imports System.Linq
Imports Database
Imports Microsoft.VisualBasic
Imports Quartz
Imports Quartz.Impl

Public Module Jobs

    'Private Scheduler As IScheduler

    'Public Sub InitiateJobs()
    '	Dim sf As ISchedulerFactory = New StdSchedulerFactory()
    '	Dim sched As IScheduler = sf.GetScheduler()

    '	Dim job As IJobDetail = JobBuilder.Create(Of PruneSessions)().WithIdentity("PruneSessions").Build()
    '	Dim trigger As ITrigger = TriggerBuilder.Create().WithIdentity("3AM").StartAt(DateTime.Today).WithCronSchedule("0 15 * * * ?").Build()

    '	Scheduler.ScheduleJob(job, trigger)
    '	Scheduler.Start()
    'End Sub

    'Private Class PruneSessions
    '	Implements IJob

    '	Public Sub Execute(context As IJobExecutionContext) Implements IJob.Execute
    '		Using DB As New stahnkeEntities2
    '			Dim InvalidSessions As IQueryable(Of session) = DB.sessions.Where(Function(s) s.isValid())
    '		End Using
    '	End Sub

    'End Class

End Module
