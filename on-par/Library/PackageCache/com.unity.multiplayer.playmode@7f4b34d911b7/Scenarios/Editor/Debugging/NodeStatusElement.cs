using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Multiplayer.PlayMode.Scenarios.Editor.GraphsFoundation;
using UnityEditor;
using UnityEngine.UIElements;

namespace Unity.Multiplayer.PlayMode.Scenarios.Debugging.Editor
{
    internal class NodeStatusElement : VisualElement
    {
        private Node m_Node;
        private Label m_StateLabel;
        private Label m_MessageLabel;
        private Label m_InputsLabel;
        private Label m_OutputsLabel;

        public NodeStatusElement(Node node)
        {
            m_Node = node;
            BuildUI();

            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            EditorApplication.update += Refresh;
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            EditorApplication.update -= Refresh;
        }

        private void BuildUI()
        {
            AddToClassList("node-status");

            var nodeNameLabel = new Label($"{m_Node.Name}") { name = "node-name" };
            m_StateLabel = new Label() { name = "node-state" };
            m_MessageLabel = new Label() { name = "node-message" };
            m_InputsLabel = new Label() { name = "node-inputs" };
            m_OutputsLabel = new Label() { name = "node-outputs" };

            Add(nodeNameLabel);
            Add(m_StateLabel);
            Add(m_MessageLabel);
            Add(m_InputsLabel);
            Add(m_OutputsLabel);
        }

        private void Refresh()
        {
            m_StateLabel.text = $"[{m_Node.State} - {m_Node.Progress * 100}% - {GetExecutionTimeText(m_Node.TimeData)}]";
            m_MessageLabel.text = m_Node.ErrorMessage;

            var inputs = GetFieldsOfType<NodeInput>(m_Node);
            var outputs = GetFieldsOfType<NodeOutput>(m_Node);

            m_InputsLabel.text = $"Inputs:\n{DumpNodeParameters(m_Node, inputs)}";
            m_OutputsLabel.text = $"Outputs:\n{DumpNodeParameters(m_Node, outputs)}";
        }

        private static string GetExecutionTimeText(NodeTimeData timeData)
        {
            if (!timeData.HasStarted)
                return "<Not started>";

            var endTime = timeData.HasEnded ? timeData.EndTime : DateTime.Now;
            var duration = endTime - timeData.StartTime;
            return $"{duration.TotalSeconds:0.00}s";
        }

        private static List<FieldInfo> GetFieldsOfType<T>(object obj)
        {
            var fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            var fieldsOfType = new List<FieldInfo>();
            foreach (var field in fields)
            {
                if (typeof(T).IsAssignableFrom(field.FieldType))
                    fieldsOfType.Add(field);
            }

            return fieldsOfType;
        }


        private static string DumpNodeParameters(Node node, List<FieldInfo> parameterFields)
        {
            if (parameterFields.Count == 0)
                return "\t<None>";

            var result = "";

            foreach (var field in parameterFields)
            {
                var name = field.Name;
                var parameter = (NodeParameter)field.GetValue(node);
                var value = parameter?.GetValue<object>();
                result += $"\t- {name}: {(value != null ? value : "<null>")}\n";
            }

            return result.TrimEnd('\n');
        }
    }
}
