using System;
using UnityEngine;

namespace Unity.Multiplayer.PlayMode.Scenarios.Editor.GraphsFoundation
{
    [Serializable]
    internal struct NodeTimeData
    {
        [SerializeField] private long m_StartTimeTicks;
        [SerializeField] private long m_EndTimeTicks;
        [SerializeField] private long m_MonitoringEndTimeTicks;

        public DateTime StartTime
        {
            get => new(m_StartTimeTicks);
            set => m_StartTimeTicks = value.Ticks;
        }

        public DateTime EndTime
        {
            get => new(m_EndTimeTicks);
            set => m_EndTimeTicks = value.Ticks;
        }

        public DateTime MonitoringEndTime
        {
            get => new(m_MonitoringEndTimeTicks);
            set => m_MonitoringEndTimeTicks = value.Ticks;
        }

        public bool HasStarted => m_StartTimeTicks != default;
        public bool HasEnded => m_EndTimeTicks != default;
        public bool HasMonitoringEnded => m_MonitoringEndTimeTicks != default;
    }
}
