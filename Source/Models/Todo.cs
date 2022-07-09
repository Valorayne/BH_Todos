﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Todos.Source.Utils;

namespace Todos.Source.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Todo
    {
        public bool IsNew { get; set; }
        
        [JsonProperty] public string Description { get; set; }
        [JsonProperty] public DateTime CreatedAt { get; private set; }
        [JsonProperty] public List<DateTime> Executions { get; private set; }
        [JsonProperty] public TodoSchedule? Schedule { get; set; }

        public Todo() : this(DateTime.Now, new List<DateTime>())
        {
            IsNew = true;
        }
        
        [JsonConstructor]
        private Todo(DateTime createdAt, List<DateTime> executions)
        {
            CreatedAt = createdAt;
            Executions = executions;
            Schedule = new TodoSchedule
            {
                Type = TodoScheduleType.DailyServer
            };
        }

        public DateTime? LastExecution => Executions.Count > 0 ? Executions.Last() : (DateTime?)null;
        
        public bool Done
        {
            get
            {
                if (!Schedule.HasValue)
                    return LastExecution.HasValue;

                switch (Schedule.Value.Type)
                {
                    case TodoScheduleType.DailyServer:
                        return LastExecution.HasValue && LastExecution.Value > DateUtils.LastDailyReset;
                    case TodoScheduleType.WeeklyServer:
                        return LastExecution.HasValue && LastExecution.Value > DateUtils.LastWeeklyReset;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                if (value) Executions.Add(DateTime.Now);
                else Executions.RemoveAt(Executions.Count - 1);
            }
        }
        
        public void Save()
        {
            Data.Save(this);
        }

        public void Delete()
        {
            Data.Delete(this);
        }
    }
}